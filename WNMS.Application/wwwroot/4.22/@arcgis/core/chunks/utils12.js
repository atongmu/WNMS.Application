/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{a as e,t,c as i}from"./screenUtils.js";import{d as r}from"./utils7.js";r.light,r.dark;function n(e,t){return e.r===t.r&&e.g===t.g&&e.b===t.b}function l(e,t){let i=0;if(e.length===t.length){let r=e.every(((e,i)=>n(e,t[i])));if(r)i=1;else{r=e.slice(0).reverse().every(((e,i)=>n(e,t[i]))),r&&(i=-1)}}return i}function s(r,n){return Math.ceil(function(e){const t=e.width,r=e.height;let n=e.pixelSizeAt(e.toMap(i(.5*t,.5*r),{exclude:[]}));if(n<=0&&(n=e.pixelSizeAt(e.toMap(i(.5*t,.95*r),{exclude:[]})),n<=0)){const t=e.camera.position.clone();t.z=0,n=2*e.pixelSizeAt(t)}return n}(n)*e(t(r)))}export{l as h,s as t};
