// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../geometry ../../core/Evented ../../core/accessorSupport/decorators/property ../../core/arrayUtils ../../core/has ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass ../../layers/support/LOD ./constraints/GeometryConstraint ./constraints/RotationConstraint ./constraints/ZoomConstraint ../../geometry/Extent ../../geometry/Polygon".split(" "),function(l,c,m,b,d,A,B,C,w,x,n,p,q,y,z){m={base:null,
key:"type",typeMap:{extent:y,polygon:z}};b=function(r){function k(){var a=r.apply(this,arguments)||this;a.lods=null;a.minScale=0;a.maxScale=0;a.minZoom=-1;a.maxZoom=-1;a.rotationEnabled=!0;a.snapToZoom=!0;return a}l._inheritsLoose(k,r);var f=k.prototype;f.initialize=function(){this.watch(["_zoom","_rotation"],this.emit.bind(this,"update"),!0)};f.destroy=function(){this.view=null;this._set("_zoom",null);this._set("_rotation",null)};f.canZoomInTo=function(a){const e=this.effectiveMaxScale;return 0===
e||a>=e};f.canZoomOutTo=function(a){const e=this.effectiveMinScale;return 0===e||a<=e};f.constrain=function(a,e){this._zoom.constrain(a,e);this._rotation.constrain(a,e);this._geometry.constrain(a,e);return a};f.constrainByGeometry=function(a){return this._geometry.constrain(a)};f.fit=function(a){return this._zoom.fit(a)};f.zoomToScale=function(a){return this._zoom.zoomToScale(a)};f.scaleToZoom=function(a){return this._zoom.scaleToZoom(a)};f.snapScale=function(a){return this._zoom.snapToClosestScale(a)};
f.snapToNextScale=function(a){return this._zoom.snapToNextScale(a)};f.snapToPreviousScale=function(a){return this._zoom.snapToPreviousScale(a)};l._createClass(k,[{key:"geometry",set:function(a){a?this._set("geometry",a):this._set("geometry",null)}},{key:"_defaultLODs",get:function(){var a,e,h;const g=null==(a=this.view)?void 0:null==(e=a.defaultsFromMap)?void 0:e.tileInfo;a=null==(h=this.view)?void 0:h.spatialReference;return g&&a&&g.spatialReference.equals(a)?g.lods:null}},{key:"_geometry",get:function(){return new n.GeometryConstraint({geometry:this.geometry,
spatialReference:this.view.spatialReference})}},{key:"_rotation",get:function(){return new p({rotationEnabled:this.rotationEnabled})}},{key:"_zoom",get:function(){const a=this._get("_zoom"),e=this.lods||this._defaultLODs,h=this.minZoom,g=this.maxZoom,t=this.minScale,u=this.maxScale,v=this.snapToZoom;return a&&a.lods===e&&a.minZoom===h&&a.maxZoom===g&&a.minScale===t&&a.maxScale===u&&a.snapToZoom===v?a:new q({lods:e,minZoom:h,maxZoom:g,minScale:t,maxScale:u,snapToZoom:v})}}]);return k}(b.EventedAccessor);
c.__decorate([d.property({readOnly:!0,aliasOf:"_zoom.effectiveLODs"})],b.prototype,"effectiveLODs",void 0);c.__decorate([d.property({readOnly:!0,aliasOf:"_zoom.effectiveMinScale"})],b.prototype,"effectiveMinScale",void 0);c.__decorate([d.property({readOnly:!0,aliasOf:"_zoom.effectiveMaxScale"})],b.prototype,"effectiveMaxScale",void 0);c.__decorate([d.property({readOnly:!0,aliasOf:"_zoom.effectiveMinZoom"})],b.prototype,"effectiveMinZoom",void 0);c.__decorate([d.property({readOnly:!0,aliasOf:"_zoom.effectiveMaxZoom"})],
b.prototype,"effectiveMaxZoom",void 0);c.__decorate([d.property({types:m,value:null})],b.prototype,"geometry",null);c.__decorate([d.property({type:[x]})],b.prototype,"lods",void 0);c.__decorate([d.property()],b.prototype,"minScale",void 0);c.__decorate([d.property()],b.prototype,"maxScale",void 0);c.__decorate([d.property()],b.prototype,"minZoom",void 0);c.__decorate([d.property()],b.prototype,"maxZoom",void 0);c.__decorate([d.property()],b.prototype,"rotationEnabled",void 0);c.__decorate([d.property()],
b.prototype,"snapToZoom",void 0);c.__decorate([d.property()],b.prototype,"view",void 0);c.__decorate([d.property()],b.prototype,"_defaultLODs",null);c.__decorate([d.property({type:n.GeometryConstraint})],b.prototype,"_geometry",null);c.__decorate([d.property({type:p})],b.prototype,"_rotation",null);c.__decorate([d.property({readOnly:!0,type:q})],b.prototype,"_zoom",null);return b=c.__decorate([w.subclass("esri.views.2d.MapViewConstraints")],b)});