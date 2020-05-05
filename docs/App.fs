module App

open Browser.Dom
open Elmish
open Elmish.React
open Feliz
open Feliz.Recharts
open Feliz.Markdown
open Feliz.PigeonMaps
open Feliz.Router
open Fable.Core.JsInterop
open Fable.SimpleHttp
open Zanaptak.TypedCssClasses

type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>
type FA = CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>

type Highlight =
    static member inline highlight (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-highlight", createObj !!properties)

type State = 
    { CurrentPath : string list
      CurrentTab: string list }

let init () =
    let path = 
        match document.URL.Split('#') with
        | [| _ |] -> []
        | [| _; path |] -> path.Split('/') |> List.ofArray |> List.tail
        | _ -> []
    { CurrentPath = path
      CurrentTab = path }, Cmd.none

type Msg =
    | TabToggled of string list
    | UrlChanged of string list

let update msg state =
    match msg with
    | UrlChanged segments -> 
        { state with CurrentPath = segments }, 
        match state.CurrentTab with
        | [ ] when segments.Length > 2 -> 
            segments
            |> TabToggled
            |> Cmd.ofMsg
        | _ -> Cmd.none
    | TabToggled tabs ->
        match tabs with
        | [ ] -> { state with CurrentTab = [ ] }, Cmd.none
        | _ -> { state with CurrentTab = tabs }, Cmd.none

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

let samples = [
    "feliz-elmish-counter", Examples.ElmishCounter.app()
    "simple-components", Examples.ReactComponents.simple
    "multiple-state-variables", Examples.multipleStateVariables()
    "hover-animations", Examples.animationSample
    "stateful-counter", Examples.ReactComponents.counter()
    "effectful-tab-counter", delayedComponent {| load = Examples.effectfulTabCounter |}
    "effectful-async", delayedComponent {| load = Examples.asyncEffect |}
    "effectful-async-once", delayedComponent {| load = Examples.asyncEffectOnce |}
    "effectful-user-id", delayedComponent {| load = Examples.effectfulUserId |}
    "effectful-timer", delayedComponent {| load = Examples.timer |}
    "static-html", Examples.staticHtml()
    "static-markup", Examples.staticMarkup()
    "strict-mode", delayedComponent {| load = Examples.strictModeExample |}
    "recharts-main", Samples.Recharts.AreaCharts.Main.chart()
    "recharts-area-simpleareachart", Samples.Recharts.AreaCharts.SimpleAreaChart.chart()
    "recharts-area-stackedareachart", Samples.Recharts.AreaCharts.StackedAreaChart.chart()
    "recharts-area-tinyareachart", Samples.Recharts.AreaCharts.TinyAreaChart.chart()
    "recharts-area-optionalvalues", Samples.Recharts.AreaCharts.OptionalValues.chart()
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
    "recharts-line-optionalvalues", Samples.Recharts.LineCharts.OptionalValues.chart()
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
    "pigeonmaps-map-stamenterrain", Samples.PigeonMaps.CustomProviders.pigeonMap
    "popover-basic-sample", Samples.Popover.Basic.sample
    "elmish-components-counter", Samples.ElmishComponents.application
    "elmish-components-dispose", Samples.ElmishComponents.WithDispose.application
    "focus-input-example", Examples.focusInputExample()
    "forward-ref-example", Examples.forwardRefParent()
    "code-splitting", delayedComponent {| load = Examples.codeSplitting |}
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
let codeBlockRenderer' = React.functionComponent(fun (input: {| codeProps: Markdown.ICodeProperties |}) ->
    if input.codeProps.language <> null && input.codeProps.language.Contains ":" then
        let languageParts = input.codeProps.language.Split(':')
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
                prop.text(input.codeProps.value)
            ]
        ]
    else
        Highlight.highlight [
            prop.className "fsharp"
            prop.text(input.codeProps.value)
        ])

let codeBlockRenderer (codeProps: Markdown.ICodeProperties) = codeBlockRenderer' {| codeProps = codeProps |}

