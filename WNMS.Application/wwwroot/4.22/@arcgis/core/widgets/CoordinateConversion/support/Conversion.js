/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as o}from"../../../chunks/tslib.es6.js";import r from"../../../core/Accessor.js";import{property as s}from"../../../core/accessorSupport/decorators/property.js";import"../../../core/lang.js";import"../../../chunks/ensureType.js";import{subclass as t}from"../../../core/accessorSupport/decorators/subclass.js";import"../../../chunks/deprecate.js";import"../../../chunks/Logger.js";import"../../../config.js";import"../../../chunks/object.js";import"../../../chunks/string.js";import"../../../chunks/metadata.js";import"../../../chunks/handleUtils.js";import"../../../chunks/ArrayPool.js";import"../../../core/scheduling.js";import"../../../chunks/nextTick.js";import"../../../core/promiseUtils.js";import"../../../core/Error.js";let e=class extends r{constructor(o){super(o),this.format=null,this.position={coordinate:null,location:null}}get displayCoordinate(){const o=this.get("format");return o&&o.getDisplayCoordinate(this.get("position.coordinate"))}};o([s({readOnly:!0})],e.prototype,"displayCoordinate",null),o([s()],e.prototype,"format",void 0),o([s()],e.prototype,"position",void 0),e=o([t("esri.widgets.CoordinateConversion.support.Conversion")],e);const i=e;export{i as default};
