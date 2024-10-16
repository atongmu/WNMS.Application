// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports"],function(a){a.logFailedGeometryProjectionError=function(b,c,d){d.warnOnce(`Failed to project analysis geometry (id: '${b.id}'), projection from spatial reference `+`(wkid: '${c.wkid}') to view spatial reference is not supported. `+"Projection may be possible after calling projection.load().")};Object.defineProperty(a,"__esModule",{value:!0})});