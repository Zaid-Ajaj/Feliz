# Feliz.ElmishComponents  [![Nuget](https://img.shields.io/nuget/v/Feliz.ElmishComponents.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.ElmishComponents)

Feliz can be used and integrated into your Elmish application inside the `render` functions just like any other React binding. However, the Elm(ish) componentization techniques are known to require a lot of boilerplate code in the application, especially when it comes to data and event communication between parent and child components. Luckily, Feliz includes a gem called `Feliz.ElmishComponents` that enables you to build Elmish components using `init`, `update` and `render` as standalone React components. These React componments will each keep track of their internal state, and have their own dispatch loop. Data and event communication will then go through React via the props.

### Installation

```
dotnet add package Feliz.ElmishComponents
```

### Usage

The library includes a single function: `React.elmishComponent(init, update, render)` which you can use to build your React components:

```fsharp
open Feliz
open Feliz.ElmishComponents
open Elmish

let init : State * Cmd<Msg> = ...
let update (msg: Msg) (state: State) : State * Cmd<Msg> = ...
let render (state: State) (dispatch: Msg -> unit) : ReactElement = ...

let application : ReactElement = React.elmishComponent(init, update, render)

open Browser.Dom

ReactDOM.render(application, document.getElementById "feliz-app")
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

let application = React.elmishComponent(init, update, render)

open Browser.Dom

ReactDOM.render(application, document.getElementById "feliz-app")
```
