// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../core/maybe"],function(c,d){c.areToolManipulatorsEditable=function(b){return b.visible&&b.getEditableFlag(0)&&b.getEditableFlag(1)};c.evaluateToolConstructorArguments=function(b){return d.isNone(b)?{}:"function"===typeof b?b():b};c.getToolAttachDetachHandles=function(b){return[b.on("after-add",a=>{a=a.item;a.view&&a.view.ready&&!a.attached&&a.attach()}),b.on("after-remove",a=>{a=a.item;a.active&&(a.view.activeTool=null);a.attached&&a.detach()})]};Object.defineProperty(c,"__esModule",
{value:!0})});