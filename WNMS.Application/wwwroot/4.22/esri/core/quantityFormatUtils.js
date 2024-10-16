// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./unitFormatUtils"],function(e,f){e.formatDMS=function(b){if("angle"!==b.measure)throw Error("quantity is not an angle");return f.formatDMS(b.value,b.unit)};e.formatDecimal=function(b,a,c,d=2,g="abbr"){return f.formatDecimal(b,a.toUnit(c).value,c,d,g)};e.formatImperialArea=function(b,a,c=2,d="abbr"){if("area"!==a.measure)throw Error("quantity is not an area");return f.formatImperialArea(b,a.value,a.unit,c,d)};e.formatImperialLength=function(b,a,c=2,d="abbr"){if("length"!==a.measure)throw Error("quantity is not a length");
return f.formatImperialLength(b,a.value,a.unit,c,d)};e.formatImperialVerticalLength=function(b,a,c=2,d="abbr"){if("length"!==a.measure)throw Error("quantity is not a length");return f.formatImperialVerticalLength(b,a.value,a.unit,c,d)};e.formatMetricArea=function(b,a,c=2,d="abbr"){if("area"!==a.measure)throw Error("quantity is not an area");return f.formatMetricArea(b,a.value,a.unit,c,d)};e.formatMetricLength=function(b,a,c=2,d="abbr"){if("length"!==a.measure)throw Error("quantity is not a length");
return f.formatMetricLength(b,a.value,a.unit,c,d)};e.formatMetricVerticalLength=function(b,a,c=2,d="abbr"){if("length"!==a.measure)throw Error("quantity is not a length");return f.formatMetricVerticalLength(b,a.value,a.unit,c,d)};Object.defineProperty(e,"__esModule",{value:!0})});