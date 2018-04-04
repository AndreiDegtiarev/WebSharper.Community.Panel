namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
type WrapControlsAligment = 
|Vertical
|Horizontal

[<JavaScript>]
module WrapControls =

    let Render icons  aligment content= 
        let mouseOver = Var.Create false
        let icons = div[Attr.DynamicStyle "display" (View.Map (fun value -> if not value then "none" else "block") mouseOver.View)] 
                           icons
        match aligment with
        |Vertical -> div[][icons;content]
        |Horizontal -> (table[on.mouseEnter(fun _ _ ->mouseOver.Value <- true)
                              on.mouseLeave(fun _ _ -> mouseOver.Value <- false)][
                                tr[][td[][content];td[][icons]]
                               ])
                               //.OnMouseEnter(fun _ _ ->mouseOver.Value <- true).OnMouseLeave(fun _ _ -> mouseOver.Value <- false)
