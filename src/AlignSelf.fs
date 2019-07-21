namespace Feliz

open Fable.Core

[<Erase>]
type alignSelf =
    /// Default. The element inherits its parent container's align-items property, or "stretch" if it has no parent container.
    static member inline auto = "auto"
    /// The element is positioned to fit the container
    static member inline stretch : IAlignSelfStyle = unbox "stretch"
    /// The element is positioned at the center of the container
    static member inline center : IAlignSelfStyle = unbox "center"
    /// The element is positioned at the beginning of the container
    static member inline flexStart : IAlignSelfStyle = unbox "flex-start"
    /// The element is positioned at the end of the container
    static member inline flexEnd : IAlignSelfStyle = unbox "flex-end"
    /// The element is positioned at the baseline of the container
    static member inline baseline : IAlignSelfStyle = unbox "baseline"
    /// Sets this property to its default value
    static member inline initial : IAlignSelfStyle = unbox "initial"
    /// Inherits this property from its parent element
    static member inline inheritFromParent : IAlignSelfStyle = unbox "inherit"