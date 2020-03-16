namespace Feliz

open Browser.Types
open Fable.Core.JsInterop
open Fable.Core
open Feliz.Styles

[<StringEnum; RequireQualifiedAccess>]
type AriaDropEffect =
    /// A duplicate of the source object will be dropped into the target.
    | Copy
    /// A function supported by the drop target is executed, using the drag
    /// source as an input.
    | Execute
    /// A reference or shortcut to the dragged object will be created in the
    /// target object.
    | Link
    /// The source object will be removed from its current location and dropped
    /// into the target.
    | Move
    /// No operation can be performed; effectively cancels the drag operation if
    /// an attempt is made to drop on this object. Ignored if combined with any
    /// other token value. e.g. 'none copy' is equivalent to a 'copy' value.
    | None
    /// There is a popup menu or dialog that allows the user to choose one of
    /// the drag operations (copy, move, link, execute) and any other drag
    /// functionality, such as cancel.
    | Popup

[<StringEnum; RequireQualifiedAccess>]
type AriaRelevant =
    /// Element nodes are added to the DOM within the live region.
    | Additions
    /// Equivalent to the combination of all values, "additions removals text".
    | All
    /// Text or element nodes within the live region are removed from the DOM.
    | Removals
    /// Text is added to any DOM descendant nodes of the live region.
    | Text

