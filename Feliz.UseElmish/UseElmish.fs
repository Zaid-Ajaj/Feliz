namespace Feliz.UseElmish

open Feliz
open Elmish

[<AutoOpen>]
module UseElmishExtensions =
    type private ElmishObservable<'Model, 'Msg>() =
        let mutable hasDisposedOnce = false
        let mutable state: 'Model option = None
        let mutable listener: ('Model -> unit) option = None
        let mutable dispatcher: ('Msg -> unit) option = None

        member _.Value = state
        member _.HasDisposedOnce = hasDisposedOnce

        member _.SetState (model: 'Model) (dispatch: 'Msg -> unit) =
            state <- Some model
            dispatcher <- Some dispatch
            match listener with
            | None -> ()
            | Some listener -> listener model

        member _.Dispatch(msg) =
            match dispatcher with
            | None -> () // Error?
            | Some dispatch -> dispatch msg

        member _.Subscribe(f) =
            match listener with
            | Some _ -> ()
            | None -> listener <- Some f

        /// Disposes state (and dispatcher) but keeps subscription
        member _.DisposeState() =
            match state with
            | Some state ->
                match box state with
                | :? System.IDisposable as disp -> disp.Dispose()
                | _ -> ()
            | _ -> ()
            dispatcher <- None
            state <- None
            hasDisposedOnce <- true

    let private runProgram (program: unit -> Program<'Arg, 'Model, 'Msg, unit>) (arg: 'Arg) (obs: ElmishObservable<'Model, 'Msg>) () =
        program()
        |> Program.withSetState obs.SetState
        |> Program.runWith arg

        match obs.Value with
        | None -> failwith "Elmish program has not initialized"
        | Some v -> v        

    let disposeState (state: obj) =
        match box state with
        | :? System.IDisposable as disp -> disp.Dispose()
        | _ -> ()

    type React with
        [<Hook>]
        static member useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, ?dependencies: obj array) =
            // Don't use useMemo here because React doesn't guarantee it won't recreate it again
            let obs, _ = React.useState(fun () -> ElmishObservable<'Model, 'Msg>())

            let state, setState = React.useState(runProgram program arg obs)

            React.useEffect((fun () ->
                if obs.HasDisposedOnce then
                    runProgram program arg obs () |> setState
                React.createDisposable(obs.DisposeState)
            ), defaultArg dependencies [||])

            obs.Subscribe(setState)
            state, obs.Dispatch

        [<Hook>]
        static member useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>, ?dependencies: obj array) =
            React.useElmish(program, (), ?dependencies=dependencies)

        [<Hook>]
        static member useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg, ?dependencies=dependencies)

        [<Hook>]
        static member useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), ?dependencies=dependencies)

        [<Hook>]
        static member useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())), ?dependencies=dependencies)
