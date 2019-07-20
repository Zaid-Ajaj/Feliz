namespace Feliz

type alignContent() =
    /// Default value. Lines stretch to take up the remaining space.
    static member inline stretch : IAlignContentStyle = unbox "stretch"
    /// Lines are packed toward the center of the flex container.
    static member inline center : IAlignContentStyle = unbox "center"
    /// Lines are packed toward the start of the flex container.
    static member inline flexStart : IAlignContentStyle = unbox "flex-start"
    /// Lines are packed toward the end of the flex container.
    static member inline flexEnd : IAlignContentStyle = unbox "flex-end"
    /// Lines are evenly distributed in the flex container.
    static member inline spaceBetween : IAlignContentStyle = unbox "space-between"
    /// Lines are evenly distributed in the flex container, with half-size spaces on either end.
    static member inline spaceAround : IAlignContentStyle = unbox "space-around"
