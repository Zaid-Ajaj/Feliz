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
    static member useEffect(effect: unit -> unit) = Interop.reactApi.useEffect(effect)
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> unit, a: 'a) =
        ReactInterop.useEffectWithDeps effect [| a |]
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> unit, a: 'a, b: 'b) =
        ReactInterop.useEffectWithDeps effect [| unbox a; unbox b |]
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> unit, a: 'a, b: 'b, c: 'c) =
        ReactInterop.useEffectWithDeps effect [| unbox a; unbox b; unbox c |]
    /// The `useEffect` hook that creates an effect for React function components which re-evaluates the hook once the dependencies change.
    static member useEffect(effect: unit -> unit, a: 'a, b: 'b, c: 'c, d: 'd) =
        ReactInterop.useEffectWithDeps effect [| unbox a; unbox b; unbox c; unbox d |]
    static member functionComponent(render: 'props -> Fable.React.ReactElement) =
        Fable.React.FunctionComponent.Of(render=render, memoizeWith=Fable.React.Helpers.memoEqualsButFunctions)
    static member functionComponent(name: string, render: 'props -> Fable.React.ReactElement) =
        Fable.React.FunctionComponent.Of(displayName=name, render=render, memoizeWith=Fable.React.Helpers.memoEqualsButFunctions)