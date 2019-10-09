namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type line =
    static member inline name(value: string) = Interop.mkAttr "name" value
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
    static member inline stroke (value: string) = Interop.mkAttr "stroke" value
    static member inline xAxisId (value: string) = Interop.mkAttr "xAxisId" value
    static member inline yAxisId (value: string) = Interop.mkAttr "yAxisId" value
    static member inline xAxisId (value: int) = Interop.mkAttr "xAxisId" value
    static member inline yAxisId (value: int) = Interop.mkAttr "yAxisId" value
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
    static member inline dot(value: bool) = Interop.mkAttr "dot" value
    static member inline dot(render: IDotProperties<'a> -> Fable.React.ReactElement) = Interop.mkAttr "dot" render
    static member inline activeDot(value: bool) = Interop.mkAttr "activeDot" value
    static member inline activeDot(render: IDotProperties<'a> -> Fable.React.ReactElement) = Interop.mkAttr "activeDot" render
    static member inline label(value: string) = Interop.mkAttr "label" value
    static member inline label(value: int) = Interop.mkAttr "label" value
    static member inline label(value: float) = Interop.mkAttr "label" value
    static member inline label(value: Fable.React.ReactElement) = Interop.mkAttr "label" value
    static member inline label(value: ILabelProperties -> Fable.React.ReactElement) = Interop.mkAttr "label" value
    /// If set false, animation of area will be disabled.
    static member inline isAnimationActive (value: bool) = Interop.mkAttr "isAnimationActive" value
    /// Specifies when the animation should begin, the unit of this option is ms.
    static member inline animationBegin (value: int) = Interop.mkAttr "animationBegin" value
    /// Specifies the duration of animation, the unit of this option is ms. Default is `1500ms`.
    static member inline animationDuration (value: int) = Interop.mkAttr "animationDuration" value
    /// Specifies the duration of animation. Default is `1500ms`.
    static member inline animationDuration (value: TimeSpan) = Interop.mkAttr "animationDuration" value.TotalMilliseconds

module line =
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

    /// The type of easing function. Default is `ease`.
    [<Erase>]
    type animationEasing =
        static member inline ease = Interop.mkAttr "animationEasing" "ease"
        static member inline easeIn = Interop.mkAttr "animationEasing" "ease-in"
        static member inline easeOut = Interop.mkAttr "animationEasing" "ease-out"
        static member inline easeInOut = Interop.mkAttr "animationEasing" "ease-in-out"
        static member inline linear = Interop.mkAttr "animationEasing" "linear"