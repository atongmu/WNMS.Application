/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as o}from"../../../../chunks/tslib.es6.js";import s from"../../../../core/Accessor.js";import{property as r}from"../../../../core/accessorSupport/decorators/property.js";import"../../../../core/lang.js";import{cast as t}from"../../../../core/accessorSupport/decorators/cast.js";import{subclass as e}from"../../../../core/accessorSupport/decorators/subclass.js";import p from"./ButtonMenuItem.js";import"../../../../chunks/deprecate.js";import"../../../../chunks/Logger.js";import"../../../../config.js";import"../../../../chunks/object.js";import"../../../../chunks/string.js";import"../../../../chunks/metadata.js";import"../../../../chunks/handleUtils.js";import"../../../../chunks/ArrayPool.js";import"../../../../core/scheduling.js";import"../../../../chunks/nextTick.js";import"../../../../core/promiseUtils.js";import"../../../../core/Error.js";import"../../../../chunks/ensureType.js";let c=class extends s{constructor(o){super(o),this.items=null,this.open=!1}castItems(o){return o?o.map((o=>o instanceof p?o:new p(o))):null}};o([r()],c.prototype,"items",void 0),o([t("items")],c.prototype,"castItems",null),o([r()],c.prototype,"open",void 0),c=o([e("esri.widgets.FeatureTable.Grid.support.ButtonMenuViewModel")],c);const i=c;export{i as default};