let renderMarkdown = React.functionComponent(fun (input: {| path: string; content: string |}) ->
    Html.div [
        prop.className [ Bulma.Content; "scrollbar" ]
        prop.style [ 
            style.width (length.percent 100)
            style.padding (0,20)
        ]
        prop.children [
            if input.path.StartsWith "https://raw.githubusercontent.com" then
                Html.h2 [
                    Html.i [ prop.className [ FA.Fa; FA.FaGithub ] ]
                    Html.anchor [
                        prop.style [ style.marginLeft 10; style.color.lightGreen ]
                        prop.href (githubPath input.path)
                        prop.text "View on Github"
                    ]
                ]

            Markdown.markdown [
                markdown.source input.content
                markdown.escapeHtml false
                markdown.renderers [
                    markdown.renderers.code codeBlockRenderer
                ]
            ]
        ]
    ])

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

        | Loaded (Error (status, _)) ->
            State.LoadedMarkdown (sprintf "Status %d: could not load markdown" status), Cmd.none

    let render path (state: State) dispatch =
        match state with
        | Initial -> Html.none
        | Loading -> centeredSpinner
        | LoadedMarkdown content -> renderMarkdown {| path = (resolvePath path); content = content |}
        | Errored error ->
            Html.h1 [
                prop.style [ style.color.crimson ]
                prop.text error
            ]

    let load (path: string list) = React.elmishComponent("LoadMarkdown", init path, update, render path, key = resolvePath path)

// A collapsable nested menu for the sidebar
// keeps internal state on whether the items should be visible or not based on the collapsed state
let nestedMenuList' = React.functionComponent(fun (input: {| state: State; name: string; basePath: string list; elems: (string list -> Fable.React.ReactElement) list; dispatch: Msg -> unit |}) ->
    let collapsed = 
        match input.state.CurrentTab with
        | [ ] -> false
        | _ -> 
            input.basePath 
            |> List.indexed 
            |> List.forall (fun (i, segment) -> 
                List.tryItem i input.state.CurrentTab 
                |> Option.map ((=) segment) 
                |> Option.defaultValue false) 

    Html.li [
        Html.anchor [
            prop.className Bulma.IsUnselectable
            prop.onClick <| fun _ -> 
                match collapsed with
                | true -> input.dispatch <| TabToggled (input.basePath |> List.rev |> List.tail |> List.rev)
                | false -> input.dispatch <| TabToggled input.basePath
            prop.children [
                Html.i [
                    prop.style [ style.marginRight 10 ]
                    prop.className [
                        FA.Fa
                        if not collapsed then FA.FaAngleDown else FA.FaAngleUp
                    ]
                ]
                Html.span input.name
            ]
        ]

        Html.ul [
            prop.className Bulma.MenuList
            prop.style [ 
                if not collapsed then yield! [ style.display.none ] 
            ]
            prop.children (input.elems |> List.map (fun f -> f input.basePath))
        ]
    ])

// top level label
let menuLabel' = React.functionComponent (fun (input: {| content: string |}) ->
    Html.p [
        prop.className [ Bulma.MenuLabel; Bulma.IsUnselectable ]
        prop.text input.content
    ])

// top level menu
let menuList' = React.functionComponent(fun (input: {| items: Fable.React.ReactElement list |}) ->
    Html.ul [
        prop.className Bulma.MenuList
        prop.style [ style.width (length.percent 95) ]
        prop.children input.items
    ])

let menuItem' = React.functionComponent(fun (input: {| currentPath: string list; name: string; path: string list |}) ->
    Html.li [
        Html.anchor [
            prop.className [
                if input.currentPath = input.path then Bulma.IsActive
                if input.currentPath = input.path then Bulma.HasBackgroundPrimary
            ]
            prop.text input.name
            prop.href (sprintf "#/%s" (String.concat "/" input.path))
        ]
    ])

let menuLabel (content: string) =
    menuLabel' {| content = content |}

let menuList (items: Fable.React.ReactElement list) =
    menuList' {| items = items |}

