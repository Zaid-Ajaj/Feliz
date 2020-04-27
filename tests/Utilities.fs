[<AutoOpen>]
module Utilities

open System
open Feliz 
open Browser.Types
open Browser.Dom
open Fable.Mocha
open Fable.Core
open Fable.ReactTestingLibrary
 
type IRenderer =
    inherit IDisposable
    abstract Container : unit -> HTMLElement

let renderReact (element: ReactElement) =
    let id = Guid.NewGuid().ToString()
    let container = document.createElement("div")
    container.setAttribute("id", id)
    document.body.appendChild(container) |> ignore
    ReactDOM.render(element, container)
    { new IRenderer with
        member this.Container() = container
        member this.Dispose() = document.getElementById(id).remove() }

let testReact name test = 
    testCase name <| fun _ -> 
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        test()

let testReactAsync name test = 
    testCaseAsync name <| async {
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        let! _ = test
        return ()
    }

let ftestReact name test = 
    ftestCase name <| fun _ -> 
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        test()

let ftestReactAsync name test = 
    ftestCaseAsync name <| async {
        use rtl = { new IDisposable with member this.Dispose() = RTL.cleanup() }
        let! _ = test
        return ()
    }

[<Emit("$1['style'][$0]")>]
let getStyle<'t> (key: string) (x: HTMLElement) : 't = jsNative 