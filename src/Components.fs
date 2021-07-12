namespace App

open Feliz
open Feliz.Router
open Feliz.MaterialUI
open Feliz.MaterialUI.MaterialTable
open Fable.SimpleJson
open Fable.SimpleHttp
open Feliz.Bulma

type private RowData =
            { Date: string
              Open: float
              High:float
              Low: float
              Close: float
              Volume: int }

// Type is created automatically from the url
type StockData = Fable.JsonProvider.Generator<"https://api.npoint.io/c9bccf3decda3bc293f2">

type Components =
    /// <summary>
    /// The simplest possible React component.
    /// Shows a header with the text Hello World
    /// </summary>
    [<ReactComponent>]
    static member HelloWorld() = Html.h1 "Hello World"

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Counter() =
        let (count, setCount) = React.useState(0)
        Html.div [
            Html.h1 count
            Html.button [
                prop.onClick (fun _ -> setCount(count + 1))
                prop.text "Increment"
            ]
        ]

    /// <summary>
    /// A React component that uses Feliz.Router
    /// to determine what to show based on the current URL
    /// </summary>
    [<ReactComponent>]
    static member Router() =
        let (currentUrl, updateUrl) = React.useState(Router.currentUrl())
        React.router [
            router.onUrlChanged updateUrl
            router.children [
                match currentUrl with
                | [ ] -> Html.h1 "Index"
                | [ "hello" ] -> Components.HelloWorld()
                | [ "counter" ] -> Components.Counter()
                | otherwise -> Html.h1 "Not found"
            ]
        ]
    /// <summary>
    /// The material table React component.
    /// Shows stock data in table format
    /// </summary>
    [<ReactComponent>]
    static member MaterialTable() = 
        let theme = Styles.useTheme()
        Mui.materialTable [
            materialTable.title "Stock History"
            materialTable.columns [
                columns.column [
                    column.title "Date"
                    column.field<RowData> (fun rd -> nameof rd.Date)
                ]
                columns.column [
                    column.title "Open"
                    column.field<RowData> (fun rd -> nameof rd.Open)
                ]
                columns.column [
                    column.title "High"
                    column.field<RowData> (fun rd -> nameof rd.High)
                    column.type'.numeric
                ]
                columns.column [
                    column.title "Low"
                    column.field<RowData> (fun rd -> nameof rd.Low)
                ]
                columns.column [
                    column.title "Close"
                    column.field<RowData> (fun rd -> nameof rd.Close)
                ]
                columns.column [
                    column.title "Volume"
                    column.field<RowData> (fun rd -> nameof rd.Volume)
                ]
            ]
            materialTable.data<RowData> (fun query ->
                Promise.create (fun resolve reject ->
                    async {
                        let! (statusCode, responseText) =
                            sprintf "https://api.npoint.io/c9bccf3decda3bc293f2"
                            |> Http.get
                        if statusCode = 200 then
                            responseText
                            |> Json.tryParseAs<RowData[]>
                            |> function
                            | Ok resJson -> 
                                { data = resJson
                                  page = 1
                                  totalCount = 50 }
                                |> resolve
                            | Error e -> e |> System.Exception |> reject
                    }
                    |> Async.StartImmediate
                )
            )
        ]
    /// <summary>
    /// The simplest possible React component.
    /// Shows a header with the text Hello World
    /// </summary>
    [<ReactComponent>]
    static member CardComponent() = 
        
        async {
            let! (_, res) = Http.get "https://api.npoint.io/c9bccf3decda3bc293f2"
            let stocks = StockData.ParseArray res
            for stock in stocks do
                // If the JSON schema changes, this will fail compilation
                printfn "ID %i, USER: %i, TITLE %s, COMPLETED %b"
                stock.Date
                stock.Open
                stock.High
                stock.Low
            //stocks.fun x -> x.)
                Bulma.card [
                    Bulma.cardContent [
                        Bulma.content "Lorem ipsum dolor sit ... nec iaculis mauris."
                    ]
                ]
        } |> Async.StartImmediate
        Bulma.card [
                Bulma.cardContent [
                    Bulma.content "Lorem ipsum dolor sit ... nec iaculis mauris."
                ]
        ]
        