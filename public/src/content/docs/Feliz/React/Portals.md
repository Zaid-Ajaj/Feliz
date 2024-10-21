# Portals

Portals provide a way to render components outside the DOM hierarchy of the parent component. Portals are often used to create dialog components in React applications.

You can create a portal component by calling the function `ReactDOM.createPortal : ReactElement -> HTMLElement -> ReactElement`.

Usually, you use components by first creating an extra mount point outside of the React root DOM element.

```html
<html>
    <body>
        <div id="root"></div>
        <div id="portal-root"></div>
    </body>
</html>
```

Then you can create a reusable component `portal` to render a child component into the portal DOM root.

```fsharp:portals
open Browser.Dom

[<ReactComponent>]
let Portal(content: ReactElement) =
    let root = document.getElementById("root")
    ReactDOM.createPortal(content, root)

[<ReactComponent>]
let PortalsPopup() =
    Html.div [
        prop.style [
            style.position.absolute
            style.top 10
            style.right 10
            style.padding 10
            style.backgroundColor.lightGreen
        ]
        prop.children [
            Html.p [
                prop.text "Portals can be used to escape the parent component."
            ]
        ]
    ]

[<ReactComponent>]
let PortalsContainer() =
    Html.div [
        prop.style [
            style.padding 10
            style.overflow.hidden
        ]
        prop.children [
            Portal(PortalsPopup())
        ]
    ]
```
