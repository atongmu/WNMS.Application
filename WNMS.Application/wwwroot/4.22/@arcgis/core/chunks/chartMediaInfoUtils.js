/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"./tslib.es6.js";import{property as t}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"./ensureType.js";import{subclass as o}from"../core/accessorSupport/decorators/subclass.js";import{M as e}from"./MediaInfo.js";import s from"../popup/content/support/ChartMediaInfoValue.js";import{s as a}from"./jsonMap.js";let p=class extends e{constructor(r){super(r),this.type=null,this.value=null}};r([t({type:["bar-chart","column-chart","line-chart","pie-chart"],readOnly:!0,json:{read:!1,write:!0}})],p.prototype,"type",void 0),r([t({type:s,json:{write:!0}})],p.prototype,"value",void 0),p=r([o("esri.popup.content.mixins.ChartMediaInfo")],p);const c=p,i=a()({barchart:"bar-chart",columnchart:"column-chart",linechart:"line-chart",piechart:"pie-chart"});export{c as C,i as c};
