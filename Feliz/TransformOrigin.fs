namespace Feliz

[<Erase>]
type origin =
    static member inline left : ITransformOrigin = "left"
    static member inline center : ITransformOrigin = "center"
    static member inline right : ITransformOrigin = "right"
    static member inline top : ITransformOrigin = "top"
    static member inline bottom : ITransformOrigin = "bottom"

    static member inline percentage (value: int) : ITransformOrigin =
        unbox<string> value + "%"
    static member inline percentage (value: float) : ITransformOrigin =
        unbox<string> value + "%" 
