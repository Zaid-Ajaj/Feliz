[<RequireQualifiedAccess>]
module FPSStats

open Browser.Dom
open Elmish
open Feliz
open System

type State =
    { Frames: float
      StartTime: DateTime
      PrevTime: DateTime
      Fps: int }

let render = React.memo(fun () ->
    let afRequest = React.useRef 0.
    let state,setState = React.useState { Frames = 0.; StartTime = DateTime.Now; PrevTime = DateTime.Now; Fps = 0 }
    let calcFPS () =
        let currentTime = DateTime.Now
        
        if currentTime > (state.PrevTime.AddSeconds(1.)) then
            (state.Frames + 1.) / (currentTime - state.PrevTime).TotalSeconds
            |> Some
        else None
        |> function
        | Some fps -> 
            { state with 
                Fps = (int fps)
                Frames = 0.
                PrevTime = currentTime }
            |> setState
        | None -> setState { state with Frames = state.Frames + 1. }
    
    React.useEffect(fun () -> 
        afRequest.current <- (window.requestAnimationFrame (fun _ -> calcFPS())))
    
    React.useEffectOnce(fun () -> 
        React.createDisposable (fun () -> window.cancelAnimationFrame afRequest.current)
    )
    
    Html.div [
        prop.text (sprintf "FPS: %A" state.Fps)
    ])