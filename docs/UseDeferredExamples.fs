[<RequireQualifiedAccess>]
module UseDeferredExamples

open Feliz
open Feliz.UseDeferred

let basicDeferred = React.functionComponent("BasicDeferred", fun () ->
    let loadData = async {
        do! Async.Sleep 1000
        return "Hello!"
    }

    let data = React.useDeferred(loadData, [|  |])

    match data with
    | Deferred.HasNotStartedYet -> Html.none
    | Deferred.InProgress -> Html.i [ prop.className [ "fa"; "fa-refresh"; "fa-spin"; "fa-2x" ] ]
    | Deferred.Failed error -> Html.div error.Message
    | Deferred.Resolved content -> Html.h1 content
)

let basicDeferredV2 = React.functionComponent("BasicDeferredV2", fun () ->
    let loadData = async {
        do! Async.Sleep 1000
        return "Hello!"
    }

    let (data, setData) = React.useState(Deferred.HasNotStartedYet)

    let startLoadingData = React.useDeferredCallback((fun () -> loadData), setData)

    React.useEffect(startLoadingData, [| |])

    match data with
    | Deferred.HasNotStartedYet -> Html.none
    | Deferred.InProgress -> Html.i [ prop.className [ "fa"; "fa-refresh"; "fa-spin"; "fa-2x" ] ]
    | Deferred.Failed error -> Html.div error.Message
    | Deferred.Resolved content -> Html.h1 content
)

let parallelDeferred = React.functionComponent("ParallelDeferred", fun () ->
    let loadIds = async {
        do! Async.Sleep 1000
        return [ 1 .. 5 ]
    }

    let loadItem itemId = async {
        do! Async.Sleep (itemId * 1000)
        return sprintf "Loaded item %d" itemId
    }

    let itemIds = React.useDeferred(loadIds, [|  |])

    let items = React.useDeferredParallel(itemIds, fun ids -> [ for itemId in ids -> itemId, loadItem itemId ])

    match itemIds with
    | Deferred.HasNotStartedYet -> Html.none
    | Deferred.InProgress -> Html.i [ prop.className [ "fa"; "fa-refresh"; "fa-spin"; "fa-2x" ] ]
    | Deferred.Failed error -> Html.h1 error.Message
    | Deferred.Resolved ids ->
        Html.ul [
            for (id, item) in items ->
            React.keyedFragment(id, [
                match item with
                | Deferred.HasNotStartedYet -> Html.none
                | Deferred.InProgress -> Html.li [ Html.i [ prop.className [ "fa"; "fa-refresh"; "fa-spin" ] ] ]
                | Deferred.Failed error -> Html.h1 error.Message
                | Deferred.Resolved item -> Html.li item
            ])
        ]
)

let login username password = async {
    do! Async.Sleep 1500
    if username = "admin" && password = "admin"
    then return Ok "admin"
    else return Error "Credentials incorrect"
}

let loginForm = React.functionComponent("LoginForm", fun () ->
    let (loginState, setLoginState) = React.useState(Deferred.HasNotStartedYet)
    let usernameRef = React.useInputRef()
    let passwordRef = React.useInputRef()

    let login() =
        match usernameRef.current, passwordRef.current with
        | Some username, Some password -> login username.value password.value
        | _ -> failwith "Component hasn't been initialized"

    let startLogin = React.useDeferredCallback(login, setLoginState)

    let message =
        match loginState with
        | Deferred.HasNotStartedYet -> Html.none
        | Deferred.InProgress -> Html.none
        | Deferred.Failed error ->
            Html.p [
                prop.style [ style.color.red ]
                prop.text (sprintf "Internal error: %s" error.Message)
            ]

        | Deferred.Resolved (Ok user) ->
            Html.p [
                prop.style [ style.color.green ]
                prop.text (sprintf "User %s logged in" user)
            ]

        | Deferred.Resolved (Error error) ->
            Html.p [
                prop.style [ style.color.red ]
                prop.text (sprintf "Login error: %s" error)
            ]

    Html.div [
        Html.div [
            prop.className "field"
            prop.children [
                Html.label [ prop.className "label"; prop.text "Username" ]
                Html.div [
                    prop.className "control"
                    prop.children [
                        Html.input [
                            prop.className "input"
                            prop.ref usernameRef
                            prop.placeholder "Username"
                            prop.disabled (Deferred.inProgress loginState)
                        ]
                    ]
                ]
            ]
        ]

        Html.div [
            prop.className "field"
            prop.children [
                Html.label [ prop.className "label"; prop.text "Password" ]
                Html.div [
                    prop.className "control"
                    prop.children [
                        Html.input [
                            prop.className "input"
                            prop.ref passwordRef
                            prop.placeholder "Password"
                            prop.disabled (Deferred.inProgress loginState)
                            prop.type'.password
                        ]
                    ]
                ]
            ]
        ]

        message

        Html.button [
            prop.className [
                "button is-primary";
                if Deferred.inProgress loginState
                then "is-loading"
            ]

            prop.text "Login"
            prop.disabled (Deferred.inProgress loginState)
            prop.onClick(fun _ -> startLogin())
        ]
    ])
