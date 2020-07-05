// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------
#nowarn "0213"
#r "paket: groupref Build //"
#load "./lint.fsx"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.JavaScript
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Tools
open System

let solutionFile = "Feliz.sln"

let repo = "https://github.com/Zaid-Ajaj/Feliz"

let templates = 
    !! (__SOURCE_DIRECTORY__ @@ "Feliz/paket.template")
    ++ (__SOURCE_DIRECTORY__ @@ "Feliz.*/paket.template")

let bin              = __SOURCE_DIRECTORY__ @@ "bin"
let mainReleaseNotes = __SOURCE_DIRECTORY__ @@ "Feliz/RELEASE_NOTES.md"

// Read additional information from the release notes document
let felizReleaseNotes = ReleaseNotes.load mainReleaseNotes

let fsSrc =
    !! (__SOURCE_DIRECTORY__ @@ "Feliz/*.fs")
    ++ (__SOURCE_DIRECTORY__ @@ "Feliz.*/*.fs")
    ++ (__SOURCE_DIRECTORY__ @@ "tests/**/*.fs")
    ++ (__SOURCE_DIRECTORY__ @@ "docs/**/*.fs")

let foldExcludeGlobs (g: IGlobbingPattern) (d: string) = g -- d
let foldIncludeGlobs (g: IGlobbingPattern) (d: string) = g ++ d

// Files that have bindings to other languages where name linting needs to be more relaxed.
let relaxedNameLinting = fsSrc |> List.ofSeq

let fsRelaxedNameLinting =
    let baseGlob s =
        !! s
        -- (__SOURCE_DIRECTORY__  @@ "src/**/AssemblyInfo.*")
        -- (__SOURCE_DIRECTORY__  @@ "src/**/obj/**")
        -- (__SOURCE_DIRECTORY__  @@ "tests/**/obj/**")
    match relaxedNameLinting with
    | [h] when relaxedNameLinting.Length = 1 -> baseGlob h |> Some
    | h::t -> List.fold foldIncludeGlobs (baseGlob h) t |> Some
    | _ -> None

let getEnvFromAllOrNone (s: string) =
    let envOpt (envVar: string) =
        if String.IsNullOrEmpty envVar then None
        else Some(envVar)

    let procVar = Environment.GetEnvironmentVariable(s) |> envOpt
    let userVar = Environment.GetEnvironmentVariable(s, EnvironmentVariableTarget.User) |> envOpt
    let machVar = Environment.GetEnvironmentVariable(s, EnvironmentVariableTarget.Machine) |> envOpt

    match procVar,userVar,machVar with
    | Some(v), _, _
    | _, Some(v), _
    | _, _, Some(v)
        -> Some(v)
    | _ -> None

// --------------------------------------------------------------------------------------
// Clean tasks

Target.create "Clean" <| fun _ ->
    let clean () =
        !! (__SOURCE_DIRECTORY__ @@ "Feliz/**/bin")
        ++ (__SOURCE_DIRECTORY__ @@ "Feliz/**/obj")
        ++ (__SOURCE_DIRECTORY__ @@ "Feliz.*/**/bin")
        ++ (__SOURCE_DIRECTORY__ @@ "Feliz.*/**/obj")
        ++ (__SOURCE_DIRECTORY__ @@ "tests/**/bin")
        ++ (__SOURCE_DIRECTORY__ @@ "tests/**/obj")
        ++ (__SOURCE_DIRECTORY__ @@ "public/bundle.*")
        ++ bin
        |> Seq.toList
        |> Shell.cleanDirs

    TaskRunner.runWithRetries clean 10

// --------------------------------------------------------------------------------------
// Restore tasks

let restoreSolution () =
    solutionFile
    |> DotNet.restore id

Target.create "Restore" <| fun _ ->
    TaskRunner.runWithRetries restoreSolution 5

Target.create "NpmInstall" <| fun _ ->
    Npm.exec "install" id

// --------------------------------------------------------------------------------------
// Lint source code

Target.create "Lint" <| fun _ ->
    fsSrc
    |> (fun src -> List.fold foldExcludeGlobs src relaxedNameLinting)
    |> (fun fGlob ->
        match fsRelaxedNameLinting with
        | Some(glob) ->
            [(false, fGlob); (true, glob)]
        | None -> [(false, fGlob)])
    |> Seq.map (fun (b,glob) -> (b,glob |> List.ofSeq))
    |> List.ofSeq
    |> Lint.lintFiles

