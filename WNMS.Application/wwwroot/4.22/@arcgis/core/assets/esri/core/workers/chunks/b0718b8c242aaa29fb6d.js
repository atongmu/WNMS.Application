"use strict";(self.webpackChunkRemoteClient=self.webpackChunkRemoteClient||[]).push([[2883],{59765:(e,t,r)=>{r.d(t,{Z:()=>p,t:()=>u});var i=r(29768),s=r(74673),n=r(35197),a=r(34250),l=(r(76506),r(91306),r(17533)),o=r(57251);r(21972),r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991);const u=(0,o.s)()({esriTimeUnitsMilliseconds:"milliseconds",esriTimeUnitsSeconds:"seconds",esriTimeUnitsMinutes:"minutes",esriTimeUnitsHours:"hours",esriTimeUnitsDays:"days",esriTimeUnitsWeeks:"weeks",esriTimeUnitsMonths:"months",esriTimeUnitsYears:"years",esriTimeUnitsDecades:"decades",esriTimeUnitsCenturies:"centuries",esriTimeUnitsUnknown:void 0});var d;let c=d=class extends s.a{constructor(e){super(e),this.value=0,this.unit="milliseconds"}toMilliseconds(){return(0,n.c)(this.value,this.unit,"milliseconds")}clone(){return new d({value:this.value,unit:this.unit})}};(0,i._)([(0,a.Cb)({type:Number,json:{write:!0},nonNullable:!0})],c.prototype,"value",void 0),(0,i._)([(0,a.Cb)({type:u.apiValues,json:{type:u.jsonValues,read:u.read,write:u.write},nonNullable:!0})],c.prototype,"unit",void 0),c=d=(0,i._)([(0,l.j)("esri.TimeInterval")],c);const p=c},62162:(e,t,r)=>{r.d(t,{P:()=>_});var i=r(29768),s=r(88762),n=r(82058),a=r(41864),l=r(60991),o=r(92143),u=r(76506),d=r(50406),c=r(32101),p=r(34250),h=(r(91306),r(97714)),y=r(17533),m=r(2906),v=r(49900),f=r(56420),g=r(74653);const b=o.L.getLogger("esri.layers.mixins.PortalLayer"),_=e=>{let t=class extends e{constructor(){super(...arguments),this.resourceReferences={portalItem:null,paths:[]},this.userHasEditingPrivileges=!0}destroy(){var e;null==(e=this.portalItem)||e.destroy(),this.portalItem=null}set portalItem(e){e!==this._get("portalItem")&&(this.removeOrigin("portal-item"),this._set("portalItem",e))}readPortalItem(e,t,r){if(t.itemId)return new f.default({id:t.itemId,portal:r&&r.portal})}writePortalItem(e,t){e&&e.id&&(t.itemId=e.id)}async loadFromPortal(e,t){if(this.portalItem&&this.portalItem.id)try{const i=await r.e(8642).then(r.bind(r,8642)).then((e=>e.l));return(0,d.k_)(t),await i.load({instance:this,supportedTypes:e.supportedTypes,validateItem:e.validateItem,supportsData:e.supportsData},t)}catch(e){throw(0,d.D_)(e)||b.warn(`Failed to load layer (${this.title}, ${this.id}) portal item (${this.portalItem.id})\n  ${e}`),e}}async finishLoadEditablePortalLayer(e){this._set("userHasEditingPrivileges",await this.fetchUserHasEditingPrivileges(e).catch((e=>((0,d.r9)(e),!0))))}async fetchUserHasEditingPrivileges(e){const t=this.url?null==s.id?void 0:s.id.findCredential(this.url):null;if(!t)return!0;const r=w.credential===t?w.user:await this.fetchEditingUser(e);return w.credential=t,w.user=r,(0,u.b)(r)||null==r.privileges||r.privileges.includes("features:user:edit")}async fetchEditingUser(e){var t,r;const i=null==(t=this.portalItem)||null==(r=t.portal)?void 0:r.user;if(i)return i;const l=s.id.findServerInfo(this.url);if(null==l||!l.owningSystemUrl)return null;const o=`${l.owningSystemUrl}/sharing/rest`,d=v.Z.getDefault();if(d&&d.loaded&&(0,c.Fv)(d.restUrl)===(0,c.Fv)(o))return d.user;const p=`${o}/community/self`,h=(0,u.i)(e)?e.signal:null,y=await(0,a.r)((0,n.default)(p,{authMode:"no-prompt",query:{f:"json"},signal:h}));return y.ok?g.default.fromJSON(y.value.data):null}read(e,t){t&&(t.layer=this),super.read(e,t)}write(e,t){const r=t&&t.portal,i=this.portalItem&&this.portalItem.id&&(this.portalItem.portal||v.Z.getDefault());return r&&i&&!(0,c.tm)(i.restUrl,r.restUrl)?(t.messages&&t.messages.push(new l.Z("layer:cross-portal",`The layer '${this.title} (${this.id})' cannot be persisted because it refers to an item on a different portal than the one being saved to. To save the scene, set the layer.portalItem to null or save the scene to the same portal as the item associated with the layer`,{layer:this})),null):super.write(e,{...t,layer:this})}};return(0,i._)([(0,p.Cb)({type:f.default})],t.prototype,"portalItem",null),(0,i._)([(0,h.r)("web-document","portalItem",["itemId"])],t.prototype,"readPortalItem",null),(0,i._)([(0,m.w)("web-document","portalItem",{itemId:{type:String}})],t.prototype,"writePortalItem",null),(0,i._)([(0,p.Cb)()],t.prototype,"resourceReferences",void 0),(0,i._)([(0,p.Cb)({readOnly:!0})],t.prototype,"userHasEditingPrivileges",void 0),t=(0,i._)([(0,y.j)("esri.layers.mixins.PortalLayer")],t),t},w={credential:null,user:null}},19902:(e,t,r)=>{r.d(t,{T:()=>c});var i=r(29768),s=r(93314),n=r(59765),a=r(34250),l=(r(76506),r(91306),r(97714)),o=r(17533),u=r(77361),d=r(14249);const c=e=>{let t=class extends e{constructor(){super(...arguments),this.timeExtent=null,this.timeOffset=null,this.useViewTime=!0}readOffset(e,t){const r=t.timeInfo.exportOptions;if(!r)return null;const i=r.timeOffset,s=n.t.fromJSON(r.timeOffsetUnits);return i&&s?new n.Z({value:i,unit:s}):null}set timeInfo(e){(0,d.UF)(e,this.fieldsIndex),this._set("timeInfo",e)}};return(0,i._)([(0,a.Cb)({type:s.Z,json:{write:!1}})],t.prototype,"timeExtent",void 0),(0,i._)([(0,a.Cb)({type:n.Z})],t.prototype,"timeOffset",void 0),(0,i._)([(0,l.r)("service","timeOffset",["timeInfo.exportOptions"])],t.prototype,"readOffset",null),(0,i._)([(0,a.Cb)({value:null,type:u.Z,json:{write:!0,origins:{"web-document":{read:!1,write:!1}}}})],t.prototype,"timeInfo",null),(0,i._)([(0,a.Cb)({type:Boolean,json:{read:{source:"timeAnimation"},write:{target:"timeAnimation"},origins:{"web-scene":{read:!1,write:!1}}}})],t.prototype,"useViewTime",void 0),t=(0,i._)([(0,o.j)("esri.layers.mixins.TemporalLayer")],t),t}},21132:(e,t,r)=>{r.d(t,{a:()=>g,b:()=>c,c:()=>y,d:()=>_,f:()=>h,i:()=>v,p:()=>p,s:()=>f,t:()=>m,w:()=>b});var i=r(76506),s=r(32101),n=r(38742);const a={mapserver:"MapServer",imageserver:"ImageServer",featureserver:"FeatureServer",sceneserver:"SceneServer",streamserver:"StreamServer",vectortileserver:"VectorTileServer"},l=Object.values(a),o=new RegExp(`^((?:https?:)?\\/\\/\\S+?\\/rest\\/services\\/(.+?)\\/(${l.join("|")}))(?:\\/(?:layers\\/)?(\\d+))?`,"i"),u=new RegExp(`^((?:https?:)?\\/\\/\\S+?\\/([^\\/\\n]+)\\/(${l.join("|")}))(?:\\/(?:layers\\/)?(\\d+))?`,"i"),d=/(.*?)\/(?:layers\/)?(\d+)\/?$/i;function c(e){return!!o.test(e)}function p(e){const t=(0,s.mN)(e),r=t.path.match(o)||t.path.match(u);if(!r)return null;const[,i,n,l,d]=r,c=n.indexOf("/");return{title:y(-1!==c?n.slice(c+1):n),serverType:a[l.toLowerCase()],sublayer:null!=d&&""!==d?parseInt(d,10):null,url:{path:i}}}function h(e){const t=(0,s.mN)(e).path.match(d);return t?{serviceUrl:t[1],sublayerId:Number(t[2])}:null}function y(e){return(e=e.replace(/\s*[/_]+\s*/g," "))[0].toUpperCase()+e.slice(1)}function m(e,t){const r=[];if(e){const t=p(e);(0,i.i)(t)&&t.title&&r.push(t.title)}if(t){const e=y(t);r.push(e)}if(2===r.length){if(-1!==r[0].toLowerCase().indexOf(r[1].toLowerCase()))return r[0];if(-1!==r[1].toLowerCase().indexOf(r[0].toLowerCase()))return r[1]}return r.join(" - ")}function v(e){if(!e)return!1;const t=-1!==(e=e.toLowerCase()).indexOf(".arcgis.com/"),r=-1!==e.indexOf("//services")||-1!==e.indexOf("//tiles")||-1!==e.indexOf("//features");return t&&r}function f(e,t){return e?(0,s.Qj)((0,s.Hu)(e,t)):e}function g(e){let{url:t}=e;if(!t)return{url:t};t=(0,s.Hu)(t,e.logger);const r=(0,s.mN)(t),n=p(r.path);let a;if((0,i.i)(n))null!=n.sublayer&&null==e.layer.layerId&&(a=n.sublayer),t=n.url.path;else if(e.nonStandardUrlAllowed){const e=h(r.path);(0,i.i)(e)&&(t=e.serviceUrl,a=e.sublayerId)}return{url:(0,s.Qj)(t),layerId:a}}function b(e,t,r,i,a){(0,n.w)(t,i,"url",a),i.url&&null!=e.layerId&&(i.url=(0,s.v_)(i.url,r,e.layerId.toString()))}function _(e){if(!e)return!1;const t=e.toLowerCase(),r=-1!==t.indexOf("/services/"),i=-1!==t.indexOf("/mapserver/wmsserver"),s=-1!==t.indexOf("/imageserver/wmsserver"),n=-1!==t.indexOf("/wmsserver");return r&&(i||s||n)}},41864:(e,t,r)=>{r.d(t,{a:()=>o,f:()=>n,m:()=>a,r:()=>l});var i=r(76506),s=r(50406);function n(e,t,r){return(0,s.as)(e.map(((e,i)=>t.apply(r,[e,i]))))}function a(e,t,r){return(0,s.as)(e.map(((e,i)=>t.apply(r,[e,i])))).then((e=>e.map((e=>e.value))))}function l(e){return(0,i.b)(e)?(0,s.DB)():e.then((e=>({ok:!0,value:e}))).catch((e=>({ok:!1,error:e})))}function o(e){return e.then((e=>({ok:!0,value:e}))).catch((e=>((0,s.r9)(e),{ok:!1,error:e})))}},91162:(e,t,r)=>{r.d(t,{a:()=>o,l:()=>l});var i=r(41864),s=r(15324),n=r(22739),a=r(76506);async function l(e,t){return await e.load(),o(e,t)}async function o(e,t){const r=[],l=(...e)=>{for(const t of e)(0,a.b)(t)||(Array.isArray(t)?l(...t):s.Z.isCollection(t)?t.forEach((e=>l(e))):n.L.isLoadable(t)&&r.push(t))};t(l);let o=null;if(await(0,i.m)(r,(async e=>{var t;!1!==(await(0,i.r)((t=e,"loadAll"in t&&"function"==typeof t.loadAll?e.loadAll():e.load()))).ok||o||(o=e)})),o)throw o.loadError;return e}},39597:(e,t,r)=>{r.d(t,{a:()=>a,r:()=>n});var i=r(76506),s=r(21972);function n(e,t,r={}){return l(e,t,r,o)}function a(e,t,r={}){return l(e,t,r,u)}function l(e,t,r={},n){let a=null;const l=r.once?(e,r)=>{n(e)&&((0,i.r)(a),t(e,r))}:(e,r)=>{n(e)&&t(e,r)};if(a=(0,s.w)(e,l,r.sync,r.equals),r.initial){const t=e();l(t,t)}return a}function o(e){return!0}function u(e){return!!e}r(17533)},95873:(e,t,r)=>{r.d(t,{r:()=>m,p:()=>y,W:()=>p});var i=r(29768),s=r(21972),n=r(82933),a=r(34250),l=r(17533),o=r(76506),u=r(39597),d=r(6906),c=r(617);r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(91306),r(60991),r(50406),r(15324),r(76996);let p=class extends s.Z{constructor(){super(...arguments),this.updating=!1,this.handleId=0,this.handles=new n.Z,this.scheduleHandleId=0,this.pendingPromises=new Set}destroy(){this.removeAll(),this.handles.destroy()}add(e,t,r,i=0){const s=0!=(1&i),n=++this.handleId;s||this.installSyncUpdatingWatch(e,t,n);const a=0!=(2&i)?(0,c.S1)(e,t,r,s):e.watch(t,r,s);return this.handles.add(a,n),{remove:()=>this.handles.remove(n)}}addOnCollectionPropertyChange(e,t,r,i=0){const s=0!=(2&i),n=++this.handleId;return this.handles.add([(0,c.on)(e,t,"after-changes",this.createSyncUpdatingCallback()),(0,c.on)(e,t,"change",r,s?e=>{r({added:e.items,removed:[],moved:[],target:e})}:void 0)],n),{remove:()=>{this.handles.remove(n)}}}addOnCollectionChange(e,t,r=0){const i=0!=(2&r),s=++this.handleId;return this.handles.add([e.on("after-changes",this.createSyncUpdatingCallback()),e.on("change",t)],s),i&&t({added:e.items,removed:[],moved:[],target:e}),{remove:()=>{this.handles.remove(s)}}}addPromise(e){if((0,o.b)(e))return e;const t=++this.handleId;this.handles.add({remove:()=>{this.pendingPromises.delete(e)&&(0!==this.pendingPromises.size||this.handles.has(h)||this._set("updating",!1))}},t),this.pendingPromises.add(e),this._set("updating",!0);const r=()=>this.handles.remove(t);return e.then(r,r),e}removeAll(){this.pendingPromises.clear(),this.handles.removeAll(),this._set("updating",!1)}installSyncUpdatingWatch(e,t,r){const i=this.createSyncUpdatingCallback(),s=(0,u.r)((()=>(0,a.v)(e,t)),i,{sync:!0,equals:()=>!1});return this.handles.add(s,r),s}createSyncUpdatingCallback(){return()=>{this.handles.remove(h),++this.scheduleHandleId;const e=this.scheduleHandleId;this._get("updating")||this._set("updating",!0),this.handles.add((0,d.Os)((()=>{e===this.scheduleHandleId&&(this._set("updating",this.pendingPromises.size>0),this.handles.remove(h))})),h)}}};(0,i._)([(0,a.Cb)({readOnly:!0})],p.prototype,"updating",void 0),p=(0,i._)([(0,l.j)("esri.views.support.WatchUpdatingTracking")],p);const h=-42,y=e=>{let t=class extends e{destroy(){var e,t;this.destroyed||(null==(e=this._get("handles"))||e.destroy(),null==(t=this._get("updatingHandles"))||t.destroy())}get handles(){return this._get("handles")||new n.Z}get updatingHandles(){return this._get("updatingHandles")||new p}};return(0,i._)([(0,a.Cb)({readOnly:!0})],t.prototype,"handles",null),(0,i._)([(0,a.Cb)({readOnly:!0})],t.prototype,"updatingHandles",null),t=(0,i._)([(0,l.j)("esri.core.HandleOwner")],t),t};let m=class extends(y(s.Z)){};m=(0,i._)([(0,l.j)("esri.core.HandleOwner")],m)},82933:(e,t,r)=>{r.d(t,{Z:()=>d});var i=r(29768),s=r(21972),n=r(15324),a=r(76506),l=r(34250),o=r(17533);r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991),r(91306),r(76996);let u=class extends s.Z{constructor(e){super(e),this._groups=new Map}destroy(){this.removeAll()}get size(){let e=0;return this._groups.forEach((t=>{e+=t.length})),e}add(e,t){if(!this._isHandle(e)&&!Array.isArray(e)&&!n.Z.isCollection(e))return this;const r=this._getOrCreateGroup(t);return Array.isArray(e)||n.Z.isCollection(e)?e.forEach((e=>this._isHandle(e)&&r.push(e))):r.push(e),this.notifyChange("size"),this}forEach(e,t){if("function"==typeof e)this._groups.forEach((t=>t.forEach(e)));else{const r=this._getGroup(e);r&&t&&r.forEach(t)}}has(e){return this._groups.has(this._ensureGroupKey(e))}remove(e){if(Array.isArray(e)||n.Z.isCollection(e))return e.forEach(this.remove,this),this;if(!this.has(e))return this;const t=this._getGroup(e);for(let e=0;e<t.length;e++)t[e].remove();return this._deleteGroup(e),this.notifyChange("size"),this}removeAll(){return this._groups.forEach((e=>{for(let t=0;t<e.length;t++)e[t].remove()})),this._groups.clear(),this.notifyChange("size"),this}_isHandle(e){return e&&!!e.remove}_getOrCreateGroup(e){if(this.has(e))return this._getGroup(e);const t=[];return this._groups.set(this._ensureGroupKey(e),t),t}_getGroup(e){return(0,a.a)(this._groups.get(this._ensureGroupKey(e)))}_deleteGroup(e){return this._groups.delete(this._ensureGroupKey(e))}_ensureGroupKey(e){return e||"_default_"}};(0,i._)([(0,l.Cb)({readOnly:!0})],u.prototype,"size",null),u=(0,i._)([(0,o.j)("esri.core.Handles")],u);const d=u},617:(e,t,r)=>{r.d(t,{S1:()=>n,on:()=>d,OY:()=>u,cm:()=>l,N1:()=>a,LR:()=>o,tH:()=>c});var i=r(50406);r(76506),r(60991),r(92143),r(31450),r(71552),r(40642);const s=/\?(\.|$)/g;function n(e,t,r,i){const n=Array.isArray(t)?t:t.indexOf(",")>-1?t.split(","):[t],a=function(e,t,r,i){return e.watch(t,r,i)}(e,t,r,i);for(const t of n){const i=t.trim().replace(s,"$1"),n=e.get(i);r.call(e,n,n,i,e)}return a}function a(e,t,r,i){return c(e,t,p,r,i)}function l(e,t,r,i){return c(e,t,h,r,i)}function o(e,t,r,i){return c(e,t,y,r,i)}function u(e,t,r,i){return c(e,t,m,r,i)}function d(e,t,r,s,a,l,o){const u={};function d(t){const i=u[t];i&&(l&&l(i.target,t,e,r),i.handle.remove(),delete u[t])}const c=n(e,t,((t,n,l)=>{d(l),(0,i.i)(t)&&(u[l]={handle:(0,i.o)(t,r,s),target:t},a&&a(t,l,e,r))}),o);return{remove(){c.remove();for(const e in u)d(e)}}}function c(e,t,r,s,n){const a="function"==typeof s?s:null,l="object"==typeof s?s:null;"boolean"==typeof s&&(n=s);let o,u=!1;function d(){o&&(o.remove(),o=null)}const c=(0,i.dD)();(0,i.fu)(l,(()=>{d(),c.reject((0,i.zE)())}));const p={then:c.promise.then.bind(c.promise),catch:c.promise.catch.bind(c.promise),remove:d};return Object.freeze(p),o=function(e,t,r,i,s){const n=e.watch(t,((t,s,n,a)=>{r&&!r(t)||null==i||i.call(e,t,s,n,a)}),s);if(Array.isArray(t))for(const s of t){const n=e.get(s);r&&r(n)&&(null==i||i.call(e,n,n,t,e))}else{const s=e.get(t);r&&r(s)&&(null==i||i.call(e,s,s,t,e))}return n}(e,t,r,((t,r,i,s)=>{u=!0,d(),a&&a.call(e,t,r,i,s),c.resolve({value:t,oldValue:r,propertyName:i,target:s})}),n),u&&d(),p}function p(e){return!!e}function h(e){return!e}function y(e){return!0===e}function m(e){return!1===e}},62883:(e,t,r)=>{r.r(t),r.d(t,{default:()=>k});var i=r(29768),s=r(82058),n=r(93314),a=r(60991),l=r(95873),o=r(91162),u=r(76506),d=r(54179),c=r(50406),p=r(34250),h=r(91306),y=r(97714),m=r(17533),v=r(2906),f=r(21972),g=r(21801),b=r(69997),_=r(41617),w=r(21781),S=r(81655),I=r(75025),x=r(11118),C=r(8547),O=r(89440),T=r(62162),E=r(16647),U=r(58912),j=r(19902),D=r(67541),L=r(20820),P=r(58142);const F={visible:"visibleSublayers",definitionExpression:"layerDefs",labelingInfo:"hasDynamicLayers",labelsVisible:"hasDynamicLayers",opacity:"hasDynamicLayers",minScale:"visibleSublayers",maxScale:"visibleSublayers",renderer:"hasDynamicLayers",source:"hasDynamicLayers"};let N=class extends((0,l.p)(f.Z)){constructor(e){super(e),this.floors=null,this.scale=0}destroy(){this.layer=null}get dynamicLayers(){if(!this.hasDynamicLayers)return null;const e=this.visibleSublayers.map((e=>{const t=(0,P.g)(this.floors,e);return e.toExportImageJSON(t)}));return e.length?JSON.stringify(e):null}get hasDynamicLayers(){return this.layer&&(0,L.a)(this.visibleSublayers,this.layer.serviceSublayers,this.layer)}set layer(e){this._get("layer")!==e&&(this._set("layer",e),this.handles.remove("layer"),e&&this.handles.add([e.allSublayers.on("change",(()=>this.notifyChange("visibleSublayers"))),e.on("sublayer-update",(e=>this.notifyChange(F[e.propertyName])))],"layer"))}get layers(){const e=this.visibleSublayers;return e?e.length?"show:"+e.map((e=>e.id)).join(","):"show:-1":null}get layerDefs(){var e;const t=!(null==(e=this.floors)||!e.length),r=this.visibleSublayers.filter((e=>null!=e.definitionExpression||t&&null!=e.floorInfo));return r.length?JSON.stringify(r.reduce(((e,t)=>{const r=(0,P.g)(this.floors,t),i=(0,u.i)(r)?(0,P.c)(r,t):t.definitionExpression;return e[t.id]=i,e}),{})):null}get version(){this.commitProperty("layers"),this.commitProperty("layerDefs"),this.commitProperty("dynamicLayers"),this.commitProperty("timeExtent");const e=this.layer;return e&&(e.commitProperty("dpi"),e.commitProperty("imageFormat"),e.commitProperty("imageTransparency"),e.commitProperty("gdbVersion")),(this._get("version")||0)+1}get visibleSublayers(){const e=[];if(!this.layer)return e;const t=this.layer.sublayers,r=t=>{const i=this.scale,s=0===i,n=0===t.minScale||i<=t.minScale,a=0===t.maxScale||i>=t.maxScale;t.visible&&(s||n&&a)&&(t.sublayers?t.sublayers.forEach(r):e.unshift(t))};t&&t.forEach(r);const i=this._get("visibleSublayers");return!i||i.length!==e.length||i.some(((t,r)=>e[r]!==t))?e:i}toJSON(){const e=this.layer;let t={dpi:e.dpi,format:e.imageFormat,transparent:e.imageTransparency,gdbVersion:e.gdbVersion||null};return this.hasDynamicLayers&&this.dynamicLayers?t.dynamicLayers=this.dynamicLayers:t={...t,layers:this.layers,layerDefs:this.layerDefs},t}};(0,i._)([(0,p.Cb)({readOnly:!0})],N.prototype,"dynamicLayers",null),(0,i._)([(0,p.Cb)()],N.prototype,"floors",void 0),(0,i._)([(0,p.Cb)({readOnly:!0})],N.prototype,"hasDynamicLayers",null),(0,i._)([(0,p.Cb)()],N.prototype,"layer",null),(0,i._)([(0,p.Cb)({readOnly:!0})],N.prototype,"layers",null),(0,i._)([(0,p.Cb)({readOnly:!0})],N.prototype,"layerDefs",null),(0,i._)([(0,p.Cb)({type:Number})],N.prototype,"scale",void 0),(0,i._)([(0,p.Cb)(D.f)],N.prototype,"timeExtent",void 0),(0,i._)([(0,p.Cb)({readOnly:!0})],N.prototype,"version",null),(0,i._)([(0,p.Cb)({readOnly:!0})],N.prototype,"visibleSublayers",null),N=(0,i._)([(0,m.j)("esri.layers.mixins.ExportImageParameters")],N);var A=r(9530);r(31450),r(71552),r(88762),r(32101),r(92143),r(40642),r(74673),r(86656),r(22723),r(35197),r(23639),r(91055),r(6906),r(82933),r(15324),r(76996),r(39597),r(617),r(41864),r(22739),r(20543),r(73796),r(60947),r(91597),r(86787),r(35132),r(89623),r(53785),r(57251),r(95587),r(74569),r(84069),r(44567),r(98380),r(92896),r(22781),r(32422),r(82673),r(10738),r(22203),r(21132),r(38742),r(92200),r(65949),r(54174),r(65775),r(29794),r(5777),r(49900),r(3482),r(67477),r(78533),r(74653),r(91091),r(58943),r(56420),r(73173),r(74742),r(28239),r(59765),r(77361),r(14249),r(60217),r(1557),r(47842),r(17298),r(85557),r(34394),r(86748),r(29107),r(30574),r(2157),r(25977),r(58076),r(98242),r(7471),r(54414),r(59465),r(1648),r(8925),r(33921),r(45154),r(16769),r(55531),r(30582),r(593),r(85699),r(96055),r(47776),r(18033),r(6331),r(62048),r(4292),r(75626),r(72652),r(29641),r(30493),r(70821),r(34229),r(37029),r(55306),r(96467),r(63571),r(30776),r(48027),r(82426),r(63130),r(25696),r(66396),r(42775),r(95834),r(57150),r(76726),r(20444),r(76393),r(78548),r(2497),r(49906),r(46527),r(11799),r(48649),r(98402),r(9960),r(30823),r(53326),r(92482),r(5853),r(39141),r(48243),r(34635),r(10401),r(70737),r(8487),r(17817),r(90814),r(15459),r(61847),r(16796),r(16955),r(22401),r(77894),r(55187),r(8586),r(44509),r(69814),r(11305),r(62259),r(44790),r(5909),r(60669),r(48208),r(51589),r(65684),r(12158),r(74864),r(63683),r(94479),r(45702),r(51127),r(74071),r(51723),r(23243),r(51669),r(6090),r(3977),r(36741),r(11253),r(90319),r(38822),r(74057),r(23761),r(48190),r(94070),r(43022),r(16218),r(9075),r(71206),r(89241),r(91700),r(51979),r(63136),r(32037),r(60698),r(90811),r(86758),r(95310),r(93939),r(238),r(71831),r(20208),r(78303),r(9801),r(53523),r(42911),r(46826),r(45433),r(46495),r(97546),r(54732),r(1709),r(77807),r(50203),r(6941),r(2180),r(69218),r(31292),r(27207),r(78893);let Z=class extends((0,x.B)((0,j.T)((0,U.S)((0,S.S)((0,S.A)((0,I.A)((0,O.O)((0,T.P)((0,d.M)((0,E.R)((0,w.A)((0,C.C)((0,l.p)(_.Z)))))))))))))){constructor(...e){super(...e),this.datesInUnknownTimezone=!1,this.dpi=96,this.gdbVersion=null,this.imageFormat="png24",this.imageMaxHeight=2048,this.imageMaxWidth=2048,this.imageTransparency=!0,this.isReference=null,this.labelsVisible=!1,this.operationalLayerType="ArcGISMapServiceLayer",this.sourceJSON=null,this.sublayers=null,this.type="map-image",this.url=null}normalizeCtorArgs(e,t){return"string"==typeof e?{url:e,...t}:e}load(e){const t=(0,u.i)(e)?e.signal:null;return this.addResolvingPromise(this.loadFromPortal({supportedTypes:["Map Service"]},e).catch(c.r9).then((()=>this._fetchService(t)))),Promise.resolve(this)}readImageFormat(e,t){const r=t.supportedImageFormatTypes;return r&&r.indexOf("PNG32")>-1?"png32":"png24"}writeSublayers(e,t,r,i){if(!this.loaded||!e)return;const s=e.slice().reverse().flatten((({sublayers:e})=>e&&e.toArray().reverse())).toArray();let n=!1;if(this.capabilities&&this.capabilities.operations.supportsExportMap&&this.capabilities.exportMap.supportsDynamicLayers){const e=(0,f.n)(i.origin);if(3===e){const e=this.createSublayersForOrigin("service").sublayers;n=(0,L.s)(s,e,2)}else if(e>3){const e=this.createSublayersForOrigin("portal-item");n=(0,L.s)(s,e.sublayers,(0,f.n)(e.origin))}}const a=[],l={writeSublayerStructure:n,...i};let o=n;s.forEach((e=>{const t=e.write({},l);a.push(t),o=o||"user"===e.originOf("visible")})),a.some((e=>Object.keys(e).length>1))&&(t.layers=a),o&&(t.visibleLayers=s.filter((e=>e.visible)).map((e=>e.id)))}createExportImageParameters(e,t,r,i){const s=i&&i.pixelRatio||1;e&&this.version>=10&&(e=e.clone().shiftCentralMeridian());const n=new N({layer:this,floors:null==i?void 0:i.floors,scale:(0,b.g)({extent:e,width:t})*s}),a=n.toJSON();n.destroy();const l=!i||!i.rotation||this.version<10.3?{}:{rotation:-i.rotation},o=e&&e.spatialReference,u=o.wkid||JSON.stringify(o.toJSON());a.dpi*=s;const d={};if(null!=i&&i.timeExtent){const{start:e,end:t}=i.timeExtent.toJSON();d.time=e&&t&&e===t?""+e:`${null==e?"null":e},${null==t?"null":t}`}else this.timeInfo&&!this.timeInfo.hasLiveData&&(d.time="null,null");return{bbox:e&&e.xmin+","+e.ymin+","+e.xmax+","+e.ymax,bboxSR:u,imageSR:u,size:t+","+r,...a,...l,...d}}async fetchImage(e,t,r,i){var n;const l={responseType:"image",signal:null!=(n=null==i?void 0:i.signal)?n:null,query:{...this.parsedUrl.query,...this.createExportImageParameters(e,t,r,i),f:"image",...this.refreshParameters,...this.customParameters,token:this.apiKey}},o=this.parsedUrl.path+"/export";return null==l.query.dynamicLayers||this.capabilities.exportMap.supportsDynamicLayers?(0,s.default)(o,l).then((e=>e.data)).catch((e=>{if((0,c.D_)(e))throw e;throw new a.Z("mapimagelayer:image-fetch-error",`Unable to load image: ${o}`,{error:e})})):Promise.reject(new a.Z("mapimagelayer:dynamiclayer-not-supported",`service ${this.url} doesn't support dynamic layers, which is required to be able to change the sublayer's order, rendering, labeling or source.`,{query:l.query}))}async fetchRecomputedExtents(e={}){const t={...e,query:{returnUpdates:!0,f:"json",...this.customParameters,token:this.apiKey}},{data:r}=await(0,s.default)(this.url,t),{extent:i,fullExtent:a,timeExtent:l}=r,o=i||a;return{fullExtent:o&&g.Z.fromJSON(o),timeExtent:l&&n.Z.fromJSON({start:l[0],end:l[1]})}}loadAll(){return(0,o.l)(this,(e=>{e(this.allSublayers)}))}async _fetchService(e){if(this.sourceJSON)return void this.read(this.sourceJSON,{origin:"service",url:this.parsedUrl});const{data:t,ssl:r}=await(0,s.default)(this.parsedUrl.path,{query:{f:"json",...this.parsedUrl.query,...this.customParameters,token:this.apiKey},signal:e});r&&(this.url=this.url.replace(/^http:/i,"https:")),this.sourceJSON=t,this.read(t,{origin:"service",url:this.parsedUrl})}};(0,i._)([(0,p.Cb)({type:Boolean})],Z.prototype,"datesInUnknownTimezone",void 0),(0,i._)([(0,p.Cb)()],Z.prototype,"dpi",void 0),(0,i._)([(0,p.Cb)()],Z.prototype,"gdbVersion",void 0),(0,i._)([(0,p.Cb)()],Z.prototype,"imageFormat",void 0),(0,i._)([(0,y.r)("imageFormat",["supportedImageFormatTypes"])],Z.prototype,"readImageFormat",null),(0,i._)([(0,p.Cb)({json:{origins:{service:{read:{source:"maxImageHeight"}}}}})],Z.prototype,"imageMaxHeight",void 0),(0,i._)([(0,p.Cb)({json:{origins:{service:{read:{source:"maxImageWidth"}}}}})],Z.prototype,"imageMaxWidth",void 0),(0,i._)([(0,p.Cb)()],Z.prototype,"imageTransparency",void 0),(0,i._)([(0,p.Cb)({type:Boolean,json:{read:!1,write:{enabled:!0,overridePolicy:()=>({enabled:!1})}}})],Z.prototype,"isReference",void 0),(0,i._)([(0,p.Cb)({json:{read:!1,write:!1}})],Z.prototype,"labelsVisible",void 0),(0,i._)([(0,p.Cb)({type:["ArcGISMapServiceLayer"]})],Z.prototype,"operationalLayerType",void 0),(0,i._)([(0,p.Cb)({json:{read:!1,write:!1}})],Z.prototype,"popupEnabled",void 0),(0,i._)([(0,p.Cb)()],Z.prototype,"sourceJSON",void 0),(0,i._)([(0,p.Cb)({json:{write:{ignoreOrigin:!0}}})],Z.prototype,"sublayers",void 0),(0,i._)([(0,v.w)("sublayers",{layers:{type:[A.Z]},visibleLayers:{type:[h.I]}})],Z.prototype,"writeSublayers",null),(0,i._)([(0,p.Cb)({type:["show","hide","hide-children"]})],Z.prototype,"listMode",void 0),(0,i._)([(0,p.Cb)({json:{read:!1},readOnly:!0,value:"map-image"})],Z.prototype,"type",void 0),(0,i._)([(0,p.Cb)(D.u)],Z.prototype,"url",void 0),Z=(0,i._)([(0,m.j)("esri.layers.MapImageLayer")],Z);const k=Z},77361:(e,t,r)=>{r.d(t,{Z:()=>f});var i,s=r(29768),n=r(93314),a=r(59765),l=r(74673),o=r(76506),u=r(34250),d=(r(91306),r(97714)),c=r(17533),p=r(2906);r(35197),r(21972),r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991),r(57251);let h=i=class extends l.a{constructor(e){super(e),this.respectsDaylightSaving=!1,this.timezone=null}readRespectsDaylightSaving(e,t){return void 0!==t.respectsDaylightSaving?t.respectsDaylightSaving:void 0!==t.respectDaylightSaving&&t.respectDaylightSaving}clone(){const{respectsDaylightSaving:e,timezone:t}=this;return new i({respectsDaylightSaving:e,timezone:t})}};(0,s._)([(0,u.Cb)({type:Boolean,json:{write:!0}})],h.prototype,"respectsDaylightSaving",void 0),(0,s._)([(0,d.r)("respectsDaylightSaving",["respectsDaylightSaving","respectDaylightSaving"])],h.prototype,"readRespectsDaylightSaving",null),(0,s._)([(0,u.Cb)({type:String,json:{read:{source:"timeZone"},write:{target:"timeZone"}}})],h.prototype,"timezone",void 0),h=i=(0,s._)([(0,c.j)("esri.layers.support.TimeReference")],h);const y=h;var m;let v=m=class extends l.a{constructor(e){super(e),this.cumulative=!1,this.endField=null,this.fullTimeExtent=null,this.hasLiveData=!1,this.interval=null,this.startField=null,this.timeReference=null,this.trackIdField=null,this.useTime=!0}readFullTimeExtent(e,t){if(!t.timeExtent||!Array.isArray(t.timeExtent)||2!==t.timeExtent.length)return null;const r=new Date(t.timeExtent[0]),i=new Date(t.timeExtent[1]);return new n.Z({start:r,end:i})}writeFullTimeExtent(e,t){e&&(0,o.i)(e.start)&&(0,o.i)(e.end)?t.timeExtent=[e.start.getTime(),e.end.getTime()]:t.timeExtent=null}readInterval(e,t){return t.timeInterval&&t.timeIntervalUnits?new a.Z({value:t.timeInterval,unit:a.t.fromJSON(t.timeIntervalUnits)}):t.defaultTimeInterval&&t.defaultTimeIntervalUnits?new a.Z({value:t.defaultTimeInterval,unit:a.t.fromJSON(t.defaultTimeIntervalUnits)}):null}writeInterval(e,t){if(e){const r=e.toJSON();t.timeInterval=r.value,t.timeIntervalUnits=r.unit}else t.timeInterval=null,t.timeIntervalUnits=null}clone(){const{cumulative:e,endField:t,hasLiveData:r,interval:i,startField:s,timeReference:n,fullTimeExtent:a,trackIdField:l,useTime:u}=this;return new m({cumulative:e,endField:t,hasLiveData:r,interval:i,startField:s,timeReference:(0,o.d9)(n),fullTimeExtent:(0,o.d9)(a),trackIdField:l,useTime:u})}};(0,s._)([(0,u.Cb)({type:Boolean,json:{read:{source:"exportOptions.timeDataCumulative"},write:{target:"exportOptions.timeDataCumulative"}}})],v.prototype,"cumulative",void 0),(0,s._)([(0,u.Cb)({type:String,json:{read:{source:"endTimeField"},write:{target:"endTimeField",allowNull:!0}}})],v.prototype,"endField",void 0),(0,s._)([(0,u.Cb)({type:n.Z,json:{write:{enabled:!0,allowNull:!0}}})],v.prototype,"fullTimeExtent",void 0),(0,s._)([(0,d.r)("fullTimeExtent",["timeExtent"])],v.prototype,"readFullTimeExtent",null),(0,s._)([(0,p.w)("fullTimeExtent")],v.prototype,"writeFullTimeExtent",null),(0,s._)([(0,u.Cb)({type:Boolean,json:{write:!0}})],v.prototype,"hasLiveData",void 0),(0,s._)([(0,u.Cb)({type:a.Z,json:{write:{enabled:!0,allowNull:!0}}})],v.prototype,"interval",void 0),(0,s._)([(0,d.r)("interval",["timeInterval","timeIntervalUnits","defaultTimeInterval","defaultTimeIntervalUnits"])],v.prototype,"readInterval",null),(0,s._)([(0,p.w)("interval")],v.prototype,"writeInterval",null),(0,s._)([(0,u.Cb)({type:String,json:{read:{source:"startTimeField"},write:{target:"startTimeField",allowNull:!0}}})],v.prototype,"startField",void 0),(0,s._)([(0,u.Cb)({type:y,json:{write:{enabled:!0,allowNull:!0}}})],v.prototype,"timeReference",void 0),(0,s._)([(0,u.Cb)({type:String,json:{write:{enabled:!0,allowNull:!0}}})],v.prototype,"trackIdField",void 0),(0,s._)([(0,u.Cb)({type:Boolean,json:{read:{source:"exportOptions.useTime"},write:{target:"exportOptions.useTime"}}})],v.prototype,"useTime",void 0),v=m=(0,s._)([(0,c.j)("esri.layers.support.TimeInfo")],v);const f=v}}]);