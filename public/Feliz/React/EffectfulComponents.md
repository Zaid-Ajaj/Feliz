# Components with Effects

Aside from using states variables, React components can also execute side-effects. These are things like subscriptions, timers, network calls, modifying the DOM or even logging something to the console. React gives us a special hook to work with effects: the `React.useEffect` hook.

Executing side-effects from the `React.useEffect` hook is better than "just calling" your side-effect immediately in the render function definition because you can decide when and how the *effect* is re-executed and how to execute *cleanup* work after the component unmounts from the DOM. For example, loading external data using Http is a side-effect that you might want to execute on the initial render of the component but not every time the component re-renders.

There are different ways you can use `React.useEffect` and they are divivded into the following API sections.

### Effects executed on every render cycle

Here, we use the following signature of `React.useEffect`:
```fsharp
React.useEffect : (unit -> unit) -> unit
```
For example, let us have the same counter example and execute an effect that changes the title of the tab every time the count state changes:
```fsharp:effectful-tab-counter
[<ReactComponent>]
let TabCounter() =
    let (count, setCount) = React.useState(0)
    // execute this effect on every render cycle
    React.useEffect(fun () -> Browser.Dom.document.title <- sprintf "Count = %d" count)

    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ]
```

See how the title of the page changes as you click on the "Increment" button.

<div style="background-color:yellow">

> **Tip**

> If you’re familiar with React class lifecycle methods, you can think of useEffect Hook as componentDidMount, componentDidUpdate, and componentWillUnmount combined.

</div>

### Effects executed only once

Here, you can have effects that make a HTTP request, subscribe to a web socket or initiate a timer only after the component has mounted, but not again when one of the state variables changes the value like in the example above. You can also say that the effect **"doesn't depend"** on any of the state variables or input properties.

```fsharp:effectful-async
[<ReactComponent>]
let EffectWithAsyncOnce() =
    let (isLoading, setLoading) = React.useState(false)
    let (content, setContent) = React.useState("")

    let loadData() = async {
        setLoading true
        do! Async.Sleep 1500
        setLoading false
        setContent "Content"
    }

    React.useEffect(loadData >> Async.StartImmediate, [| |])

    Html.div [
        if isLoading
        then Html.h1 "Loading"
        else Html.h1 content
    ]
```
Notice the signature of `React.useEffect`:
```fsharp
React.useEffect : (unit -> unit) * obj array -> unit
```
Where we used `obj array` as an empty array. This array determintes the **dependencies** of the effect. When any of the depdencies change, the effect is re-executed. Giving an empty array means that the effect is never re-executed after the initial execution which makes it perfect for loading data once.

Likewise, you can use the short-hand `React.useEffectOnce` which calls `React.useEffect` with an empty dependencies array:
```fsharp:effectful-async-once
[<ReactComponent>]
let EffectWithAsyncOnce() =
    let (isLoading, setLoading) = React.useState(false)
    let (content, setContent) = React.useState("")

    let loadData() = async {
        setLoading true
        do! Async.Sleep 1500
        setLoading false
        setContent "Content"
    }

    React.useEffectOnce(loadData >> Async.StartImmediate)

    Html.div [
        if isLoading
        then Html.h1 "Loading"
        else Html.h1 content
    ]
```

### Effects re-executed when a state variable changes

Using an empty dependency array causes the effect to run only once but a lot of the times you want to re-execute when one of the state variables change (i.e. loading user information if the user ID has changed) but *not* every single one.

In the following sample, we have a function component that simulates loading user information based on the `userId` state variable and only reload when that state variable changes. There are other state variables that control the text color, when these change the effect is not re-executed because it doesn't depend on them.

```fsharp:effectful-user-id
let rnd = System.Random()

[<ReactComponent>]
let EffectsUsingDependencies() =
    let (isLoading, setLoading) = React.useState(false)
    let (content, setContent) = React.useState("")
    let (userId, setUserId) = React.useState(0)
    let (textColor, setTextColor) = React.useState(color.red)

    let loadData() = async {
        setLoading true
        do! Async.Sleep 1500
        setLoading false
        setContent (sprintf "User %d" userId)
    }

    React.useEffect(loadData >> Async.StartImmediate, [| box userId |])

    Html.div [
        Html.h1 [
            prop.style [ style.color textColor ]
            prop.text (if isLoading then "Loading" else content)
        ]

        Html.button [
            prop.text "Red"
            prop.onClick (fun _ -> setTextColor(color.red))
        ]

        Html.button [
            prop.text "Blue"
            prop.onClick (fun _ -> setTextColor(color.blue))
        ]

        Html.button [
            prop.text "Update User ID"
            prop.onClick (fun _ -> setUserId(rnd.Next(1, 100)))
        ]
    ]
```