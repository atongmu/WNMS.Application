/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"../../chunks/tslib.es6.js";import{a as s}from"../../chunks/JSONSupport.js";import{property as o}from"../../core/accessorSupport/decorators/property.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import{subclass as t}from"../../core/accessorSupport/decorators/subclass.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Error.js";let e=class extends s{constructor(r){super(r),this.format=null,this.itemId=null,this.url=null}};r([o()],e.prototype,"format",void 0),r([o({json:{read:{source:"itemID"},write:{target:"itemID"}}})],e.prototype,"itemId",void 0),r([o()],e.prototype,"url",void 0),e=r([t("esri/rest/support/RasterData")],e);const p=e;export{p as default};
