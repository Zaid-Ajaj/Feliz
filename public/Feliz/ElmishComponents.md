# Feliz.ElmishComponents  [![Nuget](https://img.shields.io/nuget/v/Feliz.ElmishComponents.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.ElmishComponents)

Feliz can be used and integrated into your Elmish application inside the `render` functions just like any other React binding. However, the Elm(ish) componentization techniques are known to require quite some boilerplate code in the application, especially when it comes to data and event communication between parent and child components.

Feliz includes a library called `Feliz.ElmishComponents` that enables you to build lightweight Elmish components using `init`, `update` and `render` as standalone React components.

These React componments will each keep track of their internal state, and have their own dispatch loop instead of having the parent components manage their state explicitly. Data and event communication will then go through React via the props.

### Installation

```
dotnet add package Feliz.ElmishComponents
```

### Usage

The library includes a single function: `React.elmishComponent(name, init, update, render, [key])` which you can use to build your React components:

```fsharp
open Feliz
open Feliz.ElmishComponents
open Elmish

let init : State * Cmd<Msg> = ...
let update (msg: Msg) (state: State) : State * Cmd<Msg> = ...
let render (state: State) (dispatch: Msg -> unit) : ReactElement = ...

let application : ReactElement = React.elmishComponent("Application", init, update, render)

open Browser.Dom

ReactDOM.render(application, document.getElementById "feliz-app")
```

### Compare with "traditional Elmish"

To understand how this library simplifies Elmish composition, head to the [elmish-compostion](https://github.com/Zaid-Ajaj/elmish-composition) repository where a login example is implemented once using Elmish style composition (parent manages all children explicitly) and once again implemented using this library to greatly simplify the parent component (parent only manages its own state).

### Careful with Component names and Keys

The first argument of the function `React.elmishComponent` is the required name of that component which works effectively as a *key* of that component! This means if you built a component using a static name (such as `TodoItem`) to use it in a loop, you have to give it an explicit `key:
```fsharp
module Todo =
    let init todoItem = (* ... *)
    let update msg state = (* ... *)
    let render state dispatch = (* ... *)


let TodoItem (todoItem: Todo) =
    React.elmishComponent("TodoItem", Todo.init todo, Todo.update, Todo.render, todo.Id)

Html.div [
    for todo in state.Todos -> TodoItem todo
]
```
If you don't provide the key in the component definition, then you can wrap the component in a `React.keyedFragment`:
```fs
let TodoItem (todoItem: Todo) =
    React.elmishComponent("TodoItem", Todo.init todo, Todo.update, Todo.render)

Html.div [
    for todo in state.Todos ->
        React.keyedFragment(todo.Id, [
            TodoItem todo
        ])
]
```

### Full Live Example
Here follows a sample React component that uses this technique:

```fsharp:elmish-components-counter
module App

open Elmish
open Feliz
open Feliz.ElmishComponents

type State = { Count: int }

type Msg =
    | Increment
    | Decrement
    | IncrementIndirect
    | IncrementTwice
    | IncrementDelayed
    | IncrementTwiceDelayed

let init : State * Cmd<Msg> = { Count = 0 }, Cmd.none

let update (msg: Msg) (state: State) : State * Cmd<Msg> =
    match msg with
    | Increment ->
        { state with Count = state.Count + 1 }, Cmd.ofSub (fun dispatch -> printfn "Increment")

    | Decrement ->
        { state with Count = state.Count - 1 }, Cmd.ofSub (fun dispatch -> printfn "Decrement")

    | IncrementIndirect ->
        state, Cmd.ofMsg Increment

    | IncrementTwice ->
        state, Cmd.batch [ Cmd.ofMsg Increment; Cmd.ofMsg Increment ]

    | IncrementDelayed ->
        state, Cmd.OfAsync.perform (fun () ->
            async {
                do! Async.Sleep 1000;
                return IncrementIndirect
            }) () (fun msg -> msg)

    | IncrementTwiceDelayed ->
        state, Cmd.batch [ Cmd.ofMsg IncrementDelayed; Cmd.ofMsg IncrementDelayed ]

let render state dispatch =
    Html.div [
        Html.button [
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Increment"
        ]

        Html.button [
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Decrement"
        ]

        Html.button [
            prop.onClick (fun _ -> dispatch IncrementIndirect)
            prop.text "IncrementIndirect"
        ]

        Html.button [
            prop.onClick (fun _ -> dispatch IncrementDelayed)
            prop.text "IncrementDelayed"
        ]

        Html.button [
            prop.onClick (fun _ -> dispatch IncrementTwice)
            prop.text "Increment Twice"
        ]

        Html.button [
            prop.onClick (fun _ -> dispatch IncrementTwiceDelayed)
            prop.text "IncrementTwiceDelayed"
        ]

        Html.h1 state.Count
    ]

let application = React.elmishComponent("Counter", init, update, render)

open Browser.Dom

ReactDOM.render(application, document.getElementById "feliz-app")
```

### Disposable Resources

You can implement `System.IDisposable` on your `State` when using resources
that need to be cleaned up before your component unmounts.

You can see in this example if you load the component and then unload it
your web console will have the message "I was disposed!"

```fsharp:elmish-components-dispose
module App

open Elmish
open Feliz
open Feliz.ElmishComponents

type State =
    { Count: int }

    interface System.IDisposable with
        member _.Dispose () = JS.console.log("I was disposed!")

type Msg =
    | Increment
    | Decrement

let init : State * Cmd<Msg> = { Count = 0 }, Cmd.none

let update (msg: Msg) (state: State) : State * Cmd<Msg> =
    match msg with
    | Increment ->
        { state with Count = state.Count + 1 }, Cmd.ofSub (fun dispatch -> printfn "Increment")

    | Decrement ->
        { state with Count = state.Count - 1 }, Cmd.ofSub (fun dispatch -> printfn "Decrement")

let render state dispatch =
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

let application = React.elmishComponent("CounterWithDispose", init, update, render)

open Browser.Dom

ReactDOM.render(application, document.getElementById "feliz-app")
```
