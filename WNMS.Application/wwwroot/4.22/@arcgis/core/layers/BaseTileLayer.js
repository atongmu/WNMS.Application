/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"../chunks/tslib.es6.js";import t from"../request.js";import s from"../core/Error.js";import{property as r}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"../chunks/ensureType.js";import{subclass as o}from"../core/accessorSupport/decorators/subclass.js";import i from"../geometry/Extent.js";import p from"../geometry/SpatialReference.js";import{a as n}from"../chunks/aaBoundingRect.js";import m from"./Layer.js";import{B as c}from"../chunks/BlendLayer.js";import{R as l}from"../chunks/RefreshableLayer.js";import{S as a}from"../chunks/ScaleRangeLayer.js";import j from"./support/TileInfo.js";import"../config.js";import"../chunks/object.js";import"../kernel.js";import"../core/urlUtils.js";import"../chunks/Logger.js";import"../chunks/string.js";import"../core/promiseUtils.js";import"../chunks/metadata.js";import"../chunks/handleUtils.js";import"../geometry/Geometry.js";import"../chunks/JSONSupport.js";import"../core/Accessor.js";import"../chunks/deprecate.js";import"../chunks/ArrayPool.js";import"../core/scheduling.js";import"../chunks/nextTick.js";import"../chunks/reader.js";import"../chunks/writer.js";import"../geometry/Point.js";import"../core/accessorSupport/decorators/cast.js";import"../geometry/support/webMercatorUtils.js";import"../chunks/Ellipsoid.js";import"../chunks/mathUtils.js";import"../chunks/common.js";import"../geometry.js";import"../geometry/Multipoint.js";import"../chunks/zmUtils.js";import"../geometry/Polygon.js";import"../chunks/extentUtils.js";import"../geometry/Polyline.js";import"../chunks/typeUtils.js";import"../chunks/jsonMap.js";import"../geometry/support/jsonUtils.js";import"../chunks/Evented.js";import"../chunks/Identifiable.js";import"../chunks/Loadable.js";import"../chunks/Promise.js";import"../chunks/jsonUtils.js";import"../chunks/parser.js";import"../chunks/colorUtils.js";import"../chunks/screenUtils.js";import"../chunks/mat4.js";import"../chunks/_commonjsHelpers.js";import"../core/Collection.js";import"../chunks/shared.js";import"../chunks/unitUtils.js";import"../chunks/projectionEllipsoid.js";import"./support/LOD.js";const u={id:"0/0/0",level:0,row:0,col:0,extent:null};let h=class extends(c(a(l(m)))){constructor(){super(...arguments),this.tileInfo=j.create({spatialReference:p.WebMercator,size:256}),this.type="base-tile",this.fullExtent=new i(-20037508.342787,-20037508.34278,20037508.34278,20037508.342787,p.WebMercator),this.spatialReference=p.WebMercator}getTileBounds(e,t,s,r){const o=r||n();return u.level=e,u.row=t,u.col=s,u.extent=o,this.tileInfo.updateTileInfo(u),u.extent=null,o}fetchTile(e,s,r,o={}){const{signal:i}=o,p=this.getTileUrl(e,s,r),n={responseType:"image",signal:i,query:{...this.refreshParameters}};return t(p,n).then((e=>e.data))}getTileUrl(){throw new s("basetilelayer:gettileurl-not-implemented","getTileUrl() is not implemented")}};e([r({type:j})],h.prototype,"tileInfo",void 0),e([r({type:["show","hide"]})],h.prototype,"listMode",void 0),e([r({readOnly:!0,value:"base-tile"})],h.prototype,"type",void 0),e([r({nonNullable:!0})],h.prototype,"fullExtent",void 0),e([r()],h.prototype,"spatialReference",void 0),h=e([o("esri.layers.BaseTileLayer")],h);const k=h;export{k as default};
