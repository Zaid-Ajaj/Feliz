namespace Feliz.Markdown

open Fable.Core
open Fable.Core.JsInterop
open Feliz

type IMarkdownRenderer = interface end

type ICodeProperties =
    abstract language : string
    abstract value : string

type markdown =
    static member inline source (value: string) = Interop.mkAttr "source" value
    static member inline escapeHtml(value: bool) = Interop.mkAttr "escapeHtml" value
    static member inline renderers (renderers: IMarkdownRenderer list) =
        Interop.mkAttr "renderers" (createObj !!renderers)

type Markdown =
    static member inline markdown (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-markdown", createObj !!properties)

module markdown =
    type renderers =
        static member inline code (render: ICodeProperties -> Fable.React.ReactElement) =
            unbox<IMarkdownRenderer> (Interop.mkAttr "code" render)