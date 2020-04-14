namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type xAxis =
    /// The key of data displayed in the x-axis.
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> int) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> float) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> string option) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> int option) = Interop.mkAttr "dataKey" f
    static member inline dataKey (f: 'a -> float option) = Interop.mkAttr "dataKey" f
    /// If set true, the axis do not display in the chart.
    static member inline hide (value: bool) = Interop.mkAttr "hide" value
    /// The unique id of x-axis.
    static member inline xAxisId (value: int) = Interop.mkAttr "xAxisId" value
    /// The unique id of x-axis.
    static member inline xAxisId (value: string) = Interop.mkAttr "xAxisId" value
    /// Sets the type of the axis to `number`
    static member inline number = Interop.mkAttr "type" "number"
    /// Sets the type of the axis to `category`
    static member inline category = Interop.mkAttr "type" "category"
    /// If set true, flips ticks around the axis line, displaying the labels inside the chart instead of outside.
    /// Default is `false`.
    static member inline mirror (value: bool) = Interop.mkAttr "mirror" value
    /// Allow the ticks of XAxis to be decimals or not. Default is `true`.
    static member inline allowDecimals (value: bool) = Interop.mkAttr "allowDecimals" value
    /// When domain of the axis is specified and the type of the axis is 'number', if allowDataOverflow is set to be false, the domain will be adjusted when the minimum value of data is smaller than domain[0] or the maximum value of data is greater than domain[1] so that the axis displays all data values. If set to true, graphic elements (line, area, bars) will be clipped to conform to the specified domain. Default is `false`.
    static member inline allowDataOverflow (value: bool) = Interop.mkAttr "allowDataOverflow" value
    /// Allow the axis has duplicated categorys or not when the type of axis is "category".
    static member inline allowDuplicatedCategory (value: bool) = Interop.mkAttr "allowDuplicatedCategory" value
    /// The count of axis ticks. Not used if 'type' is 'category'. Default is `5`.
    static member inline tickCount (value: int) = Interop.mkAttr "tickCount" value
    static member inline domain (min : IAxisDomain, max: IAxisDomain) = Interop.mkAttr "domain" [| min; max |]
    static member inline interval (value: int) = Interop.mkAttr "interval" value
    /// Specify the padding of x-axis. Default is `xAxis.padding(left=0, right=0)`.
    static member inline padding(?top: int, ?right: int, ?left: int, ?bottom: int) =
        let padding = createObj [
            "top" ==> Option.defaultValue 0 top
            "right" ==> Option.defaultValue 0 right
            "left" ==> Option.defaultValue 0 left
            "bottom" ==> Option.defaultValue 0 bottom
        ]

        Interop.mkAttr "padding" padding
    static member inline tickMargin (value: int) = Interop.mkAttr "tickMargin" value
    static member inline tickMargin (value: float) = Interop.mkAttr "tickMargin" value
    static member inline unit (value: string) = Interop.mkAttr "unit" value
    static member inline name (value: string) = Interop.mkAttr "name" value
    static member inline tick (tickRenderer : IXAxisTickProperties -> ReactElement) = Interop.mkAttr "tick" tickRenderer

module xAxis =
    [<Erase>]
    /// The orientation of axis. Default is `bottom`.
    type orientation =
        static member inline top = Interop.mkAttr "orientation" "top"
        static member inline bottom = Interop.mkAttr "orientation" "bottom"

    [<Erase>]
    type interval =
        static member inline preserveStart = Interop.mkAttr "interval" "preserveStart"
        static member inline preserveEnd = Interop.mkAttr "interval" "preserveEnd"
        static member inline preserveStartEnd = Interop.mkAttr "interval" "preserveStartEnd"

    [<Erase>]
    type scale =
        static member inline auto = Interop.mkAttr "scale" "auto"
        static member inline linear = Interop.mkAttr "scale" "linear"
        static member inline pow = Interop.mkAttr "scale" "pow"
        static member inline sqrt = Interop.mkAttr "scale" "sqrt"
        static member inline log = Interop.mkAttr "scale" "log"
        static member inline identity = Interop.mkAttr "scale" "identity"
        static member inline time = Interop.mkAttr "scale" "time"
        static member inline band = Interop.mkAttr "scale" "band"
        static member inline point = Interop.mkAttr "scale" "point"
        static member inline ordinal = Interop.mkAttr "scale" "ordinal"
        static member inline quantile = Interop.mkAttr "scale" "quantile"
        static member inline quantize = Interop.mkAttr "scale" "quantize"
        static member inline utc = Interop.mkAttr "scale" "utc"
        static member inline sequential = Interop.mkAttr "scale" "sequential"
        static member inline threshold = Interop.mkAttr "scale" "threshold"
