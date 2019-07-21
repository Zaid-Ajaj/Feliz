module App

open Elmish
open Elmish.React
open Feliz

type State = { Count : int }

let init() = { Count = 0 }

type Msg = Increment | Decrement

let update msg state =
    match msg with
    | Increment -> { state with Count = state.Count + 1  }
    | Decrement -> { state with Count = state.Count - 1  }

let render state dispatch =
    Html.div [
        attr.id "main"
        attr.style [ style.padding 20 ]
        attr.children [
            Html.button [
                attr.style [ style.marginRight 5 ]
                attr.onClick (fun _ -> dispatch Increment)
                attr.content "Increment"
            ]

            Html.button [
                attr.style [ style.marginRight 5 ]
                attr.onClick (fun _ -> dispatch Decrement)
                attr.content "Increment"
            ]

            Html.h1 state.Count
        ]
    ]


Program.mkSimple init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run