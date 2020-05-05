# Code Splitting

There are two components involved when implementing code splitting: `React.lazy'` and `React.suspense`.

#### React.lazy'

This function allows you to dynamically import a component from another file asynchronously.

The benefit of this is it allows your bundler (such as webpack) to keep those files separate. The 
benefit of this is that you get faster initial renders in your application.

#### React.suspense

This component allows you to provide children that if they are loading an element will be rendered
as a placeholder while they load. 

This works for anything in the child tree, so you could for example put `suspense` at the top of your
application if you were so inclined.

In summary, place `suspense` where you want to see the loading indicator and `lazy'` where you want
to do the code splitting.

#### Configuration

You will need to do some minor pre-build steps to get this to work.

What's recommended is to call your dynamic imports from an empty temp directory. When you are ready
to build your application you will want to make a call like: 

```bash
fable-splitter ./docs/App.fsproj -o ./docs/temp --allFiles
```

which you can place in your `package.json` to make easier:

```json
{
    ...
    "scripts": {
        "compile-code-splitting": "fable-splitter ./docs/App.fsproj -o ./docs/temp --allFiles",
        // With NPM
        "build": "npm run-script compile-code-splitting && webpack --mode production"
        // With Yarn
        "build": "yarn compile-code-splitting && webpack --mode production"
    },
    ...
}
```

> You will need a step like this for each project that uses these features.

```fsharp:code-splitting
// CodeSplitting.fs
module CodeSplitting

open Fable.Core.JsInterop
open Feliz

let myCodeSplitComponent = React.functionComponent(fun () ->
    Html.div [
        prop.text "I was loaded asynchronously!"
    ])

exportDefault myCodeSplitComponent

// Example.fs
let myNonCodeSplitComponent = React.functionComponent(fun () ->
    Html.div [
        prop.text "I was loaded synchronously!"
    ])

let centeredSpinner =
    Html.div [
        prop.style [
            style.textAlign.center
            style.marginLeft length.auto
            style.marginRight length.auto
            style.marginTop 50
        ]
        prop.children [
            Html.li [
                prop.className [
                    FA.Fa
                    FA.FaRefresh
                    FA.FaSpin
                    FA.Fa3X
                ]
            ]
        ]
    ]

let codeSplitting = React.functionComponent(fun () ->
    Html.div [
        prop.children [
            myNonCodeSplitComponent()
            React.suspense([
                Html.div [
                    React.lazy'((fun () -> 
                        promise { 
                            do! Promise.sleep 2000
                            return! JsInterop.importDynamic "./temp/CodeSplitting.js"
                        }
                    ),())
                ]
            ], centeredSpinner)
        ]
    ])
```

See the [React docs] for more information about code splitting.

[React docs]:https://reactjs.org/docs/code-splitting.html
