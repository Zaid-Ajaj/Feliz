# Feliz [![Build Status](https://travis-ci.org/Zaid-Ajaj/Feliz.svg?branch=master)](https://travis-ci.org/Zaid-Ajaj/Feliz) [![Nuget](https://img.shields.io/nuget/v/Feliz.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz)

A fresh re-implementations of the base React DSL used within Elmish applications, optimized for maximum happpiness!

Here is how it looks like:
```fsharp
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

This implementation puts emphasis on:
 - Consistent **formatting**: no more awkward indentation using two lists for every element
 - Discoverable **attributes**: no more functions, `Html` attributes or css properties globally available.
 - Proper **documentation**: for each attribute and css property
 - Fully **Type-safe**: no more `Margin of obj` but instead utilizing a plethora of overloaded functions to account for the overloaded nature of `CSS` attributes
 - Included **color list** of most commonly used `Html` colors.
 - **Compatible**: with the current DSL used in applications
 
 Using the styles looks like this:
 ```fsharp
let customStyles =
    attr.style [
        style.fontSize 20
        style.borderRadius 15
        style.borderRadius "20px"
        style.margin 10
        style.margin "20px"
        style.margin(10, 10, 10, 20)
        style.width 10
        style.height 100
        style.height "100%"
        style.backgroundColor colors.fuchsia
        style.border(3, borderStyle.dashed, colors.crimson)
        style.borderColor colors.yellowGreen
        style.color colors.lightGoldenRodYellow
        style.alignContent alignContent.flexStart
        style.textDecorationColor colors.red
        style.visibility visibility.hidden
        style.textDecoration textDecorationLine.lineThrough
        style.position position.sticky
        style.display display.flex
    ]
 ```