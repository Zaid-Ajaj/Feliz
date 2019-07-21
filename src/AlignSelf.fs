namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type alignSelf =
    /// Default. The element inherits its parent container's align-items property, or "stretch" if it has no parent container.
    static member inline auto = "auto"
    /// The element is positioned to fit the container
    static member inline stretch : IAlignSelf = unbox "stretch"
    /// The element is positioned at the center of the container
    static member inline center : IAlignSelf = unbox "center"
    /// The element is positioned at the beginning of the container
    static member inline flexStart : IAlignSelf = unbox "flex-start"
    /// The element is positioned at the end of the container
    static member inline flexEnd : IAlignSelf = unbox "flex-end"
    /// The element is positioned at the baseline of the container
    static member inline baseline : IAlignSelf = unbox "baseline"
    /// Sets this property to its default value
    static member inline initial : IAlignSelf = unbox "initial"
    /// Inherits this property from its parent element
    static member inline inheritFromParent : IAlignSelf = unbox "inherit"