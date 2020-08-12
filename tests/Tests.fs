module Tests

open Fable.Mocha
open Feliz
open Browser
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

    React.useEffect(fun () ->
        async {
            do! Async.Sleep 25
            if count < 10 then setCount (count + 1)
        } |> Async.StartImmediate
    )

    Html.div [
        memoCompTestAreEqualWithKey {| count = count |}
    ])

let selectMultipleWithDefault = React.functionComponent(fun (input: {| isDefaultValue: bool; isValue: bool; isValueOrDefault: bool |}) ->
    React.fragment [
        Html.select [
            prop.multiple true
            prop.testId "select-multiple"
            if input.isDefaultValue then
                prop.defaultValue [3]
            elif input.isValue then
                prop.value [3]
            elif input.isValueOrDefault then
                prop.valueOrDefault [3]

            prop.children [
                Html.option [
                    prop.testId "val1"
                    prop.value 1
                    prop.text "A"
                ]
                Html.option [
                    prop.testId "val2"
                    prop.value 2
                    prop.text "B"
                ]
                Html.option [
                    prop.testId "val3"
                    prop.value 3
                    prop.text "C"
                ]
            ]
        ]
    ])

let textfComp = React.functionComponent(fun (input: {| str: string; i: int |}) ->
    Html.div [
        prop.children [
            Html.div [
                prop.testId "textf-str"
                prop.textf "Hello! %s" input.str
            ]
            Html.div [
                prop.testId "textf-int"
                prop.textf "Hello! %i" input.i
            ]
            Html.div [
                prop.testId "textf-two"
                prop.textf "Hello! %s %i" input.str input.i
            ]
            Html.div [
                prop.testId "textf-three"
                prop.textf "Hello! %s %i %s" input.str input.i (input.str + (string input.i))
            ]
        ]
    ])

let htmlTextfComp = React.functionComponent(fun (input: {| str: string; i: int |}) ->
    Html.div [
        prop.children [
            Html.div [
                prop.testId "textf-str"
                prop.children [
                    Html.textf "Hello! %s" input.str
                ]
            ]
            Html.div [
                prop.testId "textf-int"
                prop.children [
                    Html.textf "Hello! %i" input.i
                ]
            ]
            Html.div [
                prop.testId "textf-two"
                prop.children [
                    Html.textf "Hello! %s %i" input.str input.i
                ]
            ]
            Html.div [
                prop.testId "textf-three"
                prop.children [
                    Html.textf "Hello! %s %i %s" input.str input.i (input.str + (string input.i))
                ]
            ]
        ]
    ])

module TokenCancellation =
    let useToken = React.functionComponent(fun (input: {| failedCallback: unit -> unit |}) ->
        let token = React.useCancellationToken()

        React.useEffectOnce(fun () ->
            async {
                do! Async.Sleep 500
                input.failedCallback()
            }
            |> fun a -> Async.StartImmediate(a,token.current)
        )

        Html.none)

    let resultComp = React.functionComponent(fun (input: {| result: string |}) ->
        Html.div [
            prop.testId "useTokenCancellation"
            prop.text input.result
        ])

    let main = React.functionComponent(fun (input: {| failTest: bool |}) ->
        let renderChild,setRenderChild = React.useState true
        let resultText,setResultText = React.useState ""

        let setFailed = React.useCallbackRef <| fun () -> setResultText "Failed"

        React.useLayoutEffectOnce(fun () ->
            async {
                do! Async.Sleep 100

                if not input.failTest then
                    setResultText "Disposed"
                    setRenderChild false
            }
            |> Async.StartImmediate
        )

        Html.div [
            if renderChild then
                useToken {| failedCallback = setFailed |}
            resultComp {| result = resultText |}
        ])

module OptionalDispose =
    let optionalDispose = React.functionComponent(fun (input: {| onDispose: System.IDisposable option |}) ->
        React.useEffectOnce(fun () -> input.onDispose)

        Html.div [
            prop.testId "dispose-inner"
        ])

    let render = React.functionComponent(fun (input: {| isSome: bool |}) ->
        let wasDisposed,setWasDisposed = React.useState false
        let disposeElement,setDisposeElement = React.useState false

        let setWasDisposed = React.useCallbackRef(setWasDisposed)

        Html.div [
            Html.div [
                prop.testId "was-disposed"
                prop.textf "%b" wasDisposed
            ]
            if not disposeElement then
                if input.isSome then
                    optionalDispose {| onDispose = Some { new System.IDisposable with member _.Dispose () = setWasDisposed true } |}
                else optionalDispose {| onDispose = None |}
            Html.button [
                prop.testId "dispose-button"
                prop.onClick <| fun _ -> setDisposeElement (not disposeElement)
            ]
        ])

