/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"../../chunks/tslib.es6.js";import t from"../../Color.js";import{h as o}from"../../chunks/handleUtils.js";import{C as r,b as s,o as i}from"../../core/lang.js";import n,{m as p}from"./ElevationProfileLine.js";import{on as a}from"../../core/watchUtils.js";import{property as l}from"../../core/accessorSupport/decorators/property.js";import"../../chunks/ensureType.js";import{subclass as c}from"../../core/accessorSupport/decorators/subclass.js";import{GeometryDescriptor as m}from"../../chunks/ElevationQuery.js";import{d as u}from"../../chunks/elevationInfoUtils.js";import"../../chunks/colorUtils.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/Evented.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/metadata.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Error.js";import"../../chunks/reactiveUtils.js";import"../../chunks/uuid.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../chunks/reader.js";import"../../chunks/writer.js";import"../../geometry/Geometry.js";import"../../chunks/JSONSupport.js";import"../../geometry/SpatialReference.js";import"../../geometry/support/webMercatorUtils.js";import"../../chunks/Ellipsoid.js";import"../../chunks/vec4f64.js";import"../../chunks/unitUtils.js";import"../../chunks/jsonMap.js";import"../../chunks/projectionEllipsoid.js";import"../../chunks/asyncUtils.js";import"../../geometry/Multipoint.js";import"../../geometry/Extent.js";import"../../chunks/zmUtils.js";import"../../geometry/Polyline.js";import"../../chunks/extentUtils.js";import"../../geometry/projection.js";import"../../chunks/mat4.js";import"../../chunks/pe.js";import"../../chunks/assets.js";import"../../request.js";import"../../kernel.js";import"../../core/urlUtils.js";import"../../geometry/Polygon.js";import"../../chunks/aaBoundingRect.js";import"../../chunks/geodesicConstants.js";import"../../geometry/support/GeographicTransformation.js";import"../../geometry/support/GeographicTransformationStep.js";import"../../chunks/zscale.js";import"../../layers/support/ElevationSampler.js";import"../../geometry.js";import"../../chunks/typeUtils.js";import"../../geometry/support/jsonUtils.js";import"../../chunks/unitConversionUtils.js";import"../../chunks/lengthUtils.js";let h=class extends n{constructor(e){super(e),this.type="input",this.color=new t("#00c8c8"),this.viewVisualizationEnabled=!1,this.numSamplesForPreview=50,this.numSamplesPerChunk=500,this.chartFillEnabled=!1,this.chartStrokeOffsetY=-1,this._getQueryElevationDependencies=p(((e,t,o,s,i)=>r(e,(e=>({elevationInfo:e,visibleLayers:t,view:o,stationary:s,spatialReference:i})))))}get queryElevationDependencies(){const e=this._viewModel.view;return s(e)?null:this._getQueryElevationDependencies(this._elevationInfo,this._visibleLayers,e,e.stationary,e.spatialReference)}get available(){return!this._viewModel.inputIsSketched}get _elevationInfo(){return r(this._viewModel.input,u)}get _visibleLayers(){var e;const t=null==(e=this._viewModel)?void 0:e.view,o=r(t,(e=>{var t,o;return null==(t=e.map)||null==(o=t.allLayers)?void 0:o.filter((e=>e.visible)).toArray()}));return i(o,[])}async queryElevation(e,{noDataValue:t,signal:o}){const r=this.queryElevationDependencies;if(s(r))throw new Error("ElevationProfileLineInput: no dependencies");const{view:n,elevationInfo:p,spatialReference:a}=r;if("on-the-ground"===p.mode&&"3d"===n.type){const r=await m.fromGeometry(e).project(a,o),s=n.elevationProvider;return r.coordinates.forEach((e=>{e.z=i(s.getElevation(e.x,e.y,0,a,"ground"),0)})),{geometry:r.export(),noDataValue:t}}return{geometry:e,noDataValue:t}}attach(e){const t=()=>this._onChange();return o([super.attach(e),this.watch("queryElevationDependencies",t),a(e,"view.elevationProvider","elevation-change",t)])}};e([l({type:t,nonNullable:!0})],h.prototype,"color",void 0),e([l()],h.prototype,"viewVisualizationEnabled",void 0),e([l()],h.prototype,"queryElevationDependencies",null),e([l()],h.prototype,"available",null),e([l()],h.prototype,"_elevationInfo",null),e([l()],h.prototype,"_visibleLayers",null),h=e([c("esri.widgets.ElevationProfile.ElevationProfileLineInput")],h);const j=h;export{j as default};
