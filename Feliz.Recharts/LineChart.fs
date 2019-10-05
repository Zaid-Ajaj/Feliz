namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type lineChart =
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

module lineChart =

    /// The layout of area in the chart.
    type layout =
        static member inline horizontal = Interop.mkAttr "layout" "horizontal"
        static member inline vertical = Interop.mkAttr "layout" "vertical"

