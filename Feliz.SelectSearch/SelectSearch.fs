namespace Feliz.SelectSearch

open Feliz
open Fable.Core
open Fable.Core.JsInterop

type ISelectSearchAttribute = interface end

module Interop =
    [<Emit "Object.entries($0)">]
    let objectEntries (x: obj) : (string * obj) array = jsNative

[<Erase>]
type SelectSearch() =
    static member inline selectSearch (properties: ISelectSearchAttribute list) =
        Interop.reactApi.createElement(importDefault "react-select-search", createObj !!properties)

type SelectItem = { value: string; name: string; disabled: bool }

type SelectGroup = { name: string; items: SelectItem list; disabled: bool }

[<RequireQualifiedAccess>]
type SelectOption =
    | Item of SelectItem
    | Group of SelectGroup

[<StringEnum; RequireQualifiedAccess>]
type SearchClasses =
    | [<CompiledName "container">] Container
    | [<CompiledName "value">] Value
    | [<CompiledName "input">] Input
    | [<CompiledName "select">] Select
    | [<CompiledName "options">] Options
    | [<CompiledName "option">] Option
    | [<CompiledName "option">] Group
    | [<CompiledName "group-header">] GroupHeader
    | [<CompiledName "is-selected">] Selected
    | [<CompiledName "is-highlighted">] Highlighted
    | [<CompiledName "is-loading">] Loading
    | [<CompiledName "has-focus">] HasFocus
    with
        [<Emit "$0">]
        member self.ClassName = jsNative

type OptionRendererProps = {
    optionAttributes: IReactProperty seq
    optionClassName: string
    option: SelectItem
}

