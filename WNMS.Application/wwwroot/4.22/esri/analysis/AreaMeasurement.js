// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ./Analysis ../core/lang ../core/Logger ../core/maybe ../core/unitUtils ../core/accessorSupport/decorators/property ../core/accessorSupport/ensureType ../core/accessorSupport/decorators/subclass ../geometry/Polygon".split(" "),function(g,c,b,k,l,m,n,d,t,p,q){var f;const r=l.getLogger("esri.analysis.AreaMeasurement");b=f=function(h){function e(a){a=h.call(this,a)||this;a.type="area-measurement";a.extent=null;a.unit=null;return a}g._inheritsLoose(e,
h);e.prototype.clone=function(){return new f({geometry:k.clone(this.geometry),unit:this.unit,...this.cloneBaseAnalysisProperties()})};g._createClass(e,[{key:"geometry",set:function(a){m.isNone(a)?(this._set("geometry",null),this._set("extent",null)):(1<a.rings.length&&r.warn("Measuring polygons with multiple rings is not supported."),this._set("geometry",a.clone()),this._set("extent",a.extent))}}]);return e}(b);c.__decorate([d.property({readOnly:!0})],b.prototype,"type",void 0);c.__decorate([d.property({value:null,
type:q})],b.prototype,"geometry",null);c.__decorate([d.property({readOnly:!0})],b.prototype,"extent",void 0);c.__decorate([d.property({type:n.measurementAreaUnits,value:null})],b.prototype,"unit",void 0);return b=f=c.__decorate([p.subclass("esri.analysis.AreaMeasurement")],b)});