// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../core/JSONSupport ../../core/lang ../../core/accessorSupport/decorators/property ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass ./SearchTableField".split(" "),function(h,c,a,k,f,n,l,m){var d;a=d=function(g){function e(b){b=g.call(this,b)||this;b.field=null;b.id=null;return b}h._inheritsLoose(e,g);e.prototype.clone=function(){return new d(k.clone({field:this.field,id:this.id}))};return e}(a.JSONSupport);
c.__decorate([f.property({type:m,json:{write:{isRequired:!0}}})],a.prototype,"field",void 0);c.__decorate([f.property({type:String,json:{write:{isRequired:!0}}})],a.prototype,"id",void 0);return a=d=c.__decorate([l.subclass("esri.webdoc.applicationProperties.SearchTable")],a)});