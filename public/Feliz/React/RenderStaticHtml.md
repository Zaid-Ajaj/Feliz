# Render Static Html

Using Feliz, you can render static HTML as a string from a `ReactElement` using the `ReactDOMServer` API.


### `ReactDOMServer.renderToString()`

```fsharp:static-html
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
```

### `ReactDOMServer.renderToStaticMarkup()`

```fsharp:static-markup
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
```