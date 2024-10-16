/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"./tslib.es6.js";import{property as e}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"./ensureType.js";import{subclass as s}from"../core/accessorSupport/decorators/subclass.js";const o=o=>{let t=class extends o{constructor(){super(...arguments),this.customParameters=null}};return r([e({type:Object,json:{write:{overridePolicy:r=>({enabled:!!(r&&Object.keys(r).length>0)})}}})],t.prototype,"customParameters",void 0),t=r([s("esri.layers.mixins.CustomParametersMixin")],t),t};export{o as C};