module RefDispose =
    let refDispose = React.functionComponent(fun (input: {| onDispose: System.IDisposable |}) ->
        React.useEffectOnce(fun () -> input.onDispose)

        Html.div [
            prop.testId "dispose-inner"
        ])

    let render = React.functionComponent(fun () ->
        let disposedCount,setDisposedCount = React.useState 0
        let disposedCountRef = React.useRef 0
        let disposeElement,setDisposeElement = React.useState false

        let setDisposedCount =
            React.useCallbackRef(fun () ->
                disposedCountRef.current <- disposedCountRef.current + 1
                setDisposedCount(disposedCountRef.current)
            )

        let disposalOne = React.useRef(Some { new System.IDisposable with member _.Dispose () = setDisposedCount() })
        let disposalTwo = React.useRef(Some { new System.IDisposable with member _.Dispose () = setDisposedCount() })

        let multipleDispose = React.createDisposable(disposalOne, disposalTwo)

        Html.div [
            Html.div [
                prop.testId "disposed-count"
                prop.textf "%i" disposedCount
            ]
            if not disposeElement then
                refDispose {| onDispose = multipleDispose |}
            Html.button [
                prop.testId "dispose-button"
                prop.onClick <| fun _ -> setDisposeElement (not disposeElement)
            ]
        ])