let allItems = React.functionComponent(fun (input: {| state: State; dispatch: Msg -> unit |} ) ->
    let dispatch = React.useCallback(input.dispatch, [||])

    let menuItem (name: string) (basePath: string list) =
        menuItem' 
            {| currentPath = input.state.CurrentPath
               name = name
               path = basePath |}
    
    let nestedMenuItem (name: string) (extendedPath: string list) (basePath: string list) =
        let path = basePath @ extendedPath
        menuItem' 
            {| currentPath = input.state.CurrentPath
               name = name
               path = path |}

    let nestedMenuList (name: string) (basePath: string list) (items: (string list -> Fable.React.ReactElement) list) =
        nestedMenuList' 
            {| state = input.state
               name = name
               basePath = basePath
               elems = items
               dispatch = dispatch |}
    
    let subNestedMenuList (name: string) (basePath: string list) (items: (string list -> Fable.React.ReactElement) list) (addedBasePath: string list) =
        nestedMenuList' 
            {| state = input.state
               name = name
               basePath = (addedBasePath @ basePath)
               elems = items
               dispatch = dispatch |}

    Html.div [
        prop.className "scrollbar"
        prop.children [
            menuList [
                menuItem "Overview" [ ]
                menuItem "Installation" [ Urls.Feliz; Urls.Installation ]
                menuItem "Project Template" [ Urls.Feliz; Urls.ProjectTemplate ]
                menuItem "Syntax" [ Urls.Feliz; Urls.Syntax ]
                menuItem "React API Support" [ Urls.Feliz; Urls.ReactApiSupport ]
                menuItem "Type-Safe Styling" [ Urls.Feliz; Urls.TypeSafeStyling ]
                menuItem "Type-Safe CSS" [ Urls.Feliz; Urls.TypeSafeCss ]
                menuItem "Use with Elmish" [ Urls.Feliz; Urls.UseWithElmish ]
                menuItem "Contributing" [ Urls.Feliz; Urls.Contributing ]
                menuItem "Aliasing props" [ Urls.Feliz; Urls.Aliasing ]
                nestedMenuList "React Components" [ Urls.Feliz; Urls.React ] [
                    nestedMenuItem "Stateless Components" [ Urls.StatelessComponents ]
                    nestedMenuItem "Not Just Functions" [ Urls.NotJustFunctions ]
                    nestedMenuItem "Stateful Components" [ Urls.StatefulComponents ]
                    nestedMenuItem "Components with Effects" [ Urls.EffectfulComponents ]
                    nestedMenuItem "Subscriptions with Effects" [ Urls.SubscriptionsWithEffects ]
                    nestedMenuItem "Context Propagation" [ Urls.ContextPropagation ]
                    nestedMenuItem "Hover Animations" [ Urls.HoverAnimations ]
                    nestedMenuItem "Using References" [ Urls.Refs ]
                    nestedMenuItem "Common Pitfalls" [ Urls.CommonPitfalls ]
                    nestedMenuItem "Render Static Html" [ Urls.RenderStaticHtml ]
                    nestedMenuItem "Strict Mode" [ Urls.StrictMode ]
                    nestedMenuItem "Code Splitting" [ Urls.CodeSplitting ]
                ]
                nestedMenuList "Ecosystem" [ Urls.Ecosystem ] [
                    nestedMenuItem "Feliz.ElmishComponents" [ Urls.ElmishComponents ]
                    nestedMenuItem "Feliz.Router" [ Urls.Router ]
                    nestedMenuItem "Feliz.Recharts" [ Urls.Recharts ]
                    nestedMenuItem "Feliz.PigeonMaps" [ Urls.PigeonMaps ]
                    nestedMenuItem "Feliz.MaterialUI" [ Urls.Mui ]
                    nestedMenuItem "Feliz.Plotly" [ Urls.Plotly ]
                    nestedMenuItem "Feliz.Bulma" [ Urls.Bulma ]
                    nestedMenuItem "Feliz.Popover" [ Urls.Popover ]
                    nestedMenuItem "Feliz.ViewEngine" [ Urls.ViewEngine ]
                ]
                nestedMenuList "Feliz.PigeonMaps" [ Urls.PigeonMaps ] [
                    nestedMenuItem "Overview" [ Urls.Overview ]
                    nestedMenuItem "Installation" [ Urls.Installation ]
                ]
                nestedMenuList "Feliz.Recharts" [ Urls.Recharts ] [
                    nestedMenuItem "Overview" [ Urls.Overview ]
                    nestedMenuItem "Installation" [ Urls.Installation ]
                    subNestedMenuList "Line Charts" [ Urls.LineCharts ] [
                        nestedMenuItem "Simple Line Chart" [ Urls.SimpleLineChart ]
                        nestedMenuItem "Responsive Full Width" [ Urls.ResponsiveFullWidth ]
                        nestedMenuItem "Customized Label" [ Urls.CustomizedLabelLineChart ]
                        nestedMenuItem "Optional Values" [ Urls.LineChartOptionalValues ]
                        nestedMenuItem "Biaxial Line Chart" [ Urls.BiaxialLineChart ]
                    ]
                    subNestedMenuList "Bar Charts" [ Urls.BarCharts ] [
                        nestedMenuItem "Simple Bar Chart" [ Urls.SimpleBarChart ]
                        nestedMenuItem "Tiny Bar Chart" [ Urls.TinyBarChart ]
                        nestedMenuItem "Stacked Bar Chart" [ Urls.StackedBarChart ]
                        nestedMenuItem "Mix Bar Chart" [ Urls.MixBarChart ]
                        nestedMenuItem "Positive And Negative" [ Urls.PositiveAndNegative ]
                    ]
                    subNestedMenuList "Area Charts" [ Urls.AreaCharts ] [
                        nestedMenuItem "Simple Area Chart" [ Urls.SimpleAreaChart ]
                        nestedMenuItem "Stacked Area Chart" [ Urls.StackedAreaChart ]
                        nestedMenuItem "Tiny Area Chart" [ Urls.TinyAreaChart ]
                        nestedMenuItem "Responsive Full Width" [ Urls.ResponsiveFullWidth ]
                        nestedMenuItem "Optional Values" [ Urls.AreaChartOptionalValues ]
                        nestedMenuItem "Synchronized Charts" [ Urls.SynchronizedAreaChart ]
                        nestedMenuItem "AreaChartFillByValue" [ Urls.AreaChartFillByValue ]
                    ]
                    subNestedMenuList "Pie Charts" [ Urls.PieCharts ] [
                        nestedMenuItem "Two Level Pie Chart" [ Urls.TwoLevelPieChart ]
                        nestedMenuItem "Straight Angle Pie Chart" [ Urls.StraightAngle ]
                        nestedMenuItem "Customized Label Pie Chart" [ Urls.CustomizedLabelPieChart ]
                    ]
                ]
            ]
        ]
    ])

