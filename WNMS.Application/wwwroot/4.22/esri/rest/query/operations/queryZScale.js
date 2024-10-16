// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../../core/maybe","../../../geometry/support/zscale"],function(c,d,e){c.applyFeatureSetZUnitScaling=function(b,f,a){if(a&&a.features&&a.hasZ&&(b=e.getGeometryZScaler(a.geometryType,f,b.outSpatialReference),!d.isNone(b)))for(const g of a.features)b(g.geometry)};Object.defineProperty(c,"__esModule",{value:!0})});