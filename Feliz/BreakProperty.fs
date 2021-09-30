namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type breakStyle =
    /// Specifies the auto property.
    ///
    /// Allows, but does not force, any break (page, column, or region) to be inserted right after the principal box.
    static member inline auto: IBreakProperty = unbox "auto"

    /// Specifies the avoid property.
    ///
    /// Avoids any break (page, column, or region) from being inserted right after the principal box.
    static member inline avoid: IBreakProperty = unbox "avoid"

    /// Specifies the always property.
    ///
    /// Avoids any break (page, column, or region) from being inserted right after the principal box.
    static member inline always: IBreakProperty = unbox "always"

    /// Specifies the all property.
    ///
    /// Forces a page break right after the principal box. Breaking through all possible fragmentation contexts. So a break inside a multicol container, which was inside a page container would force a column and page break.
    static member inline all: IBreakProperty = unbox "all"

    /// Specifies the avoid-page property.
    ///
    /// Avoids any page break right after the principal box.
    static member inline avoidPage: IBreakProperty = unbox "avoid-page"

    /// Specifies the page property.
    ///
    /// Forces a page break right after the principal box.
    static member inline page: IBreakProperty = unbox "page"

    /// Specifies the left property.
    ///
    /// Forces one or two page breaks right after the principal box, whichever will make the next page into a left page.
    static member inline left: IBreakProperty = unbox "left"

    /// Specifies the right property.
    ///
    /// Forces one or two page breaks right after the principal box, whichever will make the next page into a right page.
    static member inline right: IBreakProperty = unbox "right"

    /// Specifies the recto property.
    ///
    /// Forces one or two page breaks right after the principal box, whichever will make the next page into a recto page. (A recto page is a right page in a left-to-right spread or a left page in a right-to-left spread.)
    static member inline recto: IBreakProperty = unbox "recto"

    /// Specifies the verso property.
    ///
    /// Forces one or two page breaks right after the principal box, whichever will make the next page into a verso page. (A verso page is a left page in a left-to-right spread or a right page in a right-to-left spread.)
    static member inline verso: IBreakProperty = unbox "verso"

    /// Specifies the avoid-column property.
    ///
    /// Avoids any column break right after the principal box.
    static member inline avoidColumn: IBreakProperty = unbox "avoid-column"

    /// Specifies the column property.
    ///
    /// Forces a column break right after the principal box.
    static member inline column: IBreakProperty = unbox "column"

    /// Specifies the avoid-region property.
    ///
    /// Avoids any region break right after the principal box.
    static member inline avoidRegion: IBreakProperty = unbox "avoid-region"

    /// Specifies the region property.
    ///
    /// Forces a region break right after the principal box.
    static member inline region: IBreakProperty = unbox "region"

    /// Sets this property to its default value.
    static member inline initial: IBreakInsideProperty = unbox "initial"

    /// Inherits this property from its parent element.
    static member inline inheritFromParent: IBreakInsideProperty = unbox "inherit"

    /// Resets to its inherited value if the property naturally inherits from its parent,
    /// and to its initial value if not.
    static member inline unset: IBreakInsideProperty = unbox "unset"
