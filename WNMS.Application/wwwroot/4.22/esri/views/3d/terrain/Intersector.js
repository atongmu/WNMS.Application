// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../webgl-engine/lib/intersectorUtils"],function(b,c){b.isOverlayIntersectorResult=function(a){return c.isValidIntersectorResult(a)&&3===a.intersector&&!!a.target};b.isTerrainIntersectorResult=function(a){return c.isValidIntersectorResult(a)&&2===a.intersector&&!!a.target};Object.defineProperty(b,"__esModule",{value:!0})});