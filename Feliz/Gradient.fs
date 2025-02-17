namespace Feliz

open System

open Feliz.Styles

type IGradientAngle = interface end

type gradientAngle =
    static member inline deg (value: float) : IGradientAngle = unbox (string<float> value + "deg")
    static member inline grad (value: float) : IGradientAngle = unbox (string<float> value + "grad")
    static member inline rad (value: float) : IGradientAngle = unbox (string<float> value + "rad")
    static member inline turn (value: float) : IGradientAngle = unbox (string<float> value + "turn")

// Linear Types
type IGradientCorner = interface end

type gradientCorner =
    static member inline top: IGradientCorner = unbox "top"
    static member inline right: IGradientCorner = unbox "right"
    static member inline bottom: IGradientCorner = unbox "bottom"
    static member inline left: IGradientCorner = unbox "left"

type ILinearColorStop = interface end

type linearColorStop =
    static member inline colorStop (color: string) : ILinearColorStop = unbox color
    static member inline colorStop (color: string, stop: ICssUnit) : ILinearColorStop = unbox (color + " " + unbox stop)
    static member inline colorStop (color: string, stop: ICssUnit, additionalStop: ICssUnit) : ILinearColorStop =
        color + " " + unbox stop + " " + unbox additionalStop
        |> unbox

type ILinearColorStopAndHint = interface end

type linearColorStopAndHint =
    static member inline colorStop (color: string) : ILinearColorStopAndHint = unbox color
    static member inline colorStop (hint: ICssUnit, color: string) : ILinearColorStopAndHint =
        unbox (unbox hint + ", " + unbox color)
    static member inline colorStop (color: string, stop: ICssUnit) : ILinearColorStopAndHint =
        unbox (color + " " + unbox stop)
    static member inline colorStop (hint: ICssUnit, color: string, stop: ICssUnit) : ILinearColorStopAndHint =
        unbox (unbox hint + ", " + color + " " + unbox stop)
    static member inline colorStop (color: string, stop: ICssUnit, additionalStop: ICssUnit) : ILinearColorStopAndHint =
        unbox (color + " " + unbox stop + " " + unbox additionalStop)
    static member inline colorStop
        (hint: ICssUnit, color: string, stop: ICssUnit, additionalStop: ICssUnit)
        : ILinearColorStopAndHint
        =
        unbox hint
        + ", "
        + color
        + " "
        + unbox stop
        + " "
        + unbox additionalStop
        |> unbox

type ILinearGradientOptions = interface end

type linearGradientOptions =
    static member inline angle (angle: IGradientAngle) : ILinearGradientOptions = unbox angle
    static member inline corner (corner: IGradientCorner) : ILinearGradientOptions = unbox corner

// Radial Types
type IRadialShape = interface end

type radialShape =
    static member inline circle: IRadialShape = unbox "circle"
    static member inline ellipse: IRadialShape = unbox "ellipse"

type IRadialSize = interface end

type radialSize =
    static member inline closestCorner: IRadialSize = unbox "closest-corner"
    static member inline closestSide: IRadialSize = unbox "closest-side"
    static member inline farthestCorner: IRadialSize = unbox "farthest-corner"
    static member inline farthestSide: IRadialSize = unbox "farthest-side"
    static member inline length (length: ICssUnit) : IRadialSize = unbox length

// Conic Types
type IConicColorHintVariant = interface end

type conicColorHintVariant =
    static member inline angle (angle: IGradientAngle) : IConicColorHintVariant = unbox angle
    static member inline percentage (value: float) : IConicColorHintVariant = unbox (string<float> value + "%")

type IAngleColorStop = interface end

type angleColorStop =
    static member inline colorStop (color: string) : IAngleColorStop = unbox color
    static member inline colorStop (color: string, stop: ICssUnit) : IAngleColorStop = unbox (color + unbox stop)
    static member inline colorStop (color: string, stop: ICssUnit, additionalStop: ICssUnit) : IAngleColorStop =
        color + unbox stop + unbox additionalStop |> unbox

type IAngleColorHintAndStop = interface end

type angleColorHintAndStop =
    static member inline colorStop (color: string) : IAngleColorHintAndStop = unbox color
    static member inline colorStop (color: string, stop: ICssUnit) : IAngleColorHintAndStop = unbox (color + unbox stop)
    static member inline colorStop (color: string, stop: ICssUnit, additionalStop: ICssUnit) : IAngleColorHintAndStop =
        color + unbox stop + unbox additionalStop |> unbox
    static member inline colorStop (hint: IConicColorHintVariant, color: string) : IAngleColorHintAndStop =
        unbox hint + ", " + color |> unbox
    static member inline colorStop
        (hint: IConicColorHintVariant, color: string, stop: ICssUnit)
        : IAngleColorHintAndStop
        =
        unbox hint + ", " + color + " " + unbox stop |> unbox
    static member inline colorStop
        (hint: IConicColorHintVariant, color: string, stop: ICssUnit, additionalStop: ICssUnit)
        : IAngleColorHintAndStop
        =
        unbox hint
        + ", "
        + color
        + " "
        + unbox stop
        + " "
        + unbox additionalStop
        |> unbox

