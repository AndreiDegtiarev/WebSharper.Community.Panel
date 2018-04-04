namespace WebSharper.Community.Panel

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client

[<JavaScript>]
type Rect =
    {
        left:double
        right:double
        top:double
        bottom:double
    }
    static member Create left top right bottom =
        {
            left =left
            top=top
            right=right
            bottom=bottom
        }
    static member fromDomRect (elem:Dom.Element) = 
        if elem <> null then
            let domRc=elem.GetBoundingClientRect()
            {left=0.0;top=0.0;right=domRc.Width;bottom=domRc.Height}
        else 
            Rect.Create 0.0 0.0 0.0 0.0
    static member fromPanel (panel:Panel) = 
        (Rect.fromDomRect panel.Element.Value)
         .offset panel.Left.Value panel.Top.Value       
    member x.isEmpty = x.left >= x.right || x.top >= x.bottom
    member x.offset x_offset y_offset ={left=x.left+x_offset;top = x.top + y_offset;right=x.right+x_offset;bottom=x.bottom+y_offset}
    member x.intersect rect:Rect =
        let left=max x.left  rect.left   
        let right = min x.right  rect.right
        let top = max x.top  rect.top   
        let bottom = min x.bottom  rect.bottom 
        {left=left;right=max left right;top=top;bottom = max top bottom}
    member x.width=x.right - x.left
    member x.height=x.bottom - x.top
    member x.minSize = min x.width x.height
    member x.inflate width height =
        {left=x.left - width;top=x.top-height;right=x.right + width;bottom=x.bottom+height;}
    override x.ToString() = "left:"+x.left.ToString()+" "+ " top:"+x.top.ToString()+" right:"+x.right.ToString()+" bottom:"+x.bottom.ToString()