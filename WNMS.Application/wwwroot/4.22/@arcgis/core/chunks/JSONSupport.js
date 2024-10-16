/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as r}from"./tslib.es6.js";import e,{n as t,i as o}from"../core/Accessor.js";import{v as n,e as s}from"../core/accessorSupport/decorators/property.js";import{g as i,m as a}from"./metadata.js";import{o as u,a as c,b as f,subclass as l}from"../core/accessorSupport/decorators/subclass.js";import{e as d}from"../core/lang.js";import p from"../core/Error.js";import{L as g}from"./Logger.js";function m(r,e,t){if(!r||!r.read||!1===r.read.enabled||!r.read.source)return!1;const o=r.read.source;if("string"==typeof o){if(o===e)return!0;if(o.indexOf(".")>-1&&0===o.indexOf(e)&&s(o,t))return!0}else for(const r of o){if(r===e)return!0;if(r.indexOf(".")>-1&&0===r.indexOf(e)&&s(r,t))return!0}return!1}function O(r,e,t,o,n){let s=u(e[t],n);(function(r){return r&&(!r.read||!1!==r.read.enabled&&!r.read.source)})(s)&&(r[t]=!0);for(const i of Object.getOwnPropertyNames(e))s=u(e[i],n),m(s,t,o)&&(r[i]=!0)}function w(r,e,t,o){const n=t.metadatas,s=c(n[e],"any",o),i=s&&s.default;if(void 0===i)return;const a="function"==typeof i?i.call(r,e,o):i;void 0!==a&&t.set(e,a)}const y={origin:"service"};function N(r,e,t=y){if(!e||"object"!=typeof e)return;const o=i(r),s=o.metadatas,a={};for(const r of Object.getOwnPropertyNames(e))O(a,s,r,e,t);o.setDefaultOrigin(t.origin);for(const i of Object.getOwnPropertyNames(a)){const a=u(s[i],t).read,c=a&&a.source;let f;f=c&&"string"==typeof c?n(e,c):e[i],a&&a.reader&&(f=a.reader.call(r,f,e,t)),void 0!==f&&o.set(i,f)}if(!t||!t.ignoreDefaults)for(const e of Object.getOwnPropertyNames(s))a[e]||w(r,e,o,t);o.setDefaultOrigin("user")}function v(r,e,t,o=y){var n;const s={...o,messages:[]};t(s),null==(n=s.messages)||n.forEach((e=>{"warning"!==e.type||r.loaded?o&&o.messages&&o.messages.push(e):r.loadWarnings.push(e)}))}const S=g.getLogger("esri.core.accessorSupport.write");function b(r,e,t,o,n){var s,i;const a={};return null==(s=e.write)||null==(i=s.writer)||i.call(r,o,a,t,n),a}function h(r,e,o,n,s,i){if(!n||!n.write)return!1;const a=r.get(o);if(!s&&n.write.overridePolicy){const e=n.write.overridePolicy.call(r,a,o,i);void 0!==e&&(s=e)}if(s||(s=n.write),!s||!1===s.enabled)return!1;if((null===a&&!s.allowNull&&!s.writerEnsuresNonNull||void 0===a)&&s.isRequired){const e=new p("web-document-write:property-required",`Missing value for required property '${o}' on '${r.declaredClass}'`,{propertyName:o,target:r});return e&&i&&i.messages?i.messages.push(e):e&&!i&&S.error(e.name,e.message),!1}if(void 0===a)return!1;if(null===a&&!s.allowNull&&!s.writerEnsuresNonNull)return!1;if(function(r,e,t,o,n){const s=o.default;if(void 0===s)return!1;if(null!=o.defaultEquals)return o.defaultEquals(n);if("function"==typeof s){if(Array.isArray(n)){const o=s.call(r,e,t);return d(o,n)}return!1}return s===n}(r,o,i,n,a))return!1;if(void 0!==n.default)return!0;if(!s.ignoreOrigin&&i&&i.origin){if(e.store.originOf(o)<t(i.origin))return!1}return!0}function j(r,e,t,o){const n=i(r),s=n.metadatas,a=f(s[e],o);return!!a&&h(r,n,e,a,t,o)}function J(r,e,t){if(r&&"function"==typeof r.toJSON&&(!r.toJSON.isDefaultToJSON||!r.write))return a(e,r.toJSON());const n=i(r),s=n.metadatas;for(const i in s){const l=f(s[i],t);if(!h(r,n,i,l,void 0,t))continue;const d=r.get(i),p=b(r,l,l.write&&"string"==typeof l.write.target?l.write.target:i,d,t);var u,c;if(Object.keys(p).length>0)e=a(e,p),null!=t&&null!=(u=t.resources)&&null!=(c=u.pendingOperations)&&c.length&&Promise.all(t.resources.pendingOperations).then((()=>a(e,p))),t&&t.writtenProperties&&t.writtenProperties.push({target:r,propName:i,oldOrigin:o(n.store.originOf(i)),newOrigin:t.origin})}return e}const P=e=>{let t=class extends e{constructor(...r){super(...r)}read(r,e){N(this,r,e)}write(r={},e){return J(this,r,e)}toJSON(r){return this.write({},r)}static fromJSON(r,e){return x.call(this,r,e)}};return t=r([l("esri.core.JSONSupport")],t),t.prototype.toJSON.isDefaultToJSON=!0,t};function x(r,e){if(!r)return null;if(r.declaredClass)throw new Error("JSON object is already hydrated");const t=new this;return t.read(r,e),t}let E=class extends(P(e)){};E=r([l("esri.core.JSONSupport")],E);export{P as J,E as a,j as b,v as c,N as r,J as w};
