# Render Static Html

Using Feliz, you can render static HTML as a string from a `ReactElement` using the `ReactDOMServer` API.


### `ReactDOMServer.renderToString()`

```fsharp:static-html
[<ReactComponent>]
let StaticHtml() =
    let html = Html.div [
        prop.style [ style.padding 20 ]
        prop.children [
            Html.h1 "Html content"
            Html.br [ ]
        ]
    ]

    Html.pre [
        Html.text (ReactDOMServer.renderToString html)
    ]
```

### `ReactDOMServer.renderToStaticMarkup()`

```fsharp:static-markup
[<ReactComponent>]
let StaticMarkup() =
    let html = Html.div [
        prop.style [ style.padding 20 ]
        prop.children [
            Html.h1 "Html content"
            Html.br [ ]
        ]
    ]

    Html.pre [
        Html.text (ReactDOMServer.renderToStaticMarkup html)
    ]
```