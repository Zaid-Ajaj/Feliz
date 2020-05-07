module Tests

open System
open Fable.Mocha
open Feliz
open Browser
open Browser.Dom
open Browser.Types
open Fable.ReactTestingLibrary
open Fable.Core

let counter = React.functionComponent(fun (props: {| initialCount: int |}) ->
    let (count, setCount) = React.useState(props.initialCount)
    Html.div [
        Html.h1 [
            prop.testId "count"
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount (count + 1))
            prop.testId "increment"
        ]
    ])

let counterWithDebugValue = React.functionComponent(fun () ->
    let (count, setCount) = React.useState(0)
    React.useDebugValue(sprintf "Count is %d" count)
    Html.div [
        Html.h1 [
            prop.testId "count"
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount (count + 1))
            prop.testId "increment"
        ]
    ])

let effectOnceComponent = React.functionComponent(fun (props: {| effectTriggered: unit -> unit |}) ->
    let (count, setCount) = React.useState(0)
    React.useEffectOnce(fun _ -> props.effectTriggered())
    Html.div [
        Html.h1 [
            prop.testId "count"
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount (count + 1))
            prop.testId "increment"
        ]
    ])

let useEffectEveryRender = React.functionComponent(fun (props: {| effectTriggered: unit -> unit |}) ->
    let (count, setCount) = React.useState(0)
    React.useEffect(fun _ -> props.effectTriggered())
    Html.div [
        Html.h1 [
            prop.testId "count"
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount (count + 1))
            prop.testId "increment"
        ]
    ])

let useLayoutEffectEveryRender = React.functionComponent(fun (props: {| effectTriggered: unit -> unit |}) ->
    let (count, setCount) = React.useState(0)
    React.useLayoutEffect(fun _ -> props.effectTriggered())
    Html.div [
        Html.h1 [
            prop.testId "count"
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount (count + 1))
            prop.testId "increment"
        ]
    ])

let renderCount = React.functionComponent(fun (input: {| label: string |}) ->
    let countRef = React.useRef 0
        
    let mutable currentCount = countRef.current

    React.useEffect(fun () -> countRef.current <- currentCount)

    currentCount <- currentCount + 1

    Html.div [
        prop.testId input.label
        prop.text (sprintf "%i" currentCount)
    ])

let callbackRefButton = React.memo(fun (input: {| onClick: unit -> unit |}) ->
    Html.div [
        renderCount {| label = "Button" |}
        Html.button [
            prop.testId "callbackRefButton"
            prop.onClick <| fun _ -> input.onClick()
            prop.text "Show renders"
        ]
    ])

let callbackRef = React.functionComponent(fun () -> 
    let count,setCount = React.useState 1
    let resultText,setResultText = React.useState ""

    React.useEffect((fun () ->
        let interval = JS.setInterval(fun () -> setCount(count + 1)) 1000
        React.createDisposable(fun () -> JS.clearInterval(interval))
    ), [| count :> obj |])

    let showCount = React.useCallbackRef(fun () -> setResultText (string count))

    Html.div [
        prop.testId "callbackRef"
        prop.text resultText
        prop.children [
            renderCount {| label = "Main" |}
            callbackRefButton {| onClick = showCount |}
        ]
    ])


let focusInputExample = React.functionComponent(fun () ->
    let inputRef = React.useInputRef()
    let focusTextInput() = inputRef.current |> Option.iter (fun el -> el.focus())

    Html.div [
        Html.input [
            prop.ref inputRef
            prop.type'.text
            prop.testId "focused-input"
        ]

        Html.button [
            prop.testId "focus-input"
            prop.onClick (fun _ -> focusTextInput())
            prop.text "Focus Input"
        ]
    ])

let forwardRefChild = React.forwardRef("forwardChild", fun ((), ref) ->
    Html.div [
        prop.children [
            Html.input [
                prop.testId "focus-input"
                prop.type'.text
                prop.ref ref
            ]
        ]
    ])

let forwardRefParent = React.functionComponent(fun () ->
    let inputRef = React.useInputRef()

    Html.div [
        prop.children [
            forwardRefChild((), inputRef)
            Html.button [
                prop.testId "focus-button"
                prop.text "Click!"
                prop.onClick <| fun ev ->
                    inputRef.current 
                    |> Option.iter (fun elem -> elem.focus())
            ]
        ]
    ])

let forwardRefImperativeChild = React.forwardRef(fun ((), ref) ->
    let divText,setDivText = React.useState ""
    let inputRef = React.useInputRef()

    React.useImperativeHandle(ref, fun () ->
        inputRef.current
        |> Option.map(fun innerRef ->
            {| focus = fun () -> setDivText innerRef.className |})
    )

    Html.div [
        Html.input [
            prop.className "Howdy!"
            prop.type'.text
            prop.ref inputRef
        ]
        Html.div [
            prop.testId "focus-text"
            prop.text divText
        ]
    ])

