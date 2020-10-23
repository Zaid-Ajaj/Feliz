namespace Feliz

open Fable
open Fable.AST

/// <summary>Transforms a function into a React function component. Make sure the function is defined at the module level</summary>
type ReactComponentAttribute() =
    inherit MemberDeclarationPluginAttribute()
    override _.FableMinimumVersion = "3.0"

    /// <summary>Transforms call-site into createElement calls</summary>
    override _.TransformCall(compiler, memb, expr) =
        let membArgs = memb.CurriedParameterGroups |> List.concat
        match expr with
        | Fable.Call(callee, info, typeInfo, range) when List.length membArgs = List.length info.Args ->
            let callee =
                match callee with
                | Fable.Expr.IdentExpr ident -> Fable.Expr.IdentExpr { ident with Name = AstUtils.capitalize ident.Name }
                | _ -> callee

            if info.Args.Length = 1 && AstUtils.isRecord compiler info.Args.[0].Type then
                // F# Component { Value = 1 }
                // JSX <Component Value={1} />
                // JS createElement(Component, { Value: 1 })
                if AstUtils.recordHasField "Key" compiler info.Args.[0].Type then
                    // However, when the key property is upper-case (which is common in record fields)
                    // then we should rewrite it
                    let modifiedRecord = AstUtils.emitJs "((value) => { value.key = value.Key; return value; })($0)" [info.Args.[0]]
                    AstUtils.makeCall (AstUtils.makeImport "createElement" "react") [callee; modifiedRecord]
                else
                    AstUtils.makeCall (AstUtils.makeImport "createElement" "react") [callee; info.Args.[0]]
            elif info.Args.Length = 1 && info.Args.[0].Type = Fable.Type.Unit then
                // F# Component()
                // JSX <Component />
                // JS createElement(Component, null)
                AstUtils.makeCall (AstUtils.makeImport "createElement" "react") [callee; AstUtils.nullValue]
            else
            let propsObj =
                List.zip membArgs info.Args
                |> List.choose (fun (arg, expr) -> arg.Name |> Option.map (fun k -> k, expr))
                |> AstUtils.objExpr

            // TODO: Detect usage of an input called `children` anc spread it into the component
            AstUtils.makeCall (AstUtils.makeImport "createElement" "react") [callee; propsObj]
        | _ -> expr

    override this.Transform(compiler, decl) =
        if decl.Info.IsValue || decl.Info.IsGetter || decl.Info.IsSetter then
            // Invalid attribute usage
            let errorMessage = sprintf "Expecting a function declation for %s when using [<ReactComponent>]" decl.Name
            compiler.LogWarning(errorMessage, ?range=decl.Body.Range)
            decl
        else
            // TODO: make sure isRecord works with records
            if decl.Args.Length = 1 && AstUtils.isRecord compiler decl.Args.[0].Type then
                // do not rewrite components accepting records as input
                { decl with Name = AstUtils.capitalize decl.Name }
            else if decl.Args.Length = 1 && decl.Args.[0].Type = Fable.Type.Unit then
                // remove arguments from functions requiring unit as input
                { decl with Args = [ ]; Name = AstUtils.capitalize decl.Name }
            else
            // rewrite all other arguments into getters of a single props object
            // TODO: transform any callback into into useCallback(callback) to stabilize reference
            let propsArg = AstUtils.makeIdent (sprintf "%sProps" decl.Name)
            let body =
                ([], decl.Args) ||> List.fold (fun bindings arg ->
                    // TODO: detect usage of "key" and emit warning
                    let getterKind = Fable.ByKey(Fable.ExprKey(AstUtils.makeStrConst arg.DisplayName))
                    let getter = Fable.Get(Fable.IdentExpr propsArg, getterKind, Fable.Any, None)
                    (arg, getter)::bindings)
                |> List.rev
                |> fun bindings -> Fable.Let(bindings, decl.Body)

            { decl with
                Args = [propsArg]
                Body = body
                Name = AstUtils.capitalize decl.Name }