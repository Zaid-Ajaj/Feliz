module Feliz.ReactApi

open Fable.React
open Fable.Core

type ReactChildren =
    abstract toArray: ReactElement -> ReactElement seq
    abstract toArray: ReactElement seq -> ReactElement seq

type IReactApi =
    abstract useState<'t> : initial:'t -> ('t * ('t -> unit))
    abstract useRef<'t> : initial: 't -> Fable.React.IRefValue<'t>
    abstract useReducer : ('state -> 'msg -> 'state) -> 'state -> ('state * ('msg -> unit))
    abstract useEffect : obj * 't array -> unit
    abstract useEffect : obj -> unit
    abstract useEffect : (unit -> unit) -> unit
    abstract createElement: comp: obj * props: obj -> ReactElement
    abstract createElement: comp: obj * props: obj * [<ParamList>] children: ReactElement seq -> ReactElement
    abstract Children : ReactChildren
    abstract useCallback : callbackFunction: ('a -> 'b) -> dependencies: obj array -> ('a -> 'b)
    abstract useMemo : createFunction: (unit -> 'a) -> dependencies: obj array -> 'a
    abstract memo: render: ('props -> ReactElement) * areEqual: ('props -> 'props -> bool) -> ('props -> ReactElement)
    abstract createContext: defaultValue: 'a -> IContext<'a>
    abstract useContext: ctx: IContext<'a> -> 'a
