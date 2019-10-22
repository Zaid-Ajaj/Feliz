# Feliz.PigeonMaps [![Nuget](https://img.shields.io/nuget/v/Feliz.PigeonMaps.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Feliz.PigeonMaps)

Feliz-style bindings for [pigeon-maps](https://github.com/mariusandra/pigeon-maps), React maps without external dependencies. This binding includes it's own custom `PigeonMaps.marker` component to build map markers manually.

```fsharp:pigeonmaps-map-basic
open Feliz
open Feliz.PigeonMaps

let pigeonMap = PigeonMaps.map [
    map.center(50.879, 4.6997)
    map.zoom 12
    map.height 350
    map.children [
        PigeonMaps.marker [
            marker.anchor(50.879, 4.6997)
            marker.offsetLeft 15
            marker.offsetTop 30
            marker.render (fun marker -> [
                Html.i [
                    if marker.hovered
                    then prop.style [ style.color.red; style.cursor.pointer ]
                    prop.className [ "fa"; "fa-map-marker"; "fa-2x" ]
                ]
            ])
        ]
    ]
]
```