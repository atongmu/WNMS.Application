// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../core/JSONSupport ../../core/accessorSupport/decorators/property ../../core/arrayUtils ../../core/has ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass".split(" "),function(a,h,c,k,f,m,n,p,l){var d;a.CodedValue=d=function(g){function e(b){b=g.call(this,b)||this;b.name=null;b.code=null;return b}h._inheritsLoose(e,g);e.prototype.clone=function(){return new d({name:this.name,code:this.code})};
return e}(k.JSONSupport);c.__decorate([f.property({type:String,json:{write:!0}})],a.CodedValue.prototype,"name",void 0);c.__decorate([f.property({type:[String,Number],json:{write:!0}})],a.CodedValue.prototype,"code",void 0);a.CodedValue=d=c.__decorate([l.subclass("esri.layers.support.CodedValue")],a.CodedValue);a.default=a.CodedValue;Object.defineProperty(a,"__esModule",{value:!0})});