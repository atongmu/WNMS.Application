// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/Accessor ../../../../core/Handles ../../../../core/mathUtils ../../../../core/maybe ../../../../core/Quantity ../../../../core/reactiveUtils ../../../../core/watchUtils ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../../../../geometry/geometryEngine ../../../../geometry/Polyline ../../../../geometry/projection ../../../../geometry/projectionEllipsoid ../../../../geometry/support/geodesicUtils ../support/UnitNormalizer ../../support/ElevationProvider".split(" "),
function(h,B,p,E,F,m,q,r,n,G,w,N,O,P,H,C,t,I,J,l,x,A,K,L){h.DirectLineMeasurementController=function(D){function y(a){a=D.call(this,a)||this;a._unitNormalizer=new K.UnitNormalizer;a._handles=new F;a._tempStartPosition=t.create();a._tempEndPosition=t.create();a._tempCornerPosition=t.create();return a}B._inheritsLoose(y,D);var k=y.prototype;k.initialize=function(){this._handles.add(G.whenOnce(this.view,"ready",()=>this._initialize(),!0))};k.destroy=function(){this._handles=q.destroyMaybe(this._handles)};
k._initialize=function(){const a=this.view.spatialReference;var b=x.getSphericalPCPF(a);this._sphericalPCPF=b=b===x.SphericalECEFSpatialReference?x.WGS84ECEFSpatialReference:b;const e=l.canProjectWithoutEngine(a,b);this._unitNormalizer.spatialReference=e?b:a;this._handles.add([n.react(()=>({viewData:this.viewData,startPoint:this.analysis.startPoint}),({viewData:d,startPoint:c})=>{d.elevationAlignedStartPoint=this._applyElevationAlignment(c)},n.syncAndInitial),n.react(()=>({viewData:this.viewData,
endPoint:this.analysis.endPoint}),({viewData:d,endPoint:c})=>{d.elevationAlignedEndPoint=this._applyElevationAlignment(c)},n.syncAndInitial),n.react(()=>({result:this._computedResult,viewData:this.viewData}),({result:d,viewData:c})=>{c.result=d},n.syncAndInitial)])};k._applyElevationAlignment=function(a){if(q.isNone(a)||a.hasZ)return a;a=a.clone();a.z=q.unwrapOr(L.getElevationAtPoint(this.view.elevationProvider,a),0);return a};k._euclideanDistances=function(a,b){var e=a.clone();e.z=b.z;const d=this._tempStartPosition;
var c=this._tempEndPosition,g=this._tempCornerPosition,f=this.view.spatialReference;const z=this._sphericalPCPF;f=l.canProjectWithoutEngine(f,z)?z:f;l.projectPointToVector(a,d,f);l.projectPointToVector(b,c,f);l.projectPointToVector(e,g,f);e=C.distance(d,c);c=C.distance(g,c);a=Math.abs(b.z-a.z);b=M=>this._unitNormalizer.normalizeDistance(M);g=b(e);c=b(c);a=b(a);return{direct:new r(g,"meters"),horizontal:new r(c,"meters"),vertical:new r(a,"meters")}};k._exactGeodesicDistanceAndAngle=function(a,b,e){const d=
a.spatialReference;var c=new J({spatialReference:d});c.addPath([a,b]);c=d.isGeographic&&A.isSupported(d)?A.geodesicLengths([c],"meters")[0]:d.isWebMercator?I.geodesicLength(c,"meters"):void 0;const {distance:g,angle:f}=c?{distance:c,angle:this._fallbackGeodesicAngle(c,d)}:this._fallbackGeodesicDistance(a,b,e);return{distance:new r(g,"meters"),angle:new r(f,"degrees")}};k._fallbackGeodesicAngle=function(a,b){return a/x.getReferenceEllipsoid(b).metersPerDegree};k._fallbackGeodesicDistance=function(a,
b,e){if(l.projectPointToWGS84ComparableLonLat(a,u)){l.projectPointToWGS84ComparableLonLat(b,v);a=m.deg2rad(u[0]);b=m.deg2rad(u[1]);e=m.deg2rad(v[0]);const d=m.deg2rad(v[1]);a=m.acosClamped(Math.sin(b)*Math.sin(d)+Math.cos(b)*Math.cos(d)*Math.cos(Math.abs(e-a)));a=m.rad2deg(a);b={distance:0};A.inverseGeodeticSolver(b,[u[0],u[1]],[v[0],v[1]]);return{distance:b.distance,angle:a}}return{distance:e,angle:this._fallbackGeodesicAngle(e,a.spatialReference)}};B._createClass(y,[{key:"_computedResult",get:function(){const {elevationAlignedStartPoint:a,
elevationAlignedEndPoint:b}=this.viewData;if(q.isNone(a)||q.isNone(b))return null;const e=this._euclideanDistances(a,b),d=this._exactGeodesicDistanceAndAngle(a,b,e.horizontal.value);let c=null;switch(this.viewData.measurementMode){case 0:{var g;c="euclidean";const f=this.viewData.unit,z=1E5<(null==(g=e.horizontal)?void 0:g.value);if("degrees"===f||"degrees-minutes-seconds"===f||z)c="geodesic";break}case 1:c="euclidean";break;case 2:c="geodesic"}return{distance:"euclidean"===c?e.direct:d.distance,
mode:c,directDistance:e.direct,horizontalDistance:e.horizontal,verticalDistance:e.vertical,geodesicDistance:d.distance,geodesicAngle:d.angle}}}]);return y}(E);p.__decorate([w.property()],h.DirectLineMeasurementController.prototype,"view",void 0);p.__decorate([w.property()],h.DirectLineMeasurementController.prototype,"analysis",void 0);p.__decorate([w.property()],h.DirectLineMeasurementController.prototype,"viewData",void 0);p.__decorate([w.property()],h.DirectLineMeasurementController.prototype,"_computedResult",
null);h.DirectLineMeasurementController=p.__decorate([H.subclass("esri.views.3d.analysis.DirectLineMeasurement.DirectLineMeasurementController")],h.DirectLineMeasurementController);const u=t.create(),v=t.create();Object.defineProperty(h,"__esModule",{value:!0})});