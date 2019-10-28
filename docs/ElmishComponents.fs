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

let application = React.elmishComponent(init, update, render)