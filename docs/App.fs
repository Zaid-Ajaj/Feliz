module App

open Elmish
open Elmish.React
open Feliz
open Feliz.Recharts
open Feliz.Markdown
open Feliz.PigeonMaps
open Feliz.Router
open Fable.Core
open Fable.Core.JsInterop
open Fable.SimpleHttp
open Fable.Core.Experimental
open Zanaptak.TypedCssClasses
open System.Collections.Generic
open System

type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>
type FA = CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>

type Highlight =
    static member inline highlight (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-highlight", createObj !!properties)

type State = {
    CurrentPath : string list
}

let init() = { CurrentPath = [ ] }, Cmd.none

type Msg =
    | Increment
    | Decrement
    | UrlChanged of string list

let update msg state =
    match msg with
    | UrlChanged segments -> { state with CurrentPath = segments }, Cmd.none
    | _ -> state, Cmd.none

[
    style.display.flex
    style.display.none
    style.fontSize 20
    style.borderRadius 15
    style.textAlign.center
    style.alignContent.flexStart
    style.textDecorationColor.blue
    style.visibility.hidden
    style.textDecoration.lineThrough
    style.position.sticky
    style.borderBottomWidth 20
    style.borderBottomWidth (length.em 10)
    style.borderBottomColor color.red
    style.borderBottomStyle borderStyle.dashed
    style.borderStyle.dotted
    style.margin(length.em 1, length.em 0)
    style.marginBottom 10
    style.marginBottom (length.em 1)
    style.boxShadow(10, 10, color.black)
    style.boxShadow(10, 10, 10, color.black)
    style.boxShadow(0, 0, 10, color.black)
    style.boxShadow(0, 0, 10, 10, color.darkGray)
    style.boxShadow.none
    style.height length.auto
    style.borderRadius 20
    style.borderRadius (length.rem 10)
    style.margin 10
    style.backgroundRepeat.repeatX
    style.backgroundPosition.fixedNoScroll
    style.color.blue
    style.cursor.pointer
    style.margin (length.px 10)
    style.margin(10, 10, 10, 20)
    style.margin(10, 10, 10)
    style.margin(10, 10)
    style.margin.auto
    style.borderCollapse.collapse
    style.width 10
    style.height 100
    style.height (length.vh 50)
    style.height (length.percent 100)
    style.backgroundColor.fuchsia
    style.backgroundColor "#FFFFFF"
    style.border(3, borderStyle.dashed, color.crimson)
    style.borderColor.blue
    style.fontFamily font.aharoni
    style.transform.scale3D(20, 20, 20)
    style.transform.translateX(100)
    style.transform.translateY(100)
    style.transform.translateZ(100)
    style.transform [ transform.scale(0.5) ]
    style.textTransform.capitalize
    style.textTransform.lowercase
    style.fontStretch.extraCondensed
    style.fontVariant.smallCaps
    style.fontStyle.italic
    style.fontSize 20
    style.fontSize (length.em 2)
    style.color.crimson
    style.color "#000000"
]
|> List.iter (fun x -> Browser.Dom.console.log(createObj [!!x]))

let emptyStyles state dispatch =
    Html.div [
        prop.style [ true, [ style.color.red ] ]
        prop.children [
            Html.h1 [
                prop.style [ style.color.red ]
                prop.children [
                    Html.span [
                        prop.style [ ]
                        prop.text "Hello"
                    ]
                ]
            ]
        ]
    ]

let keyedFragments state dispatch =
    React.keyedFragment(1, [
        Html.div [
            React.keyedFragment("hello", [
                Html.h1 "Hello"
                Html.div [ ]
                Html.div [ Html.h1 "More stuff" ]
                Html.div [ Html.h2 [ Html.strong "Bold" ] ]
                Html.ul [
                    Html.li [ Html.strong "Wow" ]
                    Html.li "So"
                    Html.li (Html.em "Lightweight")
                    Html.ol [
                        Html.li [ Html.strong "More" ]
                        Html.li [ ]
                        Html.li [ Html.none ]
                    ]
                ]
            ])
        ]
    ])

let inputs state dispatch =
    Html.form [
        Html.input [ prop.type'.checkbox ]
        Html.input [ prop.type' "password" ]
    ]

let staticHtml = React.functionComponent(fun () ->
    let html = Html.div [
        prop.style [ style.padding 20 ]
        prop.children [
            Html.h1 "Html content"
            Html.br [ ]
        ]
    ]


    Html.pre [
        Html.text (ReactDOMServer.renderToString html)
    ])


let staticMarkup = React.functionComponent(fun () ->
    let html = Html.div [
        prop.style [ style.padding 20 ]
        prop.children [
            Html.h1 "Html content"
            Html.br [ ]
        ]
    ]


    Html.pre [
        Html.text (ReactDOMServer.renderToStaticMarkup html)
    ])

let multipleStateVariables = React.functionComponent(fun () ->
    let (count, setCount) = React.useState(0)
    let (textColor, setTextColor) = React.useState(color.red)

    Html.div [
        Html.h1 [
            prop.style [ style.color textColor ]
            prop.text count
        ]

        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]

        Html.button [
            prop.text "Red"
            prop.onClick (fun _ -> setTextColor(color.red))
        ]

        Html.button [
            prop.text "Blue"
            prop.onClick (fun _ -> setTextColor(color.blue))
        ]
    ])

let asyncEffect = React.functionComponent(fun () ->
    let (isLoading, setLoading) = React.useState(false)
    let (content, setContent) = React.useState("")

    let loadData() = async {
        setLoading true
        do! Async.Sleep 1500
        setLoading false
        setContent "Content"
    }

    React.useEffect(loadData >> Async.StartImmediate, [||])

    Html.div [
        if isLoading
        then Html.h1 "Loading"
        else Html.h1 content
    ])

let asyncEffectOnce = React.functionComponent(fun () ->
    let (isLoading, setLoading) = React.useState(false)
    let (content, setContent) = React.useState("")

    let loadData() = async {
        setLoading true
        do! Async.Sleep 1500
        setLoading false
        setContent "Content"
    }

    React.useEffectOnce(loadData >> Async.StartImmediate)

    Html.div [
        if isLoading
        then Html.h1 "Loading"
        else Html.h1 content
    ])

[<Emit("setTimeout($0, $1)")>]
let setTimeout (f: unit -> unit) (timeout: int) : int = jsNative

[<Emit("clearTimeout($0)")>]
let clearTimeout (id: int) : unit = jsNative

let timer = React.functionComponent(fun () ->
    let (paused, setPaused) = React.useState(false)
    let (value, setValue) = React.useState(0)

    let subscribeToTimer() =
        // start the ticking
        let subscriptionId = setTimeout (fun _ -> if not paused then setValue (value + 1)) 1000
        // return IDisposable with cleanup code
        { new IDisposable with member this.Dispose() = clearTimeout(subscriptionId) }

    React.useEffect(subscribeToTimer)

    Html.div [
        Html.h1 value

        Html.button [
            prop.className [
                Bulma.Button
                Bulma.IsLarge
                if paused then Bulma.IsPrimary else Bulma.IsWarning
            ]

            prop.onClick (fun _ -> setPaused(not paused))
            prop.text (if paused then "Resume" else "Pause")
        ]
    ])

let delayedComponent = React.functionComponent (fun (props: {| load: unit -> ReactElement |}) ->
    let (started, setStarted) = React.useState(false)
    Html.div [
        if not started then Html.button [
            prop.className [ Bulma.Button; Bulma.IsPrimary; Bulma.IsLarge ]
            prop.onClick (fun _ -> setStarted(true))
            prop.children [
                Html.i [ prop.className [ "fa"; "fa-rocket" ] ]
                Html.span [
                    prop.style [ style.marginLeft 10 ]
                    prop.text "Run Sample"
                ]
            ]
        ]

        if started then props.load()
    ])


let rnd = System.Random()
let effectfulUserId = React.functionComponent(fun () ->
    let (isLoading, setLoading) = React.useState(false)
    let (content, setContent) = React.useState("")
    let (userId, setUserId) = React.useState(0)
    let (textColor, setTextColor) = React.useState(color.red)

    let loadData() = async {
        setLoading true
        do! Async.Sleep 1500
        setLoading false
        setContent (sprintf "User %d" userId)
    }

    React.useEffect(loadData >> Async.StartImmediate, [| box userId |])

    Html.div [
        Html.h1 [
            prop.style [ style.color textColor ]
            prop.text (if isLoading then "Loading" else content)
        ]

        Html.button [
            prop.text "Red"
            prop.onClick (fun _ -> setTextColor(color.red))
        ]

        Html.button [
            prop.text "Blue"
            prop.onClick (fun _ -> setTextColor(color.blue))
        ]

        Html.button [
            prop.text "Update User ID"
            prop.onClick (fun _ -> setUserId(rnd.Next(1, 100)))
        ]
    ])

let effectfulTabCounter = React.functionComponent(fun () ->
    let (count, setCount) = React.useState(0)

    // execute this effect on every render cycle
    React.useEffect(fun () -> Browser.Dom.document.title <- sprintf "Count = %d" count)

    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ])

let counterApp state dispatch =
    Html.div [
        prop.id "main"
        prop.style [ style.padding 20 ]
        prop.children [

            Html.button [
                prop.style [ style.marginRight 5 ]
                prop.onClick (fun _ -> dispatch Increment)
                prop.text "Increment"
            ]

            Html.button [
                prop.style [ style.marginLeft 5 ]
                prop.onClick (fun _ -> dispatch Decrement)
                prop.text "Decrement"
            ]

            Html.h1 "Count"
        ]
    ]

let keyWarnings state dispatch =
    Html.div [
        prop.id "id"
        prop.className ["class"]
        prop.children [
            Html.div "text"
            Html.div [
                prop.id "id"
                prop.className [ true,"class"; false, "other"; true, "conditional" ]
                prop.children [
                    Html.text "text"
                    Html.div [
                        prop.id "id"
                        prop.className "class"
                        prop.children [
                            Html.text "text"
                            Html.div [
                                prop.id "id"
                                prop.className "class"
                                prop.children [
                                    Html.text "text"
                                    Html.div []
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

let fragmentTests =
    Html.div [
        prop.children [
            React.fragment [
                Html.h1 "One"
                Html.h2 "Two"
            ]
        ]
    ]

// Re-use state internally using React hooks
let animationsOnHover' = React.functionComponent(fun (props: {| content: ReactElement |}) -> [
    let (hovered, setHovered) = React.useState(false)
    Html.div [
        prop.style [
            style.padding 10
            style.transitionDuration (TimeSpan.FromMilliseconds 1000.0)
            style.transitionProperty [
                transitionProperty.backgroundColor
                transitionProperty.color
            ]

            if hovered then
               style.backgroundColor.lightBlue
               style.color.black
            else
               style.backgroundColor.limeGreen
               style.color.white
        ]
        prop.onMouseEnter (fun _ -> setHovered(true))
        prop.onMouseLeave (fun _ -> setHovered(false))
        prop.children [ props.content ]
    ]
])

let animationsOnHover content = animationsOnHover' {| content = React.fragment content |}
let animationSample =
    Html.div [
        animationsOnHover [ Html.span "Hover me!" ]
        animationsOnHover [ Html.p "So smooth" ]
    ]

module ReactComponents =
    type Greeting = { Name: string option }
    let greeting = React.functionComponent(fun (props: Greeting) ->
        Html.div [
            Html.span "Hello, "
            Html.span (Option.defaultValue "World" props.Name)
        ])

    let simple = Html.div [
        prop.className "content"
        prop.children [
            greeting { Name = Some "John" }
            greeting { Name = None }
        ]
    ]

    let counter = React.functionComponent(fun () ->
        let (count, setCount) = React.useState(0)
        Html.div [
            Html.h1 count
            Html.button [
                prop.text "Increment"
                prop.onClick (fun _ -> setCount(count + 1))
            ]
        ])

let counter = React.functionComponent(fun () ->
    let (count, setCount) = React.useState 0

    let modifyDocumentTitle() =
        Browser.Dom.window.document.title <- string count

    React.useEffect(modifyDocumentTitle)

    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ])

[<Emit("setInterval($0, $1)")>]
let setInterval (f: unit -> unit) (n: int) : int = jsNative

[<Emit("clearInterval($0)")>]
let clearInterval (n: int) : unit = jsNative

let ticker = React.functionComponent("Ticker", fun (input: {| start: int |}) ->
    let (tick, setTick) = React.useState input.start

    let tickerEffect() : IDisposable =
        let interval = setInterval (fun () -> setTick(tick + 1)) 1000
        React.createDisposable(fun () -> clearInterval(interval))

    React.useEffect(tickerEffect, [| box input.start |])

    Html.div [
        Html.h1 tick
    ])

let hooksAreAwesome =
    React.fragment [
        counter()
        Html.hr [ ]
        ticker {| start = 0 |}
        Html.hr [ ]
        ticker {| start = 10 |}
    ]

module ElmishCounter =
    type State = { Count : int }
    type Msg = Increment | Decrement

    let initialState = { Count = 0 }

    let update (state: State) = function
        | Increment -> { state with Count = state.Count + 1 }
        | Decrement -> { state with Count = state.Count - 1 }

    let app = React.functionComponent("Counter", fun () ->
        let (state, dispatch) = React.useReducer(update, initialState)
        Html.div [
            Html.button [
                prop.onClick (fun _ -> dispatch Increment)
                prop.text "Increment"
            ]

            Html.button [
                prop.onClick (fun _ -> dispatch Decrement)
                prop.text "Decrement"
            ]

            Html.h1 state.Count
        ])

let samples = [
    "feliz-elmish-counter", ElmishCounter.app()
    "simple-components", ReactComponents.simple
    "multiple-state-variables", multipleStateVariables()
    "hover-animations", animationSample
    "stateful-counter", ReactComponents.counter()
    "effectful-tab-counter", delayedComponent {| load = effectfulTabCounter |}
    "effectful-async", delayedComponent {| load = asyncEffect |}
    "effectful-async-once", delayedComponent {| load = asyncEffectOnce |}
    "effectful-user-id", delayedComponent {| load = effectfulUserId |}
    "effectful-timer", delayedComponent {| load = timer |}
    "static-html", staticHtml()
    "static-markup", staticMarkup()
    "recharts-main", Samples.Recharts.AreaCharts.Main.chart()
    "recharts-area-simpleareachart", Samples.Recharts.AreaCharts.SimpleAreaChart.chart()
    "recharts-area-stackedareachart", Samples.Recharts.AreaCharts.StackedAreaChart.chart()
    "recharts-area-tinyareachart", Samples.Recharts.AreaCharts.TinyAreaChart.chart()
    "recharts-area-synchronized", Samples.Recharts.AreaCharts.SynchronizedAreaChart.chart()
    "recharts-area-fillbyvalue", Samples.Recharts.AreaCharts.AreaChartFillByValue.chart()
    "recharts-line-simplelinechart", Samples.Recharts.LineCharts.SimpleLineChart.chart()
    "recharts-line-responsivefullwidth", Samples.Recharts.LineCharts.ResponsiveFullWidth.chart()
    "recharts-area-responsefullwidth", Samples.Recharts.AreaCharts.ResponsiveFullWidth.chart()
    "recharts-bar-simplebarchart", Samples.Recharts.BarCharts.SimpleBarChart.chart()
    "recharts-bar-stackedbarchart", Samples.Recharts.BarCharts.StackedBarChart.chart()
    "recharts-bar-mixbarchart", Samples.Recharts.BarCharts.MixBarChart.chart()
    "recharts-bar-tinybarchart", Samples.Recharts.BarCharts.TinyBarChart.chart()
    "recharts-line-customizedlabellinechart", Samples.Recharts.LineCharts.CustomizedLabelLineChart.chart()
    "recharts-bar-positiveandnagative", Samples.Recharts.BarCharts.PostiveAndNegative.chart()
    "recharts-line-biaxial", Samples.Recharts.LineCharts.BiaxialLineChart.chart()
    "recharts-pie-twolevel", Samples.Recharts.PieCharts.TwoLevelPieChart.chart()
    "recharts-pie-straightangle", Samples.Recharts.PieCharts.StraightAngle.chart()
    "recharts-pie-customizedlabelpiechart", Samples.Recharts.PieCharts.CustomizedLabelPieChart.chart()
    "pigeonmaps-map-basic", Samples.PigeonMaps.Main.pigeonMap
    "pigeonmaps-map-cities", Samples.PigeonMaps.DynamicMarkers.citiesMap()
    "pigeonmaps-map-popover-hover", Samples.PigeonMaps.MarkerOverlaysOnHover.citiesMap()
    "pigeonmaps-map-popover", Samples.PigeonMaps.MarkerOverlays.citiesMap()
    "pigoenmaps-map-closebutton", Samples.PigeonMaps.MarkerWithCloseButton.citiesMap()
    "popover-basic-sample", Samples.Popover.Basic.sample
    "elmish-components-counter", Samples.ElmishComponents.application
]

let githubPath (rawPath: string) =
    let parts = rawPath.Split('/')
    if parts.Length > 5
    then sprintf "http://www.github.com/%s/%s" parts.[3] parts.[4]
    else rawPath

let centeredSpinner =
    Html.div [
        prop.style [
            style.textAlign.center
            style.marginLeft length.auto
            style.marginRight length.auto
            style.marginTop 50
        ]
        prop.children [
            Html.li [
                prop.className [
                    FA.Fa
                    FA.FaRefresh
                    FA.FaSpin
                    FA.Fa3X
                ]
            ]
        ]
    ]

/// Renders a code block from markdown using react-highlight.
/// Injects sample React components when the code block has language of the format <language>:<sample-name>
let codeBlockRenderer (codeProps: Markdown.ICodeProperties) =
    if codeProps.language <> null && codeProps.language.Contains ":" then
        let languageParts = codeProps.language.Split(':')
        let sampleName = languageParts.[1]
        let sampleApp =
            samples
            |> List.tryFind (fun (name, _) -> name = sampleName)
            |> Option.map snd
            |> Option.defaultValue (Html.h1 [
                prop.style [ style.color.crimson ];
                prop.text (sprintf "Could not find sample app '%s'" sampleName)
            ])
        Html.div [
            sampleApp
            Highlight.highlight [
                prop.className "fsharp"
                prop.text(codeProps.value)
            ]
        ]
    else
        Highlight.highlight [
            prop.className "fsharp"
            prop.text(codeProps.value)
        ]

let renderMarkdown (path: string) (content: string) =
    Html.div [
        prop.className Bulma.Content
        prop.style [ style.padding 20 ]
        prop.children [
            if path.StartsWith "https://raw.githubusercontent.com" then
                Html.h2 [
                    Html.i [ prop.className [ FA.Fa; FA.FaGithub ] ]
                    Html.anchor [
                        prop.style [ style.marginLeft 10; style.color.lightGreen ]
                        prop.href (githubPath path)
                        prop.text "View on Github"
                    ]
                ]

            Markdown.markdown [
                markdown.source content
                markdown.escapeHtml false
                markdown.renderers [
                    markdown.renderers.code codeBlockRenderer
                ]
            ]
        ]
    ]

module MarkdownLoader =

    open Feliz.ElmishComponents

    type State =
        | Initial
        | Loading
        | Errored of string
        | LoadedMarkdown of content: string

    type Msg =
        | StartLoading of path: string list
        | Loaded of Result<string, int * string>

    let init (path: string list) = Initial, Cmd.ofMsg (StartLoading path)

    let resolvePath = function
    | [ one: string ] when one.StartsWith "http" -> one
    | segments -> String.concat "/" segments
    let update (msg: Msg) (state: State) =
        match msg with
        | StartLoading path ->
            let loadMarkdownAsync() = async {
                let! (statusCode, responseText) = Http.get (resolvePath path)
                if statusCode = 200
                then return Loaded (Ok responseText)
                else return Loaded (Error (statusCode, responseText))
            }

            Loading, Cmd.OfAsync.perform loadMarkdownAsync () id

        | Loaded (Ok content) ->
            State.LoadedMarkdown content, Cmd.none

        | Loaded (Error (status, error)) ->
            State.LoadedMarkdown (sprintf "Status %d: could not load markdown" status), Cmd.none

    let render path (state: State) dispatch  =
        match state with
        | Initial -> Html.none
        | Loading -> centeredSpinner
        | LoadedMarkdown content -> renderMarkdown (resolvePath path) content
        | Errored error ->
            Html.h1 [
                prop.style [ style.color.crimson ]
                prop.text error
            ]

    let loadMarkdown' (path: string list) =
        React.elmishComponent("LoadMarkdown", init path, update, render path, key=resolvePath path)

let loadMarkdown (path: string list) = MarkdownLoader.loadMarkdown' path
// A collapsable nested menu for the sidebar
// keeps internal state on whether the items should be visible or not based on the collapsed state
let nestedMenuList' = React.functionComponent(fun (input: {| name: string; items: Fable.React.ReactElement list |}) ->
    let (collapsed, setCollapsed) = React.useState(false)
    Html.li [
        Html.anchor [
            prop.onClick (fun _ -> setCollapsed(not collapsed))
            prop.children [
                Html.i [
                    prop.style [ style.marginRight 10 ]
                    prop.className [
                        FA.Fa
                        if not collapsed then FA.FaAngleUp
                        if collapsed then FA.FaAngleDown
                    ]
                ]
                Html.span input.name
            ]
        ]

        Html.ul [
            prop.className Bulma.MenuList
            prop.style [ if collapsed then style.display.none ]
            prop.children input.items
        ]
    ])

let nestedMenuList (name: string) (items: Fable.React.ReactElement list) =
    nestedMenuList' {| name = name; items = items |}

let sidebar (state: State) dispatch =
    // top level label
    let menuLabel (content: string) =
        Html.p [
            prop.className Bulma.MenuLabel
            prop.text content
        ]

    // top level menu
    let menuList (items: Fable.React.ReactElement list) =
        Html.ul [
            prop.className Bulma.MenuList
            prop.children items
        ]

    let menuItem (name: string) (path: string list) =
        Html.li [
            Html.anchor [
                prop.className [
                    state.CurrentPath = path, Bulma.IsActive
                    state.CurrentPath = path, Bulma.HasBackgroundPrimary
                ]
                prop.text name
                prop.href (sprintf "#/%s" (String.concat "/" path))
            ]
        ]

    // the actual nav bar
    Html.aside [
        prop.className Bulma.Menu
        prop.children [
            menuLabel "Feliz"
            menuList [
                menuItem "Overview" [ ]
                menuItem "Installation" [ Urls.Feliz; Urls.Installation ]
                menuItem "Project Template" [ Urls.Feliz; Urls.ProjectTemplate ]
                menuItem "Syntax" [ Urls.Feliz; Urls.Syntax ]
                menuItem "React API Support" [ Urls.Feliz; Urls.ReactApiSupport ]
                menuItem "Type-Safe Styling" [ Urls.Feliz; Urls.TypeSafeStyling ]
                menuItem "Type-Safe CSS" [ Urls.Feliz; Urls.TypeSafeCss ]
                menuItem "Use with Elmish" [ Urls.Feliz; Urls.UseWithElmish ]
                menuItem "Elmish Components" [ Urls.Feliz; Urls.ElmishComponents ]
                menuItem "Contributing" [ Urls.Feliz; Urls.Contributing ]
                menuItem "Aliasing props" [ Urls.Feliz; Urls.Aliasing ]
                nestedMenuList "React Components" [
                    menuItem "Stateless Components" [ Urls.Feliz; Urls.React; Urls.StatelessComponents ]
                    menuItem "Not Just Functions" [ Urls.Feliz; Urls.React; Urls.NotJustFunctions ]
                    menuItem "Stateful Components" [ Urls.Feliz; Urls.React; Urls.StatefulComponents ]
                    menuItem "Components with Effects" [ Urls.Feliz; Urls.React; Urls.EffectfulComponents ]
                    menuItem "Subscriptions with Effects" [ Urls.Feliz; Urls.React; Urls.SubscriptionsWithEffects ]
                    menuItem "Context Propagation" [ Urls.Feliz; Urls.React; Urls.ContextPropagation ]
                    menuItem "Hover Animations" [ Urls.Feliz; Urls.React; Urls.HoverAnimations ]
                    menuItem "Common Pitfalls" [ Urls.Feliz; Urls.React; Urls.CommonPitfalls ]
                    menuItem "Render Static Html" [ Urls.Feliz; Urls.React; Urls.RenderStaticHtml ]
                ]

                nestedMenuList "Ecosystem" [
                    menuItem "Feliz.ElmishComponents" [ Urls.Ecosystem; Urls.ElmishComponents ]
                    menuItem "Feliz.Router" [ Urls.Ecosystem; Urls.Router ]
                    menuItem "Feliz.Recharts" [ Urls.Ecosystem; Urls.Recharts ]
                    menuItem "Feliz.PigeonMaps" [ Urls.Ecosystem; Urls.PigeonMaps ]
                    menuItem "Feliz.MaterialUI" [ Urls.Ecosystem; Urls.Mui ]
                    menuItem "Feliz.Plotly" [ Urls.Ecosystem; Urls.Plotly ]
                    menuItem "Feliz.Bulma" [ Urls.Ecosystem; Urls.Bulma ]
                    menuItem "Feliz.Popover" [ Urls.Ecosystem; Urls.Popover ]
                ]
            ]

            menuLabel "Feliz.PigeonMaps"
            menuList [
                menuItem "Overview" [ Urls.PigeonMaps; Urls.Overview ]
                menuItem "Installation" [ Urls.PigeonMaps; Urls.Installation ]
            ]
            menuLabel "Feliz.Recharts"
            menuList [
                menuItem "Overview" [ Urls.Recharts; Urls.Overview ]
                menuItem "Installation" [ Urls.Recharts; Urls.Installation ]

                nestedMenuList "Line Charts" [
                    menuItem "Simple Line Chart" [ Urls.Recharts; Urls.LineCharts; Urls.SimpleLineChart ]
                    menuItem "Responsive Full Width" [ Urls.Recharts; Urls.LineCharts; Urls.ResponsiveFullWidth ]
                    menuItem "Customized Label" [ Urls.Recharts; Urls.LineCharts; Urls.CustomizedLabelLineChart ]
                    menuItem "Biaxial Line Chart" [ Urls.Recharts; Urls.LineCharts; Urls.BiaxialLineChart ]
                ]

                nestedMenuList "Bar Charts" [
                    menuItem "Simple Bar Chart" [ Urls.Recharts; Urls.BarCharts; Urls.SimpleBarChart ]
                    menuItem "Tiny Bar Chart" [ Urls.Recharts; Urls.BarCharts; Urls.TinyBarChart ]
                    menuItem "Stacked Bar Chart" [ Urls.Recharts; Urls.BarCharts; Urls.StackedBarChart ]
                    menuItem "Mix Bar Chart" [ Urls.Recharts; Urls.BarCharts; Urls.MixBarChart ]
                    menuItem "Positive And Negative" [ Urls.Recharts; Urls.BarCharts; Urls.PositiveAndNegative ]
                ]

                nestedMenuList "Area Charts" [
                    menuItem "Simple Area Chart" [ Urls.Recharts; Urls.AreaCharts; Urls.SimpleAreaChart ]
                    menuItem "Stacked Area Chart" [ Urls.Recharts; Urls.AreaCharts; Urls.StackedAreaChart ]
                    menuItem "Tiny Area Chart" [ Urls.Recharts; Urls.AreaCharts; Urls.TinyAreaChart ]
                    menuItem "Responsive Full Width" [ Urls.Recharts; Urls.AreaCharts; Urls.ResponsiveFullWidth ]
                    menuItem "Synchronized Charts" [ Urls.Recharts; Urls.AreaCharts; Urls.SynchronizedAreaChart ]
                    menuItem "AreaChartFillByValue" [ Urls.Recharts; Urls.AreaCharts; Urls.AreaChartFillByValue ]
                ]

                nestedMenuList "Pie Charts" [
                    menuItem "Two Level Pie Chart" [ Urls.Recharts; Urls.PieCharts; Urls.TwoLevelPieChart ]
                    menuItem "Straight Angle Pie Chart" [ Urls.Recharts; Urls.PieCharts; Urls.StraightAngle ]
                    menuItem "Customized Label Pie Chart" [ Urls.Recharts; Urls.PieCharts; Urls.CustomizedLabelPieChart ]
                ]
            ]
        ]
    ]

let readme = sprintf "https://raw.githubusercontent.com/%s/%s/master/README.md"

let content state dispatch =
    match state.CurrentPath with
    | [ ] -> loadMarkdown [ "Feliz"; "README.md" ]
    | [ Urls.Feliz; Urls.Overview; ] -> loadMarkdown [ "Feliz"; "README.md" ]
    | [ Urls.Feliz; Urls.ProjectTemplate ] -> loadMarkdown [ "Feliz"; "ProjectTemplate.md" ]
    | [ Urls.Feliz; Urls.Installation ] -> loadMarkdown [ "Feliz"; "Installation.md" ]
    | [ Urls.Feliz; Urls.UseWithElmish ] -> loadMarkdown [ "Feliz"; "UseWithElmish.md" ]
    | [ Urls.Feliz; Urls.ElmishComponents ] -> loadMarkdown [ "Feliz"; "ElmishComponents.md" ]
    | [ Urls.Feliz; Urls.Contributing ] -> loadMarkdown [ "Feliz"; "Contributing.md" ]
    | [ Urls.Feliz; Urls.Syntax ] -> loadMarkdown [ "Feliz"; "Syntax.md" ]
    | [ Urls.Feliz; Urls.ReactApiSupport ] -> loadMarkdown [ "Feliz"; "ReactApiSupport.md   " ]
    | [ Urls.Feliz; Urls.TypeSafeStyling ] -> loadMarkdown [ "Feliz"; "TypeSafeStyling.md" ]
    | [ Urls.Feliz; Urls.TypeSafeCss ] -> loadMarkdown [ "Feliz"; "TypeSafeCss.md" ]
    | [ Urls.Feliz; Urls.Aliasing ] -> loadMarkdown [ "Feliz"; "AliasingProp.md" ]
    | [ Urls.Feliz; Urls.ConditionalStyling ] -> loadMarkdown [ "Feliz"; "ConditionalStyling.md" ]
    | [ Urls.Feliz; Urls.React; Urls.Standalone ] -> loadMarkdown [ "Feliz"; "React"; "Standalone.md" ]
    | [ Urls.Feliz; Urls.React; Urls.StatelessComponents ] -> loadMarkdown  [ "Feliz"; "React"; "StatelessComponents.md" ]
    | [ Urls.Feliz; Urls.React; Urls.NotJustFunctions ] -> loadMarkdown [ "Feliz"; "React"; "NotJustFunctions.md" ]
    | [ Urls.Feliz; Urls.React; Urls.StatefulComponents ] -> loadMarkdown [ "Feliz"; "React"; "StatefulComponents.md" ]
    | [ Urls.Feliz; Urls.React; Urls.EffectfulComponents ] -> loadMarkdown [ "Feliz"; "React"; "EffectfulComponents.md" ]
    | [ Urls.Feliz; Urls.React; Urls.SubscriptionsWithEffects ] -> loadMarkdown [ "Feliz"; "React"; "SubscriptionsWithEffects.md" ]
    | [ Urls.Feliz; Urls.React; Urls.ContextPropagation ] -> loadMarkdown [ "Feliz"; "React"; "ContextPropagation.md" ]
    | [ Urls.Feliz; Urls.React; Urls.HoverAnimations ] -> loadMarkdown [ "Feliz"; "React"; "HoverAnimations.md" ]
    | [ Urls.Feliz; Urls.React; Urls.Components ] -> loadMarkdown [ "Feliz"; "React"; "Components.md" ]
    | [ Urls.Feliz; Urls.React; Urls.CommonPitfalls ] -> loadMarkdown [ "Feliz"; "React"; "CommonPitfalls.md" ]
    | [ Urls.Feliz; Urls.React; Urls.RenderStaticHtml ] -> loadMarkdown [ "Feliz"; "React"; "RenderStaticHtml.md" ]
    | [ Urls.Ecosystem; Urls.ElmishComponents ] -> loadMarkdown [ "Feliz"; "ElmishComponents.md" ]
    | [ Urls.Ecosystem; Urls.Router ] -> loadMarkdown [ readme "Zaid-Ajaj" "Feliz.Router" ]
    | [ Urls.Ecosystem; Urls.Mui ] -> loadMarkdown [ readme "cmeeren" "Feliz.MaterialUI" ]
    | [ Urls.Ecosystem; Urls.Plotly ] -> loadMarkdown [ readme "Shmew" "Feliz.Plotly" ]
    | [ Urls.Ecosystem; Urls.Recharts ] -> loadMarkdown [ "Recharts"; "README.md" ]
    | [ Urls.Ecosystem; Urls.PigeonMaps ] -> loadMarkdown [ "PigeonMaps"; "README.md" ]
    | [ Urls.Ecosystem; Urls.Popover ] -> loadMarkdown [ "Popover"; "README.md" ]
    | [ Urls.Ecosystem; Urls.Bulma ] -> loadMarkdown [ readme "Dzoukr" "Feliz.Bulma" ]
    | [ Urls.Recharts; Urls.Overview ] -> loadMarkdown [ "Recharts"; "README.md" ]
    | [ Urls.Recharts; Urls.Installation ] -> loadMarkdown [ "Recharts"; "Installation.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.SimpleAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "SimpleAreaChart.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.StackedAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "StackedAreaChart.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.TinyAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "TinyAreaChart.md" ]
    | [ Urls.Recharts; Urls.LineCharts; Urls.SimpleLineChart ] -> loadMarkdown [ "Recharts"; "LineCharts"; "SimpleLineChart.md" ]
    | [ Urls.Recharts; Urls.LineCharts; Urls.ResponsiveFullWidth ] -> loadMarkdown [ "Recharts"; "LineCharts"; "ResponsiveFullWidth.md" ]
    | [ Urls.Recharts; Urls.LineCharts; Urls.CustomizedLabelLineChart ] -> loadMarkdown [ "Recharts"; "LineCharts" ; "CustomizedLabelLineChart.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.ResponsiveFullWidth ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "ResponsiveFullWidth.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.SynchronizedAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "SynchronizedAreaChart.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.AreaChartFillByValue ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "AreaChartFillByValue.md" ]
    | [ Urls.Recharts; Urls.BarCharts; Urls.SimpleBarChart ] -> loadMarkdown [ "Recharts"; "BarCharts"; "SimpleBarChart.md" ]
    | [ Urls.Recharts; Urls.BarCharts; Urls.StackedBarChart ] -> loadMarkdown [ "Recharts"; "BarCharts"; "StackedBarChart.md" ]
    | [ Urls.Recharts; Urls.BarCharts; Urls.MixBarChart ] -> loadMarkdown [ "Recharts"; "BarCharts"; "MixBarChart.md" ]
    | [ Urls.Recharts; Urls.BarCharts; Urls.TinyBarChart ] -> loadMarkdown [ "Recharts"; "BarCharts"; "TinyBarChart.md" ]
    | [ Urls.Recharts; Urls.BarCharts; Urls.PositiveAndNegative ] -> loadMarkdown [ "Recharts" ; "BarCharts"; "PositiveAndNegative.md" ]
    | [ Urls.Recharts; Urls.LineCharts; Urls.BiaxialLineChart ] -> loadMarkdown [ "Recharts"; "LineCharts"; "BiaxialLineChart.md" ]
    | [ Urls.Recharts; Urls.PieCharts; Urls.TwoLevelPieChart ] -> loadMarkdown [ "Recharts"; "PieCharts"; "TwoLevelPieChart.md" ]
    | [ Urls.Recharts; Urls.PieCharts; Urls.StraightAngle ] -> loadMarkdown [ "Recharts"; "PieCharts"; "StraightAngle.md" ]
    | [ Urls.Recharts; Urls.PieCharts; Urls.CustomizedLabelPieChart ] -> loadMarkdown [ "Recharts"; "PieCharts"; "CustomizedLabelPieChart.md" ]
    | [ Urls.PigeonMaps; Urls.Overview ] -> loadMarkdown [ "PigeonMaps"; "README.md" ]
    | [ Urls.PigeonMaps; Urls.Installation ] -> loadMarkdown [ "PigeonMaps"; "Installation.md" ]
    | [ Urls.Tests; Urls.ElmishComponents ] -> Samples.ElmishComponents.ReplacementTests.counterSwitcher()
    | segments -> Html.div [ for segment in segments -> Html.p segment ]

let main state dispatch =
    Html.div [
        prop.className [ Bulma.Tile; Bulma.IsAncestor ]
        prop.children [
            Html.div [
                prop.className [ Bulma.Tile; Bulma.Is2 ]
                prop.children [ sidebar state dispatch ]
            ]

            Html.div [
                prop.className Bulma.Tile
                prop.children [
                   content state dispatch
                ]
            ]
        ]
    ]

let render (state: State) dispatch =
    let application =
        Html.div [
            prop.style [ style.padding 30 ]
            prop.children [ main state dispatch ]
        ]

    Router.router [
        Router.onUrlChanged (UrlChanged >> dispatch)
        Router.application application
    ]

Program.mkProgram init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run