// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../../geometry/support/spatialReferenceUtils"],function(f,l){f.getUnnormalizationInfo=function(a,b){a=l.getInfo(a);return 1===b&&a?{supported:!0,lowerBoundX:a.valid[0],upperBoundX:a.valid[1]}:{supported:!1,lowerBoundX:null,upperBoundX:null}};f.unnormalize=function(a,b){if(b.supported){var g=Infinity,h=-Infinity,e=b.upperBoundX-b.lowerBoundX;a.forEach(d=>{let c=d.pos[0];for(;c<b.lowerBoundX;)c+=e;for(;c>b.upperBoundX;)c-=e;g=Math.min(g,c);h=Math.max(h,c);d.pos[0]=c});var k=h-
g;e-k<k&&a.forEach(d=>{0>d.pos[0]&&(d.pos[0]+=e)})}};Object.defineProperty(f,"__esModule",{value:!0})});