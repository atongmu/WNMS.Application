// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./componentsUtils"],function(b,c){b.createObserver=function(a,d,e){a="intersection"===a?IntersectionObserver:"mutation"===a?MutationObserver:ResizeObserver;return c.Build.isBrowser?new a(d,e):void 0}});