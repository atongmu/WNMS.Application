// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../core/JSONSupport ../../core/lang ../../core/accessorSupport/decorators/property ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass".split(" "),function(h,b,a,k,f,m,l){var c;a=c=function(g){function d(){var e=g.apply(this,arguments)||this;e.filterType=null;e.filterValues=null;return e}h._inheritsLoose(d,g);d.prototype.clone=function(){return new c({filterType:this.filterType,filterValues:k.clone(this.filterValues)})};
return d}(a.JSONSupport);b.__decorate([f.property({type:String,json:{write:!0}})],a.prototype,"filterType",void 0);b.__decorate([f.property({type:[String],json:{write:!0}})],a.prototype,"filterValues",void 0);return a=c=b.__decorate([l.subclass("esri.layers.support.BuildingFilterAuthoringInfoType")],a)});