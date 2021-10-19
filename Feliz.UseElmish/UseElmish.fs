namespace Feliz.UseElmish

open Feliz
open Elmish

type private ElmishObservable<'State, 'Msg>() =
    let mutable state: 'State option = None
    let mutable listener: ('State -> unit) option = None
    let mutable dispatcher: ('Msg -> unit) option = None

    member _.Value = state

    member _.SetState (model: 'State) (dispatch: 'Msg -> unit) =
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

[<AutoOpen>]
module UseElmishExtensions =
    type React with
        [<Hook>]
        static member useElmish(program: unit -> Program<'arg, 'State, 'Msg, unit>, arg: 'arg) =
            // Don't use useMemo here because React doesn't guarantee it won't recreate it again
            let obs, _ = React.useState(fun () -> ElmishObservable())

            let state, setState = React.useState(fun () ->
                program()
                |> Program.withSetState obs.SetState
                |> Program.runWith arg

                match obs.Value with
                | None -> failwith "Elmish program has not initialized"
                | Some v -> v)

            React.useEffectOnce(fun () ->
                React.createDisposable(fun () ->
                        match box state with
                        | :? System.IDisposable as disp -> disp.Dispose()
                        | _ -> ()))

            obs.Subscribe(setState)
            state, obs.Dispatch

        [<Hook>]
        static member useElmish(program: unit -> Program<unit, 'State, 'Msg, unit>) =
            React.useElmish(program, ())

        static member useElmish(init, update, arg, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg)

        static member useElmish(init, update, ?dependencies: obj array) =
            React.useElmish(fun () -> Program.mkProgram init update (fun _ _ -> ()))

        static member useElmish(initial, update, ?dependencies: obj array) =
            React.useElmish(fun () -> Program.mkProgram (fun () -> initial) update (fun _ _ -> ()))
