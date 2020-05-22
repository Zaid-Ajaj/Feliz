[<RequireQualifiedAccess>]
module UseElmishExamples

open Feliz
open Feliz.UseElmish
open Elmish
open System
open Fable.Core

type Msg =
    | Increment
    | Decrement

type State = { Count : int }

let init() = { Count = 0 }, Cmd.none

let update msg state =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none

let counter = React.functionComponent(fun () ->
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
)

[<Emit("setTimeout($0, $1)")>]
let setTimeout (f: unit -> unit) (timeout: int) : int = jsNative

[<Emit("clearTimeout($0)")>]
let clearTimeout (id: int) : unit = jsNative

let counterCombined = React.functionComponent(fun () ->
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
    ])