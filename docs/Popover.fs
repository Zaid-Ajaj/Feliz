module Samples.Popover

open Feliz
open Feliz.Popover

module Basic =
    let popoverWithText' = React.functionComponent(fun (input: {| content: string |}) -> [
        let (popoverOpen, toggleOpen) = React.useState false
        Popover.popover [
            popover.body [
                // the body is the content of the pop over when it is opened
                Html.div [
                    prop.text input.content
                    prop.style [
                        style.backgroundColor.white
                        style.padding 20
                        style.borderRadius 10
                        style.boxShadow(0, 0, 10, color.black)
                    ]
                ]
            ]
            popover.isOpen popoverOpen
            popover.place.above
            popover.disableTip
            popover.onOuterAction (fun _ -> toggleOpen(false))
            popover.children [
                /// The content that this popover will orient itself around.
                Html.button [
                    prop.text "Open popover"
                    prop.onClick (fun _ -> toggleOpen(not popoverOpen))
                ]
            ]
        ]
    ])

    let sample = Html.div [
        popoverWithText' {| content = "Popover Content" |}
        popoverWithText' {| content = "Another Popover" |}
    ]