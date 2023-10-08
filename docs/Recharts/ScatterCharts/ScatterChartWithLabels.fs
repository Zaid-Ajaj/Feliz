[<RequireQualifiedAccess>]
module Samples.Recharts.ScatterCharts.ScatterChartWithLabels

open Feliz
open Feliz.Recharts

type Point = { x: int; y: int; z: int }

let data = [
    { x = 100; y = 200; z = 200 }
    { x = 120; y = 100; z = 260 }
    { x = 170; y = 300; z = 400 }
    { x = 140; y = 250; z = 280 }
    { x = 150; y = 400; z = 500 }
    { x = 110; y = 280; z = 200 }
]

let chart = React.functionComponent(fun () -> [
    Recharts.scatterChart [
        scatterChart.margin(top=20, right=20, bottom=20, left=20)
        scatterChart.children [
            Recharts.cartesianGrid [ ]
            Recharts.xAxis [
                xAxis.dataKey (fun point -> point.x)
                xAxis.type'.number
                xAxis.name "stature"
                xAxis.unit "cm"
            ]
            Recharts.yAxis [
                yAxis.dataKey (fun point -> point.y)
                yAxis.type'.number
                yAxis.name "weight"
                yAxis.unit "kg"
            ]
            Recharts.tooltip [
                tooltip.cursor ("strokeDasharray", 3)
            ]
            Recharts.scatter [
                scatter.name "A school"
                scatter.data data
                scatter.fill "#8884d8"
                scatter.children [
                    Recharts.labelList [
                        labelList.dataKey (fun point -> point.x)
                    ]
                ]
            ]
        ]
    ]
])