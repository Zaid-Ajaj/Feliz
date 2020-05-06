module CodeSplitting

open Fable.Core.JsInterop
open Feliz

let myCodeSplitComponent = React.functionComponent(fun () ->
    Html.div [
        prop.text "I was loaded asynchronously!"
    ])

exportDefault myCodeSplitComponent
