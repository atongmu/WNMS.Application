// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../../../../geometry/geometryEngine ../../../../geometry/Polygon ../../../../geometry/projection ../../../../geometry/SpatialReference ../../../../geometry/support/geodesicUtils ../../support/mathUtils".split(" "),function(f,d,n,w,x,m,y,u,z){const k=n.create(),l=n.create(),r=n.create(),p=new x({rings:[[k,l,r]],spatialReference:y.WGS84}),g=n.create();f.bestFitPlane=function(a,b){if(3>a.length)throw Error("need at least 3 points to fit a plane");
z.planeFromPoints(a[0],a[1],a[2],b)};f.boundingSphere=function(a,b){const c=b.center;d.set(c,0,0,0);for(var e=0;e<a.length;++e)d.add(c,c,a[e]);d.scale(c,c,1/a.length);e=0;for(let h=0;h<a.length;++h)e=Math.max(e,d.squaredDistance(c,a[h]));b.radius=Math.sqrt(e)};f.compareSets=function(a,b){if(a===b)return!0;if(a.size!==b.size)return!1;for(let c=0;c<a.size;++c)if(!b.has(a[c]))return!1;return!0};f.fitHemisphere=function(a,b=null,c=!0){const e=(v,q)=>{if(0===q[0]&&0===q[1]&&0===q[2])return!1;for(let t=
0;t<v.length;++t)if(-1E-6>d.dot(q,v[t]))return!1;return!0};if(0===a.length)return!1;if(1===a.length)return b&&d.copy(b,a[0]),!0;d.set(g,0,0,0);for(var h=0;h<a.length;++h)d.add(g,g,a[h]);d.normalize(g,g);if(e(a,g))return b&&d.copy(b,g),!0;if(!c)return!1;for(c=0;c<a.length;++c)for(h=0;h<a.length;++h)if(c!==h&&(d.cross(g,a[c],a[h]),d.normalize(g,g),e(a,g)))return b&&d.copy(b,g),!0;return!1};f.planePointDistance=function(a,b){return d.dot(a,b)+a[3]};f.segmentLengthEuclidean=function(a,b,c){return m.projectPointToVector(a,
k,c)&&m.projectPointToVector(b,l,c)?d.distance(k,l):0};f.segmentLengthGeodesic=function(a,b){if(!m.projectPointToWGS84ComparableLonLat(a,k)||!m.projectPointToWGS84ComparableLonLat(b,l))return 0;a={distance:null};u.inverseGeodeticSolver(a,[k[0],k[1]],[l[0],l[1]]);return a.distance};f.segmentLengthGeodesicVector=function(a,b,c){const e={distance:null};u.inverseGeodeticSolver(e,[a[0],a[1]],[b[0],b[1]],c);return e.distance};f.tangentFrame=function(a,b,c){Math.abs(a[0])>Math.abs(a[1])?d.set(b,0,1,0):d.set(b,
1,0,0);d.cross(c,a,b);d.normalize(b,b);d.cross(b,c,a);d.normalize(c,c)};f.triangleAreaEuclidean=function(a,b,c){return.5*Math.abs((b[0]-a[0])*(c[1]-a[1])-(b[1]-a[1])*(c[0]-a[0]))};f.triangleAreaGeodesic=function(a,b,c,e){if(!m.projectVectorToWGS84ComparableLonLat(a,e,k)||!m.projectVectorToWGS84ComparableLonLat(b,e,l)||!m.projectVectorToWGS84ComparableLonLat(c,e,r))return 0;p.setPoint(0,0,k);p.setPoint(0,1,l);p.setPoint(0,2,r);return Math.abs(w.geodesicArea(p,"square-meters"))};Object.defineProperty(f,
"__esModule",{value:!0})});