type IGradientPosition = interface end

type gradientPosition =
    static member inline top: IGradientPosition = unbox "at top"
    static member inline right: IGradientPosition = unbox "at right"
    static member inline center: IGradientPosition = unbox "at center"
    static member inline bottom: IGradientPosition = unbox "at bottom"
    static member inline left: IGradientPosition = unbox "at left"
    static member inline rightTop: IGradientPosition = unbox "at right top"
    static member inline rightCenter: IGradientPosition = unbox "at right center"
    static member inline rightBottom: IGradientPosition = unbox "at right bottom"
    static member inline centerTop: IGradientPosition = unbox "at center top"
    static member inline centerBottom: IGradientPosition = unbox "at center bottom"
    static member inline leftTop: IGradientPosition = unbox "at left top"
    static member inline leftCenter: IGradientPosition = unbox "at left center"
    static member inline leftBottom: IGradientPosition = unbox "at left bottom"

type IAngleAndPosition = interface end

type angleAndPosition =
    static member inline angle (angle: IGradientAngle) : IAngleAndPosition = unbox ("from " + unbox angle)
    static member inline position (position: IGradientPosition) : IAngleAndPosition = unbox position

type IRadialSizeAndOrShape = interface end

type radialSizeAndOrShape =
    static member inline shape (shape: IRadialShape) : IRadialSizeAndOrShape = unbox shape
    static member inline size (size: IRadialSize) : IRadialSizeAndOrShape = unbox size
    static member inline sizeAndShape (size: IRadialSize, shape: IRadialShape) : IRadialSizeAndOrShape =
        unbox size + " " + unbox shape |> unbox

type IRectangularColorSpace = interface end

type rectangularColorSpace =
    static member inline a98Rgb: IRectangularColorSpace = unbox "a98-rgb"
    static member inline displayP3: IRectangularColorSpace = unbox "display-p3"
    static member inline lab: IRectangularColorSpace = unbox "lab"
    static member inline okLab: IRectangularColorSpace = unbox "oklab"
    static member inline prophotoRgb: IRectangularColorSpace = unbox "prophoto-rgb"
    static member inline rec2020: IRectangularColorSpace = unbox "rec2020"
    static member inline srgb: IRectangularColorSpace = unbox "srgb"
    static member inline srgbLinear: IRectangularColorSpace = unbox "srgb-linear"
    static member inline xyz: IRectangularColorSpace = unbox "xyz"
    static member inline xyzD50: IRectangularColorSpace = unbox "xyz-d50"
    static member inline xyzD65: IRectangularColorSpace = unbox "xyz-d65"

type IPolarColorSpaceVariant = interface end

type polarColorSpaceVariant =
    static member inline hsl: IPolarColorSpaceVariant = unbox "hsl"
    static member inline hwb: IPolarColorSpaceVariant = unbox "hwb"
    static member inline lch: IPolarColorSpaceVariant = unbox "lch"
    static member inline okLch: IPolarColorSpaceVariant = unbox "oklch"

type IHueInterpolationMethod = interface end

type hueInterpolationMethod =
    static member inline shorter: IHueInterpolationMethod = unbox "shorter"
    static member inline longer: IHueInterpolationMethod = unbox "longer"
    static member inline increasing: IHueInterpolationMethod = unbox "increasing"
    static member inline decreasing: IHueInterpolationMethod = unbox "decreasing"

type IColorInterpolation = interface end

type colorInterpolation =
    static member inline polar (variant: IPolarColorSpaceVariant) : IColorInterpolation =
        "in " + unbox variant + " shorter hue" |> unbox
    static member inline polarWithInterpolation
        (variant: IPolarColorSpaceVariant, interpolation: IHueInterpolationMethod)
        : IColorInterpolation
        =
        "in " + unbox interpolation + unbox variant + " hue"
        |> unbox
    static member inline rectangular (colorSpace: IRectangularColorSpace) : IColorInterpolation = unbox colorSpace

type IGradient = interface end

