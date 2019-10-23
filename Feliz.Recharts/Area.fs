namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type area =
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color
    static member inline strokeWidth (width: int) = Interop.mkAttr "strokeWidth" width
    static member inline strokeOpacity (opacity: int) = Interop.mkAttr "strokeOpacity" opacity
    static member inline strokeOpacity (opacity: float) = Interop.mkAttr "strokeOpacity" opacity
    static member inline fill (color: string) = Interop.mkAttr "fill" color
    static member inline fillOpacity (value: int) = Interop.mkAttr "fillOpacity" value
    static member inline fillOpacity (value: float) = Interop.mkAttr "fillOpacity" value
    /// Whether to connect a graph area across null points. Default is `false`.
    static member inline connectNulls (value: bool) = Interop.mkAttr "connectNulls" value
    /// The unit of data. This option will be used in tooltip.
    static member inline unit (value: string) = Interop.mkAttr "unit" value
    /// The name of data. This option will be used in tooltip and legend to represent a area. If no value was set to this option, the value of `dataKey` will be used alternatively.
    static member inline name (value: string) = Interop.mkAttr "name" value
    /// The unique id of this component, which will be used to generate unique clip path id internally.
    static member inline id(value: string) = Interop.mkAttr "id" value
    /// If set false, animation of area will be disabled.
    static member inline isAnimationActive (value: bool) = Interop.mkAttr "isAnimationActive" value
    /// Specifies when the animation should begin, the unit of this option is ms.
    static member inline animationBegin (value: int) = Interop.mkAttr "animationBegin" value
    /// Specifies the duration of animation, the unit of this option is ms. Default is `1500ms`.
    static member inline animationDuration (value: int) = Interop.mkAttr "animationDuration" value
    /// Specifies the duration of animation. Default is `1500ms`.
    static member inline animationDuration (value: TimeSpan) = Interop.mkAttr "animationDuration" value.TotalMilliseconds
    static member inline strokeDasharray([<ParamArray>] values: int []) = Interop.mkAttr "strokeDasharray" (values |> Array.map string |> String.concat " ")
    static member inline monotone = Interop.mkAttr "type" "monotone"
    static member inline basis = Interop.mkAttr "type" "basis"
    static member inline basisClosed = Interop.mkAttr "type" "basisClosed"
    static member inline basisOpen = Interop.mkAttr "type" "basisOpen"
    static member inline linear = Interop.mkAttr "type" "linear"
    static member inline linearClosed = Interop.mkAttr "type" "linearClosed"
    static member inline natural = Interop.mkAttr "type" "natural"
    static member inline monotoneX = Interop.mkAttr "type" "monotoneX"
    static member inline step = Interop.mkAttr "type" "step"
    static member inline stepBefore = Interop.mkAttr "type" "stepBefore"
    static member inline stepAfter = Interop.mkAttr "type" "stepAfter"
    static member inline monotoneY = Interop.mkAttr "type" "monotoneY"
    static member inline stackId(value: string) = Interop.mkAttr "stackId" value

[<Erase>]
module area =
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
