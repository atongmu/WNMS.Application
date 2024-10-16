// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["require","exports"],function(d,e){e.loadProcessorModule=function(f){return"heatmap"===f?new Promise((a,b)=>d(["./processors/HeatmapProcessor"],c=>a(Object.freeze({__proto__:null,default:c})),b)):new Promise((a,b)=>d(["./processors/SymbolProcessor"],c=>a(Object.freeze({__proto__:null,default:c})),b))};Object.defineProperty(e,"__esModule",{value:!0})});