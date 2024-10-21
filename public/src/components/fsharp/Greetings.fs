module Docs.Greetings

open Greeting
open Feliz

let Greetings() =
  Html.div [
      prop.style [style.border(1, borderStyle.solid, "red")]
      prop.children [
          Html.h4 "Greetings traveler!"
          Greeting (Some "John")
          Greeting None
      ]
  ]