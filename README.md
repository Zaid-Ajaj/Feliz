# Feliz (WIP) [![Nuget](https://img.shields.io/nuget/v/Feliz.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz)

A fresh retake of the base React DSL used within Elmish applications, optimized for maximum happiness.

Here is how it looks like:
```fsharp
open Feliz

let render state dispatch =
    Html.div [
        attr.id "main"
        attr.style [ style.padding 20 ]
        attr.children [

            Html.button [
                attr.style [ style.marginRight 5 ]
                attr.onClick (fun _ -> dispatch Increment)
                attr.content "Increment"
            ]

            Html.button [
                attr.style [ style.marginRight 5 ]
                attr.onClick (fun _ -> dispatch Decrement)
                attr.content "Decrement"
            ]

            Html.h1 state.Count
        ]
    ]
```

### Features

 - Consistent **formatting**: no more awkward indentation using two lists for every element
 - Discoverable **attributes** with no more functions, `Html` attributes or css properties globally available.
 - Proper **documentation**: each attribute and css property
 - Fully **Type-safe**: no more `Margin of obj` but instead utilizing a plethora of overloaded functions to account for the overloaded nature of `CSS` attributes
 - Included **color list** of most commonly used `Html` colors.
 - **Compatible** with the current DSL used in applications.
 - **Compatible** with [Femto](https://github.com/Zaid-Ajaj/Femto).

### Overloaded elements

When you want to display a single string or number simply use:
```fs
Html.h1 state.Count

Html.div "Hello there!"
```
but you could also expand the attribute:
```fs
Html.h1 [
    attr.className "title"
    attr.content "Hello there!"
]
```
Here `attr.content` is simply `attr.children [ Html.text "Hello there!" ]` so you could expand it even further:
```fs
Html.h1 [
    attr.className "title"
    attr.children [
        Html.text "Hello there"
    ]
]
```

### Type-safe style attributes

```fs
let customStyles =
    attr.style [
        style.display display.none
        style.fontSize 20
        style.borderRadius 15
        style.borderRadius (length.px 10)
        style.margin 10
        style.margin (length.px 10)
        style.margin(10, 10, 10, 20)
        style.width 10
        style.height 100
        style.height (length.vh 50)
        style.height (length.percent 100)
        style.backgroundColor colors.fuchsia
        style.border(3, borderStyle.dashed, colors.crimson)
        style.borderColor colors.yellowGreen
        style.color colors.lightGoldenRodYellow
        style.alignContent alignContent.flexStart
        style.textDecorationColor colors.red
        style.visibility visibility.hidden
        style.textDecoration textDecorationLine.lineThrough
        style.position position.sticky
    ]
```

### Conditional classes and styles

The library includes two convenient functions to apply classes or styles conditionally on elements:
```fsharp
Html.div [
    attr.classList [ true, "shiny"; state.Count < 0, "danger" ]
    attr.styleList [
        true, [ style.margin 10 ]
        state.Count >= 10, [
            style.backgroundColor colors.red
            style.fontSize 20
        ]
        state.Count >= 20, [
            style.backgroundColor colors.green
            style.fontSize 30
        ]
    ]
]
```