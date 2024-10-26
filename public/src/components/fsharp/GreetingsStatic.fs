module Docs.GreetingsStatic

open Feliz

type Components =
    [<ReactComponent>]
    static member Greeting(name: string, ?age: int) =
        Html.div [
            Html.span $"Hello, {name}!"
            if age.IsSome then
              Html.span $" You are {age} years old"
        ]

let GreetingsStatic() =
  Html.div [
      prop.style [style.border(1, borderStyle.solid, "red")]
      prop.children [
          Html.h4 "Greetings traveler!"
            // call-site is more readable because of the named parameters
          Components.Greeting(name="Jane", age=20)
          Components.Greeting(name="John")
      ]
  ]