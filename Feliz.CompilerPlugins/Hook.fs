namespace Feliz

open Fable
open Fable.AST

/// <summary>Applies to custom defined React hooks to ensure the generated code starts with "use" in order for fast-refresh to pick it up</summary>
type HookAttribute() =
    inherit MemberDeclarationPluginAttribute()
    override _.FableMinimumVersion = "3.0"

    /// <summary>Transforms call-site into createElement calls</summary>
    override _.TransformCall(compiler, memb, expr) =
        let membArgs = memb.CurriedParameterGroups |> List.concat
        match expr with
        | Fable.Call(callee, info, typeInfo, range) ->
            let callee =
                match callee with
                | Fable.Expr.IdentExpr ident ->
                    // capitalize same-file references
                    Fable.Expr.IdentExpr { ident with Name = "use" + ident.Name }
                | Fable.Expr.Import(importInfo, fableType, sourceLocation) ->
                    // capitalize component imports from different modules/files
                    let selector = "use" + importInfo.Selector
                    let modifiedImportInfo = { importInfo with Selector = selector }
                    Fable.Expr.Import(modifiedImportInfo, fableType, sourceLocation)
                | _ ->
                    callee

            Fable.Call(callee, info, typeInfo, range)

        | _ ->
            expr

    override this.Transform(compiler, file, decl) =
        if decl.Info.IsValue || decl.Info.IsGetter || decl.Info.IsSetter then
            // Invalid attribute usage
            let errorMessage = sprintf "Expecting a function declation for %s when using [<Hook>]" decl.Name
            compiler.LogWarning(errorMessage, ?range=decl.Body.Range)
            decl
        else
            { decl with Name = "use" + decl.Name }