namespace Feliz.Recharts

open Fable.Core
open Fable.Core.JsInterop
open Feliz

type Recharts =
    static member inline areaChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "AreaChart" "recharts", createObj !!properties)
    static member inline lineChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "LineChart" "recharts", createObj !!properties)
    static member inline xAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "XAxis" "recharts", createObj !!properties)
    static member inline yAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "YAxis" "recharts", createObj !!properties)
    static member inline tooltip (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Tooltip" "recharts", createObj !!properties)
    static member inline legend (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Legend" "recharts", createObj !!properties)
    static member inline area (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Area" "recharts", createObj !!properties)
    static member inline line (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Line" "recharts", createObj !!properties)
    static member inline cartesianGrid (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "CartesianGrid" "recharts", createObj !!properties)