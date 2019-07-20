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
    static member inline inlineFlex : IDisplayStyle = unbox "inline-flex"
    static member inline inlineGrid : IDisplayStyle = unbox "inline-grid"
    static member inline inlineTable : IDisplayStyle = unbox "inline-table"
    static member inline listItem : IDisplayStyle = unbox "list-item"
