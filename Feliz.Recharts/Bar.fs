namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type bar =
    static member inline name(value: string) = Interop.mkAttr "name" value
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color
    static member inline strokeWidth (width: int) = Interop.mkAttr "strokeWidth" width
    static member inline strokeOpacity (opacity: int) = Interop.mkAttr "strokeOpacity" opacity
    static member inline strokeOpacity (opacity: float) = Interop.mkAttr "strokeOpacity" opacity
    static member inline fill (color: string) = Interop.mkAttr "fill" color
    static member inline fillOpacity (value: int) = Interop.mkAttr "fillOpacity" value
    static member inline fillOpacity (value: float) = Interop.mkAttr "fillOpacity" value
    static member inline stackId (value: string) = Interop.mkAttr "stackId" value
    static member inline xAxisId (value: string) = Interop.mkAttr "xAxisId" value
    static member inline yAxisId (value: string) = Interop.mkAttr "yAxisId" value
    static member inline xAxisId (value: int) = Interop.mkAttr "xAxisId" value
    static member inline yAxisId (value: int) = Interop.mkAttr "yAxisId" value
    static member inline children (children: ReactElement list) = prop.children children
    static member inline children (children: ReactElement seq) = prop.children children


module bar =
    [<Erase>]
    type legendType =
        static member inline line = Interop.mkAttr "legendType" "line"
        static member inline square = Interop.mkAttr "legendType" "square"
        static member inline rect = Interop.mkAttr "legendType" "rect"
        static member inline circle = Interop.mkAttr "legendType" "circle"
        static member inline cross = Interop.mkAttr "legendType" "cross"
        static member inline diamond = Interop.mkAttr "legendType" "diamond"
        static member inline star = Interop.mkAttr "legendType" "star"
        static member inline triangle = Interop.mkAttr "legendType" "triangle"
        static member inline wye = Interop.mkAttr "legendType" "wye"
        static member inline none = Interop.mkAttr "legendType" "none"

    /// The layout of area in the chart.
    [<Erase>]
    type layout =
        static member inline horizontal = Interop.mkAttr "layout" "horizontal"
        static member inline vertical = Interop.mkAttr "layout" "vertical"

    /// The type of easing function. Default is `ease`.
    [<Erase>]
    type animationEasing =
        static member inline ease = Interop.mkAttr "animationEasing" "ease"
        static member inline easeIn = Interop.mkAttr "animationEasing" "ease-in"
        static member inline easeOut = Interop.mkAttr "animationEasing" "ease-out"
        static member inline easeInOut = Interop.mkAttr "animationEasing" "ease-in-out"
        static member inline linear = Interop.mkAttr "animationEasing" "linear"
