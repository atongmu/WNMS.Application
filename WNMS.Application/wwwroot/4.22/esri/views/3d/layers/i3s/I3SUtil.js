// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../request ../../../../core/arrayUtils ../../../../core/Error ../../../../core/has ../../../../core/maybe ../../../../core/promiseUtils ../../../../core/typedArrayUtil ../../../../chunks/mat3 ../../../../chunks/mat3f64 ../../../../chunks/mat4 ../../../../chunks/mat4f64 ../../../../chunks/quat ../../../../chunks/quatf32 ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../../../../chunks/vec4f64 ../../../../geometry/projection ../../../../geometry/projectionEllipsoid ../../../../geometry/SpatialReference ../../../../geometry/support/aaBoundingRect ../../../../geometry/support/spatialReferenceUtils ../../../../rest/support/Query ./I3SBinaryReader ./I3SProjectionUtil ../support/edgeUtils ../support/symbolColorUtils ../../support/orientedBoundingBox".split(" "),
function(m,S,oa,H,v,pa,x,qa,T,ra,sa,ta,U,D,ua,p,V,M,w,C,W,A,X,va,wa,xa,Y,ya,E){function N(a){return a&&parseInt(a.substring(a.lastIndexOf("/")+1,a.length),10)}function Z(a,b){var c=b[0],d=b[1];b=b[3];const e=a[0]-c;c-=a[2];const g=a[1]-d;a=d-a[3];d=Math.max(e,c,0);const f=Math.max(g,a,0);d=d*d+f*f;return d>b*b?0:0<d?1:-Math.max(e,c,g,a)>b?3:2}function aa(a,b,c){const d=[],e=c&&c.missingFields;c=c&&c.originalFields;for(const g of a){a=g.toLowerCase();let f=!1;for(const h of b)if(a===h.name.toLowerCase()){d.push(h.name);
f=!0;c&&c.push(g);break}!f&&e&&e.push(g)}return d}function O(){O=S._asyncToGenerator(function*(a,b,c,d,e){if(0===b.length)return[];const g=a.attributeStorageInfo;if(x.isSome(a.associatedLayer))try{return yield za(a.associatedLayer,b,c,d)}catch(f){if(a.associatedLayer.loaded)throw f;}if(g){b=Aa(b,c,e);if(x.isNone(b))throw new v("scenelayer:features-not-loaded","Tried to query attributes for unloaded features");const f=a.parsedUrl.path;a=yield Promise.all(b.map(h=>Ba(f,g,h.node,h.indices,d).then(r=>
{for(let n=0;n<h.graphics.length;n++){const t=h.graphics[n],F=r[n];if(t.attributes)for(const k in t.attributes)k in F||(F[k]=t.attributes[k]);t.attributes=F}return h.graphics})));return H.flatten(a)}throw new v("scenelayer:no-attribute-source","This scene layer does not have a source for attributes available");});return O.apply(this,arguments)}function Aa(a,b,c){const d=new Map,e=[];c=c();for(const h of a){var g=h.attributes[b];for(var f=0;f<c.length;f++){a=c[f];const r=a.featureIds.indexOf(g);if(0<=
r){g=d.get(a.node);g||(g={node:a.node,indices:[],graphics:[]},e.push(g),d.set(a.node,g));g.indices.push(r);for(g.graphics.push(h);0<f;f--)c[f]=c[f-1];c[0]=a;break}}}return e}function za(a,b,c,d){return P.apply(this,arguments)}function P(){P=S._asyncToGenerator(function*(a,b,c,d){b.sort((f,h)=>f.attributes[c]-h.attributes[c]);var e=b.map(f=>f.attributes[c]);const g=[];d=aa(d,a.fields,{originalFields:g});a=yield ba(a,e,d);for(e=0;e<b.length;e++){const f=b[e],h=a[e],r={};if(f.attributes)for(const n in f.attributes)r[n]=
f.attributes[n];for(let n=0;n<g.length;n++)r[g[n]]=h[d[n]];f.attributes=r}return b});return P.apply(this,arguments)}function ba(a,b,c){var d=a.capabilities.query.maxRecordCount;if(null!=d&&b.length>d)return d=H.splitIntoChunks(b,d),Promise.all(d.map(e=>ba(a,e,c))).then(H.flatten);d=new va({objectIds:b,outFields:c,orderByFields:[a.objectIdField]});return a.queryFeatures(d).then(e=>{if(e&&e.features&&e.features.length===b.length)return e.features.map(g=>g.attributes);throw new v("scenelayer:feature-not-in-associated-layer",
"Feature not found in associated feature layer");})}function Ba(a,b,c,d,e){const g=[];for(const f of b)f&&-1!==e.indexOf(f.name)&&g.push({url:`${a}/nodes/${c.resources.attributes}/attributes/${f.key}/0`,storageInfo:f});return qa.eachAlways(g.map(f=>oa(f.url,{responseType:"array-buffer"}).then(h=>wa.readBinaryAttribute(f.storageInfo,h.data)))).then(f=>{const h=[];for(const r of d){const n={};for(let t=0;t<f.length;t++)null!=f[t].value&&(n[g[t].storageInfo.name]=ca(f[t].value,r));h.push(n)}return h})}
function ca(a,b){if(!a)return null;b=a[b];return T.isInt16Array(a)?-32768===b?null:b:T.isInt32Array(a)?b===Ca?null:b:b!==b?null:b}function da(a){var b=a.store.indexCRS||a.store.geographicCRS;const c=void 0===b?a.store.indexWKT:void 0;if(c)if(a.spatialReference){if(c!==a.spatialReference.wkt)throw new v("layerview:store-spatial-reference-wkt-index-incompatible","The indeWKT of the scene layer store does not match the WKT of the layer spatial reference",{});}else throw new v("layerview:no-store-spatial-reference-wkt-index-and-no-layer-spatial-reference",
"Found indeWKT in the scene layer store but no layer spatial reference",{});b=b?new W(N(b)):a.spatialReference;return b.equals(a.spatialReference)?a.spatialReference:b}function ea(a){var b=a.store.vertexCRS||a.store.projectedCRS;const c=void 0===b?a.store.vertexWKT:void 0;if(c)if(a.spatialReference){if(c!==a.spatialReference.wkt)throw new v("layerview:store-spatial-reference-wkt-vertex-incompatible","The vertexWKT of the scene layer store does not match the WKT of the layer spatial reference",{});
}else throw new v("layerview:no-store-spatial-reference-wkt-vertex-and-no-layer-spatial-reference","Found vertexWKT in the scene layer store but no layer spatial reference",{});b=b?new W(N(b)):a.spatialReference;return b.equals(a.spatialReference)?a.spatialReference:b}function I(a,b,c){if(!w.canProjectWithoutEngine(a,b))throw new v("layerview:spatial-reference-incompatible","The spatial reference of this scene layer is incompatible with the spatial reference of the view",{});if("local"===c&&!fa(a,
b))throw new v("layerview:spatial-reference-incompatible","The spatial reference of this scene layer is incompatible with the spatial reference of the view",{});}function fa(a,b){return a.equals(b)||a.isWGS84&&b.isWebMercator||a.isWebMercator&&b.isWGS84}function ha(a,b,c){const d=da(a);a=ea(a);I(d,b,c);I(a,b,c)}function ia(a,b,c,d,e=0){if(d===C.getSphericalPCPF(d))if(b.isGeographic){var g=C.getReferenceEllipsoid(b);d=1+Math.max(0,e)/(g.radius+a.center[2]);p.set(c.center,a.center[0],a.center[1],a.center[2]+
e);w.projectBuffer(c.center,b,0,c.center,C.getSphericalPCPF(b),0,1);D.copy(c.quaternion,a.quaternion);D.conjugate(B,a.quaternion);p.set(l,0,0,1);p.transformQuat(l,l,B);p.set(l,a.halfSize[0]*Math.abs(l[0]),a.halfSize[1]*Math.abs(l[1]),a.halfSize[2]*Math.abs(l[2]));p.scale(l,l,g.inverseFlattening);p.add(c.halfSize,a.halfSize,l);p.scale(c.halfSize,c.halfSize,d)}else{{E.corners(a,Q);p.set(c.center,a.center[0],a.center[1],a.center[2]+e);w.computeTranslationToOriginAndRotation(b,c.center,u,C.getSphericalPCPF(b));
p.set(c.center,u[12],u[13],u[14]);d=2*Math.sqrt(1+u[0]+u[5]+u[10]);B[0]=(u[6]-u[9])/d;B[1]=(u[8]-u[2])/d;B[2]=(u[1]-u[4])/d;B[3]=.25*d;D.multiply(c.quaternion,B,a.quaternion);D.conjugate(B,c.quaternion);let f=d=a=0;for(g of Q)g[2]+=e,w.projectBuffer(g,b,0,g,C.getSphericalPCPF(b),0,1),p.sub(l,g,c.center),p.transformQuat(l,l,B),a=Math.max(a,Math.abs(l[0])),d=Math.max(d,Math.abs(l[1])),f=Math.max(f,Math.abs(l[2]));p.set(c.halfSize,a,d,f)}}else b.isWGS84&&(d.isWebMercator||X.isPlateCarree(d))?(p.copy(y,
a.center),y[2]+=e,e=C.getSphericalPCPF(d),w.projectBuffer(y,b,0,y,e,0,1),ja(e,a,y,d,c)):b.isWebMercator&&X.isPlateCarree(d)?(p.copy(y,a.center),y[2]+=e,ja(b,a,y,d,c)):a===c?(c.center[2]+=e,w.projectBuffer(c.center,b,0,c.center,d,0,1)):(p.set(c.center,a.center[0],a.center[1],a.center[2]+e),w.projectBuffer(c.center,b,0,c.center,d,0,1),D.copy(c.quaternion,a.quaternion),p.copy(c.halfSize,a.halfSize))}function ja(a,b,c,d,e){const g=ra.fromQuat(Da,b.quaternion);for(let h=0;8>h;++h){for(var f=0;3>f;++f)ka[f]=
b.halfSize[f]*(0!==(h&1<<f)?-1:1);for(f=0;3>f;++f){let r=c[f];for(let n=0;3>n;++n)r+=ka[n]*g[3*n+f];J[3*h+f]=r}}w.projectBuffer(J,a,0,J,d,0,8);E.compute(Ea,e)}const G=A.create(),Fa=M.create(),Ca=-(2**31);M=Y.createSolidEdgeMaterial({color:[0,0,0,0],opacity:0});let la=function(){this.material=this.edgeMaterial=null;this.castShadows=!0};const J=new Float64Array(24),Ea={data:J,size:3},ka=V.create(),y=V.create(),Da=sa.create(),u=U.create(),B=ua.create(),Q=[[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,
0],[0,0,0],[0,0,0]],R=A.create(),K=A.create(),ma=E.create(),l=[0,0,0],na={data:Array(72),size:3},L=U.create();m.SymbolInfo=la;m.addWraparound=function(a,b){return(a|0)+(b|0)|0};m.checkPointCloudLayerCompatibleWithView=function(a,b){I(a.spatialReference,b.spatialReference,b.viewingMode)};m.checkPointCloudLayerValid=function(a){var b;(b=null==a.store||null==a.store.defaultGeometrySchema)||(a=a.store.defaultGeometrySchema,b=!!(null==a.geometryType||"points"!==a.geometryType||null!=a.topology&&"PerAttributeArray"!==
a.topology||null!=a.encoding&&""!==a.encoding&&"lepcc-xyz"!==a.encoding||null==a.vertexAttributes||null==a.vertexAttributes.position));if(b)throw new v("pointcloud:unsupported-geometry-schema","The geometry schema of this point cloud scene layer is not supported.",{});};m.checkSceneLayerCompatibleWithView=function(a,b){ha(a,b.spatialReference,b.viewingMode)};m.checkSceneLayerValid=function(a){var b;(b=null==a.store||null==a.store.defaultGeometrySchema)||(b=a.store.defaultGeometrySchema,b=!!(null!=
b.geometryType&&"triangles"!==b.geometryType||null!=b.topology&&"PerAttributeArray"!==b.topology||null==b.vertexAttributes||null==b.vertexAttributes.position));if(b)throw new v("scenelayer:unsupported-geometry-schema","The geometry schema of this scene layer is not supported.",{url:a.parsedUrl.path});};m.checkSpatialReference=I;m.checkSpatialReferences=ha;m.computeVisibilityObb=function(a,b,c,d,e,g){if(!g||0===g.length||x.isNone(b))return null;const f=xa.computeGlobalTransformation(a.mbs,e,c,b);ta.invert(L,
f);let h;const r=()=>{if(!h)if(h=Q,A.empty(K),x.isSome(a.serviceObb)){ia(a.serviceObb,c,ma,b,e);E.corners(ma,h);for(var k of h)p.transformMat4(k,k,L),A.expandPointInPlace(K,k)}else{var q=a.mbs;k=q[3];w.projectVectorToVector(q,c,l,b);p.transformMat4(l,l,L);l[2]+=e;for(q=0;8>q;++q){const z=h[q];p.copy(z,[l[0]+(q&1?k:-k),l[1]+(q&2?k:-k),l[2]+(q&4?k:-k)]);A.expandPointInPlace(K,z)}}};let n=Infinity,t=-Infinity;const F=k=>{if("replace"===k.type&&(k=k.geometry,k.hasZ)){A.empty(R);var q=k.spatialReference||
d;k=k.rings.reduce((z,Ga)=>Ga.reduce((Ha,Ia)=>{w.projectVectorToVector(Ia,q,l,b);p.transformMat4(l,l,L);A.expandPointInPlace(R,l);return Math.min(l[2],Ha)},z),Infinity);r();A.intersects(K,R)&&(n=Math.min(n,k),t=Math.max(t,k))}};g.forEach(k=>F(k));if(Infinity===n)return null;g=(k,q,z)=>{p.transformMat4(l,z,f);k[q+0]=l[0];k[q+1]=l[1];k[q+2]=l[2];q+=24;z[2]=n;p.transformMat4(l,z,f);k[q+0]=l[0];k[q+1]=l[1];k[q+2]=l[2];q+=24;z[2]=t;p.transformMat4(l,z,f);k[q+0]=l[0];k[q+1]=l[1];k[q+2]=l[2]};for(let k=
0;8>k;++k)g(na.data,3*k,h[k]);return E.compute(na)};m.containsDraco=function(a){if(pa("disable-feature:i3s-draco")||!a)return!1;for(const c of a)for(const d of c.geometryBuffers){var b;if("draco"===(null==(b=d.compressedAttributes)?void 0:b.encoding))return!0}return!1};m.extractWkid=N;m.filterInPlace=function(a,b,c){let d=0,e=0;for(let g=0;g<b.length&&d<a.length;g++)a[d]===b[g]&&(c(g)&&(a[e]=a[d],e++),d++);a.length=e};m.findFieldsCaseInsensitive=aa;m.findIntersectingNodes=function(a,b,c,d,e,g){e.traverse(c,
f=>{var h=f.mbs;b!==d&&(h=Fa,w.projectBoundingSphere(f.mbs,d,h,b));h=Z(a,h);if(0===h)return!1;g(f,h);return!0})};m.getCacheKeySuffix=function(a,b){return x.isNone(b)?"@null":b===C.getSphericalPCPF(b)?"@ECEF":a.equals(b)?"":null!=b.wkid?"@"+b.wkid:null};m.getCachedAttributeValue=ca;m.getClipRect=function(a,b){if(0===b.rotationScale[1]&&0===b.rotationScale[2]&&0===b.rotationScale[3]&&0===b.rotationScale[5]&&0===b.rotationScale[6]&&0===b.rotationScale[7])return G[0]=(a[0]-b.position[0])/b.rotationScale[0],
G[1]=(a[1]-b.position[1])/b.rotationScale[4],G[2]=(a[2]-b.position[0])/b.rotationScale[0],G[3]=(a[3]-b.position[1])/b.rotationScale[4],G};m.getIndexCrs=da;m.getSymbolInfo=function(a){const b=new la;var c=!1;let d=!1;for(const g of a.symbolLayers.items)if("fill"===g.type&&g.enabled){var e=g.material;a=g.edges;x.isSome(e)&&!c&&(c=e.color,e=ya.parseColorMixMode(e.colorMixMode),x.isSome(c)?b.material={color:[c.r/255,c.g/255,c.b/255],alpha:c.a,colorMixMode:e}:b.material={color:[1,1,1],alpha:1,colorMixMode:1},
b.castShadows=g.castShadows,c=!0);x.isSome(a)&&!d&&(b.edgeMaterial=Y.createMaterialFromEdges(a,{}),d=!0)}b.material||(b.material={color:[1,1,1],alpha:1,colorMixMode:1});return b};m.getVertexCrs=ea;m.intersectBoundingBoxWithMbs=function(a,b){var c=b[0],d=b[1],e=b[2];b=b[3];const g=a[0]-c;c-=a[3];const f=a[1]-d;d-=a[4];const h=a[2]-e;a=e-a[5];e=Math.max(g,c,0);const r=Math.max(f,d,0),n=Math.max(h,a,0);e=e*e+r*r+n*n;return e>b*b?0:0<e?1:-Math.max(g,c,f,d,h,a)>b?3:2};m.intersectBoundingRectWithMbs=Z;
m.isSupportedLocalModeProjection=fa;m.objectIdFilter=function(a,b,c){let d=0,e=0;for(;d<c.length;)0<=H.binaryIndexOf(a,c[d])===b&&(c[e]=c[d],e++),d++;c.length=e};m.rendererNeedsTextures=function(a){if(null==a||"simple"!==a.type&&"class-breaks"!==a.type&&"unique-value"!==a.type||("unique-value"===a.type||"class-breaks"===a.type)&&null==a.defaultSymbol)return!0;a=a.getSymbols();if(0===a.length)return!0;for(const b of a){if("mesh-3d"!==b.type||0===b.symbolLayers.length)return!0;for(const c of b.symbolLayers.items)if("fill"!==
c.type||x.isNone(c.material)||x.isNone(c.material.color)||"replace"!==c.material.colorMixMode)return!0}return!1};m.transformObb=ia;m.transparentEdgeMaterial=M;m.whenGraphicAttributes=function(a,b,c,d,e){return O.apply(this,arguments)};Object.defineProperty(m,"__esModule",{value:!0})});