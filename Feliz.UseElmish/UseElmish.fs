module Feliz.UseElmish

open Fable.Core
open Elmish

module private Util =
    type UseSyncExternalStoreSubscribe = delegate of (unit -> unit) -> (unit -> unit)

    [<ImportMember("react")>]
    let useSyncExternalStore(subscribe: UseSyncExternalStoreSubscribe, getSnapshot: unit -> 'Model): 'Model = jsNative

    [<ImportMember("react")>]
    let useState(init: unit -> 'State): 'State * ('State -> unit) = jsNative

    type ElmishState<'Arg, 'Model, 'Msg when 'Arg : equality>(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, dependencies: obj[] option) =
        // let guid = System.Guid.NewGuid()
        // do printfn "Creating %O..." guid

        // React will run the subscribe function when the component is mounted and after each hot reload
        // However we need to store the initial model here for two reasons:
        // - It looks like useSyncExternalStore returns the state before running subscribe
        // - We can restore the model after a hot reload
        let program = program()
        let mutable state, cmd =
            let model, cmd = Program.init program arg
            (model, fun (_: 'Msg) -> ()), cmd

        let subscribe = UseSyncExternalStoreSubscribe(fun callback ->
            // printfn "Subscribing %O..." guid
            let mutable dispose = false

            let mapInit _init _arg =
                let cmd' = cmd
                // Don't run the original commands after hot reload
                cmd <- Cmd.none
                fst state, cmd'

            let mapTermination (predicate, terminate) =
                (fun msg -> predicate msg || dispose),
                (fun model ->
                    // printfn "Terminating %O..." guid
                    match box model with
                    // Before Elmish 4 it was allowed to have disposable states as a hack for termination
                    | :? System.IDisposable as disp -> disp.Dispose()
                    | _ -> terminate model)

            // Restart the program after each hot reload to get the proper dispatch reference
            program
            |> Program.map mapInit id id id id mapTermination
            |> Program.withSetState (fun model dispatch ->
                let oldModel = fst state
                state <- model, dispatch
                // Skip re-renders if model hasn't changed
                if not(obj.ReferenceEquals(model, oldModel)) then
                    callback())
            |> Program.runWith arg

            (fun () ->
                // printfn "Unsubscribing %O..." guid
                dispose <- true))

        member _.State = state
        member _.Subscribe = subscribe
        member _.IsOutdated(arg', dependencies') = arg <> arg' || dependencies <> dependencies'

open Util

type React =
    [<Hook>]
    static member useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, ?dependencies: obj array): 'Model * ('Msg -> unit) =
        // Don't use useMemo here because React doesn't guarantee it won't recreate it again
        let state, setState = useState(fun () -> ElmishState(program, arg, dependencies))
        if state.IsOutdated(arg, dependencies) then
            ElmishState(program, arg, dependencies) |> setState
        useSyncExternalStore(state.Subscribe, fun () -> state.State)

    static member inline useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>, ?dependencies: obj array) =
        React.useElmish(program, (), ?dependencies=dependencies)

    static member inline useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg, ?dependencies=dependencies)

    static member inline useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), ?dependencies=dependencies)

    static member inline useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())), ?dependencies=dependencies)
