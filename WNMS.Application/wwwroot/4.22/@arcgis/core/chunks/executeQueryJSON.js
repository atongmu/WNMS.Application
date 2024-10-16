/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{p as t}from"./utils3.js";import{e as r,a,b as s}from"./query.js";import o from"../rest/support/Query.js";import n from"../rest/support/FeatureSet.js";async function e(a,s,n){const e=t(a);return r(e,o.from(s),{...n}).then((t=>t.data.count))}async function c(r,s,n){const e=t(r);return a(e,o.from(s),{...n}).then((t=>t.data.objectIds))}async function u(t,r,a){const s=await f(t,r,a);return n.fromJSON(s)}async function f(r,a,n){const e=t(r),c={...n},u=o.from(a),{data:f}=await s(e,u,u.sourceSpatialReference,c);return f}export{c as a,e as b,f as c,u as e};
