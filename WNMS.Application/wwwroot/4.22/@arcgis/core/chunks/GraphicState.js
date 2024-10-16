/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"./tslib.es6.js";import{E as s}from"./Evented.js";import{property as o}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"./ensureType.js";import{subclass as t}from"../core/accessorSupport/decorators/subclass.js";let e=class extends s.EventedAccessor{constructor(r){super(r),this.tracking=!1,this.displaying=!1,this.isDraped=!1}};r([o({constructOnly:!0})],e.prototype,"graphic",void 0),r([o()],e.prototype,"tracking",void 0),r([o()],e.prototype,"displaying",void 0),r([o()],e.prototype,"isDraped",void 0),e=r([t("esri.views.3d.layers.graphics.GraphicState")],e);export{e as G};
