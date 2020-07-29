namespace Feliz

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Feliz.ReactApi

[<RequireQualifiedAccess>]
module Interop =
    let reactApi : IReactApi = importDefault "react"
    let reactElement (name: string) (props: 'a) : ReactElement = import "createElement" "react"
    let mkAttr (key: string) (value: obj) : IReactProperty = unbox (key, value)
    let mkStyle (key: string) (value: obj) : IStyleAttribute = unbox (key, value)
    let inline reactElementWithChild (name: string) (child: 'a) =
        reactElement name (createObj [ "children" ==> [| child |] ])
    let inline reactElementWithChildren (name: string) (children: #seq<ReactElement>) =
        reactElement name (createObj [ "children" ==> reactApi.Children.toArray (Array.ofSeq children) ])
    let inline createElement name (properties: IReactProperty list) : ReactElement =
        reactElement name (createObj !!properties)
