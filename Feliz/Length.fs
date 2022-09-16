namespace Feliz

open Fable.Core
open Feliz.Styles

/// Specifies a number of specialized CSS units
[<Erase>]
type length =
    /// Pixels are (1px = 1/96th of 1in).
    ///
    /// **Note**: Pixels (px) are relative to the viewing device. For low-dpi devices, 1px is one device pixel (dot) of the display. For printers and high resolution screens 1px implies multiple device pixels.
    static member inline px(value: int) : ICssUnit = unbox ((unbox<string>value) + "px")
    /// Pixels are (1px = 1/96th of 1in).
    ///
    /// **Note**: Pixels (px) are relative to the viewing device. For low-dpi devices, 1px is one device pixel (dot) of the display. For printers and high resolution screens 1px implies multiple device pixels.
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
    /// Relative to width of the "0" (zero)
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
    /// The browser calculates the length.
    static member inline auto : ICssUnit = unbox "auto"
    /// calculated length, frequency, angle, time, percentage, number or integer
    static member inline calc(value:string) : ICssUnit = unbox ("calc(" + (unbox<string>value) + ")")
    /// Relative to width of the grid layout in correlation with the other fr's in the grid
    static member inline fr(value: int) : ICssUnit = unbox ((unbox<string>value) + "fr")
    /// Relative to width of the grid layout in correlation with the other fr's in the grid
    static member inline fr(value: double) : ICssUnit = unbox ((unbox<string>value) + "fr")
    