namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type cartesianGrid =
    static member inline strokeDasharray([<ParamArray>] values: int []) = Interop.mkAttr "strokeDasharray" (values |> Array.map string |> String.concat " ")
    static member inline stroke (value: string) = Interop.mkAttr "stroke" value
    static member inline strokeOpacity (opacity: int) = Interop.mkAttr "strokeOpacity" opacity
    static member inline strokeOpacity (opacity: float) = Interop.mkAttr "strokeOpacity" opacity
    /// When set to false, no horizontal grid lines will be drawn. Default is `true`.
    static member inline horizontal (value: bool) = Interop.mkAttr "horizontal" value
    /// When set to false, no v grid lines will be drawn. Default is `true`.
    static member inline vertical (value: bool) = Interop.mkAttr "vertical" value
    /// The x-coordinate of grid.
    static member inline x(value: int) = Interop.mkAttr "x" value
    /// The x-coordinate of grid.
    static member inline x(value: float) = Interop.mkAttr "x" value
    /// The y-coordinate of grid.
    static member inline y(value: int) = Interop.mkAttr "y" value
    /// The y-coordinate of grid.
    static member inline y(value: float) = Interop.mkAttr "y" value