// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../../chunks/_rollupPluginBabelHelpers ../../../chunks/tslib.es6 ../../../core/accessorSupport/decorators/property ../../../core/arrayUtils ../../../core/has ../../../core/accessorSupport/ensureType ../../../core/accessorSupport/decorators/subclass ./VisualVariableLegendOptions".split(" "),function(g,d,h,a,m,n,k,l){var b;a=b=function(e){function c(){var f=e.apply(this,arguments)||this;f.customValues=null;return f}g._inheritsLoose(c,e);c.prototype.clone=function(){return new b({title:this.title,
showLegend:this.showLegend,customValues:this.customValues&&this.customValues.slice(0)})};return c}(l);d.__decorate([h.property({type:[Number],json:{write:!0}})],a.prototype,"customValues",void 0);return a=b=d.__decorate([k.subclass("esri.renderers.visualVariables.support.SizeVariableLegendOptions")],a)});