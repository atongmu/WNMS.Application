// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/mathUtils ../../../../core/screenUtils ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../../../../geometry/Point ../../../../geometry/support/ray ../PropertiesPool ./PointOfInterest ../../../support/Scheduler".split(" "),
function(d,n,f,u,v,g,D,E,F,w,h,p,x,y,z,A,B){const C=Array;d.Focus=function(q){function m(a){a=q.call(this,a)||this;a._propertiesPool=new z.PropertiesPool({location:x,renderLocation:C},n._assertThisInitialized(a));a._dirty=!0;a.renderLocation=a._propertiesPool.get("renderLocation");return a}n._inheritsLoose(m,q);var k=m.prototype;k.initialize=function(){this.handles.add([this.centerOnSurface.watch("renderLocation",()=>this.updateRenderLocation()),this.state.watch("contentCamera",()=>this.updateRenderLocation())]);
this.scheduler&&this.handles.add(this.scheduler.registerTask(B.TaskPriority.POINT_OF_INTEREST_FREQUENT,this))};k.destroy=function(){this._propertiesPool.destroy();this._propertiesPool=null};k.runTask=function(){var a=this._get("renderLocation");const c=this.centerOnSurface.renderLocation;var b=this.renderCoordsHelper,e=this.state.contentCamera;this._dirty=!1;b.worldUpAtPosition(c,r);b=Math.max(0,(Math.acos(h.dot(r,e.viewForward))-.5*Math.PI)*(e.aboveGround?1:-1));Number.isNaN(b)?a&&h.equals(a,c)||
(a=this._propertiesPool.get("renderLocation"),h.copy(a,c),this._set("renderLocation",a)):(b=1-u.clamp(b/(.5*Math.PI),0,1),b*=b*b,this._calculateScreenHorizontalEdgeOnSurface(t),e=this._propertiesPool.get("renderLocation"),h.lerp(e,c,t,b),a&&h.equals(a,e)||this._set("renderLocation",e))};k._calculateScreenHorizontalEdgeOnSurface=function(a){const c=this.state.contentCamera;var b=c.getRenderCenter(v.createRenderScreenPointArray3());b[1]=c.aboveGround?c.padding[2]:c.fullHeight-c.padding[0];if(this.estimateSurfaceIntersectionAtRenderPoint(b,
a))return a;const e=this.renderCoordsHelper.getAltitude(this.centerOnSurface.renderLocation);return c.unprojectFromRenderScreen(b,l)&&(h.subtract(l,l,c.eye),b=h.normalize(l,l),this.renderCoordsHelper.intersectManifold(y.wrap(c.eye,b),e,a))?a:this.renderCoordsHelper.setAltitude(a,e,c.eye)};k.updateRenderLocation=function(){this._dirty=!0};n._createClass(m,[{key:"updating",get:function(){return this._dirty||this.centerOnSurface.updating}},{key:"location",get:function(){const a=this._propertiesPool.get("location");
this.renderCoordsHelper.fromRenderCoords(this.renderLocation,a,this.state.spatialReference);return a}},{key:"running",get:function(){return this._dirty}}]);return m}(A.PointOfInterest);f.__decorate([g.property()],d.Focus.prototype,"_dirty",void 0);f.__decorate([g.property({constructOnly:!0})],d.Focus.prototype,"scheduler",void 0);f.__decorate([g.property({constructOnly:!0})],d.Focus.prototype,"centerOnSurface",void 0);f.__decorate([g.property({constructOnly:!0})],d.Focus.prototype,"estimateSurfaceIntersectionAtRenderPoint",
void 0);f.__decorate([g.property({readOnly:!0})],d.Focus.prototype,"updating",null);f.__decorate([g.property({readOnly:!0})],d.Focus.prototype,"location",null);f.__decorate([g.property({readOnly:!0})],d.Focus.prototype,"renderLocation",void 0);d.Focus=f.__decorate([w.subclass("esri.views.3d.support.CenterOnSurface")],d.Focus);const r=p.create(),l=p.create(),t=p.create();d.default=d.Focus;Object.defineProperty(d,"__esModule",{value:!0})});