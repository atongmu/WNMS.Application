/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{o as e,y as t,b as r,i,u as s,p as a,E as o}from"../core/lang.js";import{b as n,k as l,A as d,c,e as h,s as p,d as u,f,g as m,m as g,l as v,E as y,j as _,W as b,n as S,i as x}from"./mathUtils.js";import{c as w,u as C}from"./vec2.js";import{projectBuffer as T,computeTranslationToOriginAndRotation as R,lonLatToWebMercatorComparable as E}from"../geometry/projection.js";import{g as D}from"./projectionEllipsoid.js";import{p as O}from"./triangulationUtils.js";import{O as P,Z as L}from"./vec4f64.js";import{n as A}from"./compilerUtils.js";import{f as z,m as M,a as I,c as F,o as W,i as j,t as N}from"./mat4.js";import{c as V,I as H}from"./mat4f64.js";import{i as U,a as G,g as B}from"./ElevationProvider.js";import{u as q,o as k,W as $,O as Z,D as J,P as Q,a as Y,S as X}from"./ScreenSpacePass.js";import{_ as K}from"./tslib.es6.js";import ee,{g as te,s as re}from"../core/Accessor.js";import{E as ie}from"./Evented.js";import se from"../core/Handles.js";import{L as ae}from"./Logger.js";import{s as oe}from"./ensureType.js";import{P as ne,M as le}from"../core/scheduling.js";import{init as de}from"../core/watchUtils.js";import{property as ce}from"../core/accessorSupport/decorators/property.js";import{subclass as he}from"../core/accessorSupport/decorators/subclass.js";import{f as pe,c as ue}from"./vec2f64.js";import{a as fe,o as me,c as ge}from"./aaBoundingRect.js";import{c as ve}from"./vec2f32.js";import{V as ye,B as _e,v as be}from"./Program.js";import{F as Se}from"./FramebufferObject.js";import{T as xe}from"./Texture.js";import{C as we}from"./Camera.js";import{d as Ce,e as Te,s as Re}from"./Util2.js";import{g as Ee,f as De,S as Oe,O as Pe,R as Le,A as Ae,q as ze,C as Me,z as Ie,p as Fe,a as We,b as je,c as Ne,P as Ve,h as He,B as Ue,L as Ge,Q as Be,U as qe,V as ke,E as $e,W as Ze,G as Je,X as Qe,Y as Ye,Z as Xe,_ as Ke,$ as et,a0 as tt,a1 as rt,a2 as it,a3 as st,T as at,r as ot,D as nt,a4 as lt,N as dt,d as ct}from"./StencilUtils.js";import{d as ht}from"./screenUtils.js";import{c as pt,d as ut,f as ft,a as mt}from"./lineSegment.js";import{c as gt,f as vt,s as yt,n as _t}from"./plane.js";import{n as bt}from"./InterleavedLayout.js";import{c as St}from"./geometryDataUtils.js";import{c as xt}from"./quatf64.js";import{f as wt}from"./vec3f32.js";import{i as Ct,c as Tt}from"./utils14.js";import{P as Rt,b as Et,a as Dt,F as Ot,R as Pt,c as Lt}from"./PhysicallyBasedRendering.glsl.js";import{m as At,e as zt,O as Mt,g as It,c as Ft,i as Wt,d as jt,h as Nt,j as Vt,k as Ht,s as Ut}from"./OrderIndependentTransparency.js";import{b as Gt}from"./Intersector.js";import{g as Bt}from"./glUtil.js";import{b as qt}from"./MemCache.js";import{p as kt}from"./floatRGBA.js";import{I as $t,T as Zt,n as Jt}from"./Scheduler.js";function Qt(e){e.fragment.code.add(Ee`const float GAMMA = 2.2;
const float INV_GAMMA = 0.4545454545;
vec4 delinearizeGamma(vec4 color) {
return vec4(pow(color.rgb, vec3(INV_GAMMA)), color.w);
}
vec3 linearizeGamma(vec3 color) {
return pow(color, vec3(GAMMA));
}`)}class Yt{constructor(e=n()){this.intensity=e}}class Xt{constructor(e=n(),t=l(.57735,.57735,.57735)){this.intensity=e,this.direction=t}}class Kt{constructor(e=n(),t=l(.57735,.57735,.57735),r=!0){this.intensity=e,this.direction=t,this.castShadows=r}}class er{constructor(){this.r=[0],this.g=[0],this.b=[0]}}let tr=class extends ee{constructor(){super(...arguments),this.SCENEVIEW_HITTEST_RETURN_INTERSECTOR=!1,this.SCENEVIEW_LOCKING_LOG=!1,this.HIGHLIGHTS_GRID_OPTIMIZATION_ENABLED=!0,this.HIGHLIGHTS_PROFILE_TO_CONSOLE=!1,this.DECONFLICTOR_SHOW_VISIBLE=!1,this.DECONFLICTOR_SHOW_INVISIBLE=!1,this.DECONFLICTOR_SHOW_GRID=!1,this.LABELS_SHOW_BORDER=!1,this.OVERLAY_DRAW_DEBUG_TEXTURE=!1,this.OVERLAY_SHOW_CENTER=!1,this.SHOW_POI=!1,this.TESTS_DISABLE_OPTIMIZATIONS=!1,this.TESTS_DISABLE_FAST_UPDATES=!1,this.DRAW_MESH_GEOMETRY_NORMALS=!1,this.FEATURE_TILE_FETCH_SHOW_TILES=!1,this.FEATURE_TILE_TREE_SHOW_TILES=!1,this.TERRAIN_TILE_TREE_SHOW_TILES=!1,this.I3S_TREE_SHOW_TILES=!1,this.I3S_SHOW_MODIFICATIONS=!1,this.LOD_INSTANCE_RENDERER_DISABLE_UPDATES=!1,this.LOD_INSTANCE_RENDERER_COLORIZE_BY_LEVEL=!1,this.EDGES_SHOW_HIDDEN_TRANSPARENT_EDGES=!1}};K([ce()],tr.prototype,"SCENEVIEW_HITTEST_RETURN_INTERSECTOR",void 0),K([ce()],tr.prototype,"SCENEVIEW_LOCKING_LOG",void 0),K([ce()],tr.prototype,"HIGHLIGHTS_GRID_OPTIMIZATION_ENABLED",void 0),K([ce()],tr.prototype,"HIGHLIGHTS_PROFILE_TO_CONSOLE",void 0),K([ce()],tr.prototype,"DECONFLICTOR_SHOW_VISIBLE",void 0),K([ce()],tr.prototype,"DECONFLICTOR_SHOW_INVISIBLE",void 0),K([ce()],tr.prototype,"DECONFLICTOR_SHOW_GRID",void 0),K([ce()],tr.prototype,"LABELS_SHOW_BORDER",void 0),K([ce()],tr.prototype,"OVERLAY_DRAW_DEBUG_TEXTURE",void 0),K([ce()],tr.prototype,"OVERLAY_SHOW_CENTER",void 0),K([ce()],tr.prototype,"SHOW_POI",void 0),K([ce()],tr.prototype,"TESTS_DISABLE_OPTIMIZATIONS",void 0),K([ce()],tr.prototype,"TESTS_DISABLE_FAST_UPDATES",void 0),K([ce()],tr.prototype,"DRAW_MESH_GEOMETRY_NORMALS",void 0),K([ce()],tr.prototype,"FEATURE_TILE_FETCH_SHOW_TILES",void 0),K([ce()],tr.prototype,"FEATURE_TILE_TREE_SHOW_TILES",void 0),K([ce()],tr.prototype,"TERRAIN_TILE_TREE_SHOW_TILES",void 0),K([ce()],tr.prototype,"I3S_TREE_SHOW_TILES",void 0),K([ce()],tr.prototype,"I3S_SHOW_MODIFICATIONS",void 0),K([ce()],tr.prototype,"LOD_INSTANCE_RENDERER_DISABLE_UPDATES",void 0),K([ce()],tr.prototype,"LOD_INSTANCE_RENDERER_COLORIZE_BY_LEVEL",void 0),K([ce()],tr.prototype,"EDGES_SHOW_HIDDEN_TRANSPARENT_EDGES",void 0),tr=K([he("esri.views.3d.support.DebugFlags")],tr);const rr=new tr;function ir(e,t,r,i,s,a,o,n,l,d,c){const h=pr[c.mode];let p,u,f=0;if(T(e,t,r,i,l.spatialReference,s,n))return h.requiresAlignment(c)?(f=h.applyElevationAlignmentBuffer(i,s,a,o,n,l,d,c),p=a,u=o):(p=i,u=s),T(p,l.spatialReference,u,a,d.spatialReference,o,n)?f:void 0}function sr(t,r,i,s,a){const o=(U(t)?t.z:G(t)?t.array[t.offset+2]:t[2])||0;switch(i.mode){case"on-the-ground":{const i=e(B(r,t,"ground"),0);return a.verticalDistanceToGround=0,a.sampledElevation=i,void(a.z=i)}case"relative-to-ground":{const n=e(B(r,t,"ground"),0),l=i.geometryZWithOffset(o,s);return a.verticalDistanceToGround=l,a.sampledElevation=n,void(a.z=l+n)}case"relative-to-scene":{const n=e(B(r,t,"scene"),0),l=i.geometryZWithOffset(o,s);return a.verticalDistanceToGround=l,a.sampledElevation=n,void(a.z=l+n)}case"absolute-height":{const n=i.geometryZWithOffset(o,s),l=e(B(r,t,"ground"),0);return a.verticalDistanceToGround=n-l,a.sampledElevation=l,void(a.z=n)}default:return A(i.mode),void(a.z=0)}}function ar(e,t,r,i){return sr(e,t,r,i,fr),fr.z}function or(e,t,r){return null==t||null==r?e.definedChanged:"on-the-ground"===t&&"on-the-ground"===r?e.staysOnTheGround:t===r||"on-the-ground"!==t&&"on-the-ground"!==r?hr.UPDATE:e.onTheGroundChanged}function nr(e){return"relative-to-ground"===e||"relative-to-scene"===e}function lr(e){return"absolute-height"!==e}function dr(e,t,r,i,s){sr(t,r,s,i,fr),q(e,fr.verticalDistanceToGround);const a=fr.sampledElevation,o=z(ur,e.transformation);mr[0]=t.x,mr[1]=t.y,mr[2]=fr.z;return R(t.spatialReference,mr,o,i.spatialReference)?e.transformation=o:console.warn("Could not locate symbol object properly, it might be misplaced"),a}class cr{constructor(){this.verticalDistanceToGround=0,this.sampledElevation=0,this.z=0}}var hr;!function(e){e[e.NONE=0]="NONE",e[e.UPDATE=1]="UPDATE",e[e.RECREATE=2]="RECREATE"}(hr||(hr={}));const pr={"absolute-height":{applyElevationAlignmentBuffer:function(e,t,r,i,s,a,o,n){const l=n.calculateOffsetRenderUnits(o),d=n.featureExpressionInfoContext;t*=3,i*=3;for(let a=0;a<s;++a){const s=e[t+0],a=e[t+1],o=e[t+2];r[i+0]=s,r[i+1]=a,r[i+2]=null==d?o+l:l,t+=3,i+=3}return 0},requiresAlignment:function(e){const t=e.meterUnitOffset,r=e.featureExpressionInfoContext;return 0!==t||null!=r}},"on-the-ground":{applyElevationAlignmentBuffer:function(t,r,i,s,a,o){let n=0;const l=o.spatialReference;r*=3,s*=3;for(let d=0;d<a;++d){const a=t[r+0],d=t[r+1],c=t[r+2],h=e(o.getElevation(a,d,c,l,"ground"),0);n+=h,i[s+0]=a,i[s+1]=d,i[s+2]=h,r+=3,s+=3}return n/a},requiresAlignment:()=>!0},"relative-to-ground":{applyElevationAlignmentBuffer:function(t,r,i,s,a,o,n,l){let d=0;const c=l.calculateOffsetRenderUnits(n),h=l.featureExpressionInfoContext,p=o.spatialReference;r*=3,s*=3;for(let n=0;n<a;++n){const a=t[r+0],n=t[r+1],l=t[r+2],u=e(o.getElevation(a,n,l,p,"ground"),0);d+=u,i[s+0]=a,i[s+1]=n,i[s+2]=null==h?l+u+c:u+c,r+=3,s+=3}return d/a},requiresAlignment:()=>!0},"relative-to-scene":{applyElevationAlignmentBuffer:function(t,r,i,s,a,o,n,l){let d=0;const c=l.calculateOffsetRenderUnits(n),h=l.featureExpressionInfoContext,p=o.spatialReference;r*=3,s*=3;for(let n=0;n<a;++n){const a=t[r+0],n=t[r+1],l=t[r+2],u=e(o.getElevation(a,n,l,p,"scene"),0);d+=u,i[s+0]=a,i[s+1]=n,i[s+2]=null==h?l+u+c:u+c,r+=3,s+=3}return d/a},requiresAlignment:()=>!0}},ur=V(),fr=new cr,mr=n();class gr{constructor(e,t){this.vec3=e,this.id=t}}function vr(e,t,r,i){return new gr(l(e,t,r),i)}class yr{constructor(e,t){this.index=e,this.renderTargets=t,this.extent=fe(),this.resolution=0,this.renderLocalOrigin=vr(0,0,0,"O"),this.pixelRatio=1,this.mapUnitsPerPixel=1,this.canvasGeometries={extents:[fe(),fe(),fe()],numViews:0},this.validTargets=null,this.hasDrapedFeatureSource=!1,this.hasDrapedRasterSource=!1,this.hasTargetWithoutRasterImage=!1,this.index=e,this.validTargets=new Array(t.renderTargets.length).fill(!1)}getValidTarget(e){return this.validTargets[e]?this.renderTargets.getTarget(e):null}get needsColorWithoutRasterImage(){return this.hasDrapedRasterSource&&this.hasDrapedFeatureSource&&this.hasTargetWithoutRasterImage}getColorTexture(e){const t=1===e?this.renderTargets.getTarget(0):2===e?this.renderTargets.getTarget(2):this.renderTargets.getTarget(4);return t?t.getTexture():null}getNormalTexture(e){const t=1===e?this.renderTargets.getTarget(3):null;return t?t.getTexture():null}draw(e,t){const r=this.computeRenderTargetValidityBitfield(),i=this.needsColorWithoutRasterImage;for(const r of this.renderTargets.renderTargets)1===r.type&&!1===i?this.validTargets[r.type]=!1:this.validTargets[r.type]=e.drawTarget(this,r,t);return r^this.computeRenderTargetValidityBitfield()?0:1}computeRenderTargetValidityBitfield(){const e=this.validTargets;return+e[0]|+e[1]<<1|+e[2]<<2|+e[3]<<3|+e[4]<<4}setupGeometryViewsCyclical(e){this.setupGeometryViewsDirect();const t=.001*e.range;if(this.extent[0]-t<=e.min){const t=this.canvasGeometries.extents[this.canvasGeometries.numViews++];me(this.extent,e.range,0,t)}if(this.extent[2]+t>=e.max){const t=this.canvasGeometries.extents[this.canvasGeometries.numViews++];me(this.extent,-e.range,0,t)}}setupGeometryViewsDirect(){this.canvasGeometries.numViews=1,ge(this.canvasGeometries.extents[0],this.extent)}hasSomeSizedView(){for(let e=0;e<this.canvasGeometries.numViews;e++){const t=this.canvasGeometries.extents[e];if(t[0]!==t[2]&&t[1]!==t[3])return!0}return!1}applyViewport(e){e.setViewport(0===this.index?0:this.resolution,0,this.resolution,this.resolution)}}function _r(e,t,r){return Math.min(d(Math.max(e,t)+256),r)}class br{constructor(e,t){this.size=ve(),this._fbo=null,this._fbo=new Se(e,{colorTarget:0,depthStencilTarget:0},{target:3553,pixelFormat:6408,dataType:5121,wrapMode:33071,samplingMode:9987,hasMipmap:t,maxAnisotropy:8,width:0,height:0})}dispose(){this._fbo=t(this._fbo)}getTexture(){return this._fbo?this._fbo.colorTexture:null}isValid(){return null!==this._fbo}resize(e,t){this.size[0]=e,this.size[1]=t,this._fbo.resize(this.size[0],this.size[1])}bind(e){e.bindFramebuffer(this._fbo)}generateMipMap(){this._fbo.colorTexture.descriptor.hasMipmap&&this._fbo.colorTexture.generateMipmap()}disposeRenderTargetMemory(){var e;null==(e=this._fbo)||e.resize(0,0)}get gpuMemoryUsage(){var e,t;return null!=(e=null==(t=this._fbo)?void 0:t.gpuMemoryUsage)?e:0}}class Sr{constructor(e){const t=(t,r,i=!0)=>({type:r,fbo:new br(e,i),renderPass:t,valid:!1,lastUsed:1/0});this.renderTargets=[t(0,0),t(0,1),t(5,2,!1),t(3,3),t(0,4)]}getTarget(e){return this.renderTargets[e].fbo}dispose(){for(const e of this.renderTargets)e.fbo.dispose()}disposeRenderTargetMemory(){for(const e of this.renderTargets)e.fbo.disposeRenderTargetMemory()}validateUsageForTarget(e,t,r){if(e)t.lastUsed=r;else if(r-t.lastUsed>xr)t.fbo.disposeRenderTargetMemory(),t.lastUsed=1/0;else if(t.lastUsed<1/0)return!0;return!1}get gpuMemoryUsage(){return this.renderTargets.reduce(((e,t)=>e+t.fbo.gpuMemoryUsage),0)}}const xr=1e3;class wr{constructor(){this._outer=new Map}clear(){this._outer.clear()}get empty(){return 0===this._outer.size}get(e,t){var r;return null==(r=this._outer.get(e))?void 0:r.get(t)}set(e,t,r){const i=this._outer.get(e);i?i.set(t,r):this._outer.set(e,new Map([[t,r]]))}delete(e,t){const r=this._outer.get(e);r&&(r.delete(t),0===r.size&&this._outer.delete(e))}forEach(e){this._outer.forEach(((t,r)=>e(t,r)))}}class Cr{constructor(e){this.technique=e,this.refCount=0,this.refZeroFrame=0}}class Tr{constructor(e){this._context=e,this._perConstructorInstances=new wr,this._frameCounter=0,this._keepAliveFrameCount=Rr}get viewingMode(){return this._context.viewingMode}get constructionContext(){return this._context}dispose(){this._perConstructorInstances.forEach((e=>e.forEach((e=>e.technique.dispose())))),this._perConstructorInstances.clear()}acquire(e,t){const i=t.key;let s=this._perConstructorInstances.get(e,i);if(r(s)){const r=new e(this._context,t,(()=>this.release(r)));s=new Cr(r),this._perConstructorInstances.set(e,i,s)}return++s.refCount,s.technique}releaseAndAcquire(e,t,r){if(i(r)){if(t.key===r.key)return r;r.release()}return this.acquire(e,t)}release(e){if(r(e)||this._perConstructorInstances.empty)return;const t=this._perConstructorInstances.get(e.constructor,e.key);r(t)||(--t.refCount,0===t.refCount&&(t.refZeroFrame=this._frameCounter))}frameUpdate(){this._frameCounter++,this._keepAliveFrameCount!==Rr&&this._perConstructorInstances.forEach(((e,t)=>{e.forEach(((e,r)=>{0===e.refCount&&e.refZeroFrame+this._keepAliveFrameCount<this._frameCounter&&(e.technique.dispose(),this._perConstructorInstances.delete(t,r))}))}))}async reloadAll(){const e=new Array;this._perConstructorInstances.forEach(((t,r)=>{e.push((async(e,t)=>{const r=t.shader;r&&(await r.reload(),e.forEach((e=>{e.technique.reload(this._context)})))})(t,r))})),await Promise.all(e)}}const Rr=-1,Er=e=>class extends e{constructor(){super(...arguments),this._isDisposed=!1}dispose(){for(const t of null!=(e=this._managedDisposables)?e:[]){var e;const r=this[t];this[t]=null,r&&"function"==typeof r.dispose&&r.dispose()}this._isDisposed=!0}get isDisposed(){return this._isDisposed}};class Dr extends(Er(class{})){}function Or(){return(e,t)=>{var r,i;e.hasOwnProperty("_managedDisposables")||(e._managedDisposables=null!=(r=null==(i=e._managedDisposables)?void 0:i.slice())?r:[]);e._managedDisposables.unshift(t)}}const Pr=ae.getLogger("esri.views.3d.webgl-engine.lib.GLMaterialRepository");class Lr{constructor(e){this._glMaterial=e,this.refCnt=0,this._glMaterial=e}incRefCnt(){++this.refCnt}decRefCnt(){--this.refCnt,Ce(this.refCnt>=0)}getRefCnt(){return this.refCnt}get glMaterial(){return this._glMaterial}}class Ar{constructor(e,t,r,i){this._textureRepository=e,this._techniqueRepository=t,this.materialChanged=r,this.requestRender=i,this._id2glMaterialRef=new wr}dispose(){this._textureRepository.dispose()}acquire(e,t){this._ownMaterial(e);let i=this._id2glMaterialRef.get(t,e.id);if(r(i)){const r=e.createGLMaterial({material:e,techniqueRep:this._techniqueRepository,textureRep:this._textureRepository,output:t});i=new Lr(r),this._id2glMaterialRef.set(t,e.id,i)}return i.incRefCnt(),i.glMaterial}release(e,r){const s=this._id2glMaterialRef.get(r,e.id);i(s)&&(s.decRefCnt(),0===s.getRefCnt()&&(t(s.glMaterial),this._id2glMaterialRef.delete(r,e.id)))}_ownMaterial(e){i(e.repository)&&e.repository!==this&&Pr.error("Material is already owned by a different material repository"),e.repository=this}}var zr;!function(e){e.Default={vvSizeEnabled:!1,vvSizeMinSize:wt(1,1,1),vvSizeMaxSize:wt(100,100,100),vvSizeOffset:wt(0,0,0),vvSizeFactor:wt(1,1,1),vvSizeValue:wt(1,1,1),vvColorEnabled:!1,vvColorValues:[0,0,0,0,0,0,0,0],vvColorColors:[1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0],vvOpacityEnabled:!1,vvOpacityValues:[0,0,0,0,0,0,0,0],vvOpacityOpacities:[1,1,1,1,1,1,1,1],vvSymbolAnchor:[0,0,0],vvSymbolRotationMatrix:xt()}}(zr||(zr={}));const Mr=zr;function Ir(e,t){e.vertex.uniforms.add("intrinsicWidth","float"),t.vvSize?(e.attributes.add("sizeFeatureAttribute","float"),e.vertex.uniforms.add("vvSizeMinSize","vec3"),e.vertex.uniforms.add("vvSizeMaxSize","vec3"),e.vertex.uniforms.add("vvSizeOffset","vec3"),e.vertex.uniforms.add("vvSizeFactor","vec3"),e.vertex.code.add(Ee`float getSize() {
return intrinsicWidth * clamp(vvSizeOffset + sizeFeatureAttribute * vvSizeFactor, vvSizeMinSize, vvSizeMaxSize).x;
}`)):(e.attributes.add("size","float"),e.vertex.code.add(Ee`float getSize(){
return intrinsicWidth * size;
}`)),t.vvOpacity?(e.attributes.add("opacityFeatureAttribute","float"),e.vertex.constants.add("vvOpacityNumber","int",8),e.vertex.code.add(Ee`uniform float vvOpacityValues[vvOpacityNumber];
uniform float vvOpacityOpacities[vvOpacityNumber];
float interpolateOpacity( float value ){
if (value <= vvOpacityValues[0]) {
return vvOpacityOpacities[0];
}
for (int i = 1; i < vvOpacityNumber; ++i) {
if (vvOpacityValues[i] >= value) {
float f = (value - vvOpacityValues[i-1]) / (vvOpacityValues[i] - vvOpacityValues[i-1]);
return mix(vvOpacityOpacities[i-1], vvOpacityOpacities[i], f);
}
}
return vvOpacityOpacities[vvOpacityNumber - 1];
}
vec4 applyOpacity( vec4 color ){
return vec4(color.xyz, interpolateOpacity(opacityFeatureAttribute));
}`)):e.vertex.code.add(Ee`vec4 applyOpacity( vec4 color ){
return color;
}`),t.vvColor?(e.attributes.add("colorFeatureAttribute","float"),e.vertex.constants.add("vvColorNumber","int",8),e.vertex.code.add(Ee`uniform float vvColorValues[vvColorNumber];
uniform vec4 vvColorColors[vvColorNumber];
vec4 interpolateColor( float value ) {
if (value <= vvColorValues[0]) {
return vvColorColors[0];
}
for (int i = 1; i < vvColorNumber; ++i) {
if (vvColorValues[i] >= value) {
float f = (value - vvColorValues[i-1]) / (vvColorValues[i] - vvColorValues[i-1]);
return mix(vvColorColors[i-1], vvColorColors[i], f);
}
}
return vvColorColors[vvColorNumber - 1];
}
vec4 getColor(){
return applyOpacity(interpolateColor(colorFeatureAttribute));
}`)):(e.attributes.add("color","vec4"),e.vertex.code.add(Ee`vec4 getColor(){
return applyOpacity(color);
}`))}function Fr(e,t){e.constants.add("stippleAlphaColorDiscard","float",.001),e.constants.add("stippleAlphaHighlightDiscard","float",.5),t.stippleEnabled?function(e,t){const r=!(t.draped&&t.stipplePreferContinuous);e.fragment.include(De),e.vertex.uniforms.add("stipplePatternPixelSize","float"),e.vertex.uniforms.add("pixelRatio","float"),t.draped?e.vertex.uniforms.add("worldToScreenRatio","float"):(e.vertex.uniforms.add("worldToScreenPerDistanceRatio","float"),e.vertex.uniforms.add("camPos","vec3"),e.vertex.code.add(Ee`float computeWorldToScreenRatio(vec3 segmentCenter) {
float segmentDistanceToCamera = length(segmentCenter - camPos);
return worldToScreenPerDistanceRatio / segmentDistanceToCamera;
}`));e.varyings.add("vStippleDistance","float"),t.stippleRequiresClamp&&e.varyings.add("vStippleDistanceLimits","vec2");e.vertex.code.add(Ee`
    float discretizeWorldToScreenRatio(float worldToScreenRatio) {
      float step = ${Wr};

      float discreteWorldToScreenRatio = log(worldToScreenRatio);
      discreteWorldToScreenRatio = ceil(discreteWorldToScreenRatio / step) * step;
      discreteWorldToScreenRatio = exp(discreteWorldToScreenRatio);
      return discreteWorldToScreenRatio;
    }
  `),e.vertex.code.add(Ee`
  ${t.stippleRequiresStretchMeasure?Ee`vec3`:Ee`vec2`} computeStippleDistanceLimits(float startPseudoScreen, float segmentLengthPseudoScreen, float segmentLengthScreen, float patternLength) {
  `),r&&e.vertex.code.add(Ee`
      if (segmentLengthPseudoScreen >= patternLength) {

        // Round the screen length to get an integer number of pattern repetitions (minimum 1).
        float repetitions = segmentLengthScreen / (patternLength * pixelRatio);
        float flooredRepetitions = max(1.0, floor(repetitions + 0.5));
        float segmentLengthScreenRounded = flooredRepetitions * patternLength;

        ${t.stippleRequiresStretchMeasure?Ee`return vec3(0.0, segmentLengthScreenRounded, repetitions / flooredRepetitions);`:Ee`return vec2(0.0, segmentLengthScreenRounded);`}
      }
    `);e.vertex.code.add(Ee`
      ${t.stippleRequiresStretchMeasure?Ee`return vec3(startPseudoScreen, startPseudoScreen + segmentLengthPseudoScreen, 1.0)`:Ee`return vec2(startPseudoScreen, startPseudoScreen + segmentLengthPseudoScreen)`};
    }
  `),e.fragment.uniforms.add("stipplePatternTexture","sampler2D"),e.fragment.uniforms.add("stipplePatternSDFNormalizer","float"),e.fragment.uniforms.add("stipplePatternTextureSize","float"),e.fragment.uniforms.add("stipplePatternPixelSizeInv","float"),t.stippleOffColorEnabled&&e.fragment.uniforms.add("stippleOffColor","vec4");e.fragment.code.add(Ee`float padTexture(float u) {
return (u * stipplePatternTextureSize + 1.0)/(stipplePatternTextureSize + 2.0);
}`),e.fragment.code.add(Ee`
    float getStippleSDF(out bool isClamped) {
      ${t.stippleRequiresClamp?Ee`
          float stippleDistanceClamped = clamp(vStippleDistance, vStippleDistanceLimits.x, vStippleDistanceLimits.y);
          vec2 aaCorrectedLimits = vStippleDistanceLimits + vec2(1.0, -1.0) / gl_FragCoord.w;
          isClamped = vStippleDistance < aaCorrectedLimits.x || vStippleDistance > aaCorrectedLimits.y;`:Ee`
          float stippleDistanceClamped = vStippleDistance;
          isClamped = false;`}

      float u = stippleDistanceClamped * gl_FragCoord.w * stipplePatternPixelSizeInv;
      ${t.stippleScaleWithLineWidth?Ee`u *= vLineSizeInv;`:""}
      u = padTexture(fract(u));

      float encodedSDF = rgba2float(texture2D(stipplePatternTexture, vec2(u, 0.5)));
      return (encodedSDF * 2.0 - 1.0) * stipplePatternSDFNormalizer;
    }

    float getStippleSDF() {
      bool ignored;
      return getStippleSDF(ignored);
    }

    float getStippleAlpha() {
      bool isClamped;
      float stippleSDF = getStippleSDF(isClamped);

      float antiAliasedResult = ${t.stippleScaleWithLineWidth?Ee`clamp(stippleSDF * vLineWidth + 0.5, 0.0, 1.0);`:Ee`clamp(stippleSDF + 0.5, 0.0, 1.0);`}

      return isClamped ? floor(antiAliasedResult + 0.5) : antiAliasedResult;
    }
  `),t.stippleOffColorEnabled?e.fragment.code.add(Ee`#define discardByStippleAlpha(stippleAlpha, threshold) {}
#define blendStipple(color, stippleAlpha) mix(color, stippleOffColor, stippleAlpha)`):e.fragment.code.add(Ee`#define discardByStippleAlpha(stippleAlpha, threshold) if (stippleAlpha < threshold) { discard; }
#define blendStipple(color, stippleAlpha) vec4(color.rgb, color.a * stippleAlpha)`)}(e,t):function(e){e.fragment.code.add(Ee`float getStippleAlpha() { return 1.0; }
#define discardByStippleAlpha(_stippleAlpha_, _threshold_) {}
#define blendStipple(color, _stippleAlpha_) color`)}(e)}const Wr=Ee.float(.4);const jr=Object.freeze({__proto__:null,NUM_ROUND_JOIN_SUBDIVISIONS:1,build:function(e){const t=new Oe,r=e.stippleEnabled&&e.roundCaps,i=e.falloffEnabled||r,s=e.innerColorEnabled,a=e.stippleEnabled&&e.stippleScaleWithLineWidth||e.roundCaps,o=e.stippleEnabled&&e.stippleScaleWithLineWidth;return t.extensions.add("GL_OES_standard_derivatives"),t.include(Rt),t.include(Ir,e),t.include(Fr,{...e,stippleRequiresStretchMeasure:r}),1===e.output&&t.include(Pe,e),t.vertex.uniforms.add("proj","mat4").add("view","mat4").add("cameraNearFar","vec2").add("pixelRatio","float").add("miterLimit","float").add("screenSize","vec2"),t.attributes.add("position","vec3"),t.attributes.add("subdivisionFactor","float"),t.attributes.add("uv0","vec2"),t.attributes.add("auxpos1","vec3"),t.attributes.add("auxpos2","vec3"),t.varyings.add("vColor","vec4"),t.varyings.add("vpos","vec3"),t.varyings.add("linearDepth","float"),e.multipassTerrainEnabled&&t.varyings.add("depth","float"),a&&t.varyings.add("vLineWidth","float"),o&&t.varyings.add("vLineSizeInv","float"),s&&t.varyings.add("vLineDistance","float"),i&&t.varyings.add("vLineDistanceNorm","float"),e.falloffEnabled&&t.fragment.uniforms.add("falloff","float"),e.innerColorEnabled&&(t.fragment.uniforms.add("innerColor","vec4"),t.fragment.uniforms.add("innerWidth","float")),e.roundCaps&&t.varyings.add("vCapPosition","vec2"),r&&t.varyings.add("vStipplePatternStretch","float"),t.vertex.code.add(Ee`#define PERPENDICULAR(v) vec2(v.y, -v.x);
float interp(float ncp, vec4 a, vec4 b) {
return (-ncp - a.z) / (b.z - a.z);
}
vec2 rotate(vec2 v, float a) {
float s = sin(a);
float c = cos(a);
mat2 m = mat2(c, -s, s, c);
return m * v;
}`),t.vertex.code.add(Ee`vec4 projectAndScale(vec4 pos) {
vec4 posNdc = proj * pos;
posNdc.xy *= screenSize / posNdc.w;
return posNdc;
}`),t.vertex.code.add(Ee`
    void clipAndTransform(inout vec4 pos, inout vec4 prev, inout vec4 next, in bool isStartVertex) {
      float vnp = cameraNearFar[0] * 0.99;

      //current pos behind ncp --> we need to clip
      if(pos.z > -cameraNearFar[0]) {
        if (!isStartVertex) {
          //previous in front of ncp
          if(prev.z < -cameraNearFar[0]) {
            pos = mix(prev, pos, interp(vnp, prev, pos));
            next = pos;
          } else {
            pos = vec4(0.0, 0.0, 0.0, 1.0);
          }
        }
        //next in front of ncp
        if(isStartVertex) {
          if(next.z < -cameraNearFar[0]) {
            pos = mix(pos, next, interp(vnp, pos, next));
            prev = pos;
          } else {
            pos = vec4(0.0, 0.0, 0.0, 1.0);
          }
        }
      } else {
        //current position visible
        //previous behind ncp
        if (prev.z > -cameraNearFar[0]) {
          prev = mix(pos, prev, interp(vnp, pos, prev));
        }
        //next behind ncp
        if (next.z > -cameraNearFar[0]) {
          next = mix(next, pos, interp(vnp, next, pos));
        }
      }

      ${e.multipassTerrainEnabled?"depth = pos.z;":""}
      linearDepth = (-pos.z - cameraNearFar[0]) / (cameraNearFar[1] - cameraNearFar[0]);

      pos = projectAndScale(pos);
      next = projectAndScale(next);
      prev = projectAndScale(prev);
    }
`),t.vertex.code.add(Ee`
  void main(void) {
    // unpack values from uv0.y
    bool isStartVertex = abs(abs(uv0.y)-3.0) == 1.0;

    float coverage = 1.0;
    vpos = position;

    // Check for special value of uv0.y which is used by the Renderer when graphics
    // are removed before the VBO is recompacted. If this is the case, then we just
    // project outside of clip space.
    if (uv0.y == 0.0) {
      // Project out of clip space
      gl_Position = vec4(1e038, 1e038, 1e038, 1.0);
    }
    else {
      bool isJoin = abs(uv0.y) < 3.0;

      float lineSize = getSize();
      float lineWidth = lineSize * pixelRatio;

      ${a?Ee`vLineWidth = lineWidth;`:""}
      ${o?Ee`vLineSizeInv = 1.0 / lineSize;`:""}

      // convert sub-pixel coverage to alpha
      if (lineWidth < 1.0) {
        coverage = lineWidth;
        lineWidth = 1.0;
      }else{
        // Ribbon lines cannot properly render non-integer sizes. Round width to integer size if
        // larger than one for better quality. Note that we do render < 1 pixels more or less correctly
        // so we only really care to round anything larger than 1.
        lineWidth = floor(lineWidth + 0.5);
      }

      vec4 pos  = view * vec4(position.xyz, 1.0);
      vec4 prev = view * vec4(auxpos1.xyz, 1.0);
      vec4 next = view * vec4(auxpos2.xyz, 1.0);

      clipAndTransform(pos, prev, next, isStartVertex);

      vec2 left = (pos.xy - prev.xy);
      vec2 right = (next.xy - pos.xy);

      float leftLen = length(left);
      float rightLen = length(right);
  `),e.stippleEnabled&&t.vertex.code.add(Ee`float isEndVertex = float(!isStartVertex);
vec4 segmentInfo = mix(vec4(pos.xy, right), vec4(prev.xy, left), isEndVertex);
vec2 segmentOrigin = segmentInfo.xy;
vec2 segment = segmentInfo.zw;`),t.vertex.code.add(Ee`left = (leftLen > 0.001) ? left/leftLen : vec2(0.0, 0.0);
right = (rightLen > 0.001) ? right/rightLen : vec2(0.0, 0.0);
vec2 capDisplacementDir = vec2(0, 0);
vec2 joinDisplacementDir = vec2(0, 0);
float displacementLen = lineWidth;
if (isJoin) {
bool isOutside = (left.x * right.y - left.y * right.x) * uv0.y > 0.0;
joinDisplacementDir = normalize(left + right);
joinDisplacementDir = PERPENDICULAR(joinDisplacementDir);
if (leftLen > 0.001 && rightLen > 0.001) {
float nDotSeg = dot(joinDisplacementDir, left);
displacementLen /= length(nDotSeg * left - joinDisplacementDir);
if (!isOutside) {
displacementLen = min(displacementLen, min(leftLen, rightLen)/abs(nDotSeg));
}
}
if (isOutside && (displacementLen > miterLimit * lineWidth)) {`),e.roundJoins?t.vertex.code.add(Ee`
        vec2 startDir;
        vec2 endDir;

        if (leftLen < 0.001) {
          startDir = right;
        }
        else{
          startDir = left;
        }
        startDir = normalize(startDir);
        startDir = PERPENDICULAR(startDir);

        if (rightLen < 0.001) {
          endDir = left;
        }
        else{
          endDir = right;
        }
        endDir = normalize(endDir);
        endDir = PERPENDICULAR(endDir);

        float factor = ${e.stippleEnabled?Ee`min(1.0, subdivisionFactor * ${Ee.float(1.5)})`:Ee`subdivisionFactor`};

        float rotationAngle = acos(clamp(dot(startDir, endDir), -1.0, 1.0));
        joinDisplacementDir = rotate(startDir, -sign(uv0.y) * factor * rotationAngle);
      `):t.vertex.code.add(Ee`if (leftLen < 0.001) {
joinDisplacementDir = right;
}
else if (rightLen < 0.001) {
joinDisplacementDir = left;
}
else {
joinDisplacementDir = (isStartVertex || subdivisionFactor > 0.0) ? right : left;
}
joinDisplacementDir = normalize(joinDisplacementDir);
joinDisplacementDir = PERPENDICULAR(joinDisplacementDir);`),t.vertex.code.add(Ee`displacementLen = lineWidth;
}
} else {
if (leftLen < 0.001) {
joinDisplacementDir = right;
}
else if (rightLen < 0.001) {
joinDisplacementDir = left;
}
else {
joinDisplacementDir = isStartVertex ? right : left;
}
joinDisplacementDir = normalize(joinDisplacementDir);
joinDisplacementDir = PERPENDICULAR(joinDisplacementDir);
displacementLen = lineWidth;
capDisplacementDir = isStartVertex ? -right : left;
capDisplacementDir *= subdivisionFactor;
}`),t.vertex.code.add(Ee`
    // Displacement (in pixels) caused by join/or cap
    vec2 dpos = joinDisplacementDir * sign(uv0.y) * displacementLen + capDisplacementDir * displacementLen;

    ${i||s?Ee`float lineDistNorm = sign(uv0.y) * pos.w;`:""}

    ${s?Ee`vLineDistance = lineWidth * lineDistNorm;`:""}
    ${i?Ee`vLineDistanceNorm = lineDistNorm;`:""}

    ${e.roundCaps?Ee`vCapPosition = isJoin ? vec2(0.0) : dpos;`:""}

    pos.xy += dpos;
  `),e.stippleEnabled&&(e.draped||t.vertex.code.add(Ee`vec3 segmentCenter = mix((auxpos2 + position) * 0.5, (position + auxpos1) * 0.5, isEndVertex);
float worldToScreenRatio = computeWorldToScreenRatio(segmentCenter);`),t.vertex.code.add(Ee`float segmentLengthScreenDouble = length(segment);
float segmentLengthScreen = segmentLengthScreenDouble * 0.5;
float discreteWorldToScreenRatio = discretizeWorldToScreenRatio(worldToScreenRatio);
float segmentLengthRender = length(mix(auxpos2 - position, position - auxpos1, isEndVertex));`),e.draped?t.vertex.code.add(Ee`float segmentLengthPseudoScreen = segmentLengthScreen / pixelRatio * discreteWorldToScreenRatio / worldToScreenRatio;
float startPseudoScreen = uv0.x * discreteWorldToScreenRatio - mix(0.0, segmentLengthPseudoScreen, isEndVertex);`):t.vertex.code.add(Ee`float startPseudoScreen = mix(uv0.x, uv0.x - segmentLengthRender, isEndVertex) * discreteWorldToScreenRatio;
float segmentLengthPseudoScreen = segmentLengthRender * discreteWorldToScreenRatio;`),t.vertex.code.add(Ee`
      float patternLength = ${e.stippleScaleWithLineWidth?"lineSize * ":""} stipplePatternPixelSize;

      // Compute the coordinates at both start and end of the line segment, because we need both to clamp to in the fragment shader
      // The 0.5 factor on the screen length is to correct for pixel ratio (it is calculated at double resolution)
      ${r?Ee`
            vec3 stippleSegmentInfo = computeStippleDistanceLimits(startPseudoScreen, segmentLengthPseudoScreen, segmentLengthScreen, patternLength);
            vStippleDistanceLimits = stippleSegmentInfo.xy;
            vStipplePatternStretch = stippleSegmentInfo.z;`:Ee`
            vStippleDistanceLimits = computeStippleDistanceLimits(startPseudoScreen, segmentLengthPseudoScreen, segmentLengthScreen, patternLength);`}

      vStippleDistance = mix(vStippleDistanceLimits.x, vStippleDistanceLimits.y, isEndVertex);

      // Adjust the coordinate to the displaced position (the pattern is shortened/overextended on the in/outside of joins)
      if (segmentLengthScreenDouble >= 0.001) {
        // Project the actual vertex position onto the line segment. Note that the resulting factor is within [0..1] at the
        // original vertex positions, and slightly outside of that range at the displaced positions
        vec2 stippleDisplacement = pos.xy - segmentOrigin;
        float stippleDisplacementFactor = dot(segment, stippleDisplacement) / (segmentLengthScreenDouble * segmentLengthScreenDouble);

        // Apply this offset to the actual vertex coordinate (can be screen or pseudo-screen space)
        vStippleDistance += (stippleDisplacementFactor - isEndVertex) * (vStippleDistanceLimits.y - vStippleDistanceLimits.x);
      }

      // Cancel out perspective correct interpolation because we want this length the really represent the screen distance
      vStippleDistanceLimits *= pos.w;
      vStippleDistance *= pos.w;
    `)),t.vertex.code.add(Ee`pos.xy = pos.xy / screenSize * pos.w;
vColor = getColor();
vColor.a *= coverage;
gl_Position = pos;
}
}`),e.multipassTerrainEnabled&&(t.fragment.include(Le),t.include(Ae,e)),t.include(ze,e),t.fragment.uniforms.add("intrinsicColor","vec4"),t.fragment.include(Me),t.fragment.code.add(Ee`
  void main() {
    discardBySlice(vpos);
    ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}
  `),e.roundCaps&&t.fragment.code.add(Ee`
    float fragmentRadius = length(vCapPosition);
    float fragmentSDF = (fragmentRadius - vLineWidth) * 0.5; // Divide by 2 to transform from double pixel scale
    float capCoverage = clamp(0.5 - fragmentSDF, 0.0, 1.0);
    if (capCoverage < ${Ee.float(Ie)}) {
      discard;
    }
  `),r?t.fragment.code.add(Ee`
      vec2 stipplePosition = vec2(
        max(1.0 - getStippleSDF() * 2.0 * vStipplePatternStretch, 0.0),
        vLineDistanceNorm * gl_FragCoord.w
      );
      float stippleRadius = length(stipplePosition * vLineWidth);
      float stippleCapSDF = (stippleRadius - vLineWidth) * 0.5; // Divide by 2 to transform from double pixel scale
      float stippleCoverage = clamp(0.5 - stippleCapSDF, 0.0, 1.0);
      float stippleAlpha = step(${Ee.float(Ie)}, stippleCoverage);
    `):t.fragment.code.add(Ee`float stippleAlpha = getStippleAlpha();`),t.fragment.code.add(Ee`discardByStippleAlpha(stippleAlpha, stippleAlphaColorDiscard);
vec4 color = intrinsicColor * vColor;`),t.fragment.uniforms.add("pixelRatio","float"),e.innerColorEnabled&&t.fragment.code.add(Ee`float distToInner = abs(vLineDistance * gl_FragCoord.w) - innerWidth;
float innerAA = clamp(0.5 - distToInner, 0.0, 1.0);
float innerAlpha = innerColor.a + color.a * (1.0 - innerColor.a);
color = mix(color, vec4(innerColor.rgb, innerAlpha), innerAA);`),t.fragment.code.add(Ee`vec4 finalColor = blendStipple(color, stippleAlpha);`),e.falloffEnabled&&t.fragment.code.add(Ee`finalColor.a *= pow(max(0.0, 1.0 - abs(vLineDistanceNorm * gl_FragCoord.w)), falloff);`),t.fragment.code.add(Ee`
    if (finalColor.a < ${Ee.float(Ie)}) {
      discard;
    }

    ${7===e.output?Ee`gl_FragColor = vec4(finalColor.a);`:""}
    ${0===e.output?Ee`gl_FragColor = highlightSlice(finalColor, vpos);`:""}
    ${0===e.output&&e.OITEnabled?"gl_FragColor = premultiplyAlpha(gl_FragColor);":""}
    ${4===e.output?Ee`gl_FragColor = vec4(1.0);`:""}
    ${1===e.output?Ee`outputDepth(linearDepth);`:""}
  }
  `),t}}),Nr=new Map([["position",0],["subdivisionFactor",1],["uv0",2],["auxpos1",3],["auxpos2",4],["size",6],["sizeFeatureAttribute",6],["color",5],["colorFeatureAttribute",5],["opacityFeatureAttribute",7]]);class Vr extends Ne{constructor(e,t,r){super(e,t,r),this.stippleTextureRepository=e.stippleTextureRepository}get stippleEnabled(){return this.configuration.stippleEnabled&&4!==this.configuration.output}initializeProgram(e){const t=Vr.shader.get(),r=this.configuration,i=t.build({OITEnabled:0===r.transparencyPassType,output:r.output,slicePlaneEnabled:r.slicePlaneEnabled,sliceHighlightDisabled:!1,sliceEnabledForVertexPrograms:!1,draped:r.draped,stippleEnabled:this.stippleEnabled,stippleOffColorEnabled:r.stippleOffColorEnabled,stippleRequiresClamp:!0,stippleScaleWithLineWidth:r.stippleScaleWithLineWidth,stipplePreferContinuous:r.stipplePreferContinuous,roundCaps:r.roundCaps,roundJoins:r.roundJoins,vvColor:r.vvColor,vvSize:r.vvSize,vvInstancingEnabled:!0,vvOpacity:r.vvOpacity,falloffEnabled:r.falloffEnabled,innerColorEnabled:r.innerColorEnabled,multipassTerrainEnabled:r.multipassTerrainEnabled,cullAboveGround:r.cullAboveGround});return new Ve(e.rctx,i,Nr)}dispose(){super.dispose(),this.stippleTextureRepository.release(this.stipplePattern),this.stipplePattern=null,this.stippleTextureBind=null}bindPass(t,r){if(He(this.program,r.camera.projectionMatrix),4===this.configuration.output&&Ue(this.program,r),r.multipassTerrainEnabled&&(this.program.setUniform2fv("inverseViewport",r.inverseViewport),Ge(this.program,r)),this.program.setUniform1f("intrinsicWidth",t.width),this.program.setUniform4fv("intrinsicColor",t.color),this.program.setUniform1f("miterLimit","miter"!==t.join?0:t.miterLimit),this.program.setUniform2fv("cameraNearFar",r.camera.nearFar),this.program.setUniform1f("pixelRatio",r.camera.pixelRatio),this.program.setUniform2f("screenSize",r.camera.fullViewport[2],r.camera.fullViewport[3]),Et(this.program,t),this.stipplePattern!==t.stipplePattern){const e=t.stipplePattern;this.stippleTextureBind=this.stippleTextureRepository.swap(this.stipplePattern,e),this.stipplePattern=e}if(this.stippleEnabled){const{pixelSize:e,sdfNormalizer:a,pixels:o}=i(this.stippleTextureBind)?this.stippleTextureBind(this.program):{pixelSize:1,sdfNormalizer:1,pixels:1};if(this.program.setUniform1f("stipplePatternSDFNormalizer",a),this.program.setUniform1f("stipplePatternTextureSize",o),this.program.setUniform1f("stipplePatternPixelSize",e),this.program.setUniform1f("stipplePatternPixelSizeInv",1/e),this.configuration.draped?this.program.setUniform1f("worldToScreenRatio",1/r.screenToPCSRatio):this.program.setUniform1f("worldToScreenPerDistanceRatio",1/r.camera.perScreenPixelRatio),this.configuration.stippleOffColorEnabled){const e=s(t.stippleOffColor);this.program.setUniform4f("stippleOffColor",e[0],e[1],e[2],e.length>3?e[3]:1)}}this.configuration.falloffEnabled&&this.program.setUniform1f("falloff",t.falloff),this.configuration.innerColorEnabled&&(this.program.setUniform4fv("innerColor",e(t.innerColor,t.color)),this.program.setUniform1f("innerWidth",t.innerWidth*r.camera.pixelRatio))}bindDraw(e){Be(this.program,e),this.stippleEnabled&&!this.configuration.draped&&qe(this.program,e.origin,e.camera.viewInverseTransposeMatrix),ke(this.program,this.configuration,e),this.program.rebindTextures()}makePipelineState(e,t){const r=this.configuration,i=3===e,s=2===e;return At({blending:0===r.output||7===r.output?i?zt:Mt(e):null,depthTest:{func:It(e)},depthWrite:i?!r.transparent&&r.writeDepth&&Ft:Wt(e),colorWrite:jt,stencilWrite:r.sceneHasOcludees?$e:null,stencilTest:r.sceneHasOcludees?t?Ze:Je:null,polygonOffset:i||s?r.polygonOffset&&Hr:Nt})}initializePipeline(){const e=this.configuration,t=e.polygonOffset&&Hr;return e.occluder&&(this._occluderPipelineTransparent=At({blending:zt,polygonOffset:t,depthTest:Qe,depthWrite:null,colorWrite:jt,stencilWrite:null,stencilTest:Ye}),this._occluderPipelineOpaque=At({blending:zt,polygonOffset:t,depthTest:Qe,depthWrite:null,colorWrite:jt,stencilWrite:Xe,stencilTest:Ke}),this._occluderPipelineMaskWrite=At({blending:null,polygonOffset:t,depthTest:et,depthWrite:null,colorWrite:null,stencilWrite:$e,stencilTest:Ze})),this._occludeePipelineState=this.makePipelineState(this.configuration.transparencyPassType,!0),this.makePipelineState(this.configuration.transparencyPassType,!1)}get primitiveType(){return 5}getPipelineState(e,t){return t?this._occludeePipelineState:this.configuration.occluder?10===e?this._occluderPipelineTransparent:9===e?this._occluderPipelineOpaque:this._occluderPipelineMaskWrite:super.getPipelineState(e,t)}}Vr.shader=new We(jr,(()=>Promise.resolve().then((()=>jr))));const Hr={factor:0,units:-4};class Ur extends je{constructor(){super(...arguments),this.output=0,this.occluder=!1,this.slicePlaneEnabled=!1,this.transparent=!1,this.polygonOffset=!1,this.writeDepth=!1,this.draped=!1,this.stippleEnabled=!1,this.stippleOffColorEnabled=!1,this.stippleScaleWithLineWidth=!1,this.stipplePreferContinuous=!0,this.roundCaps=!1,this.roundJoins=!1,this.vvSize=!1,this.vvColor=!1,this.vvOpacity=!1,this.falloffEnabled=!1,this.innerColorEnabled=!1,this.sceneHasOcludees=!1,this.transparencyPassType=3,this.multipassTerrainEnabled=!1,this.cullAboveGround=!1}}K([Fe({count:8})],Ur.prototype,"output",void 0),K([Fe()],Ur.prototype,"occluder",void 0),K([Fe()],Ur.prototype,"slicePlaneEnabled",void 0),K([Fe()],Ur.prototype,"transparent",void 0),K([Fe()],Ur.prototype,"polygonOffset",void 0),K([Fe()],Ur.prototype,"writeDepth",void 0),K([Fe()],Ur.prototype,"draped",void 0),K([Fe()],Ur.prototype,"stippleEnabled",void 0),K([Fe()],Ur.prototype,"stippleOffColorEnabled",void 0),K([Fe()],Ur.prototype,"stippleScaleWithLineWidth",void 0),K([Fe()],Ur.prototype,"stipplePreferContinuous",void 0),K([Fe()],Ur.prototype,"roundCaps",void 0),K([Fe()],Ur.prototype,"roundJoins",void 0),K([Fe()],Ur.prototype,"vvSize",void 0),K([Fe()],Ur.prototype,"vvColor",void 0),K([Fe()],Ur.prototype,"vvOpacity",void 0),K([Fe()],Ur.prototype,"falloffEnabled",void 0),K([Fe()],Ur.prototype,"innerColorEnabled",void 0),K([Fe()],Ur.prototype,"sceneHasOcludees",void 0),K([Fe({count:4})],Ur.prototype,"transparencyPassType",void 0),K([Fe()],Ur.prototype,"multipassTerrainEnabled",void 0),K([Fe()],Ur.prototype,"cullAboveGround",void 0);const Gr=ae.getLogger("esri.views.3d.webgl-engine.materials.RibbonLineMaterial");class Br extends rt{constructor(e){super(e,kr),this._vertexAttributeLocations=Nr,this.techniqueConfig=new Ur,this.layout=this.createLayout()}isClosed(e,t){return Qr(this.parameters,e,t)}dispose(){}getPassParameters(){return this.parameters}getTechniqueConfig(e,t){this.techniqueConfig.output=e,this.techniqueConfig.draped=20===t.slot;const r=i(this.parameters.stipplePattern);return this.techniqueConfig.stippleEnabled=r,this.techniqueConfig.stippleOffColorEnabled=r&&i(this.parameters.stippleOffColor),this.techniqueConfig.stippleScaleWithLineWidth=r&&this.parameters.stippleScaleWithLineWidth,this.techniqueConfig.stipplePreferContinuous=r&&this.parameters.stipplePreferContinuous,this.techniqueConfig.slicePlaneEnabled=this.parameters.slicePlaneEnabled,this.techniqueConfig.sceneHasOcludees=this.parameters.sceneHasOcludees,this.techniqueConfig.roundJoins="round"===this.parameters.join,this.techniqueConfig.roundCaps=2===this.parameters.cap,this.techniqueConfig.transparent=this.parameters.transparent,this.techniqueConfig.polygonOffset=this.parameters.polygonOffset,this.techniqueConfig.writeDepth=this.parameters.writeDepth,this.techniqueConfig.vvColor=this.parameters.vvColorEnabled,this.techniqueConfig.vvOpacity=this.parameters.vvOpacityEnabled,this.techniqueConfig.vvSize=this.parameters.vvSizeEnabled,this.techniqueConfig.innerColorEnabled=this.parameters.innerWidth>0&&i(this.parameters.innerColor),this.techniqueConfig.falloffEnabled=this.parameters.falloff>0,this.techniqueConfig.occluder=8===this.parameters.renderOccluded,this.techniqueConfig.transparencyPassType=t.transparencyPassType,this.techniqueConfig.multipassTerrainEnabled=t.multipassTerrainEnabled,this.techniqueConfig.cullAboveGround=t.cullAboveGround,this.techniqueConfig}intersect(e,t,r,s,a,o,n,l,d){i(d)?this.intersectDrapedLineGeometry(e,s,d,o,n):this.intersectLineGeometry(e,t,r,s,n)}intersectDrapedLineGeometry(e,t,r,i,s){if(!t.options.selectionMode)return;const a=e.vertexAttributes.get("position").data,o=e.vertexAttributes.get("size");let n=this.parameters.width;if(this.parameters.vvSizeEnabled){const t=e.vertexAttributes.get("sizeFeatureAttribute").data[0];n*=c(this.parameters.vvSizeOffset[0]+t*this.parameters.vvSizeFactor[0],this.parameters.vvSizeMinSize[0],this.parameters.vvSizeMaxSize[0])}else o&&(n*=o.data[0]);const l=i[0],d=i[1],h=(n/2+4)*e.screenToWorldRatio;let p=Number.MAX_VALUE,u=0;for(let e=0;e<a.length-5;e+=3){const t=a[e],r=a[e+1],i=l-t,s=d-r,o=a[e+3]-t,n=a[e+4]-r,h=c((o*i+n*s)/(o*o+n*n),0,1),f=o*h-i,m=n*h-s,g=f*f+m*m;g<p&&(p=g,u=e/3)}p<h*h&&s(r.dist,r.normal,u,!1)}intersectLineGeometry(e,t,r,i,s){if(!i.options.selectionMode||Ct(t))return;if(!Te(r))return void Gr.error("intersection assumes a translation-only matrix");const a=e.vertexAttributes,o=a.get("position").data;let n=this.parameters.width;if(this.parameters.vvSizeEnabled){const e=a.get("sizeFeatureAttribute").data[0];n*=c(this.parameters.vvSizeOffset[0]+e*this.parameters.vvSizeFactor[0],this.parameters.vvSizeMinSize[0],this.parameters.vvSizeMaxSize[0])}else a.has("size")&&(n*=a.get("size").data[0]);const l=i.camera,d=ti;w(d,i.point);const _=n*l.pixelRatio/2+4*l.pixelRatio;h(hi[0],d[0]-_,d[1]+_,0),h(hi[1],d[0]+_,d[1]+_,0),h(hi[2],d[0]+_,d[1]-_,0),h(hi[3],d[0]-_,d[1]-_,0);for(let e=0;e<4;e++)if(!l.unprojectFromRenderScreen(hi[e],pi[e]))return;vt(l.eye,pi[0],pi[1],ui),vt(l.eye,pi[1],pi[2],fi),vt(l.eye,pi[2],pi[3],mi),vt(l.eye,pi[3],pi[0],gi);let b=Number.MAX_VALUE,S=0;const x=Jr(this.parameters,a,e.indices)?o.length-2:o.length-5;for(let e=0;e<x;e+=3){Yr[0]=o[e]+r[12],Yr[1]=o[e+1]+r[13],Yr[2]=o[e+2]+r[14];const t=(e+3)%o.length;if(Xr[0]=o[t]+r[12],Xr[1]=o[t+1]+r[13],Xr[2]=o[t+2]+r[14],yt(ui,Yr)<0&&yt(ui,Xr)<0||yt(fi,Yr)<0&&yt(fi,Xr)<0||yt(mi,Yr)<0&&yt(mi,Xr)<0||yt(gi,Yr)<0&&yt(gi,Xr)<0)continue;if(l.projectToRenderScreen(Yr,ri),l.projectToRenderScreen(Xr,ii),ri[2]<0&&ii[2]>0){p(Kr,Yr,Xr);const e=l.frustum,t=-yt(e[4],Yr)/u(Kr,_t(e[4]));f(Kr,Kr,t),m(Yr,Yr,Kr),l.projectToRenderScreen(Yr,ri)}else if(ri[2]>0&&ii[2]<0){p(Kr,Xr,Yr);const e=l.frustum,t=-yt(e[4],Xr)/u(Kr,_t(e[4]));f(Kr,Kr,t),m(Xr,Xr,Kr),l.projectToRenderScreen(Xr,ii)}else if(ri[2]<0&&ii[2]<0)continue;ri[2]=0,ii[2]=0;const i=ut(ft(ri,ii,oi),d);i<b&&(b=i,g(si,Yr),g(ai,Xr),S=e/3)}const C=i.rayBegin,T=i.rayEnd;if(b<_*_){let e=Number.MAX_VALUE;if(mt(ft(si,ai,oi),ft(C,T,ni),ei)){p(ei,ei,C);const t=v(ei);f(ei,ei,1/t),e=t/y(C,T)}s(e,ei,S,!1)}}computeAttachmentOrigin(e,t){const r=e.vertexAttributes;if(!r)return null;const i=e.indices,s=r.get("position");return St(s,i?i.get("position"):null,i&&Jr(this.parameters,r,i),t)}createLayout(){const e=bt().vec3f("position").f32("subdivisionFactor").vec2f("uv0").vec3f("auxpos1").vec3f("auxpos2");return this.parameters.vvSizeEnabled?e.f32("sizeFeatureAttribute"):e.f32("size"),this.parameters.vvColorEnabled?e.f32("colorFeatureAttribute"):e.vec4f("color"),this.parameters.vvOpacityEnabled&&e.f32("opacityFeatureAttribute"),e}createBufferWriter(){return new $r(this.layout,this.parameters)}requiresSlot(e,t){if(20===e)return!0;if(8===this.parameters.renderOccluded)return 2===e||9===e||10===e;const r=k(t);if(0===r||7===r){return e===(this.parameters.writeDepth?4:7)}return 2===e}createGLMaterial(e){return 0===e.output||7===e.output||4===e.output||1===e.output?new qr(e):null}validateParameters(e){"miter"!==e.join&&(e.miterLimit=0),this.requiresTransparent(e)&&(e.transparent=!0)}requiresTransparent(e){return!!((e.color&&e.color[3])<1||e.innerWidth>0&&this.colorRequiresTransparent(e.innerColor)||e.stipplePattern&&this.colorRequiresTransparent(e.stippleOffColor)||e.falloff>0)}colorRequiresTransparent(e){return i(e)&&e[3]<1&&e[3]>0}}class qr extends it{updateParameters(e){return this.ensureTechnique(Vr,e)}_updateOccludeeState(e){e.hasOccludees!==this._material.parameters.sceneHasOcludees&&this._material.setParameters({sceneHasOcludees:e.hasOccludees})}beginSlot(e){return 0!==this._output&&7!==this._output||this._updateOccludeeState(e),this.updateParameters(e)}bind(e,t){t.bindPass(this._material.getPassParameters(),e)}}const kr={width:0,color:[1,1,1,1],join:"miter",cap:0,miterLimit:5,writeDepth:!0,polygonOffset:!1,stipplePattern:null,stippleOffColor:null,stippleScaleWithLineWidth:!1,stipplePreferContinuous:!0,slicePlaneEnabled:!1,vvFastUpdate:!1,transparent:!1,isClosed:!1,falloff:0,innerWidth:0,innerColor:null,sceneHasOcludees:!1,...tt,...Mr.Default};class $r{constructor(e,t){this.parameters=t,this.numJoinSubdivisions=0,this.vertexBufferLayout=e;const r=t.stipplePattern?1:0;switch(this.parameters.join){case"miter":case"bevel":this.numJoinSubdivisions=r;break;case"round":this.numJoinSubdivisions=1+r}}isClosed(e){return Jr(this.parameters,e.vertexAttributes,e.indices)}numCapSubdivisions(e){if(this.isClosed(e))return 0;switch(this.parameters.cap){case 1:case 2:return 1;default:return 0}}allocate(e){return this.vertexBufferLayout.createBuffer(e)}elementCount(e){const t=2*this.numCapSubdivisions(e)+2,r=e.indices.get("position").length/2+1,i=this.isClosed(e);let s=i?2:2*t;const a=i?0:1,o=i?r:r-1;if(e.vertexAttributes.has("subdivisions")){const t=e.vertexAttributes.get("subdivisions").data;for(let e=a;e<o;++e){s+=4+2*t[e]}}else{s+=(o-a)*(2*this.numJoinSubdivisions+4)}return s+=2,s}write(e,t,r,i){var s;const a=li,o=di,n=ci,l=t.vertexAttributes.get("position").data,d=t.indices&&t.indices.get("position"),c=null==(s=t.vertexAttributes.get("distanceToStart"))?void 0:s.data,p=this.numCapSubdivisions(t);d&&d.length!==2*(l.length/3-1)&&console.warn("RibbonLineMaterial does not support indices");let u=null;t.vertexAttributes.has("subdivisions")&&(u=t.vertexAttributes.get("subdivisions").data);let f=1,m=0;this.parameters.vvSizeEnabled?m=t.vertexAttributes.get("sizeFeatureAttribute").data[0]:t.vertexAttributes.has("size")&&(f=t.vertexAttributes.get("size").data[0]);let v=[1,1,1,1],b=0;this.parameters.vvColorEnabled?b=t.vertexAttributes.get("colorFeatureAttribute").data[0]:t.vertexAttributes.has("color")&&(v=t.vertexAttributes.get("color").data);let S=0;this.parameters.vvOpacityEnabled&&(S=t.vertexAttributes.get("opacityFeatureAttribute").data[0]);const x=l.length/3,w=e.transformation,C=new Float32Array(r.buffer),T=this.vertexBufferLayout.stride/4;let R=i*T;const E=R;let D=0;const O=(e,t,r,i,s,a,o)=>{if(C[R++]=t[0],C[R++]=t[1],C[R++]=t[2],C[R++]=i,C[R++]=o,C[R++]=s,C[R++]=e[0],C[R++]=e[1],C[R++]=e[2],C[R++]=r[0],C[R++]=r[1],C[R++]=r[2],this.parameters.vvSizeEnabled?C[R++]=m:C[R++]=f,this.parameters.vvColorEnabled)C[R++]=b;else{const e=Math.min(4*a,v.length-4);C[R++]=v[e+0],C[R++]=v[e+1],C[R++]=v[e+2],C[R++]=v[e+3]}this.parameters.vvOpacityEnabled&&(C[R++]=S)};R+=T,h(o,l[0],l[1],l[2]),w&&_(o,o,w);const P=this.isClosed(t);if(P){const e=l.length-3;h(a,l[e],l[e+1],l[e+2]),w&&_(a,a,w)}else{g(a,o),h(n,l[3],l[4],l[5]),w&&_(n,n,w);for(let e=0;e<p;++e){const t=1-e/p;O(a,o,n,t,-4,0,D),O(a,o,n,t,4,0,D)}O(a,o,n,0,-4,0,D),O(a,o,n,0,4,0,D),g(a,o),g(o,n)}const L=P?0:1,A=P?x:x-1,z=c?(e,t,r)=>D=c[r]:(e,t,r)=>D+=y(e,t);for(let e=L;e<A;e++){const t=(e+1)%x*3;h(n,l[t+0],l[t+1],l[t+2]),w&&_(n,n,w),z(a,o,e),O(a,o,n,0,-1,e,D),O(a,o,n,0,1,e,D);const r=u?u[e]:this.numJoinSubdivisions;for(let t=0;t<r;++t){const i=(t+1)/(r+1);O(a,o,n,i,-1,e,D),O(a,o,n,i,1,e,D)}O(a,o,n,1,-2,e,D),O(a,o,n,1,2,e,D),g(a,o),g(o,n)}if(P)h(n,l[3],l[4],l[5]),w&&_(n,n,w),D=z(a,o,A),O(a,o,n,0,-1,L,D),O(a,o,n,0,1,L,D);else{D=z(a,o,A),O(a,o,n,0,-5,A,D),O(a,o,n,0,5,A,D);for(let e=0;e<p;++e){const t=(e+1)/p;O(a,o,n,t,-5,A,D),O(a,o,n,t,5,A,D)}}Zr(C,E+T,C,E,T);R=Zr(C,R-T,C,R,T)}}function Zr(e,t,r,i,s){for(let a=0;a<s;a++)r[i++]=e[t++];return i}function Jr(e,t,r){return Qr(e,t.get("position").data,r?r.get("position"):null)}function Qr(e,t,r){return!!e.isClosed&&(r?r.length>2:t.length>6)}const Yr=n(),Xr=n(),Kr=n(),ei=n(),ti=n(),ri=ht(),ii=ht(),si=n(),ai=n(),oi=pt(),ni=pt(),li=n(),di=n(),ci=n(),hi=[ht(),ht(),ht(),ht()],pi=[n(),n(),n(),n()],ui=gt(),fi=gt(),mi=gt(),gi=gt();class vi{constructor(e,t=125e4){this._originSR=e,this._gridSize=t,this._origins=new Map,this._objects=new Map,this._rootOriginId="root/"+te()}getOrigin(e){const t=this._origins.get(this._rootOriginId);if(null==t){if(i(yi))return this._origins.set(this._rootOriginId,vr(yi[0],yi[1],yi[2],this._rootOriginId)),this.getOrigin(e);const t=vr(e[0]+Math.random()-.5,e[1]+Math.random()-.5,e[2]+Math.random()-.5,this._rootOriginId);return this._origins.set(this._rootOriginId,t),t}const r=this._gridSize,s=Math.round(e[0]/r),a=Math.round(e[1]/r),o=Math.round(e[2]/r),n=`${s}/${a}/${o}`;let l=this._origins.get(n);const d=.5*r;if(p(_i,e,t.vec3),_i[0]=Math.abs(_i[0]),_i[1]=Math.abs(_i[1]),_i[2]=Math.abs(_i[2]),_i[0]<d&&_i[1]<d&&_i[2]<d){if(l){const t=Math.max(..._i);p(_i,e,l.vec3),_i[0]=Math.abs(_i[0]),_i[1]=Math.abs(_i[1]),_i[2]=Math.abs(_i[2]);if(Math.max(..._i)<t)return l}return t}return l||(l=vr(s*r,a*r,o*r,n),this._origins.set(n,l)),l}_drawOriginBox(e,t=[1,1,0,1]){const r=window.view,i=r._stage,s=t.toString();if(!this._objects.has(s)){this._material=new Br({width:2,color:t}),i.add(this._material);const e=new $({isPickable:!1}),r=new Z({castShadow:!1});i.add(r),e.add(r),i.add(e),this._objects.set(s,r)}const a=this._objects.get(s),o=[0,1,5,4,0,2,1,7,6,2,0,1,3,7,5,4,6,2,0],n=o.length,l=new Array(3*n),d=new Uint16Array(2*(n-1)),c=.5*this._gridSize;for(let t=0;t<n;t++)l[3*t+0]=e[0]+(1&o[t]?c:-c),l[3*t+1]=e[1]+(2&o[t]?c:-c),l[3*t+2]=e[2]+(4&o[t]?c:-c),t>0&&(d[2*t+0]=t-1,d[2*t+1]=t);T(l,this._originSR,0,l,r.renderSpatialReference,0,n);const h=new st([["position",{size:3,data:l,exclusive:!0}]],[["position",d]],2);i.add(h),a.addGeometry(h,this._material,H)}}let yi=null;const _i=n();class bi{constructor(e){this.rctx=e,this.camera=null,this.lastFrameCamera=new we,this.pass=0,this.slot=2,this.highlightDepthTexture=null,this.renderOccludedMask=xi,this.hasOccludees=!1}resetRenderOccludedMask(){this.renderOccludedMask=xi}get isHighlightPass(){return 5===this.pass}}class Si extends bi{constructor(e,t,r,i,s,a){super(e),this.offscreenRenderingHelper=t,this.scenelightingData=r,this.shadowMap=i,this.ssaoHelper=s,this.sliceHelper=a}}const xi=13;class wi{constructor(){this.adds=new ne,this.removes=new ne,this.updates=new ne({allocator:e=>e||new Ci,deallocator:e=>(e.renderGeometry=null,e)})}clear(){this.adds.clear(),this.removes.clear(),this.updates.clear()}prune(){this.adds.prune(),this.removes.prune(),this.updates.prune()}}class Ci{}class Ti{constructor(){this.adds=new Array,this.removes=new Array,this.updates=new Array}}function Ri(e){const t=new Map,r=e=>{let r=t.get(e);return r||(r=new Ti,t.set(e,r)),r};return e.adds.forAll((e=>{Ei(e)&&r(e.material).adds.push(e)})),e.removes.forAll((e=>{Ei(e)&&r(e.material).removes.push(e)})),e.updates.forAll((e=>{Ei(e.renderGeometry)&&r(e.renderGeometry.material).updates.push(e)})),t}function Ei(e){return e.data.indexCount>=1}class Di{constructor(){this.enabled=!0,this._time=0}get time(){return le(this._time)}advance(e){return i(e.forcedTime)?this._time!==e.forcedTime&&(this._time=e.forcedTime,!0):!(!this.enabled||0===e.dt)&&(this._time+=e.dt,!0)}}function Oi(e){e.fragment.uniforms.add("lastFrameColorMap","sampler2D"),e.fragment.uniforms.add("reprojectionMat","mat4"),e.fragment.uniforms.add("rpProjectionMat","mat4"),e.fragment.code.add(Ee`vec2 reprojectionCoordinate(vec3 projectionCoordinate)
{
vec4 zw = rpProjectionMat * vec4(0.0, 0.0, -projectionCoordinate.z, 1.0);
vec4 reprojectedCoord = reprojectionMat * vec4(zw.w * (projectionCoordinate.xy * 2.0 - 1.0), zw.z, zw.w);
reprojectedCoord.xy /= reprojectedCoord.w;
return reprojectedCoord.xy * 0.5 + 0.5;
}`)}function Pi(e,t){e.fragment.uniforms.add("nearFar","vec2"),e.fragment.uniforms.add("depthMapView","sampler2D"),e.fragment.uniforms.add("ssrViewMat","mat4"),e.fragment.uniforms.add("invResolutionHeight","float"),e.fragment.include(Le),e.include(Oi),e.fragment.code.add(Ee`
  const int maxSteps = ${t.highStepCount?"150;":"75;"}

  vec4 applyProjectionMat(mat4 projectionMat, vec3 x)
  {
    vec4 projectedCoord =  projectionMat * vec4(x, 1.0);
    projectedCoord.xy /= projectedCoord.w;
    projectedCoord.xy = projectedCoord.xy*0.5 + 0.5;
    return projectedCoord;
  }

  vec3 screenSpaceIntersection(vec3 dir, vec3 startPosition, vec3 viewDir, vec3 normal)
  {
    vec3 viewPos = startPosition;
    vec3 viewPosEnd = startPosition;

    // Project the start position to the screen
    vec4 projectedCoordStart = applyProjectionMat(rpProjectionMat, viewPos);
    vec3  Q0 = viewPos / projectedCoordStart.w; // homogeneous camera space
    float k0 = 1.0/ projectedCoordStart.w;

    // advance the position in the direction of the reflection
    viewPos += dir;

    vec4 projectedCoordVanishingPoint = applyProjectionMat(rpProjectionMat, dir);

    // Project the advanced position to the screen
    vec4 projectedCoordEnd = applyProjectionMat(rpProjectionMat, viewPos);
    vec3  Q1 = viewPos / projectedCoordEnd.w; // homogeneous camera space
    float k1 = 1.0/ projectedCoordEnd.w;

    // calculate the reflection direction in the screen space
    vec2 projectedCoordDir = (projectedCoordEnd.xy - projectedCoordStart.xy);
    vec2 projectedCoordDistVanishingPoint = (projectedCoordVanishingPoint.xy - projectedCoordStart.xy);

    float yMod = min(abs(projectedCoordDistVanishingPoint.y), 1.0);

    float projectedCoordDirLength = length(projectedCoordDir);
    float maxSt = float(maxSteps);

    // normalize the projection direction depending on maximum steps
    // this determines how blocky the reflection looks
    vec2 dP = yMod * (projectedCoordDir)/(maxSt * projectedCoordDirLength);

    // Normalize the homogeneous camera space coordinates
    vec3  dQ = yMod * (Q1 - Q0)/(maxSt * projectedCoordDirLength);
    float dk = yMod * (k1 - k0)/(maxSt * projectedCoordDirLength);

    // initialize the variables for ray marching
    vec2 P = projectedCoordStart.xy;
    vec3 Q = Q0;
    float k = k0;
    float rayStartZ = -startPosition.z; // estimated ray start depth value
    float rayEndZ = -startPosition.z;   // estimated ray end depth value
    float prevEstimateZ = -startPosition.z;
    float rayDiffZ = 0.0;
    float dDepth;
    float depth;
    float rayDiffZOld = 0.0;

    // early outs
    if (dot(normal, dir) < 0.0 || dot(-viewDir, normal) < 0.0)
      return vec3(P, 0.0);

    for(int i = 0; i < maxSteps-1; i++)
    {
      depth = -linearDepthFromTexture(depthMapView, P, nearFar); // get linear depth from the depth buffer

      // estimate depth of the marching ray
      rayStartZ = prevEstimateZ;
      dDepth = -rayStartZ - depth;
      rayEndZ = (dQ.z * 0.5 + Q.z)/ ((dk * 0.5 + k));
      rayDiffZ = rayEndZ- rayStartZ;
      prevEstimateZ = rayEndZ;

      if(-rayEndZ > nearFar[1] || -rayEndZ < nearFar[0] || P.y < 0.0  || P.y > 1.0 )
      {
        return vec3(P, 0.);
      }

      // If we detect a hit - return the intersection point, two conditions:
      //  - dDepth > 0.0 - sampled point depth is in front of estimated depth
      //  - if difference between dDepth and rayDiffZOld is not too large
      //  - if difference between dDepth and 0.025/abs(k) is not too large
      //  - if the sampled depth is not behind far plane or in front of near plane

      if((dDepth) < 0.025/abs(k) + abs(rayDiffZ) && dDepth > 0.0 && depth > nearFar[0] && depth < nearFar[1] && abs(P.y - projectedCoordStart.y) > invResolutionHeight)
      {
          return vec3(P, depth);
      }

      // continue with ray marching
      P += dP;
      Q.z += dQ.z;
      k += dk;
      rayDiffZOld = rayDiffZ;
    }
    return vec3(P, 0.0);
  }
  `)}function Li(e,t){t.ssrEnabled&&(e.bindTexture(t.linearDepthTexture,"depthMapView"),e.setUniform2fv("nearFar",t.camera.nearFar),e.setUniformMatrix4fv("ssrViewMat",t.camera.viewMatrix),e.setUniform1f("invResolutionHeight",1/t.camera.height),function(e,t){e.bindTexture(t.lastFrameColorTexture,"lastFrameColorMap"),e.setUniformMatrix4fv("reprojectionMat",t.reprojectionMatrix),e.setUniformMatrix4fv("rpProjectionMat",t.camera.projectionMatrix)}(e,t))}function Ai(e){e.fragment.code.add(Ee`float normals2FoamIntensity(vec3 n, float waveStrength){
float normalizationFactor =  max(0.015, waveStrength);
return max((n.x + n.y)*0.3303545/normalizationFactor + 0.3303545, 0.0);
}`)}function zi(e){e.fragment.code.add(Ee`vec3 foamIntensity2FoamColor(float foamIntensityExternal, float foamPixelIntensity, vec3 skyZenitColor, float dayMod){
return foamIntensityExternal * (0.075 * skyZenitColor * pow(foamPixelIntensity, 4.) +  50.* pow(foamPixelIntensity, 23.0)) * dayMod;
}`)}function Mi(e){e.fragment.uniforms.add("texWaveNormal","sampler2D"),e.fragment.uniforms.add("texWavePerturbation","sampler2D"),e.fragment.uniforms.add("waveParams","vec4"),e.fragment.uniforms.add("waveDirection","vec2"),e.include(Ai),e.fragment.code.add(Ee`const vec2  FLOW_JUMP = vec2(6.0/25.0, 5.0/24.0);
vec2 textureDenormalized2D(sampler2D _tex, vec2 _uv) {
return 2.0 * texture2D(_tex, _uv).rg - 1.0;
}
float sampleNoiseTexture(vec2 _uv) {
return texture2D(texWavePerturbation, _uv).b;
}
vec3 textureDenormalized3D(sampler2D _tex, vec2 _uv) {
return 2.0 * texture2D(_tex, _uv).rgb - 1.0;
}
float computeProgress(vec2 uv, float time) {
return fract(time);
}
float computeWeight(vec2 uv, float time) {
float progress = computeProgress(uv, time);
return 1.0 - abs(1.0 - 2.0 * progress);
}
vec3 computeUVPerturbedWeigth(sampler2D texFlow, vec2 uv, float time, float phaseOffset) {
float flowStrength = waveParams[2];
float flowOffset = waveParams[3];
vec2 flowVector = textureDenormalized2D(texFlow, uv) * flowStrength;
float progress = computeProgress(uv, time + phaseOffset);
float weight = computeWeight(uv, time + phaseOffset);
vec2 result = uv;
result -= flowVector * (progress + flowOffset);
result += phaseOffset;
result += (time - progress) * FLOW_JUMP;
return vec3(result, weight);
}
const float TIME_NOISE_TEXTURE_REPEAT = 0.3737;
const float TIME_NOISE_STRENGTH = 7.77;
vec3 getWaveLayer(sampler2D _texNormal, sampler2D _dudv, vec2 _uv, vec2 _waveDir, float time) {
float waveStrength = waveParams[0];
vec2 waveMovement = time * -_waveDir;
float timeNoise = sampleNoiseTexture(_uv * TIME_NOISE_TEXTURE_REPEAT) * TIME_NOISE_STRENGTH;
vec3 uv_A = computeUVPerturbedWeigth(_dudv, _uv + waveMovement, time + timeNoise, 0.0);
vec3 uv_B = computeUVPerturbedWeigth(_dudv, _uv + waveMovement, time + timeNoise, 0.5);
vec3 normal_A = textureDenormalized3D(_texNormal, uv_A.xy) * uv_A.z;
vec3 normal_B = textureDenormalized3D(_texNormal, uv_B.xy) * uv_B.z;
vec3 mixNormal = normalize(normal_A + normal_B);
mixNormal.xy *= waveStrength;
mixNormal.z = sqrt(1.0 - dot(mixNormal.xy, mixNormal.xy));
return mixNormal;
}
vec4 getSurfaceNormalAndFoam(vec2 _uv, float _time) {
float waveTextureRepeat = waveParams[1];
vec3 normal = getWaveLayer(texWaveNormal, texWavePerturbation, _uv * waveTextureRepeat, waveDirection, _time);
float foam  = normals2FoamIntensity(normal, waveParams[0]);
return vec4(normal, foam);
}`)}function Ii(e,t){1===t.viewingMode?e.vertex.code.add(Ee`vec3 getLocalUp(in vec3 pos, in vec3 origin) {
return normalize(pos + origin);
}`):e.vertex.code.add(Ee`vec3 getLocalUp(in vec3 pos, in vec3 origin) {
return vec3(0.0, 0.0, 1.0);
}`),1===t.viewingMode?e.vertex.code.add(Ee`mat3 getTBNMatrix(in vec3 n) {
vec3 t = normalize(cross(vec3(0.0, 0.0, 1.0), n));
vec3 b = normalize(cross(n, t));
return mat3(t, b, n);
}`):e.vertex.code.add(Ee`mat3 getTBNMatrix(in vec3 n) {
vec3 t = vec3(1.0, 0.0, 0.0);
vec3 b = normalize(cross(n, t));
return mat3(t, b, n);
}`)}function Fi(e,t){e.include(Dt,t),e.include(Qt),e.include(zi),t.ssrEnabled&&e.include(Pi,t),e.fragment.constants.add("fresnelSky","vec3",[.02,1,15]).add("fresnelMaterial","vec2",[.02,.1]).add("roughness","float",.015).add("foamIntensityExternal","float",1.7).add("ssrIntensity","float",.65).add("ssrHeightFadeStart","float",3e5).add("ssrHeightFadeEnd","float",5e5).add("waterDiffusion","float",.775).add("waterSeeColorMod","float",.8).add("correctionViewingPowerFactor","float",.4).add("skyZenitColor","vec3",[.52,.68,.9]).add("skyColor","vec3",[.67,.79,.9]),e.fragment.code.add(Ee`PBRShadingWater shadingInfo;
vec3 getSkyGradientColor(in float cosTheta, in vec3 horizon, in vec3 zenit) {
float exponent = pow((1.0 - cosTheta), fresnelSky[2]);
return mix(zenit, horizon, exponent);
}`),e.fragment.code.add(Ee`vec3 getSeaColor(in vec3 n, in vec3 v, in vec3 l, vec3 color, in vec3 lightIntensity, in vec3 localUp, in float shadow, float foamIntensity, vec3 positionView) {
float reflectionHit = 0.;
vec3 seaWaterColor = linearizeGamma(color);
vec3 h = normalize(l + v);
shadingInfo.NdotL = clamp(dot(n, l), 0.0, 1.0);
shadingInfo.NdotV = clamp(dot(n, v), 0.001, 1.0);
shadingInfo.VdotN = clamp(dot(v, n), 0.001, 1.0);
shadingInfo.NdotH = clamp(dot(n, h), 0.0, 1.0);
shadingInfo.VdotH = clamp(dot(v, h), 0.0, 1.0);
shadingInfo.LdotH = clamp(dot(l, h), 0.0, 1.0);
float upDotV = max(dot(localUp,v), 0.0);
vec3 skyHorizon = linearizeGamma(skyColor);
vec3 skyZenit = linearizeGamma(skyZenitColor);
vec3 skyColor = getSkyGradientColor(upDotV, skyHorizon, skyZenit );
float upDotL = max(dot(localUp,l),0.0);
float daytimeMod = 0.1 + upDotL * 0.9;
skyColor *= daytimeMod;
float shadowModifier = clamp(shadow, 0.8, 1.0);
vec3 fresnelModifier = fresnelReflection(shadingInfo.VdotN, vec3(fresnelSky[0]), fresnelSky[1]);
vec3 reflSky = fresnelModifier * skyColor * shadowModifier;
vec3 reflSea = seaWaterColor * mix(skyColor, upDotL * lightIntensity * LIGHT_NORMALIZATION, 2.0 / 3.0) * shadowModifier;
vec3 specular = vec3(0.0);
if(upDotV > 0.0 && upDotL > 0.0) {
vec3 specularSun = brdfSpecularWater(shadingInfo, roughness, vec3(fresnelMaterial[0]), fresnelMaterial[1]);
vec3 incidentLight = lightIntensity * LIGHT_NORMALIZATION * shadow;
specular = shadingInfo.NdotL * incidentLight * specularSun;
}
vec3 foam = vec3(0.0);
if(upDotV > 0.0) {
foam = foamIntensity2FoamColor(foamIntensityExternal, foamIntensity, skyZenitColor, daytimeMod);
}`),t.ssrEnabled?e.fragment.code.add(Ee`vec4 viewPosition = vec4(positionView.xyz, 1.0);
vec3 viewDir = normalize(viewPosition.xyz);
vec4 viewNormalVectorCoordinate = ssrViewMat *vec4(n, 0.0);
vec3 viewNormal = normalize(viewNormalVectorCoordinate.xyz);
vec4 viewUp = ssrViewMat *vec4(localUp, 0.0);
float correctionViewingFactor = pow(max(dot(-viewDir, viewUp.xyz), 0.0), correctionViewingPowerFactor);
vec3 viewNormalCorrected = mix(viewUp.xyz, viewNormal, correctionViewingFactor);
vec3 reflected = normalize(reflect(viewDir, viewNormalCorrected));
vec3 hitCoordinate = screenSpaceIntersection( normalize(reflected), viewPosition.xyz, viewDir, viewUp.xyz);
vec3 reflectedColor = vec3(0.0);
if (hitCoordinate.z > 0.0)
{
vec2 reprojectedCoordinate = reprojectionCoordinate(hitCoordinate);
vec2 dCoords = smoothstep(0.3, 0.6, abs(vec2(0.5, 0.5) - hitCoordinate.xy));
float heightMod = smoothstep(ssrHeightFadeEnd, ssrHeightFadeStart, -positionView.z);
reflectionHit = waterDiffusion * clamp(1.0 - (1.3*dCoords.y), 0.0, 1.0) * heightMod;
reflectedColor = linearizeGamma(texture2D(lastFrameColorMap, reprojectedCoordinate).xyz)* reflectionHit * fresnelModifier.y * ssrIntensity;
}
float seeColorMod =  mix(waterSeeColorMod, waterSeeColorMod*0.5, reflectionHit);
return tonemapACES((1. - reflectionHit) * reflSky + reflectedColor + reflSea * seeColorMod + specular + foam);
}`):e.fragment.code.add(Ee`return tonemapACES(reflSky + reflSea * waterSeeColorMod + specular + foam);
}`)}const Wi=Object.freeze({__proto__:null,build:function(e){const t=new Oe;return t.include(at,{linearDepth:!1}),t.attributes.add("position","vec3"),t.attributes.add("uv0","vec2"),t.vertex.uniforms.add("proj","mat4").add("view","mat4").add("localOrigin","vec3"),t.vertex.uniforms.add("waterColor","vec4"),0!==e.output&&7!==e.output||(t.include(Ii,e),t.include(Ot,e),t.varyings.add("vuv","vec2"),t.varyings.add("vpos","vec3"),t.varyings.add("vnormal","vec3"),t.varyings.add("vtbnMatrix","mat3"),e.multipassTerrainEnabled&&t.varyings.add("depth","float"),t.vertex.code.add(Ee`
      void main(void) {
        if (waterColor.a < ${Ee.float(Ie)}) {
          // Discard this vertex
          gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
          return;
        }

        vuv = uv0;
        vpos = position;

        vnormal = getLocalUp(vpos, localOrigin);
        vtbnMatrix = getTBNMatrix(vnormal);

        ${e.multipassTerrainEnabled?"depth = (view * vec4(vpos, 1.0)).z;":""}

        gl_Position = transformPosition(proj, view, vpos);
        ${0===e.output?"forwardLinearDepth();":""}
      }
    `)),e.multipassTerrainEnabled&&(t.fragment.include(Le),t.include(Ae,e)),7===e.output&&(t.include(ze,e),t.fragment.uniforms.add("waterColor","vec4"),t.fragment.code.add(Ee`
        void main() {
          discardBySlice(vpos);
          ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}

          gl_FragColor = vec4(waterColor.a);
        }
      `)),0===e.output&&(t.include(Mi,e),t.include(ze,e),e.receiveShadows&&t.include(Pt,e),t.include(Fi,e),t.fragment.uniforms.add("waterColor","vec4").add("lightingMainDirection","vec3").add("lightingMainIntensity","vec3").add("camPos","vec3").add("timeElapsed","float").add("view","mat4"),t.fragment.include(Me),t.fragment.code.add(Ee`
      void main() {
        discardBySlice(vpos);
        ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}
        vec3 localUp = vnormal;
        // the created normal is in tangent space
        vec4 tangentNormalFoam = getSurfaceNormalAndFoam(vuv, timeElapsed);

        // we rotate the normal according to the tangent-bitangent-normal-Matrix
        vec3 n = normalize(vtbnMatrix * tangentNormalFoam.xyz);
        vec3 v = -normalize(vpos - camPos);
        float shadow = ${e.receiveShadows?Ee`1.0 - readShadowMap(vpos, linearDepth)`:"1.0"};
        vec4 vPosView = view*vec4(vpos, 1.0);
        vec4 final = vec4(getSeaColor(n, v, lightingMainDirection, waterColor.rgb, lightingMainIntensity, localUp, shadow, tangentNormalFoam.w, vPosView.xyz), waterColor.w);

        // gamma correction
        gl_FragColor = delinearizeGamma(final);
        gl_FragColor = highlightSlice(gl_FragColor, vpos);
        ${e.OITEnabled?"gl_FragColor = premultiplyAlpha(gl_FragColor);":""}
      }
    `)),2===e.output&&(t.include(Ii,e),t.include(Mi,e),t.include(ze,e),t.varyings.add("vpos","vec3"),t.varyings.add("vuv","vec2"),t.vertex.code.add(Ee`
        void main(void) {
          if (waterColor.a < ${Ee.float(Ie)}) {
            // Discard this vertex
            gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
            return;
          }

          vuv = uv0;
          vpos = position;

          gl_Position = transformPosition(proj, view, vpos);
        }
    `),t.fragment.uniforms.add("timeElapsed","float"),t.fragment.code.add(Ee`void main() {
discardBySlice(vpos);
vec4 tangentNormalFoam = getSurfaceNormalAndFoam(vuv, timeElapsed);
tangentNormalFoam.xyz = normalize(tangentNormalFoam.xyz);
gl_FragColor = vec4((tangentNormalFoam.xyz + vec3(1.0)) * 0.5, tangentNormalFoam.w);
}`)),5===e.output&&(t.varyings.add("vpos","vec3"),t.vertex.code.add(Ee`
        void main(void) {
          if (waterColor.a < ${Ee.float(Ie)}) {
            // Discard this vertex
            gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
            return;
          }

          vpos = position;
          gl_Position = transformPosition(proj, view, vpos);
        }
    `),t.fragment.uniforms.add("waterColor","vec4"),t.fragment.code.add(Ee`void main() {
gl_FragColor = waterColor;
}`)),4===e.output&&(t.include(ot),t.varyings.add("vpos","vec3"),t.vertex.code.add(Ee`
      void main(void) {
        if (waterColor.a < ${Ee.float(Ie)}) {
          // Discard this vertex
          gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
          return;
        }

        vpos = position;
        gl_Position = transformPosition(proj, view, vpos);
      }
    `),t.include(ze,e),t.fragment.code.add(Ee`void main() {
discardBySlice(vpos);
outputHighlight();
}`)),t}});class ji extends Ne{constructor(e,t,r){super(e,t,r),this._textureRepository=e.waterTextureRepository}initializeProgram(e){const t=ji.shader.get(),r=this.configuration,i=t.build({OITEnabled:0===r.transparencyPassType,output:r.output,viewingMode:e.viewingMode,slicePlaneEnabled:r.slicePlaneEnabled,sliceHighlightDisabled:!1,sliceEnabledForVertexPrograms:!1,receiveShadows:r.receiveShadows,pbrMode:3,useCustomDTRExponentForWater:!0,ssrEnabled:r.useSSR,highStepCount:!0,multipassTerrainEnabled:r.multipassTerrainEnabled,cullAboveGround:r.cullAboveGround});return new Ve(e.rctx,i,nt)}bindPass(e,t){var r,i;He(this.program,t.camera.projectionMatrix),t.multipassTerrainEnabled&&(this.program.setUniform2fv("cameraNearFar",t.camera.nearFar),this.program.setUniform2fv("inverseViewport",t.inverseViewport),Ge(this.program,t)),0===this.configuration.output&&(t.lighting.setUniforms(this.program,!1),Li(this.program,t)),0!==this.configuration.output&&2!==this.configuration.output||(r=this.program,i=e,r.setUniform4f("waveParams",i.waveStrength,i.waveTextureRepeat,i.flowStrength,i.flowOffset),r.setUniform2f("waveDirection",i.waveDirection[0]*i.waveVelocity,i.waveDirection[1]*i.waveVelocity),this._textureRepository.bind(this.program)),this.program.setUniform4fv("waterColor",e.color),4===this.configuration.output&&Ue(this.program,t)}bindDraw(e){Be(this.program,e),this.program.rebindTextures(),0!==this.configuration.output&&7!==this.configuration.output||qe(this.program,e.origin,e.camera.viewInverseTransposeMatrix),0===this.configuration.output&&Lt(this.program,e),0!==this.configuration.output&&7!==this.configuration.output&&4!==this.configuration.output||ke(this.program,this.configuration,e)}setPipelineState(e){const t=this.configuration,r=3===e,i=2===e;return At({blending:2!==t.output&&4!==t.output&&t.transparent?r?zt:Mt(e):null,depthTest:{func:It(e)},depthWrite:r?t.writeDepth&&Ft:Wt(e),colorWrite:jt,polygonOffset:r||i?null:Vt(t.enableOffset)})}initializePipeline(){return this.setPipelineState(this.configuration.transparencyPassType)}}ji.shader=new We(Wi,(()=>Promise.resolve().then((()=>Wi))));class Ni extends je{constructor(){super(...arguments),this.output=0,this.receiveShadows=!1,this.slicePlaneEnabled=!1,this.transparent=!1,this.enableOffset=!0,this.writeDepth=!1,this.useSSR=!1,this.isDraped=!1,this.transparencyPassType=3,this.multipassTerrainEnabled=!1,this.cullAboveGround=!1}}K([Fe({count:8})],Ni.prototype,"output",void 0),K([Fe()],Ni.prototype,"receiveShadows",void 0),K([Fe()],Ni.prototype,"slicePlaneEnabled",void 0),K([Fe()],Ni.prototype,"transparent",void 0),K([Fe()],Ni.prototype,"enableOffset",void 0),K([Fe()],Ni.prototype,"writeDepth",void 0),K([Fe()],Ni.prototype,"useSSR",void 0),K([Fe()],Ni.prototype,"isDraped",void 0),K([Fe({count:4})],Ni.prototype,"transparencyPassType",void 0),K([Fe()],Ni.prototype,"multipassTerrainEnabled",void 0),K([Fe()],Ni.prototype,"cullAboveGround",void 0);class Vi extends it{updateParameters(e){return this.ensureTechnique(ji,e)}setElapsedTimeUniform(e){const t=.001*this._material.animation.time;e.setUniform1f("timeElapsed",t*this._material.parameters.animationSpeed)}_updateShadowState(e){e.shadowMappingEnabled!==this._material.parameters.receiveShadows&&this._material.setParameters({receiveShadows:e.shadowMappingEnabled})}_updateSSRState(e){e.ssrEnabled!==this._material.parameters.ssrEnabled&&this._material.setParameters({ssrEnabled:e.ssrEnabled})}ensureResources(e){const t=this._techniqueRep.constructionContext.waterTextureRepository;return t.ready||t.updating||t.loadTextures(e),t.ready?2:1}beginSlot(e){return 0===this._output&&(this._updateShadowState(e),this._updateSSRState(e)),this.updateParameters(e)}bind(e,t){t.bindPass(this._material.parameters,e),2!==this._output&&0!==this._output||this.setElapsedTimeUniform(t.program)}}const Hi={waveStrength:.06,waveTextureRepeat:32,waveDirection:pe(1,0),waveVelocity:.05,flowStrength:.015,flowOffset:-.5,animationSpeed:.35,color:[0,0,0,0],transparent:!0,writeDepth:!0,slicePlaneEnabled:!1,isDraped:!1,receiveShadows:!0,ssrEnabled:!1,...tt},Ui={"calm-small":{waveStrength:.005,perturbationStrength:.02,textureRepeat:12,waveVelocity:.01},"rippled-small":{waveStrength:.02,perturbationStrength:.09,textureRepeat:32,waveVelocity:.07},"slight-small":{waveStrength:.05,perturbationStrength:.07,textureRepeat:28,waveVelocity:.1},"moderate-small":{waveStrength:.075,perturbationStrength:.07,textureRepeat:24,waveVelocity:.1},"calm-medium":{waveStrength:.003125,perturbationStrength:.01,textureRepeat:8,waveVelocity:.02},"rippled-medium":{waveStrength:.035,perturbationStrength:.015,textureRepeat:12,waveVelocity:.07},"slight-medium":{waveStrength:.06,perturbationStrength:.015,textureRepeat:8,waveVelocity:.12},"moderate-medium":{waveStrength:.09,perturbationStrength:.03,textureRepeat:4,waveVelocity:.12},"calm-large":{waveStrength:.01,perturbationStrength:0,textureRepeat:4,waveVelocity:.05},"rippled-large":{waveStrength:.025,perturbationStrength:.01,textureRepeat:8,waveVelocity:.11},"slight-large":{waveStrength:.06,perturbationStrength:.02,textureRepeat:3,waveVelocity:.13},"moderate-large":{waveStrength:.14,perturbationStrength:.03,textureRepeat:2,waveVelocity:.15}};class Gi extends rt{constructor(e){super(e,Hi),this._techniqueConfig=new Ni,this.animation=new Di}getTechniqueConfig(e,t){return this._techniqueConfig.output=e,this._techniqueConfig.writeDepth=this.parameters.writeDepth,this._techniqueConfig.receiveShadows=this.parameters.receiveShadows,this._techniqueConfig.slicePlaneEnabled=this.parameters.slicePlaneEnabled,this._techniqueConfig.transparent=this.parameters.transparent,this._techniqueConfig.useSSR=this.parameters.ssrEnabled,this._techniqueConfig.isDraped=this.parameters.isDraped,this._techniqueConfig.transparencyPassType=t.transparencyPassType,this._techniqueConfig.enableOffset=t.camera.relativeElevation<Ht,this._techniqueConfig.multipassTerrainEnabled=t.multipassTerrainEnabled,this._techniqueConfig.cullAboveGround=t.cullAboveGround,this._techniqueConfig}update(e){const t=Math.min(e.camera.relativeElevation,e.camera.distance);this.animation.enabled=Math.sqrt(this.parameters.waveTextureRepeat/this.parameters.waveStrength)*t<Bi;const r=this.animation.advance(e);return this.animation.enabled&&r}intersect(e,t,r,i,s,a,o){lt(e,t,i,s,a,void 0,o)}requiresSlot(e,t){switch(k(t)){case 2:return 21===e;case 0:if(this.parameters.isDraped)return 20===e;break;case 4:return 2===e||20===e}let r=2;return this.parameters.transparent&&(r=this.parameters.writeDepth?4:7),e===r}createGLMaterial(e){if(0===e.output&&this.parameters.isDraped)return e.output=5,new Vi(e);switch(e.output){case 0:case 2:case 4:case 7:return new Vi(e)}return null}createBufferWriter(){return new J(Q)}}const Bi=35e3;class qi{constructor(e){this.first=e.from,this.count=e.to-e.from}}class ki{constructor(e=0,t=0){this.from=e,this.to=t}}class $i extends ki{constructor(e,t,r,i,s,a){super(t,r),this.id=e,this.isVisible=i,this.hasHighlights=s,this.hasOccludees=a}}function Zi(e){return Array.from(e.values()).sort(Ji)}function Ji(e,t){return e.from===t.from?e.to-t.to:e.from-t.from}function Qi(e,t){if(0===e.length)return void e.push(new qi(t));const r=e[e.length-1];if(s=t,(i=r).first+i.count>=s.from){const e=t.from-r.first+t.to-t.from;r.count=e}else e.push(new qi(t));var i,s}class Yi{constructor(e,t){this._pool=e,this._size=0,this._buffer=e.newBuffer(Xi(t))}dispose(){this._buffer=this._pool.deleteBuffer(this._buffer),this._size=0}release(){this.erase(0,this._size),this.dispose()}get vao(){return this._buffer.vao}get array(){return this._buffer.array}get size(){return this._size}grow(e){this._resize(this._size+e,!0).dispose()}alloc(e){return this._resize(e,!1)}_resize(e,t){let r;const i=function(e,t,r){if(t<=r)return e>=r?e:Xi(Math.max(2*e,r));if(e<=2*r)return e;return Xi(r)}(this._buffer.length,this._size,e);if(this._buffer.length!==i){const e=this._pool.newBuffer(i);t&&(e.array.set(this._buffer.array.subarray(0,Math.min(this._size,i))),e.vao.vertexBuffers.geometry.setSubData(e.array,0,0,e.array.byteLength)),r=this._buffer,this._buffer=e}const s=this._size;return this._size=e,r?{dispose:()=>{r.array.fill(0,0,s),this._pool.deleteBuffer(r)},copy:(e,t,i)=>this._buffer.array.set(r.array.subarray(t,i),e),hasNewBuffer:!0}:{dispose:()=>{},copy:(e,t,r)=>{e!==t&&this._buffer.array.copyWithin(e,t,r)},hasNewBuffer:!1}}erase(e,t){this._buffer.array.fill(0,e,t)}}function Xi(e){return 65536*Math.ceil(e/65536)}class Ki{constructor(e,t,r,i){this.vao=new ye(e,t,{geometry:r},{geometry:_e.createVertex(e,35044)}),this.array=new Float32Array(i),this.vao.vertexBuffers.geometry.setData(this.array)}dispose(){this.vao.dispose(!0)}get length(){return this.array.length}}const es=qt+1;class ts{constructor(e,t,r){this._rctx=e,this._locations=t,this._layout=r,this._cache=e.newCache(`MergedRenderer pool ${te()}`,rs)}dispose(){this._cache.destroy()}newBuffer(e){const t=e.toString(),r=this._cache.pop(t);if(i(r)){const e=r.pop();return r.length>0&&this._cache.put(t,r,e.array.byteLength*r.length,es),e}return new Ki(this._rctx,this._locations,this._layout,e)}deleteBuffer(e){const t=e.array.byteLength,r=e.array.length.toString(),s=this._cache.pop(r);return i(s)?(s.push(e),this._cache.put(r,s,t*s.length,-1)):this._cache.put(r,[e],t,-1),null}}function rs(e,t){if(0===t)return void e.forEach((e=>e.dispose()));const r=e.pop(),i=e.length*r.array.byteLength;return r.dispose(),i}class is{constructor(e,t,r){this._rctx=e,this._materialRepository=t,this._material=r,this.type="MergedRenderer",this._dataByOrigin=new Map,this._renderCommandData=new ne,this._hasHighlights=!1,this._hasOccludees=!1,this._glMaterials=new Y(this._material,this._materialRepository),this._bufferWriter=r.createBufferWriter(),this._bufferPool=new ts(e,r.vertexAttributeLocations,Bt(this._bufferWriter.vertexBufferLayout))}dispose(){this._glMaterials.destroy(),this._dataByOrigin.forEach((e=>e.buffer.dispose())),this._dataByOrigin.clear(),this._bufferPool.dispose()}get isEmpty(){return 0===this._dataByOrigin.size}get hasHighlights(){return this._hasHighlights}get hasOccludees(){return this._hasOccludees}get hasWater(){return!this.isEmpty&&this._material instanceof Gi}get rendersOccluded(){return!this.isEmpty&&1!==this._material.renderOccluded}modify(e){this.updateGeometries(e.updates),this.addAndRemoveGeometries(e.adds,e.removes),this.updateRenderCommands()}addAndRemoveGeometries(e,t){const r=this._bufferWriter,i=r.vertexBufferLayout.stride/4,s=this._dataByOrigin,a=function(e,t){const r=new Map;for(const t of e)as(r,t,!0);for(const e of t)as(r,e,!1);return r}(e,t);a.forEach(((e,t)=>{a.delete(t);const o=e.toAdd.reduce(((e,t)=>e+r.elementCount(t.data)),0);let n=s.get(t);if(null==n)Ce(0===e.toRemove.length),n=new ds(e.origin,new Yi(this._bufferPool,o*i)),s.set(t,n);else if(0===e.toAdd.length&&n.instances.size===e.toRemove.length)return n.buffer.dispose(),void s.delete(t);let l=0;n.instances.forEach((e=>l+=e.to-e.from));const d=e.toRemove.reduce(((e,t)=>e+r.elementCount(t.data)),0),c=n.buffer.size,h=(l+o-d)*i,p=hs;if(h<c/2?this.removeAndRebuild(n,e.toRemove,i,h,p):e.toRemove.length>0&&this.remove(n,e.toRemove,i,p),e.toAdd.length>0){const t=ps;Re(t,-e.origin[0],-e.origin[1],-e.origin[2]),this.add(n,e.toAdd,i,t,p)}const u=n.buffer.vao.vertexBuffers.geometry;ls(p),p.forAll((({from:e,to:t})=>{if(e<t){const r=n.buffer.array,i=4,s=e*i,a=t*i;u.setSubData(r,s,s,a)}})),p.clear(),n.drawCommandsDirty=!0}))}updateGeometries(e){const t=this._bufferWriter,r=t.vertexBufferLayout.stride/4;for(const i of e){const e=i.renderGeometry,s=this._dataByOrigin.get(e.origin.id),a=s&&s.instances.get(e.id);if(!a)return;const o=i.updateType;if(1&o&&(a.isVisible=e.instanceParameters.visible),9&o){const t=e.instanceParameters.visible;a.hasHighlights=!!e.instanceParameters.highlights&&t}if(16&o&&(a.hasOccludees=!!e.instanceParameters.occludees),6&o){const{array:i,vao:o}=s.buffer;Tt(e,us,fs),t.write({transformation:us,invTranspTransformation:fs},e.data,t.vertexBufferLayout.createView(i.buffer),a.from),Ce(a.from+t.elementCount(e.data)===a.to,"material VBO layout has changed"),o.vertexBuffers.geometry.setSubData(i,a.from*r*4,a.from*r*4,a.to*r*4)}s.drawCommandsDirty=!0}}updateRenderCommands(){this._hasHighlights=!1,this._hasOccludees=!1,this._dataByOrigin.forEach((e=>{e.hasHiddenInstances=!1,e.hasHighlights=!1,e.hasOccludees=!1,oe(e.instances,(t=>(t.isVisible?(t.hasHighlights&&(this._hasHighlights=!0,e.hasHighlights=!0),t.hasOccludees&&(this._hasOccludees=!0,e.hasOccludees=!0)):e.hasHiddenInstances=!0,e.hasHiddenInstances&&e.hasHighlights&&e.hasOccludees)))}));const e=e=>{if(e.drawCommandsDefault=null,e.drawCommandsHighlight=null,e.drawCommandsOccludees=null,e.drawCommandsShadowHighlightRest=null,0===e.instances.size)return;if(!os(e)){const t=this._bufferWriter.vertexBufferLayout.stride,r=4*e.buffer.size/t;return void(e.drawCommandsDefault=[{first:0,count:r}])}const t=Zi(e.instances);e.drawCommandsDefault=[],e.drawCommandsHighlight=[],e.drawCommandsOccludees=[],e.drawCommandsShadowHighlightRest=[];for(const r of t)r.isVisible&&(r.hasOccludees?Qi(e.drawCommandsOccludees,r):Qi(e.drawCommandsDefault,r),r.hasHighlights?Qi(e.drawCommandsHighlight,r):Qi(e.drawCommandsShadowHighlightRest,r))};this._dataByOrigin.forEach((t=>{t.drawCommandsDirty&&(e(t),t.drawCommandsDirty=!1)}))}updateLogic(e){return this._material.update(e)}render(e,t,s){if(null!=e&&!this._material.requiresSlot(e,t))return!1;const a=5===t||7===t;if(a&&!this._hasHighlights)return!1;const o=6===t,n=!(a||o);if(this._dataByOrigin.forEach((e=>{if(a&&!e.hasHighlights)return;const t=(a?e.drawCommandsHighlight:o&&os(e)?e.drawCommandsShadowHighlightRest:e.drawCommandsDefault)||null,r=n&&e.drawCommandsOccludees||null;(i(t)||i(r))&&this._renderCommandData.push(new cs(e.origin,e.buffer,t,r))})),0===this._renderCommandData.length)return!1;const l=this._rctx,d=this._glMaterials.load(l,t);if(r(d))return this._renderCommandData.clear(),!1;const c=d.beginSlot(s);return c.bindPipelineState(l,e,!1),l.useProgram(c.program),d.bind(s,c),this._renderCommandData.forAll((({origin:t,buffer:r,renderCommands:a,occludeeCommands:o})=>{s.origin=t,c.bindDraw(s),c.ensureAttributeLocations(r.vao),l.bindVAO(r.vao);const n=c.primitiveType;i(a)&&this.renderCommands(l,n,a),i(o)&&(c.bindPipelineState(l,e,!0),this.renderCommands(l,n,o),c.bindPipelineState(l,e,!1))})),this._renderCommandData.clear(),!0}renderCommands(e,t,r){for(let i=0;i<r.length;i++)e.drawArrays(t,r[i].first,r[i].count)}removeAndRebuild(e,t,r,i,s){for(const r of t)e.instances.delete(r.id);const a=Zi(e.instances);e.instances.clear();const o=e.buffer.size,n=e.buffer.alloc(i);let l=0;for(const t of a){const i=t.from*r,s=t.to*r;n.copy(l,i,s),t.from=l/r,l+=s-i,t.to=l/r,e.instances.set(t.id,t)}s.push(new ki(0,n.hasNewBuffer?e.buffer.array.length:o)),n.dispose(),e.buffer.erase(l,s.back().to),e.holes.clear()}remove(e,t,r,i){for(const s of t){const t=s.id,a=e.instances.get(t),o=a.from*r,n=a.to*r;e.buffer.erase(o,n),e.holes.push(new ki(a.from,a.to)),e.instances.delete(t),i.push(new ki(o,n))}ls(e.holes)}add(e,t,s,a,o){const n=this._bufferWriter;let l=n.vertexBufferLayout.createView(e.buffer.array.buffer);for(const d of t){const t=i(d.transformation)?M(us,a,d.transformation):a;I(fs,t);const c=F(fs,fs),h=n.elementCount(d.data),p=h*s;let u=ns(e.holes,h);r(u)&&(u=e.buffer.size/s,e.buffer.grow(p),l=n.vertexBufferLayout.createView(e.buffer.array.buffer)),n.write({transformation:t,invTranspTransformation:c},d.data,l,u);const f=d.instanceParameters.visible,m=!!d.instanceParameters.highlights&&f,g=!!d.instanceParameters.occludees,v=new $i(d.id,u,u+h,f,m,g);Ce(null==e.instances.get(d.id)),e.instances.set(d.id,v),o.push(new ki(v.from*s,v.to*s))}}get test(){return{material:this._material,glMaterials:this._glMaterials}}}class ss{constructor(e){this.origin=e,this.toAdd=new Array,this.toRemove=new Array}}function as(e,t,i){const s=t.origin;if(r(s))return;let a=e.get(s.id);null==a&&(a=new ss(s.vec3),e.set(s.id,a)),i?a.toAdd.push(t):a.toRemove.push(t)}function os(e){return e.hasOccludees||e.hasHighlights||e.hasHiddenInstances}function ns(e,t){let r;if(!e.some((e=>!(e.to-e.from<t)&&(r=e,!0))))return null;const i=r.from;return r.from+=t,r.from>=r.to&&e.removeUnordered(r),i}function ls(e){const t=new Map;e.forAll((e=>t.set(e.from,e)));let r=!0;for(;r;)r=!1,e.forEach((i=>{const s=t.get(i.to);s&&(i.to=s.to,t.delete(s.from),e.removeUnordered(s),r=!0)}))}class ds{constructor(e,t){this.origin=e,this.buffer=t,this.instances=new Map,this.holes=new ne({deallocator:null}),this.hasHiddenInstances=!1,this.hasHighlights=!1,this.hasOccludees=!1,this.drawCommandsDirty=!1}}class cs{constructor(e,t,r,i){this.origin=e,this.buffer=t,this.renderCommands=r,this.occludeeCommands=i}}const hs=new ne({deallocator:null}),ps=V(),us=V(),fs=V();let ms=class extends ee{constructor(){super(...arguments),this._pending=new gs,this._changes=new wi,this._materialRenderers=new Map,this._sortedMaterialRenderers=new ne,this._hasHighlights=!1,this._hasWater=!1}dispose(){this._changes.prune(),this._materialRenderers.forEach((e=>e.dispose())),this._materialRenderers.clear(),this._sortedMaterialRenderers.clear()}get updating(){return!this._pending.empty||this._changes.updates.length>0}get hasHighlights(){return this._hasHighlights}get hasWater(){return this._hasWater}get rendersOccluded(){return oe(this._materialRenderers,(e=>e.rendersOccluded))}get isEmpty(){return!this.updating&&0===this._materialRenderers.size}commitChanges(){if(!this.updating)return!1;this._processAddsRemoves();const e=Ri(this._changes);let t=!1,r=!1,i=!1;return e.forEach(((e,s)=>{let a=this._materialRenderers.get(s);if(!a&&e.adds.length>0&&(a=new is(this.rctx,this.materialRepository,s),this._materialRenderers.set(s,a),t=!0,r=!0,i=!0),!a)return;const o=r||a.hasHighlights,n=i||a.hasWater;a.modify(e),r=r||o!==a.hasHighlights,i=i||n!==a.hasWater,a.isEmpty&&(this._materialRenderers.delete(s),a.dispose(),t=!0)})),this._changes.clear(),t&&this.updateSortedMaterialRenderers(),r&&(this._hasHighlights=oe(this._materialRenderers,(e=>e.hasHighlights))),i&&(this._hasWater=oe(this._materialRenderers,(e=>e.hasWater))),this.notifyChange("updating"),!0}add(e){if(0===e.length)return;const t=this._pending.empty;for(const t of e)this._pending.adds.add(t);t&&this.notifyChange("updating")}remove(e){const t=this._pending.empty;for(const t of e)this._pending.adds.has(t)?(this._pending.removed.add(t),this._pending.adds.delete(t)):this._pending.removed.has(t)||this._pending.removes.add(t);t&&!this._pending.empty&&this.notifyChange("updating")}modify(e,t){const r=0===this._changes.updates.length;for(const r of e){const e=this._changes.updates.pushNew();e.renderGeometry=r,e.updateType=t}r&&this._changes.updates.length>0&&this.notifyChange("updating")}updateLogic(e){let t=!1;return this._sortedMaterialRenderers.forAll((({materialRenderer:r})=>t=r.updateLogic(e)||t)),t}render(e,t){for(let r=0;r<this._sortedMaterialRenderers.length;r++){const i=this._sortedMaterialRenderers.data[r];i.material.shouldRender(e)&&i.materialRenderer.render(t.slot,e.pass,t)}}updateSortedMaterialRenderers(){this._sortedMaterialRenderers.clear();let e=0;this._materialRenderers.forEach(((t,r)=>{r.insertOrder=e++,this._sortedMaterialRenderers.push({material:r,materialRenderer:t})})),this._sortedMaterialRenderers.sort(((e,t)=>{const r=t.material.renderPriority-e.material.renderPriority;return 0!==r?r:e.material.insertOrder-t.material.insertOrder}))}_processAddsRemoves(){this._changes.adds.clear(),this._changes.removes.clear(),this._changes.adds.pushArray(Array.from(this._pending.adds)),this._changes.removes.pushArray(Array.from(this._pending.removes));for(let e=0;e<this._changes.updates.length;){const t=this._changes.updates.data[e];this._pending.has(t.renderGeometry)?this._changes.updates.removeUnorderedIndex(e):e++}this._pending.clear()}get test(){return{sortedMaterialRenderers:this._sortedMaterialRenderers}}};K([ce()],ms.prototype,"rctx",void 0),K([ce()],ms.prototype,"materialRepository",void 0),K([ce()],ms.prototype,"updating",null),ms=K([he("esri.views.3d.webgl-engine.lib.SortedRenderGeometryRenderer")],ms);class gs{constructor(){this.adds=new Set,this.removes=new Set,this.removed=new Set}get empty(){return 0===this.adds.size&&0===this.removes.size&&0===this.removed.size}has(e){return this.adds.has(e)||this.removes.has(e)||this.removed.has(e)}clear(){this.adds.clear(),this.removes.clear(),this.removed.clear()}}const vs=Object.freeze({__proto__:null,build:function(){const e=new Oe;return e.include(X),e.fragment.uniforms.add("tex","sampler2D"),e.fragment.uniforms.add("color","vec4"),e.fragment.code.add(Ee`void main() {
vec4 texColor = texture2D(tex, uv);
gl_FragColor = texColor * color;
}`),e}});class ys extends Ne{initializeProgram(e){const t=ys.shader.get().build();return new Ve(e.rctx,t,nt)}initializePipeline(){return this.configuration.hasAlpha?At({blending:Ut(770,1,771,771),colorWrite:jt}):At({colorWrite:jt})}}ys.shader=new We(vs,(()=>Promise.resolve().then((()=>vs))));class _s extends je{constructor(){super(...arguments),this.hasAlpha=!1}}function bs(e,t,r){(r=r||e).length=e.length;for(let i=0;i<e.length;i++)r[i]=e[i]*t[i];return r}function Ss(e,t,r){(r=r||e).length=e.length;for(let i=0;i<e.length;i++)r[i]=e[i]*t;return r}function xs(e,t,r){(r=r||e).length=e.length;for(let i=0;i<e.length;i++)r[i]=e[i]+t[i];return r}function ws(e){return(e+1)*(e+1)}function Cs(e,t,r){const i=e[0],s=e[1],a=e[2],o=r||[];return o.length=ws(t),t>=0&&(o[0]=.28209479177),t>=1&&(o[1]=.4886025119*i,o[2]=.4886025119*a,o[3]=.4886025119*s),t>=2&&(o[4]=1.09254843059*i*s,o[5]=1.09254843059*s*a,o[6]=.31539156525*(3*a*a-1),o[7]=1.09254843059*i*a,o[8]=.54627421529*(i*i-s*s)),o}function Ts(e,t){const r=(i=t.r.length,c(Math.floor(Math.sqrt(i)-1),0,2));var i;for(const i of e)b(As,i.direction),Cs(As,r,Ps),bs(Ps,zs),Ss(Ps,i.intensity[0],Ls),xs(t.r,Ls),Ss(Ps,i.intensity[1],Ls),xs(t.g,Ls),Ss(Ps,i.intensity[2],Ls),xs(t.b,Ls);return t}function Rs(e,t,r,i){!function(e,t){const r=ws(e),i=t||{r:[],g:[],b:[]};i.r.length=i.g.length=i.b.length=r;for(let e=0;e<r;e++)i.r[e]=i.g[e]=i.b[e]=0}(t,i),h(r.intensity,0,0,0);let s=!1;const a=Es,o=Ds,n=Os;a.length=0,o.length=0,n.length=0;for(const t of e)t instanceof Kt&&!s?(g(r.direction,t.direction),r.intensity[0]=t.intensity[0],r.intensity[1]=t.intensity[1],r.intensity[2]=t.intensity[2],r.castShadows=t.castShadows,s=!0):t instanceof Kt||t instanceof Xt?a.push(t):t instanceof Yt?o.push(t):t instanceof er&&n.push(t);Ts(a,i),function(e,t){Cs(As,0,Ps);for(const r of e)t.r[0]+=Ps[0]*zs[0]*r.intensity[0]*4*Math.PI,t.g[0]+=Ps[0]*zs[0]*r.intensity[1]*4*Math.PI,t.b[0]+=Ps[0]*zs[0]*r.intensity[2]*4*Math.PI}(o,i);for(const e of n)xs(i.r,e.r),xs(i.g,e.g),xs(i.b,e.b)}K([Fe()],_s.prototype,"hasAlpha",void 0);const Es=[],Ds=[],Os=[],Ps=[0],Ls=[0],As=n(),zs=[3.141593,2.094395,2.094395,2.094395,.785398,.785398,.785398,.785398,.785398];class Ms{constructor(){this._shOrder=2,this._ambientBoost=.4,this._oldSunlight={direction:n(),ambient:{color:n(),intensity:1},diffuse:{color:n(),intensity:1}},this.globalFactor=.5,this.groundLightingFactor=.5,this._sphericalHarmonics=new er,this._mainLight={intensity:n(),direction:l(1,0,0),castShadows:!1}}get lightingMainDirection(){return this._mainLight.direction}setLightDirectionUniform(e){e.setUniform3fv("lightingMainDirection",this._mainLight.direction)}setUniforms(e,t=!1){const r=t?(1-this.groundLightingFactor)*(1-this.globalFactor):0;e.setUniform1f("lightingFixedFactor",r),e.setUniform1f("lightingGlobalFactor",this.globalFactor),this.setLightDirectionUniform(e),e.setUniform3fv("lightingMainIntensity",this._mainLight.intensity),e.setUniform1f("ambientBoostFactor",this._ambientBoost);const i=this._sphericalHarmonics;0===this._shOrder?e.setUniform3f("lightingAmbientSH0",i.r[0],i.g[0],i.b[0]):1===this._shOrder?(e.setUniform4f("lightingAmbientSH_R",i.r[0],i.r[1],i.r[2],i.r[3]),e.setUniform4f("lightingAmbientSH_G",i.g[0],i.g[1],i.g[2],i.g[3]),e.setUniform4f("lightingAmbientSH_B",i.b[0],i.b[1],i.b[2],i.b[3])):2===this._shOrder&&(e.setUniform3f("lightingAmbientSH0",i.r[0],i.g[0],i.b[0]),e.setUniform4f("lightingAmbientSH_R1",i.r[1],i.r[2],i.r[3],i.r[4]),e.setUniform4f("lightingAmbientSH_G1",i.g[1],i.g[2],i.g[3],i.g[4]),e.setUniform4f("lightingAmbientSH_B1",i.b[1],i.b[2],i.b[3],i.b[4]),e.setUniform4f("lightingAmbientSH_R2",i.r[5],i.r[6],i.r[7],i.r[8]),e.setUniform4f("lightingAmbientSH_G2",i.g[5],i.g[6],i.g[7],i.g[8]),e.setUniform4f("lightingAmbientSH_B2",i.b[5],i.b[6],i.b[7],i.b[8]))}set(e){Rs(e,this._shOrder,this._mainLight,this._sphericalHarmonics),g(this._oldSunlight.direction,this._mainLight.direction);const t=1/Math.PI;this._oldSunlight.ambient.color[0]=.282095*this._sphericalHarmonics.r[0]*t,this._oldSunlight.ambient.color[1]=.282095*this._sphericalHarmonics.g[0]*t,this._oldSunlight.ambient.color[2]=.282095*this._sphericalHarmonics.b[0]*t,f(this._oldSunlight.diffuse.color,this._mainLight.intensity,t),g(Is,this._oldSunlight.diffuse.color),f(Is,Is,this._ambientBoost*this.globalFactor),m(this._oldSunlight.ambient.color,this._oldSunlight.ambient.color,Is)}get old(){return this._oldSunlight}}const Is=n();class Fs{constructor(e){this._rctx=e,this.cache=new Map}dispose(){this.cache.forEach((e=>e.texture=t(e.texture))),this.cache.clear()}acquire(e){if(r(e))return null;const t=this.patternId(e),i=this.cache.get(t);if(i)return i.refCount++,i.bind;const s=e.pixelRatio,{encodedData:a,sdfNormalizer:o,pixels:n,paddedPixels:l}=function(e,t){const r=e.map((e=>Math.round(e*t))),i=1/t,s=Math.floor(r.reduce(((e,t)=>e+t))),a=r.reduce(((e,t)=>Math.max(e,t))),o=(Math.floor(.5*(a-1))+.5)*i,n=[];let l=1;for(const e of r){for(let t=0;t<e;t++){const r=l*(Math.min(t,e-1-t)+.5)*i/o*.5+.5;n.push(r)}l=-l}const d=Math.round(r[0]/2),c=[...n.slice(d),...n.slice(0,d)],h=s+2,p=new Uint8Array(4*h);let u=4;for(const e of c)kt(e,p,u),u+=4;return p.copyWithin(0,u-4,u),p.copyWithin(u,4,8),{encodedData:p,sdfNormalizer:o,paddedPixels:h,pixels:s}}(e.pattern,s),d=n/s,c={refCount:1,texture:null,bind:e=>(r(c.texture)&&(c.texture=new xe(this._rctx,{width:l,height:1,internalFormat:6408,pixelFormat:6408,dataType:5121,wrapMode:33071},a)),e.bindTexture(c.texture,"stipplePatternTexture"),{pixelSize:d,sdfNormalizer:o,pixels:n})};return this.cache.set(t,c),c.bind}release(e){if(r(e))return;const t=this.patternId(e),s=this.cache.get(t);s&&(s.refCount--,0===s.refCount&&(i(s.texture)&&s.texture.dispose(),this.cache.delete(t)))}swap(e,t){const r=this.acquire(t);return this.release(e),r}patternId(e){return`${e.pattern.join(",")}-r${e.pixelRatio}`}}const Ws=ae.getLogger("esri.views.3d.webgl-engine.lib.OverlayRenderer");let js=class extends(Er(ee)){constructor(e){super(e),this._overlays=null,this._overlayRenderTarget=null,this._hasHighlights=!1,this._rendersOccluded=!1,this._hasWater=!1,this._lighting=new Ms,this._handles=new se,this._frameTask=$t,this._layerRenderers=new Map,this._sortedLayerRenderersDirty=!1,this._sortedLayerRenderers=new ne,this._geometries=new Map,this.worldToPCSRatio=1,this.events=new ie,this.longitudeCyclical=null}initialize(){const e=this.view._stage.renderView;this._rctx=e.renderingContext,this._renderContext=new bi(this._rctx);const t=e.waterTextureRepository;this._stippleTextureRepository=new Fs(e.renderingContext),this._shaderTechniqueRepository=new Tr({rctx:this._rctx,viewingMode:2,stippleTextureRepository:this._stippleTextureRepository,waterTextureRepository:t}),this._handles.add([de(t,"loadingState",(()=>this.events.emit("content-changed"))),de(this,"spatialReference",(e=>this._localOrigins=new vi(e)))]),this._materialRepository=new Ar(e.textureRepository,this._shaderTechniqueRepository,(e=>{(e.renderOccluded&qs)>0!==this._rendersOccluded&&this.updateRendersOccluded(),this.events.emit("content-changed"),this.notifyChange("updating")}),(()=>this.events.emit("content-changed"))),this._lighting.groundLightingFactor=1,this._lighting.globalFactor=0,this._lighting.set([new Yt(l(1,1,1))]),this._bindParameters={slot:20,highlightDepthTexture:dt(this._rctx),camera:Hs,inverseViewport:ue(),origin:null,screenToWorldRatio:null,screenToPCSRatio:null,shadowMappingEnabled:!1,slicePlane:null,ssaoEnabled:!1,hasOccludees:!1,linearDepthTexture:null,lastFrameColorTexture:null,reprojectionMatrix:H,ssrEnabled:!1,lighting:this._lighting,transparencyPassType:3,terrainLinearDepthTexture:null,geometryLinearDepthTexture:null,multipassTerrainEnabled:!1,cullAboveGround:!1,multipassGeometryEnabled:!1,highlightColorTexture:null},this._frameTask=this.view.resourceController.scheduler.registerTask(Zt.STAGE,this),this._handles.add(this._frameTask)}dispose(){this._handles.destroy(),this._layerRenderers.forEach((e=>e.dispose())),this._layerRenderers.clear(),this._debugTextureTechnique=a(this._debugTextureTechnique),this._debugPatternTexture=t(this._debugPatternTexture),this._bindParameters.highlightDepthTexture=t(this._bindParameters.highlightDepthTexture),this._shaderTechniqueRepository=t(this._shaderTechniqueRepository),this._temporaryFBO=t(this._temporaryFBO),this._quadVAO=t(this._quadVAO),this.disposeOverlays()}get updating(){return this._sortedLayerRenderersDirty||this._frameTask.updating||oe(this._layerRenderers,(e=>e.updating))}get hasOverlays(){return i(this._overlays)&&i(this._overlayRenderTarget)}get gpuMemoryUsage(){return i(this._overlayRenderTarget)?this._overlayRenderTarget.gpuMemoryUsage:0}collectUnusedRenderTargetMemory(e){let t=!1;if(i(this._overlayRenderTarget))for(const r of this._overlayRenderTarget.renderTargets){const i=this.overlays[0].validTargets[r.type]||!this.overlays[1].validTargets[r.type];t=this._overlayRenderTarget.validateUsageForTarget(i,r,e)||t}return t}get overlays(){return e(this._overlays,[])}ensureDrapeTargets(e){i(this._overlays)&&this._overlays.forEach((t=>{t.hasTargetWithoutRasterImage=re(e,(e=>1===e.drapeTargetType))}))}ensureDrapeSources(e){i(this._overlays)&&this._overlays.forEach((t=>{t.hasDrapedFeatureSource=oe(e,((e,t)=>1===t.drapeSourceType)),t.hasDrapedRasterSource=oe(e,((e,t)=>0===t.drapeSourceType))}))}ensureOverlays(e,t){r(this._overlays)&&(this._overlayRenderTarget=new Sr(this._rctx),this._overlays=[new yr(0,this._overlayRenderTarget),new yr(1,this._overlayRenderTarget)]),this.ensureDrapeTargets(e),this.ensureDrapeSources(t)}disposeOverlays(){this._overlays=null,this._overlayRenderTarget=t(this._overlayRenderTarget),this.events.emit("textures-disposed")}get running(){return this.updating}runTask(e,t=(()=>!0)){this._frameTask.processQueue(e),e.done||this._processLayers(e,t)}_processLayers(e,t){let r=!1;for(const[i,s]of this._layerRenderers){if(e.done)break;(i.destroyed||t(i))&&(s.commitChanges()&&(r=!0,e.madeProgress()),s.isEmpty&&(r=!0,this._sortedLayerRenderersDirty=!0,this._layerRenderers.delete(i),this._handles.remove(i)))}this.updateSortedLayerRenderers(),r&&(i(this._overlays)&&0===this._layerRenderers.size&&this.disposeOverlays(),this.notifyChange("updating"),this.events.emit("content-changed"),this.updateHasHighlights(),this.updateRendersOccluded(),this.updateHasWater())}processSyncLayers(){this._processLayers(Jt,(e=>1===e.updatePolicy))}addGeometries(e,t,i){for(const t of e)r(t.origin)&&(t.origin=this._localOrigins.getOrigin(t.boundingSphere)),this._geometries.set(t.id,t);this.ensureLayerRenderer(t).add(e),2===i&&this.notifyGraphicGeometryChanged(e,t)}removeGeometries(e,t,r){for(const t of e)this._geometries.delete(s(t.id));const i=this._layerRenderers.get(t);i&&(i.remove(e),2===r&&this.notifyGraphicGeometryChanged(e,t))}updateGeometries(e,t,r){const i=this._layerRenderers.get(t);if(i)switch(i.modify(e,r),r){case 4:case 2:return this.notifyGraphicGeometryChanged(e,t);case 1:return this.notifyGraphicVisibilityChanged(e,t)}else Ws.warn("Attempted to update geometry for nonexistent layer")}notifyGraphicGeometryChanged(e,t){if(r(t.notifyGraphicGeometryChanged))return;let s;for(const r of e){const e=r.graphicUid;i(e)&&e!==s&&(t.notifyGraphicGeometryChanged(e),s=e)}}notifyGraphicVisibilityChanged(e,t){if(r(t.notifyGraphicVisibilityChanged))return;let s;for(const r of e){const e=r.graphicUid;i(e)&&e!==s&&(t.notifyGraphicVisibilityChanged(e),s=e)}}updateHighlights(e,t){const r=this._layerRenderers.get(t);r?r.modify(e,8):Ws.warn("Attempted to update highlights for nonexistent layer")}isEmpty(){return 0===this._geometries.size&&!rr.OVERLAY_DRAW_DEBUG_TEXTURE}get hasHighlights(){return this._hasHighlights}get hasWater(){return this._hasWater}get rendersOccluded(){return this._rendersOccluded}updateLogic(e){let t=!1;return this._layerRenderers.forEach((r=>t=r.updateLogic(e)||t)),t}updateLayerOrder(){this._sortedLayerRenderersDirty=!0}drawTarget(e,t,r){const a=e.canvasGeometries;if(0===a.numViews)return!1;this._screenToWorldRatio=r*e.mapUnitsPerPixel;const o=t.renderPass;if(this.isEmpty()||5===o&&!this.hasHighlights||3===o&&!this.hasWater||!e.hasSomeSizedView())return!1;const n=t.fbo;if(!n.isValid())return!1;const l=2*e.resolution,d=e.resolution;n.resize(l,d);const c=this._rctx;Hs.pixelRatio=e.pixelRatio*r,this._renderContext.pass=o,this._bindParameters.screenToWorldRatio=this._screenToWorldRatio,this._bindParameters.screenToPCSRatio=this._screenToWorldRatio*this.worldToPCSRatio,this._bindParameters.slot=3===o?21:20,e.applyViewport(this._rctx),n.bind(c),0===e.index&&(c.setClearColor(0,0,0,0),c.clearSafe(16384));const h=1===t.type?2:4===t.type?1:0;if(1===h&&(this._renderContext.renderOccludedMask=qs),rr.OVERLAY_DRAW_DEBUG_TEXTURE&&1!==h)for(let t=0;t<a.numViews;t++)this.setViewParameters(a.extents[t],e,Hs),this.drawDebugTexture(e.resolution,Vs[e.index]);return this._layerRenderers.size>0&&this._sortedLayerRenderers.forAll((({layerView:t,renderer:r})=>{if(2===h&&i(t)&&0===t.drapeSourceType)return;const p=i(t)&&i(t.fullOpacity)&&t.fullOpacity<1&&0===o;p&&(this.bindTemporaryFramebuffer(this._rctx,l,d),c.clearSafe(16384));for(let t=0;t<a.numViews;t++)this.setViewParameters(a.extents[t],e,Hs),r.render(this._renderContext,this._bindParameters);p&&i(this._temporaryFBO)&&(n.bind(c),this.view._stage.renderView.compositingHelper.composite(this._temporaryFBO.getTexture(),2,s(s(t).fullOpacity),3,e.index))})),c.bindFramebuffer(null),n.generateMipMap(),this._renderContext.resetRenderOccludedMask(),!0}bindTemporaryFramebuffer(e,t,i){r(this._temporaryFBO)&&(this._temporaryFBO=new br(e,!1)),this._temporaryFBO.resize(t,i),this._temporaryFBO.bind(e)}async reloadShaders(){await this._shaderTechniqueRepository.reloadAll()}intersect(e,t,r,i){let s=0;this._geometries.forEach((a=>{if(i&&!i(a))return;this._intersectRenderGeometry(a,r,t,0,e,s);const o=this.longitudeCyclical;o&&(a.boundingSphere[0]-a.boundingSphere[3]<o.min&&this._intersectRenderGeometry(a,r,t,o.range,e,s),a.boundingSphere[0]+a.boundingSphere[3]>o.max&&this._intersectRenderGeometry(a,r,t,-o.range,e,s)),s++}))}_intersectRenderGeometry(e,t,r,s,a,o){if(!e.instanceParameters.visible)return;let n=0;i(e.transformation)&&(s+=e.transformation[12],n=e.transformation[13]),Us[0]=r[0]-s,Us[1]=r[1]-n,Us[2]=1,Gs[0]=r[0]-s,Gs[1]=r[1]-n,Gs[2]=0,e.screenToWorldRatio=this._screenToWorldRatio,e.material.intersect(e,null,e.getShaderTransformation(),a,Us,Gs,((r,i,s)=>{!function(e,t,r,i,s,a,o){const n={layerUid:a,graphicUid:o,triangleNr:t},l=t=>{t.set(3,n,e.dist,e.normal,e.transformation,r,i)};(null==s.results.min.drapedLayerOrder||r>=s.results.min.drapedLayerOrder)&&(null==s.results.min.dist||s.results.ground.dist<=s.results.min.dist)&&l(s.results.min);0!==s.options.store&&(null==s.results.max.drapedLayerOrder||r<s.results.max.drapedLayerOrder)&&(null==s.results.max.dist||s.results.ground.dist>s.results.max.dist)&&l(s.results.max);if(2===s.options.store){const e=Gt(s.ray);l(e),s.results.all.push(e)}}(t,s,e.material.renderPriority,o,a,e.layerUid,e.graphicUid)}),e.calculateShaderTransformation,t)}ensureLayerRenderer(e){let t=this._layerRenderers.get(e);return t||(t=new ms({rctx:this._rctx,materialRepository:this._materialRepository}),this._layerRenderers.set(e,t),this._sortedLayerRenderersDirty=!0,"fullOpacity"in e&&this._handles.add(e.watch("fullOpacity",(()=>this.events.emit("content-changed"))),e),this._handles.add(de(t,"updating",(()=>this.notifyChange("updating"))),e)),t}updateSortedLayerRenderers(){if(!this._sortedLayerRenderersDirty)return;if(this._sortedLayerRenderersDirty=!1,this._sortedLayerRenderers.clear(),0===this._layerRenderers.size)return;const e=new Set(this._layerRenderers.values());this.view.allLayerViews.forEach((t=>{const r=t,i=this._layerRenderers.get(r);i&&(this._sortedLayerRenderers.push(new Ns(r,i)),e.delete(i))})),e.forEach((e=>this._sortedLayerRenderers.push(new Ns(null,e))))}setViewParameters(e,t,r){r.viewport[0]=r.viewport[1]=0,r.viewport[2]=r.viewport[3]=t.resolution,W(r.projectionMatrix,0,e[2]-e[0],0,e[3]-e[1],r.near,r.far),j(r.viewMatrix),N(r.viewMatrix,r.viewMatrix,[-e[0],-e[1],0]),this._renderContext.camera=r,this._bindParameters.camera=r,this._bindParameters.inverseViewport[0]=1/r.fullViewport[2],this._bindParameters.inverseViewport[1]=1/r.fullViewport[3]}updateHasWater(){const e=oe(this._layerRenderers,(e=>e.hasWater));e!==this._hasWater&&(this._hasWater=e,this.events.emit("has-water",e))}updateHasHighlights(){const e=oe(this._layerRenderers,(e=>e.hasHighlights));e!==this._hasHighlights&&(this._hasHighlights=e,this.events.emit("has-highlights",e))}updateRendersOccluded(){const e=oe(this._layerRenderers,(e=>e.rendersOccluded));e!==this._rendersOccluded&&(this._rendersOccluded=e,this.events.emit("renders-occluded",e))}drawDebugTexture(e,t){const r=this._rctx;this.ensureDebugPatternResources(e,e);const i=this._debugTextureTechnique.program;r.useProgram(i),this._debugTextureTechnique.bindPipelineState(r),i.setUniform4f("color",t[0],t[1],t[2],1),i.bindTexture(this._debugPatternTexture,"tex"),r.bindVAO(this._quadVAO),r.drawArrays(5,0,be(this._quadVAO,"geometry"))}ensureDebugPatternResources(e,t){if(this._debugPatternTexture)return;const r=new Uint8Array(e*t*4);let i=0;for(let s=0;s<t;s++)for(let a=0;a<e;a++){const o=Math.floor(a/10),n=Math.floor(s/10);o<2||n<2||10*o>e-20||10*n>t-20?(r[i++]=255,r[i++]=255,r[i++]=255,r[i++]=255):(r[i++]=255,r[i++]=255,r[i++]=255,r[i++]=1&o&&1&n?1&a^1&s?0:255:1&o^1&n?0:128)}this._debugPatternTexture=new xe(this._rctx,{target:3553,pixelFormat:6408,dataType:5121,samplingMode:9728,width:e,height:t},r);const s=new _s;s.hasAlpha=!0,this._debugTextureTechnique=this._shaderTechniqueRepository.acquire(ys,s),this._quadVAO=ct(this._rctx)}get test(){return{layerRenderers:this._layerRenderers}}};K([ce()],js.prototype,"_frameTask",void 0),K([ce()],js.prototype,"_sortedLayerRenderersDirty",void 0),K([(e,t)=>{var r,i;e.hasOwnProperty("_managedDisposables")||(e._managedDisposables=null!=(r=null==(i=e._managedDisposables)?void 0:i.slice())?r:[]),e._managedDisposables.unshift(t)}],js.prototype,"_shaderTechniqueRepository",void 0),K([(e,t)=>{var r,i;e.hasOwnProperty("_managedDisposables")||(e._managedDisposables=null!=(r=null==(i=e._managedDisposables)?void 0:i.slice())?r:[]),e._managedDisposables.unshift(t)}],js.prototype,"_stippleTextureRepository",void 0),K([ce({constructOnly:!0})],js.prototype,"view",void 0),K([ce()],js.prototype,"worldToPCSRatio",void 0),K([ce()],js.prototype,"spatialReference",void 0),K([ce({type:Boolean,readOnly:!0})],js.prototype,"updating",null),js=K([he("esri.views.3d.terrain.OverlayRenderer")],js);class Ns{constructor(e,t){this.layerView=e,this.renderer=t}}const Vs=[[1,.5,.5],[.5,.5,1]];const Hs=new we;Hs.near=1,Hs.far=1e4,Hs.relativeElevation=null;const Us=n(),Gs=n(),Bs=-2,qs=4,ks=1.2,$s=L,Zs=P;function Js(t){const s=[],a=[];!function(e,t,r){const{attributeData:{position:i},removeDuplicateStartEnd:s}=e,a=function(e){const t=e.length;return e[0]===e[t-3]&&e[1]===e[t-2]&&e[2]===e[t-1]}(i)&&1===s,n=i.length/3-(a?1:0),l=new Uint32Array(2*(n-1)),d=a?o(i,0,i.length-3):i;let c=0;for(let e=0;e<n-1;e++)l[c++]=e,l[c++]=e+1;t.push(["position",{size:3,data:d,exclusive:a}]),r.push(["position",l])}(t,a,s);const n=a[0][1].data,l=s[0][1].length,d=new Uint16Array(l);return function(e,t,i){const s=e.attributeData.mapPosition;if(r(s))return;i.push(["mapPos",i[0][1]]),t.push(["mapPos",{size:3,data:s}])}(t,a,s),function(t,r,s,a){if(i(t.attributeData.colorFeature))return;const o=t.attributeData.color;r.push(["color",{size:4,data:e(o,Zs)}]),s.push(["color",a])}(t,a,s,d),function(t,r,s,a){if(i(t.attributeData.sizeFeature))return;const o=t.attributeData.size;r.push(["size",{size:1,data:[e(o,1)]}]),s.push(["size",a])}(t,a,s,d),function(e,t,i,s){const a=e.attributeData.colorFeature;if(r(a))return;t.push(["colorFeatureAttribute",{size:1,data:new Float32Array([a])}]),i.push(["color",s])}(t,a,s,d),function(e,t,i,s){const a=e.attributeData.sizeFeature;if(r(a))return;t.push(["sizeFeatureAttribute",{size:1,data:new Float32Array([a])}]),i.push(["sizeFeatureAttribute",s])}(t,a,s,d),function(e,t,i,s){const a=e.attributeData.opacityFeature;if(r(a))return;t.push(["opacityFeatureAttribute",{size:1,data:new Float32Array([a])}]),i.push(["opacityFeatureAttribute",s])}(t,a,s,d),function(t,r,i,s){if("round"!==t.join)return;const a=s.length/3,o=new Float32Array(a),n=ea,l=ta;h(n,0,0,0);const d=e(t.uniformSize,1);for(let e=-1;e<a;++e){const t=e<0?a+e:e,r=(e+1)%a;if(h(l,s[3*r+0]-s[3*t+0],s[3*r+1]-s[3*t+1],s[3*r+2]-s[3*t+2]),S(l,l),e>=0){const t=1*((Math.PI-x(u(n,l)))*ra)*Ks(d);o[e]=Math.max(Math.floor(t),0)}f(n,l,-1)}r.push(["subdivisions",{size:1,data:o}]),i.push(["subdivisions",i[0][1]])}(t,a,s,n),function(e,t,i,s){if(r(e.overlayInfo)||1!==e.overlayInfo.renderCoordsHelper.viewingMode||!e.overlayInfo.spatialReference.isGeographic)return;const a=new Float64Array(s.length),o=D(e.overlayInfo.spatialReference);for(let e=0;e<a.length;e+=3)E(s,e,a,e,o);const n=s.length/3,l=new Float32Array(n+1);let d=ea,c=ta,p=0,u=0;h(d,a[u++],a[u++],a[u++]),l[0]=0;for(let e=1;e<n+1;++e)e===n&&(u=0),h(c,a[u++],a[u++],a[u++]),p+=C(d,c),l[e]=p,[d,c]=[c,d];t.push(["distanceToStart",{size:1,data:l}]),i.push(["distanceToStart",i[0][1]])}(t,a,s,n),new st(a,s,2)}function Qs(e,t,r,i){const s="polygon"===e.type?1:0,a="polygon"===e.type?e.rings:e.paths,{position:o,outlines:n}=O(a,e.hasZ,s),l=new Float64Array(o.length),d=ir(o,e.spatialReference,0,l,0,o,0,o.length/3,t,r,i),c=null!=d;return{lines:c?Ys(n,o,l):[],projectionSuccess:c,sampledElevation:d}}function Ys(e,t,r){const i=new Array;for(const{index:s,count:a}of e){if(a<=1)continue;const e=3*s,o=e+3*a;i.push({position:t.subarray(e,o),mapPosition:r?r.subarray(e,o):void 0})}return i}function Xs(e,t){const r="polygon"===e.type?1:0,i="polygon"===e.type?e.rings:e.paths,{position:s,outlines:a}=O(i,!1,r),o=T(s,e.spatialReference,0,s,t,0,s.length/3);for(let e=2;e<s.length;e+=3)s[e]=-2;return{lines:o?Ys(a,s):[],projectionSuccess:o}}function Ks(e){return 1.863798+-2.0062872/(1+e/18.2313)**.8856294}const ea=n(),ta=n(),ra=4/Math.PI;function ia(e){switch(e){case"butt":return 0;case"square":return 1;case"round":return 2;default:return null}}export{Yt as A,Js as B,wi as C,Bs as D,Xs as E,Xt as F,Qt as G,ar as H,Hi as I,Gi as J,Ui as K,dr as L,Kt as M,Ii as N,js as O,Fr as P,ks as Q,Si as R,Ms as S,ys as T,Mr as V,Fi as W,vi as a,Li as b,_r as c,rr as d,Dr as e,vr as f,Or as g,is as h,_s as i,Er as j,Fs as k,Tr as l,Ar as m,cr as n,qs as o,sr as p,hr as q,or as r,Ri as s,nr as t,ir as u,lr as v,$s as w,ia as x,Br as y,Qs as z};
