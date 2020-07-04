#load ".paket/load/netstandard2.0/Build/build.group.fsx"

open Fake.IO.FileSystemOperators
open FSharp.Compiler.Range
open FSharpLint.Application
open FSharpLint.Framework
open System

let private writeLine (str: string) (color: ConsoleColor) (writer: IO.TextWriter) =
    let originalColour = Console.ForegroundColor
    Console.ForegroundColor <- color
    writer.WriteLine str
    Console.ForegroundColor <- originalColour

let private writeInfoLine (str: string) = writeLine str ConsoleColor.White Console.Out
let private writeWarningLine (str: string) = writeLine str ConsoleColor.Yellow Console.Out
let private writeErrorLine (str: string) = writeLine str ConsoleColor.Red Console.Error

let private parserProgress =
    function
    | Starting(file) -> String.Format(Resources.GetString("ConsoleStartingFile"), file) |> writeInfoLine
    | ReachedEnd(_, warnings) ->
        String.Format(Resources.GetString("ConsoleFinishedFile"), List.length warnings) |> writeInfoLine
    | Failed(file, parseException) ->
        String.Format(Resources.GetString("ConsoleFailedToParseFile"), file) |> writeErrorLine
        "Exception Message:" + Environment.NewLine + parseException.Message + Environment.NewLine
        + "Exception Stack Trace:" + Environment.NewLine + parseException.StackTrace + Environment.NewLine
        |> writeErrorLine

let mutable private collectWarning = List.empty<string>

let getErrorMessage (range:FSharp.Compiler.Range.range) =
    let error = Resources.GetString("LintSourceError")

    String.Format(error, range.StartLine, range.StartColumn)

let highlightErrorText (range:range) (errorLine:string) =
    let highlightColumnLine =
        if String.length errorLine = 0 then "^"
        else
            errorLine
            |> Seq.mapi (fun i _ -> if i = range.StartColumn then "^" else " ")
            |> Seq.reduce (+)

    errorLine + Environment.NewLine + highlightColumnLine

let private writeLintWarning (warning : Suggestion.LintWarning) =
    let highlightedErrorText = highlightErrorText warning.Details.Range (getErrorMessage warning.Details.Range)
    let warnMsg = warning.Details.Message + Environment.NewLine + highlightedErrorText + Environment.NewLine + warning.ErrorText

    collectWarning <- ((if collectWarning.Length > 0 then Environment.NewLine + warnMsg
                        else warnMsg)
                        :: collectWarning)

    warnMsg |> writeWarningLine
    String.replicate 80 "*" |> writeInfoLine

let private handleError (str: string) = writeErrorLine str

let private handleLintResult =
    function
    | LintResult.Failure(failure) -> handleError failure.Description
    | _ -> ()

let private parseInfo (webFile: bool) =
    let config =
        if webFile then ConfigurationParam.FromFile (__SOURCE_DIRECTORY__ @@ "fsharplint.json")
        else ConfigurationParam.Default
    { CancellationToken = None
      ReceivedWarning = Some writeLintWarning
      Configuration = config
      ReportLinterProgress = Some parserProgress }
          
let lintFiles (fileList: (bool * string list) list) =
    let lintFile (webFile: bool) (file: string) =
        let sw = Diagnostics.Stopwatch.StartNew()
        try
            Lint.lintFile (parseInfo webFile) file |> handleLintResult
            sw.Stop()

        with e ->
            sw.Stop()

            let error =
                "Lint failed while analysing " + file + "." + Environment.NewLine + "Failed with: " + e.Message
                + Environment.NewLine + "Stack trace:" + e.StackTrace

            error |> handleError

    fileList
    |> List.iter (fun (webFile, fList) -> fList |> List.iter (lintFile webFile))
