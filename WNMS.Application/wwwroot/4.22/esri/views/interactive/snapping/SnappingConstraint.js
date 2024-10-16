// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../chunks/_rollupPluginBabelHelpers ../../../chunks/vec2 ../../../chunks/vec2f64 ./Settings ./snappingUtils ../../support/geometry2dUtils".split(" "),function(h,l,m,w,r,t,g){let n=function(e){this.coordinateHelper=e},v=function(e){function c(a,b){a=e.call(this,a)||this;a.point=b;return a}l._inheritsLoose(c,e);var d=c.prototype;d.objectEqual=function(a){return a instanceof c?t.objectEqual(this.point,a.point):!1};d.check=function(a){return m.squaredDistance(a,this.point)<r.defaults.pointThreshold};
d.closestTo=function(){return this.coordinateHelper.clone(this.point)};d.intersect=function(a){const b=[];a instanceof p?b.push(...g.intersectLineAndPoint({start:a.start,end:a.end,type:a instanceof k?1:0},this.point)):a instanceof u&&b.push(...g.intersectCircleAndPoint(a.center,a.radius,this.point));return b.map(f=>new q(this.coordinateHelper,f,this,a))};return c}(n),p=function(e){function c(a,b,f){a=e.call(this,a)||this;a.start=b;a.end=f;return a}l._inheritsLoose(c,e);var d=c.prototype;d.objectEqual=
function(a){return a instanceof c?t.objectEqual(this.start,a.start)&&t.objectEqual(this.end,a.end):!1};d.intersect=function(a){const b=[];a instanceof v?b.push(...g.intersectLineAndPoint({start:this.start,end:this.end,type:this instanceof k?1:0},a.point)):a instanceof c?b.push(...g.intersectLineAndRay({start:this.start,end:this.end,type:this instanceof k?1:0},{start:a.start,end:a.end,type:a instanceof k?1:0})):a instanceof u&&b.push(...g.intersectLineLikeAndCircle({start:this.start,end:this.end,type:this instanceof
k?1:0},a.center,a.radius));return b.map(f=>new q(this.coordinateHelper,f,this,a))};return c}(n),x=function(e){function c(a,b,f){a=e.call(this,a,b,f)||this;a.dir=w.create();a.p=w.create();return a}l._inheritsLoose(c,e);var d=c.prototype;d.objectEqual=function(a){return a instanceof c?e.prototype.objectEqual.call(this,a):!1};d.check=function(a){m.subtract(this.dir,this.end,this.start);m.subtract(this.p,a,this.start);return 0<=m.dot(this.dir,this.p)?g.pointToLineDistance(a,this.start,this.end)<r.defaults.pointOnLineThreshold:
!1};d.closestTo=function(a){const b=this.coordinateHelper.clone(a);g.projectPointToRay(b,a,this.start,this.end);return b};return c}(p),k=function(e){function c(a,b,f){return e.call(this,a,b,f)||this}l._inheritsLoose(c,e);var d=c.prototype;d.objectEqual=function(a){return a instanceof c?e.prototype.objectEqual.call(this,a):!1};d.check=function(a){return g.pointToLineDistance(a,this.start,this.end)<r.defaults.pointOnLineThreshold};d.closestTo=function(a){const b=this.coordinateHelper.clone(a);g.projectPointToLine(b,
a,this.start,this.end);return b};return c}(p),q=function(e){function c(a,b,f,y){a=e.call(this,a)||this;a.intersection=b;a.first=f;a.second=y;return a}l._inheritsLoose(c,e);var d=c.prototype;d.objectEqual=function(a){return a instanceof c?this.first.objectEqual(a.first)&&this.second.objectEqual(a.second):!1};d.check=function(){return!1};d.closestTo=function(a){a=this.coordinateHelper.clone(a);m.copy(a,this.intersection);return a};d.intersect=function(){return[]};return c}(n),u=function(e){function c(a,
b,f){a=e.call(this,a)||this;a.center=b;a.radius=f;return a}l._inheritsLoose(c,e);var d=c.prototype;d.objectEqual=function(a){return a instanceof c?this.center[0]===a.center[0]&&this.center[1]===a.center[1]&&this.radius===a.radius:!1};d.check=function(){return!1};d.closestTo=function(a){const b=this.coordinateHelper.clone(a);g.projectPointToCircle(b,a,this.center,this.radius);return b};d.intersect=function(a){const b=[];a instanceof p?b.push(...g.intersectLineLikeAndCircle({start:a.start,end:a.end,
type:a instanceof k?1:0},this.center,this.radius)):a instanceof v&&b.push(...g.intersectCircleAndPoint(this.center,this.radius,a.point));return b.map(f=>new q(this.coordinateHelper,f,this,a))};return c}(n);h.IntersectionConstraint=q;h.LineConstraint=k;h.LineLikeConstraint=p;h.PlanarCircleConstraint=u;h.PointConstraint=v;h.RayConstraint=x;h.SnappingConstraint=n;Object.defineProperty(h,"__esModule",{value:!0})});