// --------------------------------------------------------------------------------------
// Build tasks

Target.create "Compile" <| fun _ ->
    Npm.exec "build" id

Target.create "Build" <| fun _ ->
    DotNet.build (fun p -> { p with NoRestore = true }) "Feliz.sln"

// --------------------------------------------------------------------------------------
// Runners

Target.create "RunTests" <| fun _ ->
    Npm.exec "test" id

Target.create "Start" <| fun _ ->
    Npm.exec "start" id

// --------------------------------------------------------------------------------------
// Release Scripts

let gitPush msg =
    Git.Staging.stageAll ""
    Git.Commit.exec "" msg
    Git.Branches.push ""

Target.create "GitPush" <| fun p ->
    p.Context.Arguments
    |> List.choose (fun s ->
        match s.StartsWith("--Msg=") with
        | true -> Some(s.Substring 6)
        | false -> None)
    |> List.tryHead
    |> function
    | Some(s) -> s
    | None -> (sprintf "Bump version to %s" felizReleaseNotes.NugetVersion)
    |> gitPush

Target.create "GitTag" <| fun _ ->
    Git.Branches.tag "" felizReleaseNotes.NugetVersion
    Git.Branches.pushTag "" "origin" felizReleaseNotes.NugetVersion

Target.create "PublishPages" <| fun _ ->
    Npm.exec "publish-docs" id

Target.create "PublishDocs" <| fun _ ->
    gitPush "Publishing docs"

Target.create "Pack" <| fun _ ->
    templates
    |> Seq.iter (fun path -> 
        let template = FileInfo.ofPath(path)
        let releaseNotes = 
            template.Directory.FullName @@ "RELEASE_NOTES.md"
            |> ReleaseNotes.load

        Paket.pack(fun p ->
            { p with
                WorkingDir = template.Directory.FullName
                OutputPath = template.Directory.FullName @@ "bin"
                Version = releaseNotes.NugetVersion
                ReleaseNotes = Fake.Core.String.toLines releaseNotes.Notes
                ProjectUrl = repo
                MinimumFromLockFile = true
                IncludeReferencedProjects = true }))

let pushPackage (project: string) =
    let apiKey = 
        match getEnvFromAllOrNone "FELIZ_NUGET_KEY", getEnvFromAllOrNone "NUGET_KEY" with
        | Some nugetKey, _ -> nugetKey
        | None, Some nugetKey -> nugetKey
        | _ -> failwith "The Nuget API key must be set in a NUGET_KEY environmental variable"

    let dir = __SOURCE_DIRECTORY__ @@ project @@ "bin"

    Paket.push(fun p ->
        { p with
            ApiKey = apiKey
            WorkingDir = dir })

Target.create "PublishAll" <| fun _ ->
    templates
    |> Seq.iter (FileInfo.ofPath >> (fun fi -> fi.DirectoryName) >> pushPackage)

Target.create "Publish" <| fun p ->
    p.Context.Arguments
    |> List.choose (fun s ->
        match s.StartsWith("--Lib=") with
        | true -> Some(s.Substring 6)
        | false -> None)
    |> List.tryHead
    |> function
    | Some(s) -> pushPackage s
    | None -> failwith "Must specify a package to publish. For example: --Lib=Feliz"

// --------------------------------------------------------------------------------------
// Run all targets (except release scripts) by default. Invoke 'build -t <Target>' to override

"Clean"
    ==> "Restore"
    ==> "NpmInstall"
    ==> "Lint"
    ==> "RunTests"
    ==> "Build"
    ==> "Compile"
    ==> "PublishPages"

"Compile"
    ?=> "GitPush"
    ?=> "GitTag"

"Compile"
    ?=> "Pack"
    ?=> "Publish"
    ?=> "GitTag"

"Compile"
    ?=> "Pack"
    ?=> "PublishAll"
    ?=> "GitTag"

"RunTests" ==> "Start"

"Publish" <== ["Compile"; "PublishPages"]
"PublishAll" <== ["Compile"; "PublishPages"]

Target.runOrDefaultWithArguments "Start"
