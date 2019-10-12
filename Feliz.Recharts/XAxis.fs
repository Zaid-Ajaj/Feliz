namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type xAxis =
    /// The key of data displayed in the axis.
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    /// The key of data displayed in the axis.
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
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
