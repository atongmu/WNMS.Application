// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ./aaBoundingRect ./boundsUtils ./intersectsBase ./jsonUtils ./normalizeUtilsCommon ./spatialReferenceUtils".split(" "),function(x,C,D,y,q,m,z){function E(a,c){const d=m.getGeometryParts(a);for(const b of d)for(const e of b)e[0]+=c;return a}function F(a,c){if(!c)return a;const d=G(a,c).map(b=>b.extent);return 2>d.length?d[0]||a:2<d.length?(a.xmin=c.valid[0],a.xmax=c.valid[1],a):{rings:d.map(b=>[[b.xmin,b.ymin],[b.xmin,b.ymax],[b.xmax,b.ymax],[b.xmax,b.ymin],[b.xmin,b.ymin]])}}function A(a,
c,d){if(Array.isArray(a)){var b=a[0];b>c?(d=m.offsetMagnitude(b,c),a[0]=b+-2*d*c):b<d&&(c=m.offsetMagnitude(b,d),a[0]=b+-2*c*d)}else b=a.x,b>c?(d=m.offsetMagnitude(b,c),a.x+=-2*d*c):b<d&&(c=m.offsetMagnitude(b,d),a.x+=-2*c*d);return a}function G(a,c){const d=[],{ymin:b,ymax:e}=a;var f=a.xmax-a.xmin,h=a.xmin,g=a.xmax;const [k,t]=c.valid;var l=B(a.xmin,c);var n=l.x;const p=l.frameId;l=B(a.xmax,c);a=l.x;l=l.frameId;c=n===a&&0<f;if(f>2*t){f={xmin:h<g?n:a,ymin:b,xmax:t,ymax:e};h={xmin:k,ymin:b,xmax:h<
g?a:n,ymax:e};g={xmin:0,ymin:b,xmax:t,ymax:e};n={xmin:k,ymin:b,xmax:0,ymax:e};a=[];c=[];u(f,g)&&a.push(p);u(f,n)&&c.push(p);u(h,g)&&a.push(l);u(h,n)&&c.push(l);for(let v=p+1;v<l;v++)a.push(v),c.push(v);d.push({extent:f,frameIds:[p]},{extent:h,frameIds:[l]},{extent:g,frameIds:a},{extent:n,frameIds:c})}else n>a||c?d.push({extent:{xmin:n,ymin:b,xmax:t,ymax:e},frameIds:[p]},{extent:{xmin:k,ymin:b,xmax:a,ymax:e},frameIds:[l]}):d.push({extent:{xmin:n,ymin:b,xmax:a,ymax:e},frameIds:[p]});return d}function B(a,
c){const [d,b]=c.valid;c=2*b;var e=0;a>b?(e=Math.ceil(Math.abs(a-b)/c),a-=e*c):a<d&&(e=Math.ceil(Math.abs(a-d)/c),a+=e*c,e=-e);return{x:a,frameId:e}}function u(a,c){const {xmin:d,ymin:b,xmax:e,ymax:f}=c;return w(a,d,b)&&w(a,d,f)&&w(a,e,f)&&w(a,e,b)}function w(a,c,d){return c>=a.xmin&&c<=a.xmax&&d>=a.ymin&&d<=a.ymax?!0:!1}let H=function(){function a(){}a.prototype.cut=function(c,d){let b;if(c.rings)this.closed=!0,b=c.rings,this.minPts=4;else if(c.paths)this.closed=!1,b=c.paths,this.minPts=2;else return null;
const e=b.length;d*=-2;for(let f=0;f<e;f++){const h=b[f];if(h&&h.length>=this.minPts){const g=[];for(const k of h)g.push([k[0]+d,k[1]]);b.push(g)}}this.closed?c.rings=b:c.paths=b;return c};return a}();const r=C.create();x.normalizeCentralMeridianSync=function(a){if(!a)return null;let c=null;var d=a.spatialReference,b=z.getInfo(d);if(!b)return"toJSON"in a?a.toJSON():a;var e=z.isWebMercator(d)?102100:4326;const f=m.cutParams[e].maxX,h=m.cutParams[e].minX;d=m.cutParams[e].plus180Line;e=m.cutParams[e].minus180Line;
let g;a="toJSON"in a?a.toJSON():a;if(q.isPoint(a))g=A(a,f,h);else if(q.isMultipoint(a))a.points=a.points.map(k=>A(k,f,h)),g=a;else if(q.isExtent(a))g=F(a,b);else if(q.isPolygon(a)||q.isPolyline(a)){D.getBoundsXY(r,a);b={xmin:r[0],ymin:r[1],xmax:r[2],ymax:r[3]};const k=2*m.offsetMagnitude(b.xmin,h)*f;a=0===k?a:E(a,k);b.xmin+=k;b.xmax+=k;y.extentIntersectsPolyline(b,d)&&b.xmax!==f?c=a:y.extentIntersectsPolyline(b,e)&&b.xmin!==h?c=a:g=a}else g=a;return null!==c?(new H).cut(c,f):g};Object.defineProperty(x,
"__esModule",{value:!0})});