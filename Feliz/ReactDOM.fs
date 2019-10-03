namespace Feliz

open Fable.Core

type ReactDOM =
    [<Import("render", "react-dom")>]
    static member render(element: Fable.React.ReactElement, container: Browser.Types.HTMLElement) = jsNative
    static member render(element: Fable.React.FunctionComponent<unit>, container: Browser.Types.HTMLElement) =
        ReactDOM.render(element(), container)