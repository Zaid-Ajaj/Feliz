# Subscriptions with Effects

In the previous section, we saw how `React.useEffect` can be used to issue a side-effect and control when and how that effect is re-executed. However, there are cases where you want to have some **cleanup** code after the component unmounts or the props has changed. For example when we subscribe to some event as the component mounts, we want to unsubscribe to that event when the component unmounts.

To use `React.useEffect` with a cleanup phase, we call one these function signatures:
 - `React.useEffect : (unit -> IDisposable) -> unit`
 - `React.useEffect : (unit -> IDisposable) * obj array -> unit`

Where the first parameter of type `unit -> IDisposable` is the effect that returns `IDisposable` to signal that this effect has some cleanup code which runs after the component unmounts:

```fsharp:effectful-timer
open Feliz
open Fable.Core.JS

[<ReactComponent>]
let EffectfulTimer() =
    let (paused, setPaused) = React.useState(false)
    // using useStateWithUpdater instead of useState
    // to avoid stale closures in React.useEffect
    let (value, setValue) = React.useStateWithUpdater(0)

    let subscribeToTimer() =
        // start the timer
        let subscriptionId = setInterval (fun _ -> if not paused then setValue (fun prev -> prev + 1)) 1000
        // return IDisposable with cleanup code that stops the timer
        { new IDisposable with member this.Dispose() = clearTimeout(subscriptionId) }

    React.useEffect(subscribeToTimer, [| box paused |])

    Html.div [
        Html.h1 value

        Html.button [
            prop.className [
                Bulma.Button
                Bulma.IsLarge
                if paused then Bulma.IsWarning else Bulma.IsPrimary
            ]

            prop.onClick (fun _ -> setPaused(not paused))
            prop.text (if paused then "Resume" else "Pause")
        ]
    ]
```
Here we are subscribing with the value `subscribeToTimer` of type `unit -> IDisposable`:
```fsharp
let subscribeToTimer() =
    // start the timer
    let subscriptionId = setInterval (fun _ -> if not paused then setValue (fun prev -> prev  + 1)) 1000
    // return IDisposable with cleanup code that stops the timer
    { new IDisposable with member this.Dispose() = clearTimeout(subscriptionId) }
```
We return an `IDisposable` using an object expression but if you like functions more, you can use `React.createDisposable` which does exactly the same:
```fsharp
let subscribeToTimer() =
    // start the ticking
    let subscriptionId = setTimeout (fun _ -> if not paused then setValue (fun prev -> prev  + 1)) 1000
    // return IDisposable with cleanup code that stops the timer
    React.createDisposable(fun _ -> clearTimeout subscriptionId)
```

## useCancellationToken Hook

A common scenario is executing a promise or async function based on some
input or user event.

When this component is unmounted, if you have a pending operation in-flight
this can cause errors. The way to fix this is to pass a `CancellationToken`
to your async call so it is cancelled when that token is disposed.

To make this easier you can use `React.useCancellationToken()` which will
create a React `IRefValue` that you can pass to your calls (or children
if you have a use case of not wanting to cancel an operation, but adjust logic
if the component was unmounted).

Here is an example of how you could use this:

```fsharp:effectful-usecancellationtoken
[<ReactComponent>]
let UseToken(failedCallback: unit -> unit) =
    let token = React.useCancellationToken()
    React.useEffect(fun () ->
        async {
            do! Async.Sleep 4000
            failedCallback()
        }
        |> fun a -> Async.StartImmediate(a,token.current)
    )

    Html.none

[<ReactComponent>]
let Result(text: string) = Html.div text

[<ReactComponent>]
let Main() =
    let renderChild,setRenderChild = React.useState true
    let resultText,setResultText = React.useState "Pending..."

    let setFailed = React.useCallbackRef(fun () -> setResultText "You didn't cancel me in time!")

    Html.div [
        if renderChild then UseToken(setFailed)
        Result(resultText)
        Html.button [
            prop.classes [ Bulma.Button; Bulma.HasBackgroundPrimary; Bulma.HasTextWhite ]
            prop.disabled(resultText = "Disposed")
            prop.text "Dispose"
            prop.onClick(fun _ ->
                async {
                    setResultText "Disposed"
                    setRenderChild false
                }
                |> Async.StartImmediate
            )
        ]

        Html.button [
            prop.classes [ Bulma.Button; Bulma.HasBackgroundPrimary; Bulma.HasTextWhite ]
            prop.disabled (renderChild && resultText = "Pending...")
            prop.text "Reset"
            prop.onClick(fun _ ->
                async {
                    setResultText "Pending..."
                    setRenderChild true
                }
                |> Async.StartImmediate
            )
        ]
    ]
```
