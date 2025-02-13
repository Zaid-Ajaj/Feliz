namespace Feliz.Router

open Browser.Dom
open Browser.Types
open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Feliz
open Feliz.Listeners
open System

type IUrlSearchParamters =
    abstract entries : unit -> seq<string array>

/// Determines whether the router will push a new entry to the history of modify the current one.
[<RequireQualifiedAccess>]
type HistoryMode =
    /// A new history will be added to the entries such that if the user clicks the back button,
    /// the previous page will be shown, this is the default bahavior of the router.
    | PushState = 1
    /// Only modifies the current history entry and does not add a new one to the history stack. Clicking the back button will *not* have the effect of retuning to the previous page.
    | ReplaceState = 2

/// Determines whether the router will use path or hash based routes
[<RequireQualifiedAccess>]
type RouteMode =
    | Hash = 1
    | Path = 2

[<RequireQualifiedAccess; System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)>]
module Router =
    [<RequireQualifiedAccess>]
    module String =
        let (|Prefix|) (prefix: string) (str: string) =
            if str.StartsWith prefix then Some str
            else None

        let (|Suffix|) (suffix: string) (str: string) =
            if str.EndsWith suffix then Some str
            else None

        let inline split (sep: char) (str: string) =
            str.Split(sep)

    let [<Literal>] customNavigationEvent = "CUSTOM_NAVIGATION_EVENT"
    let inline hashPrefix str = "#/" + str
    let inline combine xs = String.concat "/" xs

    type RouterProps =
        abstract onUrlChanged: (string list -> unit) option
        abstract application: ReactElement option
        abstract hashMode: RouteMode option

    [<Emit("encodeURIComponent($0)")>]
    let encodeURIComponent (value: string) : string = jsNative
    [<Emit("decodeURIComponent($0)")>]
    let decodeURIComponent (value: string) : string = jsNative

    let encodeQueryString queryStringPairs =
        queryStringPairs
        |> List.map (fun (key, value) ->
            String.concat "=" [ encodeURIComponent key; encodeURIComponent value ])
        |> String.concat "&"
        |> function
            | "" -> ""
            | pairs -> "?" + pairs

    let encodeQueryStringInts queryStringIntPairs =
        queryStringIntPairs
        |> List.map (fun (key, value: int) ->
            String.concat "=" [ encodeURIComponent key; unbox<string> value ])
        |> String.concat "&"
        |> function
            | "" -> ""
            | pairs -> "?" + pairs

    let private normalizeRoute routeMode =
        if routeMode = RouteMode.Hash then
            function
            | String.Prefix "/" (Some path) -> "#" + path
            | String.Prefix "#/" (Some path) -> path
            | String.Prefix "#" (Some path) -> "#/" + path.Substring(1, path.Length - 1)
            | path -> "#/" + path
        else
            function
            | String.Prefix "/" (Some path) -> path
            | path -> "/" + path

    let encodeParts xs routeMode =
        xs
        |> List.map (fun (part: string) ->
            if part.Contains "?" || part.StartsWith "#" || part.StartsWith "/" then part
            else encodeURIComponent part)
        |> combine
        |> normalizeRoute routeMode

    let nav xs (mode: HistoryMode) (routeMode: RouteMode) =
        if mode = HistoryMode.PushState
        then history.pushState ((), "", encodeParts xs routeMode)
        else history.replaceState((), "", encodeParts xs routeMode)

        let ev = document.createEvent("CustomEvent")

        ev.initEvent (customNavigationEvent, true, true)
        window.dispatchEvent ev |> ignore

    /// Parses the URL into multiple path segments
    let urlSegments (path: string) (mode: RouteMode) =
        match path with
        | String.Prefix "#" (Some _) ->
            // remove the hash sign
            path.Substring(1, path.Length - 1)
        | _ when mode = RouteMode.Hash ->
            match path with
            | String.Suffix "#" (Some _)
            | String.Suffix "#/" (Some _) -> ""
            | _ -> path
        | _ -> path
        |> String.split '/'
        |> List.ofArray
        |> List.collect (fun segment ->
            if String.IsNullOrWhiteSpace segment then []
            else
                let segment = segment.TrimEnd '#'

                match segment with
                | "?" -> []
                | String.Prefix "?" (Some _) -> [ segment ]
                | _ ->
                    match segment.Split [| '?' |] with
                    | [| value |] -> [ decodeURIComponent value ]
                    | [| value; "" |] -> [ decodeURIComponent value ]
                    | [| value; query |] -> [ decodeURIComponent value; "?" + query ]
                    | _ -> [])

    [<Emit("new URLSearchParams($0)")>]
    let createUrlSearchParams (queryString: string) : IUrlSearchParamters = jsNative

    [<Emit("window.navigator.userAgent")>]
    let navigatorUserAgent : string = jsNative

    let onUrlChange routeMode urlChanged (ev: _) =
        match routeMode with
        | RouteMode.Path -> window.location.pathname + window.location.search
        | _ -> window.location.hash
        |> fun path -> urlSegments path routeMode
        |> urlChanged

    let router = React.memo(fun (input: RouterProps) ->
        let onChange = React.useCallbackRef(fun (ev: Event) ->
            let urlChanged = Option.defaultValue ignore input.onUrlChanged
            let routeMode = Option.defaultValue RouteMode.Hash input.hashMode

            onUrlChange routeMode urlChanged ev)

        if navigatorUserAgent.Contains "Trident" || navigatorUserAgent.Contains "MSIE"
        then React.useWindowListener.onHashChange(onChange)
        else React.useWindowListener.onPopState(onChange)

        React.useWindowListener.on(customNavigationEvent, onChange)

        match input.application with
        | Some elem -> elem
        | None -> Html.none)

