﻿namespace WebSharper.Community.Panel.Rect

open WebSharper
open WebSharper.JavaScript

[<JavaScript>]
module Library =
    type Rect =
        {
            left:double
            right:double
            top:double
            bottom:double
        }
        static member fromDomRect (elem:Dom.Element) = 
            let domRc=elem.GetBoundingClientRect()
            {left=0.0;top=0.0;right=domRc.Width;bottom=domRc.Height}
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
        member x.inflate width height =
            {left=x.left - width;top=x.top-height;right=x.right + width;bottom=x.bottom+height;}
        override x.ToString() = "left:"+x.left.ToString()+" "+ " top:"+x.top.ToString()+" right:"+x.right.ToString()+" bottom:"+x.bottom.ToString()