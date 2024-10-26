namespace Feliz

open Fable
open Fable.AST
open Fable.AST.Fable

[<assembly:ScanForPlugins>]
do()

// WIP: Not yet ready for use
// Attribute marks func with single param of seq/list/array as ReactComponent and convert that list into prop-object
type ReactListComponentAttribute() =
    inherit MemberDeclarationPluginAttribute()
    override this.FableMinimumVersion = "4.0"

    // replace original param with a new-object one and rebuild collection into local const by calling Object.entries
    override this.Transform(compiler, file, decl) =
        let propsArg = AstUtils.makeIdent (sprintf "%sInputProps" (AstUtils.camelCase decl.Name))
        let entries = Let(decl.Args[0], AstUtils.emitJs "(($value) => Object.entries($value))($0)" [IdentExpr(propsArg)], decl.Body)
        { decl with Args = [ propsArg ]; Body = entries }

    // take collection param as an input to Object.fromEntries and put it into react.createElement
    override this.TransformCall(compiler, memb, expr) =
        let reactElType = expr.Type
        match expr with
        | Call(callee, info, _typeInfo, _range) ->
            if info.Args.Length = 1 && AstUtils.isCollection info.Args[0].Type then
                let modified = AstUtils.emitJs "(($value) => Object.fromEntries($value))($0)" [info.Args[0]]
                AstUtils.createElement reactElType [callee; modified]
            else
                expr
        | _ ->
            expr