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

type IRenderer =
    inherit IDisposable
    abstract Container : unit -> HTMLElement

let renderReact (element: ReactElement) =
    let id = Guid.NewGuid().ToString()
    let container = document.createElement("div")
    container.setAttribute("id", id)
    document.body.appendChild(container) |> ignore
    ReactDOM.render(element, container)
    { new IRenderer with
        member this.Container() = container
        member this.Dispose() = document.getElementById(id).remove() }

let testReact name test = 
    testCase name <| fun _ -> 
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        test()

let testReactAsync name test = 
    testCaseAsync name <| async {
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        let! _ = test
        return ()
    }

let ftestReact name test = 
    ftestCase name <| fun _ -> 
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        test()

let ftestReactAsync name test = 
    ftestCaseAsync name <| async {
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        let! _ = test
        return ()
    }

[<Emit("$1['style'][$0]")>]
let getStyle<'t> (key: string) (x: HTMLElement) = jsNative 

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
]

[<EntryPoint>]
let main (args: string []) = Mocha.runTests (testSequenced felizTests)