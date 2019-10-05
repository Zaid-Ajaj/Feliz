namespace Feliz.Markdown

open Fable.Core
open Fable.Core.JsInterop
open Feliz

type markdown =
    static member inline source (value: string) = Interop.mkAttr "source" value

type Markdown =
    static member inline markdown (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-markdown", createObj !!properties)