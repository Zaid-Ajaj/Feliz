#r @"packages/build/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake

let libPath = "./src"
let testsPath = "./demo"

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
      libPath </> "obj" ]
    |> CleanDirs

Target "Clean" <| fun _ ->
    cleanCacheDirs()
    cleanBundles()

Target "InstallNpmPackages" (fun _ ->
  printfn "Node version:"
  run nodeTool "--version" __SOURCE_DIRECTORY__
  run "npm" "--version" __SOURCE_DIRECTORY__
  run "yarn" "install" __SOURCE_DIRECTORY__
)

Target "RestoreFableTestProject" <| fun _ ->
  run dotnetCli "restore" testsPath

Target "RunLiveTests" <| fun _ ->
    run npmTool "start" "."

let publish projectPath = fun () ->
    [ projectPath </> "bin"
      projectPath </> "obj" ] |> CleanDirs
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

Target "PublishNuget" (publish libPath)

Target "CompileFableTestProject" <| fun _ ->
    run npmTool "run build" "."

Target "Build" DoNothing

"Clean"
  ==> "InstallNpmPackages"
  ==> "RestoreFableTestProject"
  ==> "RunLiveTests"

"Clean"
  ==> "InstallNpmPackages"
  ==> "RestoreFableTestProject"
  ==> "CompileFableTestProject"
  ==> "Build"

RunTargetOrDefault "Build"
