namespace Feliz

open System
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types

module internal ReactInterop =
    let useEffectWithDeps (effect:  obj) (deps: obj) : unit = import "useEffectWithDeps" "./ReactInterop.js"
    let useEffect (effect: obj) : unit =  import "useEffect" "./ReactInterop.js"
    let useLayoutEffectWithDeps (effect:  obj) (deps: obj) : unit = import "useLayoutEffectWithDeps" "./ReactInterop.js"
    let useLayoutEffect (effect: obj) : unit =  import "useLayoutEffect" "./ReactInterop.js"
    let useDebugValueWithFormatter<'t>(value: 't, formatter: 't -> string) : unit = import "useDebugValue" "./ReactInterop.js"

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
            renderElement: 'props -> ReactElement,
            ?name: string,
            ?withKey: 'props -> string
        )
        : 'props -> Fable.React.ReactElement =
            name |> Option.iter (fun name -> renderElement?displayName <- name)
            fun props ->
                let props = props |> propsWithKey withKey
                Interop.reactApi.createElement(renderElement, props)
    static member
        memo
        (
            renderElement: 'props -> ReactElement,
            ?name: string,
            ?areEqual: 'props -> 'props -> bool,
            ?withKey: 'props -> string
        )
        : 'props -> Fable.React.ReactElement =
            let memoElementType = Interop.reactApi.memo(renderElement, (defaultArg areEqual (unbox null)))
            name |> Option.iter (fun name -> memoElementType?displayName <- name)
            fun props ->
                let props = props |> propsWithKey withKey
                Interop.reactApi.createElement(memoElementType, props)

