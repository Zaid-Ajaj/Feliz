# Feliz.UseDeferred

Dead-simple data fetching hook for React component.

```fsharp:use-deferred
open Feliz
open Feliz.UseDeferred

React.functionComponent("BasicDeferred", fun () ->
    let (data, setData) = React.useState(Deferred.HasNotStartedYet)

    let loadData = async {
        do! Async.Sleep 1000
        return "Hello!"
    }

    React.useDeferred(loadData, setData, [|  |])

    match data with
    | Deferred.HasNotStartedYet -> Html.none
    | Deferred.InProgress -> Html.i [ prop.className [ "fa"; "fa-refresh"; "fa-spin"; "fa-2x" ] ]
    | Deferred.Failed error -> Html.div error.Message
    | Deferred.Resolved content -> Html.h1 content
)
```

More complicated example:
```fsharp:deferred-form
open Feliz
open Feliz.UseDeferred

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
```