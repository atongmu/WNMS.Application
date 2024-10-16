// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/Accessor ../../../../core/Handles ../../../../core/watchUtils ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../../geometry/projectionEllipsoid ../../support/FrustumExtentIntersection ../../../support/Scheduler".split(" "),function(h,g,d,p,k,l,
v,w,x,q,m,r,t){d=function(n){function f(){var a=n.apply(this,arguments)||this;a.suspended=!1;a.extent=null;a.extentIntersectionDirty=!0;a._isVisibleBelowSurface=!1;a.handles=new p;a.layerView=null;a.updating=!0;return a}h._inheritsLoose(f,n);var b=f.prototype;b.setup=function(a){this.layerView=a;this.extentIntersection=new r.FrustumExtentIntersection({renderCoordsHelper:a.view.renderCoordsHelper});a=a.view;const c=a.basemapTerrain,e=a.resourceController.scheduler;this.handles.add([a.on("resize",()=>
this.viewChange()),a.state.watch("camera",()=>this.viewChange(),!0),e.registerTask(t.TaskPriority.FRUSTUM_VISIBILITY,this),c.on("elevation-bounds-change",()=>this.elevationBoundsChange())]);"local"===a.viewingMode?this.isVisibleBelowSurface=!0:this.handles.add([k.init(c,["opacity","wireframe"],()=>this.updateIsVisibleBelowSurface()),k.init(a,"map.ground.navigationConstraint.type",()=>this.updateIsVisibleBelowSurface())])};b.destroy=function(){this.extentIntersection=this.extent=this.layerView=null;
this.handles&&(this.handles.destroy(),this.handles=null)};b._setDirty=function(){this.updating||this._set("updating",!0)};b.setExtent=function(a){this.extent=a;this.extentIntersectionDirty=!0;this._setDirty()};b.viewChange=function(){this._setDirty()};b.elevationBoundsChange=function(){this._setDirty();this.extentIntersectionDirty=!0};b.updateIsVisibleBelowSurface=function(){const a=this.layerView.view,c=a.basemapTerrain,e=a.map.ground&&a.map.ground.navigationConstraint&&"none"===a.map.ground.navigationConstraint.type;
this.isVisibleBelowSurface="local"===a.viewingMode||!c.opaque||e};b.updateExtentIntersection=function(){if(this.extentIntersectionDirty){this.extentIntersectionDirty=!1;var a=this.layerView.view;if(this._isVisibleBelowSurface)var c=-.3*m.getReferenceEllipsoid(a.spatialReference).radius;else{const {min:e,max:u}=a.basemapTerrain.elevationBounds;c=e-Math.max(1,(u-e)*(1.2-1))}this.extentIntersection.update(this.extent,a.spatialReference,c)}};b.runTask=function(){this._set("updating",!1);if(this.extent){this.updateExtentIntersection();
var a=this.layerView.view.frustum,c=m.getReferenceEllipsoid(this.layerView.view.spatialReference).radius;this._set("suspended",!this.extentIntersection.isVisibleInFrustum(a,c))}else this._set("suspended",!1)};h._createClass(f,[{key:"isVisibleBelowSurface",set:function(a){this._isVisibleBelowSurface=a;this._setDirty();this.extentIntersectionDirty=!0}},{key:"running",get:function(){return this.updating}}]);return f}(d);g.__decorate([l.property({readOnly:!0})],d.prototype,"suspended",void 0);g.__decorate([l.property({readOnly:!0})],
d.prototype,"updating",void 0);return d=g.__decorate([q.subclass("esri.views.3d.layers.graphics.Graphics3DFrustumVisibility")],d)});