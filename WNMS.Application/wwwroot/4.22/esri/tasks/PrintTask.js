// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ../core/jsonMap ../core/accessorSupport/decorators/property ../core/arrayUtils ../core/has ../core/accessorSupport/ensureType ../core/accessorSupport/decorators/subclass ../rest/print ./Task".split(" "),function(g,c,b,h,r,t,u,n,d,p){const q=new b.JSONMap({esriExecutionTypeSynchronous:"sync",esriExecutionTypeAsynchronous:"async"});b=function(l){function e(...a){a=l.call(this,...a)||this;a._gpMetadata=null;a.updateDelay=1E3;return a}g._inheritsLoose(e,
l);var m=e.prototype;m.execute=function(a,f){a&&(a.updateDelay=this.updateDelay);return d.execute(this.url,a,{...this.requestOptions,...f})};m._getGpPrintParams=function(){var a=g._asyncToGenerator(function*(f){var k=d.getGpServerUrl(this.url);k=d.printCacheMap.get(k);return d.getGpPrintParams(f,k)});return function(f){return a.apply(this,arguments)}}();g._createClass(e,[{key:"mode",get:function(){return this._gpMetadata&&this._gpMetadata.executionType?q.fromJSON(this._gpMetadata.executionType):"sync"}}]);
return e}(p);c.__decorate([h.property()],b.prototype,"_gpMetadata",void 0);c.__decorate([h.property({readOnly:!0})],b.prototype,"mode",null);c.__decorate([h.property()],b.prototype,"updateDelay",void 0);return b=c.__decorate([n.subclass("esri.tasks.PrintTask")],b)});