[<RequireQualifiedAccess>]
module Tools

open System
open System.IO
open Fake.Core

module CreateProcess =
    /// Creates a cross platfrom command from the given program and arguments.
    ///
    /// For example:
    ///
    /// ```fsharp
    /// CreateProcess.xplatCommand "npm" [ "install" ]
    /// ```
    ///
    /// Will be the following on windows
    ///
    /// ```fsharp
    /// CreateProcess.fromRawCommand "cmd" [ "/C"; "npm"; "install" ]
    /// ```
    /// And the following otherwise
    ///
    /// ```fsharp
    /// CreateProcess.fromRawCommand "npm" [ "install" ]
    /// ```
    let xplatCommand program args =
        let program', args' =
            if Environment.isWindows
            then "cmd", List.concat [ [ "/C"; program ]; args ]
            else program, args

        CreateProcess.fromRawCommand program' args'
 
let executablePath (tool: string) =
    let locator =
        if Environment.isWindows
        then "C:\\Windows\\System32\\where.exe"
        else "/usr/bin/which"

    let locatorOutput =
        CreateProcess.xplatCommand locator [ tool ]
        |> CreateProcess.redirectOutput
        |> Proc.run

    if locatorOutput.ExitCode <> 0 then failwithf "Could not determine the executable path of '%s'" tool

    locatorOutput.Result.Output
    |> String.splitStr Environment.NewLine
    |> List.filter (fun path -> (Environment.isWindows && Path.HasExtension(path)) || Environment.isUnix)
    |> List.tryFind File.Exists
    |> function
        | Some executable -> executable
        | None -> failwithf "The executable paht '%s' was not found" tool

let dotnet = executablePath "dotnet"
let npm = executablePath "npm"
let node = executablePath "node"