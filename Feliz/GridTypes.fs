namespace Feliz

open System

open Fable.Core
open Feliz.Styles

module GridUtils =
    let unboxSeq (collection: 'a seq) : string =
        Seq.map unbox<string> collection |> String.concat " "

[<Erase>]
type gridColumn =
    static member inline span(value: string) : IGridSpan = unbox ("span " + value)

    static member inline span(value: string, count: int) : IGridSpan =
        unbox ("span " + value + " " + (unbox<string> count))

    static member inline span(value: int) : IGridSpan = unbox ("span " + (unbox<string> value))

[<Erase>]
type gridRow =
    static member inline span(value: string) : IGridSpan = unbox ("span " + value)

    static member inline span(value: string, count: int) : IGridSpan =
        unbox ("span " + value + " " + (unbox<string> count))

    static member inline span(value: int) : IGridSpan = unbox ("span " + (unbox<string> value))

[<Erase>]
type grid =
    static member inline namedLine(value: string) : IGridTemplateItem = unbox ("[ " + value + " ]")

    static member inline namedLines(value: string[]) : IGridTemplateItem =
        unbox ("[ " + (String.concat " " value) + " ]")

    static member inline namedLines(value: string list) : IGridTemplateItem =
        unbox ("[ " + (String.concat " " value) + " ]")

    static member inline templateWidth(value: ICssUnit) : IGridTemplateItem = unbox value
    static member inline templateWidth(value: int) : IGridTemplateItem = unbox ((unbox<string> value) + "px")
    static member inline templateWidth(value: float) : IGridTemplateItem = unbox ((unbox<string> value) + "px")

type IGridLine = interface end

[<Erase>]
type gridLine =
    static member inline auto: IGridLine = unbox "auto"
    static member inline line(identifier: string) : IGridLine = unbox identifier

    static member inline line(value: int, identifier: string) : IGridLine =
        unbox (string<int> value + " " + identifier)

    static member inline span(value: int) : IGridLine = unbox ("span " + string<int> value)
    static member inline span(identifier: string) : IGridLine = unbox ("span " + identifier)

    static member inline span(value: int, identifier: string) : IGridLine =
        unbox ("span " + string<int> value + " " + identifier)

type IGridLineNames = interface end

[<Erase>]
type gridLineNames =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <line-names> =
    ///     '[' <custom-ident>* ']'
    /// ```
    /// **CSS**
    /// ```css
    /// [test test2]
    /// ```
    /// **F#**
    /// ```f#
    /// gridLineNames.names("test", "test2")
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_grid_layout/Grid_layout_using_named_grid_lines
    static member inline names([<ParamArray>] names: string array) : IGridLineNames =
        unbox ("[ " + String.concat " " names + " ]")

type IGridTrackSize = interface end

[<Erase>]
type gridTrackSize =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-breadth> =
    ///   <length-percentage [0,∞]>  |
    ///   <flex [0,∞]>               |
    ///   min-content                |
    ///   max-content                |
    ///   auto
    ///
    /// **F#**
    /// ```f#
    /// gridTrackSize.trackBreadth(length.px 10)
    /// ```
    /// https://drafts.csswg.org/css-grid/#grid-track-concept
    static member inline trackBreadth(size: ICssUnit) : IGridTrackSize = unbox size

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-breadth> =
    ///   <length-percentage [0,∞]>  |
    ///   <flex [0,∞]>               |
    ///   min-content                |
    ///   max-content                |
    ///   auto
    ///
    /// **F#**
    /// ```f#
    /// gridTrackSize.minmax(10, length.px 100)
    /// ```
    /// https://drafts.csswg.org/css-grid/#grid-track-concept
    static member inline minmax(inflexible: int, flexible: ICssUnit) : IGridTrackSize =
        unbox ("minmax(" + string<int> inflexible + "%, " + unbox flexible + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-breadth> =
    ///   <length-percentage [0,∞]>  |
    ///   <flex [0,∞]>               |
    ///   min-content                |
    ///   max-content                |
    ///   auto
    ///
    /// **F#**
    /// ```f#
    /// gridTrackSize.minmax(length.px 50, length.px 100)
    /// ```
    /// https://drafts.csswg.org/css-grid/#grid-track-concept
    static member inline minmax(inflexible: ICssUnit, flexible: ICssUnit) : IGridTrackSize =
        unbox ("minmax(" + unbox inflexible + ", " + unbox flexible + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-breadth> =
    ///   <length-percentage [0,∞]>  |
    ///   <flex [0,∞]>               |
    ///   min-content                |
    ///   max-content                |
    ///   auto
    ///
    /// **F#**
    /// ```f#
    /// gridTrackSize.fitContent(length.px 50)
    /// ```
    /// https://drafts.csswg.org/css-grid/#grid-track-concept
    static member inline fitContent(size: ICssUnit) : IGridTrackSize =
        unbox ("fit-content(" + unbox size + ")")

type IGridTrackNameWithTrackSize = interface end

[<Erase>]
type gridTrackNameWithTrackSize =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <track-size> ]
    /// ```
    /// **F#**
    /// ```f#
    /// gridTrackNameWithTrackSize.lineWithSize(gridTrackSize.minmax(2, length.fr 1))
    /// ```
    static member inline size(size: IGridTrackSize) : IGridTrackNameWithTrackSize = unbox size

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <track-size> ]
    /// ```
    /// **F#**
    /// ```f#
    /// gridTrackNameWithTrackSize.lineWithSize(gridLineNames.names("test"), gridTrackSize.minmax(2, length.fr 1))
    /// ```
    static member inline lineWithSize(line: IGridLineNames, size: IGridTrackSize) : IGridTrackNameWithTrackSize =
        unbox (unbox line + " " + unbox size)

type IGridTrackRepeat = interface end

[<Erase>]
type gridTrackRepeat =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid-template-columns#formal_syntax
    /// Grammar:
    /// ```
    /// <track-repeat> =
    ///   repeat( [ <integer [1,∞]> ] , [ <line-names>? <track-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr));
    /// ```
    /// **F#**
    /// ```f#
    /// gridTrackRepeat.repeat(2, gridTrackSize.minmax(2, length.fr 1))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(count: int, size: IGridTrackSize) : IGridTrackRepeat =
        unbox ("repeat(" + string<int> count + ", " + unbox size + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-repeat> =
    ///   repeat( [ <integer [1,∞]> ] , [ <line-names>? <track-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, [test] minmax(2%, 1fr));
    /// ```
    /// **F#**
    /// ```f#
    /// gridTrackRepeat.repeat(2, gridLineNames.names("test"), gridTrackSize.minmax(2, length.fr 1))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(count: int, lineNames: IGridLineNames, size: IGridTrackSize) : IGridTrackRepeat =
        unbox ("repeat(" + string<int> count + ", " + unbox lineNames + " " + unbox size + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-repeat> =
    ///   repeat( [ <integer [1,∞]> ] , [ <line-names>? <track-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr) [test] minmax(2%, 1fr));
    /// ```
    /// **F#**
    /// ```f#
    /// // param array, unlimited repeat
    /// gridTrackRepeat.repeat(2, [
    ///     gridTrackNameWithTrackSize.size(gridTrackSize.minmax(2, length.fr 1))
    ///     gridTrackNameWithTrackSize.lineWithSize(gridLineNames.names("test"), gridTrackSize.minmax(2, length.fr 1))
    /// ])
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat
        (count: int, [<ParamArray>] lineNamesAndSizes: IGridTrackNameWithTrackSize array)
        : IGridTrackRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + GridUtils.unboxSeq lineNamesAndSizes
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-repeat> =
    ///   repeat( [ <integer [1,∞]> ] , [ <line-names>? <track-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr) [test]);
    /// ```
    /// **F#**
    /// ```f#
    /// // This variant adds the last optional line-names
    /// gridTrackRepeat.repeat(2, gridTrackSize.minmax(2, length.fr 1), gridLineNames.names("test"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(count: int, size: IGridTrackSize, additionalLines: IGridLineNames) : IGridTrackRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + unbox size
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-repeat> =
    ///   repeat( [ <integer [1,∞]> ] , [ <line-names>? <track-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, [test] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridTrackRepeat.repeat(2, gridLineNames.names("test"), gridTrackSize.minmax(2, length.fr 1), gridLineNames.names("test2"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat
        (count: int, lineNames: IGridLineNames, size: IGridTrackSize, additionalLines: IGridLineNames)
        : IGridTrackRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + unbox lineNames
            + " "
            + unbox size
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <track-repeat> =
    ///   repeat( [ <integer [1,∞]> ] , [ <line-names>? <track-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr) [test] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridTrackRepeat.repeat(2,
    ///     // NOTE: inverted!
    ///     // additional lineNames
    ///     gridLineNames.names("test2"),
    ///     // param array sizes and lineNames
    ///     gridTrackNameWithTrackSize.size(gridTrackSize.minmax(2, length.fr 1)),
    ///     gridTrackNameWithTrackSize.lineWithSize(gridLineNames.names("test"), gridTrackSize.minmax(2, length.fr 1))
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat
        (
            count: int,
            additionalLines: IGridLineNames,
            [<ParamArray>] lineNamesAndSizes: IGridTrackNameWithTrackSize array
        ) : IGridTrackRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + GridUtils.unboxSeq lineNamesAndSizes
            + " "
            + unbox additionalLines
            + ")"
        )

type IGridFixedSize = interface end

[<Erase>]
type gridFixedSize =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-size> =
    ///     <fixed-breadth>                                   |
    ///     minmax( <fixed-breadth> , <track-breadth> )       |
    ///     minmax( <inflexible-breadth> , <fixed-breadth> )
    /// **CSS**
    /// ```css
    /// 2%
    /// ```
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedSize.fixedBreadth(2)
    /// ```
    static member inline fixedBreadth(value: int) : IGridFixedSize = unbox (string<int> value + "%")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-size> =
    ///     <fixed-breadth>                                   |
    ///     minmax( <fixed-breadth> , <track-breadth> )       |
    ///     minmax( <inflexible-breadth> , <fixed-breadth> )
    /// **CSS**
    /// ```css
    /// minmax(10%, 100px)
    /// ```
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedSize.minmax(10, length.px 100)
    /// ```
    static member inline minmax(fixedBreadth: int, trackBreadth: ICssUnit) : IGridFixedSize =
        unbox ("minmax(" + string<int> fixedBreadth + "%, " + unbox trackBreadth + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-size> =
    ///     <fixed-breadth>                                   |
    ///     minmax( <fixed-breadth> , <track-breadth> )       |
    ///     minmax( <inflexible-breadth> , <fixed-breadth> )
    /// **CSS**
    /// ```css
    /// minmax(100px, 10%)
    /// ```
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedSize.minmax(length.px 100, 10)
    /// ```
    static member inline minmax(inflexibleBreadth: ICssUnit, fixedBreadth: int) : IGridFixedSize =
        unbox ("minmax(" + unbox inflexibleBreadth + ", " + string<int> fixedBreadth + "%)")

type IGridTrackNameWithFixedSize = interface end

[<Erase>]
type gridTrackNameWithFixedSize =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <fixed-size> ]
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedNameWithFixedSize.lineWithSize(gridFixedSize.minmax(2, length.fr 1))
    /// ```
    static member inline size(size: IGridFixedSize) : IGridTrackNameWithFixedSize = unbox size

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <fixed-size> ]
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedNameWithFixedSize.lineWithSize(gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1))
    /// ```
    static member inline lineWithSize(line: IGridLineNames, size: IGridFixedSize) : IGridTrackNameWithFixedSize =
        unbox (unbox line + " " + unbox size)

