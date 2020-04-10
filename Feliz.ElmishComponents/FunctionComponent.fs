namespace Feliz.ElmishComponents

open Feliz
open Elmish
open Microsoft.FSharp.Reflection
open Fable.Core
open System.ComponentModel

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

type ElmishComponentProps<'State, 'Msg> = 
    { Initial : 'State * Cmd<'Msg>
      Update : 'Msg -> 'State -> 'State * Cmd<'Msg>
      Render : 'State -> ('Msg -> unit) -> ReactElement
      key : string }

[<AutoOpen>]
module FuncComponent =
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline getDisposables (record: 'State) = 
        FSharpType.GetRecordFields(record.GetType()) 
        |> Array.choose (fun field -> 
            match FSharpValue.GetRecordField(record,field) with
            | :? System.IDisposable as disposable -> Some disposable
            | _ -> None)

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline elmishComponent<'State,'Msg> = React.memo(fun (input: ElmishComponentProps<'State,'Msg>) ->
        let state = React.useRef(fst input.Initial)
        let ring = React.useRef(RingBuffer(10))
        let reentered = React.useRef(false)
        let childState, setChildState = React.useState(fst input.Initial)
        let setChildState = 
            React.useCallback(fun () -> 
                JS.setTimeout(fun () -> setChildState state.current) 0 
                |> ignore)

        let token = React.useRef(new System.Threading.CancellationTokenSource())
    
        let rec dispatch (msg: 'Msg) =
            promise {
                if reentered.current then
                    ring.current.Push msg
                else
                    reentered.current <- false
                    let mutable nextMsg = Some msg

                    while nextMsg.IsSome && not (token.current.IsCancellationRequested) do
                        let msg = nextMsg.Value
                        let (state', cmd') = input.Update msg state.current
                        cmd' |> List.iter (fun sub -> sub dispatch)
                        nextMsg <- ring.current.Pop()
                        state.current <- state'
                        setChildState()
                    reentered.current <- false
            }
            |> Promise.start

        let dispatch = React.useCallback(dispatch, [| token |])

        React.useEffectOnce(fun () ->
            React.createDisposable <| fun () ->
                token.current.Cancel()
                getDisposables state.current
                |> Array.iter(fun o -> o.Dispose())
                token.current.Dispose())

        React.useEffectOnce(fun () ->
            snd input.Initial
            |> List.iter (fun sub -> sub dispatch))
            
        React.useEffect(fun () -> ring.current.Pop() |> Option.iter dispatch)

        input.Render childState dispatch)

    type React with
        /// Creates a standalone React component using an Elmish dispatch loop
        [<Experimental("Functional implementation of elmish component")>]
        static member inline elmishFuncComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            elmishComponent { Initial = init; Update = update; Render = render; key = fullKey }

        /// Creates a standalone React component using an Elmish dispatch loop
        [<Experimental("Functional implementation of elmish component")>]
        static member inline elmishFuncComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            elmishComponent { Initial = init, Cmd.none; Update = update; Render = render; key = fullKey }
    
        /// Creates a standalone React component using an Elmish dispatch loop
        [<Experimental("Functional implementation of elmish component")>]
        static member inline elmishFuncComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            elmishComponent
                { Initial = init, Cmd.none;
                  Update = fun msg state -> update msg state, Cmd.none;
                  Render = render
                  key = fullKey }
    
        /// Creates a standalone React component using an Elmish dispatch loop
        [<Experimental("Functional implementation of elmish component")>]
        static member inline elmishFuncComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            elmishComponent
                { Initial = init;
                  Update = fun msg state -> update msg state, Cmd.none;
                  Render = render
                  key = fullKey }
