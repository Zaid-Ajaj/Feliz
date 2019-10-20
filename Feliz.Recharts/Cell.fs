namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
/// Cell can be wrapped by Pie, Bar, or RadialBar to specify attributes of each child. In Pie , for example, we can specify the attributes of each child node through data, but the props of Cell have higher priority.
type cell =
    static member inline key (value: string) = Interop.mkAttr "key" value
    static member inline key (value: Guid) = Interop.mkAttr "key" (string value)
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color
    static member inline strokeWidth (width: int) = Interop.mkAttr "strokeWidth" width
    static member inline fill (color: string) = Interop.mkAttr "fill" color
    static member inline fillOpacity (value: int) = Interop.mkAttr "fillOpacity" value
    static member inline fillOpacity (value: float) = Interop.mkAttr "fillOpacity" value
    static member inline cursor (value: string) = Interop.mkAttr "cursor" value
