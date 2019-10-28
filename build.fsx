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

let mutable dotnetCli = "dotnet"

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
      "Feliz.Popover" </> "bin"
      "Feliz.Popover" </> "obj"
      "Feliz.Markdown" </> "bin"
      "Feliz.Markdown" </> "obj"
      "Feliz.PigeonMaps" </> "bin"
      "Feliz.PigeonMaps" </> "obj"
      "Feliz.Template" </> "bin"
      "Feliz.Template" </> "obj"
      "Feliz.ElmishComponents" </> "bin"
      "Feliz.ElmishComponents" </> "obj" ]
    |> CleanDirs

Target "Clean" <| fun _ ->
    cleanCacheDirs()
    cleanBundles()

Target "InstallNpmPackages" (fun _ ->
  printfn "Node version:"
  run nodeTool "--version" __SOURCE_DIRECTORY__
  run "npm" "--version" __SOURCE_DIRECTORY__
  run "npm" "install" __SOURCE_DIRECTORY__
)

Target "Start" <| fun _ ->
    run npmTool "start" "."

let publish projectPath = fun () ->
    [ projectPath </> "bin"
      projectPath </> "obj"
      projectPath </> "Content" </> "node_modules" ] |> CleanDirs
    run dotnetCli "restore --no-cache" projectPath
    run dotnetCli "pack -c Release" projectPath
    let nugetKey =
        match environVarOrNone "NUGET_KEY" with
        | Some nugetKey -> nugetKey
        | None -> failwith "The Nuget API key must be set in a NUGET_KEY environmental variable"
    let nupkg =
        Directory.GetFiles(projectPath </> "bin" </> "Release")
        |> Seq.head
        |> Path.GetFullPath

    let pushCmd = sprintf "nuget push %s -s nuget.org -k %s" nupkg nugetKey
    run dotnetCli pushCmd projectPath

Target "PublishFeliz" (publish libPath)
Target "PublishRecharts" (publish "./Feliz.Recharts")
Target "PublishPigeonMaps" (publish "./Feliz.PigeonMaps")
Target "PublishTemplate" (publish "./Feliz.Template")
Target "PublishMarkdown" (publish "./Feliz.Markdown")
Target "PublishPopover" (publish "./Feliz.Popover")
Target "PublishElmishComponents" (publish "./Feliz.ElmishComponents")

Target "PatchFeliz" <| fun _ ->
    [ publish libPath
      publish "./Feliz.Recharts"
      publish "./Feliz.PigeonMaps"
      publish "./Feliz.Popover"
      publish "./Feliz.Template"
      publish "./Feliz.ElmishComponents" ]
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
