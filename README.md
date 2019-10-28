# Feliz (beta) [![Nuget](https://img.shields.io/nuget/v/Feliz.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz) [![Build status](https://ci.appveyor.com/api/projects/status/yqinhayy1pgfcubp/branch/master?svg=true)](https://ci.appveyor.com/project/Zaid-Ajaj/feliz/branch/master)

A fresh retake of the base React DSL to build React applications, optimized for happiness.

Here is how it looks like:

```fs
open Feliz

let render state dispatch =
  Html.div [
    Html.button [
        prop.style [ style.marginRight 5 ]
        prop.onClick (fun _ -> dispatch Increment)
        prop.text "Increment"
    ]

    Html.button [
        prop.style [ style.marginLeft 5 ]
        prop.onClick (fun _ -> dispatch Decrement)
        prop.text "Decrement"
    ]

    Html.h1 state.Count
  ]
```

### Features

 - Consistent, lightweight **formatting**: no more awkward indentation using two lists for every element.
 - Discoverable **attributes** with no more functions, `Html` attributes or css properties globally available so they are easy to find.
 - Proper **documentation**: each attribute and CSS property
 - Fully **Type-safe**: no more `Margin of obj` but instead utilizing a plethora of overloaded functions to account for the overloaded nature of `CSS` attributes, covering 90%+ of the CSS styles, values and properties.
 - Included **color list** of most commonly used `Html` colors in the `colors` module.
 - **Compatible** with the current DSL used in applications.
 - **Compatible** with [Femto](https://github.com/Zaid-Ajaj/Femto).
 - Approximately **Zero** bundle size increase where everything function body is erased from the generated javascript unless you actually use said function.

### Overloaded elements

Elements can take simple values as their only input such a string, a number, a single child element or a list of child elements.

```fs
Html.h1 state.Count

Html.div "Hello there!"

Html.div [ Html.h1 "So lightweight" ]

Html.ul [
  Html.li "One"
  Html.li [ Html.strong "Two" ]
  Html.li [ Html.em "Three" ]
]
```
You could also expand the attribute:
```fs
Html.h1 [
    prop.className "title"
    prop.children [
        Html.text "Hello there"
    ]
]
```
Which is the same as:
```fs
Html.h1 [
    prop.className "title"
    prop.text "Hello there"
]
```
Single element child works too as input:
```fs
Html.h1 (Html.strong "I am bold")

// same as

Html.h1 [ Html.strong "I am bold" ]

// same as

Html.h1 [
    prop.children [ Html.strong "I am bold" ]
]
```

Input elements are also easy to work with:

```fs
Html.input [
    prop.className "input"
    prop.valueOrDefault state.Crendentials.Password // string
    prop.onChange (SetPassword >> dispatch) // (string -> unit)
    prop.withType.password
]

Html.input [
    prop.className "input checkbox"
    prop.valueOrDefault state.RememberMe // boolean
    prop.onChange (SetRememberMe >> dispatch) // (bool -> unit)
    prop.withType.checkbox
]
```
Here the `onChange` property is overloaded with the following types
```fs
// generic onChange event
type onChange = Event -> unit
// onChange for textual input boxes
type onChange = string -> unit
// onChange for boolean input boxes, i.e. checkbox
type onChange = bool -> unit
```

To render the empty element, i.e. instructing React to render nothing, use `Html.none`
```fs
match state with
| None -> Html.none
| Some data -> render data
```

### React [hooks](https://reactjs.org/docs/hooks-intro.html) and function components

Using Feliz, you can make use of React's specific functionality such as hooks and function component as they core to modern React applications, for example this how it looks in plain javascript:
```js
function Counter() {
  // Declare a new state variable, which we'll call "count"
  const [count, setCount] = useState(0);

  return (
    <div>
      <h1>{count}</h1>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}
```
In F# it looks like this:
```fs
open Feliz

let counter =
    React.functionComponent(fun () ->
        let (count, setCount) = React.useState(0)
        Html.div [
            Html.h1 count
            Html.button [
                prop.text "Increment"
                prop.onClick (fun _ -> setCount(count + 1))
            ]
        ]
    )
```

These function components can also be named such that you could inspect them using React profiling tools
```fs
let counter =
    React.functionComponent("Counter", fun () ->
        let (count, setCount) = React.useState(0)
        Html.div [
            Html.h1 count
            Html.button [
                prop.text "Increment"
                prop.onClick (fun _ -> setCount(count + 1))
            ]
        ]
    )
```

### MVU with built-in React hook: [useReducer](https://reactjs.org/docs/hooks-reference.html#usereducer)

```fs
type State = { Count : int }
type Msg = Increment | Decrement

let initialState = { Count = 0 }

let update (state: State) = function
    | Increment -> { state with Count = state.Count + 1 }
    | Decrement -> { state with Count = state.Count - 1 }

let counter = React.functionComponent("Counter", fun () ->
    let (state, dispatch) = React.useReducer(update, initialState)
    Html.div [
        Html.h3 state.Count
        Html.button [ prop.onClick (fun _ -> dispatch Increment); prop.text "Increment" ]
        Html.button [ prop.onClick (fun _ -> dispatch Decrement); prop.text "Decrement" ]
    ]
)
```

### Type-safe style attributes

```fs
let customStyles =
    prop.style [
        style.display.flex
        style.display.none
        style.fontSize 20
        style.borderRadius 15
        style.alignContent.flexStart
        style.textDecorationColor.blue
        style.visibility.hidden
        style.textDecoration.lineThrough
        style.position.sticky
        style.borderRadius (length.px 10)
        style.margin 10
        style.margin (length.px 10)
        style.margin(10, 10, 10, 20)
        style.margin(10, 10, 10)
        style.margin(10, 10)
        style.width 10
        style.height 100
        style.height (length.vh 50)
        style.height (length.percent 100)
        style.boxShadow(0, 0, 10, colors.gray)
        style.boxShadow(10, 10, 0, 5, colors.black)
        style.backgroundColor.fuchsia
        style.backgroundColor "#FFFFFF"
        style.border(3, borderStyle.dashed, colors.crimson)
        style.borderColor.blue
        style.color.red
        style.color "#000000"
    ]
```

### Missing an element, an attributes or a style?

Please file an issue, because it should be there!

### Different ways of specifying the classes of elements

```fs
prop.className "button" // => "button"
prop.className [ "btn"; "btn-primary" ] // => "btn btn-primary"
prop.className [ true, "conditional"; false, "falsy"; true, "class" ] // => "conditional class"
```

The property `className` has overloads to combine a list of classes into a single class, convenient when your classes are bound to values so that you do not need to concatenate them yourself.

### Conditional and styles

The library includes two convenient functions to apply classes or styles conditionally on elements:
```fsharp
Html.div [
    prop.style [
        true, [ style.margin 10 ]
        state.Count >= 10, [
            style.backgroundColor.red
            style.fontSize 20
        ]
        state.Count >= 20, [
            style.backgroundColor.green
            style.fontSize 30
        ]
    ]
]
```