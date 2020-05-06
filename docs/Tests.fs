module Tests

open Elmish
open Feliz
open Fable.Core
open Fable.Core.JsInterop
open System
open Browser.Types

[
    style.display.flex
    style.display.none
    style.fontSize 20
    style.borderRadius 15
    style.textAlign.center
    style.alignContent.flexStart
    style.textDecorationColor.blue
    style.visibility.hidden
    style.textDecoration.lineThrough
    style.position.sticky
    style.borderBottomWidth 20
    style.borderBottomWidth (length.em 10)
    style.borderBottomColor color.red
    style.borderBottomStyle borderStyle.dashed
    style.borderStyle.dotted
    style.margin(length.em 1, length.em 0)
    style.marginBottom 10
    style.marginBottom (length.em 1)
    style.boxShadow(10, 10, color.black)
    style.boxShadow(10, 10, 10, color.black)
    style.boxShadow(0, 0, 10, color.black)
    style.boxShadow(0, 0, 10, 10, color.darkGray)
    style.boxShadow.none
    style.height length.auto
    style.borderRadius 20
    style.borderRadius (length.rem 10)
    style.margin 10
    style.backgroundRepeat.repeatX
    style.backgroundPosition.fixedNoScroll
    style.color.blue
    style.cursor.pointer
    style.margin (length.px 10)
    style.margin(10, 10, 10, 20)
    style.margin(10, 10, 10)
    style.margin(10, 10)
    style.margin.auto
    style.borderCollapse.collapse
    style.width 10
    style.height 100
    style.height (length.vh 50)
    style.height (length.percent 100)
    style.backgroundColor.fuchsia
    style.backgroundColor "#FFFFFF"
    style.border(3, borderStyle.dashed, color.crimson)
    style.borderColor.blue
    style.fontFamily font.aharoni
    style.transform.scale3D(20, 20, 20)
    style.transform.translateX(100)
    style.transform.translateY(100)
    style.transform.translateZ(100)
    style.transform [ transform.scale(0.5) ]
    style.textTransform.capitalize
    style.textTransform.lowercase
    style.fontStretch.extraCondensed
    style.fontVariant.smallCaps
    style.fontStyle.italic
    style.fontSize 20
    style.fontSize (length.em 2)
    style.color.crimson
    style.color "#000000"
]
|> List.iter (fun x -> Browser.Dom.console.log(createObj [!!x]))

[<Emit("alert($0)")>]
let alert s : unit = jsNative

let renderCount = React.functionComponent(fun (input: {| label: string |}) ->
    let countRef = React.useRef 0
        
    let mutable currentCount = countRef.current

    React.useEffect(fun () -> countRef.current <- currentCount)

    currentCount <- currentCount + 1

    Html.div [
        prop.text (sprintf "%s render count: %i" input.label currentCount)
    ])

let callbackRefButton = React.memo(fun (input: {| onClick: unit -> unit |}) ->
    Html.div [
        renderCount {| label = "Button" |}
        Html.button [
            prop.onClick <| fun _ -> input.onClick()
            prop.text "Show renders"
        ]
    ])

let callbackRef = React.functionComponent(fun () -> 
    let count,setCount = React.useState 1

    React.useEffect((fun () ->
        let interval = JS.setInterval(fun () -> setCount(count + 1)) 1000
        React.createDisposable(fun () -> JS.clearInterval(interval))
    ), [| count :> obj |])

    let showCount = React.useCallbackRef(fun () -> alert count)

    Html.div [
        renderCount {| label = "Main" |}
        callbackRefButton {| onClick = showCount |}
    ])

let callbackNoRef = React.functionComponent(fun () -> 
    let count,setCount = React.useState 1

    React.useEffect((fun () ->
        let interval = JS.setInterval(fun () -> setCount(count + 1)) 1000
        React.createDisposable(fun () -> JS.clearInterval(interval))
    ), [| count :> obj |])

    let showCount = React.useCallback(fun () -> alert count)

    Html.div [
        renderCount {| label = "Main" |}
        callbackRefButton {| onClick = showCount |}
    ])

let runCallbackTests = React.functionComponent(fun () ->
    Html.div [
        prop.style [ 
            style.display.inheritFromParent
        ]
        prop.children [
            Html.div [
                prop.style [
                    style.paddingRight (length.em 10)
                ]
                prop.children [
                    Html.h1 [
                        prop.style [
                            style.paddingBottom (length.em 2)
                        ]
                        prop.text "Using callbackRef"
                    ]
                    callbackRef()
                ]
            ]
            Html.div [
                prop.style [
                    style.paddingRight (length.em 10)
                ]
                prop.children [
                    Html.h1 [
                        prop.style [
                            style.paddingBottom (length.em 2)
                        ]
                        prop.text "Using callback"
                    ]
                    callbackNoRef()
                ]
            ]
        ]
    ])

let fileUpload = React.functionComponent(fun () -> [
    Html.div [
        Html.h1 "Single File Selection"
        Html.input [
            prop.type'.file
            prop.onChange (fun (file: File) -> Browser.Dom.console.log(file))
        ]

        Html.h1 "Multi-File Selection"
        Html.input [
            prop.type'.file
            prop.multiple true
            prop.onChange (fun (files: File list) -> Browser.Dom.console.log(Array.ofList files))
        ]
    ]
])


let keyboardKey = React.functionComponent(fun () -> [
    let (keyPressed, setKeyPressed) = React.useState("")

    let update() = setKeyPressed (sprintf "Key 'Enter' presssed at %s" (DateTime.Now.ToString("hh:mm:ss")))

    Html.div [
        Html.h3 "Handling ENTER"
        Html.input [
            prop.onKeyUp(key.enter, fun ev -> update())
        ]

        Html.h3 "Handling SHIFT + ENTER"
        Html.input [
            prop.onKeyUp(key.shift(key.enter), fun ev -> update())
        ]

        Html.h3 "Handling CTRL + ENTER"
        Html.input [
            prop.onKeyUp(key.ctrl(key.enter), fun ev -> update())
        ]

        Html.h3 "Handling CTRL + SHIFT + ENTER"

        Html.input [
            prop.onKeyUp(key.ctrlAndShift(key.enter), fun ev -> update())
        ]

        Html.h1 keyPressed
    ]
])

let fullFocusInputExample = React.functionComponent(fun () ->
    // obtain a reference
    let inputRef = React.useRef(None)

    let focusTextInput() =
        match inputRef.current with
        | None -> ()
        | Some element ->
            let inputElement = unbox<HTMLInputElement> element
            inputElement.focus()

    Html.div [
        Html.input [
            prop.ref inputRef
            prop.type'.text
        ]

        Html.button [
            prop.onClick (fun _ -> focusTextInput())
            prop.text "Focus Input"
        ]
    ])
