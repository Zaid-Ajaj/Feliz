# Syntax

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

### Form Inputs

Input elements are dead simple to work with:

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

### The empty element

To render the empty element, i.e. instructing React to render nothing, use `Html.none`
```fs
match state with
| None -> Html.none
| Some data -> render data
```

### Specifying class names

```fs
prop.className "button" // => "button"
prop.className [ "btn"; "btn-primary" ] // => "btn btn-primary"
prop.className [ true, "conditional"; false, "falsy"; true, "class" ] // => "conditional class"
```

The property `className` has overloads to combine a list of classes into a single class, convenient when your classes are bound to values so that you do not need to concatenate them yourself.