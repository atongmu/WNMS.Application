// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../../../../chunks/_rollupPluginBabelHelpers ../../../../../chunks/tslib.es6 ../../../../../geometry ../../../../../core/Handles ../../../../../core/maybe ../../../../../core/reactiveUtils ../../../../../core/accessorSupport/decorators/property ../../../../../core/arrayUtils ../../../../../core/has ../../../../../core/accessorSupport/ensureType ../../../../../core/accessorSupport/decorators/subclass ../../../../../layers/graphics/hydratedFeatures ../../editingTools/dragEventPipeline3D ./DirectLineMeasurement3DView ./PickRequest ./PickResult ../support/measurementUtils ../../../../interactive/dragEventPipeline ../../../../interactive/InteractiveToolBase ../../../../support/screenUtils ../../../../../geometry/Point".split(" "),
function(t,e,d,A,h,u,f,L,M,N,B,C,v,D,E,F,G,p,H,w,I){d=function(x){function n(a){a=x.call(this,a)||this;a._handles=new A;a._cachedPickRequest=new E.PickRequest;a._cachedPickResult=new F.PickResult;a._isAnyPointerDown=!1;a.lineState="initial";a.startPointSurfaceLocation=null;a.endPointSurfaceLocation=null;a.startManipulator=null;a.endManipulator=null;return a}t._inheritsLoose(n,x);var c=n.prototype;c.initialize=function(){this.measurementView=new D.DirectLineMeasurement3DView({toolState:this,view:this.view,
analysis:this.analysis,cursorPoint:this.cursorPoint});this.measurementView.when().then(()=>this._initialize())};c._initialize=function(){const {start:a,end:b}=this.measurementView.createManipulators(),m=(g,k,y)=>p.createManipulatorDragEventPipeline(g,(q,J,K)=>{const z=v.hideManipulatorWhileDragging(q);J.next(z).next(v.screenToMap3D(this.view)).next(l=>"start"!==l.action?l:null).next(l=>{const r=C.clonePoint(l.mapEnd,new I);this.analysis[k]=r;q.location=r;this[y]=this._surfaceLocation(r,l.surfaceType)});
K.next(z).next(p.resetProperties(this.analysis,[k])).next(p.resetProperties(this,[y])).next(()=>{q.location=h.unwrap(this.analysis[k])})});this._handles.add([m(a,"startPoint","startPointSurfaceLocation"),m(b,"endPoint","endPointSurfaceLocation")]);[a,b].forEach(g=>{this._handles.add([g.events.on("grab-changed",()=>{const k=a.grabbing||b.grabbing;k&&"measured"===this.lineState&&(this.lineState="editing");k||(this.measurementView.finishMeasurement(),"editing"===this.lineState&&(this.lineState="measured"))})])});
this.manipulators.add(a);this.manipulators.add(b);this.startManipulator=a;this.endManipulator=b;this._handles.add(u.react(()=>this.state,g=>{"measured"===g?this.complete():this.finishToolCreation()},u.sync));"measured"===this.state?this.complete():this.startToolCreation()};c.destroy=function(){this.measurementView.destroy();this._set("measurementView",null);this._handles=h.destroyMaybe(this._handles)};c.onDetach=function(){this.analysis.startPoint=null;this.cursorPoint=this.analysis.endPoint=null;
this.measurementView.clearMeasurement()};c.onShow=function(){this.measurementView.show();this.created&&this._updateManipulatorVisibility()};c.onHide=function(){this.created&&this.measurementView.hide()};c.onInputEvent=function(a){switch(a.type){case "immediate-click":this._handleImmediateClick(a);break;case "pointer-move":this._handlePointerMove(a);break;case "pointer-down":this._handlePointerDown();break;case "pointer-up":this._handlePointerUp()}this._updateManipulatorVisibility()};c._handlePointerMove=
function(a){this._clearCachedPickRequest();const b=w.createScreenPointFromEvent(a);"mouse"===a.pointerType&&(this._hoverAt(b),"drawing"===this.lineState&&(this.endManipulator.events.emit("drag",{action:"update",start:b,screenPoint:b}),a.stopPropagation()))};c._handlePointerDown=function(){this._isAnyPointerDown=!0};c._handlePointerUp=function(){this._isAnyPointerDown=!1};c._handleImmediateClick=function(a){this._clearCachedPickRequest();if(G.isPrimaryPointerAction(a)){var b=w.createScreenPointFromEvent(a),
m=a.pointerType,g=!1;if(this.active)switch(this.lineState){case "initial":this.startManipulator.events.emit("drag",{action:"start",pointerType:m,start:b,screenPoint:b});this.startManipulator.events.emit("drag",{action:"end",start:b,screenPoint:b});h.isSome(this.analysis.startPoint)&&(this.startManipulator.interactive=!1,this.endManipulator.interactive=!1,this.lineState="drawing",this.endManipulator.events.emit("drag",{action:"start",pointerType:m,start:b,screenPoint:b}),g=!0);break;case "drawing":this.endManipulator.events.emit("drag",
{action:"update",start:b,screenPoint:b}),h.isSome(this.analysis.endPoint)&&(this.endManipulator.events.emit("drag",{action:"end",start:b,screenPoint:b}),this.startManipulator.interactive=!0,this.endManipulator.interactive=!0,this.measurementView.finishMeasurement(),g=!0)}g&&a.stopPropagation();"mouse"===a.pointerType&&this._hoverAt(b)}};c._hoverAt=function(a){const b=this._isAnyPointerDown&&"drawing"!==this.lineState&&"editing"!==this.lineState;this.measurementView.requiresCursorPoint&&!b?(a=this._pick(a),
a.mapPoint&&(this.cursorPoint=a.mapPoint)):this.cursorPoint=null};c._pick=function(a){const b=this._cachedPickRequest.screenPoint;if(b&&b.x===a.x&&b.y===a.y)return this._cachedPickResult;this._cachedPickRequest.screenPoint=a;return this._cachedPickResult=this.measurementView.pick(this._cachedPickRequest)};c._clearCachedPickRequest=function(){this._cachedPickRequest.screenPoint=null};c._surfaceLocation=function(a,b){return 0===b?"on-the-surface":a.z>=this.measurementView.getElevation(a)?"above-the-surface":
"below-the-surface"};c._updateManipulatorVisibility=function(){this.startManipulator.available=h.isSome(this.analysis.startPoint);this.endManipulator.available=h.isSome(this.analysis.endPoint)};t._createClass(n,[{key:"state",get:function(){return this.analysis.startPoint?"measured"===this.lineState?"measured":"measuring":"ready"}},{key:"creating",get:function(){return"initial"===this.lineState||"drawing"===this.lineState}},{key:"cursor",get:function(){return!this.active||"drawing"!==this.lineState&&
"initial"!==this.lineState?null:"crosshair"}},{key:"cursorPoint",set:function(a){this._set("cursorPoint",a);this.measurementView.cursorPoint=a}},{key:"validMeasurement",get:function(){return h.isSome(this.analysis.startPoint)&&h.isSome(this.analysis.endPoint)}}]);return n}(H.InteractiveToolBase);e.__decorate([f.property({readOnly:!0})],d.prototype,"state",null);e.__decorate([f.property()],d.prototype,"lineState",void 0);e.__decorate([f.property({readOnly:!0})],d.prototype,"creating",null);e.__decorate([f.property({readOnly:!0})],
d.prototype,"cursor",null);e.__decorate([f.property()],d.prototype,"cursorPoint",null);e.__decorate([f.property({constructOnly:!0})],d.prototype,"analysis",void 0);e.__decorate([f.property()],d.prototype,"measurementView",void 0);e.__decorate([f.property({constructOnly:!0})],d.prototype,"view",void 0);e.__decorate([f.property({readOnly:!0})],d.prototype,"validMeasurement",null);e.__decorate([f.property({value:null})],d.prototype,"startPointSurfaceLocation",void 0);e.__decorate([f.property({value:null})],
d.prototype,"endPointSurfaceLocation",void 0);return d=e.__decorate([B.subclass("esri.views.3d.interactive.measurementTools.directLineMeasurement3D.DirectLineMeasurement3DTool")],d)});