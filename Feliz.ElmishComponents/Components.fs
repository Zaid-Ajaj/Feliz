namespace Feliz.ElmishComponents

open Feliz
open Elmish
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
      Render : 'State -> ('Msg -> unit) -> ReactElement }

[<AutoOpen>]
module ElmishComponentExtensions =
    let inline internal getDisposable (record: 'State) = 
        match box record with
        | :? System.IDisposable as disposable -> Some disposable
        | _ -> None

    type React with
        static member useElmish<'State,'Msg> (init: 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State * Cmd<'Msg>) =
            let state = React.useRef(fst init)
            let ring = React.useRef(RingBuffer(10))
            let reentered = React.useRef(false)
            let childState, setChildState = React.useState(fst init)
            let setChildState () = JS.setTimeout(fun () -> setChildState state.current) 0 |> ignore

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
                            let (state', cmd') = update msg state.current
                            cmd' |> List.iter (fun sub -> sub dispatch)
                            nextMsg <- ring.current.Pop()
                            state.current <- state'
                            setChildState()
                        reentered.current <- false
                }
                |> Promise.start

            let dispatch = React.useCallbackRef(dispatch)

            React.useEffectOnce(fun () ->
                React.createDisposable <| fun () ->
                    token.current.Cancel()
                    getDisposable state.current
                    |> Option.iter (fun o -> o.Dispose())
                    token.current.Dispose())

            React.useEffectOnce(fun () ->
                snd init
                |> List.iter (fun sub -> sub dispatch))
                
            React.useEffect(fun () -> ring.current.Pop() |> Option.iter dispatch)

            (childState, dispatch)

        static member inline useElmish<'State,'Msg> (init: 'State, update: 'Msg -> 'State -> 'State * Cmd<'Msg>) =
            React.useElmish<'State,'Msg>((init, Cmd.none), update)

        static member inline useElmis<'State,'Msg>h (init: 'State * Cmd<'Msg>, update: 'Msg -> 'State -> 'State) =
            React.useElmish<'State,'Msg>(init, (fun msg state -> update msg state, Cmd.none))

        static member inline useElmish<'State,'Msg> (init: 'State, update: 'Msg -> 'State -> 'State) =
            React.useElmish<'State,'Msg>((init, Cmd.none), (fun msg state -> update msg state, Cmd.none))

    type React with
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        static member inline elmishComponentInner<'State,'Msg> (name: string) =
            React.memo(name, (fun (input: ElmishComponentProps<'State,'Msg>) ->
                let childState, dispatch = React.useElmish(input.Initial, input.Update)

                input.Render childState dispatch
            ))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        static member inline elmishComponentInner<'State,'Msg> (name: string, key: string) =
            React.memo(name, (fun (input: ElmishComponentProps<'State,'Msg>) ->
                let childState, dispatch = React.useElmish(input.Initial, input.Update)

                input.Render childState dispatch
            ), withKey = (fun _ -> key))

        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name: string, init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, render: 'Model -> ('Msg -> unit) -> ReactElement, ?key: string) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            React.elmishComponentInner<'Model,'Msg>(name, fullKey) { 
                Initial = init
                Update = update
                Render = render
            }

        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name: string, init: 'Model, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, render: 'Model -> ('Msg -> unit) -> ReactElement, ?key: string)  =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            React.elmishComponentInner<'Model,'Msg>(name, fullKey) { 
                Initial = (init, Cmd.none)
                Update = update
                Render = render
            }
    
        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name: string, init: 'Model, update: 'Msg -> 'Model -> 'Model, render: 'Model -> ('Msg -> unit) -> ReactElement, ?key: string) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            React.elmishComponentInner<'Model,'Msg>(name, fullKey) { 
                Initial = (init, Cmd.none)
                Update = (fun msg state -> update msg state, Cmd.none)
                Render = render
            }
    
        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name: string, init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model, render: 'Model -> ('Msg -> unit) -> ReactElement, ?key: string) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key

            React.elmishComponentInner<'Model,'Msg>(name, fullKey) { 
                Initial = init
                Update = (fun msg state -> update msg state, Cmd.none)
                Render = render
            }
