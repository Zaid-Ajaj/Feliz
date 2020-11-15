namespace Feliz.UseElmish

open Feliz
open Elmish
open Fable.Core

[<Struct>]
type internal RingState<'item> =
    | Writable of wx:'item array * ix:int
    | ReadWritable of rw:'item array * wix:int * rix:int

type internal RingBuffer<'item>(size) =
    let doubleSize ix (items: 'item array) =
        seq { yield! items |> Seq.skip ix
              yield! items |> Seq.take ix
              for _ in 0..items.Length do
                yield Unchecked.defaultof<'item> }
        |> Array.ofSeq

    let mutable state : 'item RingState =
        Writable (Array.zeroCreate (max size 10), 0)

    member _.Pop() =
        match state with
        | ReadWritable (items, wix, rix) ->
            let rix' = (rix + 1) % items.Length
            match rix' = wix with
            | true ->
                state <- Writable(items, wix)
            | _ ->
                state <- ReadWritable(items, wix, rix')
            Some items.[rix]
        | _ ->
            None

    member _.Push (item:'item) =
        match state with
        | Writable (items, ix) ->
            items.[ix] <- item
            let wix = (ix + 1) % items.Length
            state <- ReadWritable(items, wix, ix)
        | ReadWritable (items, wix, rix) ->
            items.[wix] <- item
            let wix' = (wix + 1) % items.Length
            match wix' = rix with
            | true ->
                state <- ReadWritable(items |> doubleSize rix, items.Length, 0)
            | _ ->
                state <- ReadWritable(items, wix', rix)

[<AutoOpen>]
module UseElmishExtensions =
    let inline internal getDisposable (record: 'State) =
        match box record with
        | :? System.IDisposable as disposable -> Some disposable
        | _ -> None

    type React with
        [<Hook>]
        static member useElmish<'State,'Msg> (init: 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State * Cmd<'Msg>, dependencies: obj[]) =
            let state = React.useRef(fst init)
            let ring = React.useRef(RingBuffer(10))
            let childState, setChildState = React.useState(fst init)
            let token = React.useCancellationToken()
            let setChildState () =
                JS.setTimeout(fun () ->
                    if not token.current.IsCancellationRequested then
                        setChildState state.current
                ) 0 |> ignore

            let rec dispatch (msg: 'Msg) =
                promise {
                    let mutable nextMsg = Some msg

                    while nextMsg.IsSome && not (token.current.IsCancellationRequested) do
                        let msg = nextMsg.Value
                        let (state', cmd') = update msg state.current
                        cmd' |> List.iter (fun sub -> sub dispatch)
                        nextMsg <- ring.current.Pop()
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
                state.current <- fst init
                setChildState()

                snd init
                |> List.iter (fun sub -> sub dispatch)
            ), dependencies)

            React.useEffect(fun () -> ring.current.Pop() |> Option.iter dispatch)

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
