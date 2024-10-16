/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"./chunks/tslib.es6.js";import{a as e}from"./chunks/JSONSupport.js";import{i as r,l as s,m as i}from"./core/lang.js";import{o as n,t as o}from"./chunks/timeUtils.js";import{property as l}from"./core/accessorSupport/decorators/property.js";import"./chunks/ensureType.js";import{r as a}from"./chunks/reader.js";import{subclass as m}from"./core/accessorSupport/decorators/subclass.js";import{w as u}from"./chunks/writer.js";import"./core/Accessor.js";import"./chunks/deprecate.js";import"./chunks/Logger.js";import"./config.js";import"./chunks/object.js";import"./chunks/string.js";import"./chunks/metadata.js";import"./chunks/handleUtils.js";import"./chunks/ArrayPool.js";import"./core/scheduling.js";import"./chunks/nextTick.js";import"./core/promiseUtils.js";import"./core/Error.js";var p;let c=p=class extends e{constructor(t){super(t),this.end=null,this.start=null}static get allTime(){return d}static get empty(){return h}readEnd(t,e){return null!=e.end?new Date(e.end):null}writeEnd(t,e){e.end=t?t.getTime():null}get isAllTime(){return this.equals(p.allTime)}get isEmpty(){return this.equals(p.empty)}readStart(t,e){return null!=e.start?new Date(e.start):null}writeStart(t,e){e.start=t?t.getTime():null}clone(){return new p({end:this.end,start:this.start})}equals(t){if(!t)return!1;const e=r(this.start)?this.start.getTime():this.start,s=r(this.end)?this.end.getTime():this.end,i=r(t.start)?t.start.getTime():t.start,n=r(t.end)?t.end.getTime():t.end;return e===i&&s===n}expandTo(t){if(this.isEmpty||this.isAllTime)return this.clone();const e=s(this.start,(e=>o(e,t))),r=s(this.end,(e=>n(o(e,t),1,t)));return new p({start:e,end:r})}intersection(t){if(!t)return this.clone();if(this.isEmpty||t.isEmpty)return p.empty;if(this.isAllTime)return t.clone();if(t.isAllTime)return this.clone();const e=i(this.start,-1/0,(t=>t.getTime())),r=i(this.end,1/0,(t=>t.getTime())),s=i(t.start,-1/0,(t=>t.getTime())),n=i(t.end,1/0,(t=>t.getTime()));let o,l;if(s>=e&&s<=r?o=s:e>=s&&e<=n&&(o=e),r>=s&&r<=n?l=r:n>=e&&n<=r&&(l=n),!isNaN(o)&&!isNaN(l)){const t=new p;return t.start=o===-1/0?null:new Date(o),t.end=l===1/0?null:new Date(l),t}return p.empty}offset(t,e){if(this.isEmpty||this.isAllTime)return this.clone();const s=new p,{start:i,end:o}=this;return r(i)&&(s.start=n(i,t,e)),r(o)&&(s.end=n(o,t,e)),s}union(t){if(!t||t.isEmpty)return this.clone();if(this.isEmpty)return t.clone();if(this.isAllTime||t.isAllTime)return d.clone();const e=r(this.start)&&r(t.start)?new Date(Math.min(this.start.getTime(),t.start.getTime())):null,s=r(this.end)&&r(t.end)?new Date(Math.max(this.end.getTime(),t.end.getTime())):null;return new p({start:e,end:s})}};t([l({type:Date,json:{write:{allowNull:!0}}})],c.prototype,"end",void 0),t([a("end")],c.prototype,"readEnd",null),t([u("end")],c.prototype,"writeEnd",null),t([l({readOnly:!0,json:{read:!1}})],c.prototype,"isAllTime",null),t([l({readOnly:!0,json:{read:!1}})],c.prototype,"isEmpty",null),t([l({type:Date,json:{write:{allowNull:!0}}})],c.prototype,"start",void 0),t([a("start")],c.prototype,"readStart",null),t([u("start")],c.prototype,"writeStart",null),c=p=t([m("esri.TimeExtent")],c);const d=new c,h=new c({start:void 0,end:void 0}),T=c;export{T as default};