module UseElmish =
    open Elmish
    open Feliz.UseElmish

    type State =
        { Count: int }

        interface System.IDisposable with
            member _.Dispose () = ()

    type Msg =
        | Increment

    let init = 0, Cmd.none

    let update msg state =
        match msg with
        | Increment -> state + 1, Cmd.none

    let render = React.functionComponent(fun () ->
        let state,dispatch = React.useElmish(init, update, [||])

        Html.div [
            Html.h1 [
                prop.testId "count"
                prop.text state
            ]

            Html.button [
                prop.text "Increment"
                prop.onClick (fun _ -> dispatch Increment)
                prop.testId "increment"
            ]
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

    testReact "Individual translate(int) functions render correctly" <| fun _ ->
        let render = RTL.render(Html.div [
            Html.div [
                prop.testId "test-translateX"
                prop.style [ style.transform.translateX 1 ]
            ]
            Html.div [
                prop.testId "test-translateY"
                prop.style [ style.transform.translateY 2 ]
            ]
            Html.div [
                prop.testId "test-translateZ"
                prop.style [ style.transform.translateZ 3 ]
            ]
            Html.div [
                prop.testId "test-translate"
                prop.style [ style.transform.translate(11, 22) ]
            ]
            Html.div [
                prop.testId "test-translate3D"
                prop.style [ style.transform.translate3D(111, 222, 333) ]
            ]
        ])

        let getTransform (id:string) = getStyle<string> "transform" (render.getByTestId id)
        Expect.equal (getTransform "test-translateX") "translateX(1px)" "translateX should render"
        Expect.equal (getTransform "test-translateY") "translateY(2px)" "translateY should render"
        Expect.equal (getTransform "test-translateZ") "translateZ(3px)" "translateZ should render"
        Expect.equal (getTransform "test-translate") "translate(11px, 22px)" "translate should render"
        Expect.equal (getTransform "test-translate3D") "translate3d(111px, 222px, 333px)" "translate3D should render"

    testReact "Individual translate(ICssUnit) functions render correctly" <| fun _ ->
        let render = RTL.render(Html.div [
            Html.div [
                prop.testId "test-translateX"
                prop.style [ style.transform.translateX (length.em 1) ]
            ]
            Html.div [
                prop.testId "test-translateY"
                prop.style [ style.transform.translateY (length.em 2) ]
            ]
            Html.div [
                prop.testId "test-translateZ"
                prop.style [ style.transform.translateZ (length.em 3) ]
            ]
            Html.div [
                prop.testId "test-translate"
                prop.style [ style.transform.translate(length.em 11, length.em 22) ]
            ]
            Html.div [
                prop.testId "test-translate3D"
                prop.style [ style.transform.translate3D(length.em 111, length.em 222, length.em 333) ]
            ]
        ])

        let getTransform (id:string) = getStyle<string> "transform" (render.getByTestId id)
        Expect.equal (getTransform "test-translateX") "translateX(1em)" "translateX should render"
        Expect.equal (getTransform "test-translateY") "translateY(2em)" "translateY should render"
        Expect.equal (getTransform "test-translateZ") "translateZ(3em)" "translateZ should render"
        Expect.equal (getTransform "test-translate") "translate(11em, 22em)" "translate should render"
        Expect.equal (getTransform "test-translate3D") "translate3d(111em, 222em, 333em)" "translate3D should render"

    testReact "Combined translate(int) functions render correctly" <| fun _ ->
        let render = RTL.render(Html.g [
            prop.testId "test-combined-translate"
            prop.transform [
                    transform.translateX 1
                    transform.translateY 2
                    transform.translateZ 3
                    transform.translate(11, 22)
                    transform.translate3D(111, 222, 333)
            ]
        ])
        let renderedTransform = (render.getByTestId "test-combined-translate").getAttribute "transform"
        Expect.isTrue (renderedTransform.Contains "translateX(1)") "translateX should render"
        Expect.isTrue (renderedTransform.Contains "translateY(2)") "translateY should render"
        Expect.isTrue (renderedTransform.Contains "translateZ(3)") "translateZ should render"
        Expect.isTrue (renderedTransform.Contains "translate(11, 22)") "translate should render"
        Expect.isTrue (renderedTransform.Contains "translate3d(111, 222, 333)") "translate3D should render"

    testReact "Combined translate(float) functions render correctly" <| fun _ ->
        let render = RTL.render(Html.g [
            prop.testId "test-combined-translate"
            prop.transform [
                    transform.translateX 1.1
                    transform.translateY 2.2
                    transform.translateZ 3.3
                    transform.translate(11.1, 22.2)
                    transform.translate3D(111.1, 222.2, 333.3)
            ]
        ])

        let renderedTransform = (render.getByTestId "test-combined-translate").getAttribute "transform"
        Expect.isTrue (renderedTransform.Contains "translateX(1.1)") "translateX should render"
        Expect.isTrue (renderedTransform.Contains "translateY(2.2)") "translateY should render"
        Expect.isTrue (renderedTransform.Contains "translateZ(3.3)") "translateZ should render"
        Expect.isTrue (renderedTransform.Contains "translate(11.1, 22.2)") "translate should render"
        Expect.isTrue (renderedTransform.Contains "translate3d(111.1, 222.2, 333.3)") "translate3D should render"

    testReact "Combined translate(ICssUnit) functions render correctly" <| fun _ ->
        let render = RTL.render(Html.div [
            prop.testId "test-combined-translate"
            prop.style [
                style.transform [
                    transform.translateX (length.em 1)
                    transform.translateY (length.em 2)
                    transform.translateZ (length.em 3)
                    transform.translate(length.em 11, length.em 22)
                    transform.translate3D(length.em 111, length.em 222, length.em 333)
                ]
            ]
        ])

        let renderedTransform = getStyle<string> "transform" (render.getByTestId "test-combined-translate")
        Expect.isTrue (renderedTransform.Contains "translateX(1em)") "translateX should render"
        Expect.isTrue (renderedTransform.Contains "translateY(2em)") "translateY should render"
        Expect.isTrue (renderedTransform.Contains "translateZ(3em)") "translateZ should render"
        Expect.isTrue (renderedTransform.Contains "translate(11em, 22em)") "translate should render"
        Expect.isTrue (renderedTransform.Contains "translate3d(111em, 222em, 333em)") "translate3D should render"

    testReactAsync "useCallbackRef gets updated values without re-rendering" <| async {
        let render = RTL.render(callbackRef())
        let buttonField = render.getByTestId "Button"
        let parentField = render.getByTestId "Main"
        let mainField = render.getByTestId "callbackRef"
        let buttonRef = render.getByTestId "callbackRefButton"

        do! Async.Sleep 1000

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

        Expect.equal renderCount.innerText "2" "Renders twice"

    testReact "funcComps withKey work correctly" <| fun _ ->
        let render = RTL.render(funcCompTestWithKeyDiff())
        let renderCount = render.getByTestId "funcCompTestWithKey"

        Expect.equal renderCount.innerText "1" "Renders once due to static key"

    testReact "memoComps work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestDiff())
        let renderCount = render.getByTestId "memoCompTest"

        Expect.equal renderCount.innerText "2" "Renders twice"

    testReact "memoComps withKey work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestWithKeyDiff())
        let renderCount = render.getByTestId "memoCompTestWithKey"

        Expect.equal renderCount.innerText "1" "Renders once due to static key"

    testReact "memoComps areEqual work correctly" <| fun _ ->
        let render = RTL.render(memoCompTestAreEqualDiff())
        let renderCount = render.getByTestId "memoCompTestAreEqual"

        Expect.equal renderCount.innerText "1" "Renders once due to areEqual eval"

    testReactAsync "memoComps withKey areEqual work correctly" <| async {
        let render = RTL.render(memoCompTestAreEqualWithKeyDiff())
        let renderCount = render.getByTestId "memoCompTestAreEqualWithKey"

        do! Async.Sleep 100

        do!
            RTL.waitFor <| fun () ->
                Expect.equal renderCount.innerText "3" "Renders three times due to areEqual and withKey evals"
            |> Async.AwaitPromise
    }

    testReact "can handle select multiple with defaultValue" <| fun _ ->
        let render = RTL.render(selectMultipleWithDefault({| isDefaultValue = true; isValue = false; isValueOrDefault = false |}))
        let select = render.getByTestId("select-multiple")

        Expect.isTrue (RTL.screen.getByTestId("val3") |> unbox<Browser.Types.HTMLOptionElement>).selected "val3 is set via default value"

        select.userEvent.toggleSelectOptions(["1";"2"])

        Expect.isTrue (RTL.screen.getByTestId("val1") |> unbox<Browser.Types.HTMLOptionElement>).selected "Correctly sets val1 option"
        Expect.isTrue (RTL.screen.getByTestId("val2") |> unbox<Browser.Types.HTMLOptionElement>).selected "Correctly sets val2 option"
        Expect.isTrue (RTL.screen.getByTestId("val3") |> unbox<Browser.Types.HTMLOptionElement>).selected "Does not set val3 option"

    testReact "can handle select multiple with value" <| fun _ ->
        let render = RTL.render(selectMultipleWithDefault({| isDefaultValue = false; isValue = true; isValueOrDefault = false |}))
        let select = render.getByTestId("select-multiple")

        Expect.isTrue (RTL.screen.getByTestId("val3") |> unbox<Browser.Types.HTMLOptionElement>).selected "val3 is set via default value"

        select.userEvent.toggleSelectOptions(["1";"2"])

        Expect.isFalse (RTL.screen.getByTestId("val1") |> unbox<Browser.Types.HTMLOptionElement>).selected "Setting val1 option is ignored"
        Expect.isFalse (RTL.screen.getByTestId("val2") |> unbox<Browser.Types.HTMLOptionElement>).selected "Setting val2 option is ignored"
        Expect.isTrue (RTL.screen.getByTestId("val3") |> unbox<Browser.Types.HTMLOptionElement>).selected "Does not set val3 option"

    testReact "can handle select multiple with valueOrDefault" <| fun _ ->
        let render = RTL.render(selectMultipleWithDefault({| isDefaultValue = false; isValue = false; isValueOrDefault = true |}))
        let select = render.getByTestId("select-multiple")

        Expect.isTrue (RTL.screen.getByTestId("val3") |> unbox<Browser.Types.HTMLOptionElement>).selected "val3 is set via default value"

        select.userEvent.toggleSelectOptions(["1";"2"])

        Expect.isTrue (RTL.screen.getByTestId("val1") |> unbox<Browser.Types.HTMLOptionElement>).selected "Correctly sets val1 option"
        Expect.isTrue (RTL.screen.getByTestId("val2") |> unbox<Browser.Types.HTMLOptionElement>).selected "Correctly sets val2 option"
        Expect.isTrue (RTL.screen.getByTestId("val3") |> unbox<Browser.Types.HTMLOptionElement>).selected "Does not set val3 option"

    testReact "can use string format as prop" <| fun _ ->
        let input = {| str = "hello"; i = 1 |}
        let render = RTL.render(textfComp input)

        Expect.isTrue (render.getByTestId("textf-str").innerText = (sprintf "Hello! %s" input.str)) "Correctly accepts single string parameter"
        Expect.isTrue (render.getByTestId("textf-int").innerText = (sprintf "Hello! %i" input.i)) "Correctly accepts single int parameter"
        Expect.isTrue (render.getByTestId("textf-two").innerText = (sprintf "Hello! %s %i" input.str input.i)) "Correctly accepts two parameters"
        Expect.isTrue (render.getByTestId("textf-three").innerText = (sprintf "Hello! %s %i %s" input.str input.i (input.str + (string input.i)))) "Correctly accepts three parameters"

    testReact "can use string format directly from Html" <| fun _ ->
        let input = {| str = "hello"; i = 1 |}
        let render = RTL.render(htmlTextfComp input)

        Expect.isTrue (render.getByTestId("textf-str").innerText = (sprintf "Hello! %s" input.str)) "Correctly accepts single string parameter"
        Expect.isTrue (render.getByTestId("textf-int").innerText = (sprintf "Hello! %i" input.i)) "Correctly accepts single int parameter"
        Expect.isTrue (render.getByTestId("textf-two").innerText = (sprintf "Hello! %s %i" input.str input.i)) "Correctly accepts two parameters"
        Expect.isTrue (render.getByTestId("textf-three").innerText = (sprintf "Hello! %s %i %s" input.str input.i (input.str + (string input.i)))) "Correctly accepts three parameters"

    testReactAsync "useCancellationToken works correctly" <| async {
        let render = RTL.render(TokenCancellation.main({| failTest = false |}))

        do! Async.Sleep 600

        do!
            RTL.waitFor <| fun () ->
                Expect.equal (render.getByTestId "useTokenCancellation").innerText "Disposed" "Cancels all subsequent re-renders and calls the disposal function"
            |> Async.AwaitPromise
    }

    testReactAsync "useCancellationToken works correctly - failure case" <| async {
        let render = RTL.render(TokenCancellation.main({| failTest = true |}))

        do! Async.Sleep 600

        do!
            RTL.waitFor <| fun () ->
                Expect.equal (render.getByTestId "useTokenCancellation").innerText "Failed" "Cancels all subsequent re-renders and calls the disposal function"
            |> Async.AwaitPromise
    }

    testReactAsync "Can dispose of optional IDisposables" <| async {
        let render = RTL.render(OptionalDispose.render {| isSome = true |})

        Expect.equal (render.getByTestId("was-disposed").innerText) "false" "Should not be disposed yet"

        render.getByTestId("dispose-button").click()

        do!
            RTL.waitFor <| fun () ->
                Expect.equal (render.getByTestId("was-disposed").innerText) "true" "Should have been disposed"
            |> Async.AwaitPromise
    }

    testReactAsync "Does not dispose of optional IDisposables that are None" <| async {
        let render = RTL.render(OptionalDispose.render {| isSome = false |})

        Expect.equal (render.getByTestId("was-disposed").innerText) "false" "Should not be disposed yet"

        render.getByTestId("dispose-button").click()

        let innerElem = render.queryByTestId("dispose-inner")

        if innerElem.IsSome then
            do!
                RTL.waitForElementToBeRemoved(fun () -> render.queryByTestId("dispose-inner"))
                |> Async.AwaitPromise

        Expect.equal (render.getByTestId("was-disposed").innerText) "false" "Should not have been disposed"
    }

    testReactAsync "Can dispose of multiple IDisposable option refs" <| async {
        let render = RTL.render(RefDispose.render())

        Expect.equal (render.getByTestId("disposed-count").innerText) "0" "Should not be disposed yet"

        render.getByTestId("dispose-button").click()

        do!
            RTL.waitFor <| fun () ->
                Expect.equal (render.getByTestId("disposed-count").innerText) "2" "Should have been disposed"
            |> Async.AwaitPromise
    }

    testReactAsync "useElmish works" <| async {
        let render = RTL.render(UseElmish.render())

        Expect.equal (render.getByTestId("count").innerText) "0" "Should be initial state"

        render.getByTestId("increment").click()

        do!
            RTL.waitFor <| fun () ->
                Expect.equal (render.getByTestId("count").innerText) "1" "Should have been incremented"
            |> Async.AwaitPromise
    }
]

[<EntryPoint>]
let main (args: string []) =
    let allTests = testList "All Tests" [
        felizTests
        PropHelperTests.propHelpersTests
        DelayTests.delayTests
    ]

    Mocha.runTests (testSequenced allTests)
