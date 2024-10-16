"use strict";(self.webpackChunkRemoteClient=self.webpackChunkRemoteClient||[]).push([[5528],{91171:(t,e,r)=>{r.d(e,{a:()=>C,c:()=>N,d:()=>T,e:()=>j,f:()=>_,g:()=>A,h:()=>B,i:()=>E,j:()=>L,p:()=>R});var n=r(59877),o=r(88762),s=r(82058),i=r(32101),a=r(53426),u=r(49214),c=r(34446),l=(r(74569),r(31292)),f=r(21801),d=r(48190),h=r(76506),p=r(92847),y=r(40267),m=r(87258),g=r(46646),b=r(39210),S=r(32422),O=r(95533),x=r(55823);function F(t){const e=t.toJSON();return e.attachmentTypes&&(e.attachmentTypes=e.attachmentTypes.join(",")),e.keywords&&(e.keywords=e.keywords.join(",")),e.globalIds&&(e.globalIds=e.globalIds.join(",")),e.objectIds&&(e.objectIds=e.objectIds.join(",")),e.size&&(e.size=e.size.join(",")),e}function R(t,e){const r={};for(const n of t){const{parentObjectId:t,parentGlobalId:s,attachmentInfos:u}=n;for(const n of u){const{id:u}=n,c=(0,i.qg)((0,o.Dp)(`${e}/${t}/attachments/${u}`)),l=a.Z.fromJSON(n);l.set({url:c,parentObjectId:t,parentGlobalId:s}),r[t]?r[t].push(l):r[t]=[l]}}return r}async function j(t,e,r){const o=(0,n.p)(t);return function(t,e,r){let n={query:(0,u.m)({...t.query,f:"json",...F(e)})};return r&&(n={...r,...n,query:{...r.query,...n.query}}),(0,s.default)(t.path+"/queryAttachments",n)}(o,c.Z.from(e),{...r}).then((t=>R(t.data.attachmentGroups,o.path)))}async function C(t,e,r){const o=(0,n.p)(t);return(0,u.c)(o,l.Z.from(e),{...r}).then((t=>({count:t.data.count,extent:f.Z.fromJSON(t.data.extent)})))}function w(t,e){return e}function Z(t,e,r,n){switch(r){case 0:return P(t,e+n,0);case 1:return"lowerLeft"===t.originPosition?P(t,e+n,1):function({translate:t,scale:e},r,n){return t[n]-r*e[n]}(t,e+n,1)}}function D(t,e,r,n){return 2===r?P(t,e,2):Z(t,e,r,n)}function q(t,e,r,n){return 2===r?P(t,e,3):Z(t,e,r,n)}function I(t,e,r,n){return 3===r?P(t,e,3):D(t,e,r,n)}function P({translate:t,scale:e},r,n){return t[n]+r*e[n]}class v{constructor(t){this.options=t,this.geometryTypes=["esriGeometryPoint","esriGeometryMultipoint","esriGeometryPolyline","esriGeometryPolygon"],this.previousCoordinate=[0,0],this.transform=null,this.applyTransform=w,this.lengths=[],this.currentLengthIndex=0,this.toAddInCurrentPath=0,this.vertexDimension=0,this.coordinateBuffer=null,this.coordinateBufferPtr=0,this.AttributesConstructor=function(){}}createFeatureResult(){return{fields:[],features:[]}}finishFeatureResult(t){if(this.options.applyTransform&&(t.transform=null),this.AttributesConstructor=function(){},this.coordinateBuffer=null,this.lengths.length=0,!t.hasZ)return;const e=(0,p.g)(t.geometryType,this.options.sourceSpatialReference,t.spatialReference);if(!(0,h.b)(e))for(const r of t.features)e(r.geometry)}createSpatialReference(){return{}}addField(t,e){t.fields.push(e);const r=t.fields.map((t=>t.name));this.AttributesConstructor=function(){for(const t of r)this[t]=null}}addFeature(t,e){t.features.push(e)}prepareFeatures(t){switch(this.transform=t.transform,this.options.applyTransform&&t.transform&&(this.applyTransform=this.deriveApplyTransform(t)),this.vertexDimension=2,t.hasZ&&this.vertexDimension++,t.hasM&&this.vertexDimension++,t.geometryType){case"esriGeometryPoint":this.addCoordinate=(t,e,r)=>this.addCoordinatePoint(t,e,r),this.createGeometry=t=>this.createPointGeometry(t);break;case"esriGeometryPolygon":this.addCoordinate=(t,e,r)=>this.addCoordinatePolygon(t,e,r),this.createGeometry=t=>this.createPolygonGeometry(t);break;case"esriGeometryPolyline":this.addCoordinate=(t,e,r)=>this.addCoordinatePolyline(t,e,r),this.createGeometry=t=>this.createPolylineGeometry(t);break;case"esriGeometryMultipoint":this.addCoordinate=(t,e,r)=>this.addCoordinateMultipoint(t,e,r),this.createGeometry=t=>this.createMultipointGeometry(t);break;default:(0,d.n)(t.geometryType)}}createFeature(){return this.lengths.length=0,this.currentLengthIndex=0,this.previousCoordinate[0]=0,this.previousCoordinate[1]=0,this.coordinateBuffer=null,this.coordinateBufferPtr=0,{attributes:new this.AttributesConstructor}}allocateCoordinates(){}addLength(t,e,r){0===this.lengths.length&&(this.toAddInCurrentPath=e),this.lengths.push(e)}addQueryGeometry(t,e){const{queryGeometry:r,queryGeometryType:n}=e,o=(0,y.u)(r.clone(),r,!1,!1,this.transform),s=(0,y.a)(o,n,!1,!1);t.queryGeometryType=n,t.queryGeometry={...s}}createPointGeometry(t){const e={x:0,y:0,spatialReference:t.spatialReference};return t.hasZ&&(e.z=0),t.hasM&&(e.m=0),e}addCoordinatePoint(t,e,r){switch(e=this.applyTransform(this.transform,e,r,0),r){case 0:t.x=e;break;case 1:t.y=e;break;case 2:"z"in t?t.z=e:t.m=e;break;case 3:t.m=e}}transformPathLikeValue(t,e){let r=0;return e<=1&&(r=this.previousCoordinate[e],this.previousCoordinate[e]+=t),this.applyTransform(this.transform,t,e,r)}addCoordinatePolyline(t,e,r){this.dehydratedAddPointsCoordinate(t.paths,e,r)}addCoordinatePolygon(t,e,r){this.dehydratedAddPointsCoordinate(t.rings,e,r)}addCoordinateMultipoint(t,e,r){0===r&&t.points.push([]);const n=this.transformPathLikeValue(e,r);t.points[t.points.length-1].push(n)}createPolygonGeometry(t){return{rings:[[]],spatialReference:t.spatialReference,hasZ:!!t.hasZ,hasM:!!t.hasM}}createPolylineGeometry(t){return{paths:[[]],spatialReference:t.spatialReference,hasZ:!!t.hasZ,hasM:!!t.hasM}}createMultipointGeometry(t){return{points:[],spatialReference:t.spatialReference,hasZ:!!t.hasZ,hasM:!!t.hasM}}dehydratedAddPointsCoordinate(t,e,r){0===r&&0==this.toAddInCurrentPath--&&(t.push([]),this.toAddInCurrentPath=this.lengths[++this.currentLengthIndex]-1,this.previousCoordinate[0]=0,this.previousCoordinate[1]=0);const n=this.transformPathLikeValue(e,r),o=t[t.length-1];0===r&&(this.coordinateBufferPtr=0,this.coordinateBuffer=new Array(this.vertexDimension),o.push(this.coordinateBuffer)),this.coordinateBuffer[this.coordinateBufferPtr++]=n}deriveApplyTransform(t){const{hasZ:e,hasM:r}=t;return e&&r?I:e?D:r?q:Z}}async function N(t,e,r){const o=(0,n.p)(t),s={...r},i=l.Z.from(e),a=!i.quantizationParameters,{data:c}=await(0,u.d)(o,i,new v({sourceSpatialReference:i.sourceSpatialReference,applyTransform:a}),s);return c}function J(t,e){const r=t.toJSON();return r.objectIds&&(r.objectIds=r.objectIds.join(",")),r.orderByFields&&(r.orderByFields=r.orderByFields.join(",")),!r.outFields||null!=e&&e.returnCountOnly?delete r.outFields:-1!==r.outFields.indexOf("*")?r.outFields="*":r.outFields=r.outFields.join(","),r.outSpatialReference&&(r.outSR=r.outSR.wkid||JSON.stringify(r.outSR.toJSON()),delete r.outSpatialReference),r.dynamicDataSource&&(r.layer=JSON.stringify({source:r.dynamicDataSource}),delete r.dynamicDataSource),r}async function G(t,e,r={},n){const o=(0,u.m)({...t.query,f:"json",...n,...J(e,n)});return(0,s.default)(t.path+"/queryRelatedRecords",{...r,query:{...r.query,...o}})}async function T(t,e,r){return e=g.Z.from(e),async function(t,e,r){const n=await G(t,e,r),o=n.data,s=o.geometryType,i=o.spatialReference,a={};for(const t of o.relatedRecordGroups){const e={fields:void 0,objectIdFieldName:void 0,geometryType:s,spatialReference:i,hasZ:!!o.hasZ,hasM:!!o.hasM,features:t.relatedRecords};if(null!=t.objectId)a[t.objectId]=e;else for(const r in t)t.hasOwnProperty(r)&&"relatedRecords"!==r&&(a[t[r]]=e)}return{...n,data:a}}((0,n.p)(t),e,r).then((t=>{const e=t.data,r={};return Object.keys(e).forEach((t=>r[t]=m.default.fromJSON(e[t]))),r}))}async function _(t,e,r){return e=g.Z.from(e),async function(t,e,r){const n=await G(t,e,r,{returnCountOnly:!0}),o=n.data,s={};for(const t of o.relatedRecordGroups)null!=t.objectId&&(s[t.objectId]=t.count);return{...n,data:s}}((0,n.p)(t),e,{...r}).then((t=>t.data))}function k(t,e){var r,n;const o=t.geometry,s=t.toJSON(),i=s;if((0,h.i)(o)&&(i.geometry=JSON.stringify(o),i.geometryType=(0,S.Ji)(o),i.inSR=o.spatialReference.wkid||JSON.stringify(o.spatialReference)),null!=(r=s.topFilter)&&r.groupByFields&&(i.topFilter.groupByFields=s.topFilter.groupByFields.join(",")),null!=(n=s.topFilter)&&n.orderByFields&&(i.topFilter.orderByFields=s.topFilter.orderByFields.join(",")),s.topFilter&&(i.topFilter=JSON.stringify(i.topFilter)),s.objectIds&&(i.objectIds=s.objectIds.join(",")),s.orderByFields&&(i.orderByFields=s.orderByFields.join(",")),s.outFields&&!(null!=e&&e.returnCountOnly||null!=e&&e.returnExtentOnly||null!=e&&e.returnIdsOnly)?-1!==s.outFields.indexOf("*")?i.outFields="*":i.outFields=s.outFields.join(","):delete i.outFields,s.outSR?i.outSR=s.outSR.wkid||JSON.stringify(s.outSR):o&&s.returnGeometry&&(i.outSR=i.inSR),s.returnGeometry&&delete s.returnGeometry,s.timeExtent){const t=s.timeExtent,{start:e,end:r}=t;null==e&&null==r||(i.time=e===r?e:`${null==e?"null":e},${null==r?"null":r}`),delete s.timeExtent}return i}function M(t,e,r,n={},o={}){const a="string"==typeof t?(0,i.mN)(t):t,c=e.geometry?[e.geometry]:[];return n.responseType="pbf"===r?"array-buffer":"json",(0,O.aX)(c,null,n).then((t=>{const c=t&&t[0];(0,h.i)(c)&&((e=e.clone()).geometry=c);const l=(0,u.m)({...a.query,f:r,...o,...k(e,o)});return(0,s.default)((0,i.v_)(a.path,"queryTopFeatures"),{...n,query:{...l,...n.query}})}))}async function A(t,e,r,o){const s=(0,n.p)(t),i={...o},{data:a}=await async function(t,e,r,n){const o=await M(t,e,"json",n);return(0,x.a)(e,r,o.data),o}(s,b.Z.from(e),r,i);return m.default.fromJSON(a)}async function B(t,e,r){const o=(0,n.p)(t),s=await async function(t,e,r){return(0,h.i)(e.timeExtent)&&e.timeExtent.isEmpty?Promise.resolve({data:{objectIds:[]}}):M(t,e,"json",r,{returnIdsOnly:!0})}(o,b.Z.from(e),{...r});return s.data.objectIds}async function E(t,e,r){const o=(0,n.p)(t),s=await async function(t,e,r){return(0,h.i)(e.timeExtent)&&e.timeExtent.isEmpty?Promise.resolve({data:{count:0,extent:null}}):M(t,e,"json",r,{returnExtentOnly:!0,returnCountOnly:!0}).then((t=>{const e=t.data;if(e.hasOwnProperty("extent"))return t;if(e.features)throw new Error("Layer does not support extent calculation.");if(e.hasOwnProperty("count"))throw new Error("Layer does not support extent calculation.");return t}))}(o,b.Z.from(e),{...r});return{count:s.data.count,extent:f.Z.fromJSON(s.data.extent)}}async function L(t,e,r){const o=(0,n.p)(t),s=await function(t,e,r){return(0,h.i)(e.timeExtent)&&e.timeExtent.isEmpty?Promise.resolve({data:{count:0}}):M(t,e,"json",r,{returnIdsOnly:!0,returnCountOnly:!0})}(o,b.Z.from(e),{...r});return s.data.count}},94751:(t,e,r)=>{r.d(e,{a:()=>l,c:()=>i,g:()=>c,o:()=>a,u:()=>u});var n=r(92896),o=r(60947),s=r(32422);const i={102100:{maxX:20037508.342788905,minX:-20037508.342788905,plus180Line:new n.Z({paths:[[[20037508.342788905,-20037508.342788905],[20037508.342788905,20037508.342788905]]],spatialReference:o.Z.WebMercator}),minus180Line:new n.Z({paths:[[[-20037508.342788905,-20037508.342788905],[-20037508.342788905,20037508.342788905]]],spatialReference:o.Z.WebMercator})},4326:{maxX:180,minX:-180,plus180Line:new n.Z({paths:[[[180,-180],[180,180]]],spatialReference:o.Z.WGS84}),minus180Line:new n.Z({paths:[[[-180,-180],[-180,180]]],spatialReference:o.Z.WGS84})}};function a(t,e){return Math.ceil((t-e)/(2*e))}function u(t,e){const r=c(t);for(const t of r)for(const r of t)r[0]+=e;return t}function c(t){return(0,s.oU)(t)?t.rings:t.paths}function l(t){const e=(null==t?void 0:t.isWebMercator)?102100:4326;return[i[e].minX,i[e].maxX]}},95533:(t,e,r)=>{r.d(e,{aX:()=>x});var n=r(31450),o=r(60991),s=r(92143),i=r(76506),a=r(44567),u=r(92896),c=r(94751),l=r(60947),f=r(35132),d=(r(74569),r(82058)),h=r(32101),p=r(32422);async function y(t,e,r){const n="string"==typeof t?(0,h.mN)(t):t,o=e[0].spatialReference,s=(0,p.Ji)(e[0]),i={...r,query:{...n.query,f:"json",sr:o.wkid?o.wkid:JSON.stringify(o),geometries:JSON.stringify(m(e))}};return function(t,e,r){const n=(0,p.q9)(e);return t.map((t=>{const e=n.fromJSON(t);return e.spatialReference=r,e}))}((await(0,d.default)(n.path+"/simplify",i)).data,s,o)}function m(t){return{geometryType:(0,p.Ji)(t[0]),geometries:t.map((t=>t.toJSON()))}}r(71552),r(40642),r(34250),r(91306),r(86656),r(22723),r(17533),r(2906),r(21801),r(73796),r(74673),r(21972),r(23639),r(91055),r(6906),r(50406),r(97714),r(91597),r(86787),r(89623),r(98380),r(84069),r(22781),r(57251),r(88762);const g=s.L.getLogger("esri.geometry.support.normalizeUtils");function b(t){return"polyline"===t[0].type}function S(t,e,r){if(e){const e=function(t,e){if(!(t instanceof u.Z||t instanceof a.Z)){const t="straightLineDensify: the input geometry is neither polyline nor polygon";throw g.error(t),new o.Z(t)}const r=(0,c.g)(t),n=[];for(const t of r){const r=[];n.push(r),r.push([t[0][0],t[0][1]]);for(let n=0;n<t.length-1;n++){const o=t[n][0],s=t[n][1],i=t[n+1][0],a=t[n+1][1],u=Math.sqrt((i-o)*(i-o)+(a-s)*(a-s)),c=(a-s)/u,l=(i-o)/u,f=u/e;if(f>1){for(let t=1;t<=f-1;t++){const n=t*e,i=l*n+o,a=c*n+s;r.push([i,a])}const t=(u+Math.floor(f-1)*e)/2,n=l*t+o,i=c*t+s;r.push([n,i])}r.push([i,a])}}return function(t){return"polygon"===t.type}(t)?new a.Z({rings:n,spatialReference:t.spatialReference}):new u.Z({paths:n,spatialReference:t.spatialReference})}(t,1e6);t=(0,f.Sx)(e,!0)}return r&&(t=(0,c.u)(t,r)),t}function O(t,e,r){if(Array.isArray(t)){const n=t[0];if(n>e){const r=(0,c.o)(n,e);t[0]=n+r*(-2*e)}else if(n<r){const e=(0,c.o)(n,r);t[0]=n+e*(-2*r)}}else{const n=t.x;if(n>e){const r=(0,c.o)(n,e);t=t.clone().offset(r*(-2*e),0)}else if(n<r){const e=(0,c.o)(n,r);t=t.clone().offset(e*(-2*r),0)}}return t}async function x(t,e,r){var o;if(!Array.isArray(t))return x([t],e);const s=null!=(o=null==e?void 0:e.url)?o:n.Z.geometryServiceUrl;let m,g,F,R,j,C,w,Z,D=0;const q=[],I=[];for(const e of t)if((0,i.b)(e))I.push(e);else if(m||(m=e.spatialReference,g=(0,l.g)(m),F=m.isWebMercator,C=F?102100:4326,R=c.c[C].maxX,j=c.c[C].minX,w=c.c[C].plus180Line,Z=c.c[C].minus180Line),g)if("mesh"===e.type)I.push(e);else if("point"===e.type)I.push(O(e.clone(),R,j));else if("multipoint"===e.type){const t=e.clone();t.points=t.points.map((t=>O(t,R,j))),I.push(t)}else if("extent"===e.type){const t=e.clone()._normalize(!1,!1,g);I.push(t.rings?new a.Z(t):t)}else if(e.extent){const t=e.extent,r=(0,c.o)(t.xmin,j)*(2*R);let n=0===r?e.clone():(0,c.u)(e.clone(),r);t.offset(r,0),t.intersects(w)&&t.xmax!==R?(D=t.xmax>D?t.xmax:D,n=S(n,F),q.push(n),I.push("cut")):t.intersects(Z)&&t.xmin!==j?(D=t.xmax*(2*R)>D?t.xmax*(2*R):D,n=S(n,F,360),q.push(n),I.push("cut")):I.push(n)}else I.push(e.clone());else I.push(e);let P=(0,c.o)(D,R),v=-90;const N=P,J=new u.Z;for(;P>0;){const t=360*P-180;J.addPath([[t,v],[t,-1*v]]),v*=-1,P--}if(q.length>0&&N>0){const e=await async function(t,e,r,n){const o="string"==typeof t?(0,h.mN)(t):t,s=e[0].spatialReference,i={...n,query:{...o.query,f:"json",sr:JSON.stringify(s),target:JSON.stringify({geometryType:(0,p.Ji)(e[0]),geometries:e}),cutter:JSON.stringify(r)}},a=await(0,d.default)(o.path+"/cut",i),{cutIndexes:u,geometries:c=[]}=a.data;return{cutIndexes:u,geometries:c.map((t=>{const e=(0,p.im)(t);return e.spatialReference=s,e}))}}(s,q,J,r),n=function(t,e){let r=-1;for(let n=0;n<e.cutIndexes.length;n++){const o=e.cutIndexes[n],s=e.geometries[n],i=(0,c.g)(s);for(let t=0;t<i.length;t++){const e=i[t];e.some((r=>{if(r[0]<180)return!0;{let r=0;for(let t=0;t<e.length;t++){const n=e[t][0];r=n>r?n:r}r=Number(r.toFixed(9));const n=-360*(0,c.o)(r,180);for(let r=0;r<e.length;r++){const e=s.getPoint(t,r);s.setPoint(t,r,e.clone().offset(n,0))}return!0}}))}if(o===r){if("polygon"===t[0].type)for(const e of(0,c.g)(s))t[o]=t[o].addRing(e);else if(b(t))for(const e of(0,c.g)(s))t[o]=t[o].addPath(e)}else r=o,t[o]=s}return t}(q,e),o=[],a=[];for(let e=0;e<I.length;e++){const r=I[e];if("cut"!==r)a.push(r);else{const r=n.shift(),s=t[e];(0,i.i)(s)&&"polygon"===s.type&&s.rings&&s.rings.length>1&&r.rings.length>=s.rings.length?(o.push(r),a.push("simplify")):a.push(F?(0,f.$)(r):r)}}if(!o.length)return a;const u=await y(s,o,r),l=[];for(let t=0;t<a.length;t++){const e=a[t];"simplify"!==e?l.push(e):l.push(F?(0,f.$)(u.shift()):u.shift())}return l}const G=[];for(let t=0;t<I.length;t++){const e=I[t];if("cut"!==e)G.push(e);else{const t=q.shift();G.push(!0===F?(0,f.$)(t):t)}}return Promise.resolve(G)}},15528:(t,e,r)=>{r.d(e,{Z:()=>g});var n=r(29768),o=r(60991),s=r(76506),i=r(50406),a=r(34250),u=(r(91306),r(17533)),c=r(69218),l=r(91171),f=r(2845),d=r(87258),h=r(31292),p=r(46646),y=r(658);r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(57251),r(74673),r(21972),r(23639),r(91055),r(6906),r(86787),r(59465),r(97714),r(97546),r(9801),r(53523),r(42911),r(46826),r(45433),r(54732),r(74569),r(21801),r(73796),r(60947),r(2906),r(91597),r(35132),r(89623),r(84069),r(44567),r(98380),r(92896),r(22781),r(32422),r(59877),r(32101),r(69997),r(53785),r(95587),r(88762),r(82058),r(53426),r(49214),r(95533),r(94751),r(21132),r(38742),r(76733),r(11385),r(85674),r(55823),r(92847),r(34446),r(48190),r(40267),r(39210),r(93314),r(35197),r(90549),r(23761),r(86748),r(15324),r(76996),r(14249),r(60217),r(29107),r(30574),r(2157),r(25977),r(58076),r(98242),r(7471),r(54414),r(1648),r(8925),r(33921),r(3482),r(45154),r(16769),r(55531),r(30582),r(593),r(85699),r(96055),r(47776),r(18033),r(6331),r(62048),r(4292),r(75626),r(72652),r(29641),r(30493),r(70821),r(82673),r(34229),r(37029),r(96467),r(63571),r(30776),r(48027),r(54174),r(82426),r(29794),r(63130),r(25696),r(66396),r(42775),r(95834),r(34394),r(57150),r(76726),r(20444),r(76393),r(78548),r(2497),r(49906),r(46527),r(11799),r(48649),r(98402),r(9960),r(30823),r(53326),r(92482),r(5853),r(39141),r(48243),r(34635),r(10401),r(49900),r(22739),r(20543),r(67477),r(78533),r(74653),r(91091),r(58943),r(70737),r(8487),r(17817),r(90814),r(15459),r(61847),r(16796),r(16955),r(22401),r(77894),r(55187),r(8586),r(44509),r(69814),r(11305),r(62259),r(44790),r(5909),r(60669),r(48208),r(51589),r(27207);let m=class extends y.Z{constructor(t){super(t),this.dynamicDataSource=null,this.fieldsIndex=null,this.format="json",this.gdbVersion=null,this.infoFor3D=null,this.sourceSpatialReference=null}execute(t,e){return this.executeJSON(t,e).then((r=>this.featureSetFromJSON(t,r,e)))}async executeJSON(t,e){var r;const n={...this.requestOptions,...e},o=this._normalizeQuery(t),i=null!=(null==(r=t.outStatistics)?void 0:r[0]),a=(0,s.h)("featurelayer-pbf-statistics"),u=!i||a;let c;if("pbf"===this.format&&u)try{c=await(0,l.c)(this.url,o,n)}catch(t){if("query:parsing-pbf"!==t.name)throw t;this.format="json"}return"json"!==this.format&&u||(c=await(0,f.c)(this.url,o,n)),this._normalizeFields(c.fields),c}async featureSetFromJSON(t,e,n){if(!(this._queryIs3DObjectFormat(t)&&(0,s.i)(this.infoFor3D)&&e.features&&e.features.length))return d.default.fromJSON(e);const{meshFeatureSetFromJSON:o}=await(0,i.Hl)(Promise.all([r.e(1623),r.e(4047),r.e(6087)]).then(r.bind(r,76087)),n);return o(t,this.infoFor3D,e)}executeForCount(t,e){const r={...this.requestOptions,...e},n=this._normalizeQuery(t);return(0,f.b)(this.url,n,r)}executeForExtent(t,e){const r={...this.requestOptions,...e},n=this._normalizeQuery(t);return(0,l.a)(this.url,n,r)}executeForIds(t,e){const r={...this.requestOptions,...e},n=this._normalizeQuery(t);return(0,f.a)(this.url,n,r)}executeRelationshipQuery(t,e){t=p.Z.from(t);const r={...this.requestOptions,...e};return(this.gdbVersion||this.dynamicDataSource)&&((t=t.clone()).gdbVersion=t.gdbVersion||this.gdbVersion,t.dynamicDataSource=t.dynamicDataSource||this.dynamicDataSource),(0,l.d)(this.url,t,r)}executeRelationshipQueryForCount(t,e){t=p.Z.from(t);const r={...this.requestOptions,...e};return(this.gdbVersion||this.dynamicDataSource)&&((t=t.clone()).gdbVersion=t.gdbVersion||this.gdbVersion,t.dynamicDataSource=t.dynamicDataSource||this.dynamicDataSource),(0,l.f)(this.url,t,r)}executeAttachmentQuery(t,e){const r={...this.requestOptions,...e};return(0,l.e)(this.url,t,r)}executeTopFeaturesQuery(t,e){const r={...this.requestOptions,...e};return(0,l.g)(this.parsedUrl,t,this.sourceSpatialReference,r)}executeForTopIds(t,e){const r={...this.requestOptions,...e};return(0,l.h)(this.parsedUrl,t,r)}executeForTopExtents(t,e){const r={...this.requestOptions,...e};return(0,l.i)(this.parsedUrl,t,r)}executeForTopCount(t,e){const r={...this.requestOptions,...e};return(0,l.j)(this.parsedUrl,t,r)}_normalizeQuery(t){let e=h.Z.from(t);if(e.sourceSpatialReference=e.sourceSpatialReference||this.sourceSpatialReference,(this.gdbVersion||this.dynamicDataSource)&&(e=e===t?e.clone():e,e.gdbVersion=t.gdbVersion||this.gdbVersion,e.dynamicDataSource=t.dynamicDataSource?c.D.from(t.dynamicDataSource):this.dynamicDataSource),(0,s.i)(this.infoFor3D)&&this._queryIs3DObjectFormat(t)){e=e===t?e.clone():e,e.formatOf3DObjects=null;for(const t of this.infoFor3D.queryFormats){if("3D_glb"===t.id){e.formatOf3DObjects=t.id;break}"3D_gltf"!==t.id||e.formatOf3DObjects||(e.formatOf3DObjects=t.id)}if(!e.formatOf3DObjects)throw new o.Z("query:unsupported-3d-query-formats","Could not find any supported 3D object query format. Only supported formats are 3D_glb and 3D_gltf");if((0,s.b)(e.outFields)||!e.outFields.includes("*")){e=e===t?e.clone():e,(0,s.b)(e.outFields)&&(e.outFields=[]);const{originX:r,originY:n,originZ:o,translationX:i,translationY:a,translationZ:u,scaleX:c,scaleY:l,scaleZ:f,rotationX:d,rotationY:h,rotationZ:p,rotationDeg:y}=this.infoFor3D.transformFieldRoles;e.outFields.push(r,n,o,i,a,u,c,l,f,d,h,p,y)}}return e}_normalizeFields(t){if((0,s.i)(this.fieldsIndex)&&(0,s.i)(t))for(const e of t){const t=this.fieldsIndex.get(e.name);t&&Object.assign(e,t.toJSON())}}_queryIs3DObjectFormat(t){return(0,s.i)(this.infoFor3D)&&t.returnGeometry&&"xyFootprint"!==t.multipatchOption&&!t.outStatistics}};(0,n._)([(0,a.Cb)({type:c.D})],m.prototype,"dynamicDataSource",void 0),(0,n._)([(0,a.Cb)()],m.prototype,"fieldsIndex",void 0),(0,n._)([(0,a.Cb)()],m.prototype,"format",void 0),(0,n._)([(0,a.Cb)()],m.prototype,"gdbVersion",void 0),(0,n._)([(0,a.Cb)()],m.prototype,"infoFor3D",void 0),(0,n._)([(0,a.Cb)()],m.prototype,"sourceSpatialReference",void 0),m=(0,n._)([(0,u.j)("esri.tasks.QueryTask")],m);const g=m},658:(t,e,r)=>{r.d(e,{Z:()=>c});var n=r(29768),o=r(21972),s=r(32101),i=r(34250),a=(r(76506),r(91306),r(17533));r(23639),r(92143),r(31450),r(71552),r(40642),r(86656),r(22723),r(91055),r(6906),r(50406),r(60991);let u=class extends o.Z{constructor(...t){super(...t),this.requestOptions=null,this.url=null}normalizeCtorArgs(t,e){return"string"!=typeof t?t:{url:t,...e}}get parsedUrl(){return this._parseUrl(this.url)}_parseUrl(t){return t?(0,s.mN)(t):null}_encode(t,e,r){const n={};for(const o in t){if("declaredClass"===o)continue;const s=t[o];if(null!=s&&"function"!=typeof s)if(Array.isArray(s)){n[o]=[];for(let t=0;t<s.length;t++)n[o][t]=this._encode(s[t])}else if("object"==typeof s)if(s.toJSON){const t=s.toJSON(r&&r[o]);n[o]=e?t:JSON.stringify(t)}else n[o]=e?s:JSON.stringify(s);else n[o]=s}return n}};(0,n._)([(0,i.Cb)({readOnly:!0})],u.prototype,"parsedUrl",null),(0,n._)([(0,i.Cb)()],u.prototype,"requestOptions",void 0),(0,n._)([(0,i.Cb)({type:String})],u.prototype,"url",void 0),u=(0,n._)([(0,a.j)("esri.tasks.Task")],u);const c=u}}]);