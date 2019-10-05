# Conditional Styling

The library includes two convenient functions to apply classes or styles conditionally on elements:
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