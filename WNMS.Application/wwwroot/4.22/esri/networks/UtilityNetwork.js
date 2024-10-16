// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ../request ../core/accessorSupport/decorators/property ../core/arrayUtils ../core/has ../core/accessorSupport/ensureType ../core/accessorSupport/decorators/subclass ./Network ./support/NamedTraceConfiguration ./support/TerminalConfiguration".split(" "),function(p,f,x,h,e,B,C,y,z,A,u){e=function(r){function q(a){a=r.call(this,a)||this;a.sharedNamedTraceConfigurations=[];a.type="utility";return a}p._inheritsLoose(q,r);var l=q.prototype;
l.load=function(){var a=p._asyncToGenerator(function*(b){this.addResolvingPromise(r.prototype.load.call(this,b));this.addResolvingPromise(this._loadNamedTraceConfigurationsFromNetwork(b));return this});return function(b){return a.apply(this,arguments)}}();l.getTerminalConfiguration=function(a){let b=null,d=null;var c=a.layer;let m=null;if("feature"===(null==c?void 0:c.type)){if(m=c.layerId,null===m)return null}else return null;c=a.attributes;if(null==c)return null;for(const g of Object.keys(c))"ASSETGROUP"===
g.toUpperCase()&&(b=a.getAttribute(g)),"ASSETTYPE"===g.toUpperCase()&&(d=a.getAttribute(g));if(!this.dataElement)return null;let t=null;a=this.dataElement.domainNetworks;for(const g of a){var k;if(a=null==(k=g.junctionSources)?void 0:k.find(n=>n.layerId===m)){var v;if(a=null==(v=a.assetGroups)?void 0:v.find(n=>n.assetGroupCode===b)){var w;if(a=null==(w=a.assetTypes)?void 0:w.find(n=>n.assetTypeCode===d)){t=a.terminalConfigurationId;break}}}}return null!=t?(k=this.dataElement.terminalConfigurations,
(k=null==k?void 0:k.find(g=>g.terminalConfigurationId===t))?u.fromJSON(k):null):null};l.getTierNames=function(a){var b;const d=null==(b=this.dataElement)?void 0:b.domainNetworks.find(c=>c.domainNetworkName===a);return(null==d?void 0:d.tiers.map(c=>c.name))||[]};l._loadNamedTraceConfigurationsFromNetwork=function(){var a=p._asyncToGenerator(function*(b){var d;if(0!==(null==(d=this.sharedNamedTraceConfigurations)?void 0:d.length)){d=this.sharedNamedTraceConfigurations.map(c=>c.globalId);b=yield this._fetchTraceConfigData(this.networkServiceUrl,
d,b);for(const c of this.sharedNamedTraceConfigurations)(d=null==b?void 0:b.find(m=>m.globalId===c.globalId))&&c.read(d,{origin:"service"})}});return function(b){return a.apply(this,arguments)}}();l._fetchTraceConfigData=function(a,b,d){return x(`${a}/traceConfigurations/query`,{responseType:"json",query:{globalIds:JSON.stringify(b),f:"json"},...d}).then(c=>c.data.traceConfigurations)};p._createClass(q,[{key:"serviceTerritoryFeatureLayerId",get:function(){var a;return null==(a=this.dataElement)?void 0:
a.serviceTerritoryFeatureLayerId}},{key:"rulesTableId",get:function(){var a;return null==(a=this.sourceJSON)?void 0:a.systemLayers.rulesTableId}},{key:"rulesTableUrl",get:function(){return this.sourceJSON?`${this.featureServiceUrl}/${this.rulesTableId}`:null}},{key:"terminalConfigurations",get:function(){var a;return(null==(a=this.dataElement)?void 0:a.terminalConfigurations.map(b=>u.fromJSON(b)))||[]}},{key:"domainNetworkNames",get:function(){var a;return(null==(a=this.dataElement)?void 0:a.domainNetworks.map(b=>
b.domainNetworkName))||[]}}]);return q}(z);f.__decorate([h.property({type:[A],json:{origins:{"web-map":{read:{source:"traceConfigurations"},write:{target:"traceConfigurations"}},service:{read:{source:"traceConfigurations"}}},read:!1}})],e.prototype,"sharedNamedTraceConfigurations",void 0);f.__decorate([h.property({type:["utility"],readOnly:!0,json:{read:!1,write:!1}})],e.prototype,"type",void 0);f.__decorate([h.property({readOnly:!0})],e.prototype,"serviceTerritoryFeatureLayerId",null);f.__decorate([h.property({readOnly:!0})],
e.prototype,"rulesTableId",null);f.__decorate([h.property({readOnly:!0})],e.prototype,"rulesTableUrl",null);f.__decorate([h.property({readOnly:!0})],e.prototype,"terminalConfigurations",null);f.__decorate([h.property({readOnly:!0})],e.prototype,"domainNetworkNames",null);return e=f.__decorate([y.subclass("esri.networks.UtilityNetwork")],e)});