module App

open Elmish
open Elmish.React
open Feliz
open Feliz.Recharts
open Feliz.Markdown
open Fable.SimpleHttp
open Feliz.Router
open Fable.Core
open Fable.Core.JsInterop
open Fable.Core.Experimental
open Zanaptak.TypedCssClasses
open System.Collections.Generic

type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.5/css/bulma.min.css", Naming.PascalCase>
type FA = CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>


type Highlight =
    static member inline highlight (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "react-highlight", createObj !!properties)

type State = {
    CurrentPath : string list
}

let init() = { CurrentPath = [ ] }

type Msg = Increment | Decrement | UrlChanged of string list

let update msg state =
    match msg with
    | UrlChanged segments -> { state with CurrentPath = segments }
    | _ -> state

[
    style.display.flex
    style.display.none
    style.fontSize 20
    style.borderRadius 15
    style.alignContent.flexStart
    style.textDecorationColor.blue
    style.visibility.hidden
    style.textDecoration.lineThrough
    style.position.sticky
    style.boxShadow(10, 10, colors.black)
    style.boxShadow(10, 10, 10, colors.black)
    style.boxShadow(0, 0, 10, colors.black)
    style.boxShadow(0, 0, 10, 10, colors.darkGray)
    style.borderRadius (length.px 10)
    style.margin 10
    style.backgroundRepeat.repeatX
    style.backgroundPosition.fixedNoScroll
    style.margin (length.px 10)
    style.margin(10, 10, 10, 20)
    style.margin(10, 10, 10)
    style.margin(10, 10)
    style.width 10
    style.height 100
    style.height (length.vh 50)
    style.height (length.percent 100)
    style.backgroundColor.fuchsia
    style.backgroundColor "#FFFFFF"
    style.border(3, borderStyle.dashed, colors.crimson)
    style.borderColor.blue
    style.transform.scale3D(20, 20, 20)
    style.transform.translateX(100)
    style.transform.translateY(100)
    style.transform.translateZ(100)
    style.color.red
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
    Html.keyedFragment(1, [
        Html.div [
            Html.keyedFragment("hello", [
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
        Html.input [ prop.withType.checkbox ]
        Html.input [ prop.withType "password" ]
    ]

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
            Html.fragment [
                Html.h1 "One"
                Html.h2 "Two"
            ]
        ]
    ]

type AnimationProps = { title: string }
let styledComponent title =
    // Re-use state internally using React hooks
    let animationsOnHover =
        React.functionComponent (fun (props: AnimationProps) ->
            let (hovered, setHovered) = React.useState(false)
            Html.div [
                prop.style [
                    yield! [
                        style.padding 10
                        style.transitionProperty("background-color", "color")
                        style.transitionDurationMilliseconds 500
                    ]

                    if hovered then
                       yield style.backgroundColor.lightBlue
                       yield style.color.black
                    else
                       yield style.backgroundColor.limeGreen
                       yield style.color.white
                ]

                prop.onMouseEnter (fun _ -> setHovered(true))
                prop.onMouseLeave (fun _ -> setHovered(false))
                prop.children [
                    Html.h2 props.title
                ]
            ]
        )

    animationsOnHover { title = title }


module ReactComponents =
    type Greeting = { Name: string option }
    let greeting = React.functionComponent <| fun (props: Greeting) ->
        Html.div [
            Html.span "Hello, "
            Html.span (Option.defaultValue "World" props.Name)
        ]

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
        ]
    )



let counter =
    React.functionComponent(fun () ->
        let (count, setCount) = React.useState 0
        React.useEffect(fun () ->
            Browser.Dom.window.document.title <- string count
        )

        Html.div [
            Html.h1 count
            Html.button [
                prop.text "Increment"
                prop.onClick (fun _ -> setCount(count + 1))
            ]
        ]
    )

[<Emit("setInterval($0, $1)")>]
let setInterval (f: unit -> unit) (n: int) : int = jsNative

[<Emit("clearInterval($0)")>]
let clearInterval (n: int) : unit = jsNative

let ticker =
    React.functionComponent("Ticker", fun (props: {| start: int |}) ->
        let (tick, setTick) = React.useState props.start
        React.useEffect(fun () ->
            let interval = setInterval (fun () ->
                printfn "Tick"
                setTick(tick + 1)) 1000

            React.createDisposable(fun () -> clearInterval(interval))
        , prop.start)

        Html.h1 tick
    )

let styledComponentsTests =
    Html.div [
       styledComponent "Hello"
       styledComponent "From"
       styledComponent "Fable"
    ]

let hooksAreAwesome =
    Html.fragment [
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
        ]
    )



type ICodeProperties =
    abstract language : string
    abstract value : string

