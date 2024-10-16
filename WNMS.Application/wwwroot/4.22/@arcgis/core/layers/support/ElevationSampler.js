/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import"../../geometry.js";import{b as e,o as t}from"../../core/lang.js";import{L as s}from"../../chunks/Logger.js";import{d as o}from"../../chunks/unitUtils.js";import{t as r,a as i}from"../../chunks/aaBoundingRect.js";import{e as n}from"../../geometry/Extent.js";import{project as a}from"../../geometry/support/webMercatorUtils.js";import l from"../../geometry/Point.js";import"../../chunks/ensureType.js";import"../../geometry/Geometry.js";import"../../chunks/tslib.es6.js";import"../../chunks/JSONSupport.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../core/accessorSupport/decorators/property.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/ArrayPool.js";import"../../core/accessorSupport/decorators/subclass.js";import"../../core/Error.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../chunks/reader.js";import"../../geometry/SpatialReference.js";import"../../chunks/writer.js";import"../../geometry/Multipoint.js";import"../../chunks/zmUtils.js";import"../../chunks/Ellipsoid.js";import"../../core/accessorSupport/decorators/cast.js";import"../../geometry/Polygon.js";import"../../chunks/extentUtils.js";import"../../geometry/Polyline.js";import"../../chunks/typeUtils.js";import"../../chunks/jsonMap.js";import"../../geometry/support/jsonUtils.js";import"../../chunks/projectionEllipsoid.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";const p=s.getLogger("esri.layers.support.ElevationSampler");class c{queryElevation(e){return h(e.clone(),this)}on(){return d}projectIfRequired(e,t){return f(e,t)}}class m extends c{constructor(e,t,s){super(),this.tile=e,this.noDataValue=s,this.extent=r(e.tile.extent,t.spatialReference);const i=o(t.spatialReference),n=t.lodAt(e.tile.level).resolution*i;this.demResolution={min:n,max:n}}get spatialReference(){return this.extent.spatialReference}contains(e){const t=this.projectIfRequired(e,this.spatialReference);return n(this.extent,t)}elevationAt(t){const s=this.projectIfRequired(t,this.spatialReference);if(e(s))return null;if(!this.contains(t)){const e=this.extent,s=`${e.xmin}, ${e.ymin}, ${e.xmax}, ${e.ymax}`;return p.warn("#elevationAt()",`Point used to sample elevation (${t.x}, ${t.y}) is outside of the sampler extent (${s})`),this.noDataValue}return this.tile.sample(s.x,s.y)}}class u extends c{constructor(e,t,s){let o;super(),"number"==typeof t?(this.noDataValue=t,o=null):(o=t,this.noDataValue=s),this.samplers=o?e.map((e=>new m(e,o,this.noDataValue))):e;const n=this.samplers[0];if(n){this.extent=n.extent.clone();const{min:e,max:t}=n.demResolution;this.demResolution={min:e,max:t};for(let e=1;e<this.samplers.length;e++){const t=this.samplers[e];this.extent.union(t.extent),this.demResolution.min=Math.min(this.demResolution.min,t.demResolution.min),this.demResolution.max=Math.max(this.demResolution.max,t.demResolution.max)}}else this.extent=r(i(),o.spatialReference),this.demResolution={min:0,max:0}}get spatialReference(){return this.extent.spatialReference}elevationAt(e){const t=this.projectIfRequired(e,this.spatialReference);if(!t)return null;for(const e of this.samplers)if(e.contains(t))return e.elevationAt(t);return p.warn("#elevationAt()",`Point used to sample elevation (${e.x}, ${e.y}) is outside of the sampler`),this.noDataValue}}function h(e,s){const o=f(e,s.spatialReference);if(!o)return null;switch(e.type){case"point":!function(e,s,o){e.z=t(o.elevationAt(s),0)}(e,o,s);break;case"polyline":!function(e,s,o){j.spatialReference=s.spatialReference;const r=e.hasM&&!e.hasZ;for(let i=0;i<e.paths.length;i++){const n=e.paths[i],a=s.paths[i];for(let e=0;e<n.length;e++){const s=n[e],i=a[e];j.x=i[0],j.y=i[1],r&&(s[3]=s[2]),s[2]=t(o.elevationAt(j),0)}}e.hasZ=!0}(e,o,s);break;case"multipoint":!function(e,s,o){j.spatialReference=s.spatialReference;const r=e.hasM&&!e.hasZ;for(let i=0;i<e.points.length;i++){const n=e.points[i],a=s.points[i];j.x=a[0],j.y=a[1],r&&(n[3]=n[2]),n[2]=t(o.elevationAt(j),0)}e.hasZ=!0}(e,o,s)}return e}function f(t,s){if(e(t))return null;const o=t.spatialReference;if(o.equals(s))return t;const r=a(t,s);return r||p.error(`Cannot project geometry spatial reference (wkid:${o.wkid}) to elevation sampler spatial reference (wkid:${s.wkid})`),r}const j=new l,d={remove(){}};export{c as ElevationSamplerBase,u as MultiTileElevationSampler,m as TileElevationSampler,h as updateGeometryElevation};
