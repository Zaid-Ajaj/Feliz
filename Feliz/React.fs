namespace Feliz

open System
open System.ComponentModel
open Fable.Core
open Fable.Core.JsInterop
open Browser.Types

module internal ReactInterop =
    let useDebugValueWithFormatter<'t>(value: 't, formatter: 't -> string) : unit = import "useDebugValue" "./ReactInterop.js"
    let useEffect (effect: obj) : unit =  import "useEffect" "./ReactInterop.js"
    let useEffectWithDeps (effect:  obj) (deps: obj) : unit = import "useEffectWithDeps" "./ReactInterop.js"
    let useLayoutEffect (effect: obj) : unit =  import "useLayoutEffect" "./ReactInterop.js"
    let useLayoutEffectWithDeps (effect:  obj) (deps: obj) : unit = import "useLayoutEffectWithDeps" "./ReactInterop.js"

[<EditorBrowsable(EditorBrowsableState.Never);Erase>]
[<RequireQualifiedAccess>]
module Helpers =
    let inline optDispose (disposeOption: #IDisposable option) =
        { new IDisposable with member _.Dispose () = disposeOption |> Option.iter (fun d -> d.Dispose()) }

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
            #if FABLE_COMPILER_3
            Browser.Dom.console.warn("Feliz: using React.functionComponent in Fable 3 is obsolete, please consider using the [<ReactComponent>] attribute instead which makes Feliz output better Javascript code that is compatible with react-refresh")
            #endif
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
            name |> Option.iter (fun name -> renderElement?displayName <- name)
            fun props ->
                let props = props |> propsWithKey withKey
                Interop.reactApi.createElement(memoElementType, props)

type React =
    /// Creates a disposable instance by providing the implementation of the dispose member.
    static member createDisposable(dispose: unit -> unit) =
        { new IDisposable with member _.Dispose() = dispose() }

    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline fragment xs =
        Fable.React.ReactBindings.React.createElement(Fable.React.ReactBindings.React.Fragment, obj(), xs)

    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline keyedFragment(key: int, xs) = // Fable.React.Helpers.fragment [ !!("key", key) ] xs
        Fable.React.ReactBindings.React.createElement(Fable.React.ReactBindings.React.Fragment, createObj ["key" ==> key], xs)

    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline keyedFragment(key: string, xs) =
        Fable.React.ReactBindings.React.createElement(Fable.React.ReactBindings.React.Fragment, createObj ["key" ==> key], xs)

    /// The `React.fragment` component lets you return multiple elements in your `render()` method without creating an additional DOM element.
    static member inline keyedFragment(key: System.Guid, xs) =
        Fable.React.ReactBindings.React.createElement(Fable.React.ReactBindings.React.Fragment, createObj ["key" ==> string key], xs)

    /// Placeholder empty React element to be used when importing external React components with the [<ReactComponent>] attribute.
    static member inline imported() = Html.none

    /// The `useState` hook that creates a state variable for React function components from an initialization function.
    [<Hook>]
    static member useState<'t>(initializer: unit -> 't) = Interop.reactApi.useState<unit -> 't,'t>(initializer)

    /// Accepts a reducer and returns the current state paired with a dispatch.
    [<Hook>]
    static member useReducer(update, initialState) = Interop.reactApi.useReducer update initialState

    /// The `useEffect` hook that creates a disposable effect for React function components.
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useEffect(disposableEffect, [| |])`.
    [<Hook>]
    static member useEffect(effect: unit -> #IDisposable) : unit = ReactInterop.useEffect(effect)
    /// The `useEffect` hook that creates a disposable effect for React function components.
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useEffect(disposableEffect, [| |])`.
    [<Hook>]
    static member inline useEffect(effect: unit -> #IDisposable option) = React.useEffect(effect >> Helpers.optDispose)
    /// The `useEffect` hook that creates a disposable effect for React function components.
    /// This effect takes an array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    [<Hook>]
    static member useEffect(effect: unit -> #IDisposable, dependencies: obj []) : unit = ReactInterop.useEffectWithDeps effect dependencies
    /// The `useEffect` hook that creates a disposable effect for React function components.
    /// This effect takes an array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    [<Hook>]
    static member inline useEffect(effect: unit -> #IDisposable option, dependencies: obj []) = React.useEffect(effect >> Helpers.optDispose, dependencies)

    /// The `useLayoutEffect` hook that creates a disposable effect for React function components.
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useLayoutEffect(disposableEffect, [| |])`.
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    [<Hook>]
    static member useLayoutEffect(effect: unit -> #IDisposable) : unit = ReactInterop.useLayoutEffect(effect)
    /// The `useLayoutEffect` hook that creates a disposable effect for React function components.
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useLayoutEffect(disposableEffect, [| |])`.
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    [<Hook>]
    static member inline useLayoutEffect(effect: unit -> #IDisposable option) = React.useLayoutEffect(effect >> Helpers.optDispose)
    /// The `useLayoutEffect` hook that creates a disposable effect for React function components.
    /// This effect takes an array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useLayoutEffect(effect, [| |])`.
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    [<Hook>]
    static member useLayoutEffect(effect: unit -> #IDisposable, dependencies: obj []) : unit = ReactInterop.useLayoutEffectWithDeps effect dependencies
    /// The `useLayoutEffect` hook that creates a disposable effect for React function components.
    /// This effect takes an array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useLayoutEffect(effect, [| |])`.
    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    [<Hook>]
    static member inline useLayoutEffect(effect: unit -> #IDisposable option, dependencies: obj []) =
        React.useLayoutEffect(effect >> Helpers.optDispose, dependencies)

    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    /// This effect is executed on every (re)render
    [<Hook>]
    static member useLayoutEffect(effect: unit -> unit) =
        ReactInterop.useLayoutEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    /// The signature is identical to useEffect, but it fires synchronously after all DOM mutations. Use this to read layout from the DOM and synchronously re-render. Updates scheduled inside useLayoutEffect will be flushed synchronously, before the browser has a chance to paint.
    [<Hook>]
    static member useLayoutEffect(effect: unit -> unit, dependencies: obj []) =
        ReactInterop.useLayoutEffectWithDeps
            (fun _ ->
                effect()
                React.createDisposable(ignore))
            dependencies

    [<Hook>]
    static member inline useLayoutEffectOnce(effect: unit -> unit) =
         React.useLayoutEffect(effect, [| |])

    [<Hook>]
    static member inline useLayoutEffectOnce(effect: unit -> #IDisposable) =
        React.useLayoutEffect(effect, [| |])

    [<Hook>]
    static member inline useLayoutEffectOnce(effect: unit -> #IDisposable option) =
        React.useLayoutEffect(effect, [| |])

    /// React hook to define and use an effect only once when a function component renders for the first time.
    /// This is an alias for `React.useEffect(effect, [| |])` which explicitly provides an empty array for the dependencies of the effect which means the effect will only run once.
    [<Hook>]
    static member useEffectOnce(effect: unit -> unit) =
        let calledOnce = Interop.reactApi.useRefInternal false
                
        React.useEffect (fun () -> 
            if calledOnce.current 
            then ()
            else
                calledOnce.current <- true
                effect()
        , [||])

    /// React hook to define and use a disposable effect only once when a function component renders for the first time.
    /// This is an alias for `React.useEffect(effect, [| |])` which explicitly provides an empty array for the dependencies of the effect which means the effect will only run once.
    [<Hook>]
    static member useEffectOnce(effect: unit -> #IDisposable) =
        let destroyFunc = Interop.reactApi.useRefInternal None
        let calledOnce = Interop.reactApi.useRefInternal false
        let renderAfterCalled = Interop.reactApi.useRefInternal false

        if calledOnce.current then
            renderAfterCalled.current <- true

        React.useEffect (fun () -> 
            if calledOnce.current 
            then None
            else
                calledOnce.current <- true
                destroyFunc.current <- effect() |> Some

                if renderAfterCalled.current
                then destroyFunc.current
                else None
        , [||])

    /// React hook to define and use a disposable effect only once when a function component renders for the first time.
    /// This is an alias for `React.useEffect(effect, [| |])` which explicitly provide an empty array for the dependencies of the effect which means the effect will only run once.
    [<Hook>]
    static member useEffectOnce(effect: unit -> #IDisposable option) =
        let destroyFunc = Interop.reactApi.useRefInternal None
        let calledOnce = Interop.reactApi.useRefInternal false
        let renderAfterCalled = Interop.reactApi.useRefInternal false

        if calledOnce.current then
            renderAfterCalled.current <- true

        React.useEffect (fun () -> 
            if calledOnce.current 
            then None
            else
                calledOnce.current <- true
                destroyFunc.current <- effect()

                if renderAfterCalled.current
                then destroyFunc.current
                else None
        , [||])

    /// The `useEffect` hook that creates an effect for React function components.
    /// This effect is executed *every time* the function component re-renders.
    ///
    /// To make the effect run only once, write: `React.useEffect(effect, [| |])` which explicitly states
    /// that this effect has no dependencies and should only run once on initial render.
    [<Hook>]
    static member useEffect(effect: unit -> unit) : unit =
        ReactInterop.useEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    /// The `useEffect` hook that creates an effect for React function components. This effect takes an array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    [<Hook>]
    static member useEffect(effect: unit -> unit, dependencies: obj []) : unit =
        ReactInterop.useEffectWithDeps
            (fun _ ->
                effect()
                React.createDisposable(ignore))
            dependencies

    /// Can be used to display a label for custom hooks in React DevTools.
    [<Hook>]
    static member useDebugValue(value: string) =
        ReactInterop.useDebugValueWithFormatter(value, id)

    /// Can be used to display a label for custom hooks in React DevTools.
    [<Hook>]
    static member useDebugValue(value: 't, formatter: 't -> string) =
        ReactInterop.useDebugValueWithFormatter(value, formatter)

    /// <summary>
    /// The `useCallback` hook. Returns a memoized callback. Pass an inline callback and an array of dependencies.
    /// `useCallback` will return a memoized version of the callback that only changes if one of the dependencies has changed.
    /// </summary>
    /// <param name='callbackFunction'>A callback function to be memoized.</param>
    /// <param name='dependencies'>An array of dependencies upon which the callback function depends.
    /// If not provided, defaults to empty array, representing dependencies that never change.</param>
    [<Hook>]
    static member useCallback(callbackFunction: 'a -> 'b, ?dependencies: obj array) =
        Interop.reactApi.useCallback callbackFunction (defaultArg dependencies [||])

    /// Returns a mutable ref object whose .current property is initialized to the passed argument (initialValue). The returned object will persist for the full lifetime of the component.
    ///
    /// Essentially, useRef is like a container that can hold a mutable value in its .current property.
    [<Hook>]
    static member useRef<'t>(initialValue: 't) = Interop.reactApi.useRefInternal(initialValue)

    /// A specialized version of React.useRef() that creates a reference to an input element.
    ///
    /// Useful for controlling the internal properties and methods of that element, for example to enable focus().
    [<Hook>]
    static member useInputRef() : IRefValue<HTMLInputElement option> = React.useRef(None)

    /// A specialized version of React.useRef() that creates a reference to a button element.
    [<Hook>]
    static member useButtonRef() : Fable.React.IRefValue<HTMLButtonElement option> = React.useRef(None)

    /// A specialized version of React.useRef() that creates a reference to a generic HTML element.
    ///
    /// Useful for controlling the internal properties and methods of that element, for integration with third-party libraries that require a Html element.
    [<Hook>]
    static member useElementRef() : IRefValue<HTMLElement option> = React.useRef(None)

    /// <summary>
    /// The `useMemo` hook. Returns a memoized value. Pass a "create" function and an array of dependencies.
    /// `useMemo` will only recompute the memoized value when one of the dependencies has changed.
    /// </summary>
    /// <param name='createFunction'>A create function returning a value to be memoized.</param>
    /// <param name='dependencies'>An array of dependencies upon which the create function depends.
    /// If not provided, defaults to empty array, representing dependencies that never change.</param>
    [<Hook>]
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
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    static member functionComponent(render: 'props -> ReactElement, ?withKey: 'props -> string) =
        Internal.functionComponent(render, ?withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns an element.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    [<Obsolete "React.functionComponent is obsolete. Use [<ReactComponent>] attribute to automatically convert them to React components">]
    static member functionComponent(name: string, render: 'props -> ReactElement, ?withKey: 'props -> string) =
        Internal.functionComponent(render, name, ?withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    [<Obsolete "React.functionComponent is obsolete. Use [<ReactComponent>] attribute to automatically convert them to React components">]
    static member functionComponent(render: 'props -> #seq<ReactElement>, ?withKey: 'props -> string) =
        Internal.functionComponent(render >> React.fragment, ?withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    [<Obsolete "React.functionComponent is obsolete. Use [<ReactComponent>] attribute to automatically convert them to React components">]
    static member functionComponent(name: string, render: 'props -> #seq<ReactElement>, ?withKey: 'props -> string) =
        Internal.functionComponent(render >> React.fragment, name, ?withKey=withKey)

    //
    // React.memo
    //

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    static member memo(render: 'props -> ReactElement, ?withKey: 'props -> string, ?areEqual: 'props -> 'props -> bool) =
        Internal.memo(render, ?areEqual=areEqual, ?withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function or a React.functionComponent.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    static member memo(name: string, render: 'props -> ReactElement, ?withKey: 'props -> string, ?areEqual: 'props -> 'props -> bool) =
        Internal.memo(render, name, ?areEqual=areEqual, ?withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='render'>A render function that returns a list of elements.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    static member memo(render: 'props -> #seq<ReactElement>, ?withKey: 'props -> string, ?areEqual: 'props -> 'props -> bool) =
        Internal.memo(render >> React.fragment, ?areEqual=areEqual, ?withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    static member memo(name: string, render: 'props -> #seq<ReactElement>, ?withKey: 'props -> string, ?areEqual: 'props -> 'props -> bool) =
        Internal.memo(render >> React.fragment, name, ?areEqual=areEqual, ?withKey=withKey)

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
    [<Hook>]
    static member useContext(contextObject: Fable.React.IContext<'a>) = Interop.reactApi.useContext contextObject

    /// <summary>
    /// Creates a callback that keeps the same reference during the entire lifecycle of the component while having access to
    /// the current value of the dependencies on every call.
    ///
    /// This hook should only be used for (like a dispatch) functions that are not used to provide information during render.
    ///
    /// This is not a complete replacement for the `useCallback` hook. It returns a callback that does not need explicit
    /// dependency declarations and never causes a re-render.
    /// </summary>
    /// <param name='callback'>The function call.</param>
    [<Hook>]
    static member useCallbackRef(callback: ('a -> 'b)) =
        let lastRenderCallbackRef = React.useRef(callback)

        let callbackRef =
            React.useCallback((fun (arg: 'a) ->
                lastRenderCallbackRef.current(arg)
            ), [||])

        React.useLayoutEffect(fun () ->
            // render is commited - it's safe to update the callback
            lastRenderCallbackRef.current <- callback
        )

        callbackRef

    /// <summary>
    /// Just like React.useState except that the updater function uses the previous state of the state variable as input and allows you to compute the next value using it.
    /// This is useful in cases where defining helpers functions inside the definition of a React function component would actually cache the initial value (because they become closures) during first render as opposed to using the current value after multiple render cycles.
    ///
    /// Use this instead of React.useState when your state variable is a list, an array, a dictionary, a map or other complex structures.
    /// </summary>
    static member useStateWithUpdater (initial: 't) : ('t * (('t -> 't) -> unit)) = import "useState" "react"

    /// <summary>
    /// Forwards a given ref, allowing you to pass it further down to a child.
    /// </summary>
    /// <param name='render'>A render function that returns an element.</param>
    static member forwardRef(render: ('props * IRefValue<'t> -> ReactElement)) : ('props * IRefValue<'t> -> ReactElement) =
        let forwardRefType = Interop.reactApi.forwardRef(Func<'props,IRefValue<'t>,ReactElement> (fun props ref -> render(props,ref)))
        fun (props, ref) ->
            let propsObj = props |> JsInterop.toPlainJsObj
            propsObj?ref <- ref
            Interop.reactApi.createElement(forwardRefType, propsObj)

    /// <summary>
    /// Forwards a given ref, allowing you to pass it further down to a child.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns an element.</param>
    static member forwardRef(name: string, render: ('props * IRefValue<'t> -> ReactElement)) : ('props * IRefValue<'t> -> ReactElement) =
        let forwardRefType = Interop.reactApi.forwardRef(Func<'props,IRefValue<'t>,ReactElement> (fun props ref -> render(props,ref)))
        render?displayName <- name
        fun (props, ref) ->
            let propsObj = props |> JsInterop.toPlainJsObj
            propsObj?ref <- ref
            Interop.reactApi.createElement(forwardRefType, propsObj)

    /// <summary>
    /// Highlights potential problems in an application by enabling additional checks
    /// and warnings for descendants. As well as double rendering function components.
    ///
    /// This *does not do anything* in production mode. You do not need to hide it
    /// with compiler directives.
    /// </summary>
    /// <param name='children'>The elements that will be rendered with additional
    /// checks and warnings.</param>
    static member strictMode(children: ReactElement list) =
        Interop.reactApi.createElement(Interop.reactApi.StrictMode, None, children)

    /// <summary>
    /// Lets you define a component that is loaded dynamically. Which helps with code splitting.
    /// </summary>
    /// <param name='dynamicImport'>
    ///  The dynamicImport of the component.
    ///
    ///  Such as `let asyncComponent : JS.Promise[unit -> ReactElement] = JsInterop.importDynamic "./CodeSplitting.fs"`.
    ///
    ///  Where you would then pass in `asyncComponent`.
    /// </param>
    /// <param name="props">The props to be passed to the component.</param>
    static member lazy'<'t,'props>(dynamicImport: JS.Promise<'t>, props: 'props) =
        Interop.reactApi.createElement(Interop.reactApi.lazy'(fun () -> dynamicImport),props)
    /// <summary>
    /// Lets you define a component that is loaded dynamically. Which helps with code
    /// splitting.
    /// </summary>
    /// <param name='dynamicImport'>
    /// The dynamicImport of the component.
    ///
    /// Such as `let asyncComponent : JS.Promise[unit -> ReactElement] = JsInterop.importDynamic "./CodeSplitting.fs"`.
    ///
    /// Where you would then pass in `fun () -> asyncComponent`.
    /// </param>
    /// <param name="props">The props to be passed to the component.</param>
    static member lazy'<'t,'props>(dynamicImport: unit -> JS.Promise<'t>, props: 'props) =
        Interop.reactApi.createElement(Interop.reactApi.lazy'(dynamicImport),props)

    /// <summary>
    /// Lets you specify a loading indicator whenever a child element is not yet ready
    /// to render.
    ///
    /// Currently this is only usable with `React.lazy'`.
    /// </summary>
    /// <param name='children'>The elements that will be rendered within the suspense block.</param>
    static member suspense(children: ReactElement list) =
        Interop.reactApi.createElement(Interop.reactApi.Suspense, {| fallback = Html.none |} |> JsInterop.toPlainJsObj, children)
    /// <summary>
    /// Lets you specify a loading indicator whenever a child element is not yet ready
    /// to render.
    ///
    /// Currently this is only usable with `React.lazy'`.
    /// </summary>
    /// <param name='children'>The elements that will be rendered within the suspense block.</param>
    /// <param name='fallback'>The element that will be rendered while the children are loading.</param>
    static member suspense(children: ReactElement list, fallback: ReactElement) =
        Interop.reactApi.createElement(Interop.reactApi.Suspense, {| fallback = fallback |} |> JsInterop.toPlainJsObj, children)

    /// <summary>
    /// Allows you to override the behavior of a given ref.
    ///
    /// </summary>
    /// <param name='ref'>The ref you want to override.</param>
    /// <param name='createHandle'>A function that returns a new ref with changed behavior.</param>
    [<Hook>]
    static member useImperativeHandle(ref: IRefValue<'t>, createHandle: unit -> 't) =
        Interop.reactApi.useImperativeHandleNoDeps ref createHandle

    /// <summary>
    /// Lets you specify a loading indicator whenever a child element is not yet ready
    /// to render.
    ///
    /// Currently this is only usable with `React.lazy'`.
    /// </summary>
    /// <param name='ref'>The ref you want to override.</param>
    /// <param name='createHandle'>A function that returns a new ref with changed behavior.</param>
    /// <param name='dependencies'>An array of dependencies upon which the imperative handle function depends.</param>
    [<Hook>]
    static member useImperativeHandle(ref: IRefValue<'t>, createHandle: unit -> 't, dependencies: obj []) =
        Interop.reactApi.useImperativeHandle ref createHandle dependencies

    /// <summary>
    /// Creates a CancellationToken that is cancelled when a component is unmounted.
    /// </summary>
    [<Hook>]
    static member inline useCancellationToken () =
        let cts = React.useRef(new System.Threading.CancellationTokenSource())
        let token = React.useRef(cts.current.Token)

        React.useEffectOnce(fun () ->
            React.createDisposable(fun () ->
                cts.current.Cancel()
                cts.current.Dispose()
            )
        )

        token

[<AutoOpen>]
module ReactOverloadMagic =
    type React with
        /// Creates a disposable instance by merging multiple IDisposables.
        static member inline createDisposable([<ParamArray>] disposables: #IDisposable []) =
            React.createDisposable(fun () ->
                disposables
                |> Array.iter (fun d -> d.Dispose())
            )
        /// Creates a disposable instance by merging multiple IDisposable options.
        static member inline createDisposable([<ParamArray>] disposables: #IDisposable option []) =
            React.createDisposable(fun () ->
                disposables
                |> Array.iter (Option.iter (fun d -> d.Dispose()))
            )
        /// Creates a disposable instance by merging multiple IDisposable refs.
        static member inline createDisposable([<ParamArray>] disposables: IRefValue<#IDisposable> []) =
            React.createDisposable(fun () ->
                disposables
                |> Array.iter (fun d -> d.current.Dispose())
            )

        /// Creates a disposable instance by merging multiple IDisposable refs.
        static member inline createDisposable([<ParamArray>] disposables: IRefValue<#IDisposable option> []) =
            React.createDisposable(fun () ->
                disposables
                |> Array.iter (fun d -> d.current |> Option.iter (fun d -> d.Dispose()))
            )

        /// The `useState` hook that creates a state variable for React function components.
        [<Hook>]
        static member useState<'t>(initial: 't) = Interop.reactApi.useState<'t,'t>(initial)


        [<Hook>]
        static member useStateWithUpdater<'t>(initializer: unit -> 't): ('t * (('t -> 't) -> unit)) = import "useState" "react"
