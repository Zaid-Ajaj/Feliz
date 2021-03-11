# Stateless Components
The simplest types of React components are those that are stateless: they don't internal have state and don't have any side-effects.

### Defining React Components
To define a component, use the `[<ReactComponent>]` attribute on top of a function that returns `ReactElement`. React components built with have to defined at a *module-level* (i.e. not inside another function).

```fsharp
open Feliz

// greeting : (name: string option) -> ReactElement
[<ReactComponent>]
let Greeting(name: string option) =
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" name)
    ]
```

### Using React components

Once you have defined a component like we did with `greeting` above, you can use it as a function of type `'props -> ReactElement` and combine with other components like any other UI element:

```fs:simple-components
Html.div [
    prop.className "content"
    prop.children [
        Greeting None
        Greeting (Some "John")
    ]
]
```

### Components As Atatic Functions

React components can also be defined as static members of a static class. The benefit of this approach is that you use _named_ parameters at call-site or implicitly optional parameters in the definition:
```fs
open Feliz

type Components() =
    [<ReactComponent>]
    static member Greeting(name: string, age: int) =
        Html.div [
            Html.span $"Hello, {name}! You are {age} years old"
        ]

Html.div [
    prop.className "content"
    prop.children [
        // call-site is more readable because of the named parameters
        Components.Greeting(name="Jane", age=20)
        Components.Greeting(name="John", age=25)
    ]
]
```

### Model-View-Update components

You can use the built-in `React.useReducer` hook to build simple model-view-update style components inside your application:
```fsharp:feliz-elmish-counter
type State = { Count : int }

type Msg = Increment | Decrement

let initialState = { Count = 0 }

let update (state: State) = function
    | Increment -> { state with Count = state.Count + 1 }
    | Decrement -> { state with Count = state.Count - 1 }

[<ReactComponent>]
let Counter() =
    let (state, dispatch) = React.useReducer(update, initialState)
    Html.div [
        Html.button [
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Increment"
        ]

        Html.button [
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Decrement"
        ]

        Html.h1 state.Count
    ]
```