type IGridFixedRepeat = interface end

[<Erase>]
type gridFixedRepeat =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-repeat> =
    ///     repeat( [ <integer [1,∞]> ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr));
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedRepeat.repeat(2, gridFixedSize.minmax(2, length.fr 1))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(count: int, size: IGridFixedRepeat) : IGridFixedRepeat =
        unbox ("repeat(" + string<int> count + ", " + unbox size + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-repeat> =
    ///     repeat( [ <integer [1,∞]> ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, [test] minmax(2%, 1fr));
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedRepeat.repeat(2, gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(count: int, lineNames: IGridLineNames, size: IGridFixedSize) : IGridFixedRepeat =
        unbox ("repeat(" + string<int> count + ", " + unbox lineNames + unbox size + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-repeat> =
    ///     repeat( [ <integer [1,∞]> ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr) [test] minmax(2%, 1fr));
    /// ```
    /// **F#**
    /// ```f#
    /// // param array
    /// gridFixedRepeat.repeat(2,
    ///     gridTracknameWithFixedSize.size(gridFixedSize.minmax(2, length.fr 1))
    ///     gridTracknameWithFixedSize.lineWithSize(
    ///         gridLineNames.names("test")
    ///         gridFixedSize.minmax(2, length.fr 1)
    ///     )
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat
        (count: int, [<ParamArray>] lineNamesAndSizes: IGridTrackNameWithFixedSize array)
        : IGridFixedRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + GridUtils.unboxSeq lineNamesAndSizes
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-repeat> =
    ///     repeat( [ <integer [1,∞]> ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr) [test]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedRepeat.repeat(2, gridFixedSize.minmax(2, length.fr 1), gridLineNames.names("test"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(count: int, size: IGridFixedSize, additionalLines: IGridLineNames) : IGridFixedRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + unbox size
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-repeat> =
    ///     repeat( [ <integer [1,∞]> ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, [test] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedRepeat.repeat(2, gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1), gridLineNames.names("test2"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat
        (count: int, lineNames: IGridLineNames, size: IGridFixedSize, additionalLines: IGridLineNames)
        : IGridFixedRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + unbox lineNames
            + " "
            + unbox size
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <fixed-repeat> =
    ///     repeat( [ <integer [1,∞]> ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, minmax(2%, 1fr) [test] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridFixedRepeat.repeat(2,
    ///     // NOTE: inverted!
    ///     // additional lineNames
    ///     gridLineNames.names("test2"),
    ///     // param array sizes and lineNames
    ///     gridTrackNameWithFixedSize.size(gridFixedSize.minmax(2, length.fr 1)),
    ///     gridTrackNameWithFixedSize.lineWithSize(gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1))
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat
        (
            count: int,
            additionalLines: IGridLineNames,
            [<ParamArray>] lineNamesAndSizes: IGridTrackNameWithFixedSize array
        ) : IGridFixedRepeat =
        unbox (
            "repeat("
            + string<int> count
            + ", "
            + GridUtils.unboxSeq lineNamesAndSizes
            + " "
            + unbox additionalLines
            + ")"
        )

type IGridAutoRepeat = interface end

[<Erase>]
type gridAutoRepeat =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-repeat> =
    ///     repeat( [ auto-fill | auto-fit ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, minmax(2%, 1fr) [test]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridAutoRepeat.repeatAutoFill(gridFixedSize.minmax(2, length.fr 1),gridLineNames.names("test"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFill(size: IGridFixedSize, additionalLines: IGridLineNames) : IGridAutoRepeat =
        unbox ("repeat(auto-fill, " + unbox size + " " + unbox additionalLines + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-repeat> =
    ///     repeat( [ auto-fill | auto-fit ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, [test2] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridAutoRepeat.repeatAutoFill(gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1), gridLineNames.names("test2"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFill
        (lineNames: IGridLineNames, size: IGridFixedSize, additionalLines: IGridLineNames)
        : IGridAutoRepeat =
        unbox (
            "repeat(auto-fill, "
            + unbox lineNames
            + " "
            + unbox size
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-repeat> =
    ///     repeat( [ auto-fill | auto-fit ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, minmax(2%, 1fr) [test] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridAutoRepeat.repeatAutoFill(
    ///     // NOTE: inverted!
    ///     // additional lineNames
    ///     gridLineNames.names("test2"),
    ///     // param array sizes and lineNames
    ///     gridTrackNameWithFixedSize.size(gridFixedSize.minmax(2, length.fr 1)),
    ///     gridTrackNameWithFixedSize.lineWithSize(gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1))
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFill
        (additionalLines: IGridLineNames, [<ParamArray>] lineNamesAndSizes: IGridTrackNameWithFixedSize array)
        : IGridAutoRepeat =
        unbox (
            "repeat(auto-fill, "
            + GridUtils.unboxSeq lineNamesAndSizes
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-repeat> =
    ///     repeat( [ auto-fill | auto-fit ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fit, minmax(2%, 1fr) [test]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridAutoRepeat.repeatAutoFit(gridFixedSize.minmax(2, length.fr 1),gridLineNames.names("test"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFit(size: IGridFixedSize, additionalLines: IGridLineNames) : IGridAutoRepeat =
        unbox ("repeat(auto-fit, " + unbox size + " " + unbox additionalLines + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-repeat> =
    ///     repeat( [ auto-fill | auto-fit ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fit, [test2] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridAutoRepeat.repeatAutoFit(gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1), gridLineNames.names("test2"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFit
        (lineNames: IGridLineNames, size: IGridFixedSize, additionalLines: IGridLineNames)
        : IGridAutoRepeat =
        unbox (
            "repeat(auto-fit, "
            + unbox lineNames
            + " "
            + unbox size
            + " "
            + unbox additionalLines
            + ")"
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-repeat> =
    ///     repeat( [ auto-fill | auto-fit ] , [ <line-names>? <fixed-size> ]+ <line-names>? )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fit, minmax(2%, 1fr) [test] minmax(2%, 1fr) [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridAutoRepeat.repeatAutoFit(
    ///     // NOTE: inverted!
    ///     // additional lineNames
    ///     gridLineNames.names("test2"),
    ///     // param array sizes and lineNames
    ///     gridTrackNameWithFixedSize.size(gridFixedSize.minmax(2, length.fr 1)),
    ///     gridTrackNameWithFixedSize.lineWithSize(gridLineNames.names("test"), gridFixedSize.minmax(2, length.fr 1))
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFit
        (additionalLines: IGridLineNames, [<ParamArray>] lineNamesAndSizes: IGridTrackNameWithFixedSize array)
        : IGridAutoRepeat =
        unbox (
            "repeat(auto-fit, "
            + GridUtils.unboxSeq lineNamesAndSizes
            + " "
            + unbox additionalLines
            + ")"
        )

type IGridNameRepeat = interface end

[<Erase>]
type gridNameRepeat =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <name-repeat> =
    ///     repeat( [ <integer [1,∞]> | auto-fill ] , <line-names>+ )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, [test]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridNameRepeat.repeat(2, gridLineNames.names("test"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(size: ICssUnit, line: IGridLineNames) : IGridNameRepeat =
        unbox ("repeat(" + unbox size + ", " + unbox line + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <name-repeat> =
    ///     repeat( [ <integer [1,∞]> | auto-fill ] , <line-names>+ )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(2, [test] [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridNameRepeat.repeatAutoFill(
    ///     2,
    ///     // param array
    ///     gridLineNames.names("test"),
    ///     gridLineNames.names("test2")
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeat(size: ICssUnit, [<ParamArray>] lines: IGridLineNames array) : IGridNameRepeat =
        unbox ("repeat(" + unbox size + ", " + GridUtils.unboxSeq lines + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <name-repeat> =
    ///     repeat( [ <integer [1,∞]> | auto-fill ] , <line-names>+ )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, [test]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridNameRepeat.repeatAutoFill(gridLineNames.names("test"))
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFill(line: IGridLineNames) : IGridNameRepeat =
        unbox ("repeat(auto-fill, " + unbox line + ")")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <name-repeat> =
    ///     repeat( [ <integer [1,∞]> | auto-fill ] , <line-names>+ )
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, [test] [test2]);
    /// ```
    /// **F#**
    /// ```f#
    /// gridNameRepeat.repeatAutoFill(
    ///     // param array
    ///     gridLineNames.names("test"),
    ///     gridLineNames.names("test2")
    /// )
    /// ```
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/repeat
    static member inline repeatAutoFill([<ParamArray>] lines: IGridLineNames array) : IGridNameRepeat =
        unbox ("repeat(auto-fill, " + GridUtils.unboxSeq lines + ")")

type IGridLineNameOrNameRepeat = interface end

[<Erase>]
type gridLineNameOrNameRepeat =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names> | <name-repeat> ]
    /// ```
    /// **F#**
    /// ```f#
    /// gridLineNameOrNameRepeat(gridLineNames.names("test"))
    /// ```
    static member inline line(line: IGridLineNames) : IGridLineNameOrNameRepeat = unbox line

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names> | <name-repeat> ]
    /// ```
    /// **F#**
    /// ```f#
    /// gridLineNameOrNameRepeat(gridNameRepeat.repeatAutoFill(gridLineNames.names("test")))
    /// ```
    static member inline repeat(repeat: IGridNameRepeat) : IGridLineNameOrNameRepeat = unbox repeat

type IGridLineNameList = interface end

[<Erase>]
type gridLineNameList =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names> | <name-repeat> ]+
    /// ```
    /// See IGridLineNameOrNameRepeat
    static member inline list(lineOrRepeat: IGridLineNameOrNameRepeat) : IGridLineNameList = unbox lineOrRepeat

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names> | <name-repeat> ]+
    /// ```
    /// See IGridLineNameOrNameRepeat
    static member inline list([<ParamArray>] linesOrRepeats: IGridLineNameOrNameRepeat array) : IGridLineNameList =
        unbox (GridUtils.unboxSeq linesOrRepeats)

type IGridTrackNameWithSizeOrRepeat = interface end

[<Erase>]
type gridTrackNameWithSizeOrRepeat =
    static member inline size(size: IGridFixedSize) : IGridTrackNameWithSizeOrRepeat = unbox size
    static member inline repeat(repeat: IGridFixedRepeat) : IGridTrackNameWithSizeOrRepeat = unbox repeat

    static member inline lineWithSize(line: IGridLineNames, size: IGridFixedSize) : IGridTrackNameWithSizeOrRepeat =
        unbox (unbox line + " " + unbox size)

    static member inline lineWithRepeat
        (line: IGridLineNames, repeat: IGridFixedRepeat)
        : IGridTrackNameWithSizeOrRepeat =
        unbox (unbox line + " " + unbox repeat)

type IGridAutoTrackList = interface end

[<Erase>]
type gridAutoTrackList =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, minmax(2%, 1fr) [test]);
    /// ```
    /// **F#**
    /// ```f#
    ///  gridAutoTrackList.autoRepeat (
    ///     gridAutoRepeat.repeatAutoFill (
    ///         gridFixedSize.minmax (2, length.fr 1),
    ///         gridLineNames.names ("test")
    ///     )
    ///  )
    /// ```
    static member inline autoRepeat(autoRepeat: IGridAutoRepeat) : IGridAutoTrackList = unbox autoRepeat

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize; unbox autoRepeat; GridUtils.unboxSeq namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2
              unbox line ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize
              unbox line
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize
              unbox line
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2
              unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize; unbox autoRepeat; unbox namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize; unbox autoRepeat; unbox namesAndOrSize2; unbox line ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize; unbox line; unbox autoRepeat; unbox namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize
              unbox line
              unbox autoRepeat
              unbox namesAndOrSize2
              unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (namesAndOrSize: IGridTrackNameWithSizeOrRepeat, autoRepeat: IGridAutoRepeat, line: IGridLineNames)
        : IGridAutoTrackList =
        unbox ([ unbox namesAndOrSize; unbox autoRepeat; unbox line ] |> String.concat " ")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (namesAndOrSize: IGridTrackNameWithSizeOrRepeat, line: IGridLineNames, autoRepeat: IGridAutoRepeat)
        : IGridAutoTrackList =
        unbox ([ unbox namesAndOrSize; unbox line; unbox autoRepeat ] |> String.concat " ")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox namesAndOrSize; unbox line; unbox autoRepeat; unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (namesAndOrSize: IGridTrackNameWithSizeOrRepeat, autoRepeat: IGridAutoRepeat)
        : IGridAutoTrackList =
        unbox (unbox namesAndOrSize + " " + unbox autoRepeat)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize; unbox autoRepeat; unbox namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat,
            line: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox autoRepeat
              unbox namesAndOrSize2
              unbox line ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox line
              unbox autoRepeat
              unbox namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox line
              unbox autoRepeat
              unbox namesAndOrSize2
              unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq, autoRepeat: IGridAutoRepeat)
        : IGridAutoTrackList =
        unbox (GridUtils.unboxSeq namesAndOrSize + " " + unbox autoRepeat)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2
              unbox line ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq, autoRepeat: IGridAutoRepeat, line: IGridLineNames)
        : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize; unbox autoRepeat; unbox line ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq, line: IGridLineNames, autoRepeat: IGridAutoRepeat)
        : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize; unbox line; unbox autoRepeat ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox line
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize2: IGridTrackNameWithSizeOrRepeat seq,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize
              unbox line
              unbox autoRepeat
              GridUtils.unboxSeq namesAndOrSize2
              unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ GridUtils.unboxSeq namesAndOrSize; unbox line; unbox autoRepeat; unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (autoRepeat: IGridAutoRepeat, namesAndOrSize: IGridTrackNameWithSizeOrRepeat)
        : IGridAutoTrackList =
        unbox (unbox autoRepeat + " " + unbox namesAndOrSize)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (autoRepeat: IGridAutoRepeat, namesAndOrSize: IGridTrackNameWithSizeOrRepeat, line: IGridLineNames)
        : IGridAutoTrackList =
        unbox ([ unbox autoRepeat; unbox namesAndOrSize; unbox line ] |> String.concat " ")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (line: IGridLineNames, autoRepeat: IGridAutoRepeat, namesAndOrSize: IGridTrackNameWithSizeOrRepeat)
        : IGridAutoTrackList =
        unbox ([ unbox line; unbox autoRepeat; unbox namesAndOrSize ] |> String.concat " ")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox line; unbox autoRepeat; unbox namesAndOrSize; unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (autoRepeat: IGridAutoRepeat, namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq)
        : IGridAutoTrackList =
        unbox (unbox autoRepeat + " " + GridUtils.unboxSeq namesAndOrSize)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (autoRepeat: IGridAutoRepeat, namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq, line: IGridLineNames)
        : IGridAutoTrackList =
        unbox (
            [ unbox autoRepeat; GridUtils.unboxSeq namesAndOrSize; unbox line ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (line: IGridLineNames, autoRepeat: IGridAutoRepeat, namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq)
        : IGridAutoTrackList =
        unbox (
            [ unbox line; unbox autoRepeat; GridUtils.unboxSeq namesAndOrSize ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (
            line: IGridLineNames,
            autoRepeat: IGridAutoRepeat,
            namesAndOrSize: IGridTrackNameWithSizeOrRepeat seq,
            line2: IGridLineNames
        ) : IGridAutoTrackList =
        unbox (
            [ unbox line; unbox autoRepeat; GridUtils.unboxSeq namesAndOrSize; unbox line2 ]
            |> String.concat " "
        )

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list(autoRepeat: IGridAutoRepeat, line: IGridLineNames) : IGridAutoTrackList =
        unbox (unbox autoRepeat + " " + unbox line)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list(line: IGridLineNames, autoRepeat: IGridAutoRepeat) : IGridAutoTrackList =
        unbox (unbox line + " " + unbox autoRepeat)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <auto-track-list> =
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    ///     <auto-repeat>
    ///     [ <line-names>? [ <fixed-size> | <fixed-repeat> ] ]* <line-names>?
    /// ```
    /// **CSS**
    /// ```css
    /// 2% minmax(2%, 200px) [ test1 ] repeat(auto-fill, minmax(10px, 10%) [ test2 ]) 2% minmax(2%, 10px) [ test3 ]
    /// ```
    /// **F#**
    /// ```
    /// // Multi overload function
    /// // Single (maximum) example shown
    /// gridAutoTrackList.autoRepeat (
    ///     gridAutoTrackList.list (
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 200) )
    ///         ],
    ///         gridLineNames.names ("test1"),
    ///         gridAutoRepeat.repeatAutoFill (gridFixedSize.minmax (length.px 10, 10), gridLineNames.names ("test2")),
    ///         [
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.fixedBreadth 2 )
    ///             gridTrackNameWithSizeOrRepeat.size (gridFixedSize.minmax (2, length.px 10) )
    ///         ],
    ///         gridLineNames.names ("test3")
    ///     )
    /// )
    /// ```
    static member inline list
        (line: IGridLineNames, autoRepeat: IGridAutoRepeat, line2: IGridLineNames)
        : IGridAutoTrackList =
        unbox ([ unbox line; unbox autoRepeat; unbox line2 ] |> String.concat " ")

type IGridTrackList = interface end

[<Erase>]
type gridTrackList =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <track-size> ]+ <line-names>?
    /// ```
    /// see IGridTrackNameWithSizeOrRepeat
    static member inline list([<ParamArray>] namesAndOrSize: IGridTrackNameWithSizeOrRepeat array) : IGridTrackList =
        unbox (GridUtils.unboxSeq namesAndOrSize)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <track-size> ]+ <line-names>?
    /// ```
    /// see IGridTrackNameWithSizeOrRepeat
    static member inline list
        (line: IGridLineNames, [<ParamArray>] namesAndOrSize: IGridTrackNameWithSizeOrRepeat array)
        : IGridTrackList =
        unbox (GridUtils.unboxSeq namesAndOrSize + " " + unbox line)