type selectSearch =
    /// <summary>Sets the options of the select control</summary>
    static member options (items: seq<SelectItem>) =
        let selectOptions = [|
            for item in items -> createObj [
                "value" ==> item.value
                "name" ==> item.name
                "disabled" ==> item.disabled
            ]
        |]

        unbox<ISelectSearchAttribute> ("options", selectOptions)

    /// <summary>Sets the options of the select control</summary>
    static member options (groups: seq<SelectGroup>) =
        let selectOptions = [|
            for group in groups -> createObj [
                "type" ==> "group"
                "name" ==> group.name
                "disabled" ==> group.disabled
                "items" ==> [|
                    for item in group.items -> createObj [
                        "value" ==> item.value
                        "name" ==> item.name
                    ]
                |]
            ]
        |]

        unbox<ISelectSearchAttribute> ("options", selectOptions)

    /// <summary>Sets the options of the select control</summary>
    static member options (mixedOptions: seq<SelectOption>) =
        let selectOptions = [|
            for option in mixedOptions ->
                match option with
                | SelectOption.Item item ->
                    createObj [
                        "value" ==> item.value
                        "name" ==> item.name
                        "disabled" ==> item.disabled
                    ]
                | SelectOption.Group group ->
                    createObj [
                        "type" ==> "group"
                        "name" ==> group.name
                        "disabled" ==> group.disabled
                        "items" ==> [|
                            for item in group.items -> createObj [
                                "value" ==> item.value
                                "name" ==> item.name
                                "disabled" ==> item.disabled
                            ]
                        |]
                    ]
        |]

        unbox<ISelectSearchAttribute> ("options", selectOptions)

    /// <summary>Sets the currently selected value when in controlled mode</summary>
    static member inline value(currentValue: string) = unbox<ISelectSearchAttribute> ("value", currentValue)
    /// <summary>Sets the currently selected values when in controlled mode and multiple is enabled</summary>
    static member inline value(currentValues: string[]) = unbox<ISelectSearchAttribute> ("value", currentValues)
    /// <summary>Sets the currently selected values when in controlled mode and multiple is enabled</summary>
    static member inline value(currentValues: string list) = unbox<ISelectSearchAttribute> ("value", Array.ofList currentValues)
    /// <summary>Set to true if you want to allow multiple selected options. False by default</summary>
    static member inline multiple(selectingMultipleValues: bool) = unbox<ISelectSearchAttribute> ("multiple", selectingMultipleValues)
    /// <summary>Set to true to enable search functionality. False by default</summary>
    static member inline search(searchable: bool) = unbox<ISelectSearchAttribute> ("search", searchable)
    /// <summary>Disables all functionality</summary>
    static member inline disabled(disableControl: bool) = unbox<ISelectSearchAttribute> ("disabled", disableControl)
    /// <summary>The selectbox will blur by default when selecting an option. Set this to false to prevent this behavior. True by default</summary>
    static member inline closeOnSelect(closeMenuOnSelect: bool) = unbox<ISelectSearchAttribute> ("closeOnSelect", closeMenuOnSelect)
    /// <summary>Displayed if no option is selected and/or when search field is focused with empty value.</summary>
    static member inline placeholder (text: string) = unbox<ISelectSearchAttribute> ("placeholder", text)
    /// <summary>HTML ID on the top level element</summary>
    static member inline id (elementIdentifier: string) = unbox<ISelectSearchAttribute> ("id", elementIdentifier)
    /// <summary>Autofocus on select</summary>
    static member inline autoFocus (autoFocusOnSelect: bool) = unbox<ISelectSearchAttribute>("authoFocus", autoFocusOnSelect)
    /// <summary>Set a base class name for the select</summary>
    static member inline className (name: string) = unbox<ISelectSearchAttribute>("className", name)
    /// <summary>Allows you to customize the default class names of the various elements</summary>
    static member inline className (modifyClasses: SearchClasses -> string) = unbox<ISelectSearchAttribute>("className", modifyClasses)
    /// <summary>Set empty message for empty options list</summary>
    static member inline emptyMessage (message: string) = unbox<ISelectSearchAttribute> ("emptyMessage", message)
    /// <summary>Set empty message for empty options list</summary>
    static member inline emptyMessage (message: unit -> ReactElement) = unbox<ISelectSearchAttribute> ("emptyMessage", message)
    /// <summary>Function to receive and handle value changes.</summary>
    static member inline onChange (handler: string -> unit) =  unbox<ISelectSearchAttribute> ("onChange", handler)
    /// <summary>Focus callback.</summary>
    static member inline onFocus(handler: unit -> unit) = unbox<ISelectSearchAttribute> ("onFocus", handler)
    /// <summary>Blur callback.</summary>
    static member inline onBlur(handler: unit -> unit) = unbox<ISelectSearchAttribute> ("onFocus", handler)
    static member inline renderOption (renderer: OptionRendererProps -> ReactElement) =

        let internalHandler = System.Func<obj, obj, obj, string, ReactElement>(fun props option snapshot classname ->
            let entries = Interop.objectEntries props
            renderer {
                optionAttributes = unbox (Seq.toArray entries)
                option = { name = option?name; value = option?Value; disabled = option?disabled }
                optionClassName = classname
            }
        )

        unbox<ISelectSearchAttribute> ("renderOption", internalHandler)

    static member inline filterOptions (search: SelectItem array -> string -> SelectItem array) =
        let handler (values: obj[]) (searchQuery: string) =
            let items =
                values
                |> Array.map (fun jsItem -> {
                    name = jsItem?name
                    value = jsItem?value
                    disabled = jsItem?disabled
                })

            search items searchQuery

        unbox<ISelectSearchAttribute> ("filterOptions", handler)

module selectSearch =
    importSideEffects "./SelectSearch.css"
    /// <summary>This property controls when the options list should be rendered.</summary>
    type printOptions =
        static member inline auto = unbox<ISelectSearchAttribute> ("printOptions", "auto")
        static member inline always = unbox<ISelectSearchAttribute> ("printOptions", "always")
        static member inline never = unbox<ISelectSearchAttribute> ("printOptions", "never")
        static member inline onFocus = unbox<ISelectSearchAttribute> ("printOptions", "on-focus")

    /// <summary>Disables/Enables autoComplete functionality in search field.</summary>
    type autoComplete =
        static member inline on = unbox<ISelectSearchAttribute> ("autoComplete", "on")
        static member inline off = unbox<ISelectSearchAttribute> ("autoComplete", "off")