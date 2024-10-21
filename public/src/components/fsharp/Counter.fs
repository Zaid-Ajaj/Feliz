module Docs.Counter

open Feliz

[<ReactComponent>]
let Counter() =
  let (count, setCount) = React.useState(0)
  Html.div [
      Html.h1 count
      Html.button [
        prop.text "Increment"
        prop.onClick (fun _ -> setCount(count + 1))
      ]
  ]