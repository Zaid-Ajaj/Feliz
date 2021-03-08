namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop


[<Erase>]
type tooltip =
    static member inline separator (value: string) = Interop.mkAttr "separator" value
    static member inline offset (value: int) = Interop.mkAttr "offset" value
    static member inline filterNull (value: bool) = Interop.mkAttr "filterNull" value
    static member inline cursor (value: bool) = Interop.mkAttr "cursor" value
    static member inline cursor(?stroke: string, ?strokeWidth: int) =
        let cursor = createObj [
            "stroke" ==> Option.defaultValue "" stroke
            "strokeWidth" ==> Option.defaultValue 0 strokeWidth
        ]

        Interop.mkAttr "cursor" cursor
    static member inline active (value: bool) = Interop.mkAttr "active" value
    static member inline content (render: ITooltipProperty list -> ReactElement) = Interop.mkAttr "content" render
