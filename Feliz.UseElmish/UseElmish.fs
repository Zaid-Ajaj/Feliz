module Feliz.UseElmish

open Fable.Core
open Feliz
open Elmish

module private Util =
    type UseSyncExternalStoreSubscribe = delegate of (unit -> unit) -> (unit -> unit)

    [<ImportMember("react")>]
    let useSyncExternalStore(subscribe: UseSyncExternalStoreSubscribe, getSnapshot: unit -> 'Model): 'Model = jsNative

    [<ImportMember("react")>]
    let useState(init: unit -> 'State): 'State * (unit -> 'State) = jsNative

    type ElmishState<'Arg, 'Model, 'Msg when 'Arg : equality> =
        {
            state: ('Model * ('Msg -> unit)) ref
            init: 'Arg -> 'Model * Cmd<'Msg>
            argDeps: ('Arg * obj[]) ref
            // I'm using a record here to make sure Fable always return the same reference for `subscribe`
            // This seems to be important to prevent React.useSyncExternalStore from running `subscribe` multiple times
            subscribe: UseSyncExternalStoreSubscribe
        }
        member this.UpdateDeps(arg, dependencies: obj[]) =
            let oldArg, oldDeps = this.argDeps.Value
            if arg <> oldArg || dependencies <> oldDeps then
                let _oldModel, dispatch = this.state.Value
                let newModel, cmd = this.init arg
                cmd |> List.iter (fun call -> call dispatch)
                this.argDeps.Value <- arg, dependencies
                this.state.Value <- newModel, dispatch

        static member Create(program, arg: 'Arg, dependencies: obj[]) =
            // There is a lag between setting `toBeDisposed` to true and Elmish running Program.termination
            // This is because Elmish only checks for termination when a new message comes
            // Because of this
            let mutable toBeDisposed = false
            let mutable hasInitialised = false

            // React will run the subscribe function when the component is mounted and after each hot reload
            // However we need to store the initial model here for two reasons:
            // - It looks like useSyncExternalStore returns the state before running subscribe
            // - We can restore the model after a hot reload
            let program = program()
            let model, cmd' = Program.init program arg
            let state = ref(model, fun (_: 'Msg) -> ())
            let mutable cmd = cmd'

            let mapInit _init _arg =
                let cmd' = cmd
                // Don't run the original commands after hot reload
                cmd <- Cmd.none
                state.Value |> fst, cmd'

            let mapUpdate update msg _model =
                let model, _ = state.Value
                update msg model

            let mapSubscribe subscribe model =
                if not hasInitialised then
                    hasInitialised <- true
                    subscribe model
                else
                    Sub.none

            let mapTermination (predicate, terminate) =
                (fun msg -> predicate msg || toBeDisposed),
                (fun model ->
                    hasInitialised <- false
                    // Before Elmish 4 it was allowed to have disposable states as a hack for termination
                    match box model with
                    | :? System.IDisposable as disp -> disp.Dispose()
                    | _ -> terminate model)

            {
                state = state
                init = Program.init program
                argDeps = ref(arg, dependencies)
                subscribe = UseSyncExternalStoreSubscribe(fun callback ->
                    // If this is running after a hot reload, make sure `toBeDisposed` is set back to false
                    toBeDisposed <- false

                    // Restart the program after each hot reload to get the proper dispatch reference
                    program
                    |> Program.map mapInit mapUpdate id id mapSubscribe mapTermination
                    |> Program.withSetState (fun model dispatch ->
                        let oldModel, _ = state.Value
                        state.Value <- model, dispatch
                        // Skip re-renders if model hasn't changed
                        if not(obj.ReferenceEquals(model, oldModel)) then
                            callback())
                    |> Program.runWith arg

                    (fun () -> toBeDisposed <- true))
            }

open Util

type React with
    [<Hook>]
    static member useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, ?dependencies: obj array): 'Model * ('Msg -> unit) =
        let dependencies = defaultArg dependencies [||]
        // Don't use useMemo here because React doesn't guarantee it won't recreate it again
        let state, _ = useState(fun () -> ElmishState<'Arg, 'Model, 'Msg>.Create(program, arg, dependencies))
        state.UpdateDeps(arg, dependencies)
        useSyncExternalStore(state.subscribe, fun () -> state.state.Value)

    static member inline useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>, ?dependencies: obj array) =
        React.useElmish(program, (), ?dependencies=dependencies)

    static member inline useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg, ?dependencies=dependencies)

    static member inline useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), ?dependencies=dependencies)

    static member inline useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())), ?dependencies=dependencies)
