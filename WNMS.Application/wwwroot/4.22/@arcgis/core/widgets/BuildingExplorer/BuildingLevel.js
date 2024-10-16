/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as s}from"../../chunks/tslib.es6.js";import{i as e}from"../../core/lang.js";import{property as t}from"../../core/accessorSupport/decorators/property.js";import"../../chunks/ensureType.js";import{subclass as r}from"../../core/accessorSupport/decorators/subclass.js";import{B as i,e as o,h as l,i as n}from"../../chunks/filterUtils.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../core/Error.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Collection.js";import"../../chunks/Evented.js";import"../../chunks/shared.js";import"../../chunks/collectionUtils.js";import"../../core/Handles.js";import"../../chunks/uuid.js";import"../../chunks/BuildingFilterBlock.js";import"../../chunks/JSONSupport.js";import"../../chunks/enumeration.js";import"../../chunks/jsonMap.js";import"../../chunks/utils.js";import"../../symbols/edges/Edges3D.js";import"../../Color.js";import"../../chunks/colorUtils.js";import"../../chunks/mathUtils.js";import"../../chunks/common.js";import"../../chunks/screenUtils.js";import"../../chunks/materialUtils.js";import"../../chunks/opacityUtils.js";import"../../symbols/edges/SketchEdges3D.js";import"../../symbols/edges/SolidEdges3D.js";let p=class extends i{constructor(){super(...arguments),this._levelFieldName=null,this._parseValueFromFilter=s=>{const t=new RegExp(`${this._levelFieldName}\\s*=\\s*(\\d+)`,"gi"),r=new RegExp(`${this._levelFieldName}\\s*<\\s*(\\d+)`,"gi");for(const{filterMode:i,filterExpression:o}of s.filterBlocks.items){let s=null;if("solid"===i.type?s=t.exec(o):"x-ray"===i.type&&(s=r.exec(o)),e(s))return parseInt(s[1],10)}return null}}get filterExpressions(){return this.enabled&&this._levelFieldName?{xRay:`${this._levelFieldName} < ${this.value}`,solid:`${this._levelFieldName} = ${this.value}`}:{solid:null,xRay:null}}get _firstValue(){const s=this.allowedValues;return s.length>0?s[0]:0}_setup(){const s=[];this.layers.forEach((t=>{const r=o(t,"bldglevel");e(r)&&(this._levelFieldName=r.fieldName,s.push(r))})),this._domainInfo=l(s);const t=n(this.layers,this._parseValueFromFilter);e(t)?this.select(t):(this.enabled=!1,this.value=this._firstValue)}};s([t({readOnly:!0})],p.prototype,"filterExpressions",null),s([t()],p.prototype,"_levelFieldName",void 0),s([t({readOnly:!0})],p.prototype,"_firstValue",null),p=s([r("esri.widgets.BuildingExplorer.BuildingLevel")],p);const c=p;export{c as default};
