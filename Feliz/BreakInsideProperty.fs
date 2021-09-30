namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type breakInsideStyle =
    /// Specifies the auto property.
    ///
    /// Allows, but does not force, any break (page, column, or region) to be inserted within the principal box.
    static member inline auto: IBreakInsideProperty = unbox "auto"

    /// Specifies the avoid property.
    ///
    /// Avoids any break (page, column, or region) from being inserted within the principal box.
    static member inline avoid: IBreakInsideProperty = unbox "avoid"

    /// Specifies the avoid-page property.
    ///
    /// Avoids any page break within the principal box.
    static member inline avoidPage: IBreakInsideProperty = unbox "avoid-page"

    /// Specifies the avoid-column property.
    ///
    /// Avoids any column break within the principal box.
    static member inline avoidColumn: IBreakInsideProperty = unbox "avoid-column"

    /// Specifies the avoid-region property.
    ///
    /// Avoids any region break within the principal box.
    static member inline avoidRegion: IBreakInsideProperty = unbox "avoid-region"

    /// Sets this property to its default value.
    static member inline initial: IBreakInsideProperty = unbox "initial"

    /// Inherits this property from its parent element.
    static member inline inheritFromParent: IBreakInsideProperty = unbox "inherit"

    /// Resets to its inherited value if the property naturally inherits from its parent,
    /// and to its initial value if not.
    static member inline unset: IBreakInsideProperty = unbox "unset"
