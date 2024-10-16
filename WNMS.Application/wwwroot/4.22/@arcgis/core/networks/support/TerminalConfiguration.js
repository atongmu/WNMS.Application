/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"../../chunks/tslib.es6.js";import{J as o}from"../../chunks/jsonMap.js";import{a as t}from"../../chunks/JSONSupport.js";import{property as i}from"../../core/accessorSupport/decorators/property.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import{subclass as e}from"../../core/accessorSupport/decorators/subclass.js";import s from"./Terminal.js";import"../../chunks/object.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/string.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Error.js";const n=new o({esriUNTMBidirectional:"bidirectional",esriUNTMDirectional:"directional"});let a=class extends t{constructor(r){super(r),this.defaultConfiguration=null,this.id=null,this.name=null,this.terminals=[],this.traversabilityModel=null}};r([i({type:String,json:{write:!0}})],a.prototype,"defaultConfiguration",void 0),r([i({type:Number,json:{read:{source:"terminalConfigurationId"},write:{target:"terminalConfigurationId"}}})],a.prototype,"id",void 0),r([i({type:String,json:{read:{source:"terminalConfigurationName"},write:{target:"terminalConfigurationName"}}})],a.prototype,"name",void 0),r([i({type:[s],json:{write:!0}})],a.prototype,"terminals",void 0),r([i({type:n.apiValues,json:{type:n.jsonValues,read:n.read,write:n.write}})],a.prototype,"traversabilityModel",void 0),a=r([e("esri.networks.support.TerminalConfiguration")],a);const p=a;export{p as default};