let apps = [
    "feliz-elmish-counter", ElmishCounter.app()
    "simple-components", ReactComponents.simple
    "stateful-counter", ReactComponents.counter()
    "recharts-main", Samples.Recharts.AreaCharts.Main.chart()
    "recharts-area-simpleareachart", Samples.Recharts.AreaCharts.SimpleAreaChart.chart()
    "recharts-area-stackedareachart", Samples.Recharts.AreaCharts.StackedAreaChart.chart()
    "recharts-area-tinyareachart", Samples.Recharts.AreaCharts.TinyAreaChart.chart()
    "recharts-line-simplelinechart", Samples.Recharts.LineCharts.SimpleLineChart.chart()
    "recharts-line-responsivefullwidth", Samples.Recharts.LineCharts.ResponsiveFullWidth.chart()
    "recharts-area-responsefullwidth", Samples.Recharts.AreaCharts.ResponsiveFullWidth.chart()
    "recharts-bar-simplebarchart", Samples.Recharts.BarCharts.SimpleBarChart.chart()
]

let githubPath (rawPath: string) =
    let parts = rawPath.Split('/')
    if parts.Length > 5
    then sprintf "http://www.github.com/%s/%s" parts.[3] parts.[4]
    else rawPath

let loadMarkdown (path: string list) =
    let markdown = React.functionComponent <| fun _ ->
        let isLoading, setLoading = React.useState false
        let error, setError = React.useState<Option<string>> None
        let content, setContent = React.useState ""
        let path =
            match path with
            | [ one ] when one.StartsWith "http" -> one
            | segments -> String.concat "/" segments
        React.useEffect(fun _ ->
            setLoading(true)
            async {
                let! (statusCode, responseText) = Http.get path
                setLoading(false)
                if statusCode = 200 then
                  setContent(responseText)
                  setError(None)
                else
                  setError(Some (sprintf "Status %d: could not load %s" statusCode path))
            }
            |> Async.StartImmediate
        )

        if isLoading then
            Html.div [
                prop.style [ style.textAlign.center; style.marginTop 20 ]
                prop.children [
                    Html.li [ prop.className [ FA.Fa; FA.FaRefresh; FA.FaSpin; FA.Fa3X ] ]
                ]
            ]
        else
            match error with
            | Some error -> Html.h1 [ prop.style [ style.color.crimson ]; prop.text error ]
            | None ->
                Html.div [
                    prop.className "content"
                    prop.style [ style.width (length.percent 100); style.padding 20 ]
                    prop.children [
                        if path.StartsWith "https://raw.githubusercontent.com" then
                            yield Html.h1 [
                                Html.i [ prop.className [ FA.Fa; FA.FaGithub ] ]
                                Html.anchor [
                                    prop.style [ style.marginLeft 10; style.color.lightGreen ]
                                    prop.href (githubPath path)
                                    prop.text "View on Github"
                                ]
                            ]
                        yield Markdown.markdown [
                            prop.custom("source", content)
                            prop.custom("renderers", createObj [
                                "code" ==> fun (props: ICodeProperties) ->
                                    if props.language <> null && props.language.Contains ":" then
                                        let languageParts = props.language.Split(':')
                                        let sampleName = languageParts.[1]
                                        let sampleApp =
                                            apps
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
                                                prop.text(props.value)
                                            ]
                                        ]
                                    else
                                        Highlight.highlight [
                                            prop.className "fsharp"
                                            prop.text(props.value)
                                        ]
                            ])
                        ]
                    ]
                ]
    markdown()

// A collapsable nested menu for the sidebar
// keeps internal state on whether the items should be visible or not based on the collapsed state
let nestedMenuList (name: string) (items: Fable.React.ReactElement list) =
    let statefulComponent =
        React.functionComponent("NestedMenuList", fun () ->
            let (collapsed, setCollapsed) = React.useState(false)
            Html.li [
                Html.anchor [
                    prop.style [ style.width (length.percent 100) ]
                    prop.onClick (fun _ -> setCollapsed(not collapsed))
                    prop.children [
                        Html.i [
                            prop.style [ style.marginRight 10 ]
                            prop.className [
                                true, FA.Fa;
                                not collapsed, FA.FaAngleUp;
                                collapsed, FA.FaAngleDown;
                            ]
                        ]

                        Html.span name
                    ]
                ]

                Html.ul [
                    prop.className Bulma.MenuList
                    prop.children (if not collapsed then items else [ ])
                ]
            ]
        )

    statefulComponent()

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
                prop.className [ state.CurrentPath = path, Bulma.IsActive ]
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
                menuItem "Syntax" [ Urls.Feliz; Urls.Syntax ]
                menuItem "Type-Safe CSS" [ Urls.Feliz; Urls.TypeSafeCss ]
                menuItem "Conditional Styling" [ Urls.Feliz; Urls.ConditionalStyling ]
                menuItem "Elmish Counter" [ Urls.Feliz; Urls.ElmishCounter ]
                nestedMenuList "React" [
                    menuItem "Components" [ Urls.Feliz; Urls.React; Urls.Components ]
                    menuItem "Standalone" [ Urls.Feliz; Urls.React; Urls.Standalone ]
                ]

                nestedMenuList "Ecosystem" [
                    menuItem "Feliz.Router" [ Urls.Ecosystem; Urls.Router ]
                    menuItem "Feliz.Recharts" [ Urls.Ecosystem; Urls.Recharts ]
                    menuItem "Feliz.MaterialUI" [ Urls.Ecosystem; Urls.Mui ]
                ]
            ]
            menuLabel "Feliz.Recharts"
            menuList [
                menuItem "Overview" [ Urls.Recharts; Urls.Overview ]
                menuItem "Installation" [ Urls.Recharts; Urls.Installation ]
                nestedMenuList "Line Charts" [
                    menuItem "Simple Line Chart" [ Urls.Recharts; Urls.LineCharts; Urls.SimpleLineChart ]
                    menuItem "Responsive Full Width" [ Urls.Recharts; Urls.LineCharts; Urls.ResponsiveFullWidth ]
                ]
                nestedMenuList "Bar Charts" [
                    menuItem "Simple Bar Chart" [ Urls.Recharts; Urls.BarCharts; Urls.SimpleBarChart ]
                ]
                nestedMenuList "Area Charts" [
                    menuItem "Simple Area Chart" [ Urls.Recharts; Urls.AreaCharts; Urls.SimpleAreaChart ]
                    menuItem "Stacked Area Chart" [ Urls.Recharts; Urls.AreaCharts; Urls.StackedAreaChart ]
                    menuItem "Tiny Area Chart" [ Urls.Recharts; Urls.AreaCharts; Urls.TinyAreaChart ]
                    menuItem "Responsive Full Width" [ Urls.Recharts; Urls.AreaCharts; Urls.ResponsiveFullWidth ]
                ]
            ]
        ]
    ]