/// Defines a property for the `router` element
type IRouterProperty = interface end

[<AutoOpen>]
module ReactExtension =
    type React with
        /// Initializes the router as an element of the page and starts listening to URL changes.
        static member inline router (props: IRouterProperty list) =
            Router.router (unbox<Router.RouterProps> (createObj !!props))

[<Erase>]
type router =
    /// An event that is triggered when the URL in the address bar changes, whether by hand or programmatically using `Router.navigate(...)`.
    /// The event arguments are the parts of the URL, segmented into strings:
    ///
    /// `segment "#/" => [ ]`
    ///
    /// `segment "#/home" => [ "home" ]`
    ///
    /// `segment "#/home/settings" => [ "home"; "settings" ]`
    ///
    /// `segment "#/users/1" => [ "users"; "1" ]`
    ///
    /// `segment "#/users/1/details" => [ "users"; "1"; "details" ]`
    ///
    /// with query string parameters
    ///
    /// `segment "#/users?id=1" => [ "users"; "?id=1" ]`
    ///
    /// `segment "#/home/users?id=1" => [ "home"; "users"; "?id=1" ]`
    ///
    /// `segment "#/users?id=1&format=json" => [ "users"; "?id=1&format=json" ]`
    static member inline onUrlChanged (eventHandler: string list -> unit) : IRouterProperty = unbox ("onUrlChanged", eventHandler)

    /// The element that is rendered inside where the `router` is placed. Usually this contains the root application but it could also be part of another root element.
    ///
    /// It will keep listening for URL changes as long as the `router` is rendered on screen somewhere.
    static member inline children (element: ReactElement) : IRouterProperty = unbox ("application", element)

    /// The content that is rendered inside where the `router` is placed. Usually this contains the root application but it could also be part of another root element.
    ///
    /// It will keep listening for URL changes as long as the `router` is rendered on screen somewhere.
    static member inline children (elements: ReactElement list) : IRouterProperty = unbox ("application", React.fragment elements)

    /// Use # based routes (default)
    static member inline hashMode : IRouterProperty = unbox ("hashMode", RouteMode.Hash)

    /// Use full (HTML 5) based routes instead of # based.
    /// You have to be careful about which requests you want forwarded to the server and which ones should be handled locally.
    /// To keep the request local, you have to use the 'Router.navigate' function for all the URL transitions.
    static member inline pathMode : IRouterProperty = unbox ("hashMode", RouteMode.Path)

