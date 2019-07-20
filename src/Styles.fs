namespace Feliz

type visibility() =
    /// The element is hidden (but still takes up space).
    static member inline hidden : IVisibilityStyle = unbox "hidden"
    /// Default value. The element is visible.
    static member inline visible : IVisibilityStyle = unbox "visible"
    /// Only for table rows (`<tr>`), row groups (`<tbody>`), columns (`<col>`), column groups (`<colgroup>`). This value removes a row or column, but it does not affect the table layout. The space taken up by the row or column will be available for other content.
    ///
    /// If collapse is used on other elements, it renders as "hidden"
    static member inline collapse : IVisibilityStyle = unbox "collapse"
    /// Sets this property to its default value.
    static member inline initial : IVisibilityStyle = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IVisibilityStyle = unbox "inherit"

type style() =
    static member inline margin(value: int) = Interop.mkStyle "margin" value
    static member inline margin(top: int, right: int) = Interop.mkStyle "margin" (sprintf "%dpx %dpx" top right)
    static member inline margin(top: int, right: int, bottom: int) = Interop.mkStyle "margin" (sprintf "%dpx %dpx %dpx" top right bottom)
    static member inline margin(top: int, right: int, bottom: int, left: int) = Interop.mkStyle "margin" (sprintf "%dpx %dpx %dpx %dpx" top right bottom left)
    static member inline margin(value: string) = Interop.mkStyle "margin" value
    static member inline marginLeft(value: int) = Interop.mkStyle "marginLeft" value
    static member inline marginLeft(value: string) = Interop.mkStyle "marginLeft" value
    static member inline marginRight(value: int) = Interop.mkStyle "marginRight" value
    static member inline marginRight(value: string) = Interop.mkStyle "marginRight" value
    static member inline marginTop(value: int) = Interop.mkStyle "marginTop" value
    static member inline marginTop(value: string) = Interop.mkStyle "marginTop" value
    static member inline marginBottom(value: int) = Interop.mkStyle "marginBottom" value
    static member inline marginBottom(value: string) = Interop.mkStyle "marginBottom" value
    static member inline padding(value: int) = Interop.mkStyle "padding" value
    static member inline padding(value: string) = Interop.mkStyle "padding" value
    static member inline paddingLeft(value: string) = Interop.mkStyle "paddingLeft" value
    static member inline paddingRight(value: string) = Interop.mkStyle "paddingRight" value
    static member inline paddingTop(value: string) = Interop.mkStyle "paddingTop" value
    static member inline paddingBottom(value: string) = Interop.mkStyle "paddingBottom" value
    static member inline paddingLeft(value: int) = Interop.mkStyle "paddingLeft" value
    static member inline paddingRight(value: int) = Interop.mkStyle "paddingRight" value
    static member inline paddingTop(value: int) = Interop.mkStyle "paddingTop" value
    static member inline paddingBottom(value: int) = Interop.mkStyle "paddingBottom" value
    static member inline display(options: IDisplayStyle) = Interop.mkStyle "display" options
    static member inline fontSize(size: int) = Interop.mkStyle "fontSize" size
    static member inline fontSize(size: string) = Interop.mkStyle "fontSize" size
    static member inline alignContent(option: IAlignContentStyle) = Interop.mkStyle "alignContent" option
    static member inline backgroundColor (color: string) = Interop.mkStyle "backgroundColor" color
    static member inline color (color: string) = Interop.mkStyle "color" color
    static member inline custom(key: string, value: 't) = Interop.mkStyle key value
    static member inline border(width: int, style: IBorderStyle, color: string) = Interop.mkStyle "border" (sprintf "%dpx %s %s" width (unbox style) color)
    static member inline border(width: string, style: IBorderStyle, color: string) = Interop.mkStyle "border" (sprintf "%s %s %s" width (unbox style) color)
    static member inline borderWidth (width: int) = Interop.mkStyle "borderWidth" width
    static member inline borderStyle (style: IBorderStyle) = Interop.mkStyle "borderWidth" style
    static member inline borderColor (color: string) = Interop.mkStyle "borderColor" color
    static member inline borderRadius (radius: int) = Interop.mkStyle "borderRadius" radius
    static member inline borderRadius (radius: string) = Interop.mkStyle "borderRadius" radius
    static member inline borderWidth (top: int, right: int) =
        let value = sprintf "%dpx %dpx" top right
        Interop.mkStyle "borderWidth" value
    static member inline borderWidth (top: int, right: int, bottom: int) =
        let value = sprintf "%dpx %dpx %dpx" top right bottom
        Interop.mkStyle "borderWidth" value
    static member inline borderWidth (top: int, right: int, bottom: int, left: int) =
        let value = sprintf "%dpx %dpx %dpx %dpx" top right bottom left
        Interop.mkStyle "borderWidth" value
    static member inline textAlign(alignment: ITextAlignment) = Interop.mkStyle "textAlign" alignment
    static member inline textDecorationStyle(style: ITextDecorationStyle) = Interop.mkStyle "textDecorationStyle" style
    static member inline textDecorationColor(color: string) = Interop.mkStyle "textDecorationColor" color
    static member inline textDecorationLine(line: ITextDecorationLine) = Interop.mkStyle "textDecorationLine" line
    static member inline textDecoration(line: ITextDecorationLine) = Interop.mkStyle "textDecoration" line
    static member inline textDecoration(bottom: ITextDecorationLine, top: ITextDecorationLine) = Interop.mkStyle "textDecoration" (sprintf "%s %s" (unbox bottom) (unbox top))
    static member inline textDecoration(bottom: ITextDecorationLine, top: ITextDecorationLine, style: ITextDecorationStyle) =
        let value = sprintf "%s %s %s" (unbox bottom) (unbox top) (unbox style)
        Interop.mkStyle "textDecoration" value
    static member inline textDecoration(bottom: ITextDecorationLine, top: ITextDecorationLine, style: ITextDecorationStyle, color: string) =
        let value = sprintf "%s %s %s %s" (unbox bottom) (unbox top) (unbox style) color
        Interop.mkStyle "textDecoration" value
    static member inline textIndent(value: int) = Interop.mkStyle "textIndent" value
    static member inline textIndent(value: string) = Interop.mkStyle "textIndent" value
    /// The `visibility` property specifies whether or not an element is visible.
    static member inline visibility(value: IVisibilityStyle) = Interop.mkStyle "visibility" value
    static member inline position(value: IPositionStyle) = Interop.mkStyle "position" value
    static member inline opacity(value: double) = Interop.mkStyle "opacity" value
    static member inline minWidth (value: int) = Interop.mkStyle "minWidth" value
    static member inline minWidth (value: string) = Interop.mkStyle "minWidth" value
    static member inline minHeight (value: int) = Interop.mkStyle "minHeight" value
    static member inline minHeight (value: string) = Interop.mkStyle "minHeight" value
    static member inline maxWidth (value: int) = Interop.mkStyle "maxWidth" value
    static member inline maxWidth (value: string) = Interop.mkStyle "maxWidth" value
    static member inline maxHeight (value: int) = Interop.mkStyle "maxHeight" value
    static member inline maxHeight (value: string) = Interop.mkStyle "maxHeight" value
    static member inline height (value: int) = Interop.mkStyle "height" value
    static member inline height (value: string) = Interop.mkStyle "height" value
    static member inline width (value: int) = Interop.mkStyle "width" value
    static member inline width (value: string) = Interop.mkStyle "width" value


