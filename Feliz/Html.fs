namespace Feliz

open Fable.React
open Fable.Core
open Fable.Core.JsInterop
open System

[<Erase>]
type Html =
    /// The empty element, renders nothing on screen
    static member inline none : ReactElement = unbox null
    /// The `<div>` tag defines a division or a section in an HTML document
    static member inline div xs = Interop.createElement "div" xs
    [<Obsolete("Html.fragment is obsolete, use React.fragment instead. This function will be removed in the coming v1.0 release")>]
    static member inline fragment xs = Fable.React.Helpers.fragment [] xs
    [<Obsolete("Html.keyedFragment is obsolete, use React.keyedFragment instead. This function will be removed in the coming v1.0 release")>]
    static member inline keyedFragment(key: int, xs) = Fable.React.Helpers.fragment [ !!("key", key) ] xs
    [<Obsolete("Html.keyedFragment is obsolete, use React.keyedFragment instead. This function will be removed in the coming v1.0 release")>]
    static member inline keyedFragment(key: string, xs) = Fable.React.Helpers.fragment [ !!("key", key) ] xs
    [<Obsolete("Html.keyedFragment is obsolete, use React.keyedFragment instead. This function will be removed in the coming v1.0 release")>]
    static member inline keyedFragment(key: System.Guid, xs) = Fable.React.Helpers.fragment [ !!("key", string key) ] xs
    static member inline em xs = Interop.createElement "em" xs
    static member inline iframe xs = Interop.createElement "iframe" xs
    static member inline article xs = Interop.createElement "article" xs
    static member inline article (children: #seq<ReactElement>) = Interop.reactElement "article" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline aside xs = Interop.createElement "aside" xs
    static member inline aside (children: #seq<ReactElement>) = Interop.reactElement "aside" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline footer xs = Interop.createElement "footer" xs
    static member inline footer (children: #seq<ReactElement>) = Interop.reactElement "footer" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline progress xs = Interop.createElement "progress" xs
    static member inline progress (children: #seq<ReactElement>) = Interop.reactElement "progress" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline nav xs = Interop.createElement "nav" xs
    static member inline nav (children: #seq<ReactElement>) = Interop.reactElement "nav" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline label xs = Interop.createElement "label" xs
    static member inline label (children: #seq<ReactElement>) = Interop.reactElement "label" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline fieldSet xs = Interop.createElement "fieldset" xs
    static member inline fieldSet (children: #seq<ReactElement>) = Interop.reactElement "fieldset" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline section xs = Interop.createElement "section" xs
    static member inline section (children: #seq<ReactElement>) = Interop.reactElement "section" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline figure xs = Interop.createElement "figure" xs
    static member inline figure (children: #seq<ReactElement>) = Interop.reactElement "figure" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline figcaption xs = Interop.createElement "figcaption" xs
    static member inline figcaption (children: #seq<ReactElement>) = Interop.reactElement "figcaption" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline select xs = Interop.createElement "select" xs
    static member inline select (children: #seq<ReactElement>) = Interop.reactElement "select" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline option xs = Interop.createElement "option" xs
    static member inline option (children: #seq<ReactElement>) = Interop.reactElement "option" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline strong xs = Interop.createElement "strong" xs
    static member inline table xs = Interop.createElement "table" xs
    static member inline tbody xs = Interop.createElement "tbody" xs
    static member inline tbody (children: #seq<ReactElement>) = Interop.reactElement "tbody" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline tableBody xs = Interop.createElement "tbody" xs
    static member inline tableBody (children: #seq<ReactElement>) = Interop.reactElement "tbody" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline tableRow xs = Interop.createElement "tr" xs
    static member inline tr xs = Interop.createElement "tr" xs
    static member inline tr (children: #seq<ReactElement>) = Interop.reactElement "tr" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline del xs = Interop.createElement "del" xs
    static member inline ins xs = Interop.createElement "ins" xs
    static member inline dfn xs = Interop.createElement "ins" xs
    static member inline dialog xs = Interop.createElement "dialog" xs
    static member inline tableCell xs = Interop.createElement "td" xs
    static member inline details xs = Interop.createElement "details" xs
    static member inline details (children: #seq<ReactElement>) = Interop.reactElement "details" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline summary xs = Interop.createElement "summary" xs
    static member inline summary (children: #seq<ReactElement>) = Interop.reactElement "summary" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline main xs = Interop.createElement "main" xs
    static member inline main (children: #seq<ReactElement>) = Interop.reactElement "main" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline canvas xs = Interop.createElement "canvas" xs
    static member inline td xs = Interop.createElement "td" xs
    static member inline th xs = Interop.createElement "th" xs
    static member inline tableHeader xs = Interop.createElement "th" xs
    static member inline tableHeader (children: #seq<ReactElement>) = Interop.reactElement "th" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline thead xs = Interop.createElement "thead" xs
    static member inline thead (children: #seq<ReactElement>) = Interop.reactElement "thead" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline tfoot xs = Interop.createElement "tfoot" xs
    static member inline tfoot (children: #seq<ReactElement>) = Interop.reactElement "tfoot" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline textarea xs = Interop.createElement "textarea" xs
    static member inline textarea (children: #seq<ReactElement>) = Interop.reactElement "textarea" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline video xs = Interop.createElement "video" xs
    static member inline video (children: #seq<ReactElement>) = Interop.reactElement "video" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h1 xs = Interop.createElement "h1" xs
    static member inline h2 xs = Interop.createElement "h2" xs
    static member inline h3 xs = Interop.createElement "h3" xs
    static member inline h4 xs = Interop.createElement "h4" xs
    static member inline h5 xs = Interop.createElement "h5" xs
    static member inline h6 xs = Interop.createElement "h6" xs
    static member inline button xs = Interop.createElement "button" xs
    static member inline button (children: #seq<ReactElement>) = Interop.reactElement "button" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline span xs = Interop.createElement "span" xs
    static member inline li xs = Interop.createElement "li" xs
    static member inline ul xs = Interop.createElement "ul" xs
    static member inline ul (children: #seq<ReactElement>) = Interop.reactElement "ul" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline ol xs = Interop.createElement "ol" xs
    static member inline ol (children: #seq<ReactElement>) = Interop.reactElement "ol" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline a xs = Interop.createElement "a" xs
    static member inline a (children: #seq<ReactElement>) = Interop.reactElement "a" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline anchor xs = Interop.createElement "a" xs
    static member inline anchor (children: #seq<ReactElement>) = Interop.reactElement "a" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline img xs = Interop.createElement "img" xs
    static member inline br xs = Interop.createElement "br" xs
    static member inline hr xs = Interop.createElement "hr" xs
    static member inline input xs = Interop.createElement "input" xs
    static member inline form xs = Interop.createElement "form" xs
    static member inline form (children: #seq<ReactElement>) = Interop.reactElement "form" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline i xs = Interop.createElement "i" xs
    static member inline p xs = Interop.createElement "p" xs
    static member inline paragraph xs = Interop.createElement "p" xs
    static member inline listItem xs = Interop.createElement "li" xs
    static member inline unorderedList xs = Interop.createElement "ul" xs
    static member inline unorderedList (children: #seq<ReactElement>) = Interop.reactElement "ul" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline orderedList xs = Interop.createElement "ol" xs
    static member inline orderedList (children: #seq<ReactElement>) = Interop.reactElement "ol" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline svg xs = Interop.createElement "svg" xs
    static member inline svg (children: #seq<ReactElement>) = Interop.reactElement "svg" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline circle xs = Interop.createElement "circle" xs
    static member inline circle (children: #seq<ReactElement>) = Interop.reactElement "circle" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline text xs = Interop.createElement "text" xs
    static member inline g xs = Interop.createElement "g" xs
    static member inline g (children: #seq<ReactElement>) = Interop.reactElement "g" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline line xs = Interop.createElement "line" xs
    static member inline line (children: #seq<ReactElement>) = Interop.reactElement "line" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline rect xs = Interop.createElement "rect" xs
    static member inline rect (children: #seq<ReactElement>) = Interop.reactElement "rect" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline polygon xs = Interop.createElement "polygon" xs
    static member inline polygon (children: #seq<ReactElement>) = Interop.reactElement "polygon" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline ellipse xs = Interop.createElement "ellipse" xs
    static member inline ellipse (children: #seq<ReactElement>) = Interop.reactElement "ellipse" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline defs xs = Interop.createElement "defs" xs
    static member inline defs (children: #seq<ReactElement>) = Interop.reactElement "defs" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline radialGradient xs = Interop.createElement "radialGradient" xs
    static member inline radialGradient (children: #seq<ReactElement>) = Interop.reactElement "radialGradient" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline polyline xs = Interop.createElement "polyline" xs
    static member inline polyline (children: #seq<ReactElement>) = Interop.reactElement "polyline" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline path xs = Interop.createElement "path" xs
    static member inline path (children: #seq<ReactElement>) = Interop.reactElement "path" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline pre xs = Interop.createElement "pre" xs
    static member inline code xs = Interop.createElement "code" xs
    static member inline meta xs = Interop.createElement "meta" xs
    static member inline head xs = Interop.createElement "head" xs
    static member inline head (children: #seq<ReactElement>) = Interop.reactElement "head" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline header xs = Interop.createElement "header" xs
    static member inline header (children: #seq<ReactElement>) = Interop.reactElement "header" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline body xs = Interop.createElement "body" xs
    static member inline body (children: #seq<ReactElement>) = Interop.reactElement "body" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline clipPath xs = Interop.createElement "clipPath" xs
    static member inline clipPath (children: #seq<ReactElement>) = Interop.reactElement "clipPath" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline linearGradient xs = Interop.createElement "linearGradient" xs
    static member inline linearGradient (children: #seq<ReactElement>) = Interop.reactElement "linearGradient" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline content (value: string) : ReactElement = unbox value
    static member inline content (value: int) : ReactElement = unbox value
    static member inline text (value: string) : ReactElement = unbox value
    static member inline text (value: int) : ReactElement = unbox value
    static member inline text (value: System.Guid) : ReactElement = unbox (string value)
    static member inline stop xs = Interop.createElement "stop" xs
    static member inline stop (children: #seq<ReactElement>) = Interop.reactElement "stop" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline div (value: ReactElement) = Interop.reactElement "div" (createObj [ "children" ==> [| value |] ])
    static member inline div (children: #seq<ReactElement>) = Interop.reactElement "div" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline span (value: ReactElement)  = Interop.reactElement "span" (createObj [ "children" ==> [| value |] ])
    static member inline span (children: #seq<ReactElement>) = Interop.reactElement "span" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h1 (value: ReactElement)  = Interop.reactElement "h1" (createObj [ "children" ==> [| value |] ])
    static member inline h1 (children: #seq<ReactElement>) = Interop.reactElement "h1" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h2 (value: ReactElement)  =  Interop.reactElement "h2" (createObj [ "children" ==> [| value |] ])
    static member inline h2 (children: #seq<ReactElement>) = Interop.reactElement "h2" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h3 (value: ReactElement)  =  Interop.reactElement "h3" (createObj [ "children" ==> [| value |] ])
    static member inline h3 (children: #seq<ReactElement>) = Interop.reactElement "h3" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h4 (value: ReactElement)  = Interop.reactElement "h4" (createObj [ "children" ==> [| value |] ])
    static member inline h4 (children: #seq<ReactElement>) = Interop.reactElement "h4" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h5 (value: ReactElement)  = Interop.reactElement "h5" (createObj [ "children" ==> [| value |] ])
    static member inline h5 (children: #seq<ReactElement>) = Interop.reactElement "h5" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline h6 (value: ReactElement)  = Interop.reactElement "h6" (createObj [ "children" ==> [| value |] ])
    static member inline h6 (children: #seq<ReactElement>) = Interop.reactElement "h6" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline strong(value: ReactElement)  = Interop.reactElement "strong" (createObj [ "children" ==> [| value |] ])
    static member inline strong (children: #seq<ReactElement>) = Interop.reactElement "strong" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline p(value: ReactElement) = Interop.reactElement "p" (createObj [ "children" ==> [| value |] ])
    static member inline p (children: #seq<ReactElement>) = Interop.reactElement "p" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline paragraph(value: ReactElement)  = Interop.reactElement "p" (createObj [ "children" ==> [| value |] ])
    static member inline paragraph (children: #seq<ReactElement>) = Interop.reactElement "p" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline td(value: ReactElement) = Interop.reactElement "td" (createObj [ "children" ==> [| value |] ])
    static member inline td (children: #seq<ReactElement>) = Interop.reactElement "td" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline th(value: ReactElement)  = Interop.reactElement "th" (createObj [ "children" ==> [| value |] ])
    static member inline th (children: #seq<ReactElement>) = Interop.reactElement "th" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline pre(value: ReactElement)  = Interop.reactElement "pre" (createObj [ "children" ==> [| value |] ])
    static member inline pre (children: #seq<ReactElement>) = Interop.reactElement "pre" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline pre(value: string)  = Interop.reactElement "pre" (createObj [ "children" ==> [| value |] ])
    static member inline pre(value: int)  = Interop.reactElement "pre" (createObj [ "children" ==> [| value |] ])
    static member inline pre(value: bool)  = Interop.reactElement "pre" (createObj [ "children" ==> [| value |] ])
    static member inline pre(value: float)  = Interop.reactElement "pre" (createObj [ "children" ==> [| value |] ])
    static member inline code(value: ReactElement)  = Interop.reactElement "code" (createObj [ "children" ==> [| value |] ])
    static member inline code (children: #seq<ReactElement>) = Interop.reactElement "code" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline code(value: string)  = Interop.reactElement "code" (createObj [ "children" ==> [| value |] ])
    static member inline code(value: int)  = Interop.reactElement "code" (createObj [ "children" ==> [| value |] ])
    static member inline code(value: bool)  = Interop.reactElement "code" (createObj [ "children" ==> [| value |] ])
    static member inline li(value: ReactElement) = Interop.reactElement "li" (createObj [ "children" ==> [| value |] ])
    static member inline li (children: #seq<ReactElement>) = Interop.reactElement "li" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline listItem(value: ReactElement) = Interop.reactElement "li" (createObj [ "children" ==> [| value |] ])
    static member inline listItem (children: #seq<ReactElement>) = Interop.reactElement "li" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline div (value: string) = Interop.reactElement "div" (createObj [ "children" ==> [| value |] ])
    static member inline div (value: int)  = Interop.reactElement "div" (createObj [ "children" ==> [| value |] ])
    static member inline span (value: string) = Interop.reactElement "span" (createObj [ "children" ==> [| value |] ])
    static member inline span (value: int)  = Interop.reactElement "span" (createObj [ "children" ==> [| value |] ])
    static member inline h1 (value: string)  = Interop.reactElement "h1" (createObj [ "children" ==> [| value |] ])
    static member inline h1 (value: int)  = Interop.reactElement "h1" (createObj [ "children" ==> [| value |] ])
    static member inline h2 (value: string)  =  Interop.reactElement "h2" (createObj [ "children" ==> [| value |] ])
    static member inline h2 (value: int)  =  Interop.reactElement "h2" (createObj [ "children" ==> [| value |] ])
    static member inline h3 (value: string)  =  Interop.reactElement "h3" (createObj [ "children" ==> [| value |] ])
    static member inline h3 (value: int)  =  Interop.reactElement "h3" (createObj [ "children" ==> [| value |] ])
    static member inline h4 (value: string)  = Interop.reactElement "h4" (createObj [ "children" ==> [| value |] ])
    static member inline h4 (value: int)  = Interop.reactElement "h4" (createObj [ "children" ==> [| value |] ])
    static member inline h5 (value: string)  = Interop.reactElement "h5" (createObj [ "children" ==> [| value |] ])
    static member inline h5 (value: int)  = Interop.reactElement "h5" (createObj [ "children" ==> [| value |] ])
    static member inline h6 (value: string)  = Interop.reactElement "h6" (createObj [ "children" ==> [| value |] ])
    static member inline h6 (value: int) = Interop.reactElement "h6" (createObj [ "children" ==> [| value |] ])
    static member inline strong(value: string)  = Interop.reactElement "strong" (createObj [ "children" ==> [| value |] ])
    static member inline strong(value: int) = Interop.reactElement "strong" (createObj [ "children" ==> [| value |] ])
    static member inline p(value: string) = Interop.reactElement "p" (createObj [ "children" ==> [| value |] ])
    static member inline p(value: int) = Interop.reactElement "p" (createObj [ "children" ==> [| value |] ])
    static member inline paragraph(value: string)  = Interop.reactElement "p" (createObj [ "children" ==> [| value |] ])
    static member inline paragraph(value: int)  = Interop.reactElement "p" (createObj [ "children" ==> [| value |] ])
    static member inline td(value: string) = Interop.reactElement "td" (createObj [ "children" ==> [| value |] ])
    static member inline td(value: int)  = Interop.reactElement "td" (createObj [ "children" ==> [| value |] ])
    static member inline th(value: string)  = Interop.reactElement "th" (createObj [ "children" ==> [| value |] ])
    static member inline th(value: int)  = Interop.reactElement "th" (createObj [ "children" ==> [| value |] ])
    static member inline li(value: int) = Interop.reactElement "li" (createObj [ "children" ==> [| value |] ])
    static member inline li(value: string) = Interop.reactElement "li" (createObj [ "children" ==> [| value |] ])
    static member inline listItem(value: int) = Interop.reactElement "li" (createObj [ "children" ==> [| value |] ])
    static member inline listItem(value: string) = Interop.reactElement "li" (createObj [ "children" ==> [| value |] ])
    static member inline del(value: string) = Interop.reactElement "del" (createObj [ "children" ==> [| value |] ])
    static member inline del(value: int) = Interop.reactElement "del" (createObj [ "children" ==> [| value |] ])
    static member inline del(value: ReactElement) = Interop.reactElement "del" (createObj [ "children" ==> [| value |] ])
    static member inline del (children: #seq<ReactElement>) = Interop.reactElement "del" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline ins(value: int) = Interop.reactElement "ins" (createObj [ "children" ==> [| value |] ])
    static member inline ins(value: string) = Interop.reactElement "ins" (createObj [ "children" ==> [| value |] ])
    static member inline ins (children: #seq<ReactElement>) = Interop.reactElement "ins" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline ins(value: ReactElement) = Interop.reactElement "ins" (createObj [ "children" ==> [| value |] ])
    static member inline dfn (children: #seq<ReactElement>) = Interop.reactElement "dfn" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline dfn(value: int) = Interop.reactElement "dfn" (createObj [ "children" ==> [| value |] ])
    static member inline dfn(value: string) = Interop.reactElement "dfn" (createObj [ "children" ==> [| value |] ])
    static member inline dfn(value: ReactElement) = Interop.reactElement "dfn" (createObj [ "children" ==> [| value |] ])
    static member inline dialog (children: #seq<ReactElement>) = Interop.reactElement "dialog" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline dialog(value: int) = Interop.reactElement "dialog" (createObj [ "children" ==> [| value |] ])
    static member inline dialog(value: string) = Interop.reactElement "dialog" (createObj [ "children" ==> [| value |] ])
    static member inline dialog(value: ReactElement) = Interop.reactElement "dialog" (createObj [ "children" ==> [| value |] ])
    static member inline em (children: #seq<ReactElement>) = Interop.reactElement "em" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
    static member inline em(value: int) = Interop.reactElement "em" (createObj [ "children" ==> [| value |] ])
    static member inline em(value: string) = Interop.reactElement "em" (createObj [ "children" ==> [| value |] ])
    static member inline em(value: ReactElement) = Interop.reactElement "em" (createObj [ "children" ==> [| value |] ])
    static member inline i(value: int) = Interop.reactElement "i" (createObj [ "children" ==> [| value |] ])
    static member inline i(value: string) = Interop.reactElement "i" (createObj [ "children" ==> [| value |] ])
    static member inline i(value: ReactElement) = Interop.reactElement "i" (createObj [ "children" ==> [| value |] ])
    static member inline i (children: #seq<ReactElement>) = Interop.reactElement "i" (createObj [ "children" ==> Interop.reactApi.Children.toArray (unbox<ReactElement list> children) ])