let readme = sprintf "https://raw.githubusercontent.com/%s/%s/master/README.md"

let content state dispatch =
    match state.CurrentPath with
    | [ ] -> loadMarkdown [ "Feliz"; "README.md" ]
    | [ Urls.Feliz; Urls.Overview; ] -> loadMarkdown [ "Feliz"; "README.md" ]
    | [ Urls.Feliz; Urls.Installation ] -> loadMarkdown [ "Feliz"; "Installation.md" ]
    | [ Urls.Feliz; Urls.ElmishCounter ] -> loadMarkdown [ "Feliz"; "ElmishCounter.md" ]
    | [ Urls.Feliz; Urls.Syntax ] -> loadMarkdown [ "Feliz" ; "Syntax.md" ]
    | [ Urls.Feliz; Urls.TypeSafeCss ] -> loadMarkdown [ "Feliz"; "TypeSafeCss.md" ]
    | [ Urls.Feliz; Urls.ConditionalStyling ] -> loadMarkdown [ "Feliz"; "ConditionalStyling.md" ]
    | [ Urls.Feliz; Urls.React; Urls.Standalone ] -> loadMarkdown [ "Feliz"; "React"; "Standalone.md" ]
    | [ Urls.Feliz; Urls.React; Urls.Components ] -> loadMarkdown [ "Feliz"; "React"; "Components.md" ]
    | [ Urls.Ecosystem; Urls.Router ] -> loadMarkdown [ readme "Zaid-Ajaj" "Feliz.Router" ]
    | [ Urls.Ecosystem; Urls.Mui ] -> loadMarkdown [ readme "cmeeren" "Feliz.MaterialUI" ]
    | [ Urls.Ecosystem; Urls.Recharts ] -> loadMarkdown [ "Recharts"; "README.md" ]
    | [ Urls.Recharts; Urls.Overview ] -> loadMarkdown [ "Recharts"; "README.md" ]
    | [ Urls.Recharts; Urls.Installation ] -> loadMarkdown [ "Recharts"; "Installation.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.SimpleAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "SimpleAreaChart.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.StackedAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "StackedAreaChart.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.TinyAreaChart ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "TinyAreaChart.md" ]
    | [ Urls.Recharts; Urls.LineCharts; Urls.SimpleLineChart ] -> loadMarkdown [ "Recharts"; "LineCharts"; "SimpleLineChart.md" ]
    | [ Urls.Recharts; Urls.LineCharts; Urls.ResponsiveFullWidth ] -> loadMarkdown [ "Recharts"; "LineCharts"; "ResponsiveFullWidth.md" ]
    | [ Urls.Recharts; Urls.AreaCharts; Urls.ResponsiveFullWidth ] -> loadMarkdown [ "Recharts"; "AreaCharts"; "ResponsiveFullWidth.md" ]
    | [ Urls.Recharts; Urls.BarCharts; Urls.SimpleBarChart ] -> loadMarkdown [ "Recharts"; "BarCharts"; "SimpleBarChart.md" ]
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
                prop.children [ content state dispatch ]
            ]
        ]
    ]

let application state dispatch =
    Html.div [
        prop.style [ style.padding 50 ]
        prop.children [ main state dispatch ]
    ]

let render (state: State) dispatch =
    Router.router [
        Router.onUrlChanged (UrlChanged >> dispatch)
        Router.application (application state dispatch)
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run