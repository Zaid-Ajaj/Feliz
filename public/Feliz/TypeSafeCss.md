# Type-Safe CSS

Feliz aims to provide a fully type-safe API around CSS and React styling such that the only possible values you can use are those that are valid
```fs
let exampleDiv =
    Html.div [
        prop.style [
            style.display.flex
            style.display.none
            style.fontSize 20
            style.borderRadius 15
            style.alignContent.flexStart
            style.textDecorationColor.blue
            style.visibility.hidden
            style.textDecoration.lineThrough
            style.position.sticky
            style.borderRadius (length.px 10)
            style.margin 10
            style.margin (length.px 10)
            style.margin(10, 10, 10, 20)
            style.margin(10, 10, 10)
            style.margin(10, 10)
            style.width 10
            style.height 100
            style.height (length.vh 50)
            style.height (length.percent 100)
            style.boxShadow(0, 0, 10, colors.gray)
            style.boxShadow(10, 10, 0, 5, colors.black)
            style.backgroundColor.fuchsia
            style.backgroundColor "#FFFFFF"
            style.border(3, borderStyle.dashed, colors.crimson)
            style.borderColor.blue
            style.color.red
            style.color "#000000"
        ]
    ]
```