// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../Graphic ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass".split(" "),function(a,h,f,k,l,n,p,q,m){var b;a.PointGraphic=b=function(c){function d(e){return c.call(this,e)||this}h._inheritsLoose(d,c);var g=d.prototype;g.clone=function(){return new b(this.cloneProperties())};
g.cloneProperties=function(){const {pointCloudMetadata:e}=this;return{...c.prototype.cloneProperties.call(this),pointCloudMetadata:e}};return d}(k);f.__decorate([l.property({constructOnly:!0})],a.PointGraphic.prototype,"pointCloudMetadata",void 0);a.PointGraphic=b=f.__decorate([m.subclass("esri.views.3d.layers.i3s.PointGraphic")],a.PointGraphic);Object.defineProperty(a,"__esModule",{value:!0})});