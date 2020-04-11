module Samples.ElmishComponents

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

module WithDispose =
    open Fable.Core
    open Zanaptak.TypedCssClasses

    type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>
    type FA = CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>

    let disposableComponent = React.functionComponent (fun (props: {| load: unit -> ReactElement |}) ->
        let (started, setStarted) = React.useState(false)
        Html.div [
            Html.button [
                prop.className [ Bulma.Button; Bulma.IsPrimary; Bulma.IsLarge ]
                if not started then prop.onClick (fun _ -> setStarted(true))
                else prop.onClick (fun _ -> setStarted(false))
                prop.children [
                    Html.i [ prop.className [ "fa"; "fa-rocket" ] ]
                    Html.span [
                        prop.style [ style.marginLeft 10 ]
                        if not started then prop.text "Run Sample"
                        else prop.text "Unmount Sample"
                    ]
                ]
            ]
    
            if started then props.load()
        ])

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
    
    let example () = React.elmishComponent("CounterWithDispose", init, update, render)

    let application = disposableComponent {| load = example |}

module ReplacementTests =
    type Counter = { Count : int; Title: string }
    type CounterMsg = Increment | IncrementDelayed
    let init (title: string) = { Count = 0; Title = title }, Cmd.none

    let update msg (state: Counter) : Counter * Cmd<CounterMsg> =
        match msg with
        | IncrementDelayed ->
            state, Cmd.OfAsync.perform (fun () ->
            async {
                do! Async.Sleep 3000
                return Increment
            }) () (fun msg -> msg)
        | Increment ->
            { state with Count = state.Count + 1 }, Cmd.none

    let render (state: Counter) dispatch =
        Html.div [
            Html.h1 (sprintf "%s: %d" state.Title state.Count)
            Html.button [
                prop.onClick (fun _ -> dispatch Increment)
                prop.text "Increment"
            ]
            Html.button [
                prop.onClick (fun _ -> dispatch IncrementDelayed)
                prop.text "Increment Delayed"
            ]
        ]

    let counter title = React.elmishComponent("Counter", init title, update, render)

    let counterSwitcher = React.functionComponent(fun () -> [
        let (switch, setSwitch) = React.useState(false)
        Html.div [
            if switch
            then counter "First Counter"
            else counter "Second Counter"
            Html.hr [ ]
            Html.button [
                prop.onClick (fun _ -> setSwitch(not switch))
                prop.text "Switch Counters"
            ]
        ]
    ])
