# Conditional Styling

The `prop.style` has an overload that takes conditional lists for applying a bunch of style properties based on some predicates.
```fsharp
Html.div [
    prop.style [
        true, [ style.margin 10 ]
        state.Count >= 10, [
            style.backgroundColor.red
            style.fontSize 20
        ]
        state.Count >= 20, [
            style.backgroundColor.green
            style.fontSize 30
        ]
    ]
]
```
Now with F# 4.7 it is even easier without this overload as you omit the `yield` keyword:
```fs
Html.div [
    prop.style [
        style.margin 10
        style.borderRadius 15
        if state.Errored
        then style.color.red
        else style.color.green
    ]
]
```