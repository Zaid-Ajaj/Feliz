# Working with dates

The browsers' `Html.input` elements implement native date selectors when the `type` property is either
  - `dateTimeLocal` selecting time of the day, day, month and year
  - `date` selecting day, month and year
  - `month` selecting month and year

Using Feliz, you can diectly interact with the dates using `DateTime` instances.

The simplest version using `type=date`:
```fsharp
[<ReactComponent>]
let SimpleDateInput() =
    let (selectedDate, updateDate) = React.useState(DateTime.Now)
    Html.input [
        prop.type'.date
        prop.value selectedDate
        prop.onChange (fun newValue -> updateDate newValue)
    ]
```
Using a full date _and_ time selector with `type=dateTimeLocal`:
```fsharp
[<ReactComponent>]
let SimpleDateAndTimeInput() =
    let (selectedDate, updateDate) = React.useState(DateTime.Now)
    Html.input [
        prop.type'.dateTimeLocal
        prop.value(selectedDate, includeTime=true)
        prop.onChange (fun newValue -> updateDate newValue)
    ]
```
Notice here how we say `includeTime=true` when `type=dateTimeLocal` because the browsers expect the date to be formatted with time included which what `includeTime` controls.

### Switching between a date or datetime input element:

```fsharp:working-with-dates
[<ReactComponent>]
let SwitchingBetweenDateAndDateTime() =
    let (date, setDate) = React.useState<DateTime option>(None)
    let (dateAndTime, toggleDateAndTime) = React.useState(false)

    let formattedDate =
        match date with
        | None -> "No date selected yet"
        | Some date -> "Input: " + date.ToString "yyyy-MM-dd hh:mm"

    Html.div [
        Html.h3 formattedDate

        Html.input [
            prop.value(date, includeTime=dateAndTime)
            if dateAndTime
            then prop.type'.dateTimeLocal
            else prop.type'.date
            prop.onChange (fun newValue -> setDate(Some newValue))
        ]

        Html.button [
            prop.text "Reset selected date"
            prop.disabled date.IsNone
            prop.onClick (fun _ -> setDate(None))
        ]

        Html.button [
            prop.text "Toggle date and time"
            prop.disabled date.IsNone
            prop.onClick (fun _ -> toggleDateAndTime(not dateAndTime))
        ]
    ]
```