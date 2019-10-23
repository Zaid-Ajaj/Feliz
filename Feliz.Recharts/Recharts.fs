namespace Feliz.Recharts

open Fable.Core
open Fable.Core.JsInterop
open Feliz

[<Erase>]
type Recharts =
    static member inline areaChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "AreaChart" "recharts", createObj !!properties)
    static member inline lineChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "LineChart" "recharts", createObj !!properties)
    static member inline barChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "BarChart" "recharts", createObj !!properties)
    static member inline bar (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Bar" "recharts", createObj !!properties)
    static member inline brush (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Brush" "recharts", createObj !!properties)
    static member inline xAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "XAxis" "recharts", createObj !!properties)
    static member inline yAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "YAxis" "recharts", createObj !!properties)
    static member inline zAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ZAxis" "recharts", createObj !!properties)
    static member inline tooltip (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Tooltip" "recharts", createObj !!properties)
    static member inline legend (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Legend" "recharts", createObj !!properties)
    static member inline area (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Area" "recharts", createObj !!properties)
    /// Cell can be wrapped by Pie, Bar, or RadialBar to specify attributes of each child. In Pie , for example, we can specify the attributes of each child node through data, but the props of Cell have higher priority
    static member inline cell (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Cell" "recharts", createObj !!properties)
    static member inline labelList (properties: IReactProperty list) = 
        Interop.reactApi.createElement(import "LabelList" "recharts", createObj !!properties)
    static member inline label (properties: IReactProperty list) = 
        Interop.reactApi.createElement(import "Label" "recharts", createObj !!properties)
    static member inline sector (properties: IReactProperty list) = 
        Interop.reactApi.createElement(import "Sector" "recharts", createObj !!properties)
    static member inline line (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Line" "recharts", createObj !!properties)
    static member inline cartesianGrid (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "CartesianGrid" "recharts", createObj !!properties)
    static member inline cartesianAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "CartesianAxis" "recharts", createObj !!properties)
    static member inline responsiveContainer (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ResponsiveContainer" "recharts", createObj !!properties)
    static member inline composedChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ComposedChart" "recharts", createObj !!properties)
    static member inline referenceLine (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ReferenceLine" "recharts", createObj !!properties)
    static member inline referenceArea (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ReferenceArea" "recharts", createObj !!properties)
    static member inline referenceDot (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ReferenceDot" "recharts", createObj !!properties)
    static member inline pieChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "PieChart" "recharts", createObj !!properties)
    static member inline pie (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Pie" "recharts", createObj !!properties)
    static member inline radarChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "RadarChart" "recharts", createObj !!properties)
    static member inline radar (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Radar" "recharts", createObj !!properties)
    static member inline radialBarChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "RadialBarChart" "recharts", createObj !!properties)
    static member inline radialBar (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "RadialBar" "recharts", createObj !!properties)
    static member inline scatterChart (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "ScatterChart" "recharts", createObj !!properties)
    static member inline scatter (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "Scatter" "recharts", createObj !!properties)
    static member inline polarAngleAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "PolarAngleAxis" "recharts", createObj !!properties)
    static member inline polarGrid (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "PolarGrid" "recharts", createObj !!properties)
    static member inline polarRadiusAxis (properties: IReactProperty list) =
        Interop.reactApi.createElement(import "PolarRadiusAxis" "recharts", createObj !!properties)
    static member inline funnelChart (properties: IReactProperty list) = 
        Interop.reactApi.createElement(import "FunnelChart" "recharts", createObj !!properties)
    static member inline funnel (properties: IReactProperty list) = 
        Interop.reactApi.createElement(import "Funnel" "recharts", createObj !!properties)
    static member inline errorBar (properties: IReactProperty list) = 
        Interop.reactApi.createElement(import "ErrorBar" "recharts", createObj !!properties)