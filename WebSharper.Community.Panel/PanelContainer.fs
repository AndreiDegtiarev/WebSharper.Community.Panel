﻿namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.Community.Panel


[<JavaScript>]
type PanelItem =
    {
        Key:Key
        Name : string
        Panel:Panel
    }
    static member Create name=
        {   
            Key=Key.Fresh()
            Name = name
            Panel = Panel.Create
        }

[<JavaScript>]
type PanelContainer =
    {
        PanelItems : ListModel<Key,PanelItem>
    }
    static member Create =
        {
            PanelItems = ListModel.Create (fun item ->item.Key) []
        }

    member x.CreateItem name =
            let newItem=PanelItem.Create name
            x.PanelItems.Add  newItem 
    member x.RenderPanelItem (haItem:PanelItem) =     
        let collectFreeSpace (rcContainer:Rect) (except:PanelItem)= 
                x.PanelItems
                |>List.ofSeq 
                |>List.filter (fun item -> item.Key <> except.Key)
                |>List.fold (fun (acc:Rect list) panel -> 
                                        let rcPanel = ((Rect.fromDomRect panel.Panel.element.Value)
                                                        .offset panel.Panel.lastLeft.Value panel.Panel.lastTop.Value)
                                                        .inflate 5.0 5.0
                                        Console.Log ("collectFreeSpace: " + rcPanel.ToString())   
                                        let rcTop = {left = 0.0; right = rcContainer.right; top = 0.0; bottom = rcPanel.top }
                                        let rcLeft = {left = 0.0; right = rcPanel.left; top = rcPanel.top; bottom = rcPanel.bottom }
                                        rcTop::rcLeft
                                        ::{rcLeft with left = rcPanel.right; right = rcContainer.right}
                                        ::{rcTop with top = rcPanel.bottom; bottom = rcContainer.bottom}::[]
                                        |>List.map (fun rc -> acc |> List.map (fun accRc -> accRc.intersect rc) 
                                                                  |> List.filter (fun accRect -> 
                                                                                       Console.Log ("filter: " + accRect.ToString())   
                                                                                       not accRect.isEmpty ))
                                        |>List.concat
                                        ) [rcContainer]
        (haItem.Panel.panelAttr
                [Attr.Style "Width" "150px"]
                [Attr.Class "panelTitle"]
                [tableAttr [Attr.Style "width" "100%"]
                     [tr[
                        td[text haItem.Name]
                        tdAttr[
                          Attr.Style "text-align" "right"
                          Attr.Style "vertical-align" "middle"]
                          [iAttr[Attr.Class "material-icons orange600 small"
                                 Attr.Style "cursor" "pointer"
                                 on.mouseDown (fun _ _->x.PanelItems.Remove(haItem)
                                                        )][text "clear"]
                                                              ]
                        ]]
                ]
                (divAttr
                    [Attr.Class "panelContent"]
                    [text "Content"])).OnAfterRender((fun el -> 
                        let rcPanel=Rect.fromDomRect el
                        let rcContainer = Rect.fromDomRect el.ParentElement
                        Console.Log ("Add panel: " + rcPanel.ToString() + " " + rcContainer.ToString())       
                        let foundCandidate=
                            collectFreeSpace rcContainer haItem
                            |>List.tryFind (fun rc -> 
                                      Console.Log ("Finds free rect: " + rc.ToString())             
                                      rc.width >= rcPanel.width && rc.height >= rcPanel.height)
                        match foundCandidate with 
                        |None->()
                        |Some(rc)->
                              haItem.Panel.lastLeft.Value <- rc.left + 5.0
                              haItem.Panel.lastTop.Value <- rc.top + 5.0
                             // haItem.panel.moveTo.Value <- (rc.left,rc.top)
 
                    
                    ))