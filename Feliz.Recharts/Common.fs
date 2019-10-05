namespace Feliz.Recharts

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type xAxis =
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
    static member inline hide (value: bool) = Interop.mkAttr "hide" value
    static member inline xAxisId (value: int) = Interop.mkAttr "xAxisId" value
    static member inline xAxisId (value: string) = Interop.mkAttr "xAxisId" value

[<Erase>]
type yAxis =
    static member inline dataKey (value: string) = Interop.mkAttr "dataKey" value
    static member inline dataKey (f: 'a -> string) = Interop.mkAttr "dataKey" (f (unbox null))
    static member inline hide (value: bool) = Interop.mkAttr "hide" value
    static member inline yAxisId (value: int) = Interop.mkAttr "yAxisId" value
    static member inline yAxisId (value: string) = Interop.mkAttr "yAxisId" value
