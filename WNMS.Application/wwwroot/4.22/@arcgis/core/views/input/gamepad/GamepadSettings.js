/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as o}from"../../../chunks/tslib.es6.js";import s from"../../../core/Accessor.js";import e from"../../../core/Collection.js";import{property as r}from"../../../core/accessorSupport/decorators/property.js";import"../../../core/lang.js";import"../../../chunks/ensureType.js";import{subclass as t}from"../../../core/accessorSupport/decorators/subclass.js";import c from"./GamepadInputDevice.js";import"../../../chunks/deprecate.js";import"../../../chunks/Logger.js";import"../../../config.js";import"../../../chunks/object.js";import"../../../chunks/string.js";import"../../../chunks/metadata.js";import"../../../chunks/handleUtils.js";import"../../../chunks/ArrayPool.js";import"../../../core/scheduling.js";import"../../../chunks/nextTick.js";import"../../../core/promiseUtils.js";import"../../../core/Error.js";import"../../../chunks/Evented.js";import"../../../chunks/shared.js";let p=class extends s{constructor(...o){super(...o),this.devices=new e,this.enabledFocusMode="document"}};o([r({type:e.ofType(c),readOnly:!0})],p.prototype,"devices",void 0),o([r({type:["document","view","none"]})],p.prototype,"enabledFocusMode",void 0),p=o([t("esri.views.input.gamepad.GamepadSettings")],p);const i=p;export{i as default};
