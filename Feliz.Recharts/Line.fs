namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

type line =
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





