/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"./chunks/tslib.es6.js";import r from"./Camera.js";import{geometryTypes as o}from"./geometry.js";import{a as e}from"./chunks/JSONSupport.js";import{i as s}from"./core/lang.js";import{property as i}from"./core/accessorSupport/decorators/property.js";import{cast as m}from"./core/accessorSupport/decorators/cast.js";import{subclass as p}from"./core/accessorSupport/decorators/subclass.js";import{fromJSON as c}from"./geometry/support/jsonUtils.js";import"./chunks/mathUtils.js";import"./chunks/common.js";import"./chunks/reader.js";import"./chunks/writer.js";import"./chunks/ensureType.js";import"./chunks/Logger.js";import"./config.js";import"./chunks/object.js";import"./chunks/string.js";import"./geometry/Point.js";import"./geometry/Geometry.js";import"./geometry/SpatialReference.js";import"./core/Accessor.js";import"./chunks/deprecate.js";import"./chunks/metadata.js";import"./chunks/handleUtils.js";import"./chunks/ArrayPool.js";import"./core/scheduling.js";import"./chunks/nextTick.js";import"./core/promiseUtils.js";import"./core/Error.js";import"./geometry/support/webMercatorUtils.js";import"./chunks/Ellipsoid.js";import"./chunks/mathUtils2.js";import"./geometry/Extent.js";import"./geometry/Multipoint.js";import"./chunks/zmUtils.js";import"./geometry/Polygon.js";import"./chunks/extentUtils.js";import"./geometry/Polyline.js";import"./chunks/typeUtils.js";import"./chunks/jsonMap.js";var n;let a=n=class extends e{constructor(t){super(t),this.rotation=0,this.scale=0,this.targetGeometry=null,this.camera=null}castRotation(t){return(t%=360)<0&&(t+=360),t}clone(){return new n({rotation:this.rotation,scale:this.scale,targetGeometry:s(this.targetGeometry)?this.targetGeometry.clone():null,camera:s(this.camera)?this.camera.clone():null})}};function j(){return{enabled:!this.camera}}t([i({type:Number,json:{write:!0,origins:{"web-map":{default:0,write:!0},"web-scene":{write:{overridePolicy:j}}}}})],a.prototype,"rotation",void 0),t([m("rotation")],a.prototype,"castRotation",null),t([i({type:Number,json:{write:!0,origins:{"web-map":{default:0,write:!0},"web-scene":{write:{overridePolicy:j}}}}})],a.prototype,"scale",void 0),t([i({types:o,json:{read:c,write:!0,origins:{"web-scene":{read:c,write:{overridePolicy:j}}}}})],a.prototype,"targetGeometry",void 0),t([i({type:r,json:{write:!0}})],a.prototype,"camera",void 0),a=n=t([p("esri.Viewpoint")],a);const l=a;export{l as default};
