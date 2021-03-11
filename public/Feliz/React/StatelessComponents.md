# Stateless Components
The simplest types of React components are those that are stateless: they don't internal have state and don't have any side-effects.

### Defining React Components
To define a component, use `React.functionComponent` which for most components take a render function of type `'props -> ReactElement` as input where `'props` is a record type that defines the components' properties. The input type `'props` can also be an anonymous record.

React components built with `React.functionComponent` have to defined at a *module-level* (i.e. not inside another function).

```fsharp
open Feliz

type GreetingProps = { Name: string option }

// greeting : GreetingProps -> ReactElement
[<ReactComponent>]
let Greeting(props: GreetingProps) =
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" props.Name)
    ]
```

### Using React components

Once you have defined a component like we did with `greeting` above, you can use it as a function of type `'props -> ReactElement` and combine with other components like any other UI element:

```fs:simple-components
Html.div [
    prop.className "content"
    prop.children [
        Greeting { Name = Some "John" }
        Greeting { Name = None }
    ]
]
```


### Model-View-Update components

You can use the built-in `React.useReducer` hook to build model-view-update style components inside your application:
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