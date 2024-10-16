// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require ../../../chunks/_rollupPluginBabelHelpers ../../../chunks/tslib.es6 ../../../core/Accessor ../../../core/Collection ../../../core/Error ../../../core/HandleOwner ../../../core/Logger ../../../core/maybe ../../../core/promiseUtils ../../../core/scheduling ../../../core/accessorSupport/decorators/property ../../../core/accessorSupport/decorators/subclass".split(" "),function(l,r,h,f,w,x,y,z,g,k,A,m,B){const p=n=>Object.freeze({__proto__:null,default:n}),C=z.getLogger("esri.views.3d.analysis.AnalysisViewManager3D");
f=function(n){function q(a){a=n.call(this,a)||this;a._allAnalysisViews=new w;a._creatingViewCount=0;a._items=new Map;a._scheduledUpdateHandle=null;a._analysisModules={"area-measurement":{module:null},"direct-line-measurement":{module:null},"line-of-sight":{module:null},slice:{module:null}};return a}r._inheritsLoose(q,n);var e=q.prototype;e.destroy=function(){this.detach()};e.attach=function(){this.handles.add(this._connectOwner(this.view),"analyses-owner-handles")};e.detach=function(){this.handles.remove("analyses-owner-handles");
this._update();this._creatingViewCount=0};e.whenAnalysisView=function(a){const c=this._items.get(a);return g.isNone(c)||1===c.state.list?(a=new x("AnalysisViewManager:no-analysisview-for-analysis","The analysis has not been added to the scene.",{analysis:a}),Promise.reject(a)):c.createAnalysisViewTask.promise};e._connectOwner=function(a){return this._connectAnalysesCollection(a.analyses)};e._connectAnalysesCollection=function(a){for(const b of a)this._addAnalysis(b);const c=a.on("after-add",b=>this._addAnalysis(b.item)),
d=a.on("after-remove",b=>this._removeAnalysis(b.item));return{remove:()=>{c.remove();d.remove();for(const b of a)this._removeAnalysis(b)}}};e._addAnalysis=function(a){const c=this._items.get(a);if(null==c){const d={state:{view:0,list:0},analysis:a,view:null,createAnalysisViewTask:null};this._items.set(a,d);d.createAnalysisViewTask=k.createTask(b=>this._createAnalysisViewPromise(d,b))}else c.state.list=0};e._removeAnalysis=function(a){a=this._items.get(a);null==a?C.error("Trying to remove analysis which was not added"):
(a.state.list=1,this._scheduleUpdate())};e._scheduleUpdate=function(){g.isSome(this._scheduledUpdateHandle)||(this._scheduledUpdateHandle=A.schedule(()=>this._update()))};e._update=function(){this._scheduledUpdateHandle=g.removeMaybe(this._scheduledUpdateHandle);this._items.forEach(a=>{if(1===a.state.list)switch(this._items.delete(a.analysis),a.state.view){case 0:a.createAnalysisViewTask=g.abortMaybe(a.createAnalysisViewTask);break;case 1:g.isSome(a.view)&&(this._allAnalysisViews.remove(a.view),a.view=
g.destroyMaybe(a.view),a.createAnalysisViewTask=null)}})};e._createAnalysisViewPromise=function(){var a=r._asyncToGenerator(function*(c,d){var b=c.analysis;const v=b.type,t=this._analysisModules[v];this._creatingViewCount+=1;if(g.isNone(t.module))try{t.module=yield this._loadAnalysisModule(v)}catch(u){throw--this._creatingViewCount,u;}if(k.isAborted(d))throw--this._creatingViewCount,k.createAbortError("AnalysisView creation aborted");b=new t.module.default({analysis:b,view:this.view});try{yield b.when()}catch(u){throw--this._creatingViewCount,
u;}if(k.isAborted(d))throw--this._creatingViewCount,b.destroy(),k.createAbortError("AnalysisView creation aborted");c.view=b;c.state.view=1;this._allAnalysisViews.add(b);--this._creatingViewCount;return b});return function(c,d){return a.apply(this,arguments)}}();e._loadAnalysisModule=function(a){switch(a){case "area-measurement":return new Promise((c,d)=>l(["./AreaMeasurement/AreaMeasurementView3D"],b=>c(p(b)),d));case "direct-line-measurement":return new Promise((c,d)=>l(["./DirectLineMeasurement/DirectLineMeasurementView3D"],
b=>c(p(b)),d));case "line-of-sight":return new Promise((c,d)=>l(["./LineOfSight/LineOfSightView3D"],b=>c(p(b)),d));case "slice":return new Promise((c,d)=>l(["./Slice/SliceView3D"],b=>c(p(b)),d));default:return null}};r._createClass(q,[{key:"updating",get:function(){return!this.view.ready||0!==this._creatingViewCount||this._allAnalysisViews.some(a=>a.updating)}},{key:"testInfo",get:function(){return{allAnalysisViews:this._allAnalysisViews}}}]);return q}(y.HandleOwnerMixin(f));h.__decorate([m.property()],
f.prototype,"updating",null);h.__decorate([m.property({constructOnly:!0})],f.prototype,"view",void 0);h.__decorate([m.property()],f.prototype,"_allAnalysisViews",void 0);h.__decorate([m.property()],f.prototype,"_creatingViewCount",void 0);return f=h.__decorate([B.subclass("esri.views.3d.analysis.AnalysisViewManager3D")],f)});