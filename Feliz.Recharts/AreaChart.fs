namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type areaChart =
    /// If any two categorical charts(LineChart, AreaChart, BarChart, ComposedChart) have the same syncId, these two charts can sync the position tooltip, and the startIndex, endIndex of Brush.
    static member inline syncId (value: string) = Interop.mkAttr "syncId" value
    /// The width of chart container.
    static member inline width (value: int) = Interop.mkAttr "width" value
    /// The height of chart container.
    static member inline height (value: int) = Interop.mkAttr "height" value
    /// The source data, in which each element is an object.
    static member inline data (values: seq<'a>) = Interop.mkAttr "data" (Seq.toArray values)
    /// The source data, in which each element is an object.
    static member inline data (values: 'a list) = Interop.mkAttr "data" (List.toArray values)
    /// The source data, in which each element is an object.
    static member inline data (values: 'a array) = Interop.mkAttr "data" values
    /// The base value of area.
    static member inline baseValue (value: int) = Interop.mkAttr "baseValue" value
    /// The base value of area.
    static member inline baseValue (value: float) = Interop.mkAttr "baseValue" value
    static member inline children (elements: Fable.React.ReactElement list) = prop.children elements
    /// The sizes of whitespace around the container.
    ///
    /// Default value `{ top: 5, right: 5, bottom: 5, left: 5 }`
    static member inline margin(?top: int, ?right: int, ?left: int, ?bottom: int) =
        let margin = createObj [
            "top" ==> Option.defaultValue 0 top
            "right" ==> Option.defaultValue 0 right
            "left" ==> Option.defaultValue 0 left
            "bottom" ==> Option.defaultValue 0 bottom
        ]

        Interop.mkAttr "margin" margin

    static member inline onClick (handler: ChartMouseEvent<'label, 'payload> -> unit) =
        Interop.mkAttr "onClick" <|
            fun eventArgs ->
                if isNullOrUndefined eventArgs || Interop.objectHas [ "isTooltipActive" ] eventArgs
                then ignore()
                else handler eventArgs

    static member inline onMouseEnter (handler: ChartMouseEvent<'label, 'payload> -> unit) =
        Interop.mkAttr "onMouseEnter" <|
            fun eventArgs ->
                if isNullOrUndefined eventArgs || Interop.objectHas [ "isTooltipActive" ] eventArgs
                then ignore()
                else handler eventArgs

    static member inline onMouseMove (handler: ChartMouseEvent<'label, 'payload> -> unit) =
        Interop.mkAttr "onMouseMove" <|
            fun eventArgs ->
                if isNullOrUndefined eventArgs || Interop.objectHas [ "isTooltipActive" ] eventArgs
                then ignore()
                else handler eventArgs

    static member inline onMouseLeave (handler: unit -> unit) = Interop.mkAttr "onMouseLeave" handler
    static member inline onMouseUp (handler: ChartMouseEvent<'label, 'payload> -> unit) =
        Interop.mkAttr "onMouseUp" <|
            fun eventArgs ->
                if isNullOrUndefined eventArgs || Interop.objectHas [ "isTooltipActive" ] eventArgs
                then ignore()
                else handler eventArgs

    static member inline onMouseDown (handler: ChartMouseEvent<'label, 'payload> -> unit) =
        Interop.mkAttr "onMouseDown" <|
            fun eventArgs ->
                if isNullOrUndefined eventArgs || Interop.objectHas [ "isTooltipActive" ] eventArgs
                then ignore()
                else handler eventArgs

module areaChart =
    /// The type of offset function used to generate the lower and upper values in the series array. The four types are built-in offsets in d3-shape. Default is `none`.
    [<Erase>]
    type stackOffset =
        static member inline expand = Interop.mkAttr "stackOffset" "expand"
        static member inline none = Interop.mkAttr "stackOffset" "none"
        static member inline wiggle = Interop.mkAttr "stackOffset" "wiggle"
        static member inline silhouette = Interop.mkAttr "stackOffset" "silhouette"

    /// The base value of area. Default is `auto`.
    [<Erase>]
    type baseValue =
        static member inline dataMin = Interop.mkAttr "baseValue" "dataMin"
        static member inline dataMax = Interop.mkAttr "baseValue" "dataMax"
        static member inline auto = Interop.mkAttr "baseValue" "auto"

    /// The layout of area in the chart.
    [<Erase>]
    type layout =
        static member inline horizontal = Interop.mkAttr "layout" "horizontal"
        static member inline vertical = Interop.mkAttr "layout" "vertical"