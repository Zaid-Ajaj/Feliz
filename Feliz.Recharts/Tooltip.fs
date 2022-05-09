namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop


[<Erase>]
type tooltip =
    static member inline separator (value: string) = Interop.mkTooltipAttr "separator" value
    static member inline offset (value: int) = Interop.mkTooltipAttr "offset" value
    static member inline filterNull (value: bool) = Interop.mkTooltipAttr "filterNull" value
    static member inline cursor (value: bool) = Interop.mkTooltipAttr "cursor" value
    static member inline cursor(?stroke: string, ?strokeWidth: int) =
        let cursor = createObj [
            "stroke" ==> Option.defaultValue "" stroke
            "strokeWidth" ==> Option.defaultValue 0 strokeWidth
        ]

        Interop.mkTooltipAttr "cursor" cursor
    static member inline active (value: bool) = Interop.mkTooltipAttr "active" value
    static member inline content (render: ITooltipProperty list -> ReactElement) = Interop.mkTooltipAttr "content" render
