/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{i as e,b as r}from"../core/lang.js";import{project as o}from"../geometry/support/webMercatorUtils.js";import{projectGeometry as t}from"./geometryServiceUtils.js";function s(s){const l=s.view.spatialReference,i=s.layer.fullExtent,a=e(i)&&i.spatialReference;if(r(i)||!a)return Promise.resolve(null);if(a.equals(l))return Promise.resolve(i.clone());const n=o(i,l);return e(n)?Promise.resolve(n):s.view.state.isLocal?t(i,l,s.layer.portalItem).then((e=>!s.destroyed&&e?e:void 0)).catch((()=>null)):Promise.resolve(null)}export{s as t};
