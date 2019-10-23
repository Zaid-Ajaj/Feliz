namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type referenceDot =
    static member inline xAxisId(value: string) = Interop.mkAttr "xAxisId" value
    static member inline xAxisId(value: int) = Interop.mkAttr "xAxisId" value
    static member inline xAxisId(value: float) = Interop.mkAttr "xAxisId" value
    static member inline yAxisId(value: string) = Interop.mkAttr "yAxisId" value
    static member inline yAxisId(value: int) = Interop.mkAttr "yAxisId" value
    static member inline yAxisId(value: float) = Interop.mkAttr "yAxisId" value
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color
    static member inline strokeWidth (width: int) = Interop.mkAttr "strokeWidth" width
    static member inline strokeOpacity (opacity: int) = Interop.mkAttr "strokeOpacity" opacity
    static member inline strokeOpacity (opacity: float) = Interop.mkAttr "strokeOpacity" opacity
    static member inline fill (color: string) = Interop.mkAttr "fill" color
    static member inline fillOpacity (value: int) = Interop.mkAttr "fillOpacity" value
    static member inline fillOpacity (value: float) = Interop.mkAttr "fillOpacity" value
    static member inline x(value: string) = Interop.mkAttr "x" value
    static member inline x(value: int) = Interop.mkAttr "x" value
    static member inline x(value: float) = Interop.mkAttr "x" value
    static member inline y(value: string) = Interop.mkAttr "y" value
    static member inline y(value: int) = Interop.mkAttr "y" value
    static member inline y(value: float) = Interop.mkAttr "y" value
    static member inline r(value: int) = Interop.mkAttr "r" value
    static member inline r(value: float) = Interop.mkAttr "r" value
    static member inline alwaysShow (value: bool) = Interop.mkAttr "alwaysShow" value
    static member inline inFront (value: bool) = Interop.mkAttr "inFront" value
    static member inline label (value: string) = Interop.mkAttr "label" value
    static member inline label (value: int) = Interop.mkAttr "label" value
    static member inline label (value: float) = Interop.mkAttr "label" value