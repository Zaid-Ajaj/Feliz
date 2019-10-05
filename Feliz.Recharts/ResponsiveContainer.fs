namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop
open Feliz.Styles

[<Erase>]
type responsiveContainer =
    /// The percentage value of the chart's width or a fixed width. Default is 100%.
    static member inline width (value: int) = Interop.mkAttr "width" value
    /// The percentage value of the chart's width or a fixed width. Default is 100%.
    static member inline width (value: float) = Interop.mkAttr "width" value
    /// The percentage value of the chart's width or a fixed width. Default is 100%.
    static member inline width (value: ICssUnit) = Interop.mkAttr "width" value
    /// The percentage value of the chart's width or a fixed height. Default is 100%.
    static member inline height (value: int) = Interop.mkAttr "height" value
    /// The percentage value of the chart's width or a fixed height. Default is 100%.
    static member inline height (value: float) = Interop.mkAttr "height" value
    /// The percentage value of the chart's width or a fixed height. Default is 100%.
    static member inline height (value: ICssUnit) = Interop.mkAttr "height" value
    /// Width / height. If specified, the height will be calculated by width / aspect.
    static member inline aspect (value: float) = Interop.mkAttr "aspect" value
    /// width / height. If specified, the height will be calculated by width / aspect.
    static member inline aspect (value: int) = Interop.mkAttr "aspect" value
    /// The chart that should become responsive
    static member inline chart (chart: Fable.React.ReactElement) : IReactProperty = unbox ("children", chart)
    /// The minimum width of the container
    static member inline minWidth (value: int) = Interop.mkStyle "minWidth" value
    /// The minimum width of the container
    static member inline minWidth (value: float) = Interop.mkStyle "minWidth" value
    /// The minimum width of the container
    static member inline minWidth (value: ICssUnit) = Interop.mkStyle "minWidth" value
    /// The minimum height of the container.
    static member inline minHeight (value: int) = Interop.mkStyle "minHeight" value
    /// The minimum height of the container.
    static member inline minHeight (value: float) = Interop.mkStyle "minHeight" value
    /// The minimum height of the container.
    static member inline minHeight (value: ICssUnit) = Interop.mkStyle "minHeight" value
    /// If specified a positive number, debounced function will be used to handle the resize event.
    static member inline debounce (value: int) = Interop.mkStyle "debounce" value
    /// If specified a positive number, debounced function will be used to handle the resize event.
    static member inline debounce (value: float) = Interop.mkStyle "debounce" value
    /// If specified a positive number, debounced function will be used to handle the resize event.
    static member inline debounce (value: ICssUnit) = Interop.mkStyle "debounce" value