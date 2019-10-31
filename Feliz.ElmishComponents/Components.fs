namespace Feliz.ElmishComponents

open Feliz
open Elmish

type ElmishComponentProps<'State, 'Msg> = {
    Initial : 'State * Cmd<'Msg>
    Update : 'Msg -> 'State -> 'State * Cmd<'Msg>
    Render : 'State -> ('Msg -> unit) -> ReactElement
    key : string
}

/// A React component that implements an internal Elmish dispatch loop using the program parts of `init`, `update` and `render`.
type ElmishComponent<'State, 'Msg>(props: ElmishComponentProps<'State, 'Msg>) as this =
    inherit Fable.React.Component<ElmishComponentProps<'State, 'Msg>, 'State>(props)

    do
        let initialState = fst this.props.Initial
        this.setInitState(initialState)

    override this.componentDidMount() =
        let initialEffect = snd this.props.Initial
        for subscriber in initialEffect
            do subscriber(this.dispatch)

    override this.componentDidUpdate(prevProps, prevState) =
        // if props changed reference, re-execute `init` from the program definition
        if not (System.Object.ReferenceEquals(prevProps, this.props)) then
            let initialEffect = snd this.props.Initial
            for subscriber in initialEffect
                do subscriber(this.dispatch)

    member this.dispatch(msg: 'Msg) =
        let (nextState, nextEffect) = this.props.Update msg this.state
        this.setState(fun _ _ -> nextState)
        for subscriber in nextEffect do
            Browser.Dom.window.setTimeout((fun _ -> subscriber(this.dispatch)), 0)
            |> ignore

    override this.render() =
        this.props.Render this.state this.dispatch

[<AutoOpen>]
module ElmishComponentExtensiosns =

    type React with
        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name, init, update, render, ?key:string) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key
            Fable.React.Helpers.ofType<ElmishComponent<_, _>, _, _>
                { Initial = init; Update = update; Render = render; key = fullKey }
                [ ]

        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key
            Fable.React.Helpers.ofType<ElmishComponent<_, _>, _, _>
                { Initial = init, Cmd.none; Update = update; Render = render; key = fullKey }
                [ ]

        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key
            Fable.React.Helpers.ofType<ElmishComponent<_, _>, _, _>
                { Initial = init, Cmd.none;
                  Update = fun msg state -> update msg state, Cmd.none;
                  Render = render
                  key = fullKey }
                [ ]

        /// Creates a standalone React component using an Elmish dispatch loop
        static member inline elmishComponent(name, init, update, render, ?key) =
            let fullKey =
                match key with
                | None -> name
                | Some key -> name + "-" + key
            Fable.React.Helpers.ofType<ElmishComponent<_, _>, _, _>
                { Initial = init;
                  Update = fun msg state -> update msg state, Cmd.none;
                  Render = render
                  key = fullKey }
                [ ]