namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type pie =
    static member inline name(value: string) = Interop.mkAttr "name" value
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> int) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> float) = Interop.mkAttr "dataKey" f
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color
    static member inline strokeWidth (width: int) = Interop.mkAttr "strokeWidth" width
    static member inline strokeOpacity (opacity: int) = Interop.mkAttr "strokeOpacity" opacity
    static member inline strokeOpacity (opacity: float) = Interop.mkAttr "strokeOpacity" opacity
    static member inline fill (color: string) = Interop.mkAttr "fill" color
    static member inline fillOpacity (value: int) = Interop.mkAttr "fillOpacity" value
    static member inline fillOpacity (value: float) = Interop.mkAttr "fillOpacity" value
    /// The source data, in which each element is an object.
    static member inline data (values: seq<'a>) = Interop.mkAttr "data" (Seq.toArray values)
    /// The source data, in which each element is an object.
    static member inline data (values: 'a list) = Interop.mkAttr "data" (List.toArray values)
    /// The source data, in which each element is an object.
    static member inline data (values: 'a array) = Interop.mkAttr "data" values
    /// The y-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container height.
    static member inline cy(value: float) = Interop.mkAttr "cy" value
    /// The y-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container height.
    static member inline cy(value: int) = Interop.mkAttr "cy" value
    /// /// The x-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container width. Default '50%'.
    static member inline cx(value: float) = Interop.mkAttr "cx" value
    /// /// The x-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container width. Default '50%'.
    static member inline cx(value: int) = Interop.mkAttr "cx" value
    static member inline innerRadius(value: float) = Interop.mkAttr "innerRadius" value
    static member inline innerRadius(value: int) = Interop.mkAttr "innerRadius" value
    static member inline outerRadius(value: float) = Interop.mkAttr "outerRadius" value
    static member inline outerRadius(value: int) = Interop.mkAttr "outerRadius" value
    static member inline startAngle (value: int) = Interop.mkAttr "startAngle" value
    static member inline startAngle (value: float) = Interop.mkAttr "startAngle" value
    static member inline endAngle (value: int) = Interop.mkAttr "endAngle" value
    static member inline endAngle (value: float) = Interop.mkAttr "endAngle" value
    static member inline minAngle (value: int) = Interop.mkAttr "minAngle" value
    static member inline minAngle (value: float) = Interop.mkAttr "minAngle" value
    /// The key of each sector's name. Default is `name`.
    static member inline nameKey (value: string) = Interop.mkAttr "nameKey" value
    static member inline label(value: bool) = Interop.mkAttr "label" value
    static member inline label(value: IPieLabelProperties -> ReactElement) = Interop.mkAttr "label" value
    static member inline labelLine(value: bool) = Interop.mkAttr "labelLine" value
    static member inline labelLine(value: IPieLabelProperties -> ReactElement) = Interop.mkAttr "labelLine" value
    static member inline activeIndex (value: int) = Interop.mkAttr "activeIndex" value
    static member inline activeIndex (value: int list) = Interop.mkAttr "activeIndex" (List.toArray value)
    static member inline activeIndex (value: int []) = Interop.mkAttr "activeIndex" value
    /// If set false, animation of area will be disabled.
    static member inline isAnimationActive (value: bool) = Interop.mkAttr "isAnimationActive" value
    /// Specifies when the animation should begin, the unit of this option is ms.
    static member inline animationBegin (value: int) = Interop.mkAttr "animationBegin" value
    /// Specifies the duration of animation, the unit of this option is ms. Default is `1500ms`.
    static member inline animationDuration (value: int) = Interop.mkAttr "animationDuration" value
    /// Specifies the duration of animation. Default is `1500ms`.
    static member inline animationDuration (value: TimeSpan) = Interop.mkAttr "animationDuration" value.TotalMilliseconds
    static member inline children (children: ReactElement list) = prop.children children
    static member inline children (children: ReactElement seq) = prop.children children

module pie =

    [<Erase>]
    type cy =
        /// The y-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container height.
        static member inline percentage(value: float) = Interop.mkAttr "cy" (unbox<string> value + "%")
        /// The y-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container height.
        static member inline percentage(value: int) = Interop.mkAttr "cy" (unbox<string> value + "%")

    [<Erase>]
    type innerRadius =
        static member inline percentage(value: float) = Interop.mkAttr "innerRadius" (unbox<string> value + "%")
        static member inline percentage(value: int) = Interop.mkAttr "innerRadius" (unbox<string> value + "%")

    [<Erase>]
    type outerRadius =
        static member inline percentage(value: float) = Interop.mkAttr "outerRadius" (unbox<string> value + "%")
        static member inline percentage(value: int) = Interop.mkAttr "outerRadius" (unbox<string> value + "%")

    [<Erase>]
    type cx =
        /// The x-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container width. Default '50%'.
        static member inline percentage(value: float) = Interop.mkAttr "cx" (unbox<string> value + "%")
        /// The x-coordinate of center. If set a percentage, the final value is obtained by multiplying the percentage of container width. Default '50%'.
        static member inline percentage(value: int) = Interop.mkAttr "cx" (unbox<string> value + "%")

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