type gradient =
    /// Linear gradient with optional angle and position and one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/linear-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// style.backgroundImage.gradient (
    ///     gradient.linearGradient (
    ///         linearGradientOptions.angle (gradientAngle.deg 5),
    ///         linearColorStop.colorStop ("#FF0000"),
    ///         linearColorStopAndHint.colorStop ("#00FF00"),
    ///         linearColorStopAndHint.colorStop (length.px 10, "#0000FF"),
    ///         linearColorStopAndHint.colorStop (length.px 10, "#0000FF")
    ///     )
    /// )
    /// ```
    static member inline linearGradient
        (
            options: ILinearGradientOptions,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("linear-gradient("
             + unbox<string> options
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Linear gradient with one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/linear-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// style.backgroundImage.gradient (
    ///     gradient.linearGradient (
    ///         linearColorStop.colorStop ("#FF0000"),
    ///         linearColorStopAndHint.colorStop ("#00FF00"),
    ///         linearColorStopAndHint.colorStop (length.px 10, "#0000FF"),
    ///         linearColorStopAndHint.colorStop (length.px 10, "#0000FF")
    ///     )
    /// )
    /// ```
    static member inline linearGradient
        (firstColor: ILinearColorStop, [<ParamArray>] otherColors: ILinearColorStopAndHint array)
        : IGradient
        =
        let gradientStr =
            ("linear-gradient("
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Radial gradient with shape and/or size, position and one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.radialGradient (
    ///     radialSizeAndOrShape.sizeAndShape (radialSize.length (length.px 10), radialShape.circle),
    ///     gradientPosition.leftTop,
    ///     linearColorStop.colorStop ("#FF0000"),
    ///     linearColorStopAndHint.colorStop ("#00FF00"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF")
    /// )
    /// ```
    static member inline radialGradient
        (
            shapeAndOrSize: IRadialSizeAndOrShape,
            position: IGradientPosition,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("radial-gradient("
             + unbox<string> shapeAndOrSize
             + " "
             + unbox position
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Radial gradient with position one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.radialGradient (
    ///     gradientPosition.leftTop,
    ///     linearColorStop.colorStop ("#FF0000"),
    ///     linearColorStopAndHint.colorStop ("#00FF00"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF")
    /// )
    /// ```
    static member inline radialGradient
        (
            position: IGradientPosition,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("radial-gradient("
             + unbox position
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Radial gradient with shape and/or size and one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.radialGradient (
    ///     radialSizeAndOrShape.sizeAndShape (radialSize.length (length.px 10), radialShape.circle),
    ///     linearColorStop.colorStop ("#FF0000"),
    ///     linearColorStopAndHint.colorStop ("#00FF00"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF")
    /// )
    /// ```
    static member inline radialGradient
        (
            shapeAndOrSize: IRadialSizeAndOrShape,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("radial-gradient("
             + unbox<string> shapeAndOrSize
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Radial gradient one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.radialGradient (
    ///     linearColorStop.colorStop ("#FF0000"),
    ///     linearColorStopAndHint.colorStop ("#00FF00"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF"),
    ///     linearColorStopAndHint.colorStop (length.px 10, "#0000FF")
    /// )
    /// ```
    static member inline radialGradient
        (firstColor: ILinearColorStop, [<ParamArray>] otherColors: ILinearColorStopAndHint array)
        : IGradient
        =
        let gradientStr =
            ("radial-gradient("
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Conic gradient with angle and position, color interpolation and one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.conicGradient (
    ///     angleAndPosition.position gradientPosition.rightCenter,
    ///     colorInterpolation.polar (polarColorSpaceVariant.hsl),
    ///     angleColorStop.colorStop ("#FF0000"),
    ///     angleColorHintAndStop.colorStop ("#00FF00"),
    ///     angleColorHintAndStop.colorStop (conicColorHintVariant.percentage 10, "#0000FF"),
    ///     angleColorHintAndStop.colorStop (
    ///         conicColorHintVariant.angle (gradientAngle.deg 5),
    ///         "#0000FF"
    ///     )
    /// )
    /// ```
    static member inline conicGradient
        (
            angleAndPosition: IAngleAndPosition,
            colorInterpolation: IColorInterpolation,
            firstColor: IAngleColorStop,
            [<ParamArray>] otherColors: IAngleColorHintAndStop array
        )
        : IGradient
        =
        let gradientStr =
            ("conic-gradient("
             + unbox<string> angleAndPosition
             + " "
             + unbox<string> colorInterpolation
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Conic gradient with angle and position and one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.conicGradient (
    ///     angleAndPosition.position gradientPosition.rightCenter,
    ///     angleColorStop.colorStop ("#FF0000"),
    ///     angleColorHintAndStop.colorStop ("#00FF00"),
    ///     angleColorHintAndStop.colorStop (conicColorHintVariant.percentage 10, "#0000FF"),
    ///     angleColorHintAndStop.colorStop (
    ///         conicColorHintVariant.angle (gradientAngle.deg 5),
    ///         "#0000FF"
    ///     )
    /// )
    /// ```
    static member inline conicGradient
        (
            angleAndPosition: IAngleAndPosition,
            firstColor: IAngleColorStop,
            [<ParamArray>] otherColors: IAngleColorHintAndStop array
        )
        : IGradient
        =
        let gradientStr =
            ("conic-gradient("
             + unbox<string> angleAndPosition
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Conic gradient with color interpolation and one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.conicGradient (
    ///     colorInterpolation.polar (polarColorSpaceVariant.hsl),
    ///     angleColorStop.colorStop ("#FF0000"),
    ///     angleColorHintAndStop.colorStop ("#00FF00"),
    ///     angleColorHintAndStop.colorStop (conicColorHintVariant.percentage 10, "#0000FF"),
    ///     angleColorHintAndStop.colorStop (
    ///         conicColorHintVariant.angle (gradientAngle.deg 5),
    ///         "#0000FF"
    ///     )
    /// )
    /// ```
    static member inline conicGradient
        (
            colorInterpolation: IColorInterpolation,
            firstColor: IAngleColorStop,
            [<ParamArray>] otherColors: IAngleColorHintAndStop array
        )
        : IGradient
        =
        let gradientStr =
            ("conic-gradient("
             + unbox<string> colorInterpolation
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Conic gradient one or multiple colors
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    ///
    /// Usage:
    /// ```fsharp
    /// gradient.conicGradient (
    ///     angleColorStop.colorStop ("#FF0000"),
    ///     angleColorHintAndStop.colorStop ("#00FF00"),
    ///     angleColorHintAndStop.colorStop (conicColorHintVariant.percentage 10, "#0000FF"),
    ///     angleColorHintAndStop.colorStop (
    ///         conicColorHintVariant.angle (gradientAngle.deg 5),
    ///         "#0000FF"
    ///     )
    /// )
    /// ```
    static member inline conicGradient
        (firstColor: IAngleColorStop, [<ParamArray>] otherColors: IAngleColorHintAndStop array)
        : IGradient
        =
        let gradientStr =
            ("conic-gradient("
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as linearGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/linear-gradient
    static member inline repeatingLinearGradient
        (
            options: ILinearGradientOptions,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-linear-gradient("
             + unbox<string> options
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as linearGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/linear-gradient
    static member inline repeatingLinearGradient
        (firstColor: ILinearColorStop, [<ParamArray>] otherColors: ILinearColorStopAndHint array)
        : IGradient
        =
        let gradientStr =
            ("repeating-linear-gradient("
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as radialGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    static member inline repeatingRadialGradient
        (
            shapeAndOrSize: IRadialSizeAndOrShape,
            position: IGradientPosition,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-radial-gradient("
             + unbox<string> shapeAndOrSize
             + " "
             + unbox position
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as radialGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    static member inline repeatingRadialGradient
        (
            position: IGradientPosition,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-radial-gradient("
             + unbox position
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as radialGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    static member inline repeatingRadialGradient
        (
            shapeAndOrSize: IRadialSizeAndOrShape,
            firstColor: ILinearColorStop,
            [<ParamArray>] otherColors: ILinearColorStopAndHint array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-radial-gradient("
             + unbox<string> shapeAndOrSize
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as radialGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/radial-gradient
    static member inline repeatingRadialGradient
        (firstColor: ILinearColorStop, [<ParamArray>] otherColors: ILinearColorStopAndHint array)
        : IGradient
        =
        let gradientStr =
            ("repeating-radial-gradient("
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as conicGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    static member inline repeatingConicGradient
        (
            angleAndPosition: IAngleAndPosition,
            colorInterpolation: IColorInterpolation,
            firstColor: IAngleColorStop,
            [<ParamArray>] otherColors: IAngleColorHintAndStop array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-conic-gradient("
             + unbox<string> angleAndPosition
             + " "
             + unbox<string> colorInterpolation
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as conicGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    static member inline repeatingConicGradient
        (
            angleAndPosition: IAngleAndPosition,
            firstColor: IAngleColorStop,
            [<ParamArray>] otherColors: IAngleColorHintAndStop array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-conic-gradient("
             + unbox<string> angleAndPosition
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as conicGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    static member inline repeatingConicGradient
        (
            colorInterpolation: IColorInterpolation,
            firstColor: IAngleColorStop,
            [<ParamArray>] otherColors: IAngleColorHintAndStop array
        )
        : IGradient
        =
        let gradientStr =
            ("repeating-conic-gradient("
             + unbox<string> colorInterpolation
             + ", "
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr

    /// Same as conicGradient but repeated
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/gradient/conic-gradient
    static member inline repeatingConicGradient
        (firstColor: IAngleColorStop, [<ParamArray>] otherColors: IAngleColorHintAndStop array)
        : IGradient
        =
        let gradientStr =
            ("repeating-conic-gradient("
             + unbox<string> firstColor
             + ", "
             + unbox<string> otherColors
             + ")")
        unbox gradientStr
