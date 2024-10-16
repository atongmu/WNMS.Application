// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require ../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../core/handleUtils ../../core/maybe ../../core/accessorSupport/decorators/aliasOf ../../core/arrayUtils ../../core/has ../../core/accessorSupport/decorators/cast ../../core/accessorSupport/decorators/property ../../core/accessorSupport/decorators/subclass ../Widget ./widgetUtils ./decorators/messageBundle ../../core/Logger ./jsxFactory ./SelectionToolbar/SelectionToolbarViewModel".split(" "),function(q,r,f,t,u,m,e,E,
v,g,w,x,F,y,G,h,z){const n={lassoTool:!0,rectangleTool:!0},A={createTool:"polygon",createOptions:{mode:"freehand"}},B={createTool:"rectangle"};e=function(p){function l(a,b){var c=p.call(this,a,b)||this;c._viewModelHandlesGroup=null;c.activeToolInfo=null;c.label=void 0;c.layers=[];c.messages=null;c.toolConfigs=[];c.view=null;c.viewModel=new z;c.visibleElements={...n};c._viewModelHandlesGroup=t.handlesGroup([c.viewModel.on("selection-change",k=>c.emit("selection-change",k)),c.viewModel.on("complete",
k=>{c._set("activeToolInfo",null);c.emit("complete",k)})]);return c}r._inheritsLoose(l,p);var d=l.prototype;d.destroy=function(){this._viewModelHandlesGroup=u.removeMaybe(this._viewModelHandlesGroup)};d.loadDependencies=function(){return new Promise((a,b)=>q(["../../chunks/calcite-action"],a,b))};d.castVisibleElements=function(a){return{...n,...a}};d.activate=function(a){this.cancel();switch(a){case "lasso":this._activateTool("lasso");case "rectangle":this._activateTool("rectangle");default:this._activateTool(a)}};
d.cancel=function(){this.viewModel.cancel();this._set("activeToolInfo",null)};d.render=function(){return h.tsx("div",{"aria-label":this.label,class:this.classes("esri-selection-toolbar","esri-widget")},h.tsx("div",{class:"esri-selection-toolbar__container"},this.renderDefaultTools(),this.renderCustomTools()))};d.renderDefaultTools=function(){var a;if("2d"===(null==(a=this.view)?void 0:a.type))return[this.renderRectangleTool(),this.renderLassoTool()]};d.renderCustomTools=function(){if(this.toolConfigs&&
this.toolConfigs.length)return this.toolConfigs.map(a=>{var b;return h.tsx("calcite-action",{active:(null==(b=this.activeToolInfo)?void 0:b.toolName)===a.toolName,bind:this,class:"esri-selection-toolbar__tool-button",icon:a.icon||"selection",label:a.toolName,onclick:()=>this._onCustomToolClick(a.toolName),scale:"s",text:a.toolName,title:a.toolName})})};d.renderLassoTool=function(){const {activeToolInfo:a,messages:b,visibleElements:c}=this;if(c.lassoTool)return h.tsx("calcite-action",{active:"lasso"===
(null==a?void 0:a.toolName),bind:this,icon:"lasso",label:b.selectByLasso,onclick:this._onLassoToolClick,scale:"s",text:b.selectByLasso,title:b.selectByLasso})};d.renderRectangleTool=function(){const {activeToolInfo:a,messages:b,visibleElements:c}=this;if(c.rectangleTool)return h.tsx("calcite-action",{active:"rectangle"===(null==a?void 0:a.toolName),bind:this,icon:"cursor-marquee",label:b.selectByRectangle,onclick:this._onRectangleToolClick,scale:"s",text:b.selectByRectangle,title:b.selectByRectangle})};
d._onCustomToolClick=function(a){this._toggleTool(a)};d._onLassoToolClick=function(){this._toggleTool("lasso")};d._onRectangleToolClick=function(){this._toggleTool("rectangle")};d._activateTool=function(a){var b=this._getToolOptions(a);b&&(b=this.viewModel.activate(b),this._set("activeToolInfo",{toolName:a,operation:b}))};d._toggleTool=function(a){if(this.activeToolInfo){const b=this.activeToolInfo.toolName;this.cancel();if(b===a)return}this._activateTool(a)};d._getToolOptions=function(a){if("lasso"===
a)return A;if("rectangle"===a)return B;const b=this.toolConfigs.find(C=>C.toolName===a);if(b){var {createTool:c,createOptions:k,selectionOptions:D}=b;return{createTool:c,createOptions:k,selectionOptions:D}}};return l}(x);f.__decorate([g.property({readOnly:!0})],e.prototype,"activeToolInfo",void 0);f.__decorate([g.property({aliasOf:{source:"messages.widgetLabel",overridable:!0}})],e.prototype,"label",void 0);f.__decorate([m.aliasOf("viewModel.layers")],e.prototype,"layers",void 0);f.__decorate([g.property(),
y.messageBundle("esri/widgets/support/SelectionToolbar/t9n/SelectionToolbar")],e.prototype,"messages",void 0);f.__decorate([g.property()],e.prototype,"toolConfigs",void 0);f.__decorate([m.aliasOf("viewModel.view")],e.prototype,"view",void 0);f.__decorate([g.property()],e.prototype,"viewModel",void 0);f.__decorate([g.property()],e.prototype,"visibleElements",void 0);f.__decorate([v.cast("visibleElements")],e.prototype,"castVisibleElements",null);return e=f.__decorate([w.subclass("esri.widgets.support.SelectionToolbar")],
e)});