"use strict";(self.webpackChunkRemoteClient=self.webpackChunkRemoteClient||[]).push([[5661],{58142:(e,t,r)=>{function i(e){var t;const r=e.layer;return"floorInfo"in r&&null!=(t=r.floorInfo)&&t.floorField&&"floors"in e.view?o(e.view.floors,r.floorInfo.floorField):null}function a(e,t){var r;return"floorInfo"in t&&null!=(r=t.floorInfo)&&r.floorField?o(e,t.floorInfo.floorField):null}function n(e,t){const{definitionExpression:r}=t;return e?r?`(${r}) AND (${e})`:e:r}function o(e,t){if(null==e||!e.length)return null;const r=e.filter((e=>""!==e)).map((e=>`'${e}'`));return r.push("''"),`${t} IN (${r.join(",")}) OR ${t} IS NULL`}r.d(t,{a:()=>i,c:()=>n,g:()=>a})},52228:(e,t,r)=>{function i(e){return e&&"esri.renderers.visualVariables.SizeVariable"===e.declaredClass}function a(e){return null!=e&&!isNaN(e)&&isFinite(e)}function n(e){return e.valueExpression?"expression":e.field&&"string"==typeof e.field?"field":"unknown"}function o(e,t){const r=t||n(e),i=e.valueUnit||"unknown";return"unknown"===r?"constant":e.stops?"stops":null!=e.minSize&&null!=e.maxSize&&null!=e.minDataValue&&null!=e.maxDataValue?"clamped-linear":"unknown"===i?null!=e.minSize&&null!=e.minDataValue?e.minSize&&e.minDataValue?"proportional":"additive":"identity":"real-world-size"}r.d(t,{a:()=>o,b:()=>a,g:()=>n,i:()=>i})},63232:(e,t,r)=>{r.r(t),r.d(t,{default:()=>nt});var i=r(29768),a=r(57251),n=r(34250),o=r(76506),s=r(91306),l=r(17533),u=r(31450),c=r(88762),p=r(82058),y=r(60991),d=r(99403),f=r(32101),m=r(44567),b=r(14249),h=r(74057),g=r(21972),v=r(21801),S=r(60947),w=(r(23639),r(92143),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406)),x=(r(73796),r(74673));r(97714),r(2906),r(91597),r(86787),r(35132),r(89623);let P=class extends g.Z{constructor(){super(...arguments),this.outSpatialReference=null,this.processExtent=null,this.processSpatialReference=null,this.returnFeatureCollection=!1,this.returnM=!1,this.returnZ=!1}};(0,i._)([(0,n.Cb)({type:S.Z})],P.prototype,"outSpatialReference",void 0),(0,i._)([(0,n.Cb)({type:v.Z})],P.prototype,"processExtent",void 0),(0,i._)([(0,n.Cb)({type:S.Z})],P.prototype,"processSpatialReference",void 0),(0,i._)([(0,n.Cb)({nonNullable:!0})],P.prototype,"returnFeatureCollection",void 0),(0,i._)([(0,n.Cb)({nonNullable:!0})],P.prototype,"returnM",void 0),(0,i._)([(0,n.Cb)({nonNullable:!0})],P.prototype,"returnZ",void 0),P=(0,i._)([(0,l.j)("esri/rest/geoprocessor/GPOptions")],P),P.from=(0,s.e)(P);const D=P;var I=r(59877),M=r(95533),G=r(97546);let L=class extends x.a{constructor(){super(...arguments),this.extent=null,this.height=null,this.href=null,this.opacity=1,this.rotation=0,this.scale=null,this.visible=!0,this.width=null}};(0,i._)([(0,n.Cb)({type:v.Z})],L.prototype,"extent",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"height",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"href",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"opacity",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"rotation",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"scale",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"visible",void 0),(0,i._)([(0,n.Cb)()],L.prototype,"width",void 0),L=(0,i._)([(0,l.j)("esri.layer.support.MapImage")],L);const V=L;let O=class extends x.a{constructor(e){super(e),this.itemId=null,this.url=null}};(0,i._)([(0,n.Cb)({type:String,json:{read:{source:"itemID"},write:{target:"itemID"}}})],O.prototype,"itemId",void 0),(0,i._)([(0,n.Cb)({type:String,json:{write:!0}})],O.prototype,"url",void 0),O=(0,i._)([(0,l.j)("esri.rest.support.DataFile")],O);const T=O;var C=r(87258);const _=new a.J({esriMeters:"meters",esriFeet:"feet",esriKilometers:"kilometers",esriMiles:"miles",esriNauticalMiles:"nautical-miles",esriYards:"yards"},{ignoreUnknown:!1});let J=class extends x.a{constructor(e){super(e),this.distance=0,this.units=null}};(0,i._)([(0,n.Cb)({json:{write:!0}})],J.prototype,"distance",void 0),(0,i._)([(0,n.Cb)({json:{read:_.read,write:_.write}})],J.prototype,"units",void 0),J=(0,i._)([(0,l.j)("esri/rest/support/LinearUnit")],J);const j=J,F=new a.J({GPBoolean:"boolean",GPDataFile:"data-file",GPDate:"date",GPDouble:"double",GPFeatureRecordSetLayer:"feature-record-set-layer",GPField:"field",GPLinearUnit:"linear-unit",GPLong:"long",GPRasterData:"raster-data",GPRasterDataLayer:"raster-data-layer",GPRecordSet:"record-set",GPString:"string","GPMultiValue:GPBoolean":"multi-value","GPMultiValue:GPDataFile":"multi-value","GPMultiValue:GPDate":"multi-value","GPMultiValue:GPDouble":"multi-value","GPMultiValue:GPFeatureRecordSetLayer":"multi-value","GPMultiValue:GPField":"multi-value","GPMultiValue:GPLinearUnit":"multi-value","GPMultiValue:GPLong":"multi-value","GPMultiValue:GPRasterData":"multi-value","GPMultiValue:GPRasterDataLayer":"multi-value","GPMultiValue:GPRecordSet":"multi-value","GPMultiValue:GPString":"multi-value"});let N=class extends x.a{constructor(e){super(e),this.dataType=null,this.value=null}};(0,i._)([(0,n.Cb)({json:{read:F.read,write:F.write}})],N.prototype,"dataType",void 0),(0,i._)([(0,n.Cb)()],N.prototype,"value",void 0),N=(0,i._)([(0,l.j)("esri.rest.support.ParameterValue")],N);const E=N;let R=class extends x.a{constructor(e){super(e),this.format=null,this.itemId=null,this.url=null}};(0,i._)([(0,n.Cb)()],R.prototype,"format",void 0),(0,i._)([(0,n.Cb)({json:{read:{source:"itemID"},write:{target:"itemID"}}})],R.prototype,"itemId",void 0),(0,i._)([(0,n.Cb)()],R.prototype,"url",void 0),R=(0,i._)([(0,l.j)("esri/rest/support/RasterData")],R);const k=R;var A,U=r(2710);async function $(e,t,r,i,a){const n={},o={},s=[];return function(e,t,r){for(const i in e){const a=e[i];if(a&&"object"==typeof a&&a instanceof C.default){const{features:e}=a;r[i]=[t.length,t.length+e.length],e.forEach((e=>{t.push(e.geometry)}))}}}(i,s,n),(0,M.aX)(s).then((s=>{const{outSpatialReference:l,processExtent:u,processSpatialReference:c,returnFeatureCollection:y,returnM:d,returnZ:f}=r,{path:m}=(0,I.p)(e);for(const e in n){const t=n[e];o[e]=s.slice(t[0],t[1])}const b=l?l.wkid||l:null,h=c?c.wkid||c:null,g="execute"===t?{returnFeatureCollection:y||void 0,returnM:d||void 0,returnZ:f||void 0}:null,v=Z({...u?{context:{extent:u,outSR:b,processSR:h}}:{"env:outSR":b,"env:processSR":h},...i,...g,f:"json"},null,o),S={...a,query:v};return(0,p.default)(`${m}/${t}`,S)}))}function z(e){const t=e.dataType,r=E.fromJSON(e);switch(t){case"GPBoolean":case"GPDouble":case"GPLong":case"GPString":case"GPMultiValue:GPBoolean":case"GPMultiValue:GPDouble":case"GPMultiValue:GPLong":case"GPMultiValue:GPString":return r;case"GPDate":r.value=new Date(r.value);break;case"GPDataFile":r.value=T.fromJSON(r.value);break;case"GPLinearUnit":r.value=j.fromJSON(r.value);break;case"GPFeatureRecordSetLayer":case"GPRecordSet":{const t=e.value.url;r.value=t?T.fromJSON(r.value):C.default.fromJSON(r.value);break}case"GPRasterData":case"GPRasterDataLayer":{const t=e.value.mapImage;r.value=t?V.fromJSON(t):k.fromJSON(r.value);break}case"GPField":r.value=G.Z.fromJSON(r.value);break;case"GPMultiValue:GPDate":{const e=r.value;r.value=e.map((e=>new Date(e)));break}case"GPMultiValue:GPDataFile":r.value=r.value.map((e=>T.fromJSON(e)));break;case"GPMultiValue:GPLinearUnit":r.value=r.value.map((e=>j.fromJSON(e)));break;case"GPMultiValue:GPFeatureRecordSetLayer":case"GPMultiValue:GPRecordSet":r.value=r.value.map((e=>C.default.fromJSON(e)));break;case"GPMultiValue:GPRasterData":case"GPMultiValue:GPRasterDataLayer":r.value=r.value.map((e=>e?V.fromJSON(e):k.fromJSON(r.value)));break;case"GPMultiValue:GPField":r.value=r.value.map((e=>G.Z.fromJSON(e)))}return r}function Z(e,t,r){for(const t in e){const r=e[t];Array.isArray(r)?e[t]=JSON.stringify(r.map((e=>Z({item:e},!0).item))):r instanceof Date&&(e[t]=r.getTime())}return(0,I.e)(e,t,r)}r(69997),r(53785),r(95587),r(98380),r(92896),r(94751),r(32422),r(84069),r(74569),r(22781),r(59465),r(9801),r(53523),r(42911),r(46826),r(45433),r(54732),r(23761),r(86748),r(15324),r(76996),r(60217),r(29107),r(30574),r(2157),r(25977),r(58076),r(98242),r(7471),r(54414),r(1648),r(8925),r(33921),r(3482),r(45154),r(16769),r(55531),r(30582),r(593),r(85699),r(96055),r(47776),r(18033),r(6331),r(62048),r(4292),r(75626),r(72652),r(29641),r(30493),r(70821),r(82673),r(34229),r(37029),r(96467),r(63571),r(30776),r(48027),r(54174),r(82426),r(29794),r(63130),r(25696),r(66396),r(42775),r(95834),r(34394),r(57150),r(76726),r(20444),r(76393),r(78548),r(2497),r(49906),r(46527),r(11799),r(48649),r(98402),r(9960),r(30823),r(53326),r(92482),r(5853),r(39141),r(38742),r(48243),r(34635),r(10401),r(49900),r(22739),r(20543),r(67477),r(78533),r(74653),r(91091),r(58943),r(70737),r(8487),r(17817),r(90814),r(15459),r(61847),r(16796),r(16955),r(22401),r(77894),r(55187),r(8586),r(44509),r(69814),r(11305),r(62259),r(44790),r(5909),r(60669),r(48208),r(51589);const q=new a.J({esriJobCancelled:"job-cancelled",esriJobCancelling:"job-cancelling",esriJobDeleted:"job-deleted",esriJobDeleting:"job-deleting",esriJobTimedOut:"job-timed-out",esriJobExecuting:"job-executing",esriJobFailed:"job-failed",esriJobNew:"job-new",esriJobSubmitted:"job-submitted",esriJobSucceeded:"job-succeeded",esriJobWaiting:"job-waiting"});let B=A=class extends x.a{constructor(e){super(e),this.jobId=null,this.jobStatus=null,this.messages=null,this.requestOptions=null,this.sourceUrl=null,this._timer=null}cancelJob(e){const{jobId:t,sourceUrl:r}=this,{path:i}=(0,I.p)(r),a={...this.requestOptions,...e,query:{f:"json"}};return this._clearTimer(),(0,p.default)(`${i}/jobs/${t}/cancel`,a).then((e=>{const t=A.fromJSON(e.data);return this.messages=t.messages,this.jobStatus=t.jobStatus,this}))}destroy(){clearInterval(this._timer)}checkJobStatus(e){const{path:t}=(0,I.p)(this.sourceUrl),r={...this.requestOptions,...e,query:{f:"json"}},i=`${t}/jobs/${this.jobId}`;return(0,p.default)(i,r).then((({data:e})=>{const t=A.fromJSON(e);return this.messages=t.messages,this.jobStatus=t.jobStatus,this}))}fetchResultData(e,t,r){t=D.from(t||{});const{returnFeatureCollection:i,returnM:a,returnZ:n,outSpatialReference:o}=t,{path:s}=(0,I.p)(this.sourceUrl),l=Z({returnFeatureCollection:i,returnM:a,returnZ:n,outSR:o,returnType:"data",f:"json"},null),u={...this.requestOptions,...r,query:l},c=`${s}/jobs/${this.jobId}/results/${e}`;return(0,p.default)(c,u).then((e=>z(e.data)))}fetchResultImage(e,t,r){const{path:i}=(0,I.p)(this.sourceUrl),a=Z({...t.toJSON(),f:"json"}),n={...this.requestOptions,...r,query:a},o=`${i}/jobs/${this.jobId}/results/${e}`;return(0,p.default)(o,n).then((e=>z(e.data)))}async fetchResultMapImageLayer(){const{path:e}=(0,I.p)(this.sourceUrl),t=e.indexOf("/GPServer/"),i=`${e.substring(0,t)}/MapServer/jobs/${this.jobId}`;return new(0,(await Promise.all([r.e(6420),r.e(949),r.e(7368),r.e(6583),r.e(123),r.e(208),r.e(8081),r.e(7768),r.e(3049),r.e(2883)]).then(r.bind(r,62883))).default)({url:i})}async waitForJobCompletion(e={}){const{interval:t=1e3,signal:r,statusCallback:i}=e;return new Promise(((e,a)=>{(0,w.fu)(r,(()=>{this._clearTimer(),a((0,w.zE)())})),this._clearTimer();const n=setInterval((()=>{this._timer||a((0,w.zE)()),this.checkJobStatus(this.requestOptions).then((t=>{const{jobStatus:r}=t;switch(this.jobStatus=r,r){case"job-succeeded":this._clearTimer(),e(this);break;case"job-submitted":case"job-executing":case"job-waiting":case"job-new":i&&i(this);break;case"job-cancelled":case"job-cancelling":case"job-deleted":case"job-deleting":case"job-timed-out":case"job-failed":this._clearTimer(),a(this)}}))}),t);this._timer=n}))}_clearTimer(){this._timer&&(clearInterval(this._timer),this._timer=null)}};(0,i._)([(0,n.Cb)()],B.prototype,"jobId",void 0),(0,i._)([(0,n.Cb)({json:{read:q.read}})],B.prototype,"jobStatus",void 0),(0,i._)([(0,n.Cb)({type:[U.Z]})],B.prototype,"messages",void 0),(0,i._)([(0,n.Cb)()],B.prototype,"requestOptions",void 0),(0,i._)([(0,n.Cb)({json:{write:!0}})],B.prototype,"sourceUrl",void 0),B=A=(0,i._)([(0,l.j)("esri.rest.support.JobInfo")],B);const K=B,W=new a.J({PDF:"pdf",PNG32:"png32",PNG8:"png8",JPG:"jpg",GIF:"gif",EPS:"eps",SVG:"svg",SVGZ:"svgz"}),Q=(W.fromJSON.bind(W),W.toJSON.bind(W)),Y=new a.J({MAP_ONLY:"map-only","A3 Landscape":"a3-landscape","A3 Portrait":"a3-portrait","A4 Landscape":"a4-landscape","A4 Portrait":"a4-portrait","Letter ANSI A Landscape":"letter-ansi-a-landscape","Letter ANSI A Portrait":"letter-ansi-a-portrait","Tabloid ANSI B Landscape":"tabloid-ansi-b-landscape","Tabloid ANSI B Portrait":"tabloid-ansi-b-portrait"}),H=(Y.fromJSON.bind(Y),Y.toJSON.bind(Y));let X=class extends g.Z{constructor(e){super(e),this.attributionVisible=!0,this.exportOptions={width:800,height:1100,dpi:96},this.forceFeatureAttributes=!1,this.format="png32",this.label=null,this.layout="map-only",this.layoutOptions=null,this.outScale=0,this.scalePreserved=!0,this.showLabels=!0}};(0,i._)([(0,n.Cb)()],X.prototype,"attributionVisible",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"exportOptions",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"forceFeatureAttributes",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"format",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"label",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"layout",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"layoutOptions",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"outScale",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"scalePreserved",void 0),(0,i._)([(0,n.Cb)()],X.prototype,"showLabels",void 0),X=(0,i._)([(0,l.j)("esri.rest.support.PrintTemplate")],X);const ee=X;var te=r(58142);function re(e,t){const{graphic:r,renderer:i,symbol:a}=t,n=a.type;if("text"===n||"shield-label-symbol"===n||!("visualVariables"in i)||!i.visualVariables)return;const o=i.getVisualVariablesForType("size"),s=i.getVisualVariablesForType("color"),l=i.getVisualVariablesForType("opacity"),u=i.getVisualVariablesForType("rotation"),c=o[0],p=s[0],y=l[0],f=u[0];if(c){const t="simple-marker"===n?a.style:null,i=(0,h.ap)(c,r,{shape:t});null!=i&&("simple-marker"===n?e.size=(0,d.p)(i):"picture-marker"===n?(e.width=(0,d.p)(i),e.height=(0,d.p)(i)):"simple-line"===n?e.width=(0,d.p)(i):e.outline&&(e.outline.width=(0,d.p)(i)))}if(p){const t=(0,h.Lq)(p,r);(t&&"simple-marker"===n||"simple-line"===n||"simple-fill"===n)&&(e.color=t?t.toJSON():void 0)}if(y){const t=(0,h.Km)(y,r);null!=t&&e.color&&(e.color[3]=Math.round(255*t))}f&&(e.angle=-(0,h.cM)(i,r))}function ie(e){return e&&"bing-maps"===e.type}function ae(e){return e&&"blendMode"in e&&"effect"in e}function ne(e){return e&&"csv"===e.type}function oe(e){return e&&"feature"===e.type}function se(e){return e&&"geojson"===e.type}function le(e){return e&&"graphics"===e.type}function ue(e){return e&&"group"===e.type}function ce(e){return e&&"esri.views.2d.layers.GroupLayerView2D"===e.declaredClass}function pe(e){return e&&"imagery"===e.type}function ye(e){return e&&"imagery-tile"===e.type}function de(e){return e&&"kml"===e.type}function fe(e){return e&&"map-image"===e.type}function me(e){return e&&"map-notes"===e.type}function be(e){return e&&"open-street-map"===e.type}function he(e){const t=e.layer;if(ae(t)){const r=t.blendMode;if((!r||"normal"===r)&&(t.effect||"featureEffect"in e&&e.featureEffect))return!0}return!1}function ge(e){return e&&"stream"===e.type}function ve(e){return e&&"tile"===e.type}function Se(e){return e&&"vector-tile"===e.type}function we(e){return e&&"web-tile"===e.type}function xe(e){return e&&"wms"===e.type}function Pe(e){return e&&"wmts"===e.type}r(48190),r(85557);const De={Feet:"ft",Kilometers:"km",Meters:"m",Miles:"mi"},Ie=new a.J({esriFeet:"Feet",esriKilometers:"Kilometers",esriMeters:"Meters",esriMiles:"Miles"}),Me=new a.J({esriExecutionTypeSynchronous:"sync",esriExecutionTypeAsynchronous:"async"}),Ge=new Map;async function Le(e,t,r){const i=Oe(e);let a=Ge.get(i);return Promise.resolve().then((()=>a?{data:a.gpMetadata}:(a={gpServerUrl:i,is11xService:!1,legendLayerNameMap:{},legendLayers:[]},(0,p.default)(i,{query:{f:"json"}})))).then((e=>(a.gpMetadata=e.data,a.cimVersion=a.gpMetadata.cimVersion,a.is11xService=!!a.cimVersion,Ge.set(i,a),Ve(t,a)))).then((i=>{const n=function(e){return e.gpMetadata&&e.gpMetadata.executionType?Me.fromJSON(e.gpMetadata.executionType):"sync"}(a);let o;const s=e=>"sync"===n?e.results&&e.results[0]&&e.results[0].value:o.fetchResultData("Output_File",null,r).then((e=>e.value));return"async"===n?async function(e,t,r,i){return r=D.from(r||{}),$(e,"submitJob",r,t,i).then((t=>{const r=K.fromJSON(t.data);return r.sourceUrl=e,r}))}(e,i,null,r).then((e=>(o=e,e.waitForJobCompletion({interval:t.updateDelay}).then(s)))):async function(e,t,r,i){return $(e,"execute",r=D.from(r||{}),t,i).then((e=>{const t=e.data.results||[],r=e.data.messages||[];return{results:t.map(z),messages:r.map((e=>U.Z.fromJSON(e)))}}))}(e,i,null,r).then(s)}))}async function Ve(e,t){t=t||{is11xService:!1,legendLayerNameMap:{},legendLayers:[]};const r=e.template||new ee;null==r.showLabels&&(r.showLabels=!0);const i=r.exportOptions;let a;const n=H(r.layout);if(i&&(a={dpi:i.dpi},"map_only"===n.toLowerCase()||""===n)){const e=i.width,t=i.height;a.outputSize=[e,t]}const s=r.layoutOptions;let l;if(s){let e,t;"Miles"===s.scalebarUnit||"Kilometers"===s.scalebarUnit?(e="Kilometers",t="Miles"):"Meters"!==s.scalebarUnit&&"Feet"!==s.scalebarUnit||(e="Meters",t="Feet"),l={titleText:s.titleText,authorText:s.authorText,copyrightText:s.copyrightText,customTextElements:s.customTextElements,scaleBarOptions:{metricUnit:Ie.toJSON(e),metricLabel:De[e],nonMetricUnit:Ie.toJSON(t),nonMetricLabel:De[t]}}}let u=null;null!=s&&s.legendLayers&&(u=s.legendLayers.map((e=>{t.legendLayerNameMap[e.layerId]=e.title;const r={id:e.layerId};return e.subLayerIds&&(r.subLayerIds=e.subLayerIds),r})));const c=await async function(e,t,r){const i=e.view;let a=i.spatialReference;const n={operationalLayers:await Te(i,t,r)};let s=r.ssExtent||e.extent||i.extent;if(a&&a.isWrappable&&(s=s.clone()._normalize(!0),a=s.spatialReference),n.mapOptions={extent:s&&s.toJSON(),spatialReference:a&&a.toJSON(),showAttribution:t.attributionVisible},r.ssExtent=null,i.background&&(n.background=i.background.toJSON()),i.rotation&&(n.mapOptions.rotation=-i.rotation),t.scalePreserved&&(n.mapOptions.scale=t.outScale||i.scale),(0,o.i)(i.timeExtent)){const e=(0,o.i)(i.timeExtent.start)?i.timeExtent.start.getTime():null,t=(0,o.i)(i.timeExtent.end)?i.timeExtent.end.getTime():null;n.mapOptions.time=[e,t]}return n}(e,r,t);if(c.operationalLayers){const e=new RegExp("[\\u4E00-\\u9FFF\\u0E00-\\u0E7F\\u0900-\\u097F\\u3040-\\u309F\\u30A0-\\u30FF\\u31F0-\\u31FF]"),r=/[\u0600-\u06FF]/,i=t=>{const i=t.text,a=t.font,n=a&&a.family&&a.family.toLowerCase();i&&a&&("arial"===n||"arial unicode ms"===n)&&(a.family=e.test(i)?"Arial Unicode MS":"Arial","normal"!==a.style&&r.test(i)&&(a.family="Arial Unicode MS"))},a=()=>{throw new y.Z("print-task:cim-symbol-unsupported","CIMSymbol is not supported by a print service published from ArcMap")};c.operationalLayers.forEach((e=>{var r,n,o;null!=(r=e.featureCollection)&&r.layers?e.featureCollection.layers.forEach((e=>{var r,n,o,s;if(null!=(r=e.layerDefinition)&&null!=(n=r.drawingInfo)&&null!=(o=n.renderer)&&o.symbol){const r=e.layerDefinition.drawingInfo.renderer;"esriTS"===r.symbol.type?i(r.symbol):"CIMSymbolReference"!==r.symbol.type||t.is11xService||a()}null!=(s=e.featureSet)&&s.features&&e.featureSet.features.forEach((e=>{e.symbol&&("esriTS"===e.symbol.type?i(e.symbol):"CIMSymbolReference"!==e.symbol.type||t.is11xService||a())}))})):!t.is11xService&&null!=(n=e.layerDefinition)&&null!=(o=n.drawingInfo)&&o.renderer&&JSON.stringify(e.layerDefinition.drawingInfo.renderer).includes('"type":"CIMSymbolReference"')&&a()}))}e.outSpatialReference&&(c.mapOptions.spatialReference=e.outSpatialReference.toJSON()),Object.assign(c,{exportOptions:a,layoutOptions:l||{}}),Object.assign(c.layoutOptions,{legendOptions:{operationalLayers:null!=u?u:t.legendLayers.slice()}}),t.legendLayers.length=0,Ge.set(t.gpServerUrl,t);const p={Web_Map_as_JSON:JSON.stringify(c),Format:Q(r.format),Layout_Template:n};return e.extraParameters&&Object.assign(p,e.extraParameters),p}function Oe(e){let t=e;const r=t.lastIndexOf("/GPServer/");return r>0&&(t=t.slice(0,r+9)),t}async function Te(e,t,r){const i=[],a={layerView:null,printTemplate:t,view:e};let n=0;t.scalePreserved&&(n=t.outScale||e.scale);const o=function(e,t){const r=e.allLayerViews.items;if(t===e.scale)return r.filter((e=>!e.suspended));const i=new Array;for(const e of r)ce(e.parent)&&!i.includes(e.parent)||!e.visible||t&&"isVisibleAtScale"in e&&!e.isVisibleAtScale(t)||i.push(e);return i}(e,n);for(const e of o){const t=e.layer;if(!t.loaded||ue(t))continue;let n;a.layerView=e,n=he(e)?await $e(t,a,r):ie(t)?Ce(t):ne(t)?await _e(t,a,r):oe(t)?await je(t,a,r):se(t)?await Fe(t,a,r):le(t)?await Ne(t,a,r):pe(t)?Ee(t,r):ye(t)?Re(t,r):de(t)?await ke(t,a,r):fe(t)?Ae(t,a,r):me(t)?await Ue(a,r):be(t)?{type:"OpenStreetMap"}:ge(t)?await ze(t,a,r):ve(t)?Ze(t):Se(t)?await qe(t,a,r):we(t)?Be(t):xe(t)?Ke(t):Pe(t)?We(t):await $e(t,a,r),n&&(Array.isArray(n)?i.push(...n):(n.id=t.id,n.title=r.legendLayerNameMap[t.id]||t.title,n.opacity=t.opacity,n.minScale=t.minScale||0,n.maxScale=t.maxScale||0,ae(t)&&t.blendMode&&"normal"!==t.blendMode&&(n.blendMode=t.blendMode),i.push(n)))}if(n&&i.forEach((e=>{e.minScale=0,e.maxScale=0})),e.graphics&&e.graphics.length){const a=await Je(null,e.graphics,t,r);a&&i.push(a)}return i}function Ce(e){return{culture:e.culture,key:e.key,type:"BingMaps"+("aerial"===e.style?"Aerial":"hybrid"===e.style?"Hybrid":"Road")}}async function _e(e,t,r){e.legendEnabled&&r.legendLayers.push({id:e.id});const i=t.layerView,a=t.printTemplate;let n;return!r.is11xService||i.filter?Je(e,await et(i),a,r):(n={type:"CSV"},e.write(n,{origin:"web-map"}),delete n.popupInfo,delete n.layerType,n.showLabels=a.showLabels&&e.labelsVisible,n)}async function Je(e,t,r,i){let a;const n={layerDefinition:{name:"polygonLayer",geometryType:"esriGeometryPolygon",drawingInfo:{renderer:null}},featureSet:{geometryType:"esriGeometryPolygon",features:[]}},o={layerDefinition:{name:"polylineLayer",geometryType:"esriGeometryPolyline",drawingInfo:{renderer:null}},featureSet:{geometryType:"esriGeometryPolyline",features:[]}},s={layerDefinition:{name:"pointLayer",geometryType:"esriGeometryPoint",drawingInfo:{renderer:null}},featureSet:{geometryType:"esriGeometryPoint",features:[]}},l={layerDefinition:{name:"multipointLayer",geometryType:"esriGeometryMultipoint",drawingInfo:{renderer:null}},featureSet:{geometryType:"esriGeometryMultipoint",features:[]}},u={layerDefinition:{name:"pointLayer",geometryType:"esriGeometryPoint",drawingInfo:{renderer:null}},featureSet:{geometryType:"esriGeometryPoint",features:[]}};if(u.layerDefinition.name="textLayer",delete u.layerDefinition.drawingInfo,e){if("esri.layers.FeatureLayer"===e.declaredClass||"esri.layers.StreamLayer"===e.declaredClass?n.layerDefinition.name=o.layerDefinition.name=s.layerDefinition.name=l.layerDefinition.name=i.legendLayerNameMap[e.id]||e.get("arcgisProps.title")||e.title:"esri.layers.GraphicsLayer"===e.declaredClass&&(t=e.graphics.items),e.renderer){const t=e.renderer.toJSON();n.layerDefinition.drawingInfo.renderer=t,o.layerDefinition.drawingInfo.renderer=t,s.layerDefinition.drawingInfo.renderer=t,l.layerDefinition.drawingInfo.renderer=t}if(r.showLabels&&e.labelsVisible&&"function"==typeof e.write){var c,p;const t=null==(c=e.write({},{origin:"web-map"}).layerDefinition)||null==(p=c.drawingInfo)?void 0:p.labelingInfo;t&&(a=!0,n.layerDefinition.drawingInfo.labelingInfo=t,o.layerDefinition.drawingInfo.labelingInfo=t,s.layerDefinition.drawingInfo.labelingInfo=t,l.layerDefinition.drawingInfo.labelingInfo=t)}}let y;null!=e&&e.renderer||a||(delete n.layerDefinition.drawingInfo,delete o.layerDefinition.drawingInfo,delete s.layerDefinition.drawingInfo,delete l.layerDefinition.drawingInfo);const d=null==e?void 0:e.fieldsIndex,f=null==e?void 0:e.renderer;if(d){const t=new Set;a&&await(0,b.Mu)(t,e),f&&"function"==typeof f.collectRequiredFields&&await f.collectRequiredFields(t,d),y=Array.from(t);const r=d.fields.map((e=>e.toJSON()));n.layerDefinition.fields=r,o.layerDefinition.fields=r,s.layerDefinition.fields=r,l.layerDefinition.fields=r}const h=t&&t.length;let g;for(let a=0;a<h;a++){var v;const c=t[a]||t.getItemAt(a);if(!1===c.visible||!c.geometry)continue;if(g=c.toJSON(),g.hasOwnProperty("popupTemplate")&&delete g.popupTemplate,g.geometry&&g.geometry.z&&delete g.geometry.z,g.symbol&&g.symbol.outline&&"esriCLS"===g.symbol.outline.type&&!i.is11xService)continue;!i.is11xService&&g.symbol&&g.symbol.outline&&g.symbol.outline.color&&g.symbol.outline.color[3]&&(g.symbol.outline.color[3]=255);const p=e&&e.renderer&&("valueExpression"in e.renderer&&e.renderer.valueExpression||"hasVisualVariables"in e.renderer&&e.renderer.hasVisualVariables());if(!g.symbol&&e&&e.renderer&&p&&!i.is11xService){const t=e.renderer,r=await t.getSymbolAsync(c);if(!r)continue;g.symbol=r.toJSON(),"hasVisualVariables"in t&&t.hasVisualVariables()&&re(g.symbol,{renderer:t,graphic:c,symbol:r})}if(g.symbol&&(g.symbol.angle||delete g.symbol.angle,tt(g.symbol)?g.symbol=await He(g.symbol,i):g.symbol.text&&delete g.attributes),(!r||!r.forceFeatureAttributes)&&null!=(v=y)&&v.length){const e={};y.forEach((t=>{g.attributes&&g.attributes.hasOwnProperty(t)&&(e[t]=g.attributes[t])})),g.attributes=e}"polygon"===c.geometry.type?n.featureSet.features.push(g):"polyline"===c.geometry.type?o.featureSet.features.push(g):"point"===c.geometry.type?g.symbol&&g.symbol.text?u.featureSet.features.push(g):s.featureSet.features.push(g):"multipoint"===c.geometry.type?l.featureSet.features.push(g):"extent"===c.geometry.type&&(g.geometry=m.Z.fromExtent(c.geometry).toJSON(),n.featureSet.features.push(g))}const S=[n,o,l,s,u].filter((e=>e.featureSet.features.length>0));for(const e of S){const t=e.featureSet.features.every((e=>e.symbol));!t||r&&r.forceFeatureAttributes||e.featureSet.features.forEach((e=>{delete e.attributes})),t&&delete e.layerDefinition.drawingInfo,e.layerDefinition.drawingInfo&&e.layerDefinition.drawingInfo.renderer&&await Xe(e.layerDefinition.drawingInfo.renderer,i)}return S.length?{featureCollection:{layers:S},showLabels:a}:null}async function je(e,t,r){var i,a;let n;const o=e.renderer,s=parseFloat(r.cimVersion);if(e.featureReduction&&(!r.is11xService||s<2.9)||"dot-density"===(null==o?void 0:o.type)&&(!r.is11xService||s<2.6))return $e(e,t,r);e.legendEnabled&&r.legendLayers.push({id:e.id});const l=t.layerView,{printTemplate:u,view:c}=t,p=o&&("valueExpression"in o&&o.valueExpression||"hasVisualVariables"in o&&o.hasVisualVariables()),y="feature-layer"!==(null==(i=e.source)?void 0:i.type)&&"ogc-feature"!==(null==(a=e.source)?void 0:a.type);if(!r.is11xService&&p||l.filter||y||!o||"field"in o&&null!=o.field&&("string"!=typeof o.field||!e.getField(o.field))){const t=await et(l);n=await Je(e,t,u,r)}else{var d,f;if(n={id:(m=e.write()).id,title:m.title,opacity:m.opacity,minScale:m.minScale,maxScale:m.maxScale,url:m.url,layerType:m.layerType,customParameters:m.customParameters,layerDefinition:m.layerDefinition},n.showLabels=u.showLabels&&e.labelsVisible,Qe(n,e),null!=(d=n.layerDefinition)&&null!=(f=d.drawingInfo)&&f.renderer&&(delete n.layerDefinition.minScale,delete n.layerDefinition.maxScale,await Xe(n.layerDefinition.drawingInfo.renderer,r),"visualVariables"in o&&o.visualVariables&&o.visualVariables[0])){const e=o.visualVariables[0];if("size"===e.type&&e.maxSize&&"number"!=typeof e.maxSize&&e.minSize&&"number"!=typeof e.minSize){const t=(0,h.V3)(e,c.scale);n.layerDefinition.drawingInfo.renderer.visualVariables[0].minSize=t.minSize,n.layerDefinition.drawingInfo.renderer.visualVariables[0].maxSize=t.maxSize}}const t=(0,te.a)(l);t&&(n.layerDefinition||(n.layerDefinition={}),n.layerDefinition.definitionExpression=n.layerDefinition.definitionExpression?`(${n.layerDefinition.definitionExpression}) AND (${t})`:t)}var m;return n}async function Fe(e,{layerView:t,printTemplate:r},i){return e.legendEnabled&&i.legendLayers.push({id:e.id}),Je(e,await et(t),r,i)}async function Ne(e,{printTemplate:t},r){return Je(e,null,t,r)}function Ee(e,t){e.legendEnabled&&t.legendLayers.push({id:e.id});const r={layerType:(i=e.write()).layerType,customParameters:i.customParameters};var i;if(r.bandIds=e.bandIds,r.compressionQuality=e.compressionQuality,r.format=e.format,r.interpolation=e.interpolation,(e.mosaicRule||e.definitionExpression)&&(r.mosaicRule=e.exportImageServiceParameters.mosaicRule.toJSON()),e.renderingRule||e.renderer)if(t.is11xService)e.renderingRule&&(r.renderingRule=e.renderingRule.toJSON()),e.renderer&&(r.layerDefinition=r.layerDefinition||{},r.layerDefinition.drawingInfo=r.layerDefinition.drawingInfo||{},r.layerDefinition.drawingInfo.renderer=e.renderer.toJSON());else{const t=e.exportImageServiceParameters.combineRendererWithRenderingRule();t&&(r.renderingRule=t.toJSON())}return Qe(r,e),r}function Re(e,t){e.legendEnabled&&t.legendLayers.push({id:e.id});const r={bandIds:(i=e.write()||{}).bandIds,customParameters:i.customParameters,interpolation:i.interpolation,layerDefinition:i.layerDefinition};var i;return r.layerType="ArcGISImageServiceLayer",Qe(r,e),r}async function ke(e,t,r){const i=t.printTemplate;if(r.is11xService){const t={type:"kml"};return e.write(t,{origin:"web-map"}),delete t.layerType,t.url=(0,f.Fv)(e.url),t}{const a=[],n=t.layerView;n.allVisibleMapImages.forEach(((t,r)=>{const i={id:`${e.id}_image${r}`,type:"image",title:e.id,minScale:e.minScale||0,maxScale:e.maxScale||0,opacity:e.opacity,extent:t.extent};"data:image/png;base64,"===t.href.substr(0,22)?i.imageData=t.href.substr(22):i.url=t.href,a.push(i)}));const o=[...n.allVisiblePoints.items,...n.allVisiblePolylines.items,...n.allVisiblePolygons.items],s={id:e.id,...await Je(null,o,i,r)};return a.push(s),a}}function Ae(e,{view:t},r){let i;const a={id:e.id,subLayerIds:[]};let n=[];const o=t.scale,s=e=>{const t=0===o,r=0===e.minScale||o<=e.minScale,i=0===e.maxScale||o>=e.maxScale;if(e.visible&&(t||r&&i))if(e.sublayers)e.sublayers.forEach(s);else{const t=e.toExportImageJSON(),r={id:e.id,name:e.title,layerDefinition:{drawingInfo:t.drawingInfo,definitionExpression:t.definitionExpression,source:t.source}};n.unshift(r),a.subLayerIds.push(e.id)}};var l;return e.sublayers&&e.sublayers.forEach(s),n.length&&(n=n.map((({id:e,name:t,layerDefinition:r})=>({id:e,name:t,layerDefinition:r}))),i={layerType:(l=e.write()).layerType,customParameters:l.customParameters},i.layers=n,i.visibleLayers=e.capabilities.exportMap.supportsDynamicLayers?void 0:a.subLayerIds,Qe(i,e),e.legendEnabled&&r.legendLayers.push(a)),i}async function Ue({layerView:e,printTemplate:t},r){const i=[],a=e.layer;if((0,o.i)(a.featureCollections))for(const e of a.featureCollections){const a=await Je(e,e.source,t,r);a&&i.push(...a.featureCollection.layers)}else if((0,o.i)(a.sublayers))for(const e of a.sublayers){const a=await Je(null,e.graphics,t,r);a&&i.push(...a.featureCollection.layers)}return{featureCollection:{layers:i}}}async function $e(e,{printTemplate:t,view:r},i){const a={type:"image"},n={format:"png",ignoreBackground:!0,layers:[e],rotation:0},o=i.ssExtent||r.extent.clone();let s=96,l=!0,u=!0;if(t.exportOptions){const e=t.exportOptions;e.dpi>0&&(s=e.dpi),e.width>0&&(l=e.width%2==r.width%2),e.height>0&&(u=e.height%2==r.height%2)}if("map-only"===t.layout&&t.scalePreserved&&(!t.outScale||t.outScale===r.scale)&&96===s&&(!l||!u)&&(n.area={x:0,y:0,width:r.width,height:r.height},l||(n.area.width-=1),u||(n.area.height-=1),!i.ssExtent)){const e=r.toMap((0,d.c)(n.area.width,n.area.height));o.ymin=e.y,o.xmax=e.x,i.ssExtent=o}a.extent=o.clone()._normalize(!0).toJSON();const c=await r.takeScreenshot(n),{data:p}=(0,f.sJ)(c.dataUrl);return a.imageData=p,a}async function ze(e,{layerView:t,printTemplate:r},i){return e.legendEnabled&&i.legendLayers.push({id:e.id}),Je(e,await et(t),r,i)}function Ze(e){const t={layerType:(r=e.write()).layerType,customParameters:r.customParameters};var r;return Qe(t,e),t}async function qe(e,t,r){if(r.is11xService&&e.serviceUrl&&e.styleUrl){const t=Ye(e.styleUrl,e.apiKey),i=Ye(e.serviceUrl,e.apiKey);if(!t&&!i||"2.1.0"!==r.cimVersion){const r={type:"VectorTileLayer"};return r.styleUrl=(0,f.Fv)(e.styleUrl),r.token=t,i!==t&&(r.additionalTokens=[{url:e.serviceUrl,token:i}]),r}}return $e(e,t,r)}function Be(e){const t={type:"WebTiledLayer",urlTemplate:e.urlTemplate.replace(/\${/g,"{"),credits:e.copyright};return e.subDomains&&e.subDomains.length>0&&(t.subDomains=e.subDomains),t}function Ke(e){let t;const r=[],i=e=>{e.visible&&(e.sublayers?e.sublayers.forEach(i):e.name&&r.unshift(e.name))};return e.sublayers&&e.sublayers.forEach(i),r.length&&(t={type:"wms",customLayerParameters:e.customLayerParameters,customParameters:e.customParameters,transparentBackground:e.imageTransparency,visibleLayers:r,url:(0,f.Fv)(e.url),version:e.version}),t}function We(e){const t=e.activeLayer;return{type:"wmts",customLayerParameters:e.customLayerParameters,customParameters:e.customParameters,format:t.imageFormat,layer:t.id,style:t.styleId,tileMatrixSet:t.tileMatrixSetId,url:(0,f.Fv)(e.url)}}function Qe(e,t){t.url&&(e.url=(0,f.Fv)(e.url||t.url),e.token=Ye(e.url,t.apiKey))}function Ye(e,t){var r,i;return(0,p.s)(e)&&(t||u.Z.apiKey)?t||u.Z.apiKey:null==(r=c.id)||null==(i=r.findCredential(e))?void 0:i.token}async function He(e,t){t.canvas||(t.canvas=document.createElement("canvas"));const r=1024;t.canvas.width=r,t.canvas.height=r;const i=t.canvas.getContext("2d");let a,n;if(e.path){var o;const t=new Path2D(e.path);t.closePath(),i.fillStyle=Array.isArray(e.color)?`rgba(${e.color[0]},${e.color[1]},${e.color[2]},${e.color[3]/255})`:"rgb(0,0,0)",i.fill(t);const s=function(e,t=15){const r=e.canvas.width,i=e.canvas.height,a=e.getImageData(0,0,r,i).data;let n,o,s,l,u,c;e:for(o=i;o--;)for(n=r;n--;)if(a[4*(r*o+n)+3]>t){c=o;break e}if(!c)return null;e:for(n=r;n--;)for(o=c+1;o--;)if(a[4*(r*o+n)+3]>t){u=n;break e}e:for(n=0;n<=u;++n)for(o=c+1;o--;)if(a[4*(r*o+n)+3]>t){s=n;break e}e:for(o=0;o<=c;++o)for(n=s;n<=u;++n)if(a[4*(r*o+n)+3]>t){l=o;break e}return{x:s,y:l,width:u-s,height:c-l}}(i);if(!s)return null;i.clearRect(0,0,r,r);const l=(0,d.a)(e.size)/Math.max(s.width,s.height);i.scale(l,l);const u=r/l,c=u/2-s.width/2-s.x,p=u/2-s.height/2-s.y;if(i.translate(c,p),Array.isArray(e.color)&&i.fill(t),null!=(o=e.outline)&&o.width&&Array.isArray(e.outline.color)){const r=e.outline;i.lineWidth=(0,d.a)(r.width)/l,i.lineJoin="round",i.strokeStyle=`rgba(${r.color[0]},${r.color[1]},${r.color[2]},${r.color[3]/255})`,i.stroke(t),s.width+=i.lineWidth,s.height+=i.lineWidth}s.width*=l,s.height*=l;const y=i.getImageData(512-s.width/2,512-s.height/2,Math.ceil(s.width),Math.ceil(s.height));a=y.width,n=y.height,i.canvas.width=a,i.canvas.height=n,i.putImageData(y,0,0)}else{const t="image/svg+xml"===e.contentType?"data:image/svg+xml;base64,"+e.imageData:e.url,r=(await(0,p.default)(t,{responseType:"image"})).data;a=(0,d.a)(e.width),n=(0,d.a)(e.height),i.canvas.width=a,i.canvas.height=n,i.drawImage(r,0,0,i.canvas.width,i.canvas.height)}return{type:"esriPMS",imageData:i.canvas.toDataURL("image/png").substr(22),angle:e.angle,contentType:"image/png",height:(0,d.p)(n),width:(0,d.p)(a),xoffset:e.xoffset,yoffset:e.yoffset}}async function Xe(e,t){const r=e.type;if("simple"===r&&tt(e.symbol))e.symbol=await He(e.symbol,t);else if("uniqueValue"===r||"classBreaks"===r){tt(e.defaultSymbol)&&(e.defaultSymbol=await He(e.defaultSymbol,t));const i=e["uniqueValue"===r?"uniqueValueInfos":"classBreakInfos"];if(i)for(const e of i)tt(e.symbol)&&(e.symbol=await He(e.symbol,t))}}async function et(e){return e.queryFeatures(e.createQuery()).then((e=>e.features))}function tt(e){return e&&(e.path||"image/svg+xml"===e.contentType||e.url&&e.url.endsWith(".svg"))}var rt=r(658);const it=new a.J({esriExecutionTypeSynchronous:"sync",esriExecutionTypeAsynchronous:"async"});let at=class extends rt.Z{constructor(...e){super(...e),this._gpMetadata=null,this.updateDelay=1e3}get mode(){return this._gpMetadata&&this._gpMetadata.executionType?it.fromJSON(this._gpMetadata.executionType):"sync"}execute(e,t){return e&&(e.updateDelay=this.updateDelay),Le(this.url,e,{...this.requestOptions,...t})}async _getGpPrintParams(e){const t=Oe(this.url);return Ve(e,Ge.get(t))}};(0,i._)([(0,n.Cb)()],at.prototype,"_gpMetadata",void 0),(0,i._)([(0,n.Cb)({readOnly:!0})],at.prototype,"mode",null),(0,i._)([(0,n.Cb)()],at.prototype,"updateDelay",void 0),at=(0,i._)([(0,l.j)("esri.tasks.PrintTask")],at);const nt=at}}]);