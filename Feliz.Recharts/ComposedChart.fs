namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type composedChart =
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
    /// The gap between two bar categories.
    static member inline barCategoryGap (value: int) = Interop.mkAttr "barCategoryGap" value
    /// The gap between two bar categories
    static member inline barCategoryGap (value: float) = Interop.mkAttr "barCategoryGap" value
    /// The gap between two bar categories
    static member inline barCategoryGapPercentage (value: int) = Interop.mkAttr "barCategoryGap" (unbox<string>value + "%")
    /// The gap between two bars in the same category. Default is 4.
    static member inline barGap (value: int) = Interop.mkAttr "barGap" value
    static member inline barGap (value: float) = Interop.mkAttr "barGap" value
    /// The gap between two bars in the same category.
    static member inline barGapPercentage (value: int) = Interop.mkAttr "barGap" (unbox<string>value + "%")
    static member inline barGapPercentage (value: float) = Interop.mkAttr "barGap" (unbox<string>value + "%")
    static member inline barSize (value: int) = Interop.mkAttr "barSize" value
    static member inline barSize (value: float) = Interop.mkAttr "barSize" value
    /// If false set, stacked items will be rendered left to right. If true set, stacked items will be rendered right to left. (Render direction affects SVG layering, not x position.)
    static member inline reverseStackOrder (value: bool) = Interop.mkAttr "reverseStackOrder" value

[<Erase>]
module composedChart =
    /// The layout of area in the chart.
    [<Erase>]
    type layout =
        static member inline horizontal = Interop.mkAttr "layout" "horizontal"
        static member inline vertical = Interop.mkAttr "layout" "vertical"