namespace Feliz.UseElmish

open Feliz
open Elmish
open Fable.Core

[<AutoOpen>]
module UseElmishExtensions =
    let inline internal getDisposable (record: 'State) =
        match box record with
        | :? System.IDisposable as disposable -> Some disposable
        | _ -> None

    type React with
        [<Hook>]
        static member useElmish<'State,'Msg> (init: 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State * Cmd<'Msg>, dependencies: obj[]) =
            let childState, setChildState = React.useState(fst init)
            let state = React.useRef(childState)
            let isFirstRender = React.useRef(true)
            let token = React.useCancellationToken()
            let setChildState () =
                JS.setTimeout(fun () ->
                    if not token.current.IsCancellationRequested then
                        setChildState state.current
                ) 0 |> ignore

            let rec dispatch (msg: 'Msg) =
                promise {
                    if not (token.current.IsCancellationRequested) then
                        let (state', cmd') = update msg state.current
                        cmd' |> List.iter (fun sub -> sub dispatch)
                        state.current <- state'
                        setChildState()
                }
                |> Promise.start

            let dispatch = React.useCallbackRef(dispatch)

            React.useEffect((fun () ->
                React.createDisposable(fun () ->
                    getDisposable state.current
                    |> Option.iter (fun o -> o.Dispose())
                )
            ), dependencies)

            React.useEffect((fun () ->
                // don't unnecessarily reset childState on first render
                if isFirstRender.current then
                    isFirstRender.current <- false
                else
                    state.current <- fst init
                    setChildState()

                snd init
                |> List.iter (fun sub -> sub dispatch)
            ), dependencies)

            (childState, dispatch)

        [<Hook>]
        static member inline useElmish<'State,'Msg> (init: 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State * Cmd<'Msg>) =
            React.useElmish(init, update, [||])

        [<Hook>]
        static member useElmish<'State,'Msg> (init: unit -> 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State * Cmd<'Msg>, dependencies: obj[]) =
            let init = React.useMemo(init, dependencies)

            React.useElmish(init, update, dependencies)

        [<Hook>]
        static member inline useElmish<'State,'Msg> (init: unit -> 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State * Cmd<'Msg>) =
            React.useElmish(init, update, [||])
