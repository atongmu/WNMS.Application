/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"../../../chunks/tslib.es6.js";import{property as s}from"../../../core/accessorSupport/decorators/property.js";import"../../../core/lang.js";import"../../../chunks/ensureType.js";import{subclass as o}from"../../../core/accessorSupport/decorators/subclass.js";import t from"./TextInput.js";import"../../../chunks/Logger.js";import"../../../config.js";import"../../../chunks/object.js";import"../../../chunks/string.js";import"../../../chunks/metadata.js";import"../../../chunks/handleUtils.js";import"../../../core/Error.js";import"./Input.js";import"../../../chunks/JSONSupport.js";import"../../../core/Accessor.js";import"../../../chunks/deprecate.js";import"../../../chunks/ArrayPool.js";import"../../../core/scheduling.js";import"../../../chunks/nextTick.js";import"../../../core/promiseUtils.js";var e;let c=e=class extends t{constructor(r){super(r),this.type="barcode-scanner"}clone(){return new e({maxLength:this.maxLength,minLength:this.minLength})}};r([s({type:["barcode-scanner"],json:{read:!1,write:!0}})],c.prototype,"type",void 0),c=e=r([o("esri.form.elements.inputs.BarcodeScannerInput")],c);const p=c;export{p as default};
