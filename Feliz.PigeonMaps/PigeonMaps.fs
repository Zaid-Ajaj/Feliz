namespace Feliz.PigeonMaps

open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Interop =
    /// Creates a map marker internally (which is actually `ReactElement`)
    let createMarker : obj -> IMapMarker = import "createMarker" "./Marker.js"

[<Erase>]
type PigeonMaps =
    static member inline map (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "pigeon-maps", createObj !!properties)
    static member inline marker (properties: IReactProperty list) =
        Interop.createMarker (createObj !!properties)