/// Represents the native Html properties.
type prop =
    /// List of types the server accepts, typically a file type.
    static member inline accept (value: string) = Interop.mkAttr "accept" value

    /// List of supported charsets.
    static member inline acceptCharset (value: string) = Interop.mkAttr "acceptCharset" value

    /// Defines a keyboard shortcut to activate or add focus to the element.
    static member inline accessKey (value: string) = Interop.mkAttr "accessKey" value

    /// The URI of a program that processes the information submitted via the form.
    static member inline action (value: string) = Interop.mkAttr "action" value

    /// Alternative text in case an image can't be displayed.
    static member inline alt (value: string) = Interop.mkAttr "alt" value

    /// Identifies the currently active descendant of a `composite` widget.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-activedescendant
    static member inline ariaActiveDescendant (id: string) = Interop.mkAttr "aria-activedescendant" id

    /// Indicates whether assistive technologies will present all, or only parts
    /// of, the changed region based on the change notifications defined by the
    /// `aria-relevant` attribute.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-atomic
    static member inline ariaAtomic (value: bool) = Interop.mkAttr "aria-atomic" value

    /// Indicates whether an element, and its subtree, are currently being
    /// updated.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-busy
    static member inline ariaBusy (value: bool) = Interop.mkAttr "aria-busy" value

    /// Indicates the current "checked" state of checkboxes, radio buttons, and
    /// other widgets. See related `aria-pressed` and `aria-selected`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-checked
    static member inline ariaChecked (value: bool) = Interop.mkAttr "aria-checked" value

    /// Identifies the element (or elements) whose contents or presence are
    /// controlled by the current element. See related `aria-owns`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-controls
    static member inline ariaControls ([<System.ParamArray>] ids: string []) = Interop.mkAttr "aria-controls" (String.concat " " ids)

    /// Specifies a URI referencing content that describes the object. See
    /// related `aria-describedby`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-describedat
    static member inline ariaDescribedAt (uri: string) = Interop.mkAttr "aria-describedat" uri

    /// Identifies the element (or elements) that describes the object. See
    /// related `aria-describedat` and `aria-labelledby`.
    ///
    /// The `aria-labelledby` attribute is similar to `aria-describedby` in that
    /// both reference other elements to calculate a text alternative, but a
    /// label should be concise, where a description is intended to provide more
    /// verbose information.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-describedby
    static member inline ariaDescribedBy ([<System.ParamArray>] ids: string []) = Interop.mkAttr "aria-describedby" (String.concat " " ids)

    /// Indicates that the element is perceivable but disabled, so it is not
    /// editable or otherwise operable. See related `aria-hidden` and
    /// `aria-readonly`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-disabled
    static member inline ariaDisabled (value: bool) = Interop.mkAttr "aria-disabled" value

    /// Indicates what functions can be performed when the dragged object is
    /// released on the drop target. This allows assistive technologies to
    /// convey the possible drag options available to users, including whether a
    /// pop-up menu of choices is provided by the application. Typically, drop
    /// effect functions can only be provided once an object has been grabbed
    /// for a drag operation as the drop effect functions available are
    /// dependent on the object being dragged.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-dropeffect
    static member inline ariaDropEffect ([<System.ParamArray>] values: AriaDropEffect []) = Interop.mkAttr "aria-dropeffect" (values |> unbox<string []> |> String.concat " ")

    /// Indicates whether the element, or another grouping element it controls,
    /// is currently expanded or collapsed.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-expanded
    static member inline ariaExpanded (value: bool) = Interop.mkAttr "aria-expanded" value

    /// Identifies the next element (or elements) in an alternate reading order
    /// of content which, at the user's discretion, allows assistive technology
    /// to override the general default of reading in document source order.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-flowto
    static member inline ariaFlowTo ([<System.ParamArray>] ids: string []) = Interop.mkAttr "aria-flowto" (String.concat " " ids)

    /// Indicates an element's "grabbed" state in a drag-and-drop operation.
    ///
    /// When it is set to true it has been selected for dragging, false
    /// indicates that the element can be grabbed for a drag-and-drop operation,
    /// but is not currently grabbed, and undefined (or no value) indicates the
    /// element cannot be grabbed (default).
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-grabbed
    static member inline ariaGrabbed (value: bool) = Interop.mkAttr "aria-grabbed" value

    /// Indicates that the element has a popup context menu or sub-level menu.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-haspopup
    static member inline ariaHasPopup (value: bool) = Interop.mkAttr "aria-haspopup" value

    /// Indicates that the element and all of its descendants are not visible or
    /// perceivable to any user as implemented by the author. See related
    /// `aria-disabled`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-hidden
    static member inline ariaHidden (value: bool) = Interop.mkAttr "aria-hidden" value

    /// Indicates the entered value does not conform to the format expected by
    /// the application.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-invalid
    static member inline ariaInvalid (value: bool) = Interop.mkAttr "aria-invalid" value

    /// Defines a string value that labels the current element. See related
    /// `aria-labelledby`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-label
    static member inline ariaLabel (value: string) = Interop.mkAttr "aria-label" value

    /// Defines the hierarchical level of an element within a structure.
    ///
    /// This can be applied inside trees to tree items, to headings inside a
    /// document, to nested grids, nested tablists and to other structural items
    /// that may appear inside a container or participate in an ownership
    /// hierarchy. The value for `aria-level` is an integer greater than or
    /// equal to 1.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-level
    static member inline ariaLevel (value: int) = Interop.mkAttr "aria-level" value

    /// Identifies the element (or elements) that labels the current element.
    /// See related `aria-label` and `aria-describedby`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-labelledby
    static member inline ariaLabelledBy ([<System.ParamArray>] ids: string []) = Interop.mkAttr "aria-labelledby" (String.concat " " ids)

    /// Indicates whether a text box accepts multiple lines of input or only a
    /// single line.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-multiline
    static member inline ariaMultiLine (value: bool) = Interop.mkAttr "aria-multiline" value

    /// Indicates that the user may select more than one item from the current
    /// selectable descendants.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-multiselectable
    static member inline ariaMultiSelectable (value: bool) = Interop.mkAttr "aria-multiselectable" value

    /// Identifies an element (or elements) in order to define a visual,
    /// functional, or contextual parent/child relationship between DOM elements
    /// where the DOM hierarchy cannot be used to represent the relationship.
    /// See related `aria-controls`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-owns
    static member inline ariaOwns ([<System.ParamArray>] ids: string []) = Interop.mkAttr "aria-owns" (String.concat " " ids)

    /// Indicates the current "pressed" state of toggle buttons. See related
    /// `aria-checked` and `aria-selected`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-pressed
    static member inline ariaPressed (value: bool) = Interop.mkAttr "aria-pressed" value

    /// Defines an element's number or position in the current set of listitems
    /// or treeitems. Not required if all elements in the set are present in the
    /// DOM. See related `aria-setsize`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-posinset
    static member inline ariaPosInSet (value: int) = Interop.mkAttr "aria-posinset" value

    /// Indicates that the element is not editable, but is otherwise operable.
    /// See related `aria-disabled`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-readonly
    static member inline ariaReadOnly (value: bool) = Interop.mkAttr "aria-readonly" value

    /// Indicates what user agent change notifications (additions, removals,
    /// etc.) assistive technologies will receive within a live region. See
    /// related `aria-atomic`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-relevant
    static member inline ariaRelevant ([<System.ParamArray>] values: AriaRelevant []) = Interop.mkAttr "aria-relevant" (values |> unbox<string []> |> String.concat " ")

    /// Indicates that user input is required on the element before a form may
    /// be submitted.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-required
    static member inline ariaRequired (value: bool) = Interop.mkAttr "aria-required" value
    
    /// Indicates the current "selected" state of various widgets. See related
    /// `aria-checked` and `aria-pressed`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-selected
    static member inline ariaSelected (value: bool) = Interop.mkAttr "aria-selected" value
    
    /// Defines the maximum allowed value for a range widget.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuemax
    static member inline ariaValueMax (value: float) = Interop.mkAttr "aria-valuemax" value
    /// Defines the maximum allowed value for a range widget.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuemax
    static member inline ariaValueMax (value: int) = Interop.mkAttr "aria-valuemax" value
    
    /// Defines the minimum allowed value for a range widget.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuemin
    static member inline ariaValueMin (value: float) = Interop.mkAttr "aria-valuemin" value
    /// Defines the minimum allowed value for a range widget.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuemin
    static member inline ariaValueMin (value: int) = Interop.mkAttr "aria-valuemin" value

    /// Defines the current value for a range widget. See related
    /// `aria-valuetext`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuenow
    static member inline ariaValueNow (value: float) = Interop.mkAttr "aria-valuenow" value
    /// Defines the current value for a range widget. See related
    /// `aria-valuetext`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuenow
    static member inline ariaValueNow (value: int) = Interop.mkAttr "aria-valuenow" value

    /// Defines the human readable text alternative of `aria-valuenow` for a
    /// range widget.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-valuetext
    static member inline ariaValueText (value: string) = Interop.mkAttr "aria-valuetext" value
    
    /// Defines the number of items in the current set of listitems or
    /// treeitems. Not required if all elements in the set are present in the
    /// DOM. See related `aria-posinset`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-setsize
    static member inline ariaSetSize (value: int) = Interop.mkAttr "aria-setsize" value

    /// Indicates that the script should be executed asynchronously.
    static member inline async (value: bool) = Interop.mkAttr "async" value

    /// Indicates whether controls in this form can by default have their values automatically completed by the browser.
    static member inline autoComplete (value: string) = Interop.mkAttr "autoComplete" value

    /// The element should be automatically focused after the page loaded.
    static member inline autoFocus (value: bool) = Interop.mkAttr "autoFocus" value

    /// The audio or video should play as soon as possible.
    static member inline autoPlay (value: bool) = Interop.mkAttr "autoPlay" value

    static member inline capture (value: bool) = Interop.mkAttr "capture" value

    /// Children of this React element.
    static member inline children (value: Fable.React.ReactElement) = Interop.mkAttr "children" value
    /// Children of this React element.
    static member inline children (elems: Fable.React.ReactElement seq) = Interop.mkAttr "children" (Interop.reactApi.Children.toArray elems)

    /// A URL that designates a source document or message for the information quoted. This attribute is intended to 
    /// point to information explaining the context or the reference for the quote.
    static member inline cite (value: string) = Interop.mkAttr "cite" value

    /// Specifies a CSS class for this element.
    static member inline className (value: string) = Interop.mkAttr "className" value
    /// Takes a list of conditional classes (`predicate:bool` * `className:string`), filters out the ones where the 
    /// `predicate` is false and joins the rest of them using a space to combine the classses into a single class property.
    ///
    ///`prop.className [ true, "one";  false, "two" ]`
    ///
    /// is the same as
    ///
    ///`prop.className "one"`
    ///
    static member inline className (classes: #seq<bool * string>) =
        classes
        |> Seq.filter fst
        |> Seq.map snd
        |> String.concat " "
        |> Interop.mkAttr "className"
    /// Takes a `seq<string>` and joins them using a space to combine the classses into a single class property.
    ///
    /// `prop.className [ "one"; "two" ]`
    ///
    /// is the same as
    ///
    /// `prop.className "one two"`
    static member inline className (names: seq<string>) = Interop.mkAttr "className" (String.concat " " names)

    /// Takes a `seq<string>` and joins them using a space to combine the classses into a single class property.
    ///
    /// `prop.classes [ "one"; "two" ]` => `prop.className "one two"`
    static member inline classes (names: seq<string>) = Interop.mkAttr "className" (String.concat " " names)

    /// Defines the number of columns in a textarea.
    static member inline cols (value: int) = Interop.mkAttr "cols" value

    /// Defines the number of columns a cell should span.
    static member inline colSpan (value: int) = Interop.mkAttr "colSpan" value

    /// A value associated with http-equiv or name depending on the context.
    static member inline content (value: string) = Interop.mkAttr "content" value

    /// Indicates whether the element's content is editable.
    static member inline contentEditable (value: bool) = Interop.mkAttr "contenteditable" value

    /// If true, the browser will offer controls to allow the user to control video playback, 
    /// including volume, seeking, and pause/resume playback.
    static member inline controls (value: bool) = Interop.mkAttr "controls" value

    static member inline custom (key: string, value: 't) = Interop.mkAttr key value

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

    /// Sets the inner Html content of the element.
    static member inline dangerouslySetInnerHTML (content: string) = Interop.mkAttr "dangerouslySetInnerHTML" (createObj [ "__html" ==> content ])

    /// This attribute indicates the time and/or date of the element.
    static member inline dateTime (value: string) = Interop.mkAttr "datetime" value

    /// Sets the DOM defaultChecked value when initially rendered.
    ///
    /// Typically only used with uncontrolled components.
    static member inline defaultChecked (value: bool) = Interop.mkAttr "defaultChecked" value

    /// Sets the DOM defaultValue value when initially rendered.
    ///
    /// Typically only used with uncontrolled components.
    static member inline defaultValue (value: bool) = Interop.mkAttr "defaultValue" value
    /// Sets the DOM defaultValue value when initially rendered.
    ///
    /// Typically only used with uncontrolled components.
    static member inline defaultValue (value: float) = Interop.mkAttr "defaultValue" value
    /// Sets the DOM defaultValue value when initially rendered.
    ///
    /// Typically only used with uncontrolled components.
    static member inline defaultValue (value: int) = Interop.mkAttr "defaultValue" value
    /// Sets the DOM defaultValue value when initially rendered.
    ///
    /// Typically only used with uncontrolled components.
    static member inline defaultValue (value: string) = Interop.mkAttr "defaultValue" value

    /// Indicates to a browser that the script is meant to be executed after the document 
    /// has been parsed, but before firing DOMContentLoaded.
    /// 
    /// Scripts with the defer attribute will prevent the DOMContentLoaded event from 
    /// firing until the script has loaded and finished evaluating.
    ///
    /// This attribute must not be used if the src attribute is absent (i.e. for inline scripts), 
    /// in this case it would have no effect.
    static member inline defer (value: bool) = Interop.mkAttr "defer" value

    /// Indicates whether the user can interact with the element.
    static member inline disabled (value: bool) = Interop.mkAttr "disabled" value

    /// This attribute, if present, indicates that the author intends the hyperlink to be used for downloading a resource.
    static member inline download (value: bool) = Interop.mkAttr "download" value

    /// Indicates whether the the element can be dragged.
    static member inline draggable (value: bool) = Interop.mkAttr "draggable" value

    /// SVG attribute to indicate a shift along the y-axis on the position of an element or its content.
    static member inline dy (value: float) = Interop.mkAttr "dy" value
    /// SVG attribute to indicate a shift along the y-axis on the position of an element or its content.
    static member inline dy (value: int) = Interop.mkAttr "dy" value

    /// The fill attribute has two different meanings. For shapes and text it's a presentation
    /// attribute that defines the color (or any SVG paint servers like gradients or patterns) 
    /// used to paint the element; for animation it defines the final state of the animation.
    ///
    /// As a presentation attribute, it can be applied to any element but it only has an effect 
    /// on the following eleven elements: <altGlyph>, <circle>, <ellipse>, <path>, <polygon>, 
    /// <polyline>, <rect>, <text>, <textPath>, <tref>, and <tspan>.
    ///
    /// For animation five elements are using this attribute: <animate>, <animateColor>, 
    /// <animateMotion>, <animateTransform>, and <set>.
    static member inline fill (color: string) = Interop.mkAttr "fill" color

    /// SVG attribute to define the opacity of the paint server (color, gradient, pattern, etc) applied to a shape.
    static member inline fillOpacity (value: float) = Interop.mkAttr "fillOpacity" value
    /// SVG attribute to define the opacity of the paint server (color, gradient, pattern, etc) applied to a shape.
    static member inline fillOpacity (value: int) = Interop.mkAttr "fillOpacity" value

    /// SVG attribute to define the size of the font from baseline to baseline when multiple 
    /// lines of text are set solid in a multiline layout environment.
    static member inline fontSize (value: float) = Interop.mkAttr "fontSize" value
    /// SVG attribute to define the size of the font from baseline to baseline when multiple 
    /// lines of text are set solid in a multiline layout environment.
    static member inline fontSize (value: int) = Interop.mkAttr "fontSize" value

    /// A space-separated list of other elements’ ids, indicating that those elements contributed input 
    /// values to (or otherwise affected) the calculation.
    static member inline for' (value: string) = Interop.mkAttr "for" value
    /// A space-separated list of other elements’ ids, indicating that those elements contributed input 
    /// values to (or otherwise affected) the calculation.
    static member inline for' (ids: #seq<string>) = Interop.mkAttr "for" (ids |> String.concat " ")

    /// The <form> element to associate the <meter> element with (its form owner). The value of this 
    /// attribute must be the id of a <form> in the same document. If this attribute is not set, the 
    /// <button> is associated with its ancestor <form> element, if any. This attribute is only used 
    /// if the <meter> element is being used as a form-associated element, such as one displaying a 
    /// range corresponding to an <input type="number">.
    static member inline form (value: string) = Interop.mkAttr "form" value
    
    /// Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
    static member inline hidden (value: bool) = Interop.mkAttr "hidden" value

    /// Specifies the height of elements listed here. For all other elements, use the CSS height property.
    ///
    /// <canvas>, <embed>, <iframe>, <img>, <input>, <object>, <video>
    static member inline height (value: int) = Interop.mkAttr "height" value

    /// The lower numeric bound of the high end of the measured range. This must be less than the 
    /// maximum value (max attribute), and it also must be greater than the low value and minimum 
    /// value (low attribute and min attribute, respectively), if any are specified. If unspecified, 
    /// or if greater than the maximum value, the high value is equal to the maximum value.
    static member inline high (value: float) = Interop.mkAttr "high" value
    /// The lower numeric bound of the high end of the measured range. This must be less than the 
    /// maximum value (max attribute), and it also must be greater than the low value and minimum 
    /// value (low attribute and min attribute, respectively), if any are specified. If unspecified, 
    /// or if greater than the maximum value, the high value is equal to the maximum value.
    static member inline high (value: int) = Interop.mkAttr "high" value

    /// The URL of a linked resource.
    static member inline href (value: string) = Interop.mkAttr "href" value

    /// Indicates the language of the linked resource. Allowed values are determined by BCP47. 
    ///
    /// Use this attribute only if the href attribute is present.
    static member inline hrefLang (value: string) = Interop.mkAttr "hreflang" value

    static member inline htmlFor (value: string) = Interop.mkAttr "htmlFor" value

    /// Often used with CSS to style a specific element. The value of this attribute must be unique.
    static member inline id (value: int) = Interop.mkAttr "id" (string value)
    /// Often used with CSS to style a specific element. The value of this attribute must be unique.
    static member inline id (value: string) = Interop.mkAttr "id" value

    /// Alias for `dangerouslySetInnerHTML`, sets the inner Html content of the element.
    static member inline innerHtml (content: string) = Interop.mkAttr "dangerouslySetInnerHTML" (createObj [ "__html" ==> content ])

    /// Contains inline metadata that a user agent can use to verify that a fetched resource 
    /// has been delivered free of unexpected manipulation.
    static member inline integrity (value: string) = Interop.mkAttr "integrity" value

    static member inline isChecked (value: bool) = Interop.mkAttr "checked" value

    static member inline isOpen (value: bool) = Interop.mkAttr "open" value
    
    /// A special string attribute you need to include when creating arrays of elements. 
    /// Keys help React identify which items have changed, are added, or are removed. 
    /// Keys should be given to the elements inside an array to give the elements a stable identity.
    /// 
    /// Keys only need to be unique among sibling elements in the same array. They don’t need to 
    /// be unique across the whole application or even a single component.
    static member inline key (value: System.Guid) = Interop.mkAttr "value" (string value)
    /// A special string attribute you need to include when creating arrays of elements. Keys help 
    /// React identify which items have changed, are added, or are removed. Keys should be given 
    /// to the elements inside an array to give the elements a stable identity.
    /// 
    /// Keys only need to be unique among sibling elements in the same array. They don’t need to 
    /// be unique across the whole application or even a single component.
    static member inline key (value: int) = Interop.mkAttr "key" value
    /// A special string attribute you need to include when creating arrays of elements. Keys 
    /// help React identify which 
    /// items have changed, are added, or are removed. Keys should be given to the elements 
    /// inside an array to give the elements a stable identity.
    /// 
    /// Keys only need to be unique among sibling elements in the same array. They don’t need to 
    /// be unique across the whole application or even a single component.
    static member inline key (value: string) = Interop.mkAttr "key" value

    /// Helps define the language of an element: the language that non-editable elements are 
    /// written in, or the language that the editable elements should be written in by the user.
    static member inline lang (value: string) = Interop.mkAttr "lang" value

    /// If true, the browser will automatically seek back to the start upon reaching the end of the video.
    static member inline loop (value: bool) = Interop.mkAttr "loop" value

    /// The upper numeric bound of the low end of the measured range. This must be greater than 
    /// the minimum value (min attribute), and it also must be less than the high value and 
    /// maximum value (high attribute and max attribute, respectively), if any are specified. 
    /// If unspecified, or if less than the minimum value, the low value is equal to the minimum value.
    static member inline low (value: float) = Interop.mkAttr "low" value
    /// The upper numeric bound of the low end of the measured range. This must be greater than 
    /// the minimum value (min attribute), and it also must be less than the high value and 
    /// maximum value (high attribute and max attribute, respectively), if any are specified. 
    /// If unspecified, or if less than the minimum value, the low value is equal to the minimum value.
    static member inline low (value: int) = Interop.mkAttr "low" value

    /// Indicates the maximum value allowed.
    static member inline max (value: float) = Interop.mkAttr "max" value
    /// Indicates the maximum value allowed.
    static member inline max (value: int) = Interop.mkAttr "max" value

    /// Defines the maximum number of characters allowed in the element.
    static member inline maxLength (value: int) = Interop.mkAttr "maxlength" value

    /// This attribute specifies the media that the linked resource applies to. 
    /// Its value must be a media type / media query. This attribute is mainly useful 
    /// when linking to external stylesheets — it allows the user agent to pick the 
    /// best adapted one for the device it runs on.
    ///
    /// In HTML 4, this can only be a simple white-space-separated list of media 
    /// description literals, i.e., media types and groups, where defined and allowed 
    /// as values for this attribute, such as print, screen, aural, braille. HTML5 
    /// extended this to any kind of media queries, which are a superset of the allowed 
    /// values of HTML 4.
    ///
    /// Browsers not supporting CSS3 Media Queries won't necessarily recognize the adequate 
    /// link; do not forget to set fallback links, the restricted set of media queries 
    /// defined in HTML 4.
    static member inline media (value: string) = Interop.mkAttr "media" value

    /// Indicates the minimum value allowed.
    static member inline min (value: float) = Interop.mkAttr "min" value
    /// Indicates the minimum value allowed.
    static member inline min (value: int) = Interop.mkAttr "min" value

    /// Defines the minimum number of characters allowed in the element.
    static member inline minLength (value: int) = Interop.mkAttr "minlength" value

    /// Defines which HTTP method to use when submitting the form. Can be GET (default) or POST.
    static member inline method (value: string) = Interop.mkAttr "method" value

    /// Indicates whether multiple values can be entered in an input of the type email or file.
    static member inline multiple (value: bool) = Interop.mkAttr "multiple" value
    
    /// Indicates whether the audio will be initially silenced on page load.
    static member inline muted (value: bool) = Interop.mkAttr "muted" value
    
    /// Name of the element. 
    ///
    /// For example used by the server to identify the fields in form submits.
    static member inline name (value: string) = Interop.mkAttr "name" value

    /// This Boolean attribute is set to indicate that the script should not be executed in 
    /// browsers that support ES2015 modules — in effect, this can be used to serve fallback 
    /// scripts to older browsers that do not support modular JavaScript code.
    static member inline nomodule (value: bool) = Interop.mkAttr "nomodule" value

    /// A cryptographic nonce (number used once) to whitelist scripts in a script-src 
    /// Content-Security-Policy. The server must generate a unique nonce value each time 
    /// it transmits a policy. It is critical to provide a nonce that cannot be guessed 
    /// as bypassing a resource's policy is otherwise trivial.
    static member inline nonce (value: string) = Interop.mkAttr "nonce" value

    /// SVG attribute to define where the gradient color will begin or end.
    static member inline offset (value: float) = Interop.mkAttr "offset" value
    /// SVG attribute to define where the gradient color will begin or end.
    static member inline offset (value: ICssUnit) = Interop.mkAttr "offset" value
    /// SVG attribute to define where the gradient color will begin or end.
    static member inline offset (value: int) = Interop.mkAttr "offset" value

    /// Fires when a media event is aborted.
    static member inline onAbort (handler: Event -> unit) = Interop.mkAttr "onAbort" handler
    
    static member inline onAnimationEnd (handler: AnimationEvent -> unit) = Interop.mkAttr "onAnimationEnd" handler

    static member inline onAnimationIteration (handler: AnimationEvent -> unit) = Interop.mkAttr "onAnimationIteration" handler

    static member inline onAnimationStart (handler: AnimationEvent -> unit) = Interop.mkAttr "onAnimationStart" handler

    /// Fires the moment that the element loses focus.
    static member inline onBlur (handler: FocusEvent -> unit) = Interop.mkAttr "onBlur" handler

    /// Fires when a file is ready to start playing (when it has buffered enough to begin).
    static member inline onCanPlay (handler: Event -> unit) = Interop.mkAttr "onCanPlay" handler

    /// Fires when a file can be played all the way to the end without pausing for buffering
    static member inline onCanPlayThrough (handler: Event -> unit) = Interop.mkAttr "onCanPlayThrough" handler

    /// Same as `onChange` that takes an event as input but instead let's you deal with the `checked` value changed from the `input` element 
    /// directly when it is defined as a checkbox with `prop.inputType.checkbox`.
    static member inline onChange (handler: bool -> unit) = Interop.mkAttr "onChange" (fun (ev: Event) -> handler (!!ev.target?``checked``))
    /// Fires the moment when the value of the element is changed
    static member inline onChange (handler: Event -> unit) = Interop.mkAttr "onChange" handler
    /// Same as `onChange` that takes an event as input but instead lets you deal with the selected file directly from the `input` element when it is defined as a checkbox with `prop.type'.file`.
    static member inline onChange (handler: File -> unit) = 
        let fileHandler (ev: Event) : unit = 
            let files : FileList = ev?target?files 
            if not (isNullOrUndefined files) && files.length > 0 then handler (files.item 0)
        Interop.mkAttr "onChange" fileHandler 
    /// Same as `onChange` that takes an event as input but instead lets you deal with the selected files directly from the `input` element when it is defined as a checkbox with `prop.type'.file` and `prop.multiple true`.
    static member inline onChange (handler: File list -> unit) = 
        let fileHandler (ev: Event) : unit = 
            let fileList : FileList = ev?target?files 
            if not (isNullOrUndefined fileList) then handler [ for i in 0 .. fileList.length - 1 -> fileList.item i ]
        Interop.mkAttr "onChange" fileHandler
    /// Same as `onChange` that takes an event as input but instead let's you deal with the text changed from the `input` element directly
    /// instead of extracting it from the event arguments.
    static member inline onChange (handler: string -> unit) = Interop.mkAttr "onChange" (fun (ev: Event) -> handler (!!ev.target?value))

    /// Same as `onChange` but let's you deal with the `checked` value that has changed from the `input` element directly instead of extracting it from the event arguments.
    static member inline onCheckedChange (handler: bool -> unit) = Interop.mkAttr "onChange" (fun (ev: Event) -> handler (!!ev.target?``checked``))

    /// Fires on a mouse click on the element.
    static member inline onClick (handler: MouseEvent -> unit) = Interop.mkAttr "onClick" handler

    static member inline onCompositionEnd (handler: CompositionEvent -> unit) = Interop.mkAttr "onCompositionEnd" handler

    static member inline onCompositionStart (handler: CompositionEvent -> unit) = Interop.mkAttr "onCompositionStart" handler

    static member inline onCompositionUpdate (handler: CompositionEvent -> unit) = Interop.mkAttr "onCompositionUpdate" handler

    /// Fires when a context menu is triggered.
    static member inline onContextMenu (handler: MouseEvent -> unit) = Interop.mkAttr "onContextMenu" handler

    /// Fires when the user copies the content of an element.
    static member inline onCopy (handler: ClipboardEvent -> unit) = Interop.mkAttr "onCopy" handler

    /// Fires when the user cuts the content of an element.
    static member inline onCut (handler: ClipboardEvent -> unit) = Interop.mkAttr "onCut" handler

    /// Fires when a mouse is double clicked on the element.
    static member inline onDoubleClick (handler: MouseEvent -> unit) = Interop.mkAttr "onDoubleClick" handler

    /// Fires when an element is dragged.
    static member inline onDrag (handler: DragEvent -> unit) = Interop.mkAttr "onDrag" handler

    /// Fires when the a drag operation has ended.
    static member inline onDragEnd (handler: DragEvent -> unit) = Interop.mkAttr "onDragEnd" handler

    /// Fires when an element has been dragged to a valid drop target.
    static member inline onDragEnter (handler: DragEvent -> unit) = Interop.mkAttr "onDragEnter" handler

    static member inline onDragExit (handler: DragEvent -> unit) = Interop.mkAttr "onDragExit" handler

    /// Fires when an element leaves a valid drop target.
    static member inline onDragLeave (handler: DragEvent -> unit) = Interop.mkAttr "onDragLeave" handler

    /// Fires when an element is being dragged over a valid drop target.
    static member inline onDragOver (handler: DragEvent -> unit) = Interop.mkAttr "onDragOver" handler

    /// Fires when the a drag operation has begun.
    static member inline onDragStart (handler: DragEvent -> unit) = Interop.mkAttr "onDragStart" handler

    /// Fires when dragged element is being dropped.
    static member inline onDrop (handler: DragEvent -> unit) = Interop.mkAttr "onDrop" handler

    /// Fires when the length of the media changes.
    static member inline onDurationChange (handler: Event -> unit) = Interop.mkAttr "onDurationChange" handler

    /// Fires when something bad happens and the file is suddenly unavailable (like unexpectedly disconnects).
    static member inline onEmptied (handler: Event -> unit) = Interop.mkAttr "onEmptied" handler

    static member inline onEncrypted (handler: Event -> unit) = Interop.mkAttr "onEncrypted" handler

    /// Fires when the media has reach the end (a useful event for messages like "thanks for listening").
    static member inline onEnded (handler: Event -> unit) = Interop.mkAttr "onEnded" handler

    /// Fires when an error occurs.
    static member inline onError (handler: Event -> unit) = Interop.mkAttr "onError" handler

    /// Fires the moment when the element gets focus.
    static member inline onFocus (handler: FocusEvent -> unit) = Interop.mkAttr "onFocus" handler

    /// Fires when an element gets user input.
    static member inline onInput (handler: Event -> unit) = Interop.mkAttr "onInput" handler

    /// Fires when a user is pressing a key.
    static member inline onKeyDown (handler: KeyboardEvent -> unit) = Interop.mkAttr "onKeyDown" handler

    /// Fires when a user presses a key.
    static member inline onKeyPress (handler: KeyboardEvent -> unit) = Interop.mkAttr "onKeyPress" handler

    /// Fires when a user releases a key.
    static member inline onKeyUp (handler: KeyboardEvent -> unit) = Interop.mkAttr "onKeyUp" handler

    /// Fires after the page is finished loading.
    static member inline onLoad (handler: Event -> unit) = Interop.mkAttr "onLoad" handler

    /// Fires when media data is loaded.
    static member inline onLoadedData (handler: Event -> unit) = Interop.mkAttr "onLoadedData" handler

    /// Fires when meta data (like dimensions and duration) are loaded.
    static member inline onLoadedMetadata (handler: Event -> unit) = Interop.mkAttr "onLoadedMetadata" handler

    /// Fires when the file begins to load before anything is actually loaded.
    static member inline onLoadStart (handler: Event -> unit) = Interop.mkAttr "onLoadStart" handler

    /// Fires when a mouse button is pressed down on an element.
    static member inline onMouseDown (handler: MouseEvent -> unit) = Interop.mkAttr "onMouseDown" handler

    static member inline onMouseEnter (handler: MouseEvent -> unit) = Interop.mkAttr "onMouseEnter" handler

    static member inline onMouseLeave (handler: MouseEvent -> unit) = Interop.mkAttr "onMouseLeave" handler

    /// Fires when the mouse pointer is moving while it is over an element.
    static member inline onMouseMove (handler: MouseEvent -> unit) = Interop.mkAttr "onMouseMove" handler

    /// Fires when the mouse pointer moves out of an element.
    static member inline onMouseOut (handler: MouseEvent -> unit) = Interop.mkAttr "onMouseOut" handler

    /// Fires when the mouse pointer moves over an element.
    static member inline onMouseOver (handler: MouseEvent -> unit) = Interop.mkAttr "onMouseOver" handler

    /// Fires when the user pastes some content in an element.
    static member inline onPaste (handler: ClipboardEvent -> unit) = Interop.mkAttr "onPaste" handler

    /// Fires when the media is paused either by the user or programmatically.
    static member inline onPause (handler: Event -> unit) = Interop.mkAttr "onPause" handler

    /// Fires when the media is ready to start playing.
    static member inline onPlay (handler: Event -> unit) = Interop.mkAttr "onPlay" handler

    /// Fires when the media actually has started playing
    static member inline onPlaying (handler: Event -> unit) = Interop.mkAttr "onPlaying" handler

    /// Fires when the browser is in the process of getting the media data.
    static member inline onProgress (handler: Event -> unit) = Interop.mkAttr "onProgress" handler
    
    /// Fires when the playback rate changes (like when a user switches to a slow motion or fast forward mode).
    static member inline onRateChange (handler: Event -> unit) = Interop.mkAttr "onRateChange" handler

    /// Fires when the Reset button in a form is clicked.
    static member inline onReset (handler: Event -> unit) = Interop.mkAttr "onReset" handler

    /// Fires when an element's scrollbar is being scrolled.
    static member inline onScroll (handler: UIEvent -> unit) = Interop.mkAttr "onScroll" handler

    /// Fires when the seeking attribute is set to false indicating that seeking has ended.
    static member inline onSeeked (handler: Event -> unit) = Interop.mkAttr "onSeeked" handler

    /// Fires when the seeking attribute is set to true indicating that seeking is active.
    static member inline onSeeking (handler: Event -> unit) = Interop.mkAttr "onSeeking" handler

    /// Fires after some text has been selected in an element.
    static member inline onSelect (handler: Event -> unit) = Interop.mkAttr "onSelect" handler

    /// Fires when the browser is unable to fetch the media data for whatever reason.
    static member inline onStalled (handler: Event -> unit) = Interop.mkAttr "onStalled" handler

    /// Fires when fetching the media data is stopped before it is completely loaded for whatever reason.
    static member inline onSuspend (handler: Event -> unit) = Interop.mkAttr "onSuspend" handler

    /// Fires when a form is submitted.
    static member inline onSubmit (handler: Event -> unit) = Interop.mkAttr "onSubmit" handler

    /// Same as `onChange` but let's you deal with the text changed from the `input` element directly
    /// instead of extracting it from the event arguments.
    static member inline onTextChange (handler: string -> unit) = Interop.mkAttr "onChange" (fun (ev: Event) -> handler (!!ev.target?value))
    
    /// Fires when the playing position has changed (like when the user fast forwards to a different point in the media).
    static member inline onTimeUpdate (handler: Event -> unit) = Interop.mkAttr "onTimeUpdate" handler
    
    static member inline onTouchCancel (handler: TouchEvent -> unit) = Interop.mkAttr "onTouchCancel" handler

    static member inline onTouchEnd (handler: TouchEvent -> unit) = Interop.mkAttr "onTouchEnd" handler

    static member inline onTouchMove (handler: TouchEvent -> unit) = Interop.mkAttr "onTouchMove" handler

    static member inline onTouchStart (handler: TouchEvent -> unit) = Interop.mkAttr "onTouchStart" handler

    static member inline onTransitionEnd (handler: TransitionEvent -> unit) = Interop.mkAttr "onTransitionEnd" handler

    /// Fires when the volume is changed which (includes setting the volume to "mute").
    static member inline onVolumeChange (handler: Event -> unit) = Interop.mkAttr "onVolumeChange" handler
    
    /// Fires when the media has paused but is expected to resume (like when the media pauses to buffer more data).
    static member inline onWaiting (handler: Event -> unit) = Interop.mkAttr "onWaiting" handler
    
    /// Fires when the mouse wheel rolls up or down over an element.
    static member inline onWheel (handler: WheelEvent -> unit) = Interop.mkAttr "onWheel" handler
    
    /// This attribute indicates the optimal numeric value. It must be within the range (as defined by the min 
    /// attribute and max attribute). When used with the low attribute and high attribute, it gives an indication 
    /// where along the range is considered preferable. For example, if it is between the min attribute and the 
    /// low attribute, then the lower range is considered preferred.
    static member inline optimum (value: float) = Interop.mkAttr "optimum" value
    /// This attribute indicates the optimal numeric value. It must be within the range (as defined by the min 
    /// attribute and max attribute). When used with the low attribute and high attribute, it gives an indication 
    /// where along the range is considered preferable. For example, if it is between the min attribute and the 
    /// low attribute, then the lower range is considered preferred.
    static member inline optimum (value: int) = Interop.mkAttr "optimum" value

    /// Provides a hint to the user of what can be entered in the field.
    static member inline placeholder (value: string) = Interop.mkAttr "placeholder" value

    /// Indicating that the video is to be played "inline", that is within the element's playback area. 
    ///
    /// Note that the absence of this attribute does not imply that the video will always be played in fullscreen.
    static member inline playsInline (value: bool) = Interop.mkAttr "playsinline" value

    /// Contains a space-separated list of URLs to which, when the hyperlink is followed, 
    /// POST requests with the body PING will be sent by the browser (in the background).
    /// 
    /// Typically used for tracking.
    static member inline ping (value: string) = Interop.mkAttr "ping" value
    /// Contains a space-separated list of URLs to which, when the hyperlink is followed, 
    /// POST requests with the body PING will be sent by the browser (in the background).
    /// 
    /// Typically used for tracking.
    static member inline ping (urls: #seq<string>) = Interop.mkAttr "ping" (urls |> String.concat " ")

    /// SVG attribute to define a list of points.
    static member inline points (value: string) = Interop.mkAttr "points" value

    /// A URL for an image to be shown while the video is downloading. If this attribute isn't specified, nothing 
    /// is displayed until the first frame is available, then the first frame is shown as the poster frame.
    static member inline poster (value: string) = Interop.mkAttr "poster" value

    /// SVG attribute to define the radius of a circle
    static member inline r (value: float) = Interop.mkAttr "r" value
    /// SVG attribute to define the radius of a circle
    static member inline r (value: ICssUnit) = Interop.mkAttr "r" value
    /// SVG attribute to define the radius of a circle
    static member inline r (value: int) = Interop.mkAttr "r" value

    /// Indicates whether the element can be edited.
    static member inline readOnly (value: bool) = Interop.mkAttr "readOnly" value

    /// Used to reference a DOM element or class component from within a parent component.
    static member inline ref (handler: Element -> unit) = Interop.mkAttr "ref" handler
    /// Used to reference a DOM element or class component from within a parent component.
    static member inline ref (ref: Fable.React.IRefValue<HTMLElement option>) = Interop.mkAttr "ref" ref
    
    /// For anchors containing the href attribute, this attribute specifies the relationship 
    /// of the target object to the link object. The value is a space-separated list of link 
    /// types values. The values and their semantics will be registered by some authority that 
    /// might have meaning to the document author. The default relationship, if no other is 
    /// given, is void. 
    ///
    /// Use this attribute only if the href attribute is present.
    static member inline rel (value: bool) = Interop.mkAttr "rel" value

    /// Indicates whether this element is required to fill out or not.
    static member inline required (value: bool) = Interop.mkAttr "required" value

    /// Sets the aria role
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/roles
    static member inline role ([<System.ParamArray>] roles: string []) = Interop.mkAttr "role" (String.concat " " roles)

    /// Defines the number of rows in a text area.
    static member inline rows (value: int) = Interop.mkAttr "rows" value

    /// Defines the number of rows a table cell should span over.
    static member inline rowSpan (value: int) = Interop.mkAttr "rowSpan" value

    /// The SVG rx attribute defines a radius on the x-axis.
    ///
    /// Two elements are using this attribute: <ellipse>, and <rect>
    static member inline rx (value: float) = Interop.mkAttr "rx" value
    /// The SVG rx attribute defines a radius on the x-axis.
    ///
    /// Two elements are using this attribute: <ellipse>, and <rect>
    static member inline rx (value: ICssUnit) = Interop.mkAttr "rx" value
    /// The SVG rx attribute defines a radius on the x-axis.
    ///
    /// Two elements are using this attribute: <ellipse>, and <rect>
    static member inline rx (value: int) = Interop.mkAttr "rx" value
    
    /// The SVG ry attribute defines a radius on the y-axis.
    ///
    /// Two elements are using this attribute: <ellipse>, and <rect>
    static member inline ry (value: float) = Interop.mkAttr "ry" value
    /// The SVG ry attribute defines a radius on the y-axis.
    ///
    /// Two elements are using this attribute: <ellipse>, and <rect>
    static member inline ry (value: ICssUnit) = Interop.mkAttr "ry" value
    /// The SVG ry attribute defines a radius on the y-axis.
    ///
    /// Two elements are using this attribute: <ellipse>, and <rect>
    static member inline ry (value: int) = Interop.mkAttr "ry" value

    /// Defines a value which will be selected on page load.
    static member inline selected (value: bool) = Interop.mkAttr "selected" value

    /// Defines the sizes of the icons for visual media contained in the resource. 
    /// It must be present only if the rel contains a value of icon or a non-standard 
    /// type such as Apple's apple-touch-icon.
    ///
    /// It may have the following values:
    ///
    /// `any`, meaning that the icon can be scaled to any size as it is in a vector 
    /// format, like image/svg+xml.
    ///
    /// A white-space separated list of sizes, each in the format `<width in pixels>x<height in pixels>`
    /// or `<width in pixels>X<height in pixels>`. Each of these sizes must be contained in the resource.
    static member inline sizes (value: string) = Interop.mkAttr "sizes" value

    /// Defines whether the element may be checked for spelling errors.
    static member inline spellcheck (value: bool) = Interop.mkAttr "spellcheck" value

    /// This attribute contains a positive integer indicating the number of consecutive 
    /// columns the <col> element spans. If not present, its default value is 1.
    static member inline spam (value: int) = Interop.mkAttr "span" value

    /// The URL of the embeddable content.
    static member inline src (value: string) = Interop.mkAttr "src" value

    /// Language of the track text data. It must be a valid BCP 47 language tag. 
    ///
    /// If the kind attribute is set to subtitles, then srclang must be defined.
    static member inline srcLang (value: string) = Interop.mkAttr "srclang" value

    /// One or more responsive image candidates.
    static member inline srcset (value: string) = Interop.mkAttr "srcset" value

    /// Defines the first number if other than 1.
    static member inline start (value: string) = Interop.mkAttr "start" value

    /// Indicates the stepping interval.
    static member inline step (value: float) = Interop.mkAttr "step" value
    /// Indicates the stepping interval.
    static member inline step (value: int) = Interop.mkAttr "step" value

    /// SVG attribute to indicate what color to use at a gradient stop.
    static member inline stopColor (value: string) = Interop.mkAttr "stopColor" value

    /// SVG attribute to define the opacity of a given color gradient stop.
    static member inline stopOpacity (value: float) = Interop.mkAttr "stopOpacity" value
    /// SVG attribute to define the opacity of a given color gradient stop.
    static member inline stopOpacity (value: int) = Interop.mkAttr "stopOpacity" value

    /// SVG attribute to define the color (or any SVG paint servers like gradients or patterns) used to paint the outline of the shape.
    static member inline stroke (color: string) = Interop.mkAttr "stroke" color

    /// SVG attribute to define the width of the stroke to be applied to the shape.
    static member inline strokeWidth (value: float) = Interop.mkAttr "strokeWidth" value
    /// SVG attribute to define the width of the stroke to be applied to the shape.
    static member inline strokeWidth (value: ICssUnit) = Interop.mkAttr "strokeWidth" value
    /// SVG attribute to define the width of the stroke to be applied to the shape.
    static member inline strokeWidth (value: int) = Interop.mkAttr "strokeWidth" value
    
    static member inline style (properties: #IStyleAttribute list) = Interop.mkAttr "style" (createObj !!properties)
    static member style (properties: (bool * IStyleAttribute list) list) =
        properties
        |> List.filter fst
        |> List.collect snd
        |> unbox
        |> createObj
        |> Interop.mkAttr "style"

    /// The `tabindex` global attribute indicates that its element can be focused, 
    /// and where it participates in sequential keyboard navigation (usually with the Tab key, hence the name).
    static member inline tabIndex (index: int) = Interop.mkAttr "tabindex" index

    /// Controls browser behavior when opening a link.
    static member inline target (frameName: string) = Interop.mkAttr "target" frameName

    /// Defines the text content of the element. Alias for `children [ Html.text value ]`
    static member inline text (value: float) = Interop.mkAttr "children" value
    /// Defines the text content of the element. Alias for `children [ Html.text value ]`
    static member inline text (value: int) = Interop.mkAttr "children" value
    /// Defines the text content of the element. Alias for `children [ Html.text value ]`
    static member inline text (value: string) = Interop.mkAttr "children" value

    /// The title global attribute contains text representing advisory information related to the element it belongs to.
    static member inline title (value: string) = Interop.mkAttr "title" value

    /// Sets the `type` attribute for the element.
    static member inline type' (value: string) = Interop.mkAttr "type" value
    
    /// A hash-name reference to a <map> element; that is a '#' followed by the value of a name of a map element.
    static member inline usemap (value: string) = Interop.mkAttr "usemap" value

    /// Sets the value of a React controlled component.
    static member inline value (value: bool) = Interop.mkAttr "value" value
    /// Sets the value of a React controlled component.
    static member inline value (value: float) = Interop.mkAttr "value" value
    /// Sets the value of a React controlled component.
    static member inline value (value: System.Guid) = Interop.mkAttr "value" (string value)
    /// Sets the value of a React controlled component.
    static member inline value (value: int) = Interop.mkAttr "value" value
    /// Sets the value of a React controlled component.
    static member inline value (value: string) = Interop.mkAttr "value" value

    /// `prop.ref` callback that sets the value of an input after DOM element is created.
    /// Can be used instead of `prop.defaultValue` and `prop.value` props to override input value.
    static member inline valueOrDefault (value: bool) =
        prop.ref (fun e -> if e |> isNull |> not && !!e?value <> !!value then e?value <- !!value)
    /// `prop.ref` callback that sets the value of an input after DOM element is created.
    /// Can be used instead of `prop.defaultValue` and `prop.value` props to override input value.
    static member inline valueOrDefault (value: float) =
        prop.ref (fun e -> if e |> isNull |> not && !!e?value <> !!value then e?value <- !!value)
    /// `prop.ref` callback that sets the value of an input after DOM element is created.
    /// Can be used instead of `prop.defaultValue` and `prop.value` props to override input value.
    static member inline valueOrDefault (value: System.Guid) =
        prop.ref (fun e -> if e |> isNull |> not && !!e?value <> !!value then e?value <- !!value)
    /// `prop.ref` callback that sets the value of an input after DOM element is created.
    /// Can be used instead of `prop.defaultValue` and `prop.value` props to override input value.
    static member inline valueOrDefault (value: int) =
        prop.ref (fun e -> if e |> isNull |> not && !!e?value <> !!value then e?value <- !!value)
    /// `prop.ref` callback that sets the value of an input after DOM element is created.
    /// Can be used instead of `prop.defaultValue` and `prop.value` props to override input box value.
    static member inline valueOrDefault (value: string) =
        prop.ref (fun e -> if e |> isNull |> not && !!e?value <> !!value then e?value <- !!value)
    
    /// Set visible area of the SVG image.
    static member inline viewPort (x: int, y: int, height: int, width: int) =
        Interop.mkAttr "viewport"
          ((unbox<string> x) + " " +
           (unbox<string> y) + " " +
           (unbox<string> height) + " " +
           (unbox<string> width))
    
    /// Specifies the width of elements listed here. For all other elements, use the CSS height property.
    ///
    /// <canvas>, <embed>, <iframe>, <img>, <input>, <object>, <video>
    static member inline width (value: int) = Interop.mkAttr "width" value

    /// SVG attribute to define a x-axis coordinate in the user coordinate system.
    static member inline x (value: float) = Interop.mkAttr "x" value
    /// SVG attribute to define a x-axis coordinate in the user coordinate system.
    static member inline x (value: ICssUnit) = Interop.mkAttr "x" value
    /// SVG attribute to define a x-axis coordinate in the user coordinate system.
    static member inline x (value: int) = Interop.mkAttr "x" value

    /// The x1 attribute is used to specify the first x-coordinate for drawing an SVG element that 
    /// requires more than one coordinate. Elements that only need one coordinate use the x attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline x1 (value: float) = Interop.mkAttr "x1" value
    /// The x1 attribute is used to specify the first x-coordinate for drawing an SVG element that 
    /// requires more than one coordinate. Elements that only need one coordinate use the x attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline x1 (value: ICssUnit) = Interop.mkAttr "x1" value
    /// The x1 attribute is used to specify the first x-coordinate for drawing an SVG element that 
    /// requires more than one coordinate. Elements that only need one coordinate use the x attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline x1 (value: int) = Interop.mkAttr "x1" value

    /// The x2 attribute is used to specify the second x-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the x attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline x2 (value: float) = Interop.mkAttr "x2" value
    /// The x2 attribute is used to specify the second x-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the x attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline x2 (value: ICssUnit) = Interop.mkAttr "x2" value
    /// The x2 attribute is used to specify the second x-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the x attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline x2 (value: int) = Interop.mkAttr "x2" value
    
    /// Specifies the XML Namespace of the document. 
    ///
    /// Default value is "http://www.w3.org/1999/xhtml". 
    /// 
    /// This is required in documents parsed with XML parsers, and optional in text/html documents.
    static member inline xmlns (value: string) = Interop.mkAttr "xmlns" value

    /// SVG attribute to define a y-axis coordinate in the user coordinate system.
    static member inline y (value: float) = Interop.mkAttr "y" value
    /// SVG attribute to define a y-axis coordinate in the user coordinate system.
    static member inline y (value: ICssUnit) = Interop.mkAttr "y" value
    /// SVG attribute to define a y-axis coordinate in the user coordinate system.
    static member inline y (value: int) = Interop.mkAttr "y" value

    /// The y1 attribute is used to specify the first y-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the y attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline y1 (value: float) = Interop.mkAttr "y1" value
    /// The y1 attribute is used to specify the first y-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the y attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline y1 (value: ICssUnit) = Interop.mkAttr "y1" value
    /// The y1 attribute is used to specify the first y-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the y attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline y1 (value: int) = Interop.mkAttr "y1" value

    /// The y2 attribute is used to specify the second y-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the y attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline y2 (value: float) = Interop.mkAttr "y2" value
    /// The y2 attribute is used to specify the second y-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the y attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline y2 (value: ICssUnit) = Interop.mkAttr "y2" value
    /// The y2 attribute is used to specify the second y-coordinate for drawing an SVG element that requires 
    /// more than one coordinate. Elements that only need one coordinate use the y attribute instead.
    ///
    /// Two elements are using this attribute: <line>, and <linearGradient>
    static member inline y2 (value: int) = Interop.mkAttr "y2" value

module prop =
    /// Indicates whether user input completion suggestions are provided.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-autocomplete
    [<Erase>]
    type ariaAutocomplete =
        /// A list of choices appears and the currently selected suggestion also
        /// appears inline.
        static member inline both = Interop.mkAttr "aria-autocomplete" "both"
        /// The system provides text after the caret as a suggestion for how to
        /// complete the field.
        static member inline inlinedAfterCaret = Interop.mkAttr "aria-autocomplete" "inline"
        /// A list of choices appears from which the user can choose.
        static member inline list = Interop.mkAttr "aria-autocomplete" "list"
        /// No input completion suggestions are provided.
        static member inline none = Interop.mkAttr "aria-autocomplete" "none"

    /// Indicates the current "checked" state of checkboxes, radio buttons, and
    /// other widgets. See related `aria-pressed` and `aria-selected`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-checked
    [<Erase>]
    type ariaChecked =
        /// Indicates a mixed mode value for a tri-state checkbox or
        /// `menuitemcheckbox`.
        static member inline mixed = Interop.mkAttr "aria-checked" "mixed"

    /// Indicates what functions can be performed when the dragged object is
    /// released on the drop target. This allows assistive technologies to
    /// convey the possible drag options available to users, including whether a
    /// pop-up menu of choices is provided by the application. Typically, drop
    /// effect functions can only be provided once an object has been grabbed
    /// for a drag operation as the drop effect functions available are
    /// dependent on the object being dragged.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-dropeffect
    [<Erase>]
    type ariaDropEffect =
        /// A duplicate of the source object will be dropped into the target.
        static member inline copy = Interop.mkAttr "aria-dropeffect" "copy"
        /// A function supported by the drop target is executed, using the drag
        /// source as an input.
        static member inline execute = Interop.mkAttr "aria-dropeffect" "execute"
        /// A reference or shortcut to the dragged object will be created in the
        /// target object.
        static member inline link = Interop.mkAttr "aria-dropeffect" "link"
        /// The source object will be removed from its current location and
        /// dropped into the target.
        static member inline move = Interop.mkAttr "aria-dropeffect" "move"
        /// No operation can be performed; effectively cancels the drag
        /// operation if an attempt is made to drop on this object. Ignored if
        /// combined with any other token value. e.g. 'none copy' is equivalent
        /// to a 'copy' value.
        static member inline none = Interop.mkAttr "aria-dropeffect" "none"
        /// There is a popup menu or dialog that allows the user to choose one
        /// of the drag operations (copy, move, link, execute) and any other
        /// drag functionality, such as cancel.
        static member inline popup = Interop.mkAttr "aria-dropeffect" "popup"

    /// Indicates the entered value does not conform to the format expected by
    /// the application.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-invalid
    [<Erase>]
    type ariaInvalid =
        /// A grammatical error was detected.
        static member inline grammar = Interop.mkAttr "aria-invalid" "grammar"
        /// A spelling error was detected.
        static member inline spelling = Interop.mkAttr "aria-invalid" "spelling"

    /// Indicates that an element will be updated, and describes the types of
    /// updates the user agents, assistive technologies, and user can expect
    /// from the live region.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-live
    [<Erase>]
    type ariaLive =
        /// Indicates that updates to the region have the highest priority and
        /// should be presented the user immediately.
        static member inline assertive = Interop.mkAttr "aria-live" "assertive"
        /// Indicates that updates to the region should not be presented to the
        /// user unless the used is currently focused on that region.
        static member inline off = Interop.mkAttr "aria-live" "off"
        /// Indicates that updates to the region should be presented at the next
        /// graceful opportunity, such as at the end of speaking the current
        /// sentence or when the user pauses typing.
        static member inline polite = Interop.mkAttr "aria-live" "polite"

    /// Indicates whether the element and orientation is horizontal or vertical.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-orientation
    [<Erase>]
    type ariaOrientation =
        /// The element is oriented horizontally.
        static member inline horizontal = Interop.mkAttr "aria-orientation" "horizontal"
        /// The element is oriented vertically.
        static member inline vertical = Interop.mkAttr "aria-orientation" "vertical"

    /// Indicates the current "pressed" state of toggle buttons. See related
    /// `aria-checked` and `aria-selected`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-pressed
    [<Erase>]
    type ariaPressed =
        /// Indicates a mixed mode value for a tri-state toggle button.
        static member inline mixed = Interop.mkAttr "aria-pressed" "mixed"

    /// Indicates what user agent change notifications (additions, removals,
    /// etc.) assistive technologies will receive within a live region. See
    /// related `aria-atomic`.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-relevant
    [<Erase>]
    type ariaRelevant =
        /// Element nodes are added to the DOM within the live region.
        static member inline additions = Interop.mkAttr "aria-relevant" "additions"
        /// Equivalent to the combination of all values, "additions removals
        /// text".
        static member inline all = Interop.mkAttr "aria-relevant" "all"
        /// Text or element nodes within the live region are removed from the
        /// DOM.
        static member inline removals = Interop.mkAttr "aria-relevant" "removals"
        /// Text is added to any DOM descendant nodes of the live region.
        static member inline text = Interop.mkAttr "aria-relevant" "text"    

    /// Indicates if items in a table or grid are sorted in ascending or
    /// descending order.
    ///
    /// https://www.w3.org/WAI/PF/aria-1.1/states_and_properties#aria-sort
    [<Erase>]
    type ariaSort =
        /// Items are sorted in ascending order by this column.
        static member inline ascending = Interop.mkAttr "aria-sort" "ascending"
        /// Items are sorted in descending order by this column.
        static member inline descending = Interop.mkAttr "aria-sort" "descending"
        /// There is no defined sort applied to the column.
        static member inline none = Interop.mkAttr "aria-sort" "none"
        /// A sort algorithm other than ascending or descending has been
        /// applied.
        static member inline other = Interop.mkAttr "aria-sort" "other"

    /// This attribute is only used when rel="preload" or rel="prefetch" has been set on the <link> element. 
    /// It specifies the type of content being loaded by the <link>, which is necessary for request matching, 
    /// application of correct content security policy, and setting of correct Accept request header. 
    /// Furthermore, rel="preload" uses this as a signal for request prioritization.
    [<Erase>]
    type as' =
        /// Applies to <audio> elements.
        static member inline audio = Interop.mkAttr "as" "audio"
        /// Applies to <iframe> and <frame> elements.
        static member inline document = Interop.mkAttr "as" "document"
        /// Applies to <embed> elements.
        static member inline embed = Interop.mkAttr "as" "embed"
        /// Applies to fetch and XHR.
        ///
        /// This value also requires <link> to contain the crossorigin attribute.
        static member inline fetch = Interop.mkAttr "as" "fetch"
        /// Applies to CSS @font-face.
        static member inline font = Interop.mkAttr "as" "font"
        /// Applies to <img> and <picture> elements with srcset or imageset attributes, 
        /// SVG <image> elements, and CSS *-image rules.
        static member inline image = Interop.mkAttr "as" "image"
        /// Applies to <object> elements.
        static member inline object = Interop.mkAttr "as" "object"
        /// Applies to <script> elements, Worker importScripts.
        static member inline script = Interop.mkAttr "as" "script"
        /// Applies to <link rel=stylesheet> elements, and CSS @import.
        static member inline style = Interop.mkAttr "as" "style"
        /// Applies to <track> elements.
        static member inline track = Interop.mkAttr "as" "track"
        /// Applies to <video> elements.
        static member inline video = Interop.mkAttr "as" "video"
        /// Applies to Worker and SharedWorker.
        static member inline worker = Interop.mkAttr "as" "worker"

    /// A set of values specifying the coordinates of the hot-spot region. 
    ///
    /// The number and meaning of the values depend upon the value specified for the shape attribute
    [<Erase>]
    type coords =
        static member inline rect (left: int, top: int, right: int, bottom: int) = 
            Interop.mkAttr "coords" 
                ((unbox<string> left) + "," +
                 (unbox<string> top) + "," +
                 (unbox<string> right) + "," +
                 (unbox<string> bottom))
        static member inline circle (x: int, y: int, r: int) =
            Interop.mkAttr "coords" 
                ((unbox<string> x) + "," +
                 (unbox<string> y) + "," +
                 (unbox<string> r))
        static member inline poly (x1: int, y1: int, x2: int, y2: int, x3: int, y3: int) =
            Interop.mkAttr "coords" 
                ((unbox<string> x1) + "," +
                 (unbox<string> y1) + "," +
                 (unbox<string> x2) + "," +
                 (unbox<string> y2) + "," +
                 (unbox<string> x3) + "," +
                 (unbox<string> y3))

    /// Indicates whether CORS must be used when fetching the resource.
    [<Erase>]
    type crossOrigin =
        /// A cross-origin request (i.e. with an Origin HTTP header) is performed, but no credential 
        /// is sent (i.e. no cookie, X.509 certificate, or HTTP Basic authentication). If the server 
        /// does not give credentials to the origin site (by not setting the Access-Control-Allow-Origin 
        /// HTTP header) the resource will be tainted and its usage restricted.
        static member inline anonymous = Interop.mkAttr "crossorigin" "anonymous"
        /// A cross-origin request (i.e. with an Origin HTTP header) is performed along with a credential 
        /// sent (i.e. a cookie, certificate, and/or HTTP Basic authentication is performed). If the server 
        /// does not give credentials to the origin site (through Access-Control-Allow-Credentials HTTP 
        /// header), the resource will be tainted and its usage restricted.
        static member inline useCredentials = Interop.mkAttr "crossorigin" "use-credentials"

    /// Indicates the directionality of the element's text.
    [<Erase>]
    type dir =
        /// Lets the user agent decide.
        static member inline auto = Interop.mkAttr "dir" "auto"
        /// Left to right - for languages that are written from left to right.
        static member inline ltr = Interop.mkAttr "dir" "ltr"
        /// Right to left - for languages that are written from right to left.
        static member inline rtl = Interop.mkAttr "dir" "rtl"
    
    /// The `dominantBaseline` attribute specifies the dominant baseline, which is the baseline used to align the box’s text
    /// and inline-level contents. It also indicates the default alignment baseline of any boxes participating in baseline 
    /// alignment in the box’s alignment context. It is used to determine or re-determine a scaled-baseline-table. A 
    /// scaled-baseline-table is a compound value with three components: a baseline-identifier for the dominant-baseline, a 
    /// baseline-table and a baseline-table font-size. Some values of the property re-determine all three values; other only 
    /// re-establish the baseline-table font-size. When the initial value, auto, would give an undesired result, this property 
    /// can be used to explicitly set the desired scaled-baseline-table.
    /// If there is no baseline table in the nominal font or if the baseline table lacks an entry for the desired baseline, 
    /// then the browser may use heuristics to determine the position of the desired baseline.
    [<Erase>]
    type dominantBaseline =
        /// The baseline-identifier for the dominant-baseline is set to be alphabetic, the derived baseline-table is constructed 
        /// using the alphabetic baseline-table in the font, and the baseline-table font-size is changed to the value of the 
        /// font-size attribute on this element.
        static member inline alphabetic = Interop.mkAttr "dominantBaseline" "alphabetic"
        /// If this property occurs on a <text> element, then the computed value depends on the value of the writing-mode attribute.
        ///
        /// If the writing-mode is horizontal, then the value of the dominant-baseline component is alphabetic, else if the writing-mode 
        /// is vertical, then the value of the dominant-baseline component is central. 
        ///
        /// If this property occurs on a <tspan>, <tref>, 
        /// <altGlyph> or <textPath> element, then the dominant-baseline and the baseline-table components remain the same as those of 
        /// the parent text content element. 
        ///
        /// If the computed baseline-shift value actually shifts the baseline, then the baseline-table 
        /// font-size component is set to the value of the font-size attribute on the element on which the dominant-baseline attribute 
        /// occurs, otherwise the baseline-table font-size remains the same as that of the element. 
        ///
        /// If there is no parent text content 
        /// element, the scaled-baseline-table value is constructed as above for <text> elements.
        static member inline auto = Interop.mkAttr "dominantBaseline" "auto"
        /// The baseline-identifier for the dominant-baseline is set to be central. The derived baseline-table is constructed from the 
        /// defined baselines in a baseline-table in the font. That font baseline-table is chosen using the following priority order of 
        /// baseline-table names: ideographic, alphabetic, hanging, mathematical. The baseline-table font-size is changed to the value 
        /// of the font-size attribute on this element.
        static member inline central = Interop.mkAttr "dominantBaseline" "central"
        /// The baseline-identifier for the dominant-baseline is set to be hanging, the derived baseline-table is constructed using the 
        /// hanging baseline-table in the font, and the baseline-table font-size is changed to the value of the font-size attribute on 
        /// this element.
        static member inline hanging = Interop.mkAttr "dominantBaseline" "hanging"
        /// The baseline-identifier for the dominant-baseline is set to be ideographic, the derived baseline-table is constructed using 
        /// the ideographic baseline-table in the font, and the baseline-table font-size is changed to the value of the font-size 
        /// attribute on this element.
        static member inline ideographic = Interop.mkAttr "dominantBaseline" "ideographic"
        /// The baseline-identifier for the dominant-baseline is set to be mathematical, the derived baseline-table is constructed using 
        /// the mathematical baseline-table in the font, and the baseline-table font-size is changed to the value of the font-size 
        /// attribute on this element.
        static member inline mathematical = Interop.mkAttr "dominantBaseline" "mathematical"
        /// The baseline-identifier for the dominant-baseline is set to be middle. The derived baseline-table is constructed from the 
        /// defined baselines in a baseline-table in the font. That font baseline-table is chosen using the following priority order 
        /// of baseline-table names: alphabetic, ideographic, hanging, mathematical. The baseline-table font-size is changed to the value 
        /// of the font-size attribute on this element.
        static member inline middle = Interop.mkAttr "dominantBaseline" "middle"
        /// The baseline-identifier for the dominant-baseline is set to be text-after-edge. The derived baseline-table is constructed 
        /// from the defined baselines in a baseline-table in the font. The choice of which font baseline-table to use from the 
        /// baseline-tables in the font is browser dependent. The baseline-table font-size is changed to the value of the font-size 
        /// attribute on this element.
        static member inline textAfterEdge = Interop.mkAttr "dominantBaseline" "text-after-edge"
        /// The baseline-identifier for the dominant-baseline is set to be text-before-edge. The derived baseline-table is constructed 
        /// from the defined baselines in a baseline-table in the font. The choice of which baseline-table to use from the baseline-tables 
        /// in the font is browser dependent. The baseline-table font-size is changed to the value of the font-size attribute on this element.
        static member inline textBeforeEdge = Interop.mkAttr "dominantBaseline" "text-before-edge"
        /// This value uses the top of the em box as the baseline.
        static member inline textTop = Interop.mkAttr "dominantBaseline" "text-top"

    /// How the text track is meant to be used.
    [<Erase>]
    type kind =
        /// Subtitles provide translation of content that cannot be understood by the viewer. For example dialogue 
        /// or text that is not English in an English language film.
        ///
        /// Subtitles may contain additional content, usually extra background information. For example the text 
        /// at the beginning of the Star Wars films, or the date, time, and location of a scene.
        static member inline subtitles = Interop.mkAttr "kind" "subtitles"
        /// Closed captions provide a transcription and possibly a translation of audio.
        ///
        /// It may include important non-verbal information such as music cues or sound effects. 
        /// It may indicate the cue's source (e.g. music, text, character).
        ///
        /// Suitable for users who are deaf or when the sound is muted.
        static member inline captions = Interop.mkAttr "kind" "captions"
        /// Textual description of the video content.
        ///
        /// Suitable for users who are blind or where the video cannot be seen.
        static member inline descriptions = Interop.mkAttr "kind" "descriptions"
        /// Chapter titles are intended to be used when the user is navigating the media resource.
        static member inline chapters = Interop.mkAttr "kind" "chapters"
        /// Tracks used by scripts. Not visible to the user.
        static member inline metadata = Interop.mkAttr "kind" "metadata"

    /// Provide a hint to the browser about what the author thinks will lead to the best user experience with regards 
    /// to what content is loaded before the video is played.
    [<Erase>]
    type preload =
        /// Indicates that the whole video file can be downloaded, even if the user is not expected to use it.
        static member inline auto = Interop.mkAttr "preload" "auto"
        /// Indicates that only video metadata (e.g. length) is fetched.
        static member inline metadata = Interop.mkAttr "preload" "metadata"
        /// Indicates that the video should not be preloaded.
        static member inline none = Interop.mkAttr "preload" "none"

    /// Indicates which referrer to send when fetching the script, or resources fetched by the script
    [<Erase>]
    type referrerPolicy =
        /// The Referer header will not be sent.
        static member inline noReferrer = Interop.mkAttr "referrerpolicy" "no-referrer"
        /// The Referer header will not be sent to origins without TLS (HTTPS).
        static member inline noReferrerWhenDowngrade = Interop.mkAttr "referrerpolicy" "no-referrer-when-downgrade"
        /// The sent referrer will be limited to the origin of the referring page: its scheme, host, and port.
        static member inline origin = Interop.mkAttr "referrerpolicy" "origin"
        /// The referrer sent to other origins will be limited to the scheme, the host, and the port. 
        /// Navigations on the same origin will still include the path.
        static member inline originWhenCrossOrigin = Interop.mkAttr "referrerpolicy" "origin-when-cross-origin"
        /// A referrer will be sent for same origin, but cross-origin requests will contain no referrer information.
        static member inline sameOrigin = Interop.mkAttr "referrerpolicy" "same-origin"
        /// Only send the origin of the document as the referrer when the protocol security level stays the same 
        /// (e.g. HTTPS→HTTPS), but don't send it to a less secure destination (e.g. HTTPS→HTTP).
        static member inline strictOrigin = Interop.mkAttr "referrerpolicy" "strict-origin"
        /// Send a full URL when performing a same-origin request, but only send the origin when the protocol security 
        /// level stays the same (e.g.HTTPS→HTTPS), and send no header to a less secure destination (e.g. HTTPS→HTTP).
        static member inline strictOriginWhenCrossOrigin = Interop.mkAttr "referrerpolicy" "strict-origin-when-cross-origin"
        /// The referrer will include the origin and the path (but not the fragment, password, or username). This value is unsafe, 
        /// because it leaks origins and paths from TLS-protected resources to insecure origins.
        static member inline unsafeUrl = Interop.mkAttr "referrerpolicy" "unsafe-url"

    /// https://www.w3.org/WAI/PF/aria-1.1/roles
    [<Erase>]
    type role =
        /// A message with important, and usually time-sensitive, information.
        /// See related `alertdialog` and `status`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#alert
        static member inline alert = Interop.mkAttr "role" "alert"
        /// A type of dialog that contains an alert message, where initial focus
        /// goes to an element within the dialog. See related `alert` and
        /// `dialog`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#alertdialog
        static member inline alertDialog = Interop.mkAttr "role" "alertdialog"
        /// A region declared as a web application, as opposed to a web
        /// `document`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#application
        static member inline application = Interop.mkAttr "role" "application"
        /// A section of a page that consists of a composition that forms an
        /// independent part of a document, page, or site.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#article
        static member inline article = Interop.mkAttr "role" "article"
        /// A region that contains mostly site-oriented content, rather than
        /// page-specific content.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#banner
        static member inline banner = Interop.mkAttr "role" "banner"
        /// An input that allows for user-triggered actions when clicked or
        /// pressed. See related `link`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#button
        static member inline button = Interop.mkAttr "role" "button"
        /// A checkable input that has three possible values: `true`, `false`,
        /// or `mixed`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#checkbox
        static member inline checkbox = Interop.mkAttr "role" "checkbox"
        /// A cell containing header information for a column.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#columnheader
        static member inline columnHeader = Interop.mkAttr "role" "columnheader"
        /// A presentation of a `select`; usually similar to a `textbox` where
        /// users can type ahead to select an option, or type to enter arbitrary
        /// text as a new item in the list. See related `listbox`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#combobox
        static member inline comboBox = Interop.mkAttr "role" "combobox"
        /// A supporting section of the document, designed to be complementary
        /// to the main content at a similar level in the DOM hierarchy, but
        /// remains meaningful when separated from the main content.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#complementary
        static member inline complementary = Interop.mkAttr "role" "complementary"
        /// A large perceivable region that contains information about the
        /// parent document.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#contentinfo
        static member inline contentInfo = Interop.mkAttr "role" "contentinfo"
        /// A definition of a term or concept.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#definition
        static member inline definition = Interop.mkAttr "role" "definition"
        /// A dialog is an application window that is designed to interrupt the
        /// current processing of an application in order to prompt the user to
        /// enter information or require a response. See related `alertdialog`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#dialog
        static member inline dialog = Interop.mkAttr "role" "dialog"
        /// A list of references to members of a group, such as a static table
        /// of contents.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#directory
        static member inline directory = Interop.mkAttr "role" "directory"
        /// A region containing related information that is declared as document
        /// content, as opposed to a web application.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#document
        static member inline document = Interop.mkAttr "role" "document"
        /// A `landmark` region that contains a collection of items and objects
        /// that, as a whole, combine to create a form. See related search.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#form
        static member inline form = Interop.mkAttr "role" "form"
        /// A grid is an interactive control which contains cells of tabular
        /// data arranged in rows and columns, like a table.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#grid
        static member inline grid = Interop.mkAttr "role" "grid"
        /// A cell in a grid or treegrid.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#gridcell
        static member inline gridCell = Interop.mkAttr "role" "gridcell"
        /// A set of user interface objects which are not intended to be
        /// included in a page summary or table of contents by assistive
        /// technologies.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#group
        static member inline group = Interop.mkAttr "role" "group"
        /// A heading for a section of the page.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#heading
        static member inline heading = Interop.mkAttr "role" "heading"
        /// A container for a collection of elements that form an image.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#img
        static member inline img = Interop.mkAttr "role" "img"
        /// An interactive reference to an internal or external resource that,
        /// when activated, causes the user agent to navigate to that resource.
        /// See related `button`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#link
        static member inline link = Interop.mkAttr "role" "link"
        /// A group of non-interactive list items. See related `listbox`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#list
        static member inline list = Interop.mkAttr "role" "list"
        /// A widget that allows the user to select one or more items from a
        /// list of choices. See related `combobox` and `list`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#listbox
        static member inline listBox = Interop.mkAttr "role" "listbox"
        /// A single item in a list or directory.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#listitem
        static member inline listItem = Interop.mkAttr "role" "listitem"
        /// A type of live region where new information is added in meaningful
        /// order and old information may disappear. See related `marquee`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#log
        static member inline log = Interop.mkAttr "role" "log"
        /// The main content of a document.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#main
        static member inline main = Interop.mkAttr "role" "main"
        /// A type of live region where non-essential information changes
        /// frequently. See related `log`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#marquee
        static member inline marquee = Interop.mkAttr "role" "marquee"
        /// Content that represents a mathematical expression.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#math
        static member inline math = Interop.mkAttr "role" "math"
        /// A type of widget that offers a list of choices to the user.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#menu
        static member inline menu = Interop.mkAttr "role" "menu"
        /// A presentation of `menu` that usually remains visible and is usually
        /// presented horizontally.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#menubar
        static member inline menuBar = Interop.mkAttr "role" "menubar"
        /// An option in a set of choices contained by a `menu` or `menubar`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#menuitem
        static member inline menuItem = Interop.mkAttr "role" "menuitem"
        /// A `menuitem` with a checkable state whose possible values are
        /// `true`, `false`, or `mixed`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#menuitemcheckbox
        static member inline menuItemCheckbox = Interop.mkAttr "role" "menuitemcheckbox"
        /// A checkable menuitem in a set of elements with role `menuitemradio`,
        /// only one of which can be checked at a time.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#menuitemradio
        static member inline menuItemRadio = Interop.mkAttr "role" "menuitemradio"
        /// A collection of navigational elements (usually links) for navigating
        /// the document or related documents.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#navigation
        static member inline navigation = Interop.mkAttr "role" "navigation"
        /// A section whose content is parenthetic or ancillary to the main
        /// content of the resource.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#note
        static member inline note = Interop.mkAttr "role" "note"
        /// A selectable item in a `select` list.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#option
        static member inline option = Interop.mkAttr "role" "option"
        /// An element whose implicit native role semantics will not be mapped
        /// to the accessibility API.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#presentation
        static member inline presentation = Interop.mkAttr "role" "presentation"
        /// An element that displays the progress status for tasks that take a
        /// long time.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#progressbar
        static member inline progressBar = Interop.mkAttr "role" "progressbar"
        /// A checkable input in a group of elements with role radio, only one
        /// of which can be checked at a time.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#radio
        static member inline radio = Interop.mkAttr "role" "radio"
        /// A group of radio buttons.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#radiogroup
        static member inline radioGroup = Interop.mkAttr "role" "radiogroup"
        /// A large perceivable section of a web page or document, that is
        /// important enough to be included in a page summary or table of
        /// contents, for example, an area of the page containing live sporting
        /// event statistics.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#region
        static member inline region = Interop.mkAttr "role" "region"
        /// A row of cells in a grid.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#row
        static member inline row = Interop.mkAttr "role" "row"
        /// A group containing one or more row elements in a grid.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#rowgroup
        static member inline rowGroup = Interop.mkAttr "role" "rowgroup"
        /// A cell containing header information for a row in a grid.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#rowheader
        static member inline rowHeader = Interop.mkAttr "role" "rowheader"
        /// A graphical object that controls the scrolling of content within a
        /// viewing area, regardless of whether the content is fully displayed
        /// within the viewing area.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#scrollbar
        static member inline scrollBar = Interop.mkAttr "role" "scrollbar"
        /// A divider that separates and distinguishes sections of content or
        /// groups of menuitems.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#separator
        static member inline separator = Interop.mkAttr "role" "separator"
        /// A `landmark` region that contains a collection of items and objects
        /// that, as a whole, combine to create a search facility. See related
        /// `form`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#search
        static member inline search = Interop.mkAttr "role" "search"
        /// A user input where the user selects a value from within a given
        /// range.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#slider
        static member inline slider = Interop.mkAttr "role" "slider"
        /// A form of `range` that expects the user to select from among
        /// discrete choices.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#spinbutton
        static member inline spinButton = Interop.mkAttr "role" "spinbutton"
        /// A container whose content is advisory information for the user but
        /// is not important enough to justify an alert, often but not
        /// necessarily presented as a status bar. See related `alert`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#status
        static member inline status = Interop.mkAttr "role" "status"
        /// A grouping label providing a mechanism for selecting the tab content
        /// that is to be rendered to the user.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#tab
        static member inline tab = Interop.mkAttr "role" "tab"
        /// A list of `tab` elements, which are references to `tabpanel`
        /// elements.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#tablist
        static member inline tabList = Interop.mkAttr "role" "tablist"
        /// A container for the resources associated with a `tab`, where each
        /// `tab` is contained in a `tablist`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#tabpanel
        static member inline tabPanel = Interop.mkAttr "role" "tabpanel"
        /// Input that allows free-form text as its value.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#textbox
        static member inline textBox = Interop.mkAttr "role" "textbox"
        /// A type of live region containing a numerical counter which indicates
        /// an amount of elapsed time from a start point, or the time remaining
        /// until an end point.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#timer
        static member inline timer = Interop.mkAttr "role" "timer"
        /// A collection of commonly used function buttons or controls
        /// represented in compact visual form.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#toolbar
        static member inline toolbar = Interop.mkAttr "role" "toolbar"
        /// A contextual popup that displays a description for an element.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#tooltip
        static member inline tooltip = Interop.mkAttr "role" "tooltip"
        /// A type of `list` that may contain sub-level nested groups that can
        /// be collapsed and expanded.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#tree
        static member inline tree = Interop.mkAttr "role" "tree"
        /// A `grid` whose rows can be expanded and collapsed in the same manner
        /// as for a `tree`.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#treegrid
        static member inline treeGrid = Interop.mkAttr "role" "treegrid"
        /// An option item of a `tree`. This is an element within a tree that
        /// may be expanded or collapsed if it contains a sub-level group of
        /// `treeitem` elements.
        ///
        /// https://www.w3.org/WAI/PF/aria-1.1/roles#treeitem
        static member inline treeItem = Interop.mkAttr "role" "treeitem"        

    /// The shape of the associated hot spot.
    [<Erase>]
    type shape =
        static member inline rect = Interop.mkAttr "shape" "rect"
        static member inline circle = Interop.mkAttr "shape" "circle"
        static member inline poly = Interop.mkAttr "shape" "poly"

    [<Erase>]
    type target =
        /// Opens the linked document in a new window or tab.
        static member inline blank = Interop.mkAttr "target" "_blank"
        /// Opens the linked document in the parent frame.
        static member inline parent = Interop.mkAttr "target" "_parent"
        /// Opens the linked document in the same frame as it was clicked (this is default).
        static member inline self = Interop.mkAttr "target" "_self"
        /// Opens the linked document in the full body of the window.
        static member inline top = Interop.mkAttr "target" "_top"
    
    /// The `text-anchor` attribute is used to align (start-, middle- or
    /// end-alignment) a string of pre-formatted text or auto-wrapped text where
    /// the wrapping area is determined from the `inline-size` property relative
    /// to a given point. It is not applicable to other types of auto-wrapped
    /// text. For those cases you should use `text-align`. For multi-line text,
    /// the alignment takes place for each line.
    ///
    /// The `text-anchor` attribute is applied to each individual text chunk
    /// within a given `<text>` element. Each text chunk has an initial current
    /// text position, which represents the point in the user coordinate system
    /// resulting from (depending on context) application of the `x` and `y`
    /// attributes on the `<text>` element, any `x` or `y` attribute values on a
    /// `<tspan>`, `<tref>` or `<altGlyph>` element assigned explicitly to the
    /// first rendered character in a text chunk, or determination of the
    /// initial current text position for a `<textPath>` element.
    [<Erase>]
    type textAnchor =
        /// The rendered characters are shifted such that the end of the
        /// resulting rendered text (final current text position before applying
        /// the `text-anchor` property) is at the initial current text position.
        /// For an element with a `direction` property value of `ltr` (typical
        /// for most European languages), the right side of the text is rendered
        /// at the initial text position. For an element with a `direction`
        /// property value of `rtl` (typical for Arabic and Hebrew), the left
        /// side of the text is rendered at the initial text position. For an
        /// element with a vertical primary text direction (often typical for
        /// Asian text), the bottom of the text is rendered at the initial text
        /// position.
        static member inline endOfText = Interop.mkAttr "textAnchor" "end"
        /// The rendered characters are aligned such that the middle of the text
        /// string is at the current text position. (For text on a path,
        /// conceptually the text string is first laid out in a straight line.
        /// The midpoint between the start of the text string and the end of the
        /// text string is determined. Then, the text string is mapped onto the
        /// path with this midpoint placed at the current text position.)
        static member inline middle = Interop.mkAttr "textAnchor" "middle"
        /// The rendered characters are aligned such that the start of the text
        /// string is at the initial current text position. For an element with
        /// a `direction` property value of `ltr` (typical for most European
        /// languages), the left side of the text is rendered at the initial
        /// text position. For an element with a `direction` property value of
        /// `rtl` (typical for Arabic and Hebrew), the right side of the text is
        /// rendered at the initial text position. For an element with a
        /// vertical primary text direction (often typical for Asian text), the
        /// top side of the text is rendered at the initial text position.
        static member inline startOfText = Interop.mkAttr "textAnchor" "start"

    [<Erase>]
    type transform =
        /// Defines a 2D transformation, using a matrix of six values.
        static member inline matrix(x1: int, y1: int, z1: int, x2: int, y2: int, z2: int) =
            Interop.mkAttr "transform" (
                "matrix(" +
                (unbox<string> x1) + "," +
                (unbox<string> y1) + "," +
                (unbox<string> z1) + "," +
                (unbox<string> x2) + "," +
                (unbox<string> y2) + "," +
                (unbox<string> z2) + ")"
            )
        /// Defines that there should be no transformation.
        static member inline none = Interop.mkAttr "transform" "none"
        /// Defines a perspective view for a 3D transformed element
        static member inline perspective(n: int) =
            Interop.mkAttr "transform" ("perspective(" + (unbox<string> n) + ")")
        /// Defines a 2D rotation, the angle is specified in the parameter.
        static member inline rotate(deg: int) =
            Interop.mkAttr "transform" ("rotate(" + (unbox<string> deg) + "deg)")
        /// Defines a 2D rotation, the angle is specified in the parameter.
        static member inline rotate(deg: float) =
            Interop.mkAttr "transform" ("rotate(" + (unbox<string> deg) + "deg)")
        /// Defines a 3D rotation along the X-axis.
        static member inline rotateX(deg: float) =
            Interop.mkAttr "transform" ("rotateX(" + (unbox<string> deg) + "deg)")
        /// Defines a 3D rotation along the X-axis.
        static member inline rotateX(deg: int) =
            Interop.mkAttr "transform" ("rotateX(" + (unbox<string> deg) + "deg)")
        /// Defines a 3D rotation along the Y-axis
        static member inline rotateY(deg: float) =
            Interop.mkAttr "transform" ("rotateY(" + (unbox<string> deg) + "deg)")
        /// Defines a 3D rotation along the Y-axis
        static member inline rotateY(deg: int) =
            Interop.mkAttr "transform" ("rotateY(" + (unbox<string> deg) + "deg)")
        /// Defines a 3D rotation along the Z-axis
        static member inline rotateZ(deg: float) =
            Interop.mkAttr "transform" ("rotateZ(" + (unbox<string> deg) + "deg)")
        /// Defines a 3D rotation along the Z-axis
        static member inline rotateZ(deg: int) =
            Interop.mkAttr "transform" ("rotateZ(" + (unbox<string> deg) + "deg)")
        /// Defines a 2D scale transformation.
        static member inline scale(x: int, y: int) =
            Interop.mkAttr "transform" (
                "scale(" + (unbox<string> x) + "," + (unbox<string> y) + ")"
            )
        /// Defines a 3D scale transformation
        static member inline scale3D(x: int, y: int, z: int) =
            Interop.mkAttr "transform" (
                "scale3d(" + (unbox<string> x) + "," + (unbox<string> y) + "," + (unbox<string> z) + ")"
            )
        /// Defines a scale transformation by giving a value for the X-axis.
        static member inline scaleX(x: int) =
            Interop.mkAttr "transform" ("scaleX(" + (unbox<string> x) + ")")
        /// Defines a scale transformation by giving a value for the Y-axis.
        static member inline scaleY(y: int) =
            Interop.mkAttr "transform" ("scaleY(" + (unbox<string> y) + ")")
        /// Defines a 3D translation, using only the value for the Z-axis
        static member inline scaleZ(z: int) =
            Interop.mkAttr "transform" ("scaleZ(" + (unbox<string> z) + ")")
        /// Defines a 2D skew transformation along the X- and the Y-axis.
        static member inline skew(xAngle: int, yAngle: int) =
            Interop.mkAttr "transform" ("skew(" + (unbox<string> xAngle) + "deg," + (unbox<string> yAngle) + "deg)")
        /// Defines a 2D skew transformation along the X- and the Y-axis.
        static member inline skew(xAngle: float, yAngle: float) =
            Interop.mkAttr "transform" ("skew(" + (unbox<string> xAngle) + "deg," + (unbox<string> yAngle) + "deg)")
        /// Defines a 2D skew transformation along the X-axis
        static member inline skewX(xAngle: int) =
            Interop.mkAttr "transform" ("skewX(" + (unbox<string> xAngle) + "deg)")
        /// Defines a 2D skew transformation along the X-axis
        static member inline skewX(xAngle: float) =
            Interop.mkAttr "transform" ("skewX(" + (unbox<string> xAngle) + "deg)")
        /// Defines a 2D skew transformation along the Y-axis
        static member inline skewY(xAngle: int) =
            Interop.mkAttr "transform" ("skewY(" + (unbox<string> xAngle) + "deg)")
        /// Defines a 2D skew transformation along the Y-axis
        static member inline skewY(xAngle: float) =
            Interop.mkAttr "transform" ("skewY(" + (unbox<string> xAngle) + "deg)")
        /// Defines a 2D translation.
        static member inline translate(x: int, y: int) =
            Interop.mkAttr "transform" (
                "translate(" + (unbox<string> x) + "," + (unbox<string> y) + ")"
            )
        static member inline translate(x: float, y: float) =
            Interop.mkAttr "transform" (
                "translate(" + (unbox<string> x) + "," + (unbox<string> y) + ")"
            )
        /// Defines that there should be no transformation.
        static member inline translate3D(x: int, y: int, z: int) =
            Interop.mkAttr "transform" (
                "translate3d(" + (unbox<string> x) + "," + (unbox<string> y) + "," + (unbox<string> z) + ")"
            )
        /// Defines a translation, using only the value for the X-axis.
        static member inline translateX(x: int) =
            Interop.mkAttr "transform" ("translateX(" + (unbox<string> x) + ")")
        /// Defines a translation, using only the value for the Y-axis
        static member inline translateY(y: int) =
            Interop.mkAttr "transform" ("translateY(" + (unbox<string> y) + ")")
        /// Defines a 3D translation, using only the value for the Z-axis
        static member inline translateZ(z: int) =
            Interop.mkAttr "transform" ("translateZ(" + (unbox<string> z) + ")")

    [<Erase>]
    type type' =
        /// Defines a clickable button (mostly used with a JavaScript code to activate a script)
        static member inline button = Interop.mkAttr "type" "button"
        /// Defines a checkbox
        static member inline checkbox = Interop.mkAttr "type" "checkbox"
        /// Defines a color picker
        static member inline color = Interop.mkAttr "type" "color"
        /// Defines a date control with year, month and day (no time)
        static member inline date = Interop.mkAttr "type" "date"
        /// Defines a date and time control (year, month, day, time (no timezone)
        static member inline dateTimeLocal = Interop.mkAttr "type" "datetime-local"
        /// Defines a field for an e-mail address
        static member inline email = Interop.mkAttr "type" "email"
        /// Defines a file-select field and a "Browse" button (for file uploads)
        static member inline file = Interop.mkAttr "type" "file"
        /// Defines a hidden input field
        static member inline hidden = Interop.mkAttr "type" "hidden"
        /// Defines an image as the submit button
        static member inline image = Interop.mkAttr "type" "image"
        /// Defines a month and year control (no timezone)
        static member inline month = Interop.mkAttr "type" "month"
        /// Defines a field for entering a number
        static member inline number = Interop.mkAttr "type" "number"
        /// Defines a password field
        static member inline password = Interop.mkAttr "type" "password"
        /// Defines a radio button
        static member inline radio = Interop.mkAttr "type" "radio"
        /// Defines a range control (like a slider control)
        static member inline range = Interop.mkAttr "type" "range"
        /// Defines a reset button
        static member inline reset = Interop.mkAttr "type" "reset"
        /// Defines a text field for entering a search string
        static member inline search = Interop.mkAttr "type" "search"
        /// Defines a submit button
        static member inline submit = Interop.mkAttr "type" "submit"
        /// Defines a field for entering a telephone number
        static member inline tel = Interop.mkAttr "type" "tel"
        /// Default. Defines a single-line text field
        static member inline text = Interop.mkAttr "type" "text"
        /// Defines a control for entering a time (no timezone)
        static member inline time = Interop.mkAttr "type" "time"
        /// Defines a field for entering a URL
        static member inline url = Interop.mkAttr "type" "url"
        /// Defines a week and year control (no timezone)
        static member inline week = Interop.mkAttr "type" "week"

    [<Erase>]
    type autoCapitalize =
        /// No autocapitalization is applied (all letters default to lowercase)
        static member inline off = Interop.mkAttr "autoCapitalize" "off"
        /// The first letter of each sentence defaults to a capital letter; all other letters default to lowercase
        static member inline on' = Interop.mkAttr "autoCapitalize" "on"
        /// The first letter of each word defaults to a capital letter; all other letters default to lowercase
        static member inline words = Interop.mkAttr "autoCapitalize" "words"
        /// All letters should default to uppercase
        static member inline characters = Interop.mkAttr "autoCapitalize" "characters"
