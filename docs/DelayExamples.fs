[<RequireQualifiedAccess>]
module DelayExamples

open Fable.Core
open Feliz
open Feliz.Delay
open Feliz.Delay.Templates
open Zanaptak.TypedCssClasses

type FA = CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>

let private centeredSpinner =
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

let simpleDelay = React.functionComponent(fun () ->
    React.delay [
        delay.waitFor 2000

        delay.children [
            Html.text "Here I am!"
        ]
    ])

let delayWithCustomFallback = React.functionComponent(fun () ->
    React.delay [
        delay.waitFor 2000

        delay.children [
            Html.text "Here I am!"
        ]

        delay.fallback centeredSpinner
    ])

let nestedDelays = React.functionComponent(fun () ->
    React.delay [
        delay.waitFor 2000

        delay.children [
            delayWithCustomFallback()
        ]

        delay.fallback [
            Html.text "Hanging out for a little bit..."
        ]
    ])

let myDelay = 
    React.Templates.delay [ 
        delay.waitFor 2000
        delay.fallback [
            Html.text "Delay template!"
        ]
    ]

let renderDelayTemplate = React.functionComponent(fun () ->
    myDelay [
        Html.div "Here I am from the template!"
    ])

let asyncComponent : JS.Promise<unit -> ReactElement> = JsInterop.importDynamic "./CodeSplitting.fs"

let slowImport () =
    React.lazy'((fun () ->
        promise {
            do! Promise.sleep 2000
            return! asyncComponent
        }
    ),())

let delaySuspense = React.functionComponent(fun () ->
    React.delaySuspense [
        delaySuspense.delay [
            delay.waitFor 500

            delay.children [
                centeredSpinner
            ]
        ]
        
        delaySuspense.children [
            slowImport()
        ]
    ])

let myDelaySuspense =
    React.Templates.delaySuspense [
        delay.waitFor 500

        delay.fallback [
            Html.text "Delay template!"
        ]

        delay.children [
            centeredSpinner
        ]
    ]

let renderDelaySuspenseTemplate = React.functionComponent(fun () ->
    myDelaySuspense [
        slowImport()
    ])
