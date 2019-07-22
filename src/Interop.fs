namespace Feliz

open Fable.Core
open Fable.Core.JsInterop
open Fable.React

[<RequireQualifiedAccess>]
module Interop =
    let reactElement (name: string) (props: 'a) : ReactElement = import "createElement" "react"
    let inline mkAttr (key: string) (value: obj) : IReactAttribute = unbox (key, value)
    let inline mkStyle (key: string) (value: obj) : IStyleAttribute = unbox (key, value)
    let inline createElement name (properties: IReactAttribute list) : ReactElement =
        reactElement name (keyValueList CaseRules.LowerFirst properties)