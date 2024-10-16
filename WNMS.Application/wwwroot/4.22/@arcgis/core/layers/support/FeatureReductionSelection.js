/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as s}from"../../chunks/tslib.es6.js";import{property as r}from"../../core/accessorSupport/decorators/property.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import{subclass as o}from"../../core/accessorSupport/decorators/subclass.js";import{a as t}from"../../chunks/JSONSupport.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../core/Error.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";let e=class extends t{constructor(){super(...arguments),this.type=null}};s([r({type:["selection","cluster"],readOnly:!0,json:{read:!1,write:!0}})],e.prototype,"type",void 0),e=s([o("esri.layers.support.FeatureReduction")],e);const c=e;var p;let i=p=class extends c{constructor(s){super(s),this.type="selection"}clone(){return new p}};s([r({type:["selection"]})],i.prototype,"type",void 0),i=p=s([o("esri.layers.support.FeatureReductionSelection")],i);const n=i;export{c as F,n as default};