type React =
    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline fragment xs = Fable.React.Helpers.fragment [] xs
    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline keyedFragment(key: int, xs) = Fable.React.Helpers.fragment [ !!("key", key) ] xs
    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline keyedFragment(key: string, xs) = Fable.React.Helpers.fragment [ !!("key", key) ] xs
    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline keyedFragment(key: System.Guid, xs) = Fable.React.Helpers.fragment [ !!("key", string key) ] xs
    /// The `useState` hook that create a state variable for React function components.
    static member useState<'t>(initial: 't) = Interop.reactApi.useState(initial)
    static member useReducer(update, initialState) = Interop.reactApi.useReducer update initialState
    /// The `useEffect` hook that creates a disposable effect for React function components
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useEffect(disposableEffect, [| |])`.
    static member useEffect(effect: unit -> IDisposable) : unit = ReactInterop.useEffect(effect)
    /// The `useEffect` hook that creates a disposable effect for React function components.
    /// This effect takes a array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    static member useEffect(effect: unit -> IDisposable, dependencies: obj []) : unit = ReactInterop.useEffectWithDeps effect dependencies
    /// The `useLayoutEffect` hook that creates a disposable effect for React function components
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useLayoutEffect(disposableEffect, [| |])`.
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    static member useLayoutEffect(effect: unit -> IDisposable) : unit = ReactInterop.useLayoutEffect(effect)
    /// The `useLayoutEffect` hook that creates a disposable effect for React function components.
    /// This effect takes a array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useLayoutEffect(effect, [| |])`.
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    static member useLayoutEffect(effect: unit -> IDisposable, dependencies: obj []) : unit = ReactInterop.useLayoutEffectWithDeps effect dependencies
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    /// This effect is executed on every (re)render
    static member useLayoutEffect(effect: unit -> unit) = 
        ReactInterop.useLayoutEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    static member useLayoutEffect(effect: unit -> unit, dependencies: obj []) = 
        ReactInterop.useLayoutEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    static member useLayoutEffectOnce(effect: unit -> unit) = 
         React.useLayoutEffect(effect, [| |])

    static member useLayoutEffectOnce(effect: unit -> IDisposable) = 
        React.useLayoutEffect(effect, [| |])


    /// Creates a disposable instance by providing the implementation of the dispose member
    static member createDisposable(dispose: unit -> unit) =
        { new IDisposable with member this.Dispose() = dispose() }

    /// React hook to define and use an effect only once when a function component renders for the first time.
    /// This an alias for `React.useEffect(effect, [| |])` which explicitly provide an empty array for the dependencies of the effect which means the effect will only run once.
    static member useEffectOnce(effect: unit -> unit) =
        React.useEffect(effect, [| |])

    /// React hook to define and use a disposable effect only once when a function component renders for the first time.
    /// This an alias for `React.useEffect(effect, [| |])` which explicitly provide an empty array for the dependencies of the effect which means the effect will only run once.
    static member useEffectOnce(effect: unit -> IDisposable) =
        React.useEffect(effect, [| |])
    /// The `useEffect` hook that creates an effect for React function components.
    /// This effect is executed *every time* the function component re-renders.
    ///
    /// To make the effect run only once, write: `React.useEffect(effect, [| |])` which explicitly states
    /// that this effect has no dependencies and should only run once on initial render.
    static member useEffect(effect: unit -> unit) : unit =
        ReactInterop.useEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    /// The `useEffect` hook that creates an effect for React function components. This effect takes a array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    static member useEffect(effect: unit -> unit, dependencies: obj []) : unit =
        ReactInterop.useEffectWithDeps
            (fun _ ->
                effect()
                React.createDisposable(ignore))
            dependencies

    /// Can be used to display a label for custom hooks in React DevTools.
    static member useDebugValue(value: string) = 
        ReactInterop.useDebugValueWithFormatter(value, id)

    /// Can be used to display a label for custom hooks in React DevTools.
    static member useDebugValue(value: 't, formatter: 't -> string) = 
        ReactInterop.useDebugValueWithFormatter(value, formatter)

    /// <summary>
    /// The `useCallback` hook. Returns a memoized callback. Pass an inline callback and an array of dependencies.
    /// `useCallback` will return a memoized version of the callback that only changes if one of the dependencies has changed.
    /// </summary>
    /// <param name='callbackFunction'>A callback function to be memoized.</param>
    /// <param name='dependencies'>An array of dependencies upon which the callback function depends.
    /// If not provided, defaults to empty array, representing dependencies that never change.</param>
    static member useCallback(callbackFunction: 'a -> 'b, ?dependencies: obj array) =
        Interop.reactApi.useCallback callbackFunction (defaultArg dependencies [||])

    /// Returns a mutable ref object whose .current property is initialized to the passed argument (initialValue). The returned object will persist for the full lifetime of the component.
    ///
    /// Essentially, useRef is like a container that can hold a mutable value in its .current property.
    static member useRef(initialValue) = Interop.reactApi.useRef(initialValue)

    /// A specialized version of React.useRef() that creates a reference to an input element.
    ///
    /// Useful for controlling the internal properties and methods that element, for example to enable focus().
    static member useInputRef() : IRefValue<HTMLInputElement option> = React.useRef(None)

    /// A specialized version of React.useRef() that creates a reference to a button element.
    static member useButtonRef() : IRefValue<HTMLButtonElement option> = React.useRef(None)

    /// A specialized version of React.useRef() that creates a reference to a generic HTML element.
    ///
    /// Useful for controlling the internal properties and methods that element, for integration with third-party libraries that require a Html element.
    static member useElementRef() : IRefValue<HTMLElement option> = React.useRef(None)

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
    static member functionComponent(render: 'props -> ReactElement) =
        Internal.functionComponent(render)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(name: string, render: 'props -> ReactElement) =
        Internal.functionComponent(render, name)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(withKey: 'props -> string, render: 'props -> ReactElement) =
        Internal.functionComponent(render, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member functionComponent(name: string, withKey: 'props -> string, render: 'props -> ReactElement) =
        Internal.functionComponent(render, name, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(render: 'props -> #seq<ReactElement>) =
        Internal.functionComponent(render >> React.fragment)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(name: string, render: 'props -> #seq<ReactElement>) =
        Internal.functionComponent(render >> React.fragment, name)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.functionComponent(render >> React.fragment, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(name: string, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.functionComponent(render >> React.fragment, name, withKey=withKey)

    //
    // React.memo
    //

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(render: 'props -> ReactElement) =
        Internal.memo(render)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, render: 'props -> ReactElement) =
        Internal.memo(render, name)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(areEqual: 'props -> 'props -> bool, render: 'props -> ReactElement) =
        Internal.memo(render, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(withKey: 'props -> string, render: 'props -> ReactElement) =
        Internal.memo(render, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, render: 'props -> ReactElement) =
        Internal.memo(render, name, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(name: string, withKey: 'props -> string, render: 'props -> ReactElement) =
        Internal.memo(render, name, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    static member memo(areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> ReactElement) =
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
    static member memo(name: string, areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> ReactElement) =
        Internal.memo(render, name, areEqual=areEqual, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, name)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(areEqual: 'props -> 'props -> bool, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, name, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, name, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, areEqual=areEqual, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> React.fragment, name, areEqual=areEqual, withKey=withKey)

    //
    // React.useContext
    //

    /// <summary>
    /// Creates a Context object. When React renders a component that subscribes to this Context object
    /// it will read the current context value from the closest matching Provider above it in the tree.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='defaultValue'>A default value that is only used when a component does not have a matching Provider above it in the tree.</param>
    static member createContext<'a>(?name: string, ?defaultValue: 'a) =
        let contextObject = Interop.reactApi.createContext (defaultArg defaultValue Fable.Core.JS.undefined<'a>)
        name |> Option.iter (fun name -> contextObject?displayName <- name)
        contextObject

    /// <summary>
    /// A Provider component that allows consuming components to subscribe to context changes.
    /// </summary>
    /// <param name='contextObject'>A context object returned from a previous React.createContext call.</param>
    /// <param name='contextValue'>The context value to be provided to descendant components.</param>
    /// <param name='child'>A child element.</param>
    static member contextProvider(contextObject: Fable.React.IContext<'a>, contextValue: 'a, child: ReactElement) : ReactElement =
        Interop.reactApi.createElement(contextObject?Provider, createObj ["value" ==> contextValue], [child])
    /// <summary>
    /// A Provider component that allows consuming components to subscribe to context changes.
    /// </summary>
    /// <param name='contextObject'>A context object returned from a previous React.createContext call.</param>
    /// <param name='contextValue'>The context value to be provided to descendant components.</param>
    /// <param name='children'>A sequence of child elements.</param>
    static member contextProvider(contextObject: Fable.React.IContext<'a>, contextValue: 'a, children: #seq<ReactElement>) : ReactElement =
        Interop.reactApi.createElement(contextObject?Provider, createObj ["value" ==> contextValue], children)

    /// <summary>
    /// A Consumer component that subscribes to context changes.
    /// </summary>
    /// <param name='contextObject'>A context object returned from a previous React.createContext call.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member contextConsumer(contextObject: Fable.React.IContext<'a>, render: 'a -> ReactElement) : ReactElement =
        Interop.reactApi.createElement(contextObject?Consumer, null, [!!render])
    /// <summary>
    /// A Consumer component that subscribes to context changes.
    /// </summary>
    /// <param name='contextObject'>A context object returned from a previous React.createContext call.</param>
    /// <param name='render'>A render function that returns a sequence of elements.</param>
    static member contextConsumer(contextObject: Fable.React.IContext<'a>, render: 'a -> #seq<ReactElement>) : ReactElement =
        Interop.reactApi.createElement(contextObject?Consumer, null, [!!(render >> React.fragment)])

    /// <summary>
    /// The `useContext` hook. Accepts a context object (the value returned from React.createContext) and returns the current context value for that context.
    /// The current context value is determined by the value prop of the nearest Provider component above the calling component in the tree.
    /// </summary>
    /// <param name='contextObject'>A context object returned from a previous React.createContext call.</param>
    static member useContext(contextObject: Fable.React.IContext<'a>) = Interop.reactApi.useContext contextObject

    /// <summary>
    /// Creates a callback that keeps the same reference during the entire lifecycle of the component while having access to
    /// the current variables on every call.
    ///
    /// This hook should only be used for functions that are not used to provide information during render (such as a dispatch function).
    /// </summary>
    /// <param name='callback'>The function call.</param>
    static member useCallbackStatic (callback: ('a -> 'b)) =
        let lastRenderCallbackRef = React.useRef(callback)
        
        let staticCallback = 
            React.useCallback((fun (arg: 'a) ->
                lastRenderCallbackRef.current(arg)
            ), [||])

        React.useLayoutEffect(fun () ->
            // render is commited - it's safe to update the callback
            lastRenderCallbackRef.current <- callback
        )

        staticCallback
