# Stateless Components
The simplest types of React components are those that are stateless: they don't internal have state and don't have any side-effects.

### Defining React Components
To define a component, use `React.functionComponent` which for most components take a render function of type `'props -> ReactElement` as input where `'props` is a record type that defines the components' properties. The input type `'props` can also be an anonymous record.

React components built with `React.functionComponent` have to defined at a *module-level* (i.e. not inside another function).

```fsharp
open Feliz

type Greeting = { Name: string option }

// greeting : Greeting -> ReactElement
let greeting = React.functionComponent(fun (props: Greeting) ->
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" props.Name)
    ])
```
Components can also be given a name such that you can inspect them in the browser using React profiling tools, it is **highly** recommended you give React component these names.
```fsharp
let greeting = React.functionComponent("Greeting", fun (props: Greeting) ->
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" props.Name)
    ])
```

### Using React components

Once you have defined a component like we did with `greeting` above, you can use it as a function of type `'props -> ReactElement` and combine with other components like any other UI element:

```fs:simple-components
Html.div [
    prop.className "content"
    prop.children [
        greeting { Name = Some "John" }
        greeting { Name = None }
    ]
]
```

### Simplify Definition and Usage

When building components, it is common to define the component then writing a function that instantiates that component as follows
```fs
module App

open Feliz

type Greeting = { Name: string option }

let greeting' = React.functionComponent("Greeting", fun (props: Greeting) ->
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" props.Name)
    ])

let greeting name = greeting' { Name = name }

Html.div [
    prop.className "content"
    prop.children [
        greeting (Some "John")
        greeting None
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

let counter = React.functionComponent("Counter", fun () ->
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
    ])
```