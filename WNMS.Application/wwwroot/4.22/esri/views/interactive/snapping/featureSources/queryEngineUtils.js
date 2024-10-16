// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../candidates/EdgeSnappingCandidate","../candidates/VertexSnappingCandidate"],function(c,d,e){c.convertSnappingCandidate=function(a,b){switch(a.type){case "edge":return new d.EdgeSnappingCandidate({coordinateHelper:b,edgeStart:b.pointToVector(a.start),edgeEnd:b.pointToVector(a.end),targetPoint:b.pointToVector(a.target),objectId:a.objectId});case "vertex":return new e.VertexSnappingCandidate({coordinateHelper:b,targetPoint:b.pointToVector(a.target),objectId:a.objectId})}};Object.defineProperty(c,
"__esModule",{value:!0})});