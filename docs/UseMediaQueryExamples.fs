[<RequireQualifiedAccess>]
module UseMediaQueryExamples

open Browser
open Feliz
open Elmish
open Feliz.UseMediaQuery

let useResponsiveExample = React.functionComponent(fun () ->
(*
    // default breakpoints
    {
        MobileLandscape = 576
        Tablet = 768
        Desktop = 1024
        WideScreen = 1216
    }
*)
    
    let width = React.useResponsive()

    Html.div [
        let text = 
            match width with
            | ScreenSize.Mobile -> "Mobile"
            | ScreenSize.MobileLandscape -> "MobileLandscape"
            | ScreenSize.Tablet -> "Tablet"
            | ScreenSize.Desktop -> "Desktop"
            | ScreenSize.WideScreen -> "WideScreen"
        prop.children [
            Html.h1 text
            Html.p "Resize your screen to get the media query hook to re-render the component with updated information."
        ]
    ]
)

let useResponsiveCustomBreakpointsExample = React.functionComponent(fun () ->
    let customBreakpoints = {
        MobileLandscape = 600
        Tablet = 960
        Desktop = 1280
        WideScreen = 1920
    }

    let width = React.useResponsive(customBreakpoints)

    Html.div [
        let text = 
            match width with
            | ScreenSize.Mobile -> "Mobile"
            | ScreenSize.MobileLandscape -> "MobileLandscape"
            | ScreenSize.Tablet -> "Tablet"
            | ScreenSize.Desktop -> "Desktop"
            | ScreenSize.WideScreen -> "WideScreen"
        prop.children [
            Html.h1 text
            Html.p "Resize your screen to get the media query hook to re-render the component with updated information."
        ]
    ]
)


let useMediaQueryExample = React.functionComponent(fun () ->
    let matches = React.useMediaQuery("(max-width: 850px)")

    Html.div [
        let text = 
            if matches then 
                "850px or less"
            else
                "More than 850px"
                
        prop.children [
            Html.h1 text
            Html.p "Resize your screen to get the media query hook to re-render the component with updated information."
        ]
    ]
)
