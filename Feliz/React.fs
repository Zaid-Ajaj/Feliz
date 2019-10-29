namespace Feliz

open System
open Fable.Core
open Fable.Core.JsInterop

module internal ReactInterop =
    let useEffectWithDeps (effect:  obj) (deps: obj) : unit = import "useEffectWithDeps" "./ReactInterop.js"
    let useEffect (effect: obj) : unit =  import "useEffect" "./ReactInterop.js"

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
    static member functionComponent(render: 'props -> Fable.React.ReactElement) =
        Fable.React.FunctionComponent.Of(render=render)
    static member functionComponent(name: string, render: 'props -> Fable.React.ReactElement) =
        Fable.React.FunctionComponent.Of(displayName=name, render=render)
    /// The `useCallback` hook. Returns a memoized callback. Pass an inline callback and an array of dependencies. `useCallback` will return a memoized version of the callback that only changes if one of the dependencies has changed.
    static member useCallback(callbackFunction: 'a -> 'b, dependencies: obj array) = Interop.reactApi.useCallback callbackFunction dependencies
    /// The `useMemo` hook. Returns a memoized value. Pass a "create" function and an array of dependencies. `useMemo` will only recompute the memoized value when one of the dependencies has changed.
    static member useMemo(createFunction: unit -> 'a, dependencies: obj array) = Interop.reactApi.useMemo createFunction dependencies
    /// `React.memo` is a higher order component for function components.
    /// If your function component renders the same result given the same props, you can wrap it in a call to `React.memo` for a performance boost in some cases by memoizing the result. This means that React will skip rendering the component, and reuse the last rendered result.
    /// By default it will only shallowly compare complex objects in the props object. If you want control over the comparison, you can also provide a custom comparison function as the second argument.
    static member memo(render: 'props -> Fable.React.ReactElement, ?areEqual: 'props -> 'props -> bool, ?displayName : string) : Fable.React.FunctionComponent<'props> =
        let memoElementType =
            match areEqual with
            | Some areEqual -> Fable.React.ReactElementType.memoWith areEqual render
            | None -> Fable.React.ReactElementType.memo render
        displayName |> Option.iter (fun name -> memoElementType?displayName <- name)
        fun props ->
          Fable.React.ReactElementType.create memoElementType props []
