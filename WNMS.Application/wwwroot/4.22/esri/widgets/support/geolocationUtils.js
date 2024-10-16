// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../config ../../core/Error ../../core/has ../../core/Logger ../../geometry/Point ../../geometry/support/webMercatorUtils ../../portal/Portal ../../chunks/_rollupPluginBabelHelpers ../../request ../../core/urlUtils ../../core/unitUtils ../../geometry ../../geometry/Extent ../../geometry/Geometry ../../geometry/Multipoint ../../geometry/Polygon ../../geometry/Polyline ../../rest/geometryService/units ../../rest/support/GeneralizeParameters ../../tasks/operations/generalize ../../rest/support/LengthsParameters ../../tasks/operations/lengths ../../rest/support/OffsetParameters ../../tasks/operations/offset ../../rest/geometryService/project ../../rest/support/RelationParameters ../../tasks/operations/relation ../../rest/support/TrimExtendParameters ../../tasks/operations/trimExtend ../../rest/support/ProjectParameters".split(" "),
function(e,f,g,l,m,n,p,q,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,r,Q,R,S,T,t){function u(a,c,b){if(!c)return Promise.resolve(a);const d=c.spatialReference;return d.isWGS84?Promise.resolve(a):d.isWebMercator?Promise.resolve(p.geographicToWebMercator(a)):v(b).then(h=>{if(!h)throw new g("geometry-service:missing-url","Geometry service URL is missing");const w=new t({geometries:[a],outSpatialReference:d});return r.project(h,w,b).then(x=>x[0])})}function v(a){if(f.geometryServiceUrl)return Promise.resolve(f.geometryServiceUrl);
const c=q.getDefault();return c.load(a).catch(()=>{}).then(()=>c.get("helperServices.geometry.url"))}const k=m.getLogger("esri.widgets.support.geolocationUtils"),y={maximumAge:0,timeout:15E3,enableHighAccuracy:!0};e.getCurrentPosition=function(a){a||(a=y);return new Promise((c,b)=>{setTimeout(()=>b(new g("geolocation:timeout","getting the current geolocation position timed out")),15E3);navigator.geolocation.getCurrentPosition(c,b,a)})};e.positionToPoint=function(a,c){const {position:b,view:d}=a;a=
b&&b.coords||{};a={accuracy:a.accuracy,altitude:a.altitude,altitudeAccuracy:a.altitudeAccuracy,heading:a.heading,latitude:a.latitude,longitude:a.longitude,speed:a.speed};a=b?{coords:a,timestamp:b.timestamp}:b;a=(a=a&&a.coords)?new n({longitude:a.longitude,latitude:a.latitude,z:a.altitude||null,spatialReference:{wkid:4326}}):null;return u(a,d,c)};e.supported=function(){var a;(a=l("esri-geolocation"))||k.warn("geolocation-unsupported","Geolocation unsupported.");a&&((a=window.isSecureContext)||k.warn("insecure-context",
"Geolocation requires a secure origin."));return a};Object.defineProperty(e,"__esModule",{value:!0})});