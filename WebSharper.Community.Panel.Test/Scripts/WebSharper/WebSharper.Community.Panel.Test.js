(function()
{
 "use strict";
 var Global,WebSharper,Community,Panel,Test,Client,ContentItem,ContentModel,SC$1,IntelliFactory,Runtime,UI,Doc,ListModel,List,AttrModule,View,Helper,PanelContainer,LayoutManagers,Panel$1,Var,TitleButton,PropertyGrid,Properties,PropertyGrid$1,Dialog,Interpolation,Easing,An,Trans;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Community=WebSharper.Community=WebSharper.Community||{};
 Panel=Community.Panel=Community.Panel||{};
 Test=Panel.Test=Panel.Test||{};
 Client=Test.Client=Test.Client||{};
 ContentItem=Client.ContentItem=Client.ContentItem||{};
 ContentModel=Client.ContentModel=Client.ContentModel||{};
 SC$1=Global.StartupCode$WebSharper_Community_Panel_Test$Client=Global.StartupCode$WebSharper_Community_Panel_Test$Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 UI=WebSharper&&WebSharper.UI;
 Doc=UI&&UI.Doc;
 ListModel=UI&&UI.ListModel;
 List=WebSharper&&WebSharper.List;
 AttrModule=UI&&UI.AttrModule;
 View=UI&&UI.View;
 Helper=Panel&&Panel.Helper;
 PanelContainer=Panel&&Panel.PanelContainer;
 LayoutManagers=Panel&&Panel.LayoutManagers;
 Panel$1=Panel&&Panel.Panel;
 Var=UI&&UI.Var;
 TitleButton=Panel&&Panel.TitleButton;
 PropertyGrid=Community&&Community.PropertyGrid;
 Properties=PropertyGrid&&PropertyGrid.Properties;
 PropertyGrid$1=PropertyGrid&&PropertyGrid.PropertyGrid;
 Dialog=Panel&&Panel.Dialog;
 Interpolation=UI&&UI.Interpolation;
 Easing=UI&&UI.Easing;
 An=UI&&UI.An;
 Trans=UI&&UI.Trans;
 ContentItem.New=function(Text)
 {
  return{
   Text:Text
  };
 };
 ContentModel=Client.ContentModel=Runtime.Class({
  get_Render:function()
  {
   return Doc.ConvertBy(this.Items.key,function(item)
   {
    return Doc.Element("div",[],[Doc.TextNode(item.Text)]);
   },this.Items.v);
  }
 },null,ContentModel);
 ContentModel.get_Create=function()
 {
  return ContentModel.New(ListModel.Create(function(item)
  {
   return item.Text;
  },List.ofArray([ContentItem.New("Content 1")])));
 };
 ContentModel.New=function(Items)
 {
  return new ContentModel({
   Items:Items
  });
 };
 Client.Main=function()
 {
  function animAttr(param,unit)
  {
   return AttrModule.AnimatedStyle(param,Client.TransTransition(),View.Map(function()
   {
    return Client.isExpanded().Get()?100:0;
   },Client.isExpanded().get_View()),function(x)
   {
    return Global.String(x)+unit;
   });
  }
  function animText(txt)
  {
   return Doc.Element("td",[AttrModule.Style("vertical-align","middle"),AttrModule.Style("color","White"),AttrModule.DynamicStyle("visibility",View.Map(function(isExpand)
   {
    return isExpand?"visible":"hidden";
   },Client.isExpanded().get_View())),animAttr("font-size","%")],[Doc.TextNode(txt)]);
  }
  return Doc.Element("div",[],[Doc.Element("div",[AttrModule.DynamicStyle("pointer-events",View.Map(function()
  {
   return Client.dlg().Visibility.Get()?"none":"auto";
  },Client.dlg().Visibility.get_View())),AttrModule.DynamicStyle("opacity",View.Map(function()
  {
   return Client.dlg().Visibility.Get()?"0.5":"1";
  },Client.dlg().Visibility.get_View()))],[Doc.Element("table",[],[Doc.Element("tr",[],[Doc.Element("td",[AttrModule.Style("vertical-align","top")],[Doc.Element("table",[animAttr("width","px")],[Doc.Element("tr",[],[Doc.Element("td",[],[Helper.IconNormal("dehaze",function()
  {
   if(!Client.isExpanded().Get())
    Client.isExpanded().Set(true);
   else
    Client.isExpanded().Set(false);
  })])]),Doc.Element("tr",[],[Doc.Element("td",[],[Helper.IconNormal("announcement",function()
  {
   var a;
   a=Doc.Element("div",[],[Doc.TextNode("Content")]);
   Client.dlg().ShowDialog("Dialog title",a,Global.ignore);
  })]),animText("Dialog")]),Doc.Element("tr",[],[Doc.Element("td",[],[Helper.IconNormal("add",function()
  {
   var z_index,childPanelContainer,contentItems,childPanel,titleVar,panel;
   z_index=List.ofSeq(Client.panelContainer().PanelItems).get_Length()+1;
   childPanelContainer=PanelContainer.get_Create().WithLayoutManager(LayoutManagers.StackPanelLayoutManager());
   contentItems=ContentModel.get_Create();
   childPanel=Panel$1.get_Create().WithPannelAttrs([AttrModule.Class("panelContent")]).WithPanelContent(contentItems.get_Render()).WithWidth(150).WithHeight(150);
   childPanel.IsWithTitle.Set(false);
   childPanelContainer.AddPanel(childPanel);
   titleVar=Var.Create$1("Panel "+Global.String(z_index));
   panel=Panel$1.get_Create().WithPannelAttrs([AttrModule.Style("position","absolute")]).WithTitleContent(Doc.TextView(titleVar.get_View())).WithTitleButtons(List.ofArray([TitleButton.New("add",function()
   {
    var index,c;
    index=List.ofSeq(contentItems.Items).get_Length();
    index<7?contentItems.Items.Append(ContentItem.New("Content "+(c=index+1,Global.String(c)))):void 0;
   }),TitleButton.New("edit",function(panel$1)
   {
    panel$1.EditProperties(Client.propertyGrid());
   }),TitleButton.New("clear",function(panel$1)
   {
    Client.panelContainer().PanelItems.Remove(Client.panelContainer().FindPanelItem(panel$1));
   })])).WithChildPanelContainer(childPanelContainer).WithWidth(150).WithInitialAutoLayout().WithProperties(List.ofArray([Properties.string("title1",titleVar)]));
   Client.panelContainer().AddPanel(panel);
  })]),animText("Add Panel")])]),Doc.Element("table",[],[Doc.Element("tr",[],[Doc.Element("td",[],[Client.propertyGrid().get_Render()])])])]),Doc.Element("td",[],[Client.panelContainer().get_Render()])])])]),Doc.Element("div",[],[Doc.Element("table",[],[Doc.Element("tr",[],[Doc.Element("td",[],[Client.dlg().get_Render()])])])])]);
 };
 Client.TransTransition=function()
 {
  SC$1.$cctor();
  return SC$1.TransTransition;
 };
 Client.trans=function()
 {
  SC$1.$cctor();
  return SC$1.trans;
 };
 Client.transTime=function()
 {
  SC$1.$cctor();
  return SC$1.transTime;
 };
 Client.isExpanded=function()
 {
  SC$1.$cctor();
  return SC$1.isExpanded;
 };
 Client.dlg=function()
 {
  SC$1.$cctor();
  return SC$1.dlg;
 };
 Client.propertyGrid=function()
 {
  SC$1.$cctor();
  return SC$1.propertyGrid;
 };
 Client.panelContainer=function()
 {
  SC$1.$cctor();
  return SC$1.panelContainer;
 };
 SC$1.$cctor=function()
 {
  var a,a$1,a$2;
  SC$1.$cctor=Global.ignore;
  SC$1.panelContainer=PanelContainer.get_Create().WithWidth(1800).WithHeight(1400).WithLayoutManager(LayoutManagers.FloatingPanelLayoutManager(5)).WithAttributes([]);
  SC$1.propertyGrid=PropertyGrid$1.get_Create();
  SC$1.dlg=Dialog.get_Create();
  SC$1.isExpanded=Var.Create$1(false);
  SC$1.transTime=300;
  SC$1.trans=(a=Interpolation.get_Double(),(a$1=Easing.get_CubicInOut(),(a$2=Client.transTime(),Runtime.Curried(An.Simple,2,[a,a$1,a$2]))));
  SC$1.TransTransition=Trans.Exit(function()
  {
   return((Client.trans())(0))(0);
  },Trans.Enter(function()
  {
   return((Client.trans())(0))(0);
  },Trans.Create(function($1,$2)
  {
   return((Client.trans())($1))($2);
  })));
 };
}());
