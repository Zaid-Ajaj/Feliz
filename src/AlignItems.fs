namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type alignItems =
    /// Default. Items are stretched to fit the container
    static member inline stretch : IAlignItems = unbox "stretch"
    /// Items are positioned at the center of the container
    static member inline center : IAlignItems = unbox "center"
    /// Items are positioned at the beginning of the container
    static member inline flexStart : IAlignItems = unbox "flex-start"
    /// Items are positioned at the end of the container
    static member inline flexEnd : IAlignItems = unbox "flex-end"
    /// Items are positioned at the baseline of the container
    static member inline baseline : IAlignItems = unbox "baseline"
    /// Sets this property to its default value
    static member inline initial : IAlignItems = unbox "initial"
    /// Inherits this property from its parent element
    static member inline inheritFromParent : IAlignItems = unbox "inherit"
