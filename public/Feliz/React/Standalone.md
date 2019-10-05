# Standalone React Applications

Feliz can be used to build React applications standalone without Elmish which is responsible for the state and effect management

```fsharp
module App

open Feliz

let counter = React.functionComponent <| fun () ->
    let (count, setCount) = React.useState(0)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ]

open Browser.Dom

ReactDOM.render(counter, document.getElementById "root")
```