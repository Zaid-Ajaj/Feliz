namespace Feliz

open Fable.Core
open Feliz.Styles

[<Erase>]
type visibility =
    /// The element is hidden (but still takes up space).
    static member inline hidden : IVisibility = unbox "hidden"
    /// Default value. The element is visible.
    static member inline visible : IVisibility = unbox "visible"
    /// Only for table rows (`<tr>`), row groups (`<tbody>`), columns (`<col>`), column groups (`<colgroup>`). This value removes a row or column, but it does not affect the table layout. The space taken up by the row or column will be available for other content.
    ///
    /// If collapse is used on other elements, it renders as "hidden"
    static member inline collapse : IVisibility = unbox "collapse"
    /// Sets this property to its default value.
    static member inline initial : IVisibility = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IVisibility = unbox "inherit"

[<Erase>]
/// The font-weight property sets how thick or thin characters in text should be displayed.
type fontWeight =
    /// Defines normal characters. This is default.
    static member inline normal : IFontWeight = unbox "normal"
    /// Defines thick characters.
    static member inline bold : IFontWeight = unbox "bold"
    /// Defines thicker characters
    static member inline bolder : IFontWeight = unbox "bolder"
    /// Defines lighter characters.
    static member inline lighter : IFontWeight = unbox "lighter"
    /// Sets this property to its default value.
    static member inline initial : IFontWeight = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IFontWeight = unbox "inherit"

[<Erase>]
type fontStyle =
    /// The browser displays a normal font style. This is defaut.
    static member inline normal : IFontStyle = unbox "normal"
    /// The browser displays an italic font style.
    static member inline italic : IFontStyle = unbox "italic"
    /// The browser displays an oblique font style.
    static member inline oblique : IFontStyle = unbox "oblique"
    /// Sets this property to its default value.
    static member inline initial : IFontStyle = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IFontStyle = unbox "inherit"

[<Erase>]
type fontVariant =
    /// The browser displays a normal font. This is default
    static member inline normal : IFontVariant = unbox "normal"
    /// The browser displays a small-caps font
    static member inline smallCaps : IFontVariant = unbox "small-caps"
    /// Sets this property to its default value.
    static member inline initial : IFontVariant = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IFontVariant = unbox "inherit"

[<Erase>]
type fontStretch =
    /// Makes the text as narrow as it gets.
    static member inline ultraCondensed : IFontStretch = unbox "ultra-condensed"
    /// Makes the text narrower than condensed, but not as narrow as ultra-condensed
    static member inline extraCondensed : IFontStretch = unbox "extra-condensed"
    /// Makes the text narrower than semi-condensed, but not as narrow as extra-condensed.
    static member inline condensed : IFontStretch = unbox "condensed"
    /// Makes the text narrower than normal, but not as narrow as condensed.
    static member inline semiCondensed : IFontStretch = unbox "semi-condensed"
    /// Default value. No font stretching
    static member inline normal : IFontStretch = unbox "normal"
    /// Makes the text wider than normal, but not as wide as expanded
    static member inline semiExpanded : IFontStretch = unbox "semi-expanded"
    /// Makes the text wider than semi-expanded, but not as wide as extra-expanded
    static member inline expanded : IFontStretch = unbox "expanded"
    /// Makes the text wider than expanded, but not as wide as ultra-expanded
    static member inline extraExpanded : IFontStretch = unbox "extra-expanded"
    /// Makes the text as wide as it gets.
    static member inline ultraExpanded : IFontStretch = unbox "ultra-expanded"
    /// Sets this property to its default value.
    static member inline initial : IFontStretch = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IFontStretch = unbox "inherit"

[<Erase>]
type fontKerning =
    /// Default. The browser determines whether font kerning should be applied or not
    static member inline auto : IFontKerning = unbox "auto"
    /// Specifies that font kerning is applied
    static member inline normal : IFontKerning = unbox "normal"
    /// Specifies that font kerning is not applied
    static member inline none : IFontKerning = unbox "none"

