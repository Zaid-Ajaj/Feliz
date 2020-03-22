### Using References

Within function components, React allows us to use the so-called references. There references are just like state variables except that changes in their (mutable) values do not cause a re-render in the function component lifetime. These mutable values can be used to hold a reference to DOM elements and manipulate it on a low-level as opposed to using the declarative API. For example, you can use the focus of an input element by a click of a button:
```fs:focus-input-example
let fullFocusInputExample = React.functionComponent(fun () ->
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
    ])
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
let focusInputExample = React.functionComponent(fun () ->
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
    ])
```