module internal Feliz.AstUtils

open Fable
open Fable.AST
open System
open System.Linq
open System.Text.RegularExpressions

let cleanFullDisplayName str =
    Regex.Replace(str, @"`\d+", "").Replace(".", "_")

let makeIdent name: Fable.Ident =
    { Name = name
      Type = Fable.Any
      IsCompilerGenerated = true
      IsThisArgument = false
      IsMutable = false
      Range = None }

let makeValue r value =
    Fable.Value(value, r)

let makeStrConst (x: string) =
    Fable.StringConstant x
    |> makeValue None

let nullValue = Fable.Expr.Value(Fable.ValueKind.Null(Fable.Type.Any), None)

let emitJs macro args  =
    let callInfo: Fable.CallInfo =
        { ThisArg = None
          Args = args
          SignatureArgTypes = []
          HasSpread = false
          IsJsConstructor = false
          CallMemberInfo = None }

    let emitInfo : Fable.AST.Fable.EmitInfo =
        { Macro = macro
          IsJsStatement = false
          CallInfo = callInfo }

    Fable.Expr.Emit(emitInfo, Fable.Type.Any, None)

let rec flattenList (head: Fable.Expr) (tail: Fable.Expr) =
    [
        yield head;
        match tail with
        | Fable.Expr.Value (value, range) ->
            match value with
            | Fable.ValueKind.NewList(Some(nextHead, nextTail), listType) ->
                yield! flattenList nextHead nextTail
            | Fable.ValueKind.NewList(None, listType) ->
                yield! [ ]
            | _ ->
                yield! [ Fable.Expr.Value (value, range) ]

        | _ ->
            yield! [ ]
    ]

let makeImport (selector: string) (path: string) =
    Fable.Import({ Selector = selector
                   Path = path
                   IsCompilerGenerated = true }, Fable.Any, None)

let isRecord (compiler: PluginHelper) (fableType: Fable.Type) =
    match fableType with
    | Fable.Type.AnonymousRecordType _ -> true
    | Fable.Type.DeclaredType (entity, genericArgs) -> compiler.GetEntity(entity).IsFSharpRecord
    | _ -> false

let isReactElement (fableType: Fable.Type) =
    match fableType with
    | Fable.Type.DeclaredType (entity, genericArgs) -> entity.QualifiedName.EndsWith "ReactElement"
    | _ -> false

let recordHasField name (compiler: PluginHelper) (fableType: Fable.Type) =
    match fableType with
    | Fable.Type.AnonymousRecordType (fieldNames, genericArgs) ->
        fieldNames
        |> Array.exists (fun field -> field = name)

    | Fable.Type.DeclaredType (entity, genericArgs) ->
        compiler.GetEntity(entity).FSharpFields
        |> List.exists (fun field -> field.Name = name)

    | _ ->
        false

let makeCall callee args =
    let callInfo: Fable.CallInfo =
        { ThisArg = None
          Args = args
          SignatureArgTypes = []
          HasSpread = false
          IsJsConstructor = false
          CallMemberInfo = None }
    Fable.Call(callee, callInfo, Fable.Any, None)

type MemberInfo(?info: Fable.MemberInfo,
                ?isValue: bool) =
    let infoOr f v =
        match info with
        | Some i -> f i
        | None -> v
    let argOrInfoOr arg f v =
        match arg, info with
        | Some arg, _ -> arg
        | None, Some i -> f i
        | None, None -> v
    interface Fable.MemberInfo with
        member _.IsValue = argOrInfoOr isValue (fun i -> i.IsValue) false
        member _.Attributes = infoOr (fun i -> i.Attributes) Seq.empty
        member _.HasSpread = infoOr (fun i -> i.HasSpread) false
        member _.IsPublic = infoOr (fun i -> i.IsPublic) true
        member _.IsInstance = infoOr (fun i -> i.IsInstance) true
        member _.IsMutable = infoOr (fun i -> i.IsMutable) false
        member _.IsGetter = infoOr (fun i -> i.IsGetter) false
        member _.IsSetter = infoOr (fun i -> i.IsSetter) false
        member _.IsEnumerator = infoOr (fun i -> i.IsEnumerator) false
        member _.IsMangled = infoOr (fun i -> i.IsMangled) false

let objValue (k, v): Fable.MemberDecl =
    {
        Name = k
        FullDisplayName = k
        Args = []
        Body = v
        UsedNames = Set.empty
        Info = MemberInfo(isValue=true)
        ExportDefault = false
    }


let objExpr kvs =
    Fable.ObjectExpr(List.map objValue kvs, Fable.Any, None)


let capitalize (input: string) =
    if String.IsNullOrWhiteSpace input
    then ""
    else input.First().ToString().ToUpper() + String.Join("", input.Skip(1))
