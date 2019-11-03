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
            renderElement: 'props -> ReactElement,
            ?name: string,
            ?withKey: 'props -> string
        )
        : Fable.React.FunctionComponent<'props> =
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
        : Fable.React.FunctionComponent<'props> =
            let memoElementType = Interop.reactApi.memo(renderElement, (defaultArg areEqual (unbox null)))
            name |> Option.iter (fun name -> memoElementType?displayName <- name)
            fun props ->
                let props = props |> propsWithKey withKey
                Interop.reactApi.createElement(memoElementType, props)

type React =
    /// The `useState` hook that create a state variable for React function components.
    static member useState<'t>(initial: 't) = Interop.reactApi.useState(initial)
    static member useReducer(update, initialState) = Interop.reactApi.useReducer update initialState
    /// The `useEffect` hook that creates a disposable effect for React function components
    /// This effect has no dependencies which means the effect is re-executed on every re-render.
    /// To make the effect run once (for example you subscribe once to web sockets) then provide an empty array
    /// for the dependencies: `React.useEffect(disposableEffect, [| |])`.
    static member useEffect(effect: unit -> IDisposable) = ReactInterop.useEffect(effect)
    /// The `useEffect` hook that creates a disposable effect for React function components.
    /// This effect takes a array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    static member useEffect(effect: unit -> IDisposable, dependencies: obj []) = ReactInterop.useEffectWithDeps(effect, dependencies)
    /// Creates a disposable instance by providing the implementation of the dispose member
    static member createDisposable(dispose: unit -> unit) =
        { new IDisposable with member this.Dispose() = dispose() }

    /// The `useEffect` hook that creates an effect for React function components.
    /// This effect is executed *every time* the function component re-renders.
    ///
    /// To make the effect run only once, write: `React.useEffect(effect, [| |])` which explicitly states
    /// that this effect has no dependencies and should only run once on initial render.
    static member useEffect(effect: unit -> unit) =
        ReactInterop.useEffect
            (fun _ ->
                effect()
                React.createDisposable(ignore))

    /// The `useEffect` hook that creates an effect for React function components. This effect takes a array of *dependencies*.
    /// Whenever any of these dependencies change, the effect is re-executed. To execute the effect only once,
    /// you have to explicitly provide an empty array for the dependencies: `React.useEffect(effect, [| |])`.
    static member useEffect(effect: unit -> unit, dependencies: obj []) =
        ReactInterop.useEffectWithDeps
            (fun _ ->
                effect()
                React.createDisposable(ignore), dependencies)

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
        Internal.functionComponent(render >> Html.fragment)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(name: string, render: 'props -> #seq<ReactElement>) =
        Internal.functionComponent(render >> Html.fragment, name)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.functionComponent(render >> Html.fragment, withKey=withKey)

    /// <summary>
    /// Creates a React function component from a function that accepts a "props" object and renders a result.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member functionComponent(name: string, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
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
        Internal.memo(render >> Html.fragment)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> Html.fragment, name)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(areEqual: 'props -> 'props -> bool, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> Html.fragment, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> Html.fragment, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, areEqual: 'props -> 'props -> bool, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> Html.fragment, name, areEqual=areEqual)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='name'>The component name to display in the React dev tools.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(name: string, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> Html.fragment, name, withKey=withKey)

    /// <summary>
    /// `React.memo` memoizes the result of a function component. Given the same props, React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. For more control, a custom `areEqual` function can be provided.
    /// A component key can be provided in the props object, or a custom `withKey` function can be provided.
    /// </summary>
    /// <param name='areEqual'>A custom comparison function to use instead of React's default shallow compare.</param>
    /// <param name='withKey'>A function to derive a component key from the props.</param>
    /// <param name='render'>A render function that returns a list of elements.</param>
    static member memo(areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
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
    static member memo(name: string, areEqual: 'props -> 'props -> bool, withKey: 'props -> string, render: 'props -> #seq<ReactElement>) =
        Internal.memo(render >> Html.fragment, name, areEqual=areEqual, withKey=withKey)

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
        Interop.reactApi.createElement(contextObject?Consumer, null, [!!(render >> Html.fragment)])

    /// <summary>
    /// The `useContext` hook. Accepts a context object (the value returned from React.createContext) and returns the current context value for that context.
    /// The current context value is determined by the value prop of the nearest Provider component above the calling component in the tree.
    /// </summary>
    /// <param name='contextObject'>A context object returned from a previous React.createContext call.</param>
    static member useContext(contextObject: Fable.React.IContext<'a>) = Interop.reactApi.useContext contextObject
