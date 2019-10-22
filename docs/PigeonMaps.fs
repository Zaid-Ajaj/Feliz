module Samples.PigeonMaps

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