namespace Feliz

open Fable
open Fable.AST

type IntrinsicElementChildrenAttribute(tagName:string) =
    inherit MemberDeclarationPluginAttribute()
    override _.FableMinimumVersion = "3.0"

    override _.TransformCall(logger, memb, expr) =
        match expr with
        | Fable.Call(callee, info, typeInfo, range) when info.Args.Length = 1 ->
            match info.Args.[0] with
            | Fable.Expr.Value(value, range) ->

                match value with
                | Fable.ValueKind.NewList (Some (head, tail), listType) ->
                    let element = AstUtils.makeStrConst tagName
                    AstUtils.makeCall (AstUtils.makeImport "createElement" "react") [
                        element
                        AstUtils.nullValue
                        yield! AstUtils.flattenList head tail
                    ]

                | Fable.ValueKind.NewArray (expressions, arrayType) ->
                    let element = AstUtils.makeStrConst tagName
                    AstUtils.makeCall (AstUtils.makeImport "createElement" "react") [
                        element
                        AstUtils.nullValue
                        yield! expressions
                    ]

                | _ ->
                    expr

            | _ ->
                expr
        | _ ->
            expr

    override this.Transform(logger, decl) = decl