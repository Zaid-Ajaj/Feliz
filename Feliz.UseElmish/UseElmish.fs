namespace Feliz.UseElmish

open Feliz
open Elmish
open Fable.React

[<AutoOpen>]
module UseElmishExtensions =
    type ElmishObservable<'Model, 'Msg>() =
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
            // dispatcher <- None
            // state <- None
            hasDisposedOnce <- true

    let runProgram (program: unit -> Program<'Arg, 'Model, 'Msg, unit>) (arg: 'Arg) (obs: ElmishObservable<'Model, 'Msg>) () =
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
        static member
#if DEBUG
            inline
#endif
            useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, ?dependencies: obj array) =

            // Don't use useMemo here because React doesn't guarantee it won't recreate it again
            let obs = Hooks.useStateLazy(fun () -> ElmishObservable<'Model, 'Msg>())
            let obs = obs.current

            let state = Hooks.useStateLazy(runProgram program arg obs)

            Hooks.useEffectDisposable((fun () ->
                if obs.HasDisposedOnce then
                    // In Debug mode, a component may be disconnected and reconnected during HMR
                    // TODO: We need a way to tell if the effect has been triggered because of a change in dependencies,
                    // in that case state does need to be reset.
                    let newValue =
#if DEBUG
                        let curValue = obs.Value
                        let newValue = runProgram program arg obs ()
                        defaultArg curValue newValue
#else
                        runProgram program arg obs ()
#endif
                    state.update(fun _ -> newValue)
                React.createDisposable(obs.DisposeState)
            ), defaultArg dependencies [||])

            obs.Subscribe(fun v -> state.update(fun _ -> v))
            state.current, obs.Dispatch

        static member 
#if DEBUG
            inline
#endif        
            useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>, ?dependencies: obj array) =
            React.useElmish(program, (), ?dependencies=dependencies)

        static member 
#if DEBUG
            inline
#endif        
            useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg, ?dependencies=dependencies)

        static member 
#if DEBUG
            inline
#endif        
            useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), ?dependencies=dependencies)

        static member 
#if DEBUG
            inline
#endif        
            useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
            React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())), ?dependencies=dependencies)