let sidebar = React.functionComponent(fun (input: {| state: State; dispatch: Msg -> unit |}) ->
    let dispatch = React.useCallback(input.dispatch, [||])

    // the actual nav bar
    Html.aside [
        prop.className Bulma.Menu
        prop.style [
            style.width (length.perc 100)
        ]
        prop.children [ 
            menuLabel "Feliz"
            allItems {| state = input.state; dispatch = dispatch |} 
        ]
    ])

let readme = sprintf "https://raw.githubusercontent.com/%s/%s/master/README.md"

let reactExamples (currentPath: string list) =
    match currentPath with
    | [ Urls.Standalone ] -> [ "Standalone.md" ]
    | [ Urls.StatelessComponents ] -> [ "StatelessComponents.md" ]
    | [ Urls.NotJustFunctions ] -> [ "NotJustFunctions.md" ]
    | [ Urls.StatefulComponents ] -> [ "StatefulComponents.md" ]
    | [ Urls.EffectfulComponents ] -> [ "EffectfulComponents.md" ]
    | [ Urls.SubscriptionsWithEffects ] -> [ "SubscriptionsWithEffects.md" ]
    | [ Urls.ContextPropagation ] -> [ "ContextPropagation.md" ]
    | [ Urls.HoverAnimations ] -> [ "HoverAnimations.md" ]
    | [ Urls.Refs ] -> [ "UsingReferences.md" ]
    | [ Urls.Components ] -> [ "Components.md" ]
    | [ Urls.CommonPitfalls ] -> [ "CommonPitfalls.md" ]
    | [ Urls.RenderStaticHtml ] -> [ "RenderStaticHtml.md" ]
    | [ Urls.StrictMode ] -> [ "StrictMode.md" ]
    | [ Urls.CodeSplitting ] -> [ "CodeSplitting.md" ]
    | _ -> []
    |> fun path -> [ Urls.React ] @ path

