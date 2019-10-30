namespace Feliz

open System
open Fable.Core
open Fable.Core.JsInterop

module internal ReactInterop =
    let useEffectWithDeps (effect:  obj) (deps: obj) : unit = import "useEffectWithDeps" "./ReactInterop.js"
    let useEffect (effect: obj) : unit =  import "useEffect" "./ReactInterop.js"

type internal Internal() =
    static let propsWithKey (withKey: ('props -> string) option) props =
        match withKey with
        | Some f ->
            props?key <- f props
            props
        | None -> props
    static member
        functionComponent
        (
            renderElement: 'props -> Fable.React.ReactElement,
            ?name: string,
            ?withKey: 'props -> string
        )
        : Fable.React.FunctionComponent<'props> =
            let functionElementType = Fable.React.ReactElementType.ofFunction renderElement
            name |> Option.iter (fun name -> functionElementType?displayName <- name)
            fun props ->
                let props = props |> propsWithKey withKey
                Fable.React.ReactElementType.create functionElementType props []
    static member
        memo
        (
            renderElement: 'props -> Fable.React.ReactElement,
            ?name: string,
            ?areEqual: 'props -> 'props -> bool,
            ?withKey: 'props -> string
        )
        : Fable.React.FunctionComponent<'props> =
            let memoElementType =
                match areEqual with
                | Some areEqual -> Fable.React.ReactElementType.memoWith areEqual renderElement
                | None -> Fable.React.ReactElementType.memo renderElement
            name |> Option.iter (fun name -> memoElementType?displayName <- name)
            fun props ->
                let props = props |> propsWithKey withKey
                Fable.React.ReactElementType.create memoElementType props []

type React =
    /// The `useState` hook that create a state variable for React function components.
    static member useState<'t>(initial: 't) = Interop.reactApi.useState(initial)
    static member useReducer(update, initialState) = Interop.reactApi.useReducer update initialState
    /// The `useEffect` hook that creates a disposable effect for React function components
    static member useEffect(effect: unit -> IDisposable) = ReactInterop.useEffect effect
    static member useEffect(effect: unit -> IDisposable, a: 'a) =
        ReactInterop.useEffectWithDeps effect [| a |]
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> IDisposable, a: 'a, b: 'b) =
        ReactInterop.useEffectWithDeps effect [| unbox a; unbox b |]
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> IDisposable, a: 'a, b: 'b, c: 'c) =
        ReactInterop.useEffectWithDeps effect [| unbox a; unbox b; unbox c |]
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> IDisposable, a: 'a, b: 'b, c: 'c, d: 'd) =
        ReactInterop.useEffectWithDeps effect [| unbox a; unbox b; unbox c; unbox d |]
    /// Creates a disposable instance by providing the implementation of the dispose member
    static member createDisposable(dispose: unit -> unit) =
        { new IDisposable with member this.Dispose() = dispose() }
    /// The `useEffect` hook that creates an effect for React function components.
    static member useEffect(effect: unit -> unit) =
        ReactInterop.useEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    /// <summary>
    /// The `useCallback` hook. Returns a memoized callback. Pass an inline callback and an array of dependencies.
    /// `useCallback` will return a memoized version of the callback that only changes if one of the dependencies has changed.
    /// </summary>
    /// <param name='callbackFunction'>A callback function to be memoized.</param>
    /// <param name='dependencies'>An array of dependencies upon which the callback function depends.
    /// If not provided, defaults to empty array, representing dependencies that never change.</param>
    static member useCallback(callbackFunction: 'a -> 'b, ?dependencies: obj array) =
      Interop.reactApi.useCallback callbackFunction (defaultArg dependencies [||])

    /// <summary>
    /// The `useMemo` hook. Returns a memoized value. Pass a "create" function and an array of dependencies.
    /// `useMemo` will only recompute the memoized value when one of the dependencies has changed.
    /// </summary>
    /// <param name='createFunction'>A create function returning a value to be memoized.</param>
    /// <param name='dependencies'>An array of dependencies upon which the create function depends.
    /// If not provided, defaults to empty array, representing dependencies that never change.</param>
    static member useMemo(createFunction: unit -> 'a, ?dependencies: obj array) =
      Interop.reactApi.useMemo createFunction (defaultArg dependencies [||])

    //
    // React.functionComponent
    //

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(render: 'props -> Fable.React.ReactElement) =
        Internal.functionComponent(render)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(name: string, render: 'props -> Fable.React.ReactElement) =
        Internal.functionComponent(render, name)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(withKey: 'props -> string, render: 'props -> Fable.React.ReactElement) =
        Internal.functionComponent(render, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(name: string, withKey: 'props -> string, render: 'props -> Fable.React.ReactElement) =
        Internal.functionComponent(render, name, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.functionComponent(render >> Html.fragment)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(name: string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.functionComponent(render >> Html.fragment, name)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(withKey: 'props -> string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.functionComponent(render >> Html.fragment, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(name: string, withKey: 'props -> string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.functionComponent(render >> Html.fragment, name, withKey=withKey)

    //
    // React.memo
    //

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, name)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(areEqual: 'props -> 'props -> bool, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(withKey: 'props -> string, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, name, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, withKey: 'props -> string, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, name, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, areEqual=areEqual, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> Fable.React.ReactElement) =
        Internal.memo(render, name, areEqual=areEqual, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, name)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(areEqual: 'props -> 'props -> bool, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(withKey: 'props -> string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, name, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, withKey: 'props -> string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, name, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, areEqual=areEqual, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> #seq<Fable.React.ReactElement>) =
        Internal.memo(render >> Html.fragment, name, areEqual=areEqual, withKey=withKey)
