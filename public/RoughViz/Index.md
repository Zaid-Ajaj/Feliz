# Feliz.RoughViz [![Nuget](https://img.shields.io/nuget/v/Feliz.RoughViz.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.RoughViz)

Feliz bindings for the [roughViz](https://github.com/jwilber/roughViz) visualisation library. It is a fun project when your data visualisations don't need to be formal. This binding is actually made to work with original rough-viz library than renderst to the DOM rather than an existing third-party React library which makes it a nice example to learn from.

### Bar Chart

```fsharp:rough-bar-chart
open Feliz
open Feliz.RoughViz

let fruitSales = [
    ("Oranges", 5.0)
    ("Apples", 8.2)
    ("Strawberry", 10.0)
    ("Peach", 2.0)
    ("Pineapple", 17.0)
    ("Bananas", 10.0)
    ("Mango", 6.4)
]

let roughBarChart = React.functionComponent(fun () ->
    RoughViz.barChart [
        barChart.title "Fruit Sales"
        barChart.data fruitSales
        barChart.roughness 3
        barChart.color color.skyBlue
        barChart.stroke color.darkCyan
        barChart.axisFontSize 18
        barChart.fillStyle.crossHatch
        barChart.highlight color.lightGreen
    ])
```

### Horizontal Bar Chart

Takes the same properties as the `barChart` does

```fsharp:rough-horizontal-bar-chart
open Feliz
open Feliz.RoughViz

let fruitSales = [
    ("Oranges", 5.0)
    ("Apples", 8.2)
    ("Strawberry", 10.0)
    ("Peach", 2.0)
    ("Pineapple", 17.0)
    ("Bananas", 10.0)
    ("Mango", 6.4)
]

let roughHorizontalBarChart = React.functionComponent(fun () ->
    RoughViz.horizontalBarChart [
        barChart.title "Fruit Sales"
        barChart.data fruitSales
        barChart.roughness 3
        barChart.color color.skyBlue
        barChart.stroke color.darkCyan
        barChart.axisFontSize 18
        barChart.fillStyle.crossHatch
        barChart.highlight color.lightGreen
    ])
```

### Dynamic Data Charting
The following example shows how the chart is re-rendered as the data and configurations change.

```fsharp:dynamic-rough-chart
open Feliz
open Feliz.RoughViz

let dynamicRoughChart = React.functionComponent(fun () ->
    let (data, setData) = React.useState [
        ("point1", 70.0)
        ("point2", 40.0)
        ("point3", 65.0)
    ]

    let roughness, setRoughness = React.useState 3

    let addDataPoint() =
        let pointCount = List.length data
        let pointLabel = "point" + string (pointCount + 1)
        let nextPoint = (pointLabel, System.Random().NextDouble() * 100.0)
        let nextData = List.append data [ nextPoint ]
        setData nextData

    Html.div [

        Html.button [
            prop.className "button is-primary"
            prop.onClick (fun _ -> addDataPoint())
            prop.text "Add Datapoint"
        ]

        Html.h3 (sprintf "Roughness: %d" roughness)

        Html.input [
            prop.className "input"
            prop.type'.range
            prop.min 1
            prop.max 10
            prop.valueOrDefault roughness
            prop.onChange (fun (value: string) -> try setRoughness(int value) with | _ -> ())
            prop.style [ style.marginBottom 20 ]
        ]

        RoughViz.barChart [
            barChart.title "Random Data Points"
            barChart.data data
            barChart.roughness roughness
            barChart.color color.skyBlue
            barChart.stroke color.darkCyan
            barChart.axisFontSize 18
            barChart.fillStyle.crossHatch
            barChart.highlight color.lightGreen
        ]
    ])
```