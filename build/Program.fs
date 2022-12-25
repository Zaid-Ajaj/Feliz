module Program

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Xml
open System.Xml.Linq
open Fake.IO
open Fake.Core

let path xs = Path.Combine(Array.ofList xs)

let solutionRoot = Files.findParent __SOURCE_DIRECTORY__ "Feliz.sln";

let project name = path [ solutionRoot; $"Feliz.{name}" ]

let feliz = path [ solutionRoot; "Feliz" ]
let compilerPlugins = project "CompilerPlugins" 
let delay = project "Delay"
let kawaii = project "Kawaii"
let markdown = project "Markdown"
let pigeonMaps = project "PigeonMaps"
let popover = project "Popover"
let recharts = project "Recharts" 
let roughViz = project "RoughViz"
let selectSearch = project "SelectSearch"
let svelte = project "Svelte"
let svelteComponent = project "SvelteComponent"
let template = project "Template"
let useDeferred = project "UseDeferred"
let useElmish = project "UseElmish"
let useMediaQuery = project "UseMediaQuery"

let publish projectDir =
    path [ projectDir; "bin" ] |> Shell.deleteDir
    path [ projectDir; "obj" ] |> Shell.deleteDir

    if Shell.Exec(Tools.dotnet, "pack --configuration Release", projectDir) <> 0 then
        failwithf "Packing '%s' failed" projectDir
    else
        let nugetKey =
            match Environment.environVarOrNone "NUGET_KEY" with
            | Some nugetKey -> nugetKey
            | None -> 
                printfn "The Nuget API key was not found in a NUGET_KEY environmental variable"
                printf "Enter NUGET_KEY: "
                Console.ReadLine()

        let nugetPath =
            Directory.GetFiles(path [ projectDir; "bin"; "Release" ])
            |> Seq.head
            |> Path.GetFullPath

        if Shell.Exec(Tools.dotnet, sprintf "nuget push %s -s nuget.org -k %s" nugetPath nugetKey, projectDir) <> 0
        then failwith "Publish failed"

[<EntryPoint>]
let main (args: string[]) = 
    try
        // run tasks
        match args with 
        | [| "publish-feliz" |] -> publish feliz
        | [| "publish-recharts" |] -> publish recharts
        | [| "publish-compiler-plugins" |] -> publish compilerPlugins
        | [| "publish-select-search" |] -> publish selectSearch
        | [| "publish-pigeon-maps" |] -> publish pigeonMaps
        | [| "publish-template" |] -> publish template
        | [| "publish-svelte" |] -> publish svelte
        | [| "publish-svelte-component" |] -> publish svelteComponent
        | [| "publish-popover" |] -> publish popover
        | [| "publish-delay" |] -> publish delay
        | [| "publish-kawaii" |] -> publish kawaii
        | [| "publish-markdown" |] -> publish markdown
        | [| "publish-rough-viz" |] -> publish roughViz
        | [| "publish-use-deferred" |] -> publish useDeferred
        | [| "publish-use-elmish" |] -> publish useElmish
        | [| "publish-use-media-query" |] -> publish useMediaQuery
        | _ -> printfn "Unknown args: %A" args
        
        // exit succesfully
        0
    with 
    | ex -> 
        // something bad happened
        printfn "Error occured"
        printfn "%A" ex
        1