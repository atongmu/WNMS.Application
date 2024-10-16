// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../../../core/maybe","../../../../chunks/vec3","../../../../chunks/vec3f64","../../../../chunks/boundedPlane"],function(b,c,f,g,h){function d(a){return c.isSome(a)&&c.isSome(a.dist)}const e=g.create();b.isHudIntersectorResult=function(a){return d(a)&&1===a.intersector&&!!a.target&&c.isSome(a.target.center)};b.isObjectIntersectorResult=function(a){return d(a)&&0===a.intersector&&!!a.target};b.isValidIntersectorResult=d;b.sliceFilterPredicate=function(a){return(k,l,m)=>{f.lerp(e,
k,l,m);return!h.extrusionContainsPoint(a,e)}};Object.defineProperty(b,"__esModule",{value:!0})});