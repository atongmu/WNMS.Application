// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../chunks/_rollupPluginBabelHelpers ../../../core/maybe ../../../chunks/vec2 ../../../chunks/vec2f64 ./SnappingAlgorithm ./SnappingConstraint ./snappingUtils ./candidates/RightAngleSnappingCandidate".split(" "),function(t,v,w,k,n,p,x,m,y){p=function(u){function q(){var d=u.apply(this,arguments)||this;d._tmp=n.create();return d}v._inheritsLoose(q,u);var r=q.prototype;r.snapNewVertex=function(d,b){var c=b.editGeometryOperations.data.components[0],a=c.vertices.length;const e=[];
if(2>a)return e;const g=m.anyMapPointToScreenPoint(d,b.coordinateHelper,b.elevationInfo,this.view);a=c.vertices[a-1];this._checkForSnappingCandidate(e,a.leftEdge,a.pos,d,a.leftEdge.leftVertex.pos,a.pos,b,d,g);c=c.vertices[0];this._checkForSnappingCandidate(e,c.rightEdge,c.pos,d,c.rightEdge.rightVertex.pos,c.pos,b,d,g);return e};r.snapExistingVertex=function(d,b){const c=[];var a=w.unwrap(b.vertexHandle),e=a.component;const g=e.vertices.length;if(3>g)return c;const h=m.anyMapPointToScreenPoint(d,b.coordinateHelper,
b.elevationInfo,this.view);var f=a.leftEdge;a=a.rightEdge;const l=e.vertices[0];e=e.vertices[g-1];if(!f)return this._checkForSnappingCandidate(c,l.rightEdge.rightVertex.rightEdge,l.rightEdge.rightVertex.pos,d,l.rightEdge.rightVertex.rightEdge.rightVertex.pos,l.rightEdge.rightVertex.pos,b,d,h),c;if(!a)return this._checkForSnappingCandidate(c,e.leftEdge.leftVertex.leftEdge,e.leftEdge.leftVertex.pos,d,e.leftEdge.leftVertex.leftEdge.leftVertex.pos,e.leftEdge.leftVertex.pos,b,d,h),c;f&&f.leftVertex.leftEdge&&
(e=f.leftVertex.leftEdge,this._checkForSnappingCandidate(c,e,f.leftVertex.pos,d,e.leftVertex.pos,f.leftVertex.pos,b,d,h));a&&a.rightVertex.rightEdge&&(f=a.rightVertex.rightEdge,this._checkForSnappingCandidate(c,f,a.rightVertex.pos,d,f.rightVertex.pos,a.rightVertex.pos,b,d,h));return c};r._checkForSnappingCandidate=function(d,b,c,a,e,g,h,f,l){this.edgeExceedsShortLineThreshold(b,h)&&(k.subtract(this._tmp,b.rightVertex.pos,b.leftVertex.pos),b=n.fromValues(this._tmp[1],-this._tmp[0]),c=k.dot(b,k.subtract(this._tmp,
a,c))/k.squaredLength(b),a=h.coordinateHelper,f=a.fromXYZ(k.scaleAndAdd(n.create(),g,b,c),a.getZ(f,0)),m.squareDistance(l,m.anyMapPointToScreenPoint(f,a,h.elevationInfo,this.view))<this.squaredProximityTreshold(h.pointer)&&d.push(new y.RightAngleSnappingCandidate({coordinateHelper:a,targetPoint:f,constraint:new x.RayConstraint(a,g,k.scaleAndAdd(a.createVector(),g,b,Math.sign(c))),previousVertex:e,otherVertex:g,otherVertexType:1})))};return q}(p.SnappingAlgorithm);t.RightAngleSnapper=p;Object.defineProperty(t,
"__esModule",{value:!0})});