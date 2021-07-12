module Main

open Feliz
open Browser.Dom
open Fable.Core.JsInterop
open App

importSideEffects "./styles/global.scss"

open Feliz.Bulma



let render = React.functionComponent (fun () ->
    ReactDOM.render(Components.MaterialTable(),document.getElementById "stock-comp")
    Bulma.field.div [
        //prop.style [ style.marginRight 5 ]
        Switch.checkbox [
            prop.id "mediumcheck"
            color.isInfo
            switch.isOutlined
            switch.isMedium
            prop.onClick (fun arg -> ReactDOM.render(Components.CardComponent(),document.getElementById "stock-comp"))
        ]
        Html.label [
            prop.htmlFor "mediumcheck"
            prop.text "card view"
        ]
    ]
)
   

    ReactDOM.render(
            render,
            document.getElementById "feliz-app"
        )