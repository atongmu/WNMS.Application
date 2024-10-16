// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../core/Evented ../../core/HandleOwner ../../core/maybe ../../core/accessorSupport/decorators/property ../../core/arrayUtils ../../core/has ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass ./DrawOperation ./drawSurfaces ../interactive/InteractiveToolBase".split(" "),function(b,k,d,p,q,l,e,v,w,x,r,t,m,u){b.DrawTool=function(n){function h(a){a=n.call(this,a)||this;a.defaultZ=0;a.elevationInfo=
null;a.hasZ=!0;a.mode=null;a.snapToScene=null;a.type="draw-3d";return a}k._inheritsLoose(h,n);var c=h.prototype;c.initialize=function(){const a=l.unwrapOr(this.elevationInfo,{mode:this.hasZ?"absolute-height":"on-the-ground",offset:0}),g=this.view;this.drawOperation=new t.DrawOperation({view:g,manipulators:this.manipulators,geometryType:this.geometryType,drawingMode:this.mode,hasZ:this.hasZ,defaultZ:this.defaultZ,snapToSceneEnabled:this.snapToScene,drawSurface:"3d"===g.type?new m.SceneDrawSurface(g,
a):null,elevationDrawSurface:"3d"===g.type?new m.ElevationDrawSurface(a,this.defaultZ,g):null,hasM:!1,elevationInfo:a});this.handles.add([this.drawOperation.on("vertex-add",f=>this.onVertexAdd(f)),this.drawOperation.on("vertex-remove",f=>this.onVertexRemove(f)),this.drawOperation.on("vertex-update",f=>this.onVertexUpdate(f)),this.drawOperation.on("cursor-update",f=>this.onCursorUpdate(f)),this.drawOperation.on("complete",f=>this.onComplete(f))]);this.complete()};c.destroy=function(){this.drawOperation=
l.destroyMaybe(this.drawOperation);this._set("view",null)};c.completeCreateOperation=function(){this.drawOperation.complete()};c.onDeactivate=function(){this.drawOperation.isCompleted||this.drawOperation.cancel()};c.getVertexCoords=function(){return this.drawOperation.vertices};c.onInputEvent=function(a){this.drawOperation.onInputEvent(a)};c.redo=function(){this.drawOperation.redo()};c.reset=function(){};c.undo=function(){this.drawOperation.undo()};c.onComplete=function(a){this.emit("complete",a)};
c.onCursorUpdate=function(a){this.emit("cursor-update",a)};c.onVertexAdd=function(a){this.emit("vertex-add",a)};c.onVertexRemove=function(a){this.emit("vertex-remove",a)};c.onVertexUpdate=function(a){this.emit("vertex-update",a)};k._createClass(h,[{key:"canRedo",get:function(){return this.drawOperation.canRedo}},{key:"canUndo",get:function(){return this.drawOperation.canUndo}},{key:"enabled",set:function(a){this.drawOperation.interactive=a;this._set("enabled",a)}},{key:"updating",get:function(){var a,
g;return null!=(a=null==(g=this.drawOperation)?void 0:g.updating)?a:!1}}]);return h}(q.HandleOwnerMixin(p.EventedMixin(u.InteractiveToolBase)));d.__decorate([e.property({constructOnly:!0,nonNullable:!0})],b.DrawTool.prototype,"defaultZ",void 0);d.__decorate([e.property()],b.DrawTool.prototype,"drawOperation",void 0);d.__decorate([e.property({constructOnly:!0})],b.DrawTool.prototype,"elevationInfo",void 0);d.__decorate([e.property({value:!0})],b.DrawTool.prototype,"enabled",null);d.__decorate([e.property({constructOnly:!0})],
b.DrawTool.prototype,"geometryType",void 0);d.__decorate([e.property({constructOnly:!0})],b.DrawTool.prototype,"hasZ",void 0);d.__decorate([e.property({constructOnly:!0})],b.DrawTool.prototype,"mode",void 0);d.__decorate([e.property()],b.DrawTool.prototype,"snapToScene",void 0);d.__decorate([e.property({readOnly:!0})],b.DrawTool.prototype,"type",void 0);d.__decorate([e.property({readOnly:!0})],b.DrawTool.prototype,"updating",null);d.__decorate([e.property({constructOnly:!0,nonNullable:!0})],b.DrawTool.prototype,
"view",void 0);b.DrawTool=d.__decorate([r.subclass("esri.views.draw.DrawTool")],b.DrawTool);Object.defineProperty(b,"__esModule",{value:!0})});