[<Erase>]
type Router =
    /// Parses the current URL of the page and returns the cleaned URL segments. This is default when working with hash URLs. When working with path-based URLs, use Router.currentPath() instead.
    static member inline currentUrl () =
        Router.urlSegments window.location.hash RouteMode.Hash

    /// Parses the current URL of the page and returns the cleaned URL segments. This is default when working with path URLs. When working with hash-based (#) URLs, use Router.currentUrl() instead.
    static member inline currentPath () =
        let fullPath = window.location.pathname + window.location.search
        Router.urlSegments fullPath RouteMode.Path

    static member inline format([<ParamArray>] xs: string array) =
        Router.encodeParts (List.ofArray xs) RouteMode.Hash

    static member inline format(segment: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:int, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:int, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; segment3 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; segment3 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Hash

    static member inline format(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Hash

    static member inline format(fullPath: string) : string =
        Router.encodeParts [ fullPath ] RouteMode.Hash

    static member inline format(fullPath: string list) : string =
        Router.encodeParts fullPath RouteMode.Hash

    static member inline format(segment: string, value: int) : string =
        Router.encodeParts [ segment; string value ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, value2: int) : string =
        Router.encodeParts [ segment1; string value1; string value2 ] RouteMode.Hash

    static member inline format(segment1: string, segment2: string, value1: int) : string =
        Router.encodeParts [ segment1; segment2; string value1 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, segment2: string) : string =
        Router.encodeParts [ segment1; string value1; segment2 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, segment2: string, value2: int) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, segment2: string, value2: int, segment3: string) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2; segment3 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2; segment3; segment4 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2; segment3; string value3 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, value2: int, value3: int) : string =
        Router.encodeParts [ segment1; string value1; string value2; string value3 ] RouteMode.Hash

    static member inline format(segment1: string, value1: int, value2: int, segment2: string) : string =
        Router.encodeParts [ segment1; string value1; string value2; segment2 ] RouteMode.Hash


    static member inline navigate([<ParamArray>] xs: string array) =
        Router.nav (List.ofArray xs) HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment: string, queryString: (string * string) list) =
        Router.nav [ segment + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment: string, queryString: (string * int) list) =
        Router.nav [ segment + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, queryString: (string * string) list) =
        Router.nav [ segment1; segment2 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, queryString: (string * int) list) =
        Router.nav [ segment1; segment2 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2;string  segment3 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Hash

    static member inline navigate(fullPath: string) =
        Router.nav [ fullPath ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(fullPath: string, mode: HistoryMode) =
        Router.nav [ fullPath ] mode RouteMode.Hash

    static member inline navigate(segment: string, value: int) =
        Router.nav [ segment; string value ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment: string, value: int, mode: HistoryMode) =
        Router.nav [ segment; string value ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, value2: int) =
        Router.nav [ segment1; string value1; string value2 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, value2: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; string value2 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, value1: int) =
        Router.nav [ segment1; segment2; string value1 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, segment2: string, value1: int, mode: HistoryMode) =
        Router.nav [ segment1; segment2; string value1 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string) =
        Router.nav [ segment1; string value1; segment2 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int) =
        Router.nav [ segment1; string value1; segment2; string value2 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; segment4 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; segment4 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; string value3 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; string value3 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, value2: int, value3: int) =
        Router.nav [ segment1; string value1; string value2; string value3 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, value2: int, value3: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; string value2; string value3 ] mode RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, value2: int, segment2: string) =
        Router.nav [ segment1; string value1; string value2; segment2 ] HistoryMode.PushState RouteMode.Hash

    static member inline navigate(segment1: string, value1: int, value2: int, segment2: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; string value2; segment2 ] mode RouteMode.Hash


    static member inline navigatePath([<ParamArray>] xs: string array) =
        Router.nav (List.ofArray xs) HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment: string, queryString: (string * string) list) =
        Router.nav [ segment + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment: string, queryString: (string * int) list) =
        Router.nav [ segment + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * string) list) =
        Router.nav [ segment1; segment2 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * int) list) =
        Router.nav [ segment1; segment2 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2;string  segment3 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) =
        Router.nav [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] mode RouteMode.Path

    static member inline navigatePath(fullPath: string) =
        Router.nav [ fullPath ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(fullPath: string, mode: HistoryMode) =
        Router.nav [ fullPath ] mode RouteMode.Path

    static member inline navigatePath(segment: string, value: int) =
        Router.nav [ segment; string value ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment: string, value: int, mode: HistoryMode) =
        Router.nav [ segment; string value ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, value2: int) =
        Router.nav [ segment1; string value1; string value2 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, value2: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; string value2 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, value1: int) =
        Router.nav [ segment1; segment2; string value1 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, segment2: string, value1: int, mode: HistoryMode) =
        Router.nav [ segment1; segment2; string value1 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string) =
        Router.nav [ segment1; string value1; segment2 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int) =
        Router.nav [ segment1; string value1; segment2; string value2 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; segment4 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; segment4 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; string value3 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; segment2; string value2; segment3; string value3 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, value2: int, value3: int) =
        Router.nav [ segment1; string value1; string value2; string value3 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, value2: int, value3: int, mode: HistoryMode) =
        Router.nav [ segment1; string value1; string value2; string value3 ] mode RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, value2: int, segment2: string) =
        Router.nav [ segment1; string value1; string value2; segment2 ] HistoryMode.PushState RouteMode.Path

    static member inline navigatePath(segment1: string, value1: int, value2: int, segment2: string, mode: HistoryMode) =
        Router.nav [ segment1; string value1; string value2; segment2 ] mode RouteMode.Path


    static member inline formatPath([<ParamArray>] xs: string array) =
        Router.encodeParts (List.ofArray xs) RouteMode.Path

    static member inline formatPath(segment: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:int, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:int, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; unbox<string> segment3 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; segment3 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; segment3 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; segment4; segment5; segment6 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; unbox<string> segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; unbox<string> segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryString queryString ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list) : string =
        Router.encodeParts [ segment1; unbox<string> segment2; segment3; segment4; segment5 + Router.encodeQueryStringInts queryString ] RouteMode.Path

    static member inline formatPath(fullPath: string) : string =
        Router.encodeParts [ fullPath ] RouteMode.Path

    static member inline formatPath(fullPath: string list) : string =
        Router.encodeParts fullPath RouteMode.Path

    static member inline formatPath(segment: string, value: int) : string =
        Router.encodeParts [ segment; string value ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, value2: int) : string =
        Router.encodeParts [ segment1; string value1; string value2 ] RouteMode.Path

    static member inline formatPath(segment1: string, segment2: string, value1: int) : string =
        Router.encodeParts [ segment1; segment2; string value1 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, segment2: string) : string =
        Router.encodeParts [ segment1; string value1; segment2 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, segment2: string, value2: int) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, segment2: string, value2: int, segment3: string) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2; segment3 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2; segment3; segment4 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int) : string =
        Router.encodeParts [ segment1; string value1; segment2; string value2; segment3; string value3 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, value2: int, value3: int) : string =
        Router.encodeParts [ segment1; string value1; string value2; string value3 ] RouteMode.Path

    static member inline formatPath(segment1: string, value1: int, value2: int, segment2: string) : string =
        Router.encodeParts [ segment1; string value1; string value2; segment2 ] RouteMode.Path

