/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
function a(a){return a=a||globalThis.location.hostname,l.some((c=>{var t;return null!=(null==(t=a)?void 0:t.match(c))}))}function c(a,c){return a&&(c=c||globalThis.location.hostname)?null!=c.match(t)||null!=c.match(o)?a.replace("static.arcgis.com","staticdev.arcgis.com"):null!=c.match(s)||null!=c.match(i)?a.replace("static.arcgis.com","staticqa.arcgis.com"):a:a}const t=/^devext.arcgis.com$/,s=/^qaext.arcgis.com$/,o=/^[\w-]*\.mapsdevext.arcgis.com$/,i=/^[\w-]*\.mapsqa.arcgis.com$/,l=[/^([\w-]*\.)?[\w-]*\.zrh-dev-local.esri.com$/,t,s,/^jsapps.esri.com$/,o,i];export{c as a,a as i};
