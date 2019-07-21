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
                attr.className "btn btn-success"
                attr.styleList [
                    true, [ style.marginRight 5 ]
                    state.Count >= 5, [ style.color colors.crimson; style.fontSize 16 ]
                    state.Count >= 10, [ style.color colors.white; style.fontSize 20 ]
                ]

                attr.onClick (fun _ -> dispatch Increment)
                attr.content "Increment"
            ]

            Html.button [
                attr.className "btn btn-danger"
                attr.style [ style.marginLeft 5 ]
                attr.onClick (fun _ -> dispatch Decrement)
                attr.content "Decrement"
            ]

            Html.h1 state.Count
        ]
    ]


Program.mkSimple init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run