namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Helper =
    let AttrsClick action =[Attr.Style "Color" "#FB8C00"
                            Attr.Style "cursor" "pointer"
                            on.click (fun elem _->action())]
    let Icon className id action = i(Attr.Class className::AttrsClick action)[text id] :>Doc
    let IconNormal id action = Icon "material-icons orange600" id action
    let IconSmall id action = Icon "material-icons orange600 small" id action

    let TxtIcon className id txt action = div(AttrsClick action)[
                                                table[][
                                                  tr[][td[][i[Attr.Class className][text id]]
                                                       td[Attr.Style "vertical-align" "middle"][text txt] 
                                                      ]
                                                ]
                                              ]:>Doc
    let TxtIconNormal id txt action = TxtIcon "material-icons orange600" id txt action

    let UniqueKey() = System.Guid.NewGuid().ToString()

    let MoveItemInModelList<'a,'b  when 'a : equality and 'b : equality> (items:ListModel<'a,'b>) isDown  item = 
        let listItems = items |>List.ofSeq
        let index = listItems |> List.findIndex (fun entry -> entry = item)
        let targetIndex = if isDown then index + 1 else index - 1
        if targetIndex >= 0 && targetIndex < listItems.Length then
            items.Clear()
            let (first,second) = listItems |> List.filter (fun entry -> entry <> item) |> List.splitAt targetIndex
            first@(item::second) |> List.iter (fun entry -> items.Add(entry))
            first |> List.iter (fun entry -> items.Add(entry))

