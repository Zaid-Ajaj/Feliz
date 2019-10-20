namespace Feliz.Recharts

open Feliz

[<Fable.Core.Erase>]
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

type ICellElement = interface end

[<Fable.Core.Erase>]
type domain =
    static member inline min : IAxisDomain = unbox "dataMin"
    static member inline max : IAxisDomain = unbox "dataMax"
    static member inline auto : IAxisDomain = unbox "auto"
    static member inline constant (value: int) : IAxisDomain = unbox value
    static member inline calculate (f: float -> float) : IAxisDomain = unbox f
    static member inline calculate (f: int -> float) : IAxisDomain = unbox f
    static member inline calculate (f: float -> int) : IAxisDomain = unbox f

[<Fable.Core.Erase>]
type ILabelProperties =
    abstract index : int
    abstract offset : int
    abstract x : float
    abstract y : float
    abstract value : float

[<Fable.Core.Erase>]
type IPieLabelProperties =
    abstract cy : float
    abstract cx : float
    abstract midAngle : float
    abstract innerRadius : float
    abstract outerRadius : float
    abstract percent : float
    abstract index : int

[<Fable.Core.Erase>]
type ChartCoordinate =
    abstract x : float
    abstract y : float

[<Fable.Core.Erase>]
type ChartDataPoint<'payload> =
    abstract payload: 'payload
    abstract color : string
    abstract fill : string
    abstract strokeWidth : float
    abstract value : float
    abstract dataKey : string
    abstract name : string

[<Fable.Core.Erase>]
type ChartMouseEvent<'label, 'payload> =
    abstract activeCoordinate : ChartCoordinate
    abstract activeLabel : 'label
    abstract activePayload : ChartDataPoint<'payload> array
    abstract activeToolipIndex : int
    abstract chartX : int
    abstract chartY : int

module Interop =
    let objectHas (keys: string list) (x: obj) =
        Fable.Core.JS.Object.keys(x)
        |> Seq.toList
        |> (=) keys