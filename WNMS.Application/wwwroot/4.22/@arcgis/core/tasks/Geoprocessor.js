/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"../chunks/tslib.es6.js";import{property as o}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"../chunks/ensureType.js";import{subclass as s}from"../core/accessorSupport/decorators/subclass.js";import r from"../geometry/Extent.js";import e from"../geometry/SpatialReference.js";import{e as p,s as i}from"../chunks/submitJob.js";import m from"../rest/geoprocessor/GPOptions.js";import n from"../rest/support/JobInfo.js";import l from"./Task.js";import"../chunks/Logger.js";import"../config.js";import"../chunks/object.js";import"../chunks/string.js";import"../chunks/metadata.js";import"../chunks/handleUtils.js";import"../core/Error.js";import"../geometry/Geometry.js";import"../chunks/JSONSupport.js";import"../core/Accessor.js";import"../chunks/deprecate.js";import"../chunks/ArrayPool.js";import"../core/scheduling.js";import"../chunks/nextTick.js";import"../core/promiseUtils.js";import"../chunks/reader.js";import"../chunks/writer.js";import"../geometry/Point.js";import"../core/accessorSupport/decorators/cast.js";import"../geometry/support/webMercatorUtils.js";import"../chunks/Ellipsoid.js";import"../rest/support/GPMessage.js";import"../chunks/jsonMap.js";import"../request.js";import"../kernel.js";import"../core/urlUtils.js";import"../chunks/utils3.js";import"../chunks/scaleUtils.js";import"../chunks/unitUtils.js";import"../chunks/projectionEllipsoid.js";import"../chunks/floorFilterUtils.js";import"../geometry/support/normalizeUtils.js";import"../geometry/Polygon.js";import"../chunks/extentUtils.js";import"../chunks/zmUtils.js";import"../geometry/Polyline.js";import"../chunks/normalizeUtilsCommon.js";import"../geometry/support/jsonUtils.js";import"../geometry/Multipoint.js";import"../geometry.js";import"../chunks/typeUtils.js";import"../layers/support/Field.js";import"../chunks/enumeration.js";import"../chunks/domains.js";import"../layers/support/CodedValueDomain.js";import"../layers/support/Domain.js";import"../layers/support/InheritedDomain.js";import"../layers/support/RangeDomain.js";import"../chunks/fieldType.js";import"../layers/support/MapImage.js";import"../rest/support/DataFile.js";import"../rest/support/FeatureSet.js";import"../Graphic.js";import"../PopupTemplate.js";import"../core/Collection.js";import"../chunks/Evented.js";import"../chunks/shared.js";import"../layers/support/fieldUtils.js";import"../chunks/arcadeOnDemand.js";import"../popup/content.js";import"../popup/content/AttachmentsContent.js";import"../popup/content/Content.js";import"../popup/content/CustomContent.js";import"../popup/content/ExpressionContent.js";import"../popup/ElementExpressionInfo.js";import"../popup/content/FieldsContent.js";import"../popup/FieldInfo.js";import"../popup/support/FieldInfoFormat.js";import"../chunks/date.js";import"../chunks/number.js";import"../chunks/locale.js";import"../popup/content/MediaContent.js";import"../popup/content/BarChartMediaInfo.js";import"../chunks/chartMediaInfoUtils.js";import"../chunks/MediaInfo.js";import"../popup/content/support/ChartMediaInfoValue.js";import"../popup/content/support/ChartMediaInfoValueSeries.js";import"../popup/content/ColumnChartMediaInfo.js";import"../popup/content/ImageMediaInfo.js";import"../popup/content/support/ImageMediaInfoValue.js";import"../popup/content/LineChartMediaInfo.js";import"../popup/content/PieChartMediaInfo.js";import"../popup/content/TextContent.js";import"../popup/ExpressionInfo.js";import"../popup/LayerOptions.js";import"../popup/RelatedRecordsInfo.js";import"../popup/support/RelatedRecordsInfoFieldOrder.js";import"../support/actions/ActionBase.js";import"../chunks/Identifiable.js";import"../support/actions/ActionButton.js";import"../support/actions/ActionToggle.js";import"../symbols.js";import"../symbols/CIMSymbol.js";import"../symbols/Symbol.js";import"../Color.js";import"../chunks/colorUtils.js";import"../chunks/mathUtils.js";import"../chunks/common.js";import"../symbols/ExtrudeSymbol3DLayer.js";import"../symbols/Symbol3DLayer.js";import"../chunks/utils.js";import"../symbols/edges/Edges3D.js";import"../chunks/screenUtils.js";import"../chunks/materialUtils.js";import"../chunks/opacityUtils.js";import"../symbols/edges/SketchEdges3D.js";import"../symbols/edges/SolidEdges3D.js";import"../chunks/Symbol3DMaterial.js";import"../symbols/FillSymbol.js";import"../symbols/SimpleLineSymbol.js";import"../symbols/LineSymbol.js";import"../symbols/LineSymbolMarker.js";import"../symbols/FillSymbol3DLayer.js";import"../symbols/patterns/LineStylePattern3D.js";import"../symbols/patterns/StylePattern3D.js";import"../chunks/utils2.js";import"../chunks/colors.js";import"../chunks/symbolLayerUtils3D.js";import"../chunks/aaBoundingBox.js";import"../chunks/aaBoundingRect.js";import"../symbols/Font.js";import"../symbols/IconSymbol3DLayer.js";import"../chunks/persistableUrlUtils.js";import"../symbols/LabelSymbol3D.js";import"../symbols/Symbol3D.js";import"../chunks/collectionUtils.js";import"../portal/Portal.js";import"../chunks/Loadable.js";import"../chunks/Promise.js";import"../portal/PortalQueryParams.js";import"../portal/PortalQueryResult.js";import"../portal/PortalUser.js";import"../portal/PortalFolder.js";import"../portal/PortalGroup.js";import"../symbols/LineSymbol3DLayer.js";import"../symbols/ObjectSymbol3DLayer.js";import"../symbols/PathSymbol3DLayer.js";import"../symbols/TextSymbol3DLayer.js";import"../symbols/WaterSymbol3DLayer.js";import"../chunks/Thumbnail.js";import"../chunks/Symbol3DVerticalOffset.js";import"../symbols/callouts/Callout3D.js";import"../symbols/callouts/LineCallout3D.js";import"../symbols/LineSymbol3D.js";import"../symbols/MarkerSymbol.js";import"../symbols/MeshSymbol3D.js";import"../symbols/PictureFillSymbol.js";import"../chunks/urlUtils.js";import"../symbols/PictureMarkerSymbol.js";import"../symbols/PointSymbol3D.js";import"../symbols/PolygonSymbol3D.js";import"../symbols/SimpleFillSymbol.js";import"../symbols/SimpleMarkerSymbol.js";import"../symbols/TextSymbol.js";import"../symbols/WebStyleSymbol.js";import"../rest/support/LinearUnit.js";import"../rest/support/ParameterValue.js";import"../rest/support/RasterData.js";let u=class extends l{constructor(t){super(t),this._jobs=new Map,this.outSpatialReference=null,this.processExtent=null,this.processSpatialReference=null,this.returnFeatureCollection=!1,this.returnM=!1,this.returnZ=!1}destroy(){this._jobs.forEach((t=>t.destroy())),this._jobs.clear()}cancelJob(t,o){const s=this._getOrAddJob(t),r={...this.requestOptions,...o};return s.cancelJob(r)}checkJobStatus(t,o){const s=this._getOrAddJob(t),r={...this.requestOptions,...o};return s.checkJobStatus(r)}execute(t,o){const s=new m({outSpatialReference:this.outSpatialReference,processExtent:this.processExtent,processSpatialReference:this.processSpatialReference,returnFeatureCollection:this.returnFeatureCollection,returnM:this.returnM,returnZ:this.returnZ}),r={...this.requestOptions,...o};return p(this.url,t,s,r)}getResultData(t,o,s){const r=this._getOrAddJob(t),{returnFeatureCollection:e,returnM:p,returnZ:i,outSpatialReference:n}=this,l=new m({returnFeatureCollection:e,returnM:p,returnZ:i,outSpatialReference:n,url:this.url}),u={...this.requestOptions,...s};return r.fetchResultData(o,l,u)}getResultImage(t,o,s,r){const e=this._getOrAddJob(t),p={...this.requestOptions,...r};return e.fetchResultImage(o,s,p)}async getResultMapImageLayer(t){return this._getOrAddJob(t).fetchResultMapImageLayer()}submitJob(t,o){const s=new m({outSpatialReference:this.outSpatialReference,processExtent:this.processExtent,processSpatialReference:this.processSpatialReference,returnFeatureCollection:this.returnFeatureCollection,returnM:this.returnM,returnZ:this.returnZ}),r={...this.requestOptions,...o};return i(this.url,t,s,r).then((t=>(t.sourceUrl=this.url,this._jobs.set(t.jobId,t),t)))}waitForJobCompletion(t,o={}){return this._getOrAddJob(t).waitForJobCompletion(o)}_getOrAddJob(t){let o=this._jobs.get(t);return o||(o=new n({sourceUrl:this.url,jobId:t}),this._jobs.set(o.jobId,o)),o}};t([o({type:e})],u.prototype,"outSpatialReference",void 0),t([o({type:r})],u.prototype,"processExtent",void 0),t([o({type:e})],u.prototype,"processSpatialReference",void 0),t([o({nonNullable:!0})],u.prototype,"returnFeatureCollection",void 0),t([o({nonNullable:!0})],u.prototype,"returnM",void 0),t([o({nonNullable:!0})],u.prototype,"returnZ",void 0),u=t([s("esri/tasks/Geoprocessor")],u);const a=u;export{a as default};