let forwardRefImperativeParent = React.functionComponent(fun () ->
    let ref = React.useRef<{| focus: unit -> unit |} option>(None)

    Html.div [
        forwardRefImperativeChild((), ref)
        Html.button [
            prop.testId "focus-button"
            prop.onClick <| fun ev ->
                ref.current 
                |> Option.iter (fun elem -> elem.focus())
        ]
    ])

let codeSplittingLoading = React.functionComponent(fun () ->
    Html.div [ 
        prop.testId "loading"
        prop.text "Loading" 
    ])

let asyncComponent : JS.Promise<unit -> ReactElement> = JsInterop.importDynamic "./CodeSplitting.fs"

let codeSplitting = React.functionComponent(fun () ->
    React.suspense([
        Html.div [
            React.lazy'((fun () -> 
                promise { 
                    do! Promise.sleep 1000
                    return! asyncComponent
                }
            ),())
        ]
    ], codeSplittingLoading()))

let funcCompTest = React.functionComponent(fun (input: {| count: int |}) ->
    Html.div [
        renderCount {| label = "funcCompTest" |}
        Html.div [
            prop.text (string input.count)
        ]
    ])

let funcCompTestDiff = React.functionComponent(fun () ->
    let count,setCount = React.useState 0
    
    React.useEffectOnce(fun () -> setCount (count + 1))

    Html.div [
        funcCompTest {| count = count |}
    ])

let funcCompWithKey (props: {| count: int |} ) = 
    if props.count = 0 then "originalKey" 
    else "staticKey"

let funcCompTestWithKey = React.functionComponent((fun (input: {| count: int |}) ->
    Html.div [
        renderCount {| label = "funcCompTestWithKey" |}
        Html.div [
            prop.text (string input.count)
        ]
    ]), funcCompWithKey)

let funcCompTestWithKeyDiff = React.functionComponent(fun () ->
    let count,setCount = React.useState 0
    
    React.useEffectOnce(fun () -> setCount (count + 1))

    Html.div [
        funcCompTestWithKey {| count = count |}
    ])

let memoCompTest = React.memo(fun (input: {| count: int |}) ->
    Html.div [
        renderCount {| label = "memoCompTest" |}
        Html.div [
            prop.text (string input.count)
        ]
    ])

let memoCompTestDiff = React.functionComponent(fun () ->
    let count,setCount = React.useState 0
    
    React.useEffectOnce(fun () -> setCount (count + 1))

    Html.div [
        memoCompTest {| count = count |}
    ])

let memoCompTestWithKey = React.memo((fun (input: {| count: int |}) ->
    Html.div [
        renderCount {| label = "memoCompTestWithKey" |}
        Html.div [
            prop.text (string input.count)
        ]
    ]), funcCompWithKey)

let memoCompTestWithKeyDiff = React.functionComponent(fun () ->
    let count,setCount = React.useState 0
    
    React.useEffectOnce(fun () -> setCount (count + 1))

    Html.div [
        memoCompTestWithKey {| count = count |}
    ])

let memoCompareEqual oldProps newProps = true

let memoCompTestAreEqual = React.memo((fun (input: {| count: int |}) ->
    Html.div [
        renderCount {| label = "memoCompTestAreEqual" |}
        Html.div [
            prop.text (string input.count)
        ]
    ]), areEqual = memoCompareEqual)

let memoCompTestAreEqualDiff = React.functionComponent(fun () ->
    let count,setCount = React.useState 0
    
    React.useEffectOnce(fun () -> setCount (count + 1))

    Html.div [
        memoCompTestAreEqual {| count = count |}
    ])

let memoCompAreEqual (oldProps: {| count: int |}) (newProps: {| count: int |}) =
    if oldProps.count < 2 then false
    else true

let memoCompWithKey (props: {| count: int |}) =
    if props.count > 2 then "2"
    else System.Guid() |> unbox<string>

let memoCompTestAreEqualWithKey = React.memo((fun (input: {| count: int |}) ->
    Html.div [
        renderCount {| label = "memoCompTestAreEqualWithKey" |}
        Html.div [
            prop.text (string input.count)
        ]
    ]), memoCompWithKey, areEqual = memoCompAreEqual)

let memoCompTestAreEqualWithKeyDiff = React.functionComponent(fun () ->
    let count,setCount = React.useState 0
    
    React.useEffect(fun () -> if count < 10 then setCount (count + 1))

    Html.div [
        memoCompTestAreEqualWithKey {| count = count |}
    ])

