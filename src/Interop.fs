namespace Feliz

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Feliz.React


[<RequireQualifiedAccess>]
module Interop =
    let reactApi : IReactApi = importDefault "react"
    let reactElement (name: string) (props: 'a) : ReactElement = import "createElement" "react"
    let inline mkAttr (key: string) (value: obj) : IReactProperty = unbox (key, value)
    let inline mkStyle (key: string) (value: obj) : IStyleAttribute = unbox (key, value)
    let inline createElement name (properties: IReactProperty list) : ReactElement =
        reactElement name (keyValueList CaseRules.LowerFirst properties)