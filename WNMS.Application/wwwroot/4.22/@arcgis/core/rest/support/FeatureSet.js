/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"../../chunks/tslib.es6.js";import{geometryTypes as o}from"../../geometry.js";import e from"../../Graphic.js";import{J as s}from"../../chunks/jsonMap.js";import{a as r}from"../../chunks/JSONSupport.js";import{i,u as p}from"../../core/lang.js";import{property as n}from"../../core/accessorSupport/decorators/property.js";import"../../chunks/ensureType.js";import{r as m}from"../../chunks/reader.js";import{subclass as l}from"../../core/accessorSupport/decorators/subclass.js";import{w as a}from"../../chunks/writer.js";import u from"../../geometry/SpatialReference.js";import{fromJSON as y,isPolygon as c}from"../../geometry/support/jsonUtils.js";import j from"../../layers/support/Field.js";import"../../geometry/Extent.js";import"../../chunks/string.js";import"../../chunks/object.js";import"../../geometry/Geometry.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Error.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../geometry/support/webMercatorUtils.js";import"../../chunks/Ellipsoid.js";import"../../geometry/Multipoint.js";import"../../chunks/zmUtils.js";import"../../geometry/Polygon.js";import"../../chunks/extentUtils.js";import"../../geometry/Polyline.js";import"../../chunks/typeUtils.js";import"../../PopupTemplate.js";import"../../core/Collection.js";import"../../chunks/Evented.js";import"../../chunks/shared.js";import"../../layers/support/fieldUtils.js";import"../../chunks/arcadeOnDemand.js";import"../../popup/content.js";import"../../popup/content/AttachmentsContent.js";import"../../popup/content/Content.js";import"../../popup/content/CustomContent.js";import"../../popup/content/ExpressionContent.js";import"../../popup/ElementExpressionInfo.js";import"../../popup/content/FieldsContent.js";import"../../popup/FieldInfo.js";import"../../chunks/enumeration.js";import"../../popup/support/FieldInfoFormat.js";import"../../chunks/date.js";import"../../chunks/number.js";import"../../chunks/locale.js";import"../../popup/content/MediaContent.js";import"../../popup/content/BarChartMediaInfo.js";import"../../chunks/chartMediaInfoUtils.js";import"../../chunks/MediaInfo.js";import"../../popup/content/support/ChartMediaInfoValue.js";import"../../popup/content/support/ChartMediaInfoValueSeries.js";import"../../popup/content/ColumnChartMediaInfo.js";import"../../popup/content/ImageMediaInfo.js";import"../../popup/content/support/ImageMediaInfoValue.js";import"../../popup/content/LineChartMediaInfo.js";import"../../popup/content/PieChartMediaInfo.js";import"../../popup/content/TextContent.js";import"../../popup/ExpressionInfo.js";import"../../popup/LayerOptions.js";import"../../popup/RelatedRecordsInfo.js";import"../../popup/support/RelatedRecordsInfoFieldOrder.js";import"../../support/actions/ActionBase.js";import"../../chunks/Identifiable.js";import"../../support/actions/ActionButton.js";import"../../support/actions/ActionToggle.js";import"../../symbols.js";import"../../symbols/CIMSymbol.js";import"../../symbols/Symbol.js";import"../../Color.js";import"../../chunks/colorUtils.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";import"../../symbols/ExtrudeSymbol3DLayer.js";import"../../symbols/Symbol3DLayer.js";import"../../chunks/utils.js";import"../../symbols/edges/Edges3D.js";import"../../chunks/screenUtils.js";import"../../chunks/materialUtils.js";import"../../chunks/opacityUtils.js";import"../../symbols/edges/SketchEdges3D.js";import"../../symbols/edges/SolidEdges3D.js";import"../../chunks/Symbol3DMaterial.js";import"../../symbols/FillSymbol.js";import"../../symbols/SimpleLineSymbol.js";import"../../symbols/LineSymbol.js";import"../../symbols/LineSymbolMarker.js";import"../../symbols/FillSymbol3DLayer.js";import"../../symbols/patterns/LineStylePattern3D.js";import"../../symbols/patterns/StylePattern3D.js";import"../../chunks/utils2.js";import"../../chunks/colors.js";import"../../chunks/symbolLayerUtils3D.js";import"../../chunks/aaBoundingBox.js";import"../../chunks/aaBoundingRect.js";import"../../symbols/Font.js";import"../../symbols/IconSymbol3DLayer.js";import"../../core/urlUtils.js";import"../../chunks/persistableUrlUtils.js";import"../../symbols/LabelSymbol3D.js";import"../../symbols/Symbol3D.js";import"../../chunks/collectionUtils.js";import"../../portal/Portal.js";import"../../kernel.js";import"../../request.js";import"../../chunks/Loadable.js";import"../../chunks/Promise.js";import"../../portal/PortalQueryParams.js";import"../../portal/PortalQueryResult.js";import"../../portal/PortalUser.js";import"../../portal/PortalFolder.js";import"../../portal/PortalGroup.js";import"../../symbols/LineSymbol3DLayer.js";import"../../symbols/ObjectSymbol3DLayer.js";import"../../symbols/PathSymbol3DLayer.js";import"../../symbols/TextSymbol3DLayer.js";import"../../symbols/WaterSymbol3DLayer.js";import"../../chunks/Thumbnail.js";import"../../chunks/Symbol3DVerticalOffset.js";import"../../symbols/callouts/Callout3D.js";import"../../symbols/callouts/LineCallout3D.js";import"../../symbols/LineSymbol3D.js";import"../../symbols/MarkerSymbol.js";import"../../symbols/MeshSymbol3D.js";import"../../symbols/PictureFillSymbol.js";import"../../chunks/urlUtils.js";import"../../symbols/PictureMarkerSymbol.js";import"../../symbols/PointSymbol3D.js";import"../../symbols/PolygonSymbol3D.js";import"../../symbols/SimpleFillSymbol.js";import"../../symbols/SimpleMarkerSymbol.js";import"../../symbols/TextSymbol.js";import"../../symbols/WebStyleSymbol.js";import"../../chunks/domains.js";import"../../layers/support/CodedValueDomain.js";import"../../layers/support/Domain.js";import"../../layers/support/InheritedDomain.js";import"../../layers/support/RangeDomain.js";import"../../chunks/fieldType.js";const h=new s({esriGeometryPoint:"point",esriGeometryMultipoint:"multipoint",esriGeometryPolyline:"polyline",esriGeometryPolygon:"polygon",esriGeometryEnvelope:"extent",mesh:"mesh","":null});let f=class extends r{constructor(t){super(t),this.displayFieldName=null,this.exceededTransferLimit=!1,this.features=[],this.fields=null,this.geometryType=null,this.hasM=!1,this.hasZ=!1,this.queryGeometry=null,this.spatialReference=null}readFeatures(t,o){const s=u.fromJSON(o.spatialReference),r=[];for(let o=0;o<t.length;o++){const p=t[o],n=e.fromJSON(p),m=p.geometry&&p.geometry.spatialReference;i(n.geometry)&&!m&&(n.geometry.spatialReference=s),r.push(n)}return r}writeGeometryType(t,o,e,s){if(t)return void h.write(t,o,e,s);const{features:r}=this;if(r)for(const t of r)if(t&&i(t.geometry))return void h.write(t.geometry.type,o,e,s)}readQueryGeometry(t,o){if(!t)return null;const e=!!t.spatialReference,s=y(t);return!e&&o.spatialReference&&(s.spatialReference=u.fromJSON(o.spatialReference)),s}writeSpatialReference(t,o){if(t)return void(o.spatialReference=t.toJSON());const{features:e}=this;if(e)for(const t of e)if(t&&i(t.geometry)&&t.geometry.spatialReference)return void(o.spatialReference=t.geometry.spatialReference.toJSON())}toJSON(t){const o=this.write();if(o.features&&Array.isArray(t)&&t.length>0)for(let e=0;e<o.features.length;e++){const s=o.features[e];if(s.geometry){const o=t&&t[e];s.geometry=o&&o.toJSON()||s.geometry}}return o}quantize(t){const{scale:[o,e],translate:[s,r]}=t,i=this.features,n=this._getQuantizationFunction(this.geometryType,(t=>Math.round((t-s)/o)),(t=>Math.round((r-t)/e)));for(let t=0,o=i.length;t<o;t++)n(p(i[t].geometry))||(i.splice(t,1),t--,o--);return this.transform=t,this}unquantize(){const{geometryType:t,features:o,transform:e}=this;if(!e)return this;const{translate:[s,r],scale:[p,n]}=e,m=this._getHydrationFunction(t,(t=>t*p+s),(t=>r-t*n));for(const{geometry:t}of o)i(t)&&m(t);return this.transform=null,this}_quantizePoints(t,o,e){let s,r;const i=[];for(let p=0,n=t.length;p<n;p++){const n=t[p];if(p>0){const t=o(n[0]),p=e(n[1]);t===s&&p===r||(i.push([t-s,p-r]),s=t,r=p)}else s=o(n[0]),r=e(n[1]),i.push([s,r])}return i.length>0?i:null}_getQuantizationFunction(t,o,e){return"point"===t?t=>(t.x=o(t.x),t.y=e(t.y),t):"polyline"===t||"polygon"===t?t=>{const s=c(t)?t.rings:t.paths,r=[];for(let t=0,i=s.length;t<i;t++){const i=s[t],p=this._quantizePoints(i,o,e);p&&r.push(p)}return r.length>0?(c(t)?t.rings=r:t.paths=r,t):null}:"multipoint"===t?t=>{const s=this._quantizePoints(t.points,o,e);return s.length>0?(t.points=s,t):null}:"extent"===t?t=>t:null}_getHydrationFunction(t,o,e){return"point"===t?t=>{t.x=o(t.x),t.y=e(t.y)}:"polyline"===t||"polygon"===t?t=>{const s=c(t)?t.rings:t.paths;let r,i;for(let t=0,p=s.length;t<p;t++){const p=s[t];for(let t=0,s=p.length;t<s;t++){const s=p[t];t>0?(r+=s[0],i+=s[1]):(r=s[0],i=s[1]),s[0]=o(r),s[1]=e(i)}}}:"extent"===t?t=>{t.xmin=o(t.xmin),t.ymin=e(t.ymin),t.xmax=o(t.xmax),t.ymax=e(t.ymax)}:"multipoint"===t?t=>{const s=t.points;let r,i;for(let t=0,p=s.length;t<p;t++){const p=s[t];t>0?(r+=p[0],i+=p[1]):(r=p[0],i=p[1]),p[0]=o(r),p[1]=e(i)}}:void 0}};t([n({type:String,json:{write:!0}})],f.prototype,"displayFieldName",void 0),t([n({type:Boolean,json:{write:{overridePolicy:t=>({enabled:t})}}})],f.prototype,"exceededTransferLimit",void 0),t([n({type:[e],json:{write:!0}})],f.prototype,"features",void 0),t([m("features")],f.prototype,"readFeatures",null),t([n({type:[j],json:{write:!0}})],f.prototype,"fields",void 0),t([n({type:["point","multipoint","polyline","polygon","extent","mesh"],json:{read:{reader:h.read}}})],f.prototype,"geometryType",void 0),t([a("geometryType")],f.prototype,"writeGeometryType",null),t([n({type:Boolean,json:{write:{overridePolicy:t=>({enabled:t})}}})],f.prototype,"hasM",void 0),t([n({type:Boolean,json:{write:{overridePolicy:t=>({enabled:t})}}})],f.prototype,"hasZ",void 0),t([n({types:o,json:{write:!0}})],f.prototype,"queryGeometry",void 0),t([m("queryGeometry")],f.prototype,"readQueryGeometry",null),t([n({type:u,json:{write:!0}})],f.prototype,"spatialReference",void 0),t([a("spatialReference")],f.prototype,"writeSpatialReference",null),t([n({json:{write:!0}})],f.prototype,"transform",void 0),f=t([l("esri.rest.support.FeatureSet")],f),f.prototype.toJSON.isDefaultToJSON=!0;const d=f;export{d as default};