[<Erase>]
type Cmd =
    static member inline navigate([<ParamArray>] xs: string array) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(xs) ]

    static member inline navigate(segment: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment, queryString) ]

    static member inline navigate(segment: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment, queryString, mode) ]

    static member inline navigate(segment: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment, queryString) ]

    static member inline navigate(segment: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment, queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, unbox<string> segment3 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2,string  segment3 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, unbox<string> segment3 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:int, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, unbox<string> segment3 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, unbox<string> segment2, segment3 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, unbox<string> segment2, segment3 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, unbox<string> segment2, segment3 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, unbox<string> segment2, segment3 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5, segment6, queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5, segment6, queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5, segment6, queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5, segment6, queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString) ]

    static member inline navigate(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, segment3, segment4, segment5 , queryString, mode) ]

    static member inline navigate(fullPath: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(fullPath) ]

    static member inline navigate(fullPath: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(fullPath, mode) ]

    static member inline navigate(segment: string, value: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment, value) ]

    static member inline navigate(segment: string, value: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment, value, mode) ]

    static member inline navigate(segment1: string, value1: int, value2: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, value2) ]

    static member inline navigate(segment1: string, value1: int, value2: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, value2, mode) ]

    static member inline navigate(segment1: string, segment2: string, value1: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, value1) ]

    static member inline navigate(segment1: string, segment2: string, value1: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, segment2, value1, mode) ]

    static member inline navigate(segment1: string, value1: int, segment2: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, mode) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, mode) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, segment3) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, segment3, mode) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, segment3, segment4) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, segment3, segment4, mode) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, segment3, value3) ]

    static member inline navigate(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, segment2, value2, segment3, value3, mode) ]

    static member inline navigate(segment1: string, value1: int, value2: int, value3: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, value2, value3) ]

    static member inline navigate(segment1: string, value1: int, value2: int, value3: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, value2, value3, mode) ]

    static member inline navigate(segment1: string, value1: int, value2: int, segment2: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, value2, segment2) ]

    static member inline navigate(segment1: string, value1: int, value2: int, segment2: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigate(segment1, value1, value2, segment2, mode) ]


    static member inline navigatePath([<ParamArray>] xs: string array) =
        [ fun _ -> Router.navigatePath(xs) ]

    static member inline navigatePath(segment: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment, queryString) ]

    static member inline navigatePath(segment: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment, queryString, mode) ]

    static member inline navigatePath(segment: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment, queryString) ]

    static member inline navigatePath(segment: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2,string  segment3, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:int, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: string, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, segment6, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, segment6, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, segment6, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: string, segment5: string, segment6: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, segment6, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:int, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: int, segment5: string, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * string) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString) ]

    static member inline navigatePath(segment1: string, segment2: int, segment3:string, segment4: string, segment5, queryString: (string * int) list, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, segment3, segment4, segment5, queryString, mode) ]

    static member inline navigatePath(fullPath: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(fullPath) ]

    static member inline navigatePath(fullPath: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(fullPath, mode) ]

    static member inline navigatePath(segment: string, value: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment, value) ]

    static member inline navigatePath(segment: string, value: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment, value, mode) ]

    static member inline navigatePath(segment1: string, value1: int, value2: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, value2) ]

    static member inline navigatePath(segment1: string, value1: int, value2: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, value2, mode) ]

    static member inline navigatePath(segment1: string, segment2: string, value1: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, value1) ]

    static member inline navigatePath(segment1: string, segment2: string, value1: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, segment2, value1, mode) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, mode) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, mode) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, segment3) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, segment3, mode) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, segment3, segment4) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, segment4: string, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, segment3, segment4, mode) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, segment3, value3) ]

    static member inline navigatePath(segment1: string, value1: int, segment2: string, value2: int, segment3: string, value3: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, segment2, value2, segment3, value3, mode) ]

    static member inline navigatePath(segment1: string, value1: int, value2: int, value3: int) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, value2, value3) ]

    static member inline navigatePath(segment1: string, value1: int, value2: int, value3: int, mode: HistoryMode) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, value2, value3, mode) ]

    static member inline navigatePath(segment1: string, value1: int, value2: int, segment2: string) : Cmd<'Msg> =
        [ fun _ -> Router.navigatePath(segment1, value1, value2, segment2) ]

    static member inline navigatePath(segment1: string, value1: int, value2: int, segment2: string, mode: HistoryMode) : Cmd<'Msg> =
            [ fun _ -> Router.navigatePath(segment1, value1, value2, segment2, mode) ]

