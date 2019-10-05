# React Components

Feliz provides an API that allows to easily build React components. Components are just like simple elements except that they can have internal state and can execute custom side effects. They are tremendously helpful for state re-use without polluting the global Elmish application state.

### Stateless components
The simplest types of React components are those that don't internal state, only their input properties (aka `props`), for example:
```fsharp:simple-components
type Greeting = { Name: string option }

// greeting : Greeting -> ReactElement
let greeting = React.functionComponent <| fun (props: Greeting) ->
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" props.Name)
    ]

Html.div [
    prop.className "content"
    prop.children [
        greeting { Name = Some "John" }
        greeting { Name = None }
    ]
]
```
Components can also be given a name such that you can inspect them in the browser using React profiling tools:
```fsharp
let greeting = React.functionComponent("Greeting", fun (props: Greeting) ->
    Html.div [
        Html.span "Hello, "
        Html.span (Option.defaultValue "World" props.Name)
    ]
)
```

### Stateful Components with Hooks

Components are most useful when they internal state. State with React components is incorporated using [React Hooks](https://reactjs.org/docs/hooks-intro.html) which allow you to define stateful functional components. A common hook to use with React is the `useState` hook:

```fsharp:stateful-counter
React.functionComponent(fun () ->
    let (count, setCount) = React.useState(0)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ]
)
```

The snippet above is a one-to-one translation of the Javascript equivalent in modern React applications:
```js
import React, { useState } from 'react';

function Counter() {
  // Declare a new state variable, which we'll call "count"
  const [count, setCount] = useState(0);

  return (
    <div>
      <h1>{count}</h1>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}
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
    ]
)
```