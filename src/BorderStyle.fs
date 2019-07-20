namespace Feliz

type borderStyle() =
    /// Specifies a dotted border.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline dotted : IBorderStyle = unbox "dotted"
    /// Specifies a dashed border.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline dashed : IBorderStyle = unbox "dashed"
    /// Specifies a solid border.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline solid : IBorderStyle = unbox "solid"
    /// Specifies a double border.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline double : IBorderStyle = unbox "double"
    /// Specifies a 3D grooved border. The effect depends on the border-color value.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline groove : IBorderStyle = unbox "groove"
    /// Specifies a 3D ridged border. The effect depends on the border-color value.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline ridge : IBorderStyle = unbox "ridge"
    /// Specifies a 3D inset border. The effect depends on the border-color value.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline inset : IBorderStyle = unbox "inset"
    /// Specifies a 3D outset border. The effect depends on the border-color value.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline outset : IBorderStyle = unbox "outset"
    /// Default value. Specifies no border.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=dotted
    static member inline none : IBorderStyle = unbox "none"
    /// The same as "none", except in border conflict resolution for table elements.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=hidden
    static member inline hidden : IBorderStyle = unbox "hidden"
    /// Sets this property to its default value.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=hidden
    ///
    /// Read about initial value https://www.w3schools.com/cssref/css_initial.asp
    static member inline initial : IBorderStyle = unbox "initial"
    /// Inherits this property from its parent element.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_border-style&preval=hidden
    ///
    /// Read about inherit https://www.w3schools.com/cssref/css_inherit.asp
    static member inline inheritFromParent : IBorderStyle = unbox "inherit"

    static member inline multiple(top: IBorderStyle, right: IBorderStyle) : IBorderStyle =
        sprintf "%s %s" (unbox top) (unbox right)
        |> unbox
    static member inline multiple(top: IBorderStyle, right: IBorderStyle, bottom: IBorderStyle) : IBorderStyle =
        sprintf "%s %s %s" (unbox top) (unbox right) (unbox bottom)
        |> unbox

    static member inline multiple(top: IBorderStyle, right: IBorderStyle, bottom: IBorderStyle, left: IBorderStyle) : IBorderStyle =
        sprintf "%s %s %s %s" (unbox top) (unbox right) (unbox bottom) (unbox left)
        |> unbox