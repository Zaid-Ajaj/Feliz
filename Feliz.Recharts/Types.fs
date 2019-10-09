namespace Feliz.Recharts

open Feliz

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

type domain =
    static member inline min : IAxisDomain = unbox "dataMin"
    static member inline max : IAxisDomain = unbox "dataMax"
    static member inline auto : IAxisDomain = unbox "auto"
    static member inline constant (value: int) : IAxisDomain = unbox value
    static member inline calculate (f: float -> float) : IAxisDomain = unbox f
    static member inline calculate (f: int -> float) : IAxisDomain = unbox f
    static member inline calculate (f: float -> int) : IAxisDomain = unbox f

type ILabelProperties =
    abstract index : int
    abstract offset : int
    abstract x : float
    abstract y : float
    abstract value : float

type IPieLabelProperties =
    abstract cy : float
    abstract cx : float
    abstract midAngle : float
    abstract innerRadius : float
    abstract outerRadius : float
    abstract percent : float
    abstract index : int