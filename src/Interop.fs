namespace Feliz

open Fable.Core
open Fable.Core.JsInterop
open Fable.React

type internal ReactChildren =
    abstract toArray: ReactElement -> ReactElement seq
    abstract toArray: ReactElement seq -> ReactElement seq

type internal IReactApi =
    abstract createElement: comp: obj * props: obj -> ReactElement
    abstract createElement: comp: obj * props: obj * [<ParamList>] children: ReactElement seq -> ReactElement
    abstract Children : ReactChildren


[<RequireQualifiedAccess>]
module Interop =
    let internal reactApi : IReactApi = importDefault "react"
    let reactElement (name: string) (props: 'a) : ReactElement = import "createElement" "react"
    let inline mkAttr (key: string) (value: obj) : IReactProperty = unbox (key, value)
    let inline mkStyle (key: string) (value: obj) : IStyleAttribute = unbox (key, value)
    let inline createElement name (properties: IReactProperty list) : ReactElement =
        reactElement name (keyValueList CaseRules.LowerFirst properties)