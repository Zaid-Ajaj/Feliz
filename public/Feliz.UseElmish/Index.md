# Feliz.UseElmish [![Nuget](https://img.shields.io/nuget/v/Feliz.UseElmish.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.UseElmish)

Besides being able to use Feliz in existing Elmish applications, you can also use Elmish as _part_ of your Feliz application. This is a different approach to building standalone React components that use Elmish internally to manage the state of the component but from the perspective of the consumer, it is just another React component.

This approach simplifies the original Elmish model where the application state is explicitly passed down in parts to children and events are passed up to the parent components.

The implementation of this approach is made possible using a React hook called `React.useElmish`. The following examples demonstrate how to use it:

### Install into your project
```bash
dotnet add package Feliz.UseElmish
```

Here is an example to demonstrate how to build such component:
```fsharp:use-elmish-basic
open Feliz
open Feliz.UseElmish
open Elmish

type Msg =
    | Increment
    | Decrement

type State = { Count : int }

let init() = { Count = 0 }, Cmd.none

let update msg state =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none

[<ReactComponent>]
let Counter() =
    let state, dispatch = React.useElmish(init, update, [| |])
    Html.div [
        Html.h1 state.Count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> dispatch Increment)
        ]

        Html.button [
            prop.text "Decrement"
            prop.onClick (fun _ -> dispatch Decrement)
        ]
    ]

open Browser.Dom

ReactDOM.render(Counter(), document.getElementById "feliz-app")
```
The difference here from a full-fledged Elmish applications is that there isn't an "Elmish entry point" to run the component and manage its life-cycle. Instead, the `React.useElmish` hooks manages the Elmish life-cycle internally within the React component so that it can run standalone inside other React components:
```fs
[<ReactComponent>]
let Counters() =
    Html.div [
        Counter()
        Counter()
        Counter()
    ]
```
When you need to trigger events from such an Elmish component, use React patterns where you pass a callback via the props instead of passing the `dispatch` function from the parent component.

### Understading the dependencies array
It is also important to understand the _dependencies array_ of the `React.useElmish` function
```fs
//                                             dependencies array
//                                                    |
//                                                    |
//                                                    ↓
let state, dispatch = React.useElmish(init, update, [| |])
```
This array is responsible for the _re-initialization_ of the component. For example, if your mini Elmish component loads user profile based on an input user ID like this:
```fs
[<ReactComponent>]
let UserProfile(userId: int) =
    // will initialize once even if the component is re-rendered using a different userId
    let state, dispatch = React.useElmish(init userId, update, [| |])
    renderUserProfile state disptch
```
Then you must add the `userId` to the dependencies array so that the hook knows to call `init` again and re-initialize the component:
```fs
[<ReactComponent>]
let UserProfile(userId: int) =
    //                                               inititialization dependency
    //                                                              |
    //                                                              |
    //                                                              |
    // now every time this component is rendered using              |
    // a different userId, it will reinitialize the component       ↓
    let state, dispatch = React.useElmish(init userId, update, [| box userId |])
    renderUserProfile state disptch

// Here, we are using a router so that every time the URL changes
// say from /user/20 to /user/21 then the UserProfile will be reload that user
open Feliz.Router

[<ReactComponent>]
let App() =
    let currentUrl, updateCurrentUrl = React.useState(Router.currentUrl())
    React.router [
        router.onUrlChanged updateCurrentUrl
        router.children [
            match currentUrl with
            | [ "user"; Route.Int userId ] -> UserProfile(userId)
            | _ -> Html.h1 "Not found"
        ]
    ]

open Browser.Dom

ReactDOM.render(App(), document.getElementById "feliz-app")
```

> The dependencies array follows the same rules of `React.useEffect`

### Combining with other hooks
Next, let's combine this hook with other React hooks such as `React.useState` and `React.useEffect`:
```fsharp:use-elmish-combined
[<ReactComponent>]
let Counter() =
    let localCount, setLocalCount = React.useState(0)
    let state, dispatch = React.useElmish(init, update, [| |])

    let subscribeToTimer() =
        // start the ticking
        let subscriptionId = setTimeout (fun _ -> dispatch Increment) 1000
        // return IDisposable with cleanup code
        { new IDisposable with member this.Dispose() = clearTimeout(subscriptionId) }

    React.useEffect(subscribeToTimer)

    Html.div [
        Html.h1 (state.Count + localCount)
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> dispatch Increment)
        ]

        Html.button [
            prop.text "Decrement"
            prop.onClick (fun _ -> dispatch Decrement)
        ]

        Html.button [
            prop.text "Increment local state"
            prop.onClick (fun _ -> setLocalCount(localCount + 1))
        ]
    ]
```
### Disposing of resources

Documentation/Samples WIP

> TL;DR: Have your `State/Model` type implement `IDisposable` and `React.useElmish` will take care of calling the dispose function when the component unmounts.