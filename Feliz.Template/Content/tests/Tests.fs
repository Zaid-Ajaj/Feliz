module Tests

open Fable.Mocha
open App

let appTests = testList "App tests" [
    testCase "update function works" <| fun _ ->
        // Simplified update that ignore commands/effects
        let update state msg = fst (App.update msg state)
        let initialState = { Count = 0 }
        let incomingMsgs =  [ Increment; Increment; Decrement ]
        let updatedState = List.fold update initialState incomingMsgs
        Expect.equal 1 updatedState.Count "Expected updated state to be 1"
]

let allTests = testList "All" [
    appTests
]

[<EntryPoint>]
let main (args: string[]) = Mocha.runTests allTests
