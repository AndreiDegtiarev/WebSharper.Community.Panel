namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client

[<JavaScript>]
module LayoutManagers =

    open WebSharper.Community.Panel
    let panelRect panel margin= 
        (Rect.fromPanel panel)
          //.offset panel.Left.Value panel.Top.Value)
          .inflate margin margin
    let calcClientArea panelItems (stopItem:Panel) margin=
        //Console.Log ("calcClientArea 1") 
        let rec sublist (acc:list<Panel>) (lst:list<Panel>) =
            match lst with
            |[]->acc
            |head::rest -> if head.Key = stopItem.Key then
                                acc
                           else
                                sublist (head::acc) rest 
        let rects =
            panelItems
            |>List.ofSeq 
            |>sublist []
            |>List.map (fun panel ->Console.Log (panel.InternalName) 
                                    panelRect panel margin)
        //Console.Log ("calcClientArea 2") 
        if rects.IsEmpty then
            Rect.Create 0.0 0.0 0.0 0.0
        else
            Rect.Create (rects|>List.map (fun rc -> rc.left) |>List.min)
                        (rects|>List.map (fun rc -> rc.top)  |>List.min)
                        (rects|>List.map (fun rc -> rc.right)|>List.max)
                        (rects|>List.map (fun rc -> rc.bottom)|>List.max)
    let collectFreeSpace panelItems (rcContainer:Rect) (except:Panel) margin= 
        panelItems
        |>List.ofSeq 
        |>List.filter (fun item -> item.Key <> except.Key)
        |>List.fold (fun (acc:Rect list) panel -> 
                                let rcPanel = panelRect panel 0.0
                                //Console.Log ("collectFreeSpace: " + rcPanel.ToString())   
                                let rcTop = {left = 0.0; right = rcContainer.right; top = 0.0; bottom = rcPanel.top }
                                let rcLeft = {left = 0.0; right = rcPanel.left; top = 0.0; bottom = rcContainer.bottom }
                                rcTop::rcLeft
                                ::{rcLeft with left = rcPanel.right; right = rcContainer.right}
                                ::{rcTop with top = rcPanel.bottom; bottom = rcContainer.bottom}::[]
                                |>List.map (fun rc -> acc |> List.map (fun accRc -> accRc.intersect rc) 
                                                          |> List.filter (fun accRect -> 
                                                                               //Console.Log ("filter: " + accRect.ToString())   
                                                                               not accRect.isEmpty ))
                                |>List.concat
                                ) [rcContainer] 
    let movePanelToFreeSpace panelItems (panelItem:Panel) margin= 
        let rcPanel=Rect.fromPanel (panelItem)
        let rcContainer = Rect.fromDomRect (panelItem.Element.Value.ParentElement)
        //Console.Log ("Add panel: " + rcPanel.ToString() + " " + rcContainer.ToString())       
        let foundCandidate=
            collectFreeSpace panelItems rcContainer panelItem margin
            |>List.tryFind (fun rc -> 
                      //Console.Log ("Finds free rect: " + rc.ToString())             
                      rc.width >= rcPanel.width && rc.height >= rcPanel.height)
        match foundCandidate with 
        |None->()
        |Some(rc)->
              panelItem.Left.Value <- rc.left + margin
              panelItem.Top.Value <- rc.top + margin 
    let placePanel panelContainer panelItem margin =
        let rcPanel=Rect.fromPanel (panelItem)
        let rcContainer = Rect.Create 0.0 0.0 panelContainer.Width panelContainer.Height//Rect.fromDomRect (panelItem.Element.Value.ParentElement)      
        let foundCandidate=
            collectFreeSpace panelContainer.PanelItems rcContainer panelItem margin
            |>List.tryFind (fun rc -> //Console.Log ("placePanel: " + rc.ToString()) 
                                      rc.width >= rcPanel.width && rc.height >= rcPanel.height)
        match foundCandidate with 
        |None->()
        |Some(rc)->
              panelItem.Left.Value <- rc.left + margin
              panelItem.Top.Value <- rc.top + margin 
    let relayout panelContainer exceptPanel margin =
        let panelItems = panelContainer.PanelItems
        let listOfPanelItems= panelItems |> List.ofSeq
        //let exceptPanelItem=listOfPanelItems |> List.find (fun panelItem->panelItem.Left.Value = exceptPanel.Left.Value && panelItem.Top.Value = exceptPanel.Top.Value)
        let exceptPanelItem=listOfPanelItems |> List.find (fun panelItem->panelItem.Key = exceptPanel.Key)
        let foundPanel=
            panelItems
            |>List.ofSeq
            |>List.tryFind (fun panelItem-> 
                                //Console.Log("relayout:"+(Rect.fromPanel panelItem).ToString()+" "+(Rect.fromPanel exceptPanelItem).ToString())
                                panelItem.Key <> exceptPanelItem.Key && ((Rect.fromPanel panelItem).intersect (Rect.fromPanel exceptPanelItem)).minSize > 15.0)
        match foundPanel with
        |None ->()
        |Some(panelItem) -> movePanelToFreeSpace panelItems panelItem margin  
    let FloatingPanelLayoutManager margin = {new ILayoutManager with 
                                                 override x.Relayout panelContainer exceptPanel = relayout panelContainer exceptPanel margin 
                                                 override x.PlacePanel panelContainer panelItem = placePanel panelContainer panelItem margin                                                                 
                                          }
    let StackPanelLayoutManager = (*  let resizeContainer panelContainer panel=
                                                            Console.Log("resizeContainer 1")
                                                            let rc=calcClientArea panelContainer.PanelItems panel 0.0
                                                            Console.Log("resizeContainer 2")
                                                            let rcItem=panelRect panel 0.0
                                                            Console.Log("resizeContainer "+rc.right.ToString() + " " + rcItem.right.ToString() + " " + rc.bottom.ToString() + " " + rcItem.bottom.ToString())
                                                            panelContainer.Resize (rc.right + rcItem.right + 2.0) (max rc.bottom rcItem.bottom + 2.0)
                                                            rc
                                               *)
                                    {new ILayoutManager with 
                                                 override x.Relayout panelContainer exceptPanel =
                                                            //resizeContainer panelContainer exceptPanel |>ignore
                                                            relayout panelContainer exceptPanel 0.0
                                                 override x.PlacePanel panelContainer panel = 
                                                            //let rc = resizeContainer panelContainer panel
                                                            //panel.Left.Value <- rc.right
                                                            //panel.Top.Value <- rc.top
                                                            //placePanel panelContainer panel 0.0
                                                            ()
                                    }