// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require ../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ../request ../core/Error ../core/Logger ../core/maybe ../core/MultiOriginJSONSupport ../core/promiseUtils ../core/urlUtils ../core/accessorSupport/decorators/property ../core/arrayUtils ../core/has ../core/accessorSupport/ensureType ../core/accessorSupport/decorators/reader ../core/accessorSupport/decorators/subclass ../geometry/HeightModelInfo ./Layer ./mixins/ArcGISCachedService ./mixins/ArcGISService ./mixins/OperationalLayer ./mixins/PortalLayer ./support/commonProperties ./support/LercDecoder".split(" "),
function(t,m,f,u,x,d,q,y,r,z,g,L,M,N,A,B,C,D,E,F,G,H,I,J){const v=d.getLogger("esri.layers.ElevationLayer");d=function(w){function p(...a){a=w.call(this,...a)||this;a.copyright=null;a.heightModelInfo=null;a.path=null;a.opacity=1;a.operationalLayerType="ArcGISTiledElevationServiceLayer";a.sourceJSON=null;a.type="elevation";a.url=null;a.version=null;a._lercDecoder=J.acquireDecoder();return a}m._inheritsLoose(p,w);var h=p.prototype;h.normalizeCtorArgs=function(a,b){return"string"===typeof a?{url:a,...b}:
a};h.destroy=function(){this._lercDecoder=q.releaseMaybe(this._lercDecoder)};h.readVersion=function(a,b){(a=b.currentVersion)||(a=9.3);return a};h.load=function(a){const b=q.isSome(a)?a.signal:null;this.addResolvingPromise(this.loadFromPortal({supportedTypes:["Image Service"],supportsData:!1,validateItem:e=>{for(let c=0;c<e.typeKeywords.length;c++)if("elevation 3d layer"===e.typeKeywords[c].toLowerCase())return!0;throw new x("portal:invalid-layer-item-type","Invalid layer item type '${type}', expected '${expectedType}' ",
{type:"Image Service",expectedType:"Image Service Elevation 3D Layer"});}},a).catch(r.throwIfAbortError).then(()=>this._fetchImageService(b)));return Promise.resolve(this)};h.fetchTile=function(a,b,e,c){c=c||{signal:null};const l=q.isSome(c.signal)?c.signal:c.signal=(new AbortController).signal,n={responseType:"array-buffer",signal:l},K={noDataValue:c.noDataValue,returnFileInfo:!0};return this.load().then(()=>this._fetchTileAvailability(a,b,e,c)).then(()=>u(this.getTileUrl(a,b,e),n)).then(k=>this._lercDecoder.decode(k.data,
K,l)).then(k=>({values:k.pixelData,width:k.width,height:k.height,maxZError:k.fileInfo.maxZError,noDataValue:k.noDataValue,minValue:k.minValue,maxValue:k.maxValue}))};h.getTileUrl=function(a,b,e){const c=z.objectToQuery({...this.parsedUrl.query,blankTile:!this.tilemapCache&&this.supportsBlankTile?!1:null});return`${this.parsedUrl.path}/tile/${a}/${b}/${e}${c?"?"+c:""}`};h.queryElevation=function(){var a=m._asyncToGenerator(function*(b,e){const {ElevationQuery:c}=yield new Promise((l,n)=>t(["./support/ElevationQuery"],
l,n));r.throwIfAborted(e);return(new c).query(this,b,e)});return function(b,e){return a.apply(this,arguments)}}();h.createElevationSampler=function(){var a=m._asyncToGenerator(function*(b,e){const {ElevationQuery:c}=yield new Promise((l,n)=>t(["./support/ElevationQuery"],l,n));r.throwIfAborted(e);return(new c).createSampler(this,b,e)});return function(b,e){return a.apply(this,arguments)}}();h._fetchTileAvailability=function(a,b,e,c){return this.tilemapCache?this.tilemapCache.fetchAvailability(a,b,
e,c):Promise.resolve("unknown")};h._fetchImageService=function(){var a=m._asyncToGenerator(function*(b){if(this.sourceJSON)return this.sourceJSON;b=yield u(this.parsedUrl.path,{query:{f:"json",...this.parsedUrl.query},responseType:"json",signal:b});b.ssl&&(this.url=this.url.replace(/^http:/i,"https:"));this.sourceJSON=b.data;this.read(b.data,{origin:"service",url:this.parsedUrl})});return function(b){return a.apply(this,arguments)}}();m._createClass(p,[{key:"minScale",get:function(){},set:function(a){this.constructed&&
v.warn(`${this.declaredClass}.minScale support has been removed (since 4.5)`)}},{key:"maxScale",get:function(){},set:function(a){this.constructed&&v.warn(`${this.declaredClass}.maxScale support has been removed (since 4.5)`)}},{key:"hasOverriddenFetchTile",get:function(){return!this.fetchTile.__isDefault__}}]);return p}(E.ArcGISCachedService(F.ArcGISService(G.OperationalLayer(H.PortalLayer(y.MultiOriginJSONMixin(D))))));f.__decorate([g.property({json:{read:{source:"copyrightText"}}})],d.prototype,
"copyright",void 0);f.__decorate([g.property({readOnly:!0,type:C})],d.prototype,"heightModelInfo",void 0);f.__decorate([g.property({type:String,json:{origins:{"web-scene":{read:!0,write:!0}},read:!1}})],d.prototype,"path",void 0);f.__decorate([g.property({type:["show","hide"]})],d.prototype,"listMode",void 0);f.__decorate([g.property({json:{read:!1,write:!1,origins:{service:{read:!1,write:!1},"portal-item":{read:!1,write:!1},"web-document":{read:!1,write:!1}}}})],d.prototype,"minScale",null);f.__decorate([g.property({json:{read:!1,
write:!1,origins:{service:{read:!1,write:!1},"portal-item":{read:!1,write:!1},"web-document":{read:!1,write:!1}}}})],d.prototype,"maxScale",null);f.__decorate([g.property({json:{read:!1,write:!1,origins:{"web-document":{read:!1,write:!1}}}})],d.prototype,"opacity",void 0);f.__decorate([g.property({type:["ArcGISTiledElevationServiceLayer"]})],d.prototype,"operationalLayerType",void 0);f.__decorate([g.property()],d.prototype,"sourceJSON",void 0);f.__decorate([g.property({json:{read:!1},value:"elevation",
readOnly:!0})],d.prototype,"type",void 0);f.__decorate([g.property(I.url)],d.prototype,"url",void 0);f.__decorate([g.property()],d.prototype,"version",void 0);f.__decorate([A.reader("version",["currentVersion"])],d.prototype,"readVersion",null);d=f.__decorate([B.subclass("esri.layers.ElevationLayer")],d);d.prototype.fetchTile.__isDefault__=!0;return d});