let rechartsExamples (currentPath: string list) =
    match currentPath with
    | [ Urls.Overview ] -> [ "README.md" ]
    | [ Urls.Installation ] -> [ "Installation.md" ]
    | Urls.AreaCharts :: rest ->
        match rest with
        | [ Urls.SimpleAreaChart ] -> [ "SimpleAreaChart.md" ]
        | [ Urls.StackedAreaChart ] -> [ "StackedAreaChart.md" ]
        | [ Urls.TinyAreaChart ] -> [ "TinyAreaChart.md" ]
        | [ Urls.ResponsiveFullWidth ] -> [ "ResponsiveFullWidth.md" ]
        | [ Urls.AreaChartOptionalValues ] -> [ "OptionalValues.md" ]
        | [ Urls.SynchronizedAreaChart ] -> [ "SynchronizedAreaChart.md" ]
        | [ Urls.AreaChartFillByValue ] -> [ "AreaChartFillByValue.md" ]
        | _ -> []
        |> List.append [ Urls.AreaCharts ]
    | Urls.LineCharts :: rest ->
        match rest with
        | [ Urls.SimpleLineChart ] -> [ "SimpleLineChart.md" ]
        | [ Urls.ResponsiveFullWidth ] -> [ "ResponsiveFullWidth.md" ]
        | [ Urls.CustomizedLabelLineChart ] -> [ "CustomizedLabelLineChart.md" ]
        | [ Urls.LineChartOptionalValues ] -> [ "OptionalValues.md" ]
        | [ Urls.BiaxialLineChart ] -> [ "BiaxialLineChart.md" ]
        | _ -> []
        |> List.append [ Urls.LineCharts ]
    | Urls.BarCharts :: rest ->
        match rest with
        | [ Urls.SimpleBarChart ] -> [ "SimpleBarChart.md" ]
        | [ Urls.StackedBarChart ] -> [ "StackedBarChart.md" ]
        | [ Urls.MixBarChart ] -> [ "MixBarChart.md" ]
        | [ Urls.TinyBarChart ] -> [ "TinyBarChart.md" ]
        | [ Urls.PositiveAndNegative ] -> [ "PositiveAndNegative.md" ]
        | _ -> []
        |> List.append [ Urls.BarCharts ]
    | Urls.PieCharts :: rest ->
        match rest with
        | [ Urls.TwoLevelPieChart ] -> [ "TwoLevelPieChart.md" ]
        | [ Urls.StraightAngle ] -> [ "StraightAngle.md" ]
        | [ Urls.CustomizedLabelPieChart ] -> [ "CustomizedLabelPieChart.md" ]
        | _ -> []
        |> List.append [ Urls.PieCharts ]
    | _ -> []
    |> fun path -> [ Urls.Recharts ] @ path

let (|PathPrefix|) (segments: string list) (path: string list) =
    if path.Length > segments.Length then
        match List.splitAt segments.Length path with
        | start,end' when start = segments -> Some end'
        | _ -> None
    else None

let loadOrSegment (origPath: string list) (basePath: string list) (path: string list) =
    if path |> List.isEmpty then
        Html.div [ for segment in origPath -> Html.p segment ]
    else basePath @ path |> lazyView MarkdownLoader.load

