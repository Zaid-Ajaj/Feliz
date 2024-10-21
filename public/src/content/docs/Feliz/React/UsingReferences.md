### Using References

Within function components, React allows us to use the so-called references. There references are just like state variables except that changes in their (mutable) values do not cause a re-render in the function component lifetime. These mutable values can be used to hold a reference to DOM elements and manipulate it on a low-level as opposed to using the declarative API. For example, you can use the focus of an input element by a click of a button:
```fs:focus-input-example
[<ReactComponent>]
let FullFocusInputExample() =
    // obtain a reference
    let inputRef = React.useRef(None)

    let focusTextInput() =
        match inputRef.current with
        | None -> ()
        | Some element ->
            let inputElement = unbox<HTMLInputElement> element
            inputElement.focus()

    Html.div [
        Html.input [
            prop.ref inputRef
            prop.type'.text
        ]

        Html.button [
            prop.onClick (fun _ -> focusTextInput())
            prop.text "Focus Input"
        ]
    ]
```
Here we use the `React.useRef` hook and provide it an initial value of `None` to get our `inputRef` which is of type `IRefValue<'t option>` at that point. Once you give `inputRef` to the input property `prop.ref`, the type of `inputRef` becomes more concrete and turns into `IRefValue<HTMLElement option>`. When we click on the button, the function `focusTextInput()` executes and makes the input element focused using the `focus()` function on that element but only after we unbox it into `HTMLInputElement` because otherwise a generic `HTMLElement` doesn't have that function.

> `IRefValue<'T>` is defined as container type which has a settable property `current : 'T`

This is how `React.useRef` is used in modern React apps but it is a bit involved and you have to know the types of the elements. That is why I created three specialized variants of `React.useRef` to work with Html elements:
```fs
React.useInputRef : unit -> IRefValue<HTMLInputElement option>
React.useButtonRef : unit -> IRefValue<HTMLButtonElement option>
// the more generic version that works with any Html element
React.useElementRef : unit -> IRefValue<HTMLElement option>
```
Now the above example can be simplified into:
```fs
[<ReactComponent>]
let FocusInputExample() =
    let inputRef = React.useInputRef()
    let focusTextInput() = inputRef.current |> Option.iter (fun inputElement -> inputElement.focus())

    Html.div [
        Html.input [
            prop.ref inputRef
            prop.type'.text
        ]

        Html.button [
            prop.onClick (fun _ -> focusTextInput())
            prop.text "Focus Input"
        ]
    ]
```

#### Forwarding Refs

You can also use `React.forwardRef` for passing refs to children components.

In most cases the above examples will suit your needs, but in some use-cases such as creating libraries for reusable components, it can be useful.

For more information see the React docs on [forwarding refs](https://reactjs.org/docs/forwarding-refs.html).

```fs:forward-ref-example
let forwardRefChild = React.forwardRef(fun ((), ref) ->
    Html.input [
        prop.type'.text
        prop.ref ref
    ])

let forwardRefParent = React.functionComponent(fun () ->
    let inputRef = React.useInputRef()

    Html.div [
        forwardRefChild((), inputRef)
        Html.button [
            prop.text "Focus Input"
            prop.onClick <| fun ev ->
                inputRef.current
                |> Option.iter (fun elem -> elem.focus())
        ]
    ])
```

#### Overriding behavior with useImperativeHandle

The hook `React.useImperativeHandle` allows you to override the behavior of the given `ref`.

> This should be used in conjunction with `React.forwardRef`.

**You should try to avoid using this when possible.**

In this example we override the behavior of `focus` to instead set the text value of a div:

```fs:use-imperative-handle
let forwardRefImperativeChild = React.forwardRef(fun ((), ref) ->
    let divText,setDivText = React.useState ""
    let inputRef = React.useInputRef()

    React.useImperativeHandle(ref, fun () ->
        inputRef.current
        |> Option.map(fun innerRef ->
            {| focus = fun () -> setDivText innerRef.className |})
    )

    Html.div [
        Html.input [
            prop.className "Howdy!"
            prop.type'.text
            prop.ref inputRef
        ]
        Html.div [
            prop.text divText
        ]
    ])

let forwardRefImperativeParent = React.functionComponent(fun () ->
    let ref = React.useRef<{| focus: unit -> unit |} option>(None)

    Html.div [
        forwardRefImperativeChild((), ref)
        Html.button [
            prop.text "Focus Input"
            prop.onClick <| fun ev ->
                ref.current
                |> Option.iter (fun elem -> elem.focus())
        ]
    ])
```
