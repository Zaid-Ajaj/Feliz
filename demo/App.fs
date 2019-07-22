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

let customStyles =
    prop.style [
        style.display display.none
        style.fontSize 20
        style.borderRadius 15
        style.borderRadius (length.px 10)
        style.margin 10
        style.margin (length.px 10)
        style.margin(10, 10, 10, 20)
        style.width 10
        style.height 100
        style.height (length.vh 50)
        style.height (length.percent 100)
        style.backgroundColor colors.fuchsia
        style.border(3, borderStyle.dashed, colors.crimson)
        style.borderColor colors.yellowGreen
        style.color colors.lightGoldenRodYellow
        style.alignContent alignContent.flexStart
        style.textDecorationColor colors.red
        style.visibility visibility.hidden
        style.textDecoration textDecorationLine.lineThrough
        style.position position.sticky
    ]

let render state dispatch =
    Html.div [
        prop.id "main"
        prop.style [ style.padding 20 ]
        prop.children [

            Html.button [
                prop.style [ style.marginRight 5 ]
                prop.onClick (fun _ -> dispatch Increment)
                prop.content "Increment"
            ]

            Html.button [
                prop.style [ style.marginLeft 5 ]
                prop.onClick (fun _ -> dispatch Decrement)
                prop.content "Decrement"
            ]

            Html.h1 state.Count
        ]
    ]


Program.mkSimple init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run