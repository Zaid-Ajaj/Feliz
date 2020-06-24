namespace Feliz.UseDeferred

[<RequireQualifiedAccess>]
type Deferred<'T> =
    | HasNotStartedYet
    | InProgress
    | Resolved of 'T
    | Failed of exn

[<RequireQualifiedAccess>]
/// Contains utility functions to work with value of the type Deferred<'T>
module Deferred =
    /// Returns whether the `Deferred<'T>` value has been started or not.
    let hasNotStartedYet = function
        | Deferred.HasNotStartedYet -> true
        | _ -> false

    /// Returns whether the `Deferred<'T>` value has been resolved or not.
    let resolved = function
        | Deferred.Resolved _ -> true
        | _ -> false

    /// Returns whether the `Deferred<'T>` value is in progress or not.
    let inProgress = function
        | Deferred.InProgress -> true
        | _ -> false

    /// Transforms the underlying value of the input deferred value when it exists from type to another
    let map (transform: 'T -> 'U) (deferred: Deferred<'T>) : Deferred<'U> =
        match deferred with
        | Deferred.HasNotStartedYet -> Deferred.HasNotStartedYet
        | Deferred.InProgress -> Deferred.InProgress
        | Deferred.Failed error -> Deferred.Failed error
        | Deferred.Resolved value ->
            try Deferred.Resolved (transform value)
            with error -> Deferred.Failed error

    let iter (perform: 'T -> unit) (deferred: Deferred<'T>) : unit =
        match deferred with
        | Deferred.Resolved value -> perform value
        | _ -> ()

    /// Verifies that a `Deferred<'T>` value is resolved and the resolved data satisfies a given requirement.
    let exists (predicate: 'T -> bool) = function
        | Deferred.Resolved value -> predicate value
        | _ -> false

    /// Like `map` but instead of transforming just the value into another type in the `Resolved` case, it will transform the value into potentially a different case of the the `Deferred<'T>` type.
    let bind (transform: 'T -> Deferred<'U>) (deferred: Deferred<'T>) : Deferred<'U> =
        match deferred with
        | Deferred.HasNotStartedYet -> Deferred.HasNotStartedYet
        | Deferred.InProgress -> Deferred.InProgress
        | Deferred.Failed error -> Deferred.Failed error
        | Deferred.Resolved value ->
            try transform value
            with error -> Deferred.Failed error

open Feliz

[<AutoOpen>]
module ReactHookExtensions =
    type React with
        static member useDeferred(operation: Async<'T>, dependencies: obj array) =
            let (deferred, setDeferred) = React.useState(Deferred.HasNotStartedYet)
            let token = React.useCancellationToken()
            let executeOperation = async {
                try
                    do setDeferred(Deferred<'T>.InProgress)
                    let! output = operation
                    do setDeferred(Deferred<'T>.Resolved output)
                with error ->
                    #if DEBUG
                    Browser.Dom.console.log(error)
                    #endif
                    do setDeferred(Deferred<'T>.Failed error)
            }

            React.useEffect((fun () -> Async.StartImmediate(executeOperation, token.current)), dependencies)

            deferred

        static member useDeferredCallback(operation: unit -> Async<'T>, setDeferred: Deferred<'T> -> unit) =
            let cancellationToken = React.useRef(new System.Threading.CancellationTokenSource())
            let executeOperation = async {
                try
                    do setDeferred(Deferred<'T>.InProgress)
                    let! output = operation()
                    do setDeferred(Deferred<'T>.Resolved output)
                with error ->
                    #if DEBUG
                    Browser.Dom.console.log(error)
                    #endif
                    do setDeferred(Deferred<'T>.Failed error)
            }

            React.useEffectOnce(fun () ->
                React.createDisposable(fun () -> cancellationToken.current.Cancel())
            )

            let start = React.useCallbackRef(fun () ->
                if not cancellationToken.current.IsCancellationRequested
                then Async.StartImmediate(executeOperation, cancellationToken.current.Token)
            )

            start

        static member useDeferredParallel<'T, 'U, 'Key when 'Key : comparison>(deferred: Deferred<'T>, map: 'T -> ('Key * Async<'U>) list) =
            let (data, setData) = React.useState(Map.empty)
            let addData = React.useCallbackRef(fun (key, value) -> setData(Map.add key value data))
            let token = React.useCancellationToken()
            let mapKeyedOperatons (operations: ('Key * Async<'U>) list) = [
                for (key, operation) in operations do
                    async {
                        try
                            do addData(key,  Deferred.InProgress)
                            let! output = operation
                            do addData(key, Deferred.Resolved output)
                        with error ->
                            #if DEBUG
                            Browser.Dom.console.log(error)
                            #endif
                            do addData(key, Deferred.Failed error)
                    }
            ]

            let start = React.useCallback(fun operations ->
                Fable.Core.JS.setTimeout(fun () ->
                    Async.StartImmediate(Async.Parallel(mapKeyedOperatons operations) |> Async.Ignore, token.current)
                ) 0 |> ignore
            )

            React.useEffect(fun () ->
                deferred
                |> Deferred.iter (fun data -> start(map data))
            , [| box deferred |])

            Map.toList data