#r @"packages/build/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake

let libPath = "./Feliz"
let testsPath = "./docs"

let platformTool tool winTool =
  let tool = if isUnix then tool else winTool
  tool
  |> ProcessHelper.tryFindFileOnPath
  |> function Some t -> t | _ -> failwithf "%s not found" tool

let nodeTool = "node"
let npmTool = "npm"

let dotnetCli = "dotnet"

let run fileName args workingDir =
    printfn "CWD: %s" workingDir
    let fileName, args =
        if EnvironmentHelper.isUnix
        then fileName, args else "cmd", ("/C " + fileName + " " + args)
    let ok =
        execProcess (fun info ->
            info.FileName <- fileName
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if not ok then failwith (sprintf "'%s> %s %s' task failed" workingDir fileName args)

let delete file =
    if File.Exists(file)
    then DeleteFile file
    else ()

let cleanBundles() =
    Path.Combine("public", "bundle.js")
        |> Path.GetFullPath
        |> delete
    Path.Combine("public", "bundle.js.map")
        |> Path.GetFullPath
        |> delete

let cleanCacheDirs() =
    [ testsPath </> "bin"
      testsPath </> "obj"
      libPath </> "bin"
      libPath </> "obj"
      "Feliz.Delay" </> "bin"
      "Feliz.Delay" </> "obj"
      "Feliz.Popover" </> "bin"
      "Feliz.Popover" </> "obj"
      "Feliz.Markdown" </> "bin"
      "Feliz.Markdown" </> "obj"
      "Feliz.PigeonMaps" </> "bin"
      "Feliz.PigeonMaps" </> "obj"
      "Feliz.Template" </> "bin"
      "Feliz.Template" </> "obj"
      "Feliz.Recharts" </> "bin"
      "Feliz.Recharts" </> "obj"
      "Feliz.UseDeferred" </> "bin"
      "Feliz.UseDeferred" </> "obj"
      "Feliz.UseElmish" </> "bin"
      "Feliz.UseElmish" </> "obj"
      "Feliz.UseMediaQuery" </> "bin"
      "Feliz.UseMediaQuery" </> "obj"
      "Feliz.ElmishComponents" </> "bin"
      "Feliz.ElmishComponents" </> "obj" ]
    |> CleanDirs

Target "Clean" <| fun _ ->
    cleanCacheDirs()
    cleanBundles()

Target "InstallNpmPackages" (fun _ ->
  printfn "Node version:"
  run nodeTool "--version" __SOURCE_DIRECTORY__
  run npmTool "--version" __SOURCE_DIRECTORY__
  run npmTool "install" __SOURCE_DIRECTORY__
)

Target "Start" <| fun _ ->
    run npmTool "start" "."

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

let publish projectPath = fun () ->
    [ projectPath </> "bin"
      projectPath </> "obj"
      projectPath </> "Content" </> "node_modules" ] |> CleanDirs
    run dotnetCli "restore --no-cache" projectPath
    run dotnetCli "pack -c Release" projectPath
    let nugetKey =
        match getEnvFromAllOrNone "FELIZ_NUGET_KEY", getEnvFromAllOrNone "NUGET_KEY" with
        | Some nugetKey, _ -> nugetKey
        | None, Some nugetKey -> nugetKey
        | _ -> failwith "The Nuget API key must be set in a NUGET_KEY environmental variable"
    let nupkg =
        Directory.GetFiles(projectPath </> "bin" </> "Release")
        |> Seq.head
        |> Path.GetFullPath

    let pushCmd = sprintf "nuget push %s -s nuget.org -k %s" nupkg nugetKey
    run dotnetCli pushCmd projectPath

Target "PublishFeliz" (publish libPath)
Target "PublishCompilerPlugins" (publish "./Feliz.CompilerPlugins")
Target "PublishSvelte" (publish "./Feliz.Svelte")
Target "PublishRecharts" (publish "./Feliz.Recharts")
Target "PublishRoughViz" (publish "./Feliz.RoughViz")
Target "PublishPigeonMaps" (publish "./Feliz.PigeonMaps")
Target "PublishUseDeferred" (publish "./Feliz.UseDeferred")
Target "PublishUseElmish" (publish "./Feliz.UseElmish")
Target "PublishTemplate" (publish "./Feliz.Template")
Target "PublishMarkdown" (publish "./Feliz.Markdown")
Target "PublishPopover" (publish "./Feliz.Popover")
Target "PublishUseMediaQuery" (publish "./Feliz.UseMediaQuery")
Target "PublishDelay" (publish "./Feliz.Delay")
Target "PublishElmishComponents" (publish "./Feliz.ElmishComponents")

Target "PatchFeliz" <| fun _ ->
    [ publish libPath
      publish "./Feliz.Markdown"
      publish "./Feliz.PigeonMaps"
      publish "./Feliz.Popover"
      publish "./Feliz.Recharts"
      publish "./Feliz.RoughViz"
      publish "./Feliz.Template"
      publish "./Feliz.UseDeferred"
      publish "./Feliz.UseElmish"
      publish "./Feliz.UseMediaQuery"
      publish "./Feliz.Delay" ]
   |> List.iter (fun target -> target())

Target "Compile" <| fun _ ->
    run npmTool "run build" "."

Target "Build" DoNothing

"Clean"
  ==> "InstallNpmPackages"
  ==> "Start"

"Clean"
  ==> "InstallNpmPackages"
  ==> "Compile"
  ==> "Build"

RunTargetOrDefault "Build"
