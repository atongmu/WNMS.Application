/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"../chunks/tslib.es6.js";import t from"../config.js";import{id as r}from"../kernel.js";import o,{s}from"../request.js";import{a as i,b as a}from"../chunks/deprecate.js";import l from"../core/Error.js";import{J as n}from"../chunks/JSONSupport.js";import{L as u}from"../chunks/Loadable.js";import{L as p}from"../chunks/Logger.js";import{i as d}from"../core/lang.js";import{throwIfAborted as h,isAborted as c,createAbortError as y,throwIfAbortError as m}from"../core/promiseUtils.js";import{property as v}from"../core/accessorSupport/decorators/property.js";import{e as f}from"../chunks/ensureType.js";import{r as g}from"../chunks/reader.js";import{subclass as S}from"../core/accessorSupport/decorators/subclass.js";import P from"../geometry/Extent.js";import{g as j}from"../chunks/locale.js";import _ from"./PortalQueryParams.js";import G from"./PortalQueryResult.js";import U from"./PortalUser.js";import"../chunks/object.js";import"../core/urlUtils.js";import"../chunks/string.js";import"../core/Accessor.js";import"../chunks/metadata.js";import"../chunks/handleUtils.js";import"../chunks/ArrayPool.js";import"../core/scheduling.js";import"../chunks/nextTick.js";import"../chunks/Promise.js";import"../geometry/Geometry.js";import"../geometry/SpatialReference.js";import"../chunks/writer.js";import"../geometry/Point.js";import"../core/accessorSupport/decorators/cast.js";import"../geometry/support/webMercatorUtils.js";import"../chunks/Ellipsoid.js";import"../chunks/jsonMap.js";import"./PortalFolder.js";import"./PortalGroup.js";var O;let w;const Q={PortalGroup:()=>import("./PortalGroup.js"),PortalItem:()=>import("./PortalItem.js"),PortalUser:()=>import("./PortalUser.js")};let C=O=class extends(n(u)){constructor(e){super(e),this.access=null,this.allSSL=!1,this.authMode="auto",this.authorizedCrossOriginDomains=null,this.basemapGalleryGroupQuery=null,this.bingKey=null,this.canListApps=!1,this.canListData=!1,this.canListPreProvisionedItems=!1,this.canProvisionDirectPurchase=!1,this.canSearchPublic=!0,this.canShareBingPublic=!1,this.canSharePublic=!1,this.canSignInArcGIS=!1,this.canSignInIDP=!1,this.colorSetsGroupQuery=null,this.commentsEnabled=!1,this.created=null,this.culture=null,this.customBaseUrl=null,this.defaultBasemap=null,this.defaultDevBasemap=null,this.defaultExtent=null,this.defaultVectorBasemap=null,this.description=null,this.devBasemapGalleryGroupQuery=null,this.eueiEnabled=null,this.featuredGroups=null,this.featuredItemsGroupQuery=null,this.galleryTemplatesGroupQuery=null,this.livingAtlasGroupQuery=null,this.hasCategorySchema=!1,this.helperServices=null,this.homePageFeaturedContent=null,this.homePageFeaturedContentCount=null,this.httpPort=null,this.httpsPort=null,this.id=null,this.ipCntryCode=null,this.isPortal=!1,this.isReadOnly=!1,this.layerTemplatesGroupQuery=null,this.maxTokenExpirationMinutes=null,this.modified=null,this.name=null,this.portalHostname=null,this.portalMode=null,this.portalProperties=null,this.region=null,this.rotatorPanels=null,this.showHomePageDescription=!1,this.sourceJSON=null,this.supportsHostedServices=!1,this.symbolSetsGroupQuery=null,this.templatesGroupQuery=null,this.units=null,this.url=t.portalUrl,this.urlKey=null,this.user=null,this.useStandardizedQuery=!1,this.useVectorBasemaps=!1,this.vectorBasemapGalleryGroupQuery=null}normalizeCtorArgs(e){return"string"==typeof e?{url:e}:e}destroy(){this._esriId_credentialCreateHandle&&(this._esriId_credentialCreateHandle.remove(),this._esriId_credentialCreateHandle=null)}readAuthorizedCrossOriginDomains(e){if(e)for(const r of e)-1===t.request.trustedServers.indexOf(r)&&t.request.trustedServers.push(r);return e}readDefaultBasemap(e){return this._readBasemap(e)}readDefaultDevBasemap(e){return this._readBasemap(e)}readDefaultVectorBasemap(e){return this._readBasemap(e)}get extraQuery(){const e=!(this.user&&this.user.orgId)||this.canSearchPublic;return this.id&&!e?` AND orgid:${this.id}`:null}get isOrganization(){return!!this.access}get restUrl(){let e=this.url;if(e){const t=e.indexOf("/sharing");e=t>0?e.substring(0,t):this.url.replace(/\/+$/,""),e+="/sharing/rest"}return e}get stylesGroupQuery(){return i(p.getLogger(this.declaredClass),"stylesGroupQuery",{replacement:"stylesGroupQuery3d",version:"4.21"}),this.stylesGroupQuery3d}get thumbnailUrl(){const e=this.restUrl,t=this.thumbnail;return e&&t?this._normalizeSSL(e+"/portals/self/resources/"+t):null}readUrlKey(e){return e?e.toLowerCase():e}readUser(e){let t=null;return e&&(t=U.fromJSON(e),t.portal=this),t}load(e){const t=import("../Basemap.js").then((e=>e.B)).then((({default:t})=>{h(e),w=t})).then((()=>this.sourceJSON?this.sourceJSON:this._fetchSelf(this.authMode,!1,e))).then((e=>{if(r){const e=r;this.credential=e.findCredential(this.restUrl),this.credential||this.authMode!==O.AUTH_MODE_AUTO||(this._esriId_credentialCreateHandle=e.on("credential-create",(()=>{e.findCredential(this.restUrl)&&this._signIn()})))}this.sourceJSON=e,this.read(e)}));return this.addResolvingPromise(t),Promise.resolve(this)}async createClosestFacilityTask(){a(p.getLogger(this.declaredClass),null,{replacement:"Use helperServices url with esri/rest/closestFacility",version:"4.21"}),await this.load();const e=this._getHelperServiceUrl("closestFacility");return new(0,(await import("../tasks/ClosestFacilityTask.js")).default)(e)}async createElevationLayers(){await this.load();const e=this._getHelperService("defaultElevationLayers"),t=(await import("../layers/ElevationLayer.js")).default;return e?e.map((e=>new t({id:e.id,url:e.url}))):[]}async createGeometryService(){a(p.getLogger(this.declaredClass),null,{replacement:"Use helperServices url with esri/rest/geometryService",version:"4.21"}),await this.load();const e=this._getHelperServiceUrl("geometry");return new(0,(await import("../tasks/GeometryService.js")).default)({url:e})}async createPrintTask(){a(p.getLogger(this.declaredClass),null,{replacement:"Use helperServices url with esri/rest/print",version:"4.21"}),await this.load();const e=this._getHelperServiceUrl("printTask");return new(0,(await import("../tasks/PrintTask.js")).default)(e)}async createRouteTask(){a(p.getLogger(this.declaredClass),null,{replacement:"Use helperServices url with esri/rest/route",version:"4.21"}),await this.load();const e=this._getHelperServiceUrl("route");return new(0,(await import("../tasks/RouteTask.js")).default)(e)}async createServiceAreaTask(){a(p.getLogger(this.declaredClass),null,{replacement:"Use helperServices url with esri/rest/serviceArea",version:"4.21"}),await this.load();const e=this._getHelperServiceUrl("serviceArea");return new(0,(await import("../tasks/ServiceAreaTask.js")).default)(e)}fetchBasemaps(e,r){const o=new _;return o.query=e||(t.apiKey&&s(this.url)?this.devBasemapGalleryGroupQuery:this.useVectorBasemaps?this.vectorBasemapGalleryGroupQuery:this.basemapGalleryGroupQuery),o.disableExtraQuery=!0,this.queryGroups(o,r).then((e=>{if(o.num=100,o.query='type:"Web Map" -type:"Web Application"',e.total){const t=e.results[0];return o.sortField=t.sortField||"name",o.sortOrder=t.sortOrder||"desc",t.queryItems(o,r)}return null})).then((e=>{let t;return t=e&&e.total?e.results.filter((e=>"Web Map"===e.type)).map((e=>new w({portalItem:e}))):[],t}))}fetchCategorySchema(e){return this.hasCategorySchema?this._request(this.restUrl+"/portals/self/categorySchema",e).then((e=>e.categorySchema)):c(e)?Promise.reject(y()):Promise.resolve([])}fetchFeaturedGroups(e){const t=this.featuredGroups,r=new _;if(r.num=100,r.sortField="title",t&&t.length){const o=[];for(const e of t)o.push(`(title:"${e.title}" AND owner:${e.owner})`);return r.query=o.join(" OR "),this.queryGroups(r,e).then((e=>e.results))}return c(e)?Promise.reject(y()):Promise.resolve([])}fetchRegions(e){const t=this.user&&this.user.culture||this.culture||j();return this._request(this.restUrl+"/portals/regions",{...e,query:{culture:t}})}static getDefault(){return O._default&&!O._default.destroyed||(O._default=new O),O._default}queryGroups(e,t){return this._queryPortal("/community/groups",e,"PortalGroup",t)}queryItems(e,t){return this._queryPortal("/search",e,"PortalItem",t)}queryUsers(e,t){return e.sortField||(e.sortField="username"),this._queryPortal("/community/users",e,"PortalUser",t)}toJSON(){throw new l("internal:not-yet-implemented","Portal.toJSON is not yet implemented")}static fromJSON(e){if(!e)return null;if(e.declaredClass)throw new Error("JSON object is already hydrated");return new O({sourceJSON:e})}_getHelperService(e){const t=this.helperServices&&this.helperServices[e];if(!t)throw new l("portal:service-not-found",`The \`helperServices\` do not include an entry named "${e}"`);return t}_getHelperServiceUrl(e){const t=this._getHelperService(e);if(!t.url)throw new l("portal:service-url-not-found",`The \`helperServices\` entry "${e}" does not include a \`url\` value`);return t.url}_fetchSelf(e=this.authMode,t=!1,r){const o=this.restUrl+"/portals/self",s={authMode:e,query:{culture:j().toLowerCase()},...r};return"auto"===s.authMode&&(s.authMode="no-prompt"),t&&(s.query.default=!0),this._request(o,s)}_queryPortal(e,t,r,o){const s=f(_,t),i=t=>this._request(this.restUrl+e,{...s.toRequestOptions(this),...o}).then((e=>{const r=s.clone();return r.start=e.nextStart,new G({nextQueryParams:r,queryParams:s,total:e.total,results:O._resultsToTypedArray(t,{portal:this},e,o)})})).then((e=>Promise.all(e.results.map((t=>"function"==typeof t.when?t.when():e))).then((()=>e),(t=>(m(t),e)))));return r&&Q[r]?Q[r]().then((({default:e})=>(h(o),i(e)))):i()}_signIn(){if(this.authMode===O.AUTH_MODE_ANONYMOUS)return Promise.reject(new l("portal:invalid-auth-mode",`Current "authMode"' is "${this.authMode}"`));if("failed"===this.loadStatus)return Promise.reject(this.loadError);const e=e=>Promise.resolve().then((()=>"not-loaded"===this.loadStatus?(e||(this.authMode="immediate"),this.load().then((()=>null))):"loading"===this.loadStatus?this.load().then((()=>this.credential?null:(this.credential=e,this._fetchSelf("immediate")))):this.user&&this.credential===e?null:(this.credential=e,this._fetchSelf("immediate")))).then((e=>{e&&(this.sourceJSON=e,this.read(e))}));return r?r.getCredential(this.restUrl).then((t=>e(t))):e(this.credential)}_normalizeSSL(e){return e.replace(/^http:/i,"https:").replace(":7080",":7443")}_normalizeUrl(e){const t=this.credential&&this.credential.token;return this._normalizeSSL(t?e+(e.indexOf("?")>-1?"&":"?")+"token="+t:e)}_requestToTypedArray(e,t,r){return this._request(e,t).then((e=>{const t=O._resultsToTypedArray(r,{portal:this},e);return Promise.all(t.map((t=>"function"==typeof t.when?t.when():e))).then((()=>t),(()=>t))}))}_readBasemap(e){if(e){const t=w.fromJSON(e);return t.portalItem={portal:this},t}return null}_request(e,t={}){const r={f:"json",...t.query},{authMode:s=(this.authMode===O.AUTH_MODE_ANONYMOUS?"anonymous":"auto"),body:i=null,cacheBust:a=!1,method:l="auto",responseType:n="json",signal:u}=t,p={authMode:s,body:i,cacheBust:a,method:l,query:r,responseType:n,timeout:0,signal:u};return o(this._normalizeSSL(e),p).then((e=>e.data))}static _resultsToTypedArray(e,t,r,o){let s;if(r){const i=d(o)?o.signal:null;s=r.listings||r.notifications||r.userInvitations||r.tags||r.items||r.groups||r.comments||r.provisions||r.results||r.relatedItems||r,(e||t)&&(s=s.map((r=>{const o=Object.assign(e?e.fromJSON(r):r,t);return"function"==typeof o.load&&o.load(i),o})))}else s=[];return s}};C.AUTH_MODE_ANONYMOUS="anonymous",C.AUTH_MODE_AUTO="auto",C.AUTH_MODE_IMMEDIATE="immediate",e([v()],C.prototype,"access",void 0),e([v()],C.prototype,"allSSL",void 0),e([v()],C.prototype,"authMode",void 0),e([v()],C.prototype,"authorizedCrossOriginDomains",void 0),e([g("authorizedCrossOriginDomains")],C.prototype,"readAuthorizedCrossOriginDomains",null),e([v()],C.prototype,"basemapGalleryGroupQuery",void 0),e([v()],C.prototype,"bingKey",void 0),e([v()],C.prototype,"canListApps",void 0),e([v()],C.prototype,"canListData",void 0),e([v()],C.prototype,"canListPreProvisionedItems",void 0),e([v()],C.prototype,"canProvisionDirectPurchase",void 0),e([v()],C.prototype,"canSearchPublic",void 0),e([v()],C.prototype,"canShareBingPublic",void 0),e([v()],C.prototype,"canSharePublic",void 0),e([v()],C.prototype,"canSignInArcGIS",void 0),e([v()],C.prototype,"canSignInIDP",void 0),e([v()],C.prototype,"colorSetsGroupQuery",void 0),e([v()],C.prototype,"commentsEnabled",void 0),e([v({type:Date})],C.prototype,"created",void 0),e([v()],C.prototype,"credential",void 0),e([v()],C.prototype,"culture",void 0),e([v()],C.prototype,"currentVersion",void 0),e([v()],C.prototype,"customBaseUrl",void 0),e([v()],C.prototype,"defaultBasemap",void 0),e([g("defaultBasemap")],C.prototype,"readDefaultBasemap",null),e([v()],C.prototype,"defaultDevBasemap",void 0),e([g("defaultDevBasemap")],C.prototype,"readDefaultDevBasemap",null),e([v({type:P})],C.prototype,"defaultExtent",void 0),e([v()],C.prototype,"defaultVectorBasemap",void 0),e([g("defaultVectorBasemap")],C.prototype,"readDefaultVectorBasemap",null),e([v()],C.prototype,"description",void 0),e([v()],C.prototype,"devBasemapGalleryGroupQuery",void 0),e([v()],C.prototype,"eueiEnabled",void 0),e([v({readOnly:!0})],C.prototype,"extraQuery",null),e([v()],C.prototype,"featuredGroups",void 0),e([v()],C.prototype,"featuredItemsGroupQuery",void 0),e([v()],C.prototype,"galleryTemplatesGroupQuery",void 0),e([v()],C.prototype,"livingAtlasGroupQuery",void 0),e([v()],C.prototype,"hasCategorySchema",void 0),e([v()],C.prototype,"helpBase",void 0),e([v()],C.prototype,"helperServices",void 0),e([v()],C.prototype,"helpMap",void 0),e([v()],C.prototype,"homePageFeaturedContent",void 0),e([v()],C.prototype,"homePageFeaturedContentCount",void 0),e([v()],C.prototype,"httpPort",void 0),e([v()],C.prototype,"httpsPort",void 0),e([v()],C.prototype,"id",void 0),e([v()],C.prototype,"ipCntryCode",void 0),e([v({readOnly:!0})],C.prototype,"isOrganization",null),e([v()],C.prototype,"isPortal",void 0),e([v()],C.prototype,"isReadOnly",void 0),e([v()],C.prototype,"layerTemplatesGroupQuery",void 0),e([v()],C.prototype,"maxTokenExpirationMinutes",void 0),e([v({type:Date})],C.prototype,"modified",void 0),e([v()],C.prototype,"name",void 0),e([v()],C.prototype,"portalHostname",void 0),e([v()],C.prototype,"portalMode",void 0),e([v()],C.prototype,"portalProperties",void 0),e([v()],C.prototype,"region",void 0),e([v({readOnly:!0})],C.prototype,"restUrl",null),e([v()],C.prototype,"rotatorPanels",void 0),e([v()],C.prototype,"showHomePageDescription",void 0),e([v()],C.prototype,"sourceJSON",void 0),e([v()],C.prototype,"staticImagesUrl",void 0),e([v({readOnly:!0,json:{read:!1}})],C.prototype,"stylesGroupQuery",null),e([v({json:{name:"2DStylesGroupQuery"}})],C.prototype,"stylesGroupQuery2d",void 0),e([v({json:{name:"stylesGroupQuery"}})],C.prototype,"stylesGroupQuery3d",void 0),e([v()],C.prototype,"supportsHostedServices",void 0),e([v()],C.prototype,"symbolSetsGroupQuery",void 0),e([v()],C.prototype,"templatesGroupQuery",void 0),e([v()],C.prototype,"thumbnail",void 0),e([v({readOnly:!0})],C.prototype,"thumbnailUrl",null),e([v()],C.prototype,"units",void 0),e([v()],C.prototype,"url",void 0),e([v()],C.prototype,"urlKey",void 0),e([g("urlKey")],C.prototype,"readUrlKey",null),e([v()],C.prototype,"user",void 0),e([g("user")],C.prototype,"readUser",null),e([v()],C.prototype,"useStandardizedQuery",void 0),e([v()],C.prototype,"useVectorBasemaps",void 0),e([v()],C.prototype,"vectorBasemapGalleryGroupQuery",void 0),C=O=e([S("esri.portal.Portal")],C);const k=C;export{k as default};
