/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"../../chunks/tslib.es6.js";import o from"../../request.js";import"../../core/lang.js";import{J as s}from"../../chunks/jsonMap.js";import{a as e}from"../../chunks/JSONSupport.js";import{onAbort as r,createAbortError as i}from"../../core/promiseUtils.js";import{property as a}from"../../core/accessorSupport/decorators/property.js";import"../../chunks/ensureType.js";import{subclass as p}from"../../core/accessorSupport/decorators/subclass.js";import{e as n,p as m}from"../../chunks/utils3.js";import l from"../geoprocessor/GPOptions.js";import{normalizeCentralMeridian as u}from"../../geometry/support/normalizeUtils.js";import c from"../../layers/support/Field.js";import j from"../../layers/support/MapImage.js";import b from"./DataFile.js";import y from"./FeatureSet.js";import h from"./LinearUnit.js";import d from"./ParameterValue.js";import S from"./RasterData.js";import f from"./GPMessage.js";import"../../config.js";import"../../chunks/object.js";import"../../kernel.js";import"../../core/urlUtils.js";import"../../core/Error.js";import"../../chunks/Logger.js";import"../../chunks/string.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../chunks/scaleUtils.js";import"../../chunks/unitUtils.js";import"../../chunks/projectionEllipsoid.js";import"../../geometry/SpatialReference.js";import"../../chunks/writer.js";import"../../chunks/Ellipsoid.js";import"../../chunks/floorFilterUtils.js";import"../../geometry/Extent.js";import"../../geometry/Geometry.js";import"../../chunks/reader.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../geometry/support/webMercatorUtils.js";import"../../geometry/Polygon.js";import"../../chunks/extentUtils.js";import"../../chunks/zmUtils.js";import"../../geometry/Polyline.js";import"../../chunks/normalizeUtilsCommon.js";import"../../geometry/support/jsonUtils.js";import"../../geometry/Multipoint.js";import"../../geometry.js";import"../../chunks/typeUtils.js";import"../../chunks/enumeration.js";import"../../chunks/domains.js";import"../../layers/support/CodedValueDomain.js";import"../../layers/support/Domain.js";import"../../layers/support/InheritedDomain.js";import"../../layers/support/RangeDomain.js";import"../../chunks/fieldType.js";import"../../Graphic.js";import"../../PopupTemplate.js";import"../../core/Collection.js";import"../../chunks/Evented.js";import"../../chunks/shared.js";import"../../layers/support/fieldUtils.js";import"../../chunks/arcadeOnDemand.js";import"../../popup/content.js";import"../../popup/content/AttachmentsContent.js";import"../../popup/content/Content.js";import"../../popup/content/CustomContent.js";import"../../popup/content/ExpressionContent.js";import"../../popup/ElementExpressionInfo.js";import"../../popup/content/FieldsContent.js";import"../../popup/FieldInfo.js";import"../../popup/support/FieldInfoFormat.js";import"../../chunks/date.js";import"../../chunks/number.js";import"../../chunks/locale.js";import"../../popup/content/MediaContent.js";import"../../popup/content/BarChartMediaInfo.js";import"../../chunks/chartMediaInfoUtils.js";import"../../chunks/MediaInfo.js";import"../../popup/content/support/ChartMediaInfoValue.js";import"../../popup/content/support/ChartMediaInfoValueSeries.js";import"../../popup/content/ColumnChartMediaInfo.js";import"../../popup/content/ImageMediaInfo.js";import"../../popup/content/support/ImageMediaInfoValue.js";import"../../popup/content/LineChartMediaInfo.js";import"../../popup/content/PieChartMediaInfo.js";import"../../popup/content/TextContent.js";import"../../popup/ExpressionInfo.js";import"../../popup/LayerOptions.js";import"../../popup/RelatedRecordsInfo.js";import"../../popup/support/RelatedRecordsInfoFieldOrder.js";import"../../support/actions/ActionBase.js";import"../../chunks/Identifiable.js";import"../../support/actions/ActionButton.js";import"../../support/actions/ActionToggle.js";import"../../symbols.js";import"../../symbols/CIMSymbol.js";import"../../symbols/Symbol.js";import"../../Color.js";import"../../chunks/colorUtils.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";import"../../symbols/ExtrudeSymbol3DLayer.js";import"../../symbols/Symbol3DLayer.js";import"../../chunks/utils.js";import"../../symbols/edges/Edges3D.js";import"../../chunks/screenUtils.js";import"../../chunks/materialUtils.js";import"../../chunks/opacityUtils.js";import"../../symbols/edges/SketchEdges3D.js";import"../../symbols/edges/SolidEdges3D.js";import"../../chunks/Symbol3DMaterial.js";import"../../symbols/FillSymbol.js";import"../../symbols/SimpleLineSymbol.js";import"../../symbols/LineSymbol.js";import"../../symbols/LineSymbolMarker.js";import"../../symbols/FillSymbol3DLayer.js";import"../../symbols/patterns/LineStylePattern3D.js";import"../../symbols/patterns/StylePattern3D.js";import"../../chunks/utils2.js";import"../../chunks/colors.js";import"../../chunks/symbolLayerUtils3D.js";import"../../chunks/aaBoundingBox.js";import"../../chunks/aaBoundingRect.js";import"../../symbols/Font.js";import"../../symbols/IconSymbol3DLayer.js";import"../../chunks/persistableUrlUtils.js";import"../../symbols/LabelSymbol3D.js";import"../../symbols/Symbol3D.js";import"../../chunks/collectionUtils.js";import"../../portal/Portal.js";import"../../chunks/Loadable.js";import"../../chunks/Promise.js";import"../../portal/PortalQueryParams.js";import"../../portal/PortalQueryResult.js";import"../../portal/PortalUser.js";import"../../portal/PortalFolder.js";import"../../portal/PortalGroup.js";import"../../symbols/LineSymbol3DLayer.js";import"../../symbols/ObjectSymbol3DLayer.js";import"../../symbols/PathSymbol3DLayer.js";import"../../symbols/TextSymbol3DLayer.js";import"../../symbols/WaterSymbol3DLayer.js";import"../../chunks/Thumbnail.js";import"../../chunks/Symbol3DVerticalOffset.js";import"../../symbols/callouts/Callout3D.js";import"../../symbols/callouts/LineCallout3D.js";import"../../symbols/LineSymbol3D.js";import"../../symbols/MarkerSymbol.js";import"../../symbols/MeshSymbol3D.js";import"../../symbols/PictureFillSymbol.js";import"../../chunks/urlUtils.js";import"../../symbols/PictureMarkerSymbol.js";import"../../symbols/PointSymbol3D.js";import"../../symbols/PolygonSymbol3D.js";import"../../symbols/SimpleFillSymbol.js";import"../../symbols/SimpleMarkerSymbol.js";import"../../symbols/TextSymbol.js";import"../../symbols/WebStyleSymbol.js";async function k(t,s,e,r,i){const a={},p={},n=[];return function(t,o,s){for(const e in t){const r=t[e];if(r&&"object"==typeof r&&r instanceof y){const{features:t}=r;s[e]=[o.length,o.length+t.length],t.forEach((t=>{o.push(t.geometry)}))}}}(r,n,a),u(n).then((n=>{const{outSpatialReference:l,processExtent:u,processSpatialReference:c,returnFeatureCollection:j,returnM:b,returnZ:y}=e,{path:h}=m(t);for(const t in a){const o=a[t];p[t]=n.slice(o[0],o[1])}const d=l?l.wkid||l:null,S=c?c.wkid||c:null,f="execute"===s?{returnFeatureCollection:j||void 0,returnM:b||void 0,returnZ:y||void 0}:null,k=P({...u?{context:{extent:u,outSR:d,processSR:S}}:{"env:outSR":d,"env:processSR":S},...r,...f,f:"json"},null,p),g={...i,query:k};return o(`${h}/${s}`,g)}))}function g(t){const o=t.dataType,s=d.fromJSON(t);switch(o){case"GPBoolean":case"GPDouble":case"GPLong":case"GPString":case"GPMultiValue:GPBoolean":case"GPMultiValue:GPDouble":case"GPMultiValue:GPLong":case"GPMultiValue:GPString":return s;case"GPDate":s.value=new Date(s.value);break;case"GPDataFile":s.value=b.fromJSON(s.value);break;case"GPLinearUnit":s.value=h.fromJSON(s.value);break;case"GPFeatureRecordSetLayer":case"GPRecordSet":{const o=t.value.url;s.value=o?b.fromJSON(s.value):y.fromJSON(s.value);break}case"GPRasterData":case"GPRasterDataLayer":{const o=t.value.mapImage;s.value=o?j.fromJSON(o):S.fromJSON(s.value);break}case"GPField":s.value=c.fromJSON(s.value);break;case"GPMultiValue:GPDate":{const t=s.value;s.value=t.map((t=>new Date(t)));break}case"GPMultiValue:GPDataFile":s.value=s.value.map((t=>b.fromJSON(t)));break;case"GPMultiValue:GPLinearUnit":s.value=s.value.map((t=>h.fromJSON(t)));break;case"GPMultiValue:GPFeatureRecordSetLayer":case"GPMultiValue:GPRecordSet":s.value=s.value.map((t=>y.fromJSON(t)));break;case"GPMultiValue:GPRasterData":case"GPMultiValue:GPRasterDataLayer":s.value=s.value.map((t=>t?j.fromJSON(t):S.fromJSON(s.value)));break;case"GPMultiValue:GPField":s.value=s.value.map((t=>c.fromJSON(t)))}return s}function P(t,o,s){for(const o in t){const s=t[o];Array.isArray(s)?t[o]=JSON.stringify(s.map((t=>P({item:t},!0).item))):s instanceof Date&&(t[o]=s.getTime())}return n(t,o,s)}var D;const v=new s({esriJobCancelled:"job-cancelled",esriJobCancelling:"job-cancelling",esriJobDeleted:"job-deleted",esriJobDeleting:"job-deleting",esriJobTimedOut:"job-timed-out",esriJobExecuting:"job-executing",esriJobFailed:"job-failed",esriJobNew:"job-new",esriJobSubmitted:"job-submitted",esriJobSucceeded:"job-succeeded",esriJobWaiting:"job-waiting"});let G=D=class extends e{constructor(t){super(t),this.jobId=null,this.jobStatus=null,this.messages=null,this.requestOptions=null,this.sourceUrl=null,this._timer=null}cancelJob(t){const{jobId:s,sourceUrl:e}=this,{path:r}=m(e),i={...this.requestOptions,...t,query:{f:"json"}};this._clearTimer();return o(`${r}/jobs/${s}/cancel`,i).then((t=>{const o=D.fromJSON(t.data);return this.messages=o.messages,this.jobStatus=o.jobStatus,this}))}destroy(){clearInterval(this._timer)}checkJobStatus(t){const{path:s}=m(this.sourceUrl),e={...this.requestOptions,...t,query:{f:"json"}},r=`${s}/jobs/${this.jobId}`;return o(r,e).then((({data:t})=>{const o=D.fromJSON(t);return this.messages=o.messages,this.jobStatus=o.jobStatus,this}))}fetchResultData(t,s,e){s=l.from(s||{});const{returnFeatureCollection:r,returnM:i,returnZ:a,outSpatialReference:p}=s,{path:n}=m(this.sourceUrl),u=P({returnFeatureCollection:r,returnM:i,returnZ:a,outSR:p,returnType:"data",f:"json"},null),c={...this.requestOptions,...e,query:u},j=`${n}/jobs/${this.jobId}/results/${t}`;return o(j,c).then((t=>g(t.data)))}fetchResultImage(t,s,e){const{path:r}=m(this.sourceUrl),i=P({...s.toJSON(),f:"json"}),a={...this.requestOptions,...e,query:i},p=`${r}/jobs/${this.jobId}/results/${t}`;return o(p,a).then((t=>g(t.data)))}async fetchResultMapImageLayer(){const{path:t}=m(this.sourceUrl),o=t.indexOf("/GPServer/"),s=`${t.substring(0,o)}/MapServer/jobs/${this.jobId}`;return new(0,(await import("../../layers/MapImageLayer.js")).default)({url:s})}async waitForJobCompletion(t={}){const{interval:o=1e3,signal:s,statusCallback:e}=t;return new Promise(((t,a)=>{r(s,(()=>{this._clearTimer(),a(i())})),this._clearTimer();const p=setInterval((()=>{this._timer||a(i()),this.checkJobStatus(this.requestOptions).then((o=>{const{jobStatus:s}=o;switch(this.jobStatus=s,s){case"job-succeeded":this._clearTimer(),t(this);break;case"job-submitted":case"job-executing":case"job-waiting":case"job-new":e&&e(this);break;case"job-cancelled":case"job-cancelling":case"job-deleted":case"job-deleting":case"job-timed-out":case"job-failed":this._clearTimer(),a(this)}}))}),o);this._timer=p}))}_clearTimer(){this._timer&&(clearInterval(this._timer),this._timer=null)}};t([a()],G.prototype,"jobId",void 0),t([a({json:{read:v.read}})],G.prototype,"jobStatus",void 0),t([a({type:[f]})],G.prototype,"messages",void 0),t([a()],G.prototype,"requestOptions",void 0),t([a({json:{write:!0}})],G.prototype,"sourceUrl",void 0),G=D=t([p("esri.rest.support.JobInfo")],G);const M=G;export{k as c,g as d,M as default};
