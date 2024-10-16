/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as o}from"../../chunks/tslib.es6.js";import r from"../../Color.js";import{h as e}from"../../chunks/handleUtils.js";import{b as t,C as s,o as i}from"../../core/lang.js";import n,{m as p}from"./ElevationProfileLine.js";import{b as u}from"../../chunks/unitUtils.js";import{property as c}from"../../core/accessorSupport/decorators/property.js";import"../../chunks/ensureType.js";import{subclass as a}from"../../core/accessorSupport/decorators/subclass.js";import{g as l}from"../../chunks/elevationQuerySourceUtils.js";import"../../chunks/colorUtils.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/Evented.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/metadata.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Error.js";import"../../chunks/reactiveUtils.js";import"../../chunks/uuid.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../chunks/reader.js";import"../../chunks/writer.js";import"../../geometry/Geometry.js";import"../../chunks/JSONSupport.js";import"../../geometry/SpatialReference.js";import"../../geometry/support/webMercatorUtils.js";import"../../chunks/Ellipsoid.js";import"../../chunks/vec4f64.js";import"../../chunks/jsonMap.js";import"../../chunks/projectionEllipsoid.js";import"../../Ground.js";import"../../core/Collection.js";import"../../chunks/shared.js";import"../../chunks/collectionUtils.js";import"../../chunks/compilerUtils.js";import"../../chunks/Loadable.js";import"../../chunks/Promise.js";import"../../chunks/loadAll.js";import"../../chunks/asyncUtils.js";import"../../chunks/enumeration.js";import"../../chunks/opacityUtils.js";let m=class extends n{constructor(o){super(o),this.type="ground",this.color=new r("#ff7f00"),this.viewVisualizationEnabled=!0,this.numSamplesForPreview=50,this.numSamplesPerChunk=1e3,this._getQueryElevationDependencies=p(((o,r)=>({ground:o,groundLayers:r})))}get available(){const o=this._ground;return!t(o)&&o.layers.some((o=>o.visible))}get minDemResolution(){return l(this._ground)}get queryElevationDependencies(){return this._getQueryElevationDependencies(this._ground,this._groundLayers)}get _ground(){var o;return s(null==(o=this._viewModel)?void 0:o.view,(o=>{var r;return null==(r=o.map)?void 0:r.ground}))}get _groundLayers(){const o=this._ground,r=s(o,(o=>{var r;return null==(r=o.layers)?void 0:r.toArray()}));return i(r,[])}async queryElevation(o,r){const e=this.queryElevationDependencies;if(t(e))throw new Error("ElevationProfileLineGround: no dependencies");const{ground:s}=e;if(t(s))throw new Error("No ground configured in the view");const i=await s.queryElevation(o,r),n=u(o.spatialReference),p=u(s.layers.getItemAt(0).spatialReference);if(n!==p){const o=i.geometry;o.points=o.points.map((([o,e,t])=>[o,e,t===r.noDataValue?t:t*p/n]))}return i}attach(o){return e([super.attach(o),this.watch("queryElevationDependencies",(()=>this._onChange()))])}};o([c({type:r,nonNullable:!0})],m.prototype,"color",void 0),o([c()],m.prototype,"viewVisualizationEnabled",void 0),o([c()],m.prototype,"available",null),o([c({readOnly:!0})],m.prototype,"minDemResolution",null),o([c()],m.prototype,"queryElevationDependencies",null),o([c()],m.prototype,"_ground",null),o([c()],m.prototype,"_groundLayers",null),m=o([a("esri.widgets.ElevationProfile.ElevationProfileLineGround")],m);const h=m;export{h as default};