let content = React.functionComponent(fun (input: {| state: State; dispatch: Msg -> unit |}) ->
    let loadOrSegment = loadOrSegment input.state.CurrentPath
    
    match input.state.CurrentPath with
    | [ ] -> lazyView MarkdownLoader.load [ "Feliz"; "README.md" ]
    | PathPrefix [ Urls.Feliz ] (Some res) ->
        match res with
        | [ Urls.Overview ] -> [ "README.md" ]
        | [ Urls.ProjectTemplate ] -> [ "ProjectTemplate.md" ]
        | [ Urls.Installation ] -> [ "Installation.md" ]
        | [ Urls.UseWithElmish ] -> [ "UseWithElmish.md" ]
        | [ Urls.Contributing ] -> [ "Contributing.md" ]
        | [ Urls.Syntax ] -> [ "Syntax.md" ]
        | [ Urls.ReactApiSupport ] -> [ "ReactApiSupport.md" ]
        | [ Urls.TypeSafeStyling ] -> [ "TypeSafeStyling.md" ]
        | [ Urls.TypeSafeCss ] -> [ "TypeSafeCss.md" ]
        | [ Urls.Aliasing ] -> [ "AliasingProp.md" ]
        | [ Urls.ConditionalStyling ] -> [ "ConditionalStyling.md" ]
        | PathPrefix [ Urls.React ] (Some res) -> reactExamples res 
        | _ -> []
        |> loadOrSegment [ Urls.Feliz ]
    | PathPrefix [ Urls.Ecosystem ] (Some res) ->
        match res with
        | [ Urls.ElmishComponents ] -> lazyView MarkdownLoader.load [ "Feliz"; "ElmishComponents.md" ]
        | [ Urls.Router ] -> lazyView MarkdownLoader.load [ readme "Zaid-Ajaj" "Feliz.Router" ]
        | [ Urls.Mui ] -> lazyView MarkdownLoader.load [ readme "cmeeren" "Feliz.MaterialUI" ]
        | [ Urls.Plotly ] -> lazyView MarkdownLoader.load [ readme "Shmew" "Feliz.Plotly" ]
        | [ Urls.Recharts ] -> lazyView MarkdownLoader.load [ "Recharts"; "README.md" ]
        | [ Urls.PigeonMaps ] -> lazyView MarkdownLoader.load [ "PigeonMaps"; "README.md" ]
        | [ Urls.Popover ] -> lazyView MarkdownLoader.load [ "Popover"; "README.md" ]
        | [ Urls.Bulma ] -> lazyView MarkdownLoader.load [ readme "Dzoukr" "Feliz.Bulma" ]
        | [ Urls.ViewEngine ] -> lazyView  MarkdownLoader.load [ readme "dbrattli" "Feliz.ViewEngine" ]
        | _ -> Html.div [ for segment in input.state.CurrentPath -> Html.p segment ]
    | PathPrefix [ Urls.Recharts ] (Some res) -> rechartsExamples res |> loadOrSegment []
    | PathPrefix [ Urls.PigeonMaps ] (Some res) ->
        match res with
        | [ Urls.Overview ] -> [ "README.md" ]
        | [ Urls.Installation ] -> [ "Installation.md" ]
        | _ -> []
        |> loadOrSegment [ Urls.PigeonMaps ]
    | PathPrefix [ Urls.Tests ] (Some res) ->
        match res with
        | [ Urls.CallbackRef ] -> Tests.runCallbackTests()
        | [ Urls.ElmishComponents ] -> Samples.ElmishComponents.ReplacementTests.counterSwitcher()
        | [ Urls.FileUpload ] -> Tests.fileUpload()
        | [ Urls.ForwardRef ] -> Examples.forwardRefParent()
        | [ Urls.KeyboardKey ] -> Tests.keyboardKey()
        | [ Urls.Refs ] -> Examples.focusInputExample()
        | _ -> React.fragment [ for segment in input.state.CurrentPath -> Html.p segment ]
    | segments -> React.fragment [ for segment in segments -> Html.p segment ])

let main = React.functionComponent(fun (input: {| state: State; dispatch: Msg -> unit |}) ->
    let dispatch = React.useCallback(input.dispatch, [||])
    
    Html.div [
        prop.className [ Bulma.Tile; Bulma.IsAncestor ]
        prop.children [
            Html.div [
                prop.className [ Bulma.Tile; Bulma.Is2 ]
                prop.children [ sidebar {| state = input.state; dispatch = dispatch |} ]
            ]

            Html.div [
                prop.className Bulma.Tile
                prop.style [ style.paddingTop 30 ]
                prop.children [ content {| state = input.state; dispatch = dispatch |} ]
            ]
        ]
    ])

let render' = React.functionComponent(fun (input: {| state: State; dispatch: Msg -> unit |}) ->
    let dispatch = React.useCallback(input.dispatch, [||])
    
    let application =
        Html.div [
            prop.style [ 
                style.padding 30
            ]
            prop.children [ main {| state = input.state; dispatch = dispatch |} ]
        ]

    Router.router [
        Router.onUrlChanged (UrlChanged >> dispatch)
        Router.application application
    ])

let render (state: State) dispatch = render' {| state = state; dispatch = dispatch |}

Program.mkProgram init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run
