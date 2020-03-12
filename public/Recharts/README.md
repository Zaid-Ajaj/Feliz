# Feliz.Recharts [![Nuget](https://img.shields.io/nuget/v/Feliz.Recharts.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.Recharts)

Feliz-style bindings for [recharts](http://recharts.org/en-US/), a composable charting library built on React components. The binding translates the original API of recharts in a one-to-one fashion but makes it type-safe and easily discoverable.

The following sample is the equivalent of [this sample](http://recharts.org/en-US/api) from the recharts docs.

```fsharp:recharts-main
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

let createGradient (id: string) (color: string) =
    Html.linearGradient [
        prop.id id
        prop.x1 0; prop.x2 0
        prop.y1 0; prop.y2 1
        prop.children [
            Html.stop [
                prop.offset(length.percent 5)
                prop.stopColor color
                prop.stopOpacity 0.8
            ]
            Html.stop [
                prop.offset(length.percent 95)
                prop.stopColor color
                prop.stopOpacity 0.0
            ]
        ]
    ]

let sampleChart = React.functionComponent(fun () ->
    Recharts.areaChart [
        areaChart.width 730
        areaChart.height 250
        areaChart.data data
        areaChart.margin(top=10, right=30)
        areaChart.children [
            Html.defs [
                createGradient "colorUv" "#8884d8"
                createGradient "colorPv" "#82ca9d"
            ]
            Recharts.xAxis [ xAxis.dataKey (fun point -> point.name) ]
            Recharts.yAxis [ ]
            Recharts.tooltip [ ]
            Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]

            Recharts.area [
                area.monotone
                area.dataKey (fun point -> point.uv)
                area.stroke "#8884d8"
                area.fillOpacity 1
                area.fill "url(#colorUv)"
            ]

            Recharts.area [
                area.monotone
                area.dataKey (fun point -> point.pv)
                area.stroke "#82ca9d"
                area.fillOpacity 1
                area.fill "url(#colorPv)"
            ]
        ]
    ])

open Browser.Dom

ReactDOM.render(sampleChart, document.getElementById "root")
```
