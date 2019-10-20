namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type brush =
    static member inline dataKey (key: string) = Interop.mkAttr "dataKey" key
    static member inline dataKey (key: int) = Interop.mkAttr "dataKey" key
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
    /// The stroke color of the brush
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color
    /// The fill color of the brush
    static member inline fill (color: string) = Interop.mkAttr "fill" color
    /// The height of brush. Default is 40.
    static member inline height (value: int) = Interop.mkAttr "height" value
    /// The width of brush.
    static member inline width (value: int) = Interop.mkAttr "width" value
    /// The default start index of brush. If the option is not set, the start index will be 0.
    static member inline startIndex (index: int) = Interop.mkAttr "startIndex" index
    /// The default end index of brush. If the option is not set, the end index will be calculated by the length of data.
    static member inline endIndex (index: int) = Interop.mkAttr "endIndex" index
    /// The data with gap of refreshing chart. If the option is not set, the chart will be refreshed every time. Default 1.
    static member inline gap(value: int) = Interop.mkAttr "gap" value
    /// The data with gap of refreshing chart. If the option is not set, the chart will be refreshed every time. Default 1.
    static member inline gap(value: float) = Interop.mkAttr "gap" value
    /// The x-coordinate of brush.
    static member inline x(value: int) = Interop.mkAttr "x" value
    /// The y-coordinate of brush.
    static member inline y(value: int) = Interop.mkAttr "y" value
    /// The x-coordinate of brush.
    static member inline x(value: float) = Interop.mkAttr "x" value
    /// The y-coordinate of brush.
    static member inline y(value: float) = Interop.mkAttr "y" value