module Route =
    let (|Int|_|) (input: string) =
        match Int32.TryParse input with
        | true, value -> Some value
        | _ -> None

    let (|Int64|_|) (input: string) =
        match Int64.TryParse input with
        | true, value -> Some value
        | _ -> None

    let (|Guid|_|) (input: string) =
        match Guid.TryParse input with
        | true, value -> Some value
        | _ -> None

    let (|Number|_|) (input: string) =
        match Double.TryParse input with
        | true, value -> Some value
        | _ -> None

    let (|Decimal|_|) (input: string) =
        match Decimal.TryParse input with
        | true, value -> Some value
        | _ -> None

    let (|Bool|_|) (input: string) =
        match input.ToLower() with
        | ("1"|"true")  -> Some true
        | ("0"|"false") -> Some false
        | "" -> Some true
        | _ -> None

    /// Used to parse the query string parameter of the route.
    ///
    /// For example to match against
    ///
    /// `/users?id={value}`
    ///
    /// You can pattern match:
    ///
    /// `[ "users"; Route.Query [ "id", value ] ] -> value`
    ///
    /// When `{value}` is an integer then you can pattern match:
    ///
    /// `[ "users"; Route.Query [ "id", Route.Int userId ] ] -> userId`
    let (|Query|_|) (input: string) =
        try
            let urlParams = Router.createUrlSearchParams input
            Some [ for entry in urlParams.entries() -> entry.[0], entry.[1] ]
        with | _ -> None
