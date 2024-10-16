// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../chunks/_rollupPluginBabelHelpers ../../../chunks/tslib.es6 ../../../core/accessorSupport/decorators/property ../../../core/arrayUtils ../../../core/has ../../../core/accessorSupport/ensureType ../../../core/accessorSupport/decorators/enumeration ../../../core/accessorSupport/decorators/subclass ./PointSizeAlgorithm".split(" "),function(a,g,b,h,n,p,q,k,l,m){var c;a.PointSizeSplatAlgorithm=c=function(f){function d(){var e=f.apply(this,arguments)||this;e.type="splat";e.scaleFactor=
1;return e}g._inheritsLoose(d,f);d.prototype.clone=function(){return new c({scaleFactor:this.scaleFactor})};return d}(m.default);b.__decorate([k.enumeration({pointCloudSplatAlgorithm:"splat"})],a.PointSizeSplatAlgorithm.prototype,"type",void 0);b.__decorate([h.property({type:Number,value:1,nonNullable:!0,json:{write:!0}})],a.PointSizeSplatAlgorithm.prototype,"scaleFactor",void 0);a.PointSizeSplatAlgorithm=c=b.__decorate([l.subclass("esri.renderers.support.pointCloud.PointSizeSplatAlgorithm")],a.PointSizeSplatAlgorithm);
a.default=a.PointSizeSplatAlgorithm;Object.defineProperty(a,"__esModule",{value:!0})});