// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports"],function(e){e.findInMap=function(c,d){for(const [a,b]of c)if(d(b,a))return b;return null};e.getOrCreateMapValue=function(c,d,a){const b=c.get(d);if(void 0!==b)return b;a=a();c.set(d,a);return a};e.someMap=function(c,d){for(const [a,b]of c)if(d(b,a))return!0;return!1};Object.defineProperty(e,"__esModule",{value:!0})});