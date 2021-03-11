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

**You must define the dynamic import at the module level, or it will fail!**

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

let asyncComponent : JS.Promise<unit -> ReactElement> = JsInterop.importDynamic "./CodeSplitting.fs"

let codeSplitting = React.functionComponent(fun () ->
    Html.div [
        prop.children [
            myNonCodeSplitComponent()
            React.suspense([
                Html.div [
                    React.lazy'(asyncComponent,())
                ]
            ], centeredSpinner)
        ]
    ])
```

This doesn't appear to be doing anything, this is because the file loads so quickly. To get
a better unstanding of what's happening we can map the `JS.Promise` to add a delay:

```fsharp:code-splitting-delayed

...
let codeSplittingDelayed = React.functionComponent(fun () ->
    Html.div [
        prop.children [
            myNonCodeSplitComponent()
            React.suspense([
                Html.div [
                    React.lazy'((fun () ->
                        promise {
                            do! Promise.sleep 2000
                            return! asyncComponent
                        }
                    ),())
                ]
            ], centeredSpinner)
        ]
    ])
```

See the [React docs] for more information about code splitting.

[React docs]:https://reactjs.org/docs/code-splitting.html
