namespace Feliz.PigeonMaps

open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Interop =
    /// Creates a map marker internally
    let createMarker : obj -> Fable.React.ReactElement= import "createMarker" "./Marker.js"

type PigeonMaps =
    static member inline map (properties: IReactProperty list) =
        Interop.reactApi.createElement(importDefault "pigeon-maps", createObj !!properties)
    static member marker (properties: IReactProperty list) =
        Interop.createMarker (createObj !!properties)
