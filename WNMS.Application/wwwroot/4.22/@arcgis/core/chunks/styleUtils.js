/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import e from"../request.js";import r from"../core/Error.js";import{throwIfAbortError as t}from"../core/promiseUtils.js";import{normalize as s,removeFile as o}from"../core/urlUtils.js";import n from"../portal/Portal.js";import a from"../portal/PortalQueryParams.js";import{h as l}from"../core/lang.js";const f=()=>!!l("enable-feature:disable-context-navigation"),i=()=>!!l("enable-feature:direct-3d-object-feature-layer-display"),u={};function y(e,t,s){const o=t&&t.portal||n.getDefault();let l;const f=`${o.url} - ${o.user&&o.user.username} - ${e}`;return u[f]||(u[f]=function(e,t,s){return t.load(s).then((()=>{const r=new a({disableExtraQuery:!0,query:`owner:${b} AND type:${h} AND typekeywords:"${e}"`});return t.queryItems(r,s)})).then((({results:t})=>{let o=null;const n=e.toLowerCase();if(t&&Array.isArray(t))for(const e of t){if(e.typeKeywords.some((e=>e.toLowerCase()===n))&&e.type===h&&e.owner===b){o=e;break}}if(!o)throw new r("symbolstyleutils:style-not-found",`The style '${e}' could not be found`,{styleName:e});return o.load(s)}))}(e,o,s).then((e=>(l=e,e.fetchData()))).then((r=>({data:r,baseUrl:l.itemUrl,styleName:e})))),u[f]}function c(e,s,n){return e.styleUrl?async function(e,r){try{return{data:(await d(e,r)).data,baseUrl:o(e),styleUrl:e}}catch(e){return t(e),null}}(e.styleUrl,n):e.styleName?y(e.styleName,s,n):Promise.reject(new r("symbolstyleutils:style-url-and-name-missing","Either styleUrl or styleName is required to resolve a style"))}function m(e){return null===e||"CIMSymbolReference"===e.type?e:{type:"CIMSymbolReference",symbol:e}}function p(e,r){if("cimRef"===r)return e.cimRef;if(e.formatInfos&&!l("enable-feature:force-wosr"))for(const r of e.formatInfos)if("gltf"===r.type)return r.href;return e.webRef}function d(r,t){const o={responseType:"json",query:{f:"json"},...t};return e(s(r),o)}const b="esri_en",h="Style",j="https://cdn.arcgis.com/sharing/rest/content/items/220936cc6ed342c9937abd8f180e7d1e/resources/styles/cim/{SymbolName}.json?f=json";export{j as S,f as d,i as e,c as f,m,d as r,p as s};
