// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../../chunks/_rollupPluginBabelHelpers ../../../../../chunks/tslib.es6 ../../../../../core/HandleOwner ../../../../../core/mathUtils ../../../../../core/maybe ../../../../../core/reactiveUtils ../../../../../core/accessorSupport/decorators/property ../../../../../core/arrayUtils ../../../../../core/has ../../../../../core/accessorSupport/ensureType ../../../../../core/accessorSupport/decorators/subclass".split(" "),function(g,v,l,w,r,f,t,m,y,z,A,x){g.ScaleRotateObjectSymbol3DAdapter=
function(u){function q(a){a=u.call(this,a)||this;a.angle=0;a.scale=1;a.symbol=null;a.initialAngle=0;return a}v._inheritsLoose(q,u);var n=q.prototype;n.initialize=function(){this.restart()};n.restart=function(){this.handles.remove("scale-heading-update-key");const a=f.isSome(this.graphic.symbol)&&"point-3d"===this.graphic.symbol.type?this.graphic.symbol:f.none;this.symbol=f.isSome(a)?a.clone():null;this.initialSizes=[];if(f.isSome(a)){const d=this.findLayerView();f.isSome(d)&&(this.initialSizes=a.symbolLayers.map(b=>
"object"===b.type?d.getSymbolLayerSize(a,b):null).toArray())}this.angle=0;f.isSome(a)&&a.symbolLayers.some(d=>"object"===d.type&&f.isSome(d.heading)?(this.angle=-r.deg2rad(d.heading),!0):!1);this.initialAngle=this.angle;this.scale=1;this.handles.add(t.react(()=>({angle:this.angle,scale:this.scale}),d=>this.updateSymbol(d),t.sync),"scale-heading-update-key")};n.cancel=function(){this.graphic.symbol=this.symbol};n.createUndoRecord=function(){let a=this.graphic.symbol,d=null;return{undo:b=>{d=b.symbol;
b.symbol=a;this.restart()},redo:b=>{a=b.symbol;b.symbol=d;this.restart()}}};n.updateSymbol=function({scale:a,angle:d}){var b=this.graphic.symbol;if(!f.isNone(this.symbol)&&!f.isNone(b)&&"point-3d"===b.type){b=b.clone();var p=-r.rad2deg(d-this.initialAngle),h=!1;this.forEachObjectSymbolLayerPair(this.symbol,b,(e,k,c)=>{e=f.unwrapOr(e.heading,0)+p;k.heading!==e&&(k.heading=e,h=!0);c=this.initialSizes[c];f.isSome(c)&&"width"in c&&(c.width=this.sizeFilter(c.width),c.height=this.sizeFilter(c.height),c.depth=
this.sizeFilter(c.depth),e=c.width*a,k.width!==e&&(k.width=e,h=!0),e=c.depth*a,k.depth!==e&&(k.depth=e,h=!0),c=c.height*a,k.height!==c&&(k.height=c,h=!0))});h&&(this.graphic.symbol=b)}};n.forEachObjectSymbolLayerPair=function(a,d,b){a.symbolLayers.forEach((p,h)=>{const e=d.symbolLayers.getItemAt(h);"object"===p.type&&"object"===e.type&&b(p,e,h)})};return q}(w.HandleOwner);l.__decorate([m.property()],g.ScaleRotateObjectSymbol3DAdapter.prototype,"angle",void 0);l.__decorate([m.property()],g.ScaleRotateObjectSymbol3DAdapter.prototype,
"scale",void 0);l.__decorate([m.property({constructOnly:!0})],g.ScaleRotateObjectSymbol3DAdapter.prototype,"graphic",void 0);l.__decorate([m.property()],g.ScaleRotateObjectSymbol3DAdapter.prototype,"symbol",void 0);l.__decorate([m.property({constructOnly:!0})],g.ScaleRotateObjectSymbol3DAdapter.prototype,"findLayerView",void 0);l.__decorate([m.property({constructOnly:!0})],g.ScaleRotateObjectSymbol3DAdapter.prototype,"sizeFilter",void 0);g.ScaleRotateObjectSymbol3DAdapter=l.__decorate([x.subclass("esri.views.3d.interactive.editingTools.transformGraphic.ScaleRotateObjectSymbol3DAdapter")],
g.ScaleRotateObjectSymbol3DAdapter);Object.defineProperty(g,"__esModule",{value:!0})});