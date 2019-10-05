[<RequireQualifiedAccess>]
module Samples.Recharts.AreaCharts.SimpleAreaChart

open Feliz
open Feliz.Recharts
open Fable.Core.Experimental

type Point = { name: string; uv: int; pv: int }

let data = [
    { name = "Page A"; uv = 4000; pv = 2400 }
    { name = "Page B"; uv = 3000; pv = 1398 }
    { name = "Page C"; uv = 2000; pv = 9800 }
    { name = "Page D"; uv = 2780; pv = 3908 }
    { name = "Page E"; uv = 1890; pv = 4800 }
    { name = "Page F"; uv = 2390; pv = 3800 }
    { name = "Page G"; uv = 3490; pv = 4300 }
]

let chart = React.functionComponent <| fun () ->
    Recharts.areaChart [
        areaChart.width 500
        areaChart.height 400
        areaChart.data data
        areaChart.margin(top=10, right=30)
        areaChart.children [
            Recharts.xAxis [ xAxis.dataKey (fun p -> nameof p.name) ]
            Recharts.yAxis [ ]
            Recharts.tooltip [ ]
            Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]
            Recharts.area [
                area.monotone
                area.dataKey (fun p -> nameof p.uv)
                area.stroke "#8884d8"
                area.fill "#8884d8"
            ]
        ]
    ]