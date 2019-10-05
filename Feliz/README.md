# Feliz (beta) [![Nuget](https://img.shields.io/nuget/v/Feliz.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz)

A fresh retake of the base React DSL to build React applications, optimized for happiness.

Here is how it looks like:

```fs
open Feliz

let render state dispatch =
  Html.div [
    Html.button [
        prop.style [ style.marginRight 5 ]
        prop.onClick (fun _ -> dispatch Increment)
        prop.text "Increment"
    ]

    Html.button [
        prop.style [ style.marginLeft 5 ]
        prop.onClick (fun _ -> dispatch Decrement)
        prop.text "Decrement"
    ]

    Html.h1 state.Count
  ]
```

### Features

 - Consistent, lightweight **formatting**: no more awkward indentation using two lists for every element.
 - Discoverable **attributes** with no more functions, `Html` attributes or css properties globally available so they are easy to find.
 - Proper **documentation**: each attribute and CSS property
 - Fully **Type-safe**: no more `Margin of obj` but instead utilizing a plethora of overloaded functions to account for the overloaded nature of `CSS` attributes, covering 90%+ of the CSS styles, values and properties.
 - Included **color list** of most commonly used `Html` colors in the `colors` module.
 - **Compatible** with the current DSL used in applications.
 - **Compatible** with [Femto](https://github.com/Zaid-Ajaj/Femto).
 - Approximately **Zero** bundle size increase where everything function body is erased from the generated javascript unless you actually use said function.
