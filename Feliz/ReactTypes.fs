module Feliz.ReactApi

open Browser.Types
open Fable.React
open Fable.Core
open System

type ReactChildren =
    abstract toArray: ReactElement -> ReactElement seq
    abstract toArray: ReactElement seq -> ReactElement seq

type IReactApi =
    abstract Children : ReactChildren
    abstract createContext: defaultValue: 'a -> IContext<'a>
    abstract createElement: comp: obj * props: obj -> ReactElement
    abstract createElement: comp: obj * props: obj * [<ParamList>] children: ReactElement seq -> ReactElement
    abstract forwardRef : render: Func<'Props,IRefValue<#HTMLElement option>,ReactElement> -> ('props -> IRefValue<'ref option> -> ReactElement)
    [<Emit("$0.lazy($1)")>]
    abstract lazy': import: (unit -> JS.Promise<'T>) -> 'T
    abstract memo: render: ('props -> ReactElement) * areEqual: ('props -> 'props -> bool) -> ('props -> ReactElement)
    abstract StrictMode: obj
    abstract Suspense: obj
    abstract useCallback : callbackFunction: ('a -> 'b) -> dependencies: obj array -> ('a -> 'b)
    abstract useContext: ctx: IContext<'a> -> 'a
    abstract useEffect : obj * 't array -> unit
    abstract useEffect : obj -> unit
    abstract useEffect : (unit -> unit) -> unit
    abstract useMemo : createFunction: (unit -> 'a) -> dependencies: obj array -> 'a
    abstract useReducer : ('state -> 'msg -> 'state) -> 'state -> ('state * ('msg -> unit))
    abstract useRef<'t> : initial: 't -> Fable.React.IRefValue<'t>
    abstract useState<'t> : initial:'t -> ('t * ('t -> unit))
