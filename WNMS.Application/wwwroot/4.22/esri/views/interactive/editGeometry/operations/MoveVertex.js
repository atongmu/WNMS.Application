// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports"],function(g){let h=function(){function d(a,b,e,f){this.helper=a;this.dx=b;this.dy=e;this.dz=f}var c=d.prototype;c.move=function(a,b,e,f){this.helper.addDelta(a.pos,b,e,f)};c.apply=function(a){this.move(a,this.dx,this.dy,this.dz)};c.undo=function(a){this.move(a,-this.dx,-this.dy,-this.dz)};c.canAccumulate=function(a){return a instanceof d};c.accumulate=function(a,b){this.move(a,b.dx,b.dy,b.dz)};c.accumulateParams=function(a){this.dx+=a.dx;this.dy+=a.dy;this.dz+=a.dz};return d}();
g.MoveVertex=h;Object.defineProperty(g,"__esModule",{value:!0})});