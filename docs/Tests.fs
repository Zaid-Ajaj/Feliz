module Tests

open Elmish
open Feliz
open Fable.Core
open System
open Browser.Types

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
