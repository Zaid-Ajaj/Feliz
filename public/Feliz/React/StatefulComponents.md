# Stateful Components

Components can tremendously useful when they manage their internal state. This allows for some pieces of the UI to have stateful bevavior without polluting the global Elmish state. For example when you have an accordion component, whether it is collapsed or expanded is a piece of information that might not be relevant to your Elmish models so you keep that information internal to that specific component. Otherwise, if you are "going Elmish all the way", you would to keep track of which accordion is now in it's open state and have messages that are resposible for expanding and collapsing a specific accordion.

State with React components is incorporated using [React Hooks](https://reactjs.org/docs/hooks-intro.html) which allow you to define state variables inside your functional components. A common hook to use with React is the `React.useState` hook.

> It is recommended that you use hooks only inside the defintion of your `React.functionComponent` such that you could distinguish which parts of your application are simple functions and which parts are (stateful) components.

```fsharp:stateful-counter
React.functionComponent(fun () ->
    let (count, setCount) = React.useState(0)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ])
```
The hook `React.useState` is one of the most useful hooks which allows you to define a *state variable*. The syntax of `React.useState` follows:
```fs
let (value, setValue) = React.useState (initialValue)
```
where
```fs
val initialValue : 'T
val value : 'T
val setValue : 'T -> unit
```
The `setValue` function is what is called a *setter function* which is why it is common to prefix the function with "set{*Something*}".

When you call that function, the `value` will be updated and the component re-renders using this newly updated `value`.

### Multiple State Variables

A single component can keep track of multiple state variables at the same time. Here is an example
```fsharp:multiple-state-variables
React.functionComponent(fun () ->
    let (count, setCount) = React.useState(0)
    let (textColor, setTextColor) = React.useState(color.red)

    Html.div [
        Html.h1 [
            prop.style [ style.color textColor ]
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]

        Html.button [
            prop.text "Red"
            prop.onClick (fun _ -> setTextColor(color.red))
        ]

        Html.button [
            prop.text "Blue"
            prop.onClick (fun _ -> setTextColor(color.blue))
        ]
    ])
```

### Lazy State

React will call the initialization on every rerender, even if it's no longer needed.
In most cases this has no performance impact at all, however, in some cases where the
initial value is very expensive this can cause an application to become unusable.

You can ensure that expensive initialization functions only run once via `React.useStateLazy`:

```fsharp:use-state-lazy
let useStateNormal = React.functionComponent(fun () ->
    let count,setCount = React.useState (sortNumbers())

    Html.div [
        prop.classes [ Bulma.Box ]
        prop.children [
            Html.div [
                prop.text (sprintf "Normal Count: %i" count)
            ]
        ]
    ])

let useStateLazy = React.functionComponent(fun () ->
    let count,setCount = React.useStateLazy (fun () -> sortNumbers())
 
    Html.div [
        prop.classes [ Bulma.Box ]
        prop.children [
            Html.div [
                prop.text (sprintf "Lazy Count: %i" count)
            ]
        ]
    ])
```
