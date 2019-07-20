module App

open Elmish
open Elmish.React
open Feliz
open Fable.React.Props

type State = { Count : int }

let init() = { Count = 0 }

type Msg = Increment | Decrement

let update msg state =
    match msg with
    | Increment -> { state with Count = state.Count + 1  }
    | Decrement -> { state with Count = state.Count - 1  }

let customStyles =
    attr.style [
        style.fontSize 20
        style.borderRadius 15
        style.borderRadius "20px"
        style.margin 10
        style.margin "20px"
        style.margin(10, 10, 10, 20)
        style.width 10
        style.height 100
        style.height "100%"
        style.backgroundColor colors.fuchsia
        style.border(3, borderStyle.dashed, colors.crimson)
        style.borderColor colors.yellowGreen
        style.color colors.lightGoldenRodYellow
        style.alignContent alignContent.flexStart
        style.textDecorationColor colors.red
        style.visibility visibility.hidden
        style.textDecoration textDecorationLine.lineThrough
        style.position position.sticky
        style.display display.flex
    ]

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
                attr.content "Decrement"
            ]

            Html.h1 state.Count
        ]
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run