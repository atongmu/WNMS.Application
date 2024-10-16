// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../chunks/_rollupPluginBabelHelpers ../request ../core/Error ../core/maybe ../core/object ../core/urlUtils ./utils ./support/NetworkServiceDescription ./support/RouteResultsContainer".split(" "),function(m,r,p,A,t,E,u,q,F,G){function v(){v=r._asyncToGenerator(function*(a,c,b){if(!a)throw new A("network-service:missing-url","Url to Network service is missing");var d=q.asValidOptions({f:"json",token:c},b);({data:d}=yield p(a,d));d.supportedTravelModes||(d.supportedTravelModes=[]);for(let g=
0;g<d.supportedTravelModes.length;g++)d.supportedTravelModes[g].id||(d.supportedTravelModes[g].id=d.supportedTravelModes[g].itemId);a=10.4<=d.currentVersion?H(a,c,b):I(a,b);const {defaultTravelMode:e,supportedTravelModes:k}=yield a;d.defaultTravelMode=e;d.supportedTravelModes=k;return F.fromJSON(d)});return v.apply(this,arguments)}function I(a,c){return w.apply(this,arguments)}function w(){w=r._asyncToGenerator(function*(a,c){var b,d,e=q.asValidOptions({f:"json"},c);({data:a}=yield p(a.replace(/\/rest\/.*$/i,
"/info"),e));if(!a||!a.owningSystemUrl)return{supportedTravelModes:[],defaultTravelMode:null};({owningSystemUrl:a}=a);const k=u.removeTrailingSlash(a)+"/sharing/rest/portals/self";({data:e}=yield p(k,e));e=E.getDeepValue("helperServices.routingUtilities.url",e);if(!e)return{supportedTravelModes:[],defaultTravelMode:null};a=q.parseUrl(a);a={f:"json",serviceName:/\/solve$/i.test(a.path)?"Route":/\/solveclosestfacility$/i.test(a.path)?"ClosestFacility":"ServiceAreas"};c=q.asValidOptions(a,c);e=u.removeTrailingSlash(e)+
"/GetTravelModes/execute";a=yield p(e,c);c=[];e=null;if(null!=a&&null!=(b=a.data)&&null!=(d=b.results)&&d.length){b=a.data.results;for(const l of b)if("supportedTravelModes"===l.paramName){var g;if(null!=(g=l.value)&&g.features)for(const {attributes:n}of l.value.features)n&&(b=JSON.parse(n.TravelMode),c.push(b))}else"defaultTravelMode"===l.paramName&&(e=l.value)}return{supportedTravelModes:c,defaultTravelMode:e}});return w.apply(this,arguments)}function H(a,c,b){return x.apply(this,arguments)}function x(){x=
r._asyncToGenerator(function*(a,c,b){try{const d=q.asValidOptions({f:"json",token:c},b),e=u.removeTrailingSlash(a)+"/retrieveTravelModes",{data:{supportedTravelModes:k,defaultTravelMode:g}}=yield p(e,d);return{supportedTravelModes:k,defaultTravelMode:g}}catch(d){throw new A("network-service:retrieveTravelModes","Could not get to the NAServer's retrieveTravelModes.",{error:d});}});return x.apply(this,arguments)}m.collectGeometries=function(a,c,b,d){d[b]=[c.length,c.length+a.length];a.forEach(e=>{c.push(e.geometry)})};
m.dropZValuesOffInputGeometry=function(a,c){for(let b=0;b<c.length;b++){const d=a[c[b]];if(d&&d.length)for(const e of d)e.z=void 0}console.log("The remote Network Analysis service is powered by a network dataset which is not Z-aware.\nZ-coordinates of the input geometry are ignored.")};m.fetchServiceDescription=function(a,c,b){return v.apply(this,arguments)};m.handleSolveResponse=function(a){const c=[],b=[],{directions:d=[],routes:{features:e=[],spatialReference:k=null}={},stops:{features:g=[],spatialReference:l=
null}={},barriers:n,polygonBarriers:y,polylineBarriers:z,messages:J}=a.data;let B=!0,h,C;const D=e&&k||g&&l||n&&n.spatialReference||y&&y.spatialReference||z&&z.spatialReference;d.forEach(f=>{c.push(h=f.routeName);b[h]={directions:f}});e.forEach(f=>{-1===c.indexOf(h=f.attributes.Name)&&(c.push(h),b[h]={});t.isSome(f.geometry)&&(f.geometry.spatialReference=D);b[h].route=f});g.forEach(f=>{C=f.attributes;-1===c.indexOf(h=C.RouteName||"esri.tasks.RouteTask.NULL_ROUTE_NAME")&&(c.push(h),b[h]={});"esri.tasks.RouteTask.NULL_ROUTE_NAME"!==
h&&(B=!1);t.isSome(f.geometry)&&(f.geometry.spatialReference=D);null==b[h].stops&&(b[h].stops=[]);b[h].stops.push(f)});0<g.length&&!0===B&&(b[c[0]].stops=b["esri.tasks.RouteTask.NULL_ROUTE_NAME"].stops,delete b["esri.tasks.RouteTask.NULL_ROUTE_NAME"],c.splice(c.indexOf("esri.tasks.RouteTask.NULL_ROUTE_NAME"),1));a=c.map(f=>{b[f].routeName="esri.tasks.RouteTask.NULL_ROUTE_NAME"===f?null:f;return b[f]});return G.fromJSON({routeResults:a,pointBarriers:n,polygonBarriers:y,polylineBarriers:z,messages:J})};
m.isInputGeometryZAware=function(a,c){for(let b=0;b<c.length;b++){const d=a[c[b]];if(d&&d.length)for(const e of d)if(t.isSome(e)&&e.hasZ)return!0}return!1};Object.defineProperty(m,"__esModule",{value:!0})});