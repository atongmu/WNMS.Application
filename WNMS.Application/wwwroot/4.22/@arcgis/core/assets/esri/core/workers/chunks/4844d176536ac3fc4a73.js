"use strict";(self.webpackChunkRemoteClient=self.webpackChunkRemoteClient||[]).push([[9704,7258],{87239:(e,t,r)=>{r.d(t,{f:()=>h,n:()=>y,r:()=>p});var o=r(60991),s=r(50406),n=r(3482);const i=/^([a-z]{2})(?:[-_]([A-Za-z]{2}))?$/,a={ar:!0,bg:!0,bs:!0,ca:!0,cs:!0,da:!0,de:!0,el:!0,en:!0,es:!0,et:!0,fi:!0,fr:!0,he:!0,hr:!0,hu:!0,id:!0,it:!0,ja:!0,ko:!0,lt:!0,lv:!0,nb:!0,nl:!0,pl:!0,"pt-BR":!0,"pt-PT":!0,ro:!0,ru:!0,sk:!0,sl:!0,sr:!0,sv:!0,th:!0,tr:!0,uk:!0,vi:!0,"zh-CN":!0,"zh-HK":!0,"zh-TW":!0};function l(e){var t;return null!=(t=a[e])&&t}const d=[],u=new Map;function c(e){for(const t of u.keys())f(e.pattern,t)&&u.delete(t)}function p(e){return d.includes(e)||(c(e),d.unshift(e)),{remove(){const t=d.indexOf(e);t>-1&&(d.splice(t,1),c(e))}}}async function h(e){const t=(0,n.g)();u.has(e)||u.set(e,async function(e,t){const r=[];for(const o of d)if(f(o.pattern,e))try{return await o.fetchMessageBundle(e,t)}catch(e){r.push(e)}if(r.length)throw new o.Z("intl:message-bundle-error",`Errors occurred while loading "${e}"`,{errors:r});throw new o.Z("intl:no-message-bundle-loader",`No loader found for message bundle "${e}"`)}(e,t));const r=u.get(e);return await m.add(r),r}function y(e){if(!i.test(e))return null;const[,t,r]=i.exec(e),o=t+(r?"-"+r.toUpperCase():"");return l(o)?o:l(t)?t:null}function f(e,t){return"string"==typeof e?t.startsWith(e):e.test(t)}(0,n.b)((()=>{u.clear()}));const m=new class{constructor(){this._numLoading=0}async waitForAll(){this._dfd&&await this._dfd.promise}add(e){return this._increase(),e.then((()=>this._decrease()),(()=>this._decrease())),this.waitForAll()}_increase(){this._numLoading++,this._dfd||(this._dfd=(0,s.dD)())}_decrease(){this._numLoading=Math.max(this._numLoading-1,0),this._dfd&&0===this._numLoading&&(this._dfd.resolve(),this._dfd=null)}}},65514:(e,t,r)=>{r.d(t,{bA:()=>B});var o=r(60991),s=r(76506),n=r(50406),i=r(46987),a=r(37453),l=r(88762),d=r(92143),u=r(73173),c=r(31450),p=(r(66284),r(32101)),h=r(30773),y=r(3482);r(71552),r(40642),r(22723),r(82058),r(33921),r(57251),r(87239);let f=null;f=(0,p.hF)((0,u.g)("esri/core/workers/init.js"));const m={};m.baseUrl=(0,p.hF)((0,u.g)("dojo/")),m.packages=[{name:"esri",location:"../esri"}];class g{constructor(){const e=document.createDocumentFragment();["addEventListener","dispatchEvent","removeEventListener"].forEach((t=>{this[t]=(...r)=>e[t](...r)}))}}class w{constructor(){this._dispatcher=new g,this._workerPostMessage({type:a.M.HANDSHAKE})}terminate(){}get onmessage(){return this._onmessageHandler}set onmessage(e){this._onmessageHandler&&this.removeEventListener("message",this._onmessageHandler),this._onmessageHandler=e,e&&this.addEventListener("message",e)}get onmessageerror(){return this._onmessageerrorHandler}set onmessageerror(e){this._onmessageerrorHandler&&this.removeEventListener("messageerror",this._onmessageerrorHandler),this._onmessageerrorHandler=e,e&&this.addEventListener("messageerror",e)}get onerror(){return this._onerrorHandler}set onerror(e){this._onerrorHandler&&this.removeEventListener("error",this._onerrorHandler),this._onerrorHandler=e,e&&this.addEventListener("error",e)}postMessage(e){(0,h.n)((()=>{this._workerMessageHandler(new MessageEvent("message",{data:e}))}))}dispatchEvent(e){return this._dispatcher.dispatchEvent(e)}addEventListener(e,t,r){this._dispatcher.addEventListener(e,t,r)}removeEventListener(e,t,r){this._dispatcher.removeEventListener(e,t,r)}_workerPostMessage(e){(0,h.n)((()=>{this.dispatchEvent(new MessageEvent("message",{data:e}))}))}async _workerMessageHandler(e){const t=(0,a.r)(e);if(t&&t.type===a.M.OPEN){const{modulePath:e,jobId:r}=t;let o=await a.default.loadWorker(e);o||(o=await import(e));const s=a.default.connect(o);this._workerPostMessage({type:a.M.OPENED,jobId:r,data:s})}}}const b=d.L.getLogger("esri.core.workers"),{HANDSHAKE:v}=a.M;let _,k;const j="Failed to create Worker. Fallback to execute module in main thread";async function C(e){return new Promise((t=>{function r(s){const n=(0,a.r)(s);n&&n.type===v&&(e.removeEventListener("message",r),e.removeEventListener("error",o),t(e))}function o(t){t.preventDefault(),e.removeEventListener("message",r),e.removeEventListener("error",o),b.warn("Failed to create Worker. Fallback to execute module in main thread",t),(e=new w).addEventListener("message",r),e.addEventListener("error",o)}e.addEventListener("message",r),e.addEventListener("error",o)}))}let S=0;const R=d.L.getLogger("esri.core.workers"),{ABORT:E,INVOKE:M,OPEN:N,OPENED:P,RESPONSE:O}=a.M;class x{constructor(e,t){this._outJobs=new Map,this._inJobs=new Map,this.worker=e,this.id=t,e.addEventListener("message",this._onMessage.bind(this)),e.addEventListener("error",(e=>{e.preventDefault(),R.error(e)}))}static async create(e){const t=await async function(){if(!(0,s.h)("esri-workers"))return C(new w);if(!_&&!k)try{const e='let globalId=0;const outgoing=new Map,configuration=JSON.parse("{CONFIGURATION}");self.esriConfig=configuration.esriConfig;const workerPath=self.esriConfig.workers.workerPath,HANDSHAKE=0,OPEN=1,OPENED=2,RESPONSE=3,INVOKE=4,ABORT=5;function createAbortError(){const e=new Error("Aborted");return e.name="AbortError",e}function receiveMessage(e){return e&&e.data?"string"==typeof e.data?JSON.parse(e.data):e.data:null}function invokeStaticMessage(e,o,r){const t=r&&r.signal,n=globalId++;return new Promise(((r,i)=>{if(t){if(t.aborted)return i(createAbortError());t.addEventListener("abort",(()=>{outgoing.get(n)&&(outgoing.delete(n),self.postMessage({type:5,jobId:n}),i(createAbortError()))}))}outgoing.set(n,{resolve:r,reject:i}),self.postMessage({type:4,jobId:n,methodName:e,abortable:null!=t,data:o})}))}let workerRevisionChecked=!1;function checkWorkerRevision(e){if(!workerRevisionChecked&&e.kernelInfo){workerRevisionChecked=!0;const{revision:o,buildDate:r,version:t}=configuration.kernelInfo,{revision:n,buildDate:i,version:s}=e.kernelInfo;o!==n&&console.warn(`[esri.core.workers] Version mismatch detected between ArcGIS API for JavaScript and assets:\nAPI version: ${t} [Date: ${r}, Revision: ${o.slice(0,8)}]\nAssets version: ${s} [Date: ${i}, Revision: ${n.slice(0,8)}]`)}}function messageHandler(e){const o=receiveMessage(e);if(!o)return;const r=o.jobId;switch(o.type){case 1:let e;function t(o){const t=e.connect(o);self.postMessage({type:2,jobId:r,data:t},[t])}"function"==typeof define&&define.amd?require([workerPath],(r=>{e=r.default||r,checkWorkerRevision(e),e.loadWorker(o.modulePath).then((e=>e||new Promise((e=>{require([o.modulePath],e)})))).then(t)})):"System"in self&&"function"==typeof System.import?System.import(workerPath).then((r=>(e=r.default,checkWorkerRevision(e),e.loadWorker(o.modulePath)))).then((e=>e||System.import(o.modulePath))).then(t):(self.RemoteClient||importScripts(workerPath),e=self.RemoteClient.default||self.RemoteClient,checkWorkerRevision(e),e.loadWorker(o.modulePath).then(t));break;case 3:if(outgoing.has(r)){const e=outgoing.get(r);outgoing.delete(r),o.error?e.reject(JSON.parse(o.error)):e.resolve(o.data)}}}self.dojoConfig=configuration.loaderConfig,esriConfig.workers.loaderUrl&&(self.importScripts(esriConfig.workers.loaderUrl),"function"==typeof require&&"function"==typeof require.config&&require.config(configuration.loaderConfig)),self.addEventListener("message",messageHandler),self.postMessage({type:0});'.replace('"{CONFIGURATION}"',`'${function(){let e;if(null!=c.Z.default){const t={...c.Z};delete t.default,e=JSON.parse(JSON.stringify(t))}else e=JSON.parse(JSON.stringify(c.Z));e.assetsPath=(0,p.hF)(e.assetsPath),e.request.interceptors=[],e.log.interceptors=[],e.locale=(0,y.g)(),e.has={"esri-csp-restrictions":(0,s.h)("esri-csp-restrictions"),"esri-2d-debug":!1,"esri-2d-update-debug":(0,s.h)("esri-2d-update-debug"),"esri-2d-query-centroid-enabled":(0,s.h)("esri-2d-query-centroid-enabled"),"featurelayer-pbf":(0,s.h)("featurelayer-pbf"),"featurelayer-simplify-thresholds":(0,s.h)("featurelayer-simplify-thresholds"),"featurelayer-simplify-payload-size-factors":(0,s.h)("featurelayer-simplify-payload-size-factors"),"featurelayer-simplify-mobile-factor":(0,s.h)("featurelayer-simplify-mobile-factor"),"esri-atomics":(0,s.h)("esri-atomics"),"esri-shared-array-buffer":(0,s.h)("esri-shared-array-buffer"),"esri-tiles-debug":(0,s.h)("esri-tiles-debug"),"esri-workers-arraybuffer-transfer":(0,s.h)("esri-workers-arraybuffer-transfer"),"host-webworker":1},e.workers.loaderUrl?e.workers.loaderUrl=(0,p.hF)(e.workers.loaderUrl):f&&(e.workers.loaderUrl=f),e.workers.workerPath?e.workers.workerPath=(0,p.hF)(e.workers.workerPath):e.workers.workerPath="esri/core/workers/RemoteClient";const t=c.Z.workers.loaderConfig,r=function(e){var t;const r={async:e.async,isDebug:e.isDebug,locale:e.locale,baseUrl:e.baseUrl,has:{...e.has},map:{...e.map},packages:e.packages&&e.packages.concat()||[],paths:{...e.paths}};return e.hasOwnProperty("async")||(r.async=!0),e.hasOwnProperty("isDebug")||(r.isDebug=!1),e.baseUrl||(r.baseUrl=m.baseUrl),null==(t=m.packages)||t.forEach((e=>{!function(e,t){for(const r of e)if(r.name===t.name)return;e.push(t)}(r.packages,e)})),r}({baseUrl:null==t?void 0:t.baseUrl,locale:(0,y.g)(),has:{"csp-restrictions":1,"dojo-test-sniff":0,"host-webworker":1,...null==t?void 0:t.has},map:{...null==t?void 0:t.map},paths:{...null==t?void 0:t.paths},packages:(null==t?void 0:t.packages)||[]}),o={version:l.i8,buildDate:l.rh,revision:l.LB};return JSON.stringify({esriConfig:e,loaderConfig:r,kernelInfo:o})}()}'`);_=URL.createObjectURL(new Blob([e],{type:"text/javascript"}))}catch(e){k=e||{}}let e;if(_)try{e=new Worker(_,{name:"esri-worker-"+S++})}catch(t){b.warn(j,k),e=new w}else b.warn(j,k),e=new w;return C(e)}();return new x(t,e)}terminate(){this.worker.terminate()}async open(e,t={}){const{signal:r}=t,o=(0,a.n)();return new Promise(((t,s)=>{const i={resolve:t,reject:s,abortHandle:(0,n.$F)(r,(()=>{this._outJobs.delete(o),this._post({type:E,jobId:o})}))};this._outJobs.set(o,i),this._post({type:N,jobId:o,modulePath:e})}))}_onMessage(e){const t=(0,a.r)(e);if(t)switch(t.type){case P:this._onOpenedMessage(t);break;case O:this._onResponseMessage(t);break;case E:this._onAbortMessage(t);break;case M:this._onInvokeMessage(t)}}_onAbortMessage(e){const t=this._inJobs,r=e.jobId,o=t.get(r);o&&(o.controller&&o.controller.abort(),t.delete(r))}_onInvokeMessage(e){const{methodName:t,jobId:r,data:o,abortable:s}=e,i=s?new AbortController:null,d=this._inJobs,u=l.Nv[t];let c;try{if("function"!=typeof u)throw new TypeError(`${t} is not a function`);c=u.call(null,o,{signal:i?i.signal:null})}catch(e){return void this._post({type:O,jobId:r,error:(0,a.t)(e)})}(0,n.y8)(c)?(d.set(r,{controller:i,promise:c}),c.then((e=>{d.has(r)&&(d.delete(r),this._post({type:O,jobId:r},e))}),(e=>{d.has(r)&&(d.delete(r),e||(e={message:"Error encountered at method"+t}),(0,n.D_)(e)||this._post({type:O,jobId:r,error:(0,a.t)(e||{message:`Error encountered at method ${t}`})}))}))):this._post({type:O,jobId:r},c)}_onOpenedMessage(e){var t;const{jobId:r,data:o}=e,s=this._outJobs.get(r);s&&(this._outJobs.delete(r),null==(t=s.abortHandle)||t.remove(),s.resolve(o))}_onResponseMessage(e){var t;const{jobId:r,error:s,data:n}=e,i=this._outJobs.get(r);i&&(this._outJobs.delete(r),null==(t=i.abortHandle)||t.remove(),s?i.reject(o.Z.fromJSON(JSON.parse(s))):i.resolve(n))}_post(e,t,r){return(0,a.p)(this.worker,e,t,r)}}let I=(0,s.h)("esri-workers-debug")?1:(0,s.h)("host-browser")?navigator.hardwareConcurrency-1:0;I||(I=(0,s.h)("safari")&&(0,s.h)("mac")||(0,s.h)("trident")?7:2);let F=0;const L=[];async function A(e,t){const r=new i.Z;return await r.open(e,t),r}async function B(e,t={}){if("string"!=typeof e)throw new o.Z("workers:undefined-module","modulePath is missing");let r=t.strategy||"distributed";if((0,s.h)("host-webworker")&&!(0,s.h)("esri-workers")&&(r="local"),"local"===r){let r=await a.default.loadWorker(e);r||(r=await import(e)),(0,n.k_)(t.signal);const o=t.client||r;return A([a.default.connect(r)],{...t,client:o})}if(await async function(){if(U)return U;Z=new AbortController;const e=[];for(let t=0;t<I;t++){const r=x.create(t).then((e=>(L[t]=e,e)));e.push(r)}return U=Promise.all(e),U}(),(0,n.k_)(t.signal),"dedicated"===r){const r=F++%I;return A([await L[r].open(e,t)],t)}if(t.maxNumWorkers&&t.maxNumWorkers>0){const r=Math.min(t.maxNumWorkers,I);if(r<I){const o=new Array(r);for(let s=0;s<r;++s){const r=F++%I;o[s]=L[r].open(e,t)}return A(o,t)}}return A(L.map((r=>r.open(e,t))),t)}let Z,U=null},66284:(e,t,r)=>{r.d(t,{ng:()=>h});var o=r(33921),s=r(92143),n=r(71552),i=r(40642),a=(r(3482),r(87239)),l=r(82058),d=r(60991),u=r(76506),c=r(73173);r(57251),r(31450),r(50406),r(88762),r(32101);const p=s.L.getLogger("esri.intl");function h(e,t,r={}){const{format:s={}}=r;return(0,i.r)(e,(e=>function(e,t,r){let s,i;const a=e.indexOf(":");if(-1===a?s=e.trim():(s=e.slice(0,a).trim(),i=e.slice(a+1).trim()),!s)return"";const l=(0,n.g)(s,t);if(null==l)return"";const d=r[i]||r[s];return d?function(e,t){switch(t.type){case"date":return(0,o.a)(e,t.intlOptions);case"number":return(0,o.f)(e,t.intlOptions);default:return p.warn("missing format descriptor for key {key}"),y(e)}}(l,d):i?function(e,t){switch(t.toLowerCase()){case"dateformat":return(0,o.a)(e);case"numberformat":return(0,o.f)(e);default:return p.warn(`inline format is unsupported since 4.12: ${t}`),/^(dateformat|datestring)/i.test(t)?(0,o.a)(e):/^numberformat/i.test(t)?(0,o.f)(e):y(e)}}(l,i):y(l)}(e,t,s)))}function y(e){switch(typeof e){case"string":return e;case"number":return(0,o.f)(e);case"boolean":return""+e;default:return e instanceof Date?(0,o.a)(e):""}}async function f(e){if((0,u.i)(m.fetchBundleAsset))return m.fetchBundleAsset(e);const t=await(0,l.default)(e,{responseType:"text"});return JSON.parse(t.data)}const m={};var g;(0,a.r)((g={pattern:"esri/",location:c.g},new class{constructor({base:e="",pattern:t,location:r=new URL(window.location.href)}){let o;o="string"==typeof r?e=>new URL(e,new URL(r,window.location.href)).href:r instanceof URL?e=>new URL(e,r).href:r,this.pattern="string"==typeof t?new RegExp(`^${t}`):t,this.getAssetUrl=o,e=e?e.endsWith("/")?e:e+"/":"",this.matcher=new RegExp(`^${e}(?:(.*)/)?(.*)$`)}fetchMessageBundle(e,t){return async function(e,t,r,o){const s=t.exec(r);if(!s)throw new d.Z("esri-intl:invalid-bundle",`Bundle id "${r}" is not compatible with the pattern "${t}"`);const n=s[1]?`${s[1]}/`:"",i=s[2],l=(0,a.n)(o),u=`${n}${i}.json`,c=l?`${n}${i}_${l}.json`:u;let p;try{p=await f(e(c))}catch(t){if(c===u)throw new d.Z("intl:unknown-bundle",`Bundle "${r}" cannot be loaded`,{error:t});try{p=await f(e(u))}catch(e){throw new d.Z("intl:unknown-bundle",`Bundle "${r}" cannot be loaded`,{error:e})}}return p}(this.getAssetUrl,this.matcher,e,t)}}(g)))},34446:(e,t,r)=>{r.d(t,{Z:()=>p});var o,s=r(29768),n=r(74673),i=r(76506),a=r(34250),l=r(91306),d=r(17533),u=r(2906);r(21972),r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991);let c=o=class extends n.a{constructor(e){super(e),this.attachmentTypes=null,this.attachmentsWhere=null,this.keywords=null,this.globalIds=null,this.name=null,this.num=null,this.objectIds=null,this.returnMetadata=!1,this.size=null,this.start=null,this.where=null}writeStart(e,t){t.resultOffset=this.start,t.resultRecordCount=this.num||10}clone(){return new o((0,i.d9)({attachmentTypes:this.attachmentTypes,attachmentsWhere:this.attachmentsWhere,keywords:this.keywords,where:this.where,globalIds:this.globalIds,name:this.name,num:this.num,objectIds:this.objectIds,returnMetadata:this.returnMetadata,size:this.size,start:this.start}))}};(0,s._)([(0,a.Cb)({type:[String],json:{write:!0}})],c.prototype,"attachmentTypes",void 0),(0,s._)([(0,a.Cb)({type:String,json:{read:{source:"attachmentsDefinitionExpression"},write:{target:"attachmentsDefinitionExpression"}}})],c.prototype,"attachmentsWhere",void 0),(0,s._)([(0,a.Cb)({type:[String],json:{write:!0}})],c.prototype,"keywords",void 0),(0,s._)([(0,a.Cb)({type:[Number],json:{write:!0}})],c.prototype,"globalIds",void 0),(0,s._)([(0,a.Cb)({json:{write:!0}})],c.prototype,"name",void 0),(0,s._)([(0,a.Cb)({type:Number,json:{read:{source:"resultRecordCount"}}})],c.prototype,"num",void 0),(0,s._)([(0,a.Cb)({type:[Number],json:{write:!0}})],c.prototype,"objectIds",void 0),(0,s._)([(0,a.Cb)({type:Boolean,json:{default:!1,write:!0}})],c.prototype,"returnMetadata",void 0),(0,s._)([(0,a.Cb)({type:[Number],json:{write:!0}})],c.prototype,"size",void 0),(0,s._)([(0,a.Cb)({type:Number,json:{read:{source:"resultOffset"}}})],c.prototype,"start",void 0),(0,s._)([(0,u.w)("start"),(0,u.w)("num")],c.prototype,"writeStart",null),(0,s._)([(0,a.Cb)({type:String,json:{read:{source:"definitionExpression"},write:{target:"definitionExpression"}}})],c.prototype,"where",void 0),c=o=(0,s._)([(0,d.j)("esri.rest.support.AttachmentQuery")],c),c.from=(0,l.e)(c);const p=c},87258:(e,t,r)=>{r.r(t),r.d(t,{default:()=>w});var o=r(29768),s=r(74569),n=r(23761),i=r(57251),a=r(74673),l=r(76506),d=r(34250),u=(r(91306),r(97714)),c=r(17533),p=r(2906),h=r(60947),y=r(32422),f=r(97546);r(21801),r(40642),r(71552),r(73796),r(21972),r(23639),r(92143),r(31450),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991),r(91597),r(86787),r(35132),r(89623),r(84069),r(44567),r(98380),r(92896),r(22781),r(86748),r(15324),r(76996),r(14249),r(60217),r(29107),r(30574),r(2157),r(25977),r(58076),r(98242),r(7471),r(54414),r(59465),r(1648),r(8925),r(33921),r(3482),r(45154),r(16769),r(55531),r(30582),r(593),r(85699),r(96055),r(47776),r(18033),r(6331),r(62048),r(4292),r(75626),r(72652),r(29641),r(30493),r(70821),r(82673),r(34229),r(37029),r(96467),r(63571),r(30776),r(48027),r(54174),r(82426),r(29794),r(63130),r(25696),r(66396),r(42775),r(95834),r(34394),r(57150),r(76726),r(20444),r(76393),r(78548),r(2497),r(49906),r(46527),r(11799),r(48649),r(98402),r(9960),r(30823),r(53326),r(92482),r(5853),r(39141),r(32101),r(38742),r(48243),r(34635),r(10401),r(49900),r(88762),r(82058),r(22739),r(20543),r(67477),r(78533),r(74653),r(91091),r(58943),r(70737),r(8487),r(17817),r(90814),r(15459),r(61847),r(16796),r(16955),r(22401),r(77894),r(55187),r(8586),r(44509),r(69814),r(11305),r(62259),r(44790),r(5909),r(60669),r(48208),r(51589),r(9801),r(53523),r(42911),r(46826),r(45433),r(54732);const m=new i.J({esriGeometryPoint:"point",esriGeometryMultipoint:"multipoint",esriGeometryPolyline:"polyline",esriGeometryPolygon:"polygon",esriGeometryEnvelope:"extent",mesh:"mesh","":null});let g=class extends a.a{constructor(e){super(e),this.displayFieldName=null,this.exceededTransferLimit=!1,this.features=[],this.fields=null,this.geometryType=null,this.hasM=!1,this.hasZ=!1,this.queryGeometry=null,this.spatialReference=null}readFeatures(e,t){const r=h.Z.fromJSON(t.spatialReference),o=[];for(let t=0;t<e.length;t++){const s=e[t],i=n.Z.fromJSON(s),a=s.geometry&&s.geometry.spatialReference;(0,l.i)(i.geometry)&&!a&&(i.geometry.spatialReference=r),o.push(i)}return o}writeGeometryType(e,t,r,o){if(e)return void m.write(e,t,r,o);const{features:s}=this;if(s)for(const e of s)if(e&&(0,l.i)(e.geometry))return void m.write(e.geometry.type,t,r,o)}readQueryGeometry(e,t){if(!e)return null;const r=!!e.spatialReference,o=(0,y.im)(e);return!r&&t.spatialReference&&(o.spatialReference=h.Z.fromJSON(t.spatialReference)),o}writeSpatialReference(e,t){if(e)return void(t.spatialReference=e.toJSON());const{features:r}=this;if(r)for(const e of r)if(e&&(0,l.i)(e.geometry)&&e.geometry.spatialReference)return void(t.spatialReference=e.geometry.spatialReference.toJSON())}toJSON(e){const t=this.write();if(t.features&&Array.isArray(e)&&e.length>0)for(let r=0;r<t.features.length;r++){const o=t.features[r];if(o.geometry){const t=e&&e[r];o.geometry=t&&t.toJSON()||o.geometry}}return t}quantize(e){const{scale:[t,r],translate:[o,s]}=e,n=this.features,i=this._getQuantizationFunction(this.geometryType,(e=>Math.round((e-o)/t)),(e=>Math.round((s-e)/r)));for(let e=0,t=n.length;e<t;e++)i((0,l.u)(n[e].geometry))||(n.splice(e,1),e--,t--);return this.transform=e,this}unquantize(){const{geometryType:e,features:t,transform:r}=this;if(!r)return this;const{translate:[o,s],scale:[n,i]}=r,a=this._getHydrationFunction(e,(e=>e*n+o),(e=>s-e*i));for(const{geometry:e}of t)(0,l.i)(e)&&a(e);return this.transform=null,this}_quantizePoints(e,t,r){let o,s;const n=[];for(let i=0,a=e.length;i<a;i++){const a=e[i];if(i>0){const e=t(a[0]),i=r(a[1]);e===o&&i===s||(n.push([e-o,i-s]),o=e,s=i)}else o=t(a[0]),s=r(a[1]),n.push([o,s])}return n.length>0?n:null}_getQuantizationFunction(e,t,r){return"point"===e?e=>(e.x=t(e.x),e.y=r(e.y),e):"polyline"===e||"polygon"===e?e=>{const o=(0,y.oU)(e)?e.rings:e.paths,s=[];for(let e=0,n=o.length;e<n;e++){const n=o[e],i=this._quantizePoints(n,t,r);i&&s.push(i)}return s.length>0?((0,y.oU)(e)?e.rings=s:e.paths=s,e):null}:"multipoint"===e?e=>{const o=this._quantizePoints(e.points,t,r);return o.length>0?(e.points=o,e):null}:"extent"===e?e=>e:null}_getHydrationFunction(e,t,r){return"point"===e?e=>{e.x=t(e.x),e.y=r(e.y)}:"polyline"===e||"polygon"===e?e=>{const o=(0,y.oU)(e)?e.rings:e.paths;let s,n;for(let e=0,i=o.length;e<i;e++){const i=o[e];for(let e=0,o=i.length;e<o;e++){const o=i[e];e>0?(s+=o[0],n+=o[1]):(s=o[0],n=o[1]),o[0]=t(s),o[1]=r(n)}}}:"extent"===e?e=>{e.xmin=t(e.xmin),e.ymin=r(e.ymin),e.xmax=t(e.xmax),e.ymax=r(e.ymax)}:"multipoint"===e?e=>{const o=e.points;let s,n;for(let e=0,i=o.length;e<i;e++){const i=o[e];e>0?(s+=i[0],n+=i[1]):(s=i[0],n=i[1]),i[0]=t(s),i[1]=r(n)}}:void 0}};(0,o._)([(0,d.Cb)({type:String,json:{write:!0}})],g.prototype,"displayFieldName",void 0),(0,o._)([(0,d.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],g.prototype,"exceededTransferLimit",void 0),(0,o._)([(0,d.Cb)({type:[n.Z],json:{write:!0}})],g.prototype,"features",void 0),(0,o._)([(0,u.r)("features")],g.prototype,"readFeatures",null),(0,o._)([(0,d.Cb)({type:[f.Z],json:{write:!0}})],g.prototype,"fields",void 0),(0,o._)([(0,d.Cb)({type:["point","multipoint","polyline","polygon","extent","mesh"],json:{read:{reader:m.read}}})],g.prototype,"geometryType",void 0),(0,o._)([(0,p.w)("geometryType")],g.prototype,"writeGeometryType",null),(0,o._)([(0,d.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],g.prototype,"hasM",void 0),(0,o._)([(0,d.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],g.prototype,"hasZ",void 0),(0,o._)([(0,d.Cb)({types:s.qM,json:{write:!0}})],g.prototype,"queryGeometry",void 0),(0,o._)([(0,u.r)("queryGeometry")],g.prototype,"readQueryGeometry",null),(0,o._)([(0,d.Cb)({type:h.Z,json:{write:!0}})],g.prototype,"spatialReference",void 0),(0,o._)([(0,p.w)("spatialReference")],g.prototype,"writeSpatialReference",null),(0,o._)([(0,d.Cb)({json:{write:!0}})],g.prototype,"transform",void 0),g=(0,o._)([(0,c.j)("esri.rest.support.FeatureSet")],g),g.prototype.toJSON.isDefaultToJSON=!0;const w=g},46646:(e,t,r)=>{r.d(t,{Z:()=>y});var o,s=r(29768),n=(r(74569),r(74673)),i=r(76506),a=r(34250),l=r(91306),d=r(17533),u=r(2906),c=r(69218),p=r(60947);r(21801),r(40642),r(71552),r(73796),r(97714),r(21972),r(23639),r(92143),r(31450),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991),r(91597),r(86787),r(35132),r(89623),r(84069),r(44567),r(98380),r(92896),r(22781),r(57251),r(32422),r(59465),r(97546),r(9801),r(53523),r(42911),r(46826),r(45433),r(54732);let h=o=class extends n.a{constructor(e){super(e),this.dynamicDataSource=void 0,this.gdbVersion=null,this.geometryPrecision=void 0,this.historicMoment=null,this.maxAllowableOffset=void 0,this.objectIds=null,this.orderByFields=null,this.outFields=null,this.outSpatialReference=null,this.relationshipId=void 0,this.start=void 0,this.num=void 0,this.returnGeometry=!1,this.returnM=void 0,this.returnZ=void 0,this.where=null}_writeHistoricMoment(e,t){t.historicMoment=e&&e.getTime()}writeStart(e,t){t.resultOffset=this.start,t.resultRecordCount=this.num||10,this.start>0&&null==this.where&&(t.definitionExpression="1=1")}clone(){return new o((0,i.d9)({dynamicDataSource:this.dynamicDataSource,gdbVersion:this.gdbVersion,geometryPrecision:this.geometryPrecision,historicMoment:this.historicMoment&&new Date(this.historicMoment.getTime()),maxAllowableOffset:this.maxAllowableOffset,objectIds:this.objectIds,orderByFields:this.orderByFields,outFields:this.outFields,outSpatialReference:this.outSpatialReference,relationshipId:this.relationshipId,start:this.start,num:this.num,returnGeometry:this.returnGeometry,where:this.where,returnZ:this.returnZ,returnM:this.returnM}))}};(0,s._)([(0,a.Cb)({type:c.D,json:{write:!0}})],h.prototype,"dynamicDataSource",void 0),(0,s._)([(0,a.Cb)({type:String,json:{write:!0}})],h.prototype,"gdbVersion",void 0),(0,s._)([(0,a.Cb)({type:Number,json:{write:!0}})],h.prototype,"geometryPrecision",void 0),(0,s._)([(0,a.Cb)({type:Date})],h.prototype,"historicMoment",void 0),(0,s._)([(0,u.w)("historicMoment")],h.prototype,"_writeHistoricMoment",null),(0,s._)([(0,a.Cb)({type:Number,json:{write:!0}})],h.prototype,"maxAllowableOffset",void 0),(0,s._)([(0,a.Cb)({type:[Number],json:{write:!0}})],h.prototype,"objectIds",void 0),(0,s._)([(0,a.Cb)({type:[String],json:{write:!0}})],h.prototype,"orderByFields",void 0),(0,s._)([(0,a.Cb)({type:[String],json:{write:!0}})],h.prototype,"outFields",void 0),(0,s._)([(0,a.Cb)({type:p.Z,json:{read:{source:"outSR"},write:{target:"outSR"}}})],h.prototype,"outSpatialReference",void 0),(0,s._)([(0,a.Cb)({json:{write:!0}})],h.prototype,"relationshipId",void 0),(0,s._)([(0,a.Cb)({type:Number,json:{read:{source:"resultOffset"}}})],h.prototype,"start",void 0),(0,s._)([(0,u.w)("start"),(0,u.w)("num")],h.prototype,"writeStart",null),(0,s._)([(0,a.Cb)({type:Number,json:{read:{source:"resultRecordCount"}}})],h.prototype,"num",void 0),(0,s._)([(0,a.Cb)({json:{write:!0}})],h.prototype,"returnGeometry",void 0),(0,s._)([(0,a.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],h.prototype,"returnM",void 0),(0,s._)([(0,a.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],h.prototype,"returnZ",void 0),(0,s._)([(0,a.Cb)({type:String,json:{read:{source:"definitionExpression"},write:{target:"definitionExpression"}}})],h.prototype,"where",void 0),h=o=(0,s._)([(0,d.j)("esri.rest.support.RelationshipQuery")],h),h.from=(0,l.e)(h);const y=h},39210:(e,t,r)=>{r.d(t,{Z:()=>v});var o,s=r(29768),n=r(74569),i=r(93314),a=r(57251),l=r(74673),d=r(76506),u=r(34250),c=r(91306),p=r(17533),h=r(2906),y=r(32422),f=r(90549),m=r(60947);r(21801),r(40642),r(71552),r(73796),r(97714),r(21972),r(23639),r(92143),r(31450),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991),r(91597),r(86787),r(35132),r(89623),r(84069),r(44567),r(98380),r(92896),r(22781),r(35197);const g=new a.J({esriSpatialRelIntersects:"intersects",esriSpatialRelContains:"contains",esriSpatialRelCrosses:"crosses",esriSpatialRelDisjoint:"disjoint",esriSpatialRelEnvelopeIntersects:"envelope-intersects",esriSpatialRelIndexIntersects:"index-intersects",esriSpatialRelOverlaps:"overlaps",esriSpatialRelTouches:"touches",esriSpatialRelWithin:"within",esriSpatialRelRelation:"relation"}),w=new a.J({esriSRUnit_Meter:"meters",esriSRUnit_Kilometer:"kilometers",esriSRUnit_Foot:"feet",esriSRUnit_StatuteMile:"miles",esriSRUnit_NauticalMile:"nautical-miles",esriSRUnit_USNauticalMile:"us-nautical-miles"});let b=o=class extends l.a{constructor(e){super(e),this.cacheHint=void 0,this.distance=void 0,this.geometry=null,this.geometryPrecision=void 0,this.maxAllowableOffset=void 0,this.num=void 0,this.objectIds=null,this.orderByFields=null,this.outFields=null,this.outSpatialReference=null,this.resultType=null,this.returnGeometry=!1,this.returnM=void 0,this.returnZ=void 0,this.start=void 0,this.spatialRelationship="intersects",this.timeExtent=null,this.topFilter=void 0,this.units=null,this.where="1=1"}writeStart(e,t){t.resultOffset=this.start,t.resultRecordCount=this.num||10}clone(){return new o((0,d.d9)({cacheHint:this.cacheHint,distance:this.distance,geometry:this.geometry,geometryPrecision:this.geometryPrecision,maxAllowableOffset:this.maxAllowableOffset,num:this.num,objectIds:this.objectIds,orderByFields:this.orderByFields,outFields:this.outFields,outSpatialReference:this.outSpatialReference,resultType:this.resultType,returnGeometry:this.returnGeometry,returnZ:this.returnZ,returnM:this.returnM,start:this.start,spatialRelationship:this.spatialRelationship,timeExtent:this.timeExtent,topFilter:this.topFilter,units:this.units,where:this.where}))}};(0,s._)([(0,u.Cb)({type:Boolean,json:{write:!0}})],b.prototype,"cacheHint",void 0),(0,s._)([(0,u.Cb)({type:Number,json:{write:{overridePolicy:e=>({enabled:e>0})}}})],b.prototype,"distance",void 0),(0,s._)([(0,u.Cb)({types:n.qM,json:{read:y.im,write:!0}})],b.prototype,"geometry",void 0),(0,s._)([(0,u.Cb)({type:Number,json:{write:!0}})],b.prototype,"geometryPrecision",void 0),(0,s._)([(0,u.Cb)({type:Number,json:{write:!0}})],b.prototype,"maxAllowableOffset",void 0),(0,s._)([(0,u.Cb)({type:Number,json:{read:{source:"resultRecordCount"}}})],b.prototype,"num",void 0),(0,s._)([(0,u.Cb)({json:{write:!0}})],b.prototype,"objectIds",void 0),(0,s._)([(0,u.Cb)({type:[String],json:{write:!0}})],b.prototype,"orderByFields",void 0),(0,s._)([(0,u.Cb)({type:[String],json:{write:!0}})],b.prototype,"outFields",void 0),(0,s._)([(0,u.Cb)({type:m.Z,json:{read:{source:"outSR"},write:{target:"outSR"}}})],b.prototype,"outSpatialReference",void 0),(0,s._)([(0,u.Cb)({type:String,json:{write:!0}})],b.prototype,"resultType",void 0),(0,s._)([(0,u.Cb)({json:{write:!0}})],b.prototype,"returnGeometry",void 0),(0,s._)([(0,u.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],b.prototype,"returnM",void 0),(0,s._)([(0,u.Cb)({type:Boolean,json:{write:{overridePolicy:e=>({enabled:e})}}})],b.prototype,"returnZ",void 0),(0,s._)([(0,u.Cb)({type:Number,json:{read:{source:"resultOffset"}}})],b.prototype,"start",void 0),(0,s._)([(0,h.w)("start"),(0,h.w)("num")],b.prototype,"writeStart",null),(0,s._)([(0,u.Cb)({type:String,json:{read:{source:"spatialRel",reader:g.read},write:{target:"spatialRel",writer:g.write}}})],b.prototype,"spatialRelationship",void 0),(0,s._)([(0,u.Cb)({type:i.Z,json:{write:!0}})],b.prototype,"timeExtent",void 0),(0,s._)([(0,u.Cb)({type:f.Z,json:{write:!0}})],b.prototype,"topFilter",void 0),(0,s._)([(0,u.Cb)({type:String,json:{read:w.read,write:{writer:w.write,overridePolicy(e){return{enabled:e&&this.distance>0}}}}})],b.prototype,"units",void 0),(0,s._)([(0,u.Cb)({type:String,json:{write:!0}})],b.prototype,"where",void 0),b=o=(0,s._)([(0,p.j)("esri.rest.support.TopFeaturesQuery")],b),b.from=(0,c.e)(b);const v=b},90549:(e,t,r)=>{r.d(t,{Z:()=>d});var o,s=r(29768),n=r(74673),i=r(34250),a=(r(76506),r(91306),r(17533));r(21972),r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991);let l=o=class extends n.a{constructor(e){super(e),this.groupByFields=void 0,this.topCount=void 0,this.orderByFields=void 0}clone(){return new o({groupByFields:this.groupByFields,topCount:this.topCount,orderByFields:this.orderByFields})}};(0,s._)([(0,i.Cb)({type:[String],json:{write:!0}})],l.prototype,"groupByFields",void 0),(0,s._)([(0,i.Cb)({type:Number,json:{write:!0}})],l.prototype,"topCount",void 0),(0,s._)([(0,i.Cb)({type:[String],json:{write:!0}})],l.prototype,"orderByFields",void 0),l=o=(0,s._)([(0,a.j)("esri.rest.support.TopFilter")],l);const d=l}}]);