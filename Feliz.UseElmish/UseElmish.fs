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

    type ElmishState<'Arg, 'Model, 'Msg> =
        {
            state: ('Model * ('Msg -> unit)) ref
            // I'm using a record here to make sure Fable always return the same reference for `subscribe`
            // This seems to be important to prevent React.useSyncExternalStore from running `subscribe` multiple times
            subscribe: UseSyncExternalStoreSubscribe
        }
        static member Create(program, arg: 'Arg) =
            let mutable cmd = Cmd.none
            let mutable disposed = false

            // React will run the subscribe function when the component is mounted and after each hot reload
            // However we need to store the initial model here for two reasons:
            // - It looks like useSyncExternalStore returns the state before running subscribe
            // - We can restore the model after a hot reload
            let program = program()
            let model, cmd' = Program.init program arg
            let state = ref(model, fun (_: 'Msg) -> ())
            cmd <- cmd'

            let mapInit _init _arg =
                let cmd' = cmd
                // Don't run the original commands after hot reload
                cmd <- Cmd.none
                state.Value |> fst, cmd'

            let mapTerminate (predicate, terminate) =
                (fun msg -> predicate msg || disposed),
                (fun model ->
                    // Before Elmish 4 it was allowed to have disposable states as a hack for termination
                    match box model with
                    | :? System.IDisposable as disp -> disp.Dispose()
                    | _ -> terminate model)

            {
                state = state
                subscribe = UseSyncExternalStoreSubscribe(fun callback ->
                    // If this is running after a hot reload, make sure `disposed` is set back to false
                    disposed <- false

                    // Restart the program after each hot reload to get the proper dispatch reference
                    program
                    |> Program.map mapInit id id id id mapTerminate
                    |> Program.withSetState (fun model dispatch ->
                        let oldModel, _ = state.Value
                        state.Value <- model, dispatch
                        // Skip re-renders if model hasn't changed
                        if not(obj.ReferenceEquals(model, oldModel)) then
                            callback())
                    |> Program.runWith arg

                    (fun () -> disposed <- true))
            }

open Util

type React with
    [<Hook>]
    static member useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg): 'Model * ('Msg -> unit) =
        // Don't use useMemo here because React doesn't guarantee it won't recreate it again
        let state, _ = useState(fun () -> ElmishState<'Arg, 'Model, 'Msg>.Create(program, arg))
        let state = useSyncExternalStore(state.subscribe, fun () -> state.state.Value)
        state

    static member inline useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>) =
        React.useElmish(program, ())

    static member inline useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg)

    static member inline useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())))

    static member inline useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>) =
        React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())))
