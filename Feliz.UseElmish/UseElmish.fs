module Feliz.UseElmish

open Fable.Core
open Elmish

module private Util =
    type UseSyncExternalStoreSubscribe = delegate of (unit -> unit) -> (unit -> unit)

    [<ImportMember("use-sync-external-store/shim")>]
    let useSyncExternalStore(subscribe: UseSyncExternalStoreSubscribe, getSnapshot: unit -> 'Model): 'Model = jsNative

    [<ImportMember("react")>]
    let useState(init: unit -> 'State): 'State * ('State -> unit) = jsNative

    [<ImportMember "react">]
    let useEffect(effect: unit -> unit, dependencies: obj array) : unit = jsNative

    [<Emit "setTimeout($0)">]
    let setTimeout(callback: unit -> unit) : unit = jsNative

    type ElmishState<'Arg, 'Model, 'Msg when 'Arg : equality>(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, dependencies: obj[] option) =
        // let guid = System.Guid.NewGuid()
        // do printfn "Creating %O..." guid

        // React will run the subscribe function when the component is mounted and after each hot reload
        // However we need to store the initial model here for two reasons:
        // - It looks like useSyncExternalStore returns the state before running subscribe
        // - We can restore the model after a hot reload
        let program = program()
        // Keep track of messages are that dispach from the initial No-Op dispatch
        // And dispatch them after the Elmish program has subscribed using the "real" dispatch
        let queuedMessages = ResizeArray<'Msg>()
        let mutable state, cmd =
            let mutable model, cmd = Program.init program arg
            // Initial dispatch is a No-Op before the Elmish program has subscribed
            // So here we store the messages and dispatch them after the Elmish program has subscribed
            let initialDispatch (msg: 'Msg) = queuedMessages.Add msg
            let subscribed = false
            (model, initialDispatch, subscribed, queuedMessages), cmd

        let subscribe = UseSyncExternalStoreSubscribe(fun callback ->
            // printfn "Subscribing %O..." guid
            let mutable dispose = false

            let mapInit _init _arg =
                let cmd' = cmd
                // Don't run the original commands after hot reload
                cmd <- Cmd.none
                let (model, _, _, _) = state
                model, cmd'

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
                let (oldModel, _, _, _) = state
                let subscribed = true
                state <- model, dispatch, subscribed, queuedMessages
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

[<Erase>]
type React =
    static member useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, ?dependencies: obj array): 'Model * ('Msg -> unit) =
        let state, setState = useState(fun () -> ElmishState(program, arg, dependencies))
        if state.IsOutdated(arg, dependencies) then
            ElmishState(program, arg, dependencies) |> setState
        let finalState, dispatch, subscribed, queuedMessages = useSyncExternalStore(state.Subscribe, fun () -> state.State)
        // Run any queued messages that were dispatched before the Elmish program finished subscribing
        useEffect((fun () -> 
            if subscribed && queuedMessages.Count > 0 then 
                for msg in queuedMessages do 
                    setTimeout(fun () -> dispatch msg)
                queuedMessages.Clear()
        ), [| subscribed; queuedMessages |])
        finalState, dispatch

    static member inline useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>, ?dependencies: obj array) =
        React.useElmish(program, (), ?dependencies=dependencies)

    static member inline useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg, ?dependencies=dependencies)

    static member inline useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), ?dependencies=dependencies)

    static member inline useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())), ?dependencies=dependencies)
