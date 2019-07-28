module App

open Elmish
open Elmish.React
open Feliz

open Fable.Core
open Fable.Core.JsInterop

type State = { Count : int }

let init() = { Count = 0 }

type Msg = Increment | Decrement

let update msg state =
    match msg with
    | Increment -> { state with Count = state.Count + 1  }
    | Decrement -> { state with Count = state.Count - 1  }

[
    style.display.flex
    style.display.none
    style.fontSize 20
    style.borderRadius 15
    style.alignContent.flexStart
    style.textDecorationColor.blue
    style.visibility.hidden
    style.textDecoration.lineThrough
    style.position.sticky
    style.boxShadow(10, 10, colors.black)
    style.boxShadow(10, 10, 10, colors.black)
    style.boxShadow(0, 0, 10, colors.black)
    style.boxShadow(0, 0, 10, 10, colors.darkGray)
    style.borderRadius (length.px 10)
    style.margin 10
    style.backgroundRepeat.repeatX
    style.backgroundPosition.fixedNoScroll
    style.margin (length.px 10)
    style.margin(10, 10, 10, 20)
    style.margin(10, 10, 10)
    style.margin(10, 10)
    style.width 10
    style.height 100
    style.height (length.vh 50)
    style.height (length.percent 100)
    style.backgroundColor.fuchsia
    style.backgroundColor "#FFFFFF"
    style.border(3, borderStyle.dashed, colors.crimson)
    style.borderColor.blue
    style.transform.scale3D(20, 20, 20)
    style.transform.translateX(100)
    style.transform.translateY(100)
    style.transform.translateZ(100)
    style.color.red
    style.color "#000000"
]
|> List.iter (fun x -> Browser.Dom.console.log(keyValueList CaseRules.LowerFirst [x]))

let counterApp state dispatch =
    Html.div [
        prop.id "main"
        prop.style [ style.padding 20 ]
        prop.children [

            Html.button [
                prop.style [ style.marginRight 5 ]
                prop.onClick (fun _ -> dispatch Increment)
                prop.innerText "Increment"
            ]

            Html.button [
                prop.style [ style.marginLeft 5 ]
                prop.onClick (fun _ -> dispatch Decrement)
                prop.innerText "Decrement"
            ]

            Html.h1 state.Count
        ]
    ]

let keyWarnings state dispatch =
    Html.div [
        prop.id "id"
        prop.className "class"
        prop.children [
            Html.div "text"
            Html.div [
                prop.id "id"
                prop.className "class"
                prop.children [
                    Html.text "text"
                    Html.div [
                        prop.id "id"
                        prop.className "class"
                        prop.children [
                            Html.text "text"
                            Html.div [
                                prop.id "id"
                                prop.className "class"
                                prop.children [
                                    Html.text "text"
                                    Html.div []
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

let styleComponent title =
    // Re-use state internally using React hooks
    let animationsOnHover =
        Fable.React.FunctionComponent.Of <| fun (props: {| title: string |}) ->
            let hovered = Hooks.useState(false)
            Html.div [
                prop.style [
                    yield! [
                        style.padding 10
                        style.transitionProperty("background-color", "color")
                        style.transitionDurationMilliseconds 500
                    ]

                    if hovered.current then
                       yield style.backgroundColor.lightBlue
                       yield style.color.black
                    else
                       yield style.backgroundColor.limeGreen
                       yield style.color.white
                ]

                prop.onMouseEnter (fun _ -> hovered.update(fun current -> true))
                prop.onMouseLeave (fun _ -> hovered.update(fun current -> false))
                prop.children [
                    Html.h2 props.title
                ]
            ]

    Interop.reactApi.createElement(animationsOnHover, {| title = title |})

let render state dispatch =
    Html.div [
        prop.children [
            styleComponent "Hello"
            styleComponent "From"
            styleComponent "Fable"
        ]
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run