namespace Feliz

type display() =
    /// Displays an element as an inline element (like `<span>`). Any height and width properties will have no effect.
    static member inline inlineElement : IDisplayStyle = unbox "inline"
    /// Displays an element as a block element (like `<p>`). It starts on a new line, and takes up the whole width.
    static member inline block : IDisplayStyle = unbox "block"
    /// Makes the container disappear, making the child elements children of the element the next level up in the DOM.
    static member inline contents : IDisplayStyle = unbox "contents"
    /// Displays an element as a block-level flex container.
    static member inline flex : IDisplayStyle = unbox "flex"
    /// Displays an element as a block-level grid container.
    static member inline grid : IDisplayStyle = unbox "grid"
    /// Displays an element as an inline-level block container. The element itself is formatted as an inline element, but you can apply height and width values.
    static member inline inlineBlock : IDisplayStyle = unbox "inline-block"
    /// Displays an element as an inline-level flex container.
    static member inline inlineFlex : IDisplayStyle = unbox "inline-flex"
    /// Displays an element as an inline-level grid container
    static member inline inlineGrid : IDisplayStyle = unbox "inline-grid"
    /// The element is displayed as an inline-level table.
    static member inline inlineTable : IDisplayStyle = unbox "inline-table"
    /// Let the element behave like a `<li>` element
    static member inline listItem : IDisplayStyle = unbox "list-item"
    /// Displays an element as either block or inline, depending on context.
    static member inline runIn : IDisplayStyle = unbox "run-in"
    /// Let the element behave like a `<table>` element.
    static member inline table : IDisplayStyle = unbox "table"
    /// Let the element behave like a <caption> element.
    static member inline tableCaption : IDisplayStyle = unbox "table-caption"
    /// Let the element behave like a <colgroup> element.
    static member inline tableColumnGroup : IDisplayStyle = unbox "table-column-group"
    /// Let the element behave like a <thead> element.
    static member inline tableHeaderGroup : IDisplayStyle = unbox "table-header-group"
    /// Let the element behave like a <tfoot> element.
    static member inline tableFooterGroup : IDisplayStyle = unbox "table-footer-group"
    /// Let the element behave like a <tbody> element.
    static member inline tableRowGroup : IDisplayStyle = unbox "table-row-group"
    /// Let the element behave like a <td> element.
    static member inline tableCell : IDisplayStyle = unbox "table-cell"
    /// Let the element behave like a <col> element.
    static member inline tableColumn : IDisplayStyle = unbox "table-column"
    /// Let the element behave like a <tr> element.
    static member inline tableRow : IDisplayStyle = unbox "table-row"
    /// The element is completely removed.
    static member inline none : IDisplayStyle = unbox "none"
    /// Sets this property to its default value.
    static member inline initial : IDisplayStyle = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IDisplayStyle = unbox "inherit"