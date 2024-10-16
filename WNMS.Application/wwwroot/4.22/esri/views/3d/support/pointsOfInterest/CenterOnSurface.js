// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/mathUtils ../../../../core/maybe ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../../../../geometry/Point ../../../../geometry/projection ../../../../geometry/projectionEllipsoid ../../../../geometry/support/aaBoundingRect ../debugFlags ../PropertiesPool ./PointOfInterest".split(" "),
function(d,m,f,p,u,g,I,J,K,v,e,n,w,x,y,z,A,B,C){const D=Array;d.CenterOnSurface=function(q){function l(a){a=q.call(this,a)||this;a._propertiesPool=new B.PropertiesPool({location:w,renderLocation:D},m._assertThisInitialized(a));a._currentSurfaceAltitude=0;a._latestSurfaceAltitude=0;a.cameraName="camera";a.distance=0;a.renderLocation=n.create();a.updating=!1;return a}m._inheritsLoose(l,q);var h=l.prototype;h.initialize=function(){this._frameWorker=this.scheduler.registerTask(this.task,this);this.runTask()};
h.destroy=function(){this._frameWorker&&(this._frameWorker.remove(),this._frameWorker=null);this._propertiesPool.destroy();this._propertiesPool=null};h.updateRenderLocation=function(){this.updating=!0;this._updateRenderLocation()};h.runTask=function(){this._latestSurfaceAltitude=this.estimateSurfaceAltitudeAtCenter();this._updateRenderLocation();this.updating=!1};h._updateRenderLocation=function(){const a=E;let b=this.calculateSurfaceIntersection(this._currentSurfaceAltitude,a);var c=this._currentSurfaceAltitude!==
this._latestSurfaceAltitude;!b&&c&&(b=this.calculateSurfaceIntersection(this._latestSurfaceAltitude,a))&&(this._currentSurfaceAltitude=this._latestSurfaceAltitude);c=F;b&&this.latestSurfaceAltitudeChangesDistanceSignificantly(a,c)&&(e.copy(a,c),this._currentSurfaceAltitude=this._latestSurfaceAltitude);b?this.distance=e.distance(this._camera.eye,a):(e.scale(a,this._camera.viewForward,this._get("distance")),e.add(a,a,this._camera.eye));e.exactEquals(this._get("renderLocation"),a)||this._set("renderLocation",
e.copy(this._propertiesPool.get("renderLocation"),a))};h.calculateSurfaceIntersection=function(a,b){var c=this._camera;if(!this.renderCoordsHelper.intersectManifold(c.ray,a,b))return!1;if(this.state.isGlobal){const r=y.getReferenceEllipsoid(this.renderCoordsHelper.spatialReference).radius;a=r+a;const t=e.squaredLength(c.eye),G=t<a*a,H=e.distance(c.eye,b);G&&H>r/4&&(e.scale(b,c.viewForward,a-Math.sqrt(t)),e.add(b,b,c.eye))}else c=this.surface.ready?this.surface.extent:null,u.isSome(c)&&x.projectBoundingRect(c,
this.surface.spatialReference,k,this.renderCoordsHelper.spatialReference)&&(b[0]=p.clamp(b[0],k[0],k[2]),b[1]=p.clamp(b[1],k[1],k[3]));return!0};h.latestSurfaceAltitudeChangesDistanceSignificantly=function(a,b){if(this._latestSurfaceAltitude===this._currentSurfaceAltitude||null==a)return!1;if(this.calculateSurfaceIntersection(this._latestSurfaceAltitude,b)){if(A.TESTS_DISABLE_OPTIMIZATIONS)return!0;const c=this._camera.eye;a=e.distance(c,a);b=e.distance(c,b);if(.05<Math.abs(b-a)/a)return!0}return!1};
m._createClass(l,[{key:"_camera",get:function(){return this.state[this.cameraName]}},{key:"location",get:function(){const a=this._propertiesPool.get("location");this.renderCoordsHelper.fromRenderCoords(this.renderLocation,a,this.state.spatialReference);return a}},{key:"estimatedSurfaceAltitude",get:function(){return this._latestSurfaceAltitude}},{key:"running",get:function(){return this.updating}}]);return l}(C.PointOfInterest);f.__decorate([g.property({constructOnly:!0})],d.CenterOnSurface.prototype,
"scheduler",void 0);f.__decorate([g.property({constructOnly:!0})],d.CenterOnSurface.prototype,"task",void 0);f.__decorate([g.property({constructOnly:!0})],d.CenterOnSurface.prototype,"cameraName",void 0);f.__decorate([g.property()],d.CenterOnSurface.prototype,"distance",void 0);f.__decorate([g.property({constructOnly:!0})],d.CenterOnSurface.prototype,"estimateSurfaceAltitudeAtCenter",void 0);f.__decorate([g.property({readOnly:!0})],d.CenterOnSurface.prototype,"location",null);f.__decorate([g.property({readOnly:!0})],
d.CenterOnSurface.prototype,"renderLocation",void 0);f.__decorate([g.property()],d.CenterOnSurface.prototype,"updating",void 0);d.CenterOnSurface=f.__decorate([v.subclass("esri.views.3d.support.CenterOnSurface")],d.CenterOnSurface);const E=n.create(),F=n.create(),k=z.create();d.default=d.CenterOnSurface;Object.defineProperty(d,"__esModule",{value:!0})});