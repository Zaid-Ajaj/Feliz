namespace Feliz.ElmishComponents

open Feliz
open Feliz.UseElmish
open Elmish
open System.ComponentModel

type ElmishComponentProps<'State, 'Msg> =
    { Initial : 'State * Cmd<'Msg>
      Update : 'Msg -> 'State -> 'State * Cmd<'Msg>
      Render : 'State -> ('Msg -> unit) -> ReactElement }

[<AutoOpen>]
module ElmishComponentsExtenstions =
    type React with
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        static member inline elmishComponentInner<'State,'Msg> (name: string) =
            React.memo(name, (fun (input: ElmishComponentProps<'State,'Msg>) ->
                let childState, dispatch = React.useElmish(input.Initial, input.Update, [| |])

                input.Render childState dispatch
            ))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        static member inline elmishComponentInner<'State,'Msg> (name: string, key: string) =
            React.memo(name, (fun (input: ElmishComponentProps<'State,'Msg>) ->
                let childState, dispatch = React.useElmish(input.Initial, input.Update, [| |])

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
