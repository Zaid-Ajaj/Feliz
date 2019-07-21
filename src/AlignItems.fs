namespace Feliz

open Fable.Core

[<Erase>]
type alignItems =
    /// Default. Items are stretched to fit the container
    static member inline stretch : IAlignItemStyle = unbox "stretch"
    /// Items are positioned at the center of the container
    static member inline center : IAlignItemStyle = unbox "center"
    /// Items are positioned at the beginning of the container
    static member inline flexStart : IAlignItemStyle = unbox "flex-start"
    /// Items are positioned at the end of the container
    static member inline flexEnd : IAlignItemStyle = unbox "flex-end"
    /// Items are positioned at the baseline of the container
    static member inline baseline : IAlignItemStyle = unbox "baseline"
    /// Sets this property to its default value
    static member inline initial : IAlignItemStyle = unbox "initial"
    /// Inherits this property from its parent element
    static member inline inheritFromParent : IAlignItemStyle = unbox "inherit"
