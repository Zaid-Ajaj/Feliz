# Feliz [![Build Status](https://travis-ci.org/Zaid-Ajaj/Elmish.Toastr.svg?branch=master)](https://travis-ci.org/Zaid-Ajaj/Feliz) [![Nuget](https://img.shields.io/nuget/v/Feliz.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz)


A fresh re-implementation of the React DLS used in Elmish applications for the base Html elements that will make you and your team happy :smile:

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