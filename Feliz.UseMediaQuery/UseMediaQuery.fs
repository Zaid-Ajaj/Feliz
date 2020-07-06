module Feliz.UseMediaQuery

open System
open Browser
open Browser.Types
open Feliz

open Fable.Core
open Fable.Core.JsInterop

[<RequireQualifiedAccess>]
type ScreenSize =
    | Mobile
    | MobileLandscape
    | Tablet
    | Desktop
    | WideScreen

type Breakpoints = {
    MobileLandscape: int
    Tablet: int
    Desktop: int
    WideScreen: int
}

[<RequireQualifiedAccess>]
module Breakpoints =
    let defaults = {
        MobileLandscape = 576
        Tablet = 768
        Desktop = 1024
        WideScreen = 1216
    }

let private makeQueries breakpoints =
    let mobileQuery = sprintf "(max-width: %ipx)" breakpoints.MobileLandscape
    let mobileLandscapeQuery = sprintf "(max-width: %ipx)" breakpoints.Tablet
    let tabletQuery = sprintf "(max-width: %ipx)" breakpoints.Desktop
    let desktopQuery = sprintf "(max-width: %ipx)" breakpoints.WideScreen
    mobileQuery, mobileLandscapeQuery, tabletQuery, desktopQuery

[<AutoOpen>]
module UseMediaQueryExtension =
     [<Emit("($0).addListener($1)")>]
     let private addListener mqList fn = jsNative

     [<Emit("($0).removeListener($1)")>]
     let private removeListener mqList fn = jsNative

     type React with
        /// A hook for media queries, this hook will force a component
        /// to re-render when the specified media query changes.
        static member useMediaQuery (mediaQuery: string) =
            let (mq, setMq) =
                React.useState(fun _ -> window.matchMedia(mediaQuery).matches)

            React.useEffect(fun () ->
                let mediaQueryList = window.matchMedia(mediaQuery)
                let handler = fun _ ->
                        setMq(mediaQueryList.matches)

                handler ()
                addListener mediaQueryList handler

                let ret =
                    {new IDisposable with
                        member this.Dispose() =
                            removeListener mediaQueryList handler}
                ret
            , [| mediaQuery :> obj |])

            mq

        /// A hook for responsive design, this hook will force a component
        /// to re-render the components on resize.
        /// Returns a discriminated union with the new width.
        static member useResponsive(?breakpoints: Breakpoints) =
            let breakpoints = Option.defaultValue Breakpoints.defaults breakpoints
            let m, l, t, d = makeQueries breakpoints
            let mx = React.useMediaQuery m
            let lx = React.useMediaQuery l
            let tx = React.useMediaQuery t
            let dx = React.useMediaQuery d
            match mx, lx, tx, dx with
            | true, _, _, _ -> ScreenSize.Mobile
            | _, true, _, _ -> ScreenSize.MobileLandscape
            | _, _, true, _ -> ScreenSize.Tablet
            | _, _, _, true -> ScreenSize.Desktop
            | _ -> ScreenSize.WideScreen