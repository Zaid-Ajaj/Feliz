namespace Feliz.Recharts

open Feliz
open Fable.Core

[<Erase>]
type IDotProperties<'a> =
    abstract cy : float
    abstract cx : float
    abstract dataKey : string
    abstract height : float
    abstract key : string
    abstract index : int
    abstract stroke : string
    abstract fill : string
    abstract value : float
    abstract payload : 'a

type IAxisDomain = interface end

[<Erase>]
type domain =
    static member inline min : IAxisDomain = unbox "dataMin"
    static member inline max : IAxisDomain = unbox "dataMax"
    static member inline auto : IAxisDomain = unbox "auto"
    static member inline constant (value: int) : IAxisDomain = unbox value
    static member inline calculate (f: float -> float) : IAxisDomain = unbox f
    static member inline calculate (f: int -> int) : IAxisDomain = unbox f
    static member inline calculate (f: int -> float) : IAxisDomain = unbox f
    static member inline calculate (f: float -> int) : IAxisDomain = unbox f

[<Erase>]
type ILabelProperties =
    abstract index : int
    abstract offset : int
    abstract x : float
    abstract y : float
    abstract value : float

[<Erase>]
type IXAxisTickPayload =
    abstract index : int
    abstract isShow : bool
    abstract coordinate : float
    abstract tickCoord : float
    abstract offset : float

[<Erase>]
type IXAxisTickProperties =
    abstract fill : string
    abstract index : int
    abstract height : float
    abstract stroke  : string
    abstract textAnchor : string
    abstract verticalAnchor : string
    abstract visibleTicksCount : int
    abstract width : float
    abstract x : float
    abstract y : float

[<Erase>]
type IPieLabelProperties =
    abstract cy : float
    abstract cx : float
    abstract midAngle : float
    abstract innerRadius : float
    abstract outerRadius : float
    abstract percent : float
    abstract index : int

[<Erase>]
type ChartCoordinate =
    abstract x : float
    abstract y : float

[<Erase>]
type ChartDataPoint<'payload> =
    abstract payload: 'payload
    abstract color : string
    abstract fill : string
    abstract strokeWidth : float
    abstract value : float
    abstract dataKey : string
    abstract name : string

[<Erase>]
type ChartMouseEvent<'label, 'payload> =
    abstract activeCoordinate : ChartCoordinate
    abstract activeLabel : 'label
    abstract activePayload : ChartDataPoint<'payload> array
    abstract activeToolipIndex : int
    abstract chartX : int
    abstract chartY : int

module Interop =
    [<Emit("Object.keys($0)")>]
    let internal objectKeys (x: obj) = jsNative
    let objectHas (keys: string list) (x: obj) =
        objectKeys(x)
        |> Seq.toList
        |> (=) keys
