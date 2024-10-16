/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{i as e,b as t}from"../core/lang.js";import{c as r}from"./mat3f32.js";import{b as s}from"./brushes.js";import{C as i}from"./Container.js";import n from"../core/Error.js";import{L as o}from"./Logger.js";import{D as a}from"./DisplayObject.js";import{e as c}from"./earcut.js";import{s as h}from"./vec2.js";import{c as f}from"./vec2f64.js";import{d as p,e as m}from"./featureConversionUtils.js";import{O as d}from"./OptimizedFeature.js";import{i as l}from"./number2.js";import{B as u,V as g}from"./Program.js";import{W as y}from"./enums.js";const _=o.getLogger("esri.views.2d.engine.webgl.Mesh2D"),x=(e,t,r,s)=>{let i=0;for(let s=1;s<r;s++){const r=e[2*(t+s-1)],n=e[2*(t+s-1)+1];i+=(e[2*(t+s)]-r)*(e[2*(t+s)+1]+n)}return s?i>0:i<0},w=({coords:e,lengths:t},r)=>{const s=[];for(let i=0,n=0;i<t.length;n+=t[i],i+=1){const o=n,a=[];for(;i<t.length-1&&x(e,n+t[i],t[i+1],r);i+=1,n+=t[i])a.push(n+t[i]-o);const h=e.slice(2*o,2*(n+t[i])),f=c(h,a,2);for(const e of f)s.push(e+o)}return s};class v{constructor(e,t,r,s=!1){this._cache={},this.vertices=e,this.indices=t,this.primitiveType=r,this.isMapSpace=s}static fromRect({x:e,y:t,width:r,height:s}){const i=e,n=t,o=i+r,a=n+s;return v.fromScreenExtent({xmin:i,ymin:n,xmax:o,ymax:a})}static fromPath(e){const t=p(new d,e.path,!1,!1),r=t.coords,s=new Uint32Array(w(t,!0)),i=new Uint32Array(r.length/2);for(let e=0;e<i.length;e++)i[e]=l(Math.floor(r[2*e]),Math.floor(r[2*e+1]));return new v({geometry:i},s,4)}static fromGeometry(e,t){const r=t.geometry.type;switch(r){case"polygon":return v.fromPolygon(e,t.geometry);case"extent":return v.fromMapExtent(e,t.geometry);default:return _.error(new n("mapview-bad-type",`Unable to create a mesh from type ${r}`,t)),v.fromRect({x:0,y:0,width:1,height:1})}}static fromPolygon(e,t){const r=m(new d,t,!1,!1),s=r.coords,i=new Uint32Array(w(r,!1)),n=new Uint32Array(s.length/2),o=f(),a=f();for(let t=0;t<n.length;t++)h(o,s[2*t],s[2*t+1]),e.toScreen(a,o),n[t]=l(Math.floor(a[0]),Math.floor(a[1]));return new v({geometry:n},i,4,!0)}static fromScreenExtent({xmin:e,xmax:t,ymin:r,ymax:s}){const i={geometry:new Uint32Array([l(e,r),l(t,r),l(e,s),l(e,s),l(t,r),l(t,s)])},n=new Uint32Array([0,1,2,3,4,5]);return new v(i,n,4)}static fromMapExtent(e,t){const[r,s]=e.toScreen([0,0],[t.xmin,t.ymin]),[i,n]=e.toScreen([0,0],[t.xmax,t.ymax]),o={geometry:new Uint32Array([l(r,s),l(i,s),l(r,n),l(r,n),l(i,s),l(i,n)])},a=new Uint32Array([0,1,2,3,4,5]);return new v(o,a,4)}destroy(){e(this._cache.indexBuffer)&&this._cache.indexBuffer.dispose();for(const t in this._cache.vertexBuffers)e(this._cache.vertexBuffers[t])&&this._cache.vertexBuffers[t].dispose()}get elementType(){return(e=>{switch(e.BYTES_PER_ELEMENT){case 1:return 5121;case 2:return 5123;case 4:return 5125;default:throw new n("Cannot get DataType of array")}})(this.indices)}getIndexBuffer(e,t=35044){return this._cache.indexBuffer||(this._cache.indexBuffer=u.createIndex(e,t,this.indices)),this._cache.indexBuffer}getVertexBuffers(e,t=35044){return this._cache.vertexBuffers||(this._cache.vertexBuffers=Object.keys(this.vertices).reduce(((r,s)=>({...r,[s]:u.createVertex(e,t,this.vertices[s])})),{})),this._cache.vertexBuffers}}const B=o.getLogger("esri.views.2d.engine.webgl.ClippingInfo"),L=e=>parseFloat(e)/100;class b extends a{constructor(e,t){super(),this._clip=t,this._cache={},this.stage=e,this._handle=t.watch("version",(()=>this._invalidate())),this.ready()}static fromClipArea(e,t){return new b(e,t)}_destroyGL(){e(this._cache.mesh)&&(this._cache.mesh.destroy(),this._cache.mesh=null),e(this._cache.vao)&&(this._cache.vao.dispose(),this._cache.vao=null)}destroy(){this._destroyGL(),this._handle.remove()}getVAO(e,r,s,i){const[n,o]=r.size;if("geometry"!==this._clip.type&&this._lastWidth===n&&this._lastHeight===o||(this._lastWidth=n,this._lastHeight=o,this._destroyGL()),t(this._cache.vao)){const t=this._createMesh(r,this._clip),n=t.getIndexBuffer(e),o=t.getVertexBuffers(e);this._cache.mesh=t,this._cache.vao=new g(e,s,i,o,n)}return this._cache.vao}_createTransforms(){return{dvs:r()}}_invalidate(){this._destroyGL(),this.requestRender()}_createScreenRect(e,t){const[r,s]=e.size,i="string"==typeof t.left?L(t.left)*r:t.left,n="string"==typeof t.right?L(t.right)*r:t.right,o="string"==typeof t.top?L(t.top)*s:t.top,a="string"==typeof t.bottom?L(t.bottom)*s:t.bottom,c=i,h=o;return{x:c,y:h,width:Math.max(r-n-c,0),height:Math.max(s-a-h,0)}}_createMesh(e,t){switch(t.type){case"rect":return v.fromRect(this._createScreenRect(e,t));case"path":return v.fromPath(t);case"geometry":return v.fromGeometry(e,t);default:return B.error(new n("mapview-bad-type","Unable to create ClippingInfo mesh from clip of type: ${clip.type}")),v.fromRect({x:0,y:0,width:1,height:1})}}}class A extends i{constructor(){super(...arguments),this.name=this.constructor.name}set clips(e){this._clips=e,this.children.forEach((t=>t.clips=e)),this._updateClippingInfo()}_createTransforms(){return{dvs:r()}}doRender(e){const t=this.createRenderParams(e),{painter:r,globalOpacity:s,profiler:i,drawPhase:n}=t,o=n===y.LABEL||n===y.HIGHLIGHT?1:s*this.computedOpacity;i.recordContainerStart(this.name),r.beforeRenderLayer(t,this._clippingInfos?255:0,o),this.updateTransforms(e.state),this.renderChildren(t),r.compositeLayer(t,o),i.recordContainerEnd()}renderChildren(e){t(this._renderPasses)&&(this._renderPasses=this.prepareRenderPasses(e.painter));for(const t of this.children)t.beforeRender(e);for(const t of this._renderPasses)try{t.render(e)}catch(e){}for(const t of this.children)t.afterRender(e)}createRenderParams(e){return e.requireFBO=this.requiresDedicatedFBO,e}prepareRenderPasses(e){return[e.registerRenderPass({name:"clip",brushes:[s.clip],target:()=>this._clippingInfos,drawPhase:y.MAP|y.LABEL|y.LABEL_ALPHA|y.DEBUG|y.HIGHLIGHT})]}updateTransforms(e){for(const t of this.children)t.setTransform(e)}onAttach(){super.onAttach(),this._updateClippingInfo()}onDetach(){super.onDetach(),this._updateClippingInfo()}_updateClippingInfo(){if(e(this._clippingInfos)&&(this._clippingInfos.forEach((e=>e.destroy())),this._clippingInfos=null),!this.stage)return;const t=this._clips;e(t)&&t.length&&(this._clippingInfos=t.items.map((e=>b.fromClipArea(this.stage,e)))),this.requestRender()}}export{A as W};
