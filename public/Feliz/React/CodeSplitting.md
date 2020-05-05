# Code Splitting


```fsharp
// CodeSplitting.fs
module CodeSplitting

open Fable.Core.JsInterop
open Feliz

let myCodeSplitComponent = React.functionComponent(fun () ->
    Html.div [
        prop.text "I was loaded asynchronously!"
    ])

exportDefault myCodeSplitComponent
```

```fsharp:code-splitting
let myNonCodeSplitComponent = React.functionComponent(fun () ->
    Html.div [
        prop.text "I was loaded synchronously!"
    ])

let myCodeSplitComponent : obj = React.lazy'(fun () -> JsInterop.importDynamic "./temp/CodeSplitting.js")

let centeredSpinner =
    Html.div [
        prop.style [
            style.textAlign.center
            style.marginLeft length.auto
            style.marginRight length.auto
            style.marginTop 50
        ]
        prop.children [
            Html.li [
                prop.className [
                    FA.Fa
                    FA.FaRefresh
                    FA.FaSpin
                    FA.Fa3X
                ]
            ]
        ]
    ]

let codeSplitting = React.functionComponent(fun () ->
    Html.div [
        prop.children [
            myNonCodeSplitComponent()
            React.suspense([
                Html.div [
                    Interop.reactApi.createElement(myCodeSplitComponent, ())
                ]
            ], centeredSpinner)
        ]
    ])
```

See the [React docs] for more information about code splitting.

[React docs]:https://reactjs.org/docs/strict-mode.html
