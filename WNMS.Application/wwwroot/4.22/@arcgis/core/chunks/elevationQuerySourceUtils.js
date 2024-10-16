/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import n from"../Ground.js";import{b as r,U as t,i as l,T as i}from"../core/lang.js";import{d as o}from"./unitUtils.js";function e(l){if(r(l))return null;if(l instanceof n)return s(l);const i=l.tileInfo;if(r(i))return null;const e=t(i.lods);return r(e)?null:e.resolution*o(i.spatialReference)}function s(n){var t;if(r(n))return null;const o=n.layers.items.map(u).filter(l);return null!=(t=i(o))?t:null}function u(n){return"tileInfo"in n?e(n):null}export{e as a,s as g};
