namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Input

[<JavaScript>]
type TitleButton =
    {
        icon:string
        action:Panel->unit            
    }
    member x.Render panel=
        iAttr[Attr.Class "material-icons orange600 small"
              Attr.Style "cursor" "pointer"
              on.mouseDown (fun elem _->x.action panel)
              ][text x.icon]:>Doc
and [<JavaScript>] Panel =
    {
        left:Var<double>
        top:Var<double>
        element:Var<Dom.Element>
        arrangePanels:Panel->unit
        pannelAttrs:seq<Attr>
        titleAttrs:seq<Attr>
        titleContent:seq<Doc>
        titleButtons:list<TitleButton>
        content:Doc
    }
    static member Create arrangePanels pannelAttrs titleAttrs titleContent titleButtons content=
        {   
            left = Var.Create 0.0
            top = Var.Create 0.0
            element=Var.Create null
            arrangePanels = arrangePanels
            pannelAttrs = pannelAttrs
            titleAttrs = titleAttrs
            titleContent = titleContent
            titleButtons = titleButtons
            content = content
        }
    member x.Render=
        let dragActive = Var.Create false
        let mouseOverVar = Var.Create false
        let leftOffset=Var.Create 0.0
        let topOffset=Var.Create 0.0

        let mapDragActive=View.Map (fun (v) -> 
                                                  //Console.Log ("In mapDragActive Last left:"+x.left.Value.ToString())
                                                  v&& dragActive.Value) 
                                                  Mouse.LeftPressed
        let lastHeldPos = View.UpdateWhile (0,0) mapDragActive Mouse.Position
        let toLocal = lastHeldPos.Map (fun (x_cor,y_cor)->
                                                  //Console.Log ("In toLocal")
                                                  if dragActive.Value then 
                                                      let domRectParent = x.element.Value.GetBoundingClientRect()
                                                      let domRectParentParent = x.element.Value.ParentElement.GetBoundingClientRect()
                                                      let maxX = domRectParentParent.Width-domRectParent.Width
                                                      let maxY =  domRectParentParent.Height-domRectParent.Height
                                                      let xPos=min maxX (max 0.0 ((double)x_cor - leftOffset.Value))
                                                      let yPos=min maxY (max 0.0 ((double)y_cor - topOffset.Value))
                                                      x.left.Value <- xPos
                                                      x.top.Value <- yPos
                                                      Console.Log ("Last left:"+x.left.Value.ToString())
                                                      x.arrangePanels x
                                                      (xPos,yPos)
                                                  else (x.left.Value,x.top.Value)
                                                  )
        let titleAttrsUpdated = Seq.concat [
                                    x.titleAttrs
                                    [
                                        Attr.Style "cursor" "grab"
                                        on.mouseEnter  (fun _ _ -> 
                                              //Console.Log ("mouseEnter")
                                              mouseOverVar.Value<-true)
                                        on.mouseLeave (fun _ _ -> if not dragActive.Value then mouseOverVar.Value<-false)
                                        on.mouseUp (fun _ _ -> mouseOverVar.Value<-false
                                                               dragActive.Value <- false)
                                        on.mouseDown  (fun (elm:Dom.Element) evnt ->
                                                                    if mouseOverVar.Value 
                                                                     && x.element.Value.ParentElement <> null && x.element.Value.ParentElement.ParentElement <> null then
                                                                         dragActive.Value <- true
                                                                    leftOffset.Value <- (double)evnt.ClientX - x.left.Value
                                                                    topOffset.Value <- (double)evnt.ClientY - x.top.Value
                                                                    )                                       
                                    ]|>Seq.ofList
                               ]
        let titleContentUpdated =
                            tableAttr [Attr.Style "width" "100%"]
                                      [tr[
                                         td x.titleContent
                                         tdAttr[
                                           Attr.Style "text-align" "right"
                                           Attr.Style "vertical-align" "middle"]
                                           (x.titleButtons |>List.map (fun btn -> btn.Render x))
                                         ]]
                                      

        let panelAttrsUpdated = 
                Seq.concat [
                     x.pannelAttrs
                     [
                         Attr.Style "position" "absolute"
                         Attr.DynamicStyle "left" (View.Map (fun (x) -> 
                                                           //Console.Log "x from left"
                                                           sprintf "%fpx" x) x.left.View)
                         Attr.DynamicStyle "left" (View.Map (fun (x,y) -> 
                                                           //Console.Log "x from toLocal"
                                                           sprintf "%fpx" x) toLocal)
                         Attr.DynamicStyle "top"  (View.Map (fun (x,y) -> sprintf "%fpx" y)  toLocal)
                         Attr.DynamicStyle "top"  (View.Map (fun (y) -> sprintf "%fpx" y)  x.top.View)
                     ]|>Seq.ofList
                 ]
        let resDiv = 
            divAttr
                 panelAttrsUpdated
                 [
                     divAttr titleAttrsUpdated [titleContentUpdated]
                     x.content
                 ]
        x.element.Value <- resDiv.Dom
        resDiv