[<Erase>]
/// Specifies a number of specialized CSS units
type length =
    /// Pixels are (1px = 1/96th of 1in).
    ///
    /// **Note**: Pixels (px) are relative to the viewing device. For low-dpi devices, 1px is one device pixel (dot) of the display. For printers and high resolution screens 1px implies multiple device pixels.
    static member inline px(value: int) : ICssUnit = unbox ((unbox<string>value) + "px")
    static member inline px(value: double) : ICssUnit = unbox ((unbox<string>value) + "px")
    /// Centimeters
    static member inline cm(value: int) : ICssUnit = unbox ((unbox<string>value) + "cm")
    /// Centimeters
    static member inline cm(value: double) : ICssUnit = unbox ((unbox<string>value) + "cm")
    /// Millimeters
    static member inline mm(value: int) : ICssUnit = unbox ((unbox<string>value) + "mm")
    /// Millimeters
    static member inline mm(value: double) : ICssUnit = unbox ((unbox<string>value) + "mm")
    /// Inches (1in = 96px = 2.54cm)
    static member inline inch(value: int) : ICssUnit = unbox ((unbox<string>value) + "in")
    /// Inches (1in = 96px = 2.54cm)
    static member inline inch(value: double) : ICssUnit = unbox ((unbox<string>value) + "in")
    /// Points (1pt = 1/72 of 1in)
    static member inline pt(value: int) : ICssUnit = unbox ((unbox<string>value) + "pt")
    /// Points (1pt = 1/72 of 1in)
    static member inline pt(value: double) : ICssUnit = unbox ((unbox<string>value) + "pt")
    /// Picas (1pc = 12 pt)
    static member inline pc(value: int) : ICssUnit = unbox ((unbox<string>value) + "pc")
    /// Picas (1pc = 12 pt)
    static member inline pc(value: double) : ICssUnit = unbox ((unbox<string>value) + "pc")
    /// Relative to the font-size of the element (2em means 2 times the size of the current font
    static member inline em(value: int) : ICssUnit = unbox ((unbox<string>value) + "em")
    /// Relative to the font-size of the element (2em means 2 times the size of the current font
    static member inline em(value: double) : ICssUnit = unbox ((unbox<string>value) + "em")
    /// Relative to the x-height of the current font (rarely used)
    static member inline ex(value: int) : ICssUnit = unbox ((unbox<string>value) + "ex")
    /// Relative to the x-height of the current font (rarely used)
    static member inline ex(value: double) : ICssUnit = unbox ((unbox<string>value) + "ex")
    static member inline ch(value: int) : ICssUnit = unbox ((unbox<string>value) + "ch")
    /// Relative to font-size of the root element
    static member inline rem(value: double) : ICssUnit = unbox ((unbox<string>value) + "rem")
    /// Relative to font-size of the root element
    static member inline rem(value: int) : ICssUnit = unbox ((unbox<string>value) + "rem")
    /// Relative to 1% of the height of the viewport*
    ///
    /// **Viewport** = the browser window size. If the viewport is 50cm wide, 1vw = 0.5cm.
    static member inline vh(value: int) : ICssUnit = unbox ((unbox<string>value) + "vh")
    /// Relative to 1% of the height of the viewport*
    ///
    /// **Viewport** = the browser window size. If the viewport is 50cm wide, 1vw = 0.5cm.
    static member inline vh(value: double) : ICssUnit = unbox ((unbox<string>value) + "vh")
    /// Relative to 1% of the width of the viewport*
    ///
    /// **Viewport** = the browser window size. If the viewport is 50cm wide, 1vw = 0.5cm.
    static member inline vw(value: int) : ICssUnit = unbox ((unbox<string>value) + "vw")
    /// Relative to 1% of the width of the viewport*
    ///
    /// **Viewport** = the browser window size. If the viewport is 50cm wide, 1vw = 0.5cm.
    static member inline vw(value: double) : ICssUnit = unbox ((unbox<string>value) + "vw")
    /// Relative to 1% of viewport's smaller dimension
    static member inline vmin(value: double) : ICssUnit = unbox ((unbox<string>value) + "vmin")
    /// Relative to 1% of viewport's smaller dimension
    static member inline vmin(value: int) : ICssUnit = unbox ((unbox<string>value) + "vmin")
    /// Relative to 1% of viewport's larger dimension
    static member inline vmax(value: double) : ICssUnit = unbox ((unbox<string>value) + "vmax")
    /// Relative to 1% of viewport's* larger dimension
    static member inline vmax(value: int) : ICssUnit = unbox ((unbox<string>value) + "vmax")
    /// Relative to the parent element
    static member inline perc(value: int) : ICssUnit = unbox ((unbox<string>value) + "%")
    /// Relative to the parent element
    static member inline perc(value: double) : ICssUnit = unbox ((unbox<string>value) + "%")
    /// Relative to the parent element
    static member inline percent(value: int) : ICssUnit = unbox ((unbox<string>value) + "%")
    /// Relative to the parent element
    static member inline percent(value: double) : ICssUnit = unbox ((unbox<string>value) + "%")
    static member inline auto : ICssUnit = unbox "auto"

