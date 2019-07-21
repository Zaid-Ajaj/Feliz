namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type position =

    /// Default value. Elements render in order, as they appear in the document flow.
    static member inline defaultStatic : IPosition = unbox "static"
    /// The element is positioned relative to its first positioned (not static) ancestor element.
    static member inline absolute : IPosition = unbox "absolute"
    /// The element is positioned relative to the browser window
    static member inline fixedRelativeToWindow : IPosition = unbox "fixed"
    /// The element is positioned relative to its normal position, so "left:20px" adds 20 pixels to the element's LEFT position.
    static member inline relative : IPosition = unbox "relative"
    /// The element is positioned based on the user's scroll position
    ///
    /// A sticky element toggles between relative and fixed, depending on the scroll position. It is positioned relative until a given offset position is met in the viewport - then it "sticks" in place (like position:fixed).
    ///
    /// Note: Not supported in IE/Edge 15 or earlier. Supported in Safari from version 6.1 with a -webkit- prefix.
    static member inline sticky : IPosition = unbox "sticky"
    static member inline initial : IPosition = unbox "initial"
    static member inline inheritFromParent : IPosition = unbox "inherit"
