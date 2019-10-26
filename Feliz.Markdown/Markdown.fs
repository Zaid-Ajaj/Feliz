namespace Feliz.Markdown

open Fable.Core
open Fable.Core.JsInterop
open Feliz

type IMarkdownRenderer = interface end

type ICodeProperties =
    abstract language : string
    abstract value : string

type INodeType = interface end

/// Contains all possible node types
type nodeTypes =
    static member inline root = unbox<INodeType> "root"
    static member inline text = unbox<INodeType> "text"
    static member inline breakTag = unbox<INodeType> "break"
    static member inline paragraph = unbox<INodeType> "paragraph"
    static member inline emphasis = unbox<INodeType> "emphasis"
    static member inline strong = unbox<INodeType> "strong"
    static member inline thematicBreak = unbox<INodeType> "thematicBreak"
    static member inline blockquote = unbox<INodeType> "blockquote"
    static member inline delete = unbox<INodeType> "delete"
    static member inline link = unbox<INodeType> "link"
    static member inline image = unbox<INodeType> "image"
    static member inline linkReference = unbox<INodeType> "linkReference"
    static member inline imageReference = unbox<INodeType> "imageReference"
    static member inline table = unbox<INodeType> "table"
    static member inline tableHead = unbox<INodeType> "tableHead"
    static member inline tableBody = unbox<INodeType> "tableBody"
    static member inline tableRow = unbox<INodeType> "tableRow"
    static member inline tableCell = unbox<INodeType> "tableCell"
    static member inline list = unbox<INodeType> "list"
    static member inline listItem = unbox<INodeType> "listItem"
    static member inline definition = unbox<INodeType> "definition"
    static member inline heading = unbox<INodeType> "heading"
    static member inline inlineCode = unbox<INodeType> "inlineCode"
    static member inline code = unbox<INodeType> "code"
    static member inline html = unbox<INodeType> "html"
    static member inline virtualHtml = unbox<INodeType> "virtualHtml"

[<Erase>]
type markdown =
    /// The Markdown source to parse (**REQUIRED**).
    static member inline source (value: string) = Interop.mkAttr "source" value
    /// Setting to `false` will cause HTML to be rendered (see notes below about proper HTML support). Be aware that setting this to false might cause security issues if the input is user-generated. Use at your own risk. (default: `true`).
    static member inline escapeHtml(value: bool) = Interop.mkAttr "escapeHtml" value
    /// Setting to `true` will skip inlined and blocks of HTML (default: `false`).
    static member inline skipHtml(value: bool) = Interop.mkAttr "skipHtml" value
    /// Setting to `true` will add `data-sourcepos` attributes to all elements, indicating where in the markdown source they were rendered from (default: `false`).
    static member inline sourcePosition(value: bool) = Interop.mkAttr "sourcePos" value
    /// Setting to `true` will pass a sourcePosition property to all renderers with structured source position information (default: `false`).
    static member inline rawSourcePosition(value: bool) = Interop.mkAttr "rawSourcePos" value
    /// Setting to `true` will pass index and parentChildCount props to all renderers (default: `false`).
    static member inline includeNodeIndex (value: bool) = Interop.mkAttr "includeNodeIndex" value
    /// Defines which types of nodes should be allowed (rendered). (default: all types).
    static member inline allowedTypes (types: INodeType list) = Interop.mkAttr "allowedTypes" (Array.ofList types)
    /// Defines which types of nodes should be allowed (rendered). (default: all types).
    static member inline allowedTypes (types: INodeType array) = Interop.mkAttr "allowedTypes" types
    /// Defines which types of nodes should be disallowed (not rendered). (default: none).
    static member inline disallowedTypes (types: INodeType list) = Interop.mkAttr "disallowedTypes" (Array.ofList types)
    /// Defines which types of nodes should be disallowed (not rendered). (default: none).
    static member inline disallowedTypes (types: INodeType array) = Interop.mkAttr "disallowedTypes" types
    static member inline renderers (renderers: IMarkdownRenderer list) =
        Interop.mkAttr "renderers" (createObj !!renderers)

type Markdown =
    static member inline markdown (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-markdown", createObj !!properties)

module markdown =
    type renderers =
        static member inline code (render: ICodeProperties -> Fable.React.ReactElement) =
            unbox<IMarkdownRenderer> (Interop.mkAttr "code" render)