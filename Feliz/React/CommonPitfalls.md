# Common Pitfalls (Applies only to Fable 2.x)

When using React components, there are a couple of gotcha's to watch out for. Here are some examples to follow from.

## Components must take a record as input

You might be tempted to use a tuple for multiple parameters of a React component but React expects a single object as the input properties. Use records, anonymous records or `unit` when defining input for React components.

### <i class="fa fa-times" style="color:red; margin-right: 5px" ></i> Don't do this

```fsharp
let counter = React.functionComponent(fun (min: int, max: int) ->
    let (count, setCount) = React.useState(min)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> if count < max then setCount(count + 1))
        ]
    ])
```
### <i class="fa fa-check" style="color:lightgreen; margin-right: 5px" ></i> Do this
```fsharp
let counter = React.functionComponent(fun (input: {| min: int; max: int |}) ->
    let (count, setCount) = React.useState(input.min)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> if count < input.max then setCount(count + 1))
        ]
    ])
```
### <i class="fa fa-check" style="color:lightgreen; margin-right: 5px" ></i> Or this

```fsharp
type CounterProps = { min: int; max: int }

let counter = React.functionComponent(fun (input: CounterProps) ->
    let (count, setCount) = React.useState(input.min)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> if count < input.max then setCount(count + 1))
        ]
    ])
```
## Components must be defined at the module level

Even though you can define a React component inside another function, you definitely should *not* because the internal state of that component will be reset once the parent function (inside of which the component is defined) is re-evaulauated.

### <i class="fa fa-times" style="color:red; margin-right: 5px" ></i> Don't do this
```fsharp
let counter (start: int) =
    let counter' = React.functionComponent(fun (input: {| start: int |}) ->
        let (count, setCount) = React.useState(input.start)
        Html.div [
            Html.h1 count
            Html.button [
                prop.text "Increment"
                prop.onClick (fun _ -> setCount(count + 1))
            ]
        ])

    counter' {| start = start |}
```
### <i class="fa fa-check" style="color:lightgreen; margin-right: 5px" ></i> Do this
```fsharp
let counter' = React.functionComponent("Counter", fun (input: {| start: int |}) ->
    let (count, setCount) = React.useState(input.start)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ])

let counter (start: int) = counter' {| start = start |}
```