let felizTests = testList "Feliz Tests" [

    testCase "Html elements can be rendered" <| fun _ ->
        use renderer = renderReact(Html.div [
            Html.h1 [
                prop.testId "header"
                prop.text "Hello world"
            ]
        ])

        let container = renderer.Container()
        Expect.equal "Hello world" container.innerText "The content is correct"

    testReact "RTL works with Feliz" <| fun _ ->
        let render = RTL.render(Html.div [
            Html.h1 [
                prop.testId "header"
                prop.text "Hello world"
            ]
        ])

        let header = render.getByTestId "header"
        Expect.equal "Hello world" header.innerText "The content is correct"

    testReact "React function component works: counter example" <| fun _ ->
        let render = RTL.render(counter {| initialCount = 10 |})
        let count = render.getByTestId "count"
        let increment = render.getByTestId "increment"
        Expect.equal "10" count.innerText "Initial count is rendered"
        RTL.userEvent.click(increment)
        Expect.equal "11" count.innerText "After one click, the count becomes 11"
        RTL.userEvent.click(increment)
        Expect.equal "12" count.innerText "After another click, the count becomes 12"

    testReact "React function component works: counter example with debug value" <| fun _ ->
        let render = RTL.render(counterWithDebugValue())
        let count = render.getByTestId "count"
        let increment = render.getByTestId "increment"
        Expect.equal "0" count.innerText "Initial count is rendered"
        RTL.userEvent.click(increment)
        Expect.equal "1" count.innerText "After one click, the count becomes 11"
        RTL.userEvent.click(increment)
        Expect.equal "2" count.innerText "After another click, the count becomes 12"

    testReact "React.useEffectOnce(unit -> unit) executes onece" <| fun _ ->
        let mutable effectCount = 0
        let render = RTL.render(effectOnceComponent {| effectTriggered = fun () -> effectCount <- effectCount + 1 |})
        let count = render.getByTestId "count"
        let increment = render.getByTestId "increment"
        Expect.equal 1 effectCount "Effect has been been executed once when the component has been rendered"
        Expect.equal "0" count.innerText "Count has initial value of zero"
        RTL.userEvent.click(increment)
        Expect.equal "1" count.innerText "Component has been updated/re-rendered"
        Expect.equal 1 effectCount "Effect count stays the same"
        RTL.userEvent.click(increment)
        Expect.equal "2" count.innerText "Component has been updated/re-rendered again"
        Expect.equal 1 effectCount "Effect count stays the same"

    testReactAsync "React.useEffect(unit -> unit) executes on each (re)render" <| async {
        let mutable effectCount = 0
        let render = RTL.render(useEffectEveryRender {| effectTriggered = fun () -> effectCount <- effectCount + 1 |})
        let count = render.getByTestId "count"
        let increment = render.getByTestId "increment"
        Expect.equal 1 effectCount "Effect has been been executed when the component has been rendered"
        Expect.equal "0" count.innerText "Count has initial value of zero"
        RTL.userEvent.click(increment)
        do! Async.Sleep 100

        Expect.equal "1" count.innerText "Component has been updated/re-rendered"
        Expect.equal 2 effectCount "Effect count has increased to two"
        RTL.userEvent.click(increment)

        do! Async.Sleep 100
        Expect.equal "2" count.innerText "Component has been updated/re-rendered again"
        Expect.equal 3 effectCount "Effect count has increased three"
    }

    testReact "React.useLayoutEffect(unit -> unit) executes on each (re)render" <| fun _ ->
        let mutable effectCount = 0
        let render = RTL.render(useLayoutEffectEveryRender {| effectTriggered = fun () -> effectCount <- effectCount + 1 |})
        let count = render.getByTestId "count"
        let increment = render.getByTestId "increment"
        Expect.equal 1 effectCount "Effect has been been executed when the component has been rendered"
        Expect.equal "0" count.innerText "Count has initial value of zero"
        RTL.userEvent.click(increment)
        Expect.equal "1" count.innerText "Component has been updated/re-rendered"
        Expect.equal 2 effectCount "Effect count has increased to two"
        RTL.userEvent.click(increment)
        Expect.equal "2" count.innerText "Component has been updated/re-rendered again"
        Expect.equal 3 effectCount "Effect count has increased three"

    testReact "Focusing input element works with React refs" <| fun _ -> 
        let render = RTL.render(focusInputExample())
        let focusedInput = render.getByTestId "focused-input"
        let focusInputButton = render.getByTestId "focus-input"
        Expect.isFalse (focusedInput = unbox document.activeElement) "Input is not focused yet before clicking button"
        RTL.userEvent.click(focusInputButton)
        Expect.isTrue (focusedInput = unbox document.activeElement) "Input is now active"

    testReact "Styles are rendered correctly" <| fun _ ->
        let render = RTL.render(Html.div [
            prop.style [
                style.color.red
                style.margin 20
                style.fontSize 22
                style.fontStyle.italic
            ]

            prop.testId "styled-div"
        ])

        let container = render.getByTestId "styled-div"
        Expect.equal "rgb(255, 0, 0)" (getStyle "color" container) "Text color is red"
        Expect.equal "20px" (getStyle "margin" container) "Margin is used correctly"
        Expect.equal "22px" (getStyle "fontSize" container) "Font size is used correctly"
        Expect.equal "italic" (getStyle "fontStyle" container) "Font style is used correctly"

    testReactAsync "useCallbackRef gets updated values without re-rendering" <| async {
        let render = RTL.render(callbackRef())
        let buttonField = render.getByTestId "Button"
        let parentField = render.getByTestId "Main"
        let mainField = render.getByTestId "callbackRef"
        let buttonRef = render.getByTestId "callbackRefButton"

        do! Async.Sleep 2000

        do! RTL.waitFor(fun () -> RTL.userEvent.click(buttonRef)) |> Async.AwaitPromise
        Expect.isTrue (buttonField.innerText = "1") "Child component has not re-rendered"
        Expect.isTrue (parentField.innerText <> "1") "Parent component has re-rendered"
        Expect.isTrue (mainField.innerText <> "1" && mainField.innerText <> "") "Count was updated by child component"
    }

    testReactAsync "forwardRef works correctly" <| async {
        let render = RTL.render(forwardRefParent())
        let button = render.getByTestId "focus-button"
        let input = render.getByTestId "focus-input"

        Expect.isFalse (input = unbox document.activeElement) "Input is not focused yet before clicking button"
        do! RTL.waitFor(fun () -> RTL.userEvent.click(button)) |> Async.AwaitPromise
        Expect.isTrue (input = unbox document.activeElement) "Input is now active"
    }

    testReactAsync "lazy and suspense works" <| async {
        let render = RTL.render(codeSplitting())
        let loader = render.getByTestId("loading")

        Expect.isTrue (loader.innerText = "Loading") "Loading element is displayed"
        Expect.isTrue (render.queryByTestId("async-load", true).IsNone) "Code-split element is not displayed"
        
        do! 
            RTL.waitForElementToBeRemoved((fun () -> render.queryByTestId("loading")), [
                waitForOption.timeout 5000
            ]) |> Async.AwaitPromise

        Expect.isTrue (render.queryByTestId("loading").IsNone) "Loading element is not displayed"

        do! 
            RTL.waitFor(fun () -> 
                Expect.isTrue (render.queryByTestId("async-load").IsSome) "Code-split element is displayed"
            ) |> Async.AwaitPromise
    }

    testReactAsync "useImperativeHandle works correctly" <| async {
        let render = RTL.render(forwardRefImperativeParent())
        let button = render.getByTestId "focus-button"
        let text = render.getByTestId "focus-text"

        Expect.isFalse (text.innerText <> "") "Div has empty text value"
        do! RTL.waitFor(fun () -> RTL.userEvent.click(button)) |> Async.AwaitPromise
        Expect.isTrue (text.innerText = "Howdy!") "Div has text value set"
    }

    testReact "funcComps work correctly" <| fun _ ->
        let render = RTL.render(funcCompTestDiff())
        let renderCount = render.getByTestId "funcCompTest"

        Expect.equal "2" renderCount.innerText "Renders twice"

    testReact "funcComps withKey work correctly" <| fun _ ->
        let render = RTL.render(funcCompTestWithKeyDiff())
        let renderCount = render.getByTestId "funcCompTestWithKey"

        Expect.equal "1" renderCount.innerText "Renders once due to static key"

    testReact "memoComps work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestDiff())
        let renderCount = render.getByTestId "memoCompTest"

        Expect.equal "2" renderCount.innerText "Renders twice"

    testReact "memoComps withKey work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestWithKeyDiff())
        let renderCount = render.getByTestId "memoCompTestWithKey"

        Expect.equal "1" renderCount.innerText "Renders once due to static key"

    testReact "memoComps areEqual work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestAreEqualDiff())
        let renderCount = render.getByTestId "memoCompTestAreEqual"

        Expect.equal "1" renderCount.innerText "Renders once due to areEqual eval"

    testReact "memoComps withKey areEqual work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestAreEqualWithKeyDiff())
        let renderCount = render.getByTestId "memoCompTestAreEqualWithKey"

        Expect.equal "3" renderCount.innerText "Renders three times due to areEqual and withKey evals"
]

[<EntryPoint>]
let main (args: string []) = Mocha.runTests (testSequenced felizTests)