type IGridTemplateRowsOrColumns = interface end

[<Erase>]
type gridTemplateRowsOrColumns =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template-columns> =
    ///     none                       |
    ///     <track-list>               |
    ///     <auto-track-list>          |
    ///     subgrid <line-name-list>?
    /// ```
    static member inline none: IGridTemplateRowsOrColumns = unbox "none"

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template-columns> =
    ///     none                       |
    ///     <track-list>               |
    ///     <auto-track-list>          |
    ///     subgrid <line-name-list>?
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, minmax(2%, 1fr) [test])
    /// [test] minmax(2px, 88%) [test2] minmax(2%, 10px) [test3]
    /// ```
    /// **F#**
    /// ```
    /// gridTemplateRowsOrColumns.trackList (
    ///     gridTrackList.list (
    ///         gridLineNames.names ("test"),
    ///         gridTrackNameWithSizeOrRepeat.lineWithSize (
    ///             gridLineNames.names ("test2"),
    ///             gridFixedSize.minmax (length.px 2, 88)
    ///         ),
    ///         gridTrackNameWithSizeOrRepeat.lineWithSize (
    ///             gridLineNames.names ("test3"),
    ///             gridFixedSize.minmax (2, length.px 10)
    ///         )
    ///     )
    /// )
    /// ```
    static member inline trackList(trackList: IGridTrackList) : IGridTemplateRowsOrColumns = unbox trackList

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template-columns> =
    ///     none                       |
    ///     <track-list>               |
    ///     <auto-track-list>          |
    ///     subgrid <line-name-list>?
    /// ```
    /// **CSS**
    /// ```css
    /// repeat(auto-fill, minmax(2%, 1fr) [test])
    /// ```
    /// **F#**
    /// ```
    /// gridTemplateRowsOrColumns.autoTrackList (
    ///     gridAutoTrackList.autoRepeat (
    ///         gridAutoRepeat.repeatAutoFill (
    ///             gridFixedSize.minmax (2, length.fr 1), gridLineNames.names ("test")
    ///         )
    ///     )
    /// )
    /// ```
    static member inline autoTrackList(autoTrackList: IGridAutoTrackList) : IGridTemplateRowsOrColumns =
        unbox autoTrackList

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template-columns> =
    ///     none                       |
    ///     <track-list>               |
    ///     <auto-track-list>          |
    ///     subgrid <line-name-list>?
    /// ```
    /// **CSS**
    /// ```css
    /// subgrid
    /// ```
    /// **F#**
    /// ```
    /// gridTemplateRowsOrColumns.subGrid ()
    /// ```
    static member inline subGrid() : IGridTemplateRowsOrColumns = unbox "subgrid"

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template-columns> =
    ///     none                       |
    ///     <track-list>               |
    ///     <auto-track-list>          |
    ///     subgrid <line-name-list>?
    /// ```
    /// **CSS**
    /// ```css
    /// subgrid repeat(auto-fill, [test])
    /// ```
    /// **F#**
    /// ```
    /// gridTemplateRowsOrColumns.subGrid (
    ///     gridLineNameList.list (
    ///         gridLineNameOrNameRepeat.repeat (
    ///             gridNameRepeat.repeatAutoFill (
    ///                 gridLineNames.names ("test")
    ///             )
    ///         )
    ///     )
    /// )
    /// ```
    static member inline subGrid(lineNameList: IGridLineNameList) : IGridTemplateRowsOrColumns =
        unbox ("subgrid " + unbox lineNameList)

type IGridCustom = interface end

[<Erase>]
type gridCustom =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// "custom"
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom("cutom")
    /// ```
    static member inline custom(name: string) : IGridCustom = unbox ("\"" + name + "\"")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// [test] "custom"
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom(gridLineNames.names("test"), "cutom")
    /// ```
    static member inline custom(line: IGridLineNames, name: string) : IGridCustom =
        unbox (unbox line + "\"" + name + "\"")

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// [test] "custom" 1fr
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom(gridLineNames.names("test"), "cutom", gridTrackSize.trackBreadth(length.fr 1))
    /// ```
    static member inline custom(line: IGridLineNames, name: string, trackSize: IGridTrackSize) : IGridCustom =
        unbox (unbox line + "\"" + name + "\"" + unbox trackSize)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// [test] "custom" 1fr [test2]
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom(gridLineNames.names("test"), "cutom", gridTrackSize.trackBreadth(length.fr 1), gridLineNames.names("test2"))
    /// ```
    static member inline custom
        (line: IGridLineNames, name: string, trackSize: IGridTrackSize, line2: IGridLineNames)
        : IGridCustom =
        unbox (unbox line + "\"" + name + "\"" + unbox trackSize + unbox line2)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// "custom" 1fr [test]
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom("cutom", gridTrackSize.trackBreadth(length.fr 1), gridLineNames.names("test"))
    /// ```
    static member inline custom(name: string, trackSize: IGridTrackSize, line2: IGridLineNames) : IGridCustom =
        unbox ("\"" + name + "\"" + unbox trackSize + unbox line2)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// "custom" 1fr
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom("cutom", gridTrackSize.trackBreadth(length.fr 1))
    /// ```
    static member inline custom(name: string, trackSize: IGridTrackSize) : IGridCustom =
        unbox ("\"" + name + "\"" + unbox trackSize)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// "custom" [test]
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom("cutom", gridLineNames.names("test"))
    /// ```
    static member inline custom(name: string, line2: IGridLineNames) : IGridCustom =
        unbox ("\"" + name + "\"" + unbox line2)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// [ <line-names>? <string> <track-size>? <line-names>? ]
    /// ```
    /// **CSS**
    /// ```css
    /// [test] "custom" [test2]
    /// ```
    /// **F#**
    /// ```
    /// gridCustom.custom(gridLineNames.names("test"), "cutom", gridLineNames.names("test2"))
    /// ```
    static member inline custom(line: IGridLineNames, name: string, line2: IGridLineNames) : IGridCustom =
        unbox (unbox line + "\"" + name + "\"" + unbox line2)

