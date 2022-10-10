namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type labelList =
    /// Specifies data key for the label list 
    /// 
    /// ie: labelList.dataKey (fun (d:{| name : string; data : int64 |}) -> d.name)
    static member inline dataKey (f: 'a -> string) = Interop.mkLabelListAttr "dataKey" (System.Func<_,_> f)

module labelList =
    [<Erase>]
    type position =
        static member inline top = Interop.mkProperty<ILabelListProperty> "position" "top"
        static member inline left = Interop.mkProperty<ILabelListProperty> "position" "left"
        static member inline right = Interop.mkProperty<ILabelListProperty> "position" "right"
        static member inline bottom = Interop.mkProperty<ILabelListProperty> "position" "bottom"
        static member inline inside = Interop.mkProperty<ILabelListProperty> "position" "inside"
        static member inline outside = Interop.mkProperty<ILabelListProperty> "position" "outside"
        static member inline insideLeft = Interop.mkProperty<ILabelListProperty> "position" "insideLeft"
        static member inline insideRight = Interop.mkProperty<ILabelListProperty> "position" "insideRight"
        static member inline insideTop = Interop.mkProperty<ILabelListProperty> "position" "insideTop"
        static member inline insideBottom = Interop.mkProperty<ILabelListProperty> "position" "insideBottom"
        static member inline insideTopLeft = Interop.mkProperty<ILabelListProperty> "position" "insideTopLeft"
        static member inline insideBottomLeft = Interop.mkProperty<ILabelListProperty> "position" "insideBottomLeft"
        static member inline insideTopRight = Interop.mkProperty<ILabelListProperty> "position" "insideTopRight"
        static member inline insideBottomRight = Interop.mkProperty<ILabelListProperty> "position" "insideBottomRight"
        static member inline insideStart = Interop.mkProperty<ILabelListProperty> "position" "insideStart"
        static member inline insideEnd = Interop.mkProperty<ILabelListProperty> "position" "insideEnd"
        static member inline end = Interop.mkProperty<ILabelListProperty> "position" "end"
        static member inline center = Interop.mkProperty<ILabelListProperty> "position" "center"