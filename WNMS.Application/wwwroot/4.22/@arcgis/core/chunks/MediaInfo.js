/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"./tslib.es6.js";import{a as o}from"./JSONSupport.js";import{property as r}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"./ensureType.js";import{subclass as e}from"../core/accessorSupport/decorators/subclass.js";let p=class extends o{constructor(t){super(t),this.altText=null,this.caption="",this.title="",this.type=null}};t([r({type:String,json:{write:!0}})],p.prototype,"altText",void 0),t([r({type:String,json:{write:!0}})],p.prototype,"caption",void 0),t([r({type:String,json:{write:!0}})],p.prototype,"title",void 0),t([r({type:["image","bar-chart","column-chart","line-chart","pie-chart"],readOnly:!0,json:{read:!1,write:!0}})],p.prototype,"type",void 0),p=t([e("esri.popup.content.mixins.MediaInfo")],p);const s=p;export{s as M};
