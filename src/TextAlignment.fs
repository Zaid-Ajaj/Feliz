namespace Feliz

type textAlign() =
    /// Aligns the text to the left.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_text-align
    static member inline left : ITextAlignment = unbox "left"
    /// Aligns the text to the right.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_text-align&preval=right
    static member inline right : ITextAlignment = unbox "right"
    /// Centers the text.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_text-align&preval=center
    static member inline center : ITextAlignment = unbox "center"
    /// Stretches the lines so that each line has equal width (like in newspapers and magazines).
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_text-align&preval=justify
    static member inline justify : ITextAlignment = unbox "justify"
    /// Sets this property to its default value.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_text-align&preval=initial
    static member inline initial : ITextAlignment = unbox "initial"
    /// Inherits this property from its parent element.
    ///
    /// See example https://www.w3schools.com/cssref/playit.asp?filename=playcss_text-align&preval=initial
    static member inline inheritFromParent : ITextAlignment = unbox "inheritFromParent"
