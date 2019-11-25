# Subscriptions with Effects

In the previous section, we saw how `React.useEffect` can be used to issue a side-effect and control when and how that effect is re-executed. However, there are cases where you want to have some **cleanup** code after the component unmounts or the props has changed. For example when we subscribe to some event as the component mounts, we want to unsubscribe to that event when the component unmounts.

To use `React.useEffect` with a cleanup phase, we call one these function signatures:
 - `React.useEffect : (unit -> IDisposable) -> unit`
 - `React.useEffect : (unit -> IDisposable) * obj array -> unit`

Where the first parameter of type `unit -> IDisposable` is the effect that returns `IDisposable` to signal that this effect has some cleanup code which runs after the component unmounts:

```fsharp:effectful-timer
React.functionComponent(fun () ->
    let (paused, setPaused) = React.useState(false)
    let (value, setValue) = React.useState(0)

    let subscribeToTimer() =
        // start the ticking
        let subscriptionId = setTimeout (fun _ -> if not paused then setValue (value + 1)) 1000
        // return IDisposable with cleanup code
        { new IDisposable with member this.Dispose() = clearTimeout(subscriptionId) }

    React.useEffect(subscribeToTimer)

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
    ])
```
Here we are subscribing with the value `subscribeToTimer` of type `unit -> IDisposable`:
```fsharp
let subscribeToTimer() =
    // start the ticking
    let subscriptionId = setTimeout (fun _ -> if not paused then setValue (value + 1)) 1000
    // return IDisposable with cleanup code
    { new IDisposable with member this.Dispose() = clearTimeout(subscriptionId) }
```
We return an `IDisposable` using an object expression but if you like functions more, you can use `React.createDisposable` which does exactly the same:
```fsharp
let subscribeToTimer() =
    // start the ticking
    let subscriptionId = setTimeout (fun _ -> if not paused then setValue (value + 1)) 1000
    // return IDisposable with cleanup code
    React.createDisposable(fun _ -> clearTimeout subscriptionId)
```