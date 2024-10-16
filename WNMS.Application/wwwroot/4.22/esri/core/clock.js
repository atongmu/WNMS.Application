// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports"],function(a){function b(c){return{setTimeout:(d,e)=>{const f=c.setTimeout(d,e);return{remove:()=>c.clearTimeout(f)}}}}const g=b(globalThis);a.default=g;a.wrap=b;Object.defineProperty(a,"__esModule",{value:!0})});