type IGridExplicitTrackList = interface end

[<Erase>]
type explicitTrackList =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <explicit-track-list> =
    ///     [ <line-names>? <track-size> ]+ <line-names>?
    /// ```
    /// see IGridTrackNameWithSize
    static member inline list
        ([<ParamArray>] linesWithSizes: IGridTrackNameWithFixedSize array)
        : IGridExplicitTrackList =
        unbox (GridUtils.unboxSeq linesWithSizes)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <explicit-track-list> =
    ///     [ <line-names>? <track-size> ]+ <line-names>?
    /// ```
    /// see IGridTrackNameWithSize
    static member inline list
        (line: IGridLineNames, [<ParamArray>] linesWithSizes: IGridTrackNameWithFixedSize array)
        : IGridExplicitTrackList =
        unbox (GridUtils.unboxSeq linesWithSizes + unbox line)

type IGridTemplate = interface end

[<Erase>]
type gridTemplate =
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template> =
    ///     none                                                |
    ///     [ <'grid-template-rows'> / <'grid-template-columns'> ]  |
    ///     [ <line-names>? <string> <track-size>? <line-names>? ]+ [ / <explicit-track-list> ]?
    /// ```
    static member inline none: IGridTemplate = unbox "none"

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template> =
    ///     none                                                |
    ///     [ <'grid-template-rows'> / <'grid-template-columns'> ]  |
    ///     [ <line-names>? <string> <track-size>? <line-names>? ]+ [ / <explicit-track-list> ]?
    /// ```
    /// Combines rows and columns to a grid template
    /// see IGridTemplateRowsOrColumns
    static member inline template
        (rows: IGridTemplateRowsOrColumns, columns: IGridTemplateRowsOrColumns)
        : IGridTemplate =
        unbox (unbox rows + " / " + unbox columns)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template> =
    ///     none                                                |
    ///     [ <'grid-template-rows'> / <'grid-template-columns'> ]  |
    ///     [ <line-names>? <string> <track-size>? <line-names>? ]+ [ / <explicit-track-list> ]?
    /// ```
    /// Builds a template from custom strings with lineNames and sizes
    /// see IGridCustom
    static member inline custom([<ParamArray>] customTemplates: IGridCustom array) : IGridTemplate =
        unbox (GridUtils.unboxSeq customTemplates)

    /// https://developer.mozilla.org/en-US/docs/Web/CSS/grid#formal_syntax
    /// Grammar:
    /// ```
    /// <grid-template> =
    ///     none                                                |
    ///     [ <'grid-template-rows'> / <'grid-template-columns'> ]  |
    ///     [ <line-names>? <string> <track-size>? <line-names>? ]+ [ / <explicit-track-list> ]?
    /// ```
    /// Builds a template from custom strings with lineNames, sizes, and IGridExplicitTracklist
    /// see IGridCustom and IGridExplicitTracklist
    static member inline custom
        (explicitTrackList: IGridExplicitTrackList, [<ParamArray>] customTemplates: IGridCustom array)
        : IGridTemplate =
        unbox (GridUtils.unboxSeq customTemplates + unbox explicitTrackList)
