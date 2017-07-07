namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html


[<JavaScript>]
type Dialog =
    {
        Title:Var<string>
        Content:Var<Elt>
        Visibility:Var<bool>
        OKCallback:Var<unit->unit>
    }
    static member Create=
        {
            Title = Var.Create ""
            Content = Var.Create (div[])
            Visibility = Var.Create false
            OKCallback = Var.Create (fun () -> ())
        }
    member x.ShowDialog title content okCallback = 
                                  x.Title.Value <- title
                                  x.Content.Value <- content
                                  x.Visibility.Value <- true
                                  x.OKCallback.Value <- okCallback
    member x.Render =
        divAttr[Attr.Style "position" "absolute"
                Attr.Style "left" "50%"
                Attr.Style "top" "50%"
                Attr.Style "z-index" "1"
                Attr.Style "background-color" "white";
                Attr.Style "min-height" "100px";
                Attr.Style "min-width" "200px";
                Attr.DynamicStyle "display" (View.Map (fun (isVis) -> if isVis then "block" else "none")  x.Visibility.View )
                ][  
                 tableAttr[
                     Attr.Style "background-color" "grey"
                     Attr.Style "display" "block"
                     Attr.Style "min-height" "100px"
                     Attr.Style "min-width" "200px" 
                  ]
                    [  
                      table[ 
                        tr[tdAttr[Attr.Style "border-style" "hidden"
                                  Attr.Style "background" "#404040"
                                  Attr.Style "color" "rgb(200,200,200)"
                                  Attr.Style "padding" "0px 2px 5px 2px"
                                  Attr.Style "Width" "200px"
                                  Attr.Style "font-size" "medium"][textView x.Title.View]]
                      ]
                      table[
                         tr[tdAttr[Attr.Style "padding" "5px 0px 0px 2px"][x.Content.View |> Doc.BindView (fun content->content)]]
                         tr[
                            tdAttr [Attr.Style "padding" "20px 12px 2px 20px"][
                                buttonAttr [Attr.Style "border-radius" "2px"
                                            on.click (fun _ _ ->x.Visibility.Value <-false
                                                                x.OKCallback.Value())] [text "OK"]
                             ]
                            tdAttr [Attr.Style "padding" "20px 12px 2px 50px"] [
                                buttonAttr [Attr.Style "border-radius" "2px"
                                            on.click (fun _ _ -> x.Visibility.Value<-false)] [text "Cancel"]
                              ] 
                         ]     
                      ]
                    ]
            ]
