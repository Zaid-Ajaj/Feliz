module DelayedComponent

open Elmish
open Feliz
open Zanaptak.TypedCssClasses

type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>
type FA = CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>

let render = React.functionComponent (fun (props: {| load: unit -> ReactElement |}) ->
    let (started, setStarted) = React.useState(false)
    Html.div [
        if not started then Html.button [
            prop.className [ Bulma.Button; Bulma.IsPrimary; Bulma.IsLarge ]
            prop.onClick (fun _ -> setStarted(true))
            prop.children [
                Html.i [ prop.className [ "fa"; "fa-rocket" ] ]
                Html.span [
                    prop.style [ style.marginLeft 10 ]
                    prop.text "Run Sample"
                ]
            ]
        ]

        if started then props.load()
    ])
