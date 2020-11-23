namespace Feliz

open Fable.Core
open Feliz.Styles

type Svg =
    /// SVG Image element, not to be confused with HTML img alias.
    static member inline image xs = Interop.createElement "image" xs
    /// SVG Image element, not to be confused with HTML img alias.
    static member inline image (children: #seq<ReactElement>) = Interop.reactElementWithChildren "image" children
    /// The svg element is a container that defines a new coordinate system and viewport. It is used as the outermost element of SVG documents, but it can also be used to embed an SVG fragment inside an SVG or HTML document.
    static member inline svg xs = Interop.createElement "svg" xs
    /// The svg element is a container that defines a new coordinate system and viewport. It is used as the outermost element of SVG documents, but it can also be used to embed an SVG fragment inside an SVG or HTML document.
    static member inline svg (children: #seq<ReactElement>) = Interop.reactElementWithChildren "svg" children
    static member inline circle xs = Interop.createElement "circle" xs
    static member inline circle (children: #seq<ReactElement>) = Interop.reactElementWithChildren "circle" children
    static member inline clipPath xs = Interop.createElement "clipPath" xs
    static member inline clipPath (children: #seq<ReactElement>) = Interop.reactElementWithChildren "clipPath" children
    /// The <defs> element is used to store graphical objects that will be used at a later time. Objects created inside a <defs> element are not rendered directly. To display them you have to reference them (with a <use> element for example). https://developer.mozilla.org/en-US/docs/Web/SVG/Element/defs
    static member inline defs xs = Interop.createElement "defs" xs
    /// The <defs> element is used to store graphical objects that will be used at a later time. Objects created inside a <defs> element are not rendered directly. To display them you have to reference them (with a <use> element for example). https://developer.mozilla.org/en-US/docs/Web/SVG/Element/defs
    static member inline defs (children: #seq<ReactElement>) = Interop.reactElementWithChildren "defs" children
    /// The <desc> element provides an accessible, long-text description of any SVG container element or graphics element.
    static member inline desc xs = Interop.createElement "desc" xs
    /// The <desc> element provides an accessible, long-text description of any SVG container element or graphics element.
    static member inline desc (value: string) = Interop.reactElementWithChild "desc" value
    static member inline ellipse xs = Interop.createElement "ellipse" xs
    static member inline ellipse (children: #seq<ReactElement>) = Interop.reactElementWithChildren "ellipse" children
    static member inline feBlend xs = Interop.createElement "feBlend" xs
    static member inline feBlend (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feBlend" children
    static member inline feColorMatrix xs = Interop.createElement "feColorMatrix" xs
    static member inline feColorMatrix (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feColorMatrix" children
    static member inline feComponentTransfer xs = Interop.createElement "feComponentTransfer" xs
    static member inline feComponentTransfer (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feComponentTransfer" children
    static member inline feComposite xs = Interop.createElement "feComposite" xs
    static member inline feComposite (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feComposite" children
    static member inline feConvolveMatrix xs = Interop.createElement "feConvolveMatrix" xs
    static member inline feConvolveMatrix (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feConvolveMatrix" children
    static member inline feDiffuseLighting xs = Interop.createElement "feDiffuseLighting" xs
    static member inline feDiffuseLighting (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feDiffuseLighting" children
    static member inline feDisplacementMap xs = Interop.createElement "feDisplacementMap" xs
    static member inline feDisplacementMap (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feDisplacementMap" children
    static member inline feDistantLight xs = Interop.createElement "feDistantLight" xs
    static member inline feDistantLight (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feDistantLight" children
    static member inline feDropShadow xs = Interop.createElement "feDropShadow" xs
    static member inline feDropShadow (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feDropShadow" children
    static member inline feFlood xs = Interop.createElement "feFlood" xs
    static member inline feFlood (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feFlood" children
    static member inline feFuncA xs = Interop.createElement "feFuncA" xs
    static member inline feFuncA (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feFuncA" children
    static member inline feFuncB xs = Interop.createElement "feFuncB" xs
    static member inline feFuncB (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feFuncB" children
    static member inline feFuncG xs = Interop.createElement "feFuncG" xs
    static member inline feFuncG (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feFuncG" children
    static member inline feFuncR xs = Interop.createElement "feFuncR" xs
    static member inline feFuncR (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feFuncR" children
    static member inline feGaussianBlur xs = Interop.createElement "feGaussianBlur" xs
    static member inline feGaussianBlur (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feGaussianBlur" children
    static member inline feImage xs = Interop.createElement "feImage" xs
    static member inline feImage (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feImage" children
    static member inline feMerge xs = Interop.createElement "feMerge" xs
    static member inline feMerge (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feMerge" children
    static member inline feMergeNode xs = Interop.createElement "feMergeNode" xs
    static member inline feMergeNode (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feMergeNode" children
    static member inline feMorphology xs = Interop.createElement "feMorphology" xs
    static member inline feMorphology (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feMorphology" children
    static member inline feOffset xs = Interop.createElement "feOffset" xs
    static member inline feOffset (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feOffset" children
    static member inline fePointLight xs = Interop.createElement "fePointLight" xs
    static member inline fePointLight (children: #seq<ReactElement>) = Interop.reactElementWithChildren "fePointLight" children
    static member inline feSpecularLighting xs = Interop.createElement "feSpecularLighting" xs
    static member inline feSpecularLighting (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feSpecularLighting" children
    static member inline feSpotLight xs = Interop.createElement "feSpotLight" xs
    static member inline feSpotLight (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feSpotLight" children
    static member inline feTile xs = Interop.createElement "feTile" xs
    static member inline feTile (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feTile" children
    static member inline feTurbulence xs = Interop.createElement "feTurbulence" xs
    static member inline feTurbulence (children: #seq<ReactElement>) = Interop.reactElementWithChildren "feTurbulence" children
    static member inline filter xs = Interop.createElement "filter" xs
    static member inline filter (children: #seq<ReactElement>) = Interop.reactElementWithChildren "filter" children
    static member inline foreignObject xs = Interop.createElement "foreignObject" xs
    static member inline foreignObject (children: #seq<ReactElement>) = Interop.reactElementWithChildren "foreignObject" children
    /// The <g> SVG element is a container used to group other SVG elements.
    ///
    /// Transformations applied to the <g> element are performed on its child elements, and its attributes are inherited by its children. It can also group multiple elements to be referenced later with the <use> element.
    static member inline g xs = Interop.createElement "g" xs
    /// The <g> SVG element is a container used to group other SVG elements.
    ///
    /// Transformations applied to the <g> element are performed on its child elements, and its attributes are inherited by its children. It can also group multiple elements to be referenced later with the <use> element.
    static member inline g (children: #seq<ReactElement>) = Interop.reactElementWithChildren "g" children
    static member inline line xs = Interop.createElement "line" xs
    static member inline line (children: #seq<ReactElement>) = Interop.reactElementWithChildren "line" children
    static member inline linearGradient xs = Interop.createElement "linearGradient" xs
    static member inline linearGradient (children: #seq<ReactElement>) = Interop.reactElementWithChildren "linearGradient" children
    /// The <marker> element defines the graphic that is to be used for drawing arrowheads or polymarkers on a given <path>, <line>, <polyline> or <polygon> element.
    static member inline marker xs = Interop.createElement "marker" xs
    /// The <marker> element defines the graphic that is to be used for drawing arrowheads or polymarkers on a given <path>, <line>, <polyline> or <polygon> element.
    static member inline marker (children: #seq<ReactElement>) = Interop.reactElementWithChildren "marker" children
    static member inline mask xs = Interop.createElement "marker" xs
    static member inline mask (children: #seq<ReactElement>) = Interop.reactElementWithChildren "marker" children
    static member inline mpath xs = Interop.createElement "mpath" xs
    static member inline mpath (children: #seq<ReactElement>) = Interop.reactElementWithChildren "mpath" children
    static member inline path xs = Interop.createElement "path" xs
    static member inline path (children: #seq<ReactElement>) = Interop.reactElementWithChildren "path" children
    static member inline pattern xs = Interop.createElement "pattern" xs
    static member inline pattern (children: #seq<ReactElement>) = Interop.reactElementWithChildren "pattern" children
    static member inline polygon xs = Interop.createElement "polygon" xs
    static member inline polygon (children: #seq<ReactElement>) = Interop.reactElementWithChildren "polygon" children
    static member inline polyline xs = Interop.createElement "polyline" xs
    static member inline polyline (children: #seq<ReactElement>) = Interop.reactElementWithChildren "polyline" children
    static member inline set xs = Interop.createElement "set" xs
    static member inline set (children: #seq<ReactElement>) = Interop.reactElementWithChildren "set" children
    static member inline stop xs = Interop.createElement "stop" xs
    static member inline stop (children: #seq<ReactElement>) = Interop.reactElementWithChildren "stop" children
    static member inline style xs = Interop.createElement "style" xs
    static member inline style (value: string) = Interop.reactElementWithChild "style" value
    static member inline switch xs = Interop.createElement "switch" xs
    static member inline switch (children: #seq<ReactElement>) = Interop.reactElementWithChildren "switch" children
    static member inline symbol xs = Interop.createElement "symbol" xs
    static member inline symbol (children: #seq<ReactElement>) = Interop.reactElementWithChildren "symbol" children
    static member inline text xs = Interop.createElement "text" xs
    static member inline text (content: string) = Interop.reactElementWithChild "text" content
    static member inline title xs = Interop.createElement "title" xs
    static member inline title (content: string) = Interop.reactElementWithChild "title" content
    static member inline textPath xs = Interop.createElement "textPath" xs
    static member inline textPath (children: #seq<ReactElement>) = Interop.reactElementWithChildren "textPath" children
    static member inline tspan xs = Interop.createElement "tspan" xs
    static member inline tspan (children: #seq<ReactElement>) = Interop.reactElementWithChildren "tspan" children
    static member inline use' xs = Interop.createElement "use" xs
    static member inline use' (children: #seq<ReactElement>) = Interop.reactElementWithChildren "use" children
    static member inline radialGradient xs = Interop.createElement "radialGradient" xs
    static member inline radialGradient (children: #seq<ReactElement>) = Interop.reactElementWithChildren "radialGradient" children
    static member inline rect xs = Interop.createElement "rect" xs
    static member inline rect (children: #seq<ReactElement>) = Interop.reactElementWithChildren "rect" children
    static member inline view xs = Interop.createElement "view" xs
    static member inline view (children: #seq<ReactElement>) = Interop.reactElementWithChildren "view" children

type svg =
    /// Controls the amplitude of the gamma function of a component transfer element when
    /// its type attribute is gamma.
    static member inline amplitude (value: float) = Interop.svgAttribute "amplitude" value
    /// Controls the amplitude of the gamma function of a component transfer element when
    /// its type attribute is gamma.
    static member inline amplitude (value: int) = Interop.svgAttribute "amplitude" value
    /// Indicates the name of the CSS property or attribute of the target element
    /// that is going to be changed during an animation.
    static member inline attributeName (value: string) = Interop.svgAttribute "attributeName" value
    /// Specifies the direction angle for the light source on the XY plane (clockwise),
    /// in degrees from the x axis.
    static member inline azimuth (value: float) = Interop.svgAttribute "azimuth" value
    /// Specifies the direction angle for the light source on the XY plane (clockwise),
    /// in degrees from the x axis.
    static member inline azimuth (value: int) = Interop.svgAttribute "azimuth" value
    /// Represents the base frequency parameter for the noise function of the
    /// <feTurbulence> filter primitive.
    static member inline baseFrequency (value: float) = Interop.mkAttr "baseFrequency" value
    /// Represents the base frequency parameter for the noise function of the
    /// <feTurbulence> filter primitive.
    static member inline baseFrequency (value: int) = Interop.mkAttr "baseFrequency" value
    /// Represents the base frequency parameter for the noise function of the
    /// <feTurbulence> filter primitive.
    static member inline baseFrequency (horizontal: float, vertical: float) = Interop.mkAttr "baseFrequency" (unbox<string> horizontal  + "," + unbox<string> vertical)
    /// Represents the base frequency parameter for the noise function of the
    /// <feTurbulence> filter primitive.
    static member inline baseFrequency (horizontal: float, vertical: int) = Interop.mkAttr "baseFrequency" (unbox<string> horizontal  + "," + unbox<string> vertical)
    /// Represents the base frequency parameter for the noise function of the
    /// <feTurbulence> filter primitive.
    static member inline baseFrequency (horizontal: int, vertical: float) = Interop.mkAttr "baseFrequency" (unbox<string> horizontal  + "," + unbox<string> vertical)
    /// Represents the base frequency parameter for the noise function of the
    /// <feTurbulence> filter primitive.
    static member inline baseFrequency (horizontal: int, vertical: int) = Interop.mkAttr "baseFrequency" (unbox<string> horizontal  + "," + unbox<string> vertical)
    /// The SVG cx attribute define the x-axis coordinate of a center point.
    ///
    /// Three elements are using this attribute: <circle>, <ellipse>, and <radialGradient>
    static member inline cx (value: float) = Interop.mkAttr "cx" value
    /// The SVG cx attribute define the x-axis coordinate of a center point.
    ///
    /// Three elements are using this attribute: <circle>, <ellipse>, and <radialGradient>
    static member inline cx (value: ICssUnit) = Interop.mkAttr "cx" value
    /// The SVG cx attribute define the x-axis coordinate of a center point.
    ///
    /// Three elements are using this attribute: <circle>, <ellipse>, and <radialGradient>
    static member inline cx (value: int) = Interop.mkAttr "cx" value

    /// The SVG cy attribute define the y-axis coordinate of a center point.
    ///
    /// Three elements are using this attribute: <circle>, <ellipse>, and <radialGradient>
    static member inline cy (value: float) = Interop.mkAttr "cy" value
    /// The SVG cy attribute define the y-axis coordinate of a center point.
    ///
    /// Three elements are using this attribute: <circle>, <ellipse>, and <radialGradient>
    static member inline cy (value: ICssUnit) = Interop.mkAttr "cy" value
    /// The SVG cy attribute define the y-axis coordinate of a center point.
    ///
    /// Three elements are using this attribute: <circle>, <ellipse>, and <radialGradient>
    static member inline cy (value: int) = Interop.mkAttr "cy" value
    /// SVG attribute to indicate a shift along the x-axis on the position of an element or its content.
    static member inline dx (value: float) = Interop.svgAttribute "dx" value
    /// SVG attribute to indicate a shift along the x-axis on the position of an element or its content.
    static member inline dx (value: int) = Interop.svgAttribute "dx" value

    /// SVG attribute to indicate a shift along the y-axis on the position of an element or its content.
    static member inline dy (value: float) = Interop.svgAttribute "dy" value
    /// SVG attribute to indicate a shift along the y-axis on the position of an element or its content.
    static member inline dy (value: int) = Interop.svgAttribute "dy" value
    /// Defines a SVG path to be drawn.
    static member inline d (path: seq<char * (float list list)>) =
        PropHelpers.createSvgPathFloat path
        |> Interop.svgAttribute "d"
    /// Defines a SVG path to be drawn.
    static member inline d (path: seq<char * (int list list)>) =
        PropHelpers.createSvgPathInt path
        |> Interop.svgAttribute "d"
    /// Defines a SVG path to be drawn.
    static member inline d (path: string) = Interop.svgAttribute "d" path

module svg =
    [<Erase>]
    /// Controls whether or not an animation is cumulative.
    type accumulate =
        /// Specifies that repeat iterations are not cumulative.
        static member inline none = Interop.svgAttribute "accumulate" "none"
        /// Specifies that each repeat iteration after the first builds upon
        /// the last value of the previous iteration.
        static member inline sum = Interop.svgAttribute "accumulate" "sum"

    /// Controls whether or not an animation is additive.
    [<Erase>]
    type additive =
        /// Specifies that the animation will override the underlying value of
        /// the attribute and other lower priority animations.
        static member inline replace = Interop.svgAttribute "additive" "replace"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline sum = Interop.svgAttribute "additive" "sum"

    /// Controls whether or not an animation is additive.
    [<Erase>]
    type alignmentBaseline =
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline alphabetic = Interop.svgAttribute "alignment-baseline" "alphabetic"
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline baseline = Interop.svgAttribute "alignment-baseline" "baseline"
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline bottom = Interop.svgAttribute "alignment-baseline" "bottom"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline center = Interop.svgAttribute "alignment-baseline" "center"
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline central = Interop.svgAttribute "alignment-baseline" "central"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline hanging = Interop.svgAttribute "alignment-baseline" "hanging"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline ideographic = Interop.svgAttribute "alignment-baseline" "ideographic"
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline mathematical = Interop.svgAttribute "alignment-baseline" "mathematical"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline middle = Interop.svgAttribute "alignment-baseline" "middle"
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline textAfterEdge = Interop.svgAttribute "alignment-baseline" "text-after-edge"
        /// Uses the dominant baseline choice of the parent. Matches the box’s
        /// corresponding baseline to that of its parent.
        static member inline textBeforeEdge = Interop.svgAttribute "alignment-baseline" "text-before-edge"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline textBottom = Interop.svgAttribute "alignment-baseline" "text-bottom"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline textTop = Interop.svgAttribute "alignment-baseline" "text-top"
        /// Specifies that the animation will add to the underlying value of
        /// the attribute and other lower priority animations.
        static member inline top = Interop.svgAttribute "alignment-baseline" "top"