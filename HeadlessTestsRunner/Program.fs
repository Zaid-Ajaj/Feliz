[<EntryPoint>]
let main argv =
    "../public-tests"
    |> System.IO.Path.GetFullPath
    |> Puppeteer.runTests
    |> Async.RunSynchronously