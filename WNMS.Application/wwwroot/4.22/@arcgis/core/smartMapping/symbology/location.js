/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import o from"../../Color.js";import{c as t,g as r,a as e}from"../../chunks/symbologyUtils.js";import{t as i}from"../../chunks/utils12.js";import"../../chunks/colorUtils.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/utils7.js";import"../../chunks/arcadeOnDemand.js";import"../../geometry.js";import"../../geometry/Extent.js";import"../../chunks/tslib.es6.js";import"../../core/accessorSupport/decorators/property.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../core/accessorSupport/decorators/subclass.js";import"../../core/Error.js";import"../../geometry/Geometry.js";import"../../chunks/JSONSupport.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../chunks/reader.js";import"../../geometry/SpatialReference.js";import"../../chunks/writer.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../geometry/support/webMercatorUtils.js";import"../../chunks/Ellipsoid.js";import"../../geometry/Multipoint.js";import"../../chunks/zmUtils.js";import"../../geometry/Polygon.js";import"../../chunks/extentUtils.js";import"../../geometry/Polyline.js";import"../../chunks/typeUtils.js";import"../../chunks/jsonMap.js";import"../../geometry/support/jsonUtils.js";import"../../chunks/basemapUtils.js";import"../../Basemap.js";import"../../core/Collection.js";import"../../chunks/Evented.js";import"../../chunks/shared.js";import"../../chunks/collectionUtils.js";import"../../chunks/Loadable.js";import"../../chunks/Promise.js";import"../../chunks/loadAll.js";import"../../chunks/asyncUtils.js";import"../../core/urlUtils.js";import"../../portal/Portal.js";import"../../kernel.js";import"../../request.js";import"../../chunks/locale.js";import"../../portal/PortalQueryParams.js";import"../../portal/PortalQueryResult.js";import"../../portal/PortalUser.js";import"../../portal/PortalFolder.js";import"../../portal/PortalGroup.js";import"../../portal/PortalItem.js";import"../../chunks/assets.js";import"../../portal/PortalItemResource.js";import"../../portal/PortalRating.js";import"../../chunks/messages.js";import"../../chunks/writeUtils.js";import"../../chunks/screenUtils.js";const s=t({themeDictionary:{default:{name:"default",label:"Default",description:"Default theme for basic visualization of features.",schemes:{point:{light:{primary:{color:[77,77,77,1],outline:{color:[255,255,255,.25],width:"1px"},size:"8px"},secondary:[{color:[226,119,40,1],outline:{color:[255,255,255,.25],width:"1px"},size:"8px"},{color:[255,255,255,1],outline:{color:[51,51,51,.25],width:"1px"},size:"8px"}]},dark:{primary:{color:[255,255,255,1],outline:{color:[92,92,92,.25],width:"1px"},size:"8px"},secondary:[{color:[226,119,40,1],outline:{color:[255,255,255,.25],width:"1px"},size:"8px"},{color:[26,26,26,1],outline:{color:[178,178,178,.25],width:"1px"},size:"8px"}]}},polyline:{light:{primary:{color:[77,77,77,1],width:"2px"},secondary:[{color:[226,119,40,1],width:"2px"},{color:[255,255,255,1],width:"2px"}]},dark:{primary:{color:[255,255,255,1],width:"2px"},secondary:[{color:[226,119,40,1],width:"2px"},{color:[26,26,26,1],width:"2px"}]}},polygon:{light:{primary:{size:"12px",color:[227,139,79,1],outline:{color:[255,255,255,.25],width:"1px"},opacity:.8},secondary:[{size:"12px",color:[128,128,128,1],outline:{color:[255,255,255,.25],width:"1px"},opacity:.8},{size:"12px",color:[255,255,255,1],outline:{color:[128,128,128,.25],width:"1px"},opacity:.8}]},dark:{primary:{size:"12px",color:[227,139,79,1],outline:{color:[92,92,92,.25],width:"1px"},opacity:.8},secondary:[{size:"12px",color:[178,178,178,1],outline:{color:[92,92,92,.25],width:"1px"},opacity:.8},{size:"12px",color:[26,26,26,1],outline:{color:[128,128,128,.25],width:"1px"},opacity:.8}]}}}}}});function c(o){return r(s,o)}function p(o){const t=e({basemap:o.basemap,geometryType:o.geometryType,basemapTheme:o.basemapTheme,theme:s.get("default")});if(!t)return;const{schemesInfo:r,basemapId:i,basemapTheme:c}=t;return{primaryScheme:n(o,r.primary),secondarySchemes:r.secondary.map((t=>n(o,t))).filter(Boolean),basemapId:i,basemapTheme:c}}function l(t){if(!t)return;const r={...t};return r.color&&(r.color=new o(r.color)),"outline"in r&&r.outline&&(r.outline={color:r.outline.color&&new o(r.outline.color),width:r.outline.width}),r}function n(t,r){const e="mesh"!==t.geometryType&&t.worldScale?t.view:null;switch(t.geometryType){case"point":case"multipoint":{const t=r;return function(t,r){return{color:new o(t.color),outline:{color:new o(t.outline.color),width:t.outline.width},size:r?i(t.size,r):t.size,opacity:1}}({color:t.color,outline:{...t.outline},size:t.size},e)}case"polyline":{const t=r;return function(t,r){return{color:new o(t.color),width:r?i(t.width,r):t.width,opacity:1}}({color:t.color,width:t.width},e)}case"polygon":{const t=r;return function(t,r){return{color:new o(t.color),outline:{color:new o(t.outline.color),width:t.outline.width},size:r?i(t.size,r):t.size,opacity:t.opacity}}({size:t.size,color:t.color,outline:{...t.outline},opacity:t.opacity},e)}case"mesh":{const t=r;return function(t){return{color:new o(t.color),opacity:t.opacity}}({color:t.color,opacity:t.opacity})}}}export{l as cloneScheme,p as getSchemes,c as getThemes};
