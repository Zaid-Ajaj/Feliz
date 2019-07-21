namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type alignContent =
    /// Default value. Lines stretch to take up the remaining space.
    static member inline stretch : IAlignContent = unbox "stretch"
    /// Lines are packed toward the center of the flex container.
    static member inline center : IAlignContent = unbox "center"
    /// Lines are packed toward the start of the flex container.
    static member inline flexStart : IAlignContent = unbox "flex-start"
    /// Lines are packed toward the end of the flex container.
    static member inline flexEnd : IAlignContent = unbox "flex-end"
    /// Lines are evenly distributed in the flex container.
    static member inline spaceBetween : IAlignContent = unbox "space-between"
    /// Lines are evenly distributed in the flex container, with half-size spaces on either end.
    static member inline spaceAround : IAlignContent = unbox "space-around"
