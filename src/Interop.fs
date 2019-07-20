namespace Feliz

open Fable.Core
open Fable.Core.JsInterop
open Fable.React

[<RequireQualifiedAccess>]
module Interop =
    let reactElement (name: string) (props: 'a) (children: 'b) : ReactElement = import "createElement" "react"

    let inline mkAttr (key: string) (value: obj) : IReactAttribute = unbox (key, value)
    let inline mkStyle (key: string) (value: obj) : IStyleAttribute = unbox (key, value)

    [<Emit("$2[$0] = $1")>]
    let setProperty (key: string) (value: obj) (objectLiteral: obj) = jsNative

    let createObj (properties: obj list) =
        let propsObj = obj()
        let propsList = unbox<(string * obj) list> properties
        for (key, value) in propsList do
            setProperty key value propsObj
        propsObj

    let extract (pred: 't -> bool) (xs: 't list) : 't option * 't list  =
      let rec extract' pred xs elem acc =
        match xs, elem with
        | [ ], Some _ -> elem, acc
        | [ ], None -> None, acc
        | values, Some _ -> elem, List.append values acc
        | x :: rest, None when pred x -> Some x, List.append acc rest
        | x :: rest, None -> extract' pred rest None (List.append [x] acc)

      extract' pred xs None [ ]

    let createElement name (properties: IReactAttribute list) : ReactElement =
      let props = unbox<(string * obj) list> properties
      match extract (fun (key, value) -> key = "children") props with
      | None, rest ->
          let restProps = createObj (unbox rest)
          reactElement name restProps null
      | Some (key, value), rest ->
          let restProps = createObj (unbox rest)
          reactElement name restProps (unbox value)

    let createVoid name (properties: IReactAttribute list) : ReactElement =
      let props = unbox<(string * obj) list> properties
      match extract (fun (key, value) -> key = "children") props with
      | _, rest ->
          let restProps = createObj (unbox rest)
          reactElement name restProps null