[<Erase>]
type overflow =
    /// The content is not clipped, and it may be rendered outside the left and right edges. This is default.
    static member inline visible : IOverflow = unbox "visibile"
    /// The content is clipped - and no scrolling mechanism is provided.
    static member inline hidden : IOverflow = unbox "hidden"
    /// The content is clipped and a scrolling mechanism is provided.
    static member inline scroll : IOverflow = unbox "scroll"
    /// Should cause a scrolling mechanism to be provided for overflowing boxes
    static member inline auto : IOverflow = unbox "auto"
    /// Sets this property to its default value.
    static member inline initial : IOverflow = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IOverflow = unbox "inherit"

[<Erase>]
type wordWrap =
    /// Break words only at allowed break points
    static member inline normal : IWordWrap = unbox "normal"
    /// Allows unbreakable words to be broken
    static member inline breakWord : IWordWrap = unbox "break-word"
    /// Sets this property to its default value.
    static member inline initial : IWordWrap = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IWordWrap = unbox "inherit"

[<Erase>]
type backgroundRepeat =
    /// The background image is repeated both vertically and horizontally. This is default.
    static member inline repeat : IBackgroundRepeat = unbox "repeat"
    /// The background image is only repeated horizontally.
    static member inline repeatX : IBackgroundRepeat = unbox "repeat-x"
    /// The background image is only repeated vertically.
    static member inline repeatY : IBackgroundRepeat = unbox "repeat-y"
    /// The background-image is not repeated.
    static member inline noRepeat : IBackgroundRepeat = unbox "no-repeat"
    /// Sets this property to its default value.
    static member inline initial : IBackgroundRepeat = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IBackgroundRepeat = unbox "inherit"

[<Erase>]
type backgroundClip =
    /// Default value. The background is clipped to the border box.
    static member inline borderBox : IBackgroundClip = unbox "border-box"
    /// The background is clipped to the padding box.
    static member inline paddingBox : IBackgroundClip = unbox "padding-box"
    /// The background is clipped to the content box
    static member inline contentBox : IBackgroundClip = unbox "content-box"
    /// Sets this property to its default value.
    static member inline initial : IBackgroundClip = unbox "initial"
    /// Inherits this property from its parent element.
    static member inline inheritFromParent : IBackgroundClip = unbox "inherit"


[<Erase>]
type style =
    /// The zIndex property sets or returns the stack order of a positioned element.
    ///
    /// An element with greater stack order (1) is always in front of another element with lower stack order (0).
    ///
    /// **Tip**: A positioned element is an element with the position property set to: relative, absolute, or fixed.
    ///
    /// **Tip**: This property is useful if you want to create overlapping elements.
    static member inline zIndex(value: int) = Interop.mkStyle "zIndex" value
    static member inline margin(value: int) = Interop.mkStyle "margin" value
    static member inline margin(top: int, right: int) = Interop.mkStyle "margin" (sprintf "%dpx %dpx" top right)
    static member inline margin(top: int, right: int, bottom: int) = Interop.mkStyle "margin" (sprintf "%dpx %dpx %dpx" top right bottom)
    static member inline margin(top: int, right: int, bottom: int, left: int) = Interop.mkStyle "margin" (sprintf "%dpx %dpx %dpx %dpx" top right bottom left)
    static member inline margin(value: ICssUnit) = Interop.mkStyle "margin" value
    static member inline marginLeft(value: int) = Interop.mkStyle "marginLeft" value
    static member inline marginLeft(value: ICssUnit) = Interop.mkStyle "marginLeft" value
    static member inline marginRight(value: int) = Interop.mkStyle "marginRight" value
    static member inline marginRight(value: ICssUnit) = Interop.mkStyle "marginRight" value
    static member inline marginTop(value: int) = Interop.mkStyle "marginTop" value
    static member inline marginTop(value: ICssUnit) = Interop.mkStyle "marginTop" value
    static member inline marginBottom(value: int) = Interop.mkStyle "marginBottom" value
    static member inline marginBottom(value: ICssUnit) = Interop.mkStyle "marginBottom" value
    static member inline padding(value: int) = Interop.mkStyle "padding" value
    static member inline padding(top: int, right: int) = Interop.mkStyle "padding" (sprintf "%dpx %dpx" top right)
    static member inline padding(top: int, right: int, bottom: int) = Interop.mkStyle "padding" (sprintf "%dpx %dpx %dpx" top right bottom)
    static member inline padding(top: int, right: int, bottom: int, left: int) = Interop.mkStyle "padding" (sprintf "%dpx %dpx %dpx %dpx" top right bottom left)
    static member inline padding(value: ICssUnit) = Interop.mkStyle "padding" value
    static member inline paddingBottom(value: int) = Interop.mkStyle "paddingBottom" value
    static member inline paddingBottom(value: ICssUnit) = Interop.mkStyle "paddingBottom" value
    static member inline paddingLeft(value: int) = Interop.mkStyle "paddingLeft" value
    static member inline paddingLeft(value: ICssUnit) = Interop.mkStyle "paddingLeft" value
    static member inline paddingRight(value: int) = Interop.mkStyle "paddingRight" value
    static member inline paddingRight(value: ICssUnit) = Interop.mkStyle "paddingRight" value
    static member inline paddingTop(value: int) = Interop.mkStyle "paddingTop" value
    static member inline paddingTop(value: ICssUnit) = Interop.mkStyle "paddingTop" value
    static member inline display(options: IDisplay) = Interop.mkStyle "display" options
    static member inline fontSize(size: int) = Interop.mkStyle "fontSize" size
    static member inline fontSize(size: ICssUnit) = Interop.mkStyle "fontSize" size
    static member inline wordWrap (style: IWordWrap) = Interop.mkStyle "wordWrap" style
    static member inline alignContent(option: IAlignContent) = Interop.mkStyle "alignContent" option
    static member inline backgroundColor (color: string) = Interop.mkStyle "backgroundColor" color
    static member inline color (color: string) = Interop.mkStyle "color" color
    static member inline top(value: int) = Interop.mkStyle "top" value
    static member inline top(value: ICssUnit) = Interop.mkStyle "top" value
    static member inline bottom(value: int) = Interop.mkStyle "bottom" value
    static member inline bottom(value: ICssUnit) = Interop.mkStyle "bottom" value
    static member inline left(value: int) = Interop.mkStyle "left" value
    static member inline left(value: ICssUnit) = Interop.mkStyle "left" value
    static member inline right(value: int) = Interop.mkStyle "right" value
    static member inline right(value: ICssUnit) = Interop.mkStyle "right" value
    static member inline overflow(value: IOverflow) = Interop.mkStyle "overflow" value
    static member inline overflowX(value: IOverflow) = Interop.mkStyle "overflowX" value
    static member inline overflowY(value: IOverflow) = Interop.mkStyle "overflowY" value
    static member inline custom(key: string, value: 't) = Interop.mkStyle key value
    static member inline border(width: int, style: IBorderStyle, color: string) = Interop.mkStyle "border" (sprintf "%dpx %s %s" width (unbox style) color)
    static member inline border(width: ICssUnit, style: IBorderStyle, color: string) = Interop.mkStyle "border" (sprintf "%dpx %s %s" (unbox width) (unbox style) color)
    static member inline border(width: string, style: IBorderStyle, color: string) = Interop.mkStyle "border" (sprintf "%s %s %s" width (unbox style) color)
    static member inline borderWidth (width: int) = Interop.mkStyle "borderWidth" width
    static member inline borderStyle (style: IBorderStyle) = Interop.mkStyle "borderWidth" style
    static member inline borderColor (color: string) = Interop.mkStyle "borderColor" color
    static member inline borderRadius (radius: int) = Interop.mkStyle "borderRadius" radius
    static member inline fontStyle (style: IFontStyle) = Interop.mkStyle "fontStyle" style
    /// The `font-kerning` property controls the usage of the kerning information stored in a font.
    ///
    /// **Tip**: Kerning defines how letters are spaced.
    ///
    /// **Note**: For fonts that do not include kerning data, this property will have no visible effect.
    static member inline fontKerning (kerning: IFontKerning) = Interop.mkStyle "fontKerning" kerning
    static member inline fontFamily (family: string) = Interop.mkStyle "fontFamily" family
    static member inline fontVariant (variant: IFontVariant) = Interop.mkStyle "fontVariant" variant
    static member inline fontWeight (weight: IFontWeight) = Interop.mkStyle "fontWeight" weight
    /// Defines from thin to thick characters. 400 is the same as normal, and 700 is the same as bold.
    /// Possible values are [100, 200, 300, 400, 500, 600, 700, 800, 900]
    static member inline fontWeight (weight: int) = Interop.mkStyle "fontWeight" weight
    /// The `font-stretch` property allows you to make text wider or narrower.
    ///
    /// Note: The `font-stretch` property will not work on just any font! It will only work if the font family has width-variant faces. The font-stretch property itself does not stretch a font.
    static member inline fontStretch (stretch: IFontStretch) = Interop.mkStyle "fontStretch" stretch
    static member inline borderRadius (radius: ICssUnit) = Interop.mkStyle "borderRadius" radius
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
    static member inline textDecorationStyle(style: ITextDecoration) = Interop.mkStyle "textDecorationStyle" style
    static member inline textDecorationColor(color: string) = Interop.mkStyle "textDecorationColor" color
    static member inline textDecorationLine(line: ITextDecorationLine) = Interop.mkStyle "textDecorationLine" line
    static member inline textDecoration(line: ITextDecorationLine) = Interop.mkStyle "textDecoration" line
    static member inline textDecoration(bottom: ITextDecorationLine, top: ITextDecorationLine) = Interop.mkStyle "textDecoration" (sprintf "%s %s" (unbox bottom) (unbox top))
    static member inline textDecoration(bottom: ITextDecorationLine, top: ITextDecorationLine, style: ITextDecoration) =
        let value = sprintf "%s %s %s" (unbox bottom) (unbox top) (unbox style)
        Interop.mkStyle "textDecoration" value
    static member inline textDecoration(bottom: ITextDecorationLine, top: ITextDecorationLine, style: ITextDecoration, color: string) =
        let value = sprintf "%s %s %s %s" (unbox bottom) (unbox top) (unbox style) color
        Interop.mkStyle "textDecoration" value
    static member inline textIndent(value: int) = Interop.mkStyle "textIndent" value
    static member inline textIndent(value: string) = Interop.mkStyle "textIndent" value
    /// The `visibility` property specifies whether or not an element is visible.
    static member inline visibility(value: IVisibility) = Interop.mkStyle "visibility" value
    static member inline position(value: IPosition) = Interop.mkStyle "position" value
    static member inline opacity(value: double) = Interop.mkStyle "opacity" value
    static member inline minWidth (value: int) = Interop.mkStyle "minWidth" value
    static member inline minWidth (value: ICssUnit) = Interop.mkStyle "minWidth" value
    static member inline backgroundPosition  (position: string) = Interop.mkStyle "backgroundPosition" position
    static member inline cursor (value: string) = Interop.mkStyle "cursor" value
    static member inline minWidth (value: string) = Interop.mkStyle "minWidth" value
    static member inline minHeight (value: int) = Interop.mkStyle "minHeight" value
    static member inline minHeight (value: ICssUnit) = Interop.mkStyle "minHeight" value
    static member inline maxWidth (value: int) = Interop.mkStyle "maxWidth" value
    static member inline maxWidth (value: ICssUnit) = Interop.mkStyle "maxWidth" value
    static member inline maxHeight (value: int) = Interop.mkStyle "maxHeight" value
    static member inline maxHeight (value: ICssUnit) = Interop.mkStyle "maxHeight" value
    static member inline height (value: int) = Interop.mkStyle "height" value
    static member inline height (value: ICssUnit) = Interop.mkStyle "height" value
    static member inline width (value: int) = Interop.mkStyle "width" value
    static member inline width (value: ICssUnit) = Interop.mkStyle "width" value
    static member inline backgroundSize (value: string) = Interop.mkStyle "backgroundSize" value
    static member inline backgroundImage (value: string) = Interop.mkStyle "backgroundImage" value
    static member inline backgroundImageUrl (value: string) = Interop.mkStyle "backgroundImage" ("url(" + value + ")")
    static member inline backgroundRepeat (repeat: IBackgroundRepeat) = Interop.mkStyle "backgroundRepeat" repeat
    static member inline backgroundClip (clip: IBackgroundClip) = Interop.mkStyle "backgroundClip" clip
    static member inline alignItems(alignment: IAlignItems) = Interop.mkStyle "alignItems" alignment
    static member inline alignSelf(alignment: IAlignSelf) = Interop.mkStyle "alignSelf" alignment
