namespace Feliz.Recharts

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Label =
    [<StringEnum;RequireQualifiedAccess>]
    type Position =
        | Top
        | Left
        | Right
        | Bottom
        | Inside
        | Outside
        | InsideLeft
        | InsideRight
        | InsideTop
        | InsideBottom
        | InsideTopLeft
        | InsideBottomLeft
        | InsideTopRight
        | InsideBottomRight
        | InsideStart
        | InsideEnd
        | End
        | Center