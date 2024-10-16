// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/Accessor ../../../../core/maybe ../../../../core/promiseUtils ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../../support/requestImageUtils ./glUtil3D ./SMAATechnique ../../../webgl/FramebufferObject ../../../webgl/Texture".split(" "),
function(u,h,n,l,v,f,m,p,A,B,C,w,x,y,q,r,z){h.SmaaRenderPass=function(t){function k(d,a){var c=t.call(this,{})||this;c.rctx=d;c._techniqueRep=a;c._isEnabled=!1;return c}n._inheritsLoose(k,t);var g=k.prototype;g.normalizeCtorArgs=function(){return{}};g.dispose=function(){this._abortController=f.abortMaybe(this._abortController);this.disable()};g._loadResources=function(d){if(f.isSome(this._abortController))return!1;if(f.isSome(this._searchTexture))return!0;this._abortController=new AbortController;
const a=this._abortController.signal;(new Promise((c,b)=>u(["./SmaaRenderPassData"],c,b))).then(c=>this._loadTextures(c,a)).then(()=>d()).finally(()=>this._abortController=null);return!1};g._loadTextures=function(d,a){m.throwIfAborted(a);return Promise.all([this.loadTextureFromBase64(d.areaTexture,9729,6407),this.loadTextureFromBase64(d.searchTexure,9728,6409)]).then(([c,b])=>{m.isAborted(a)?(c.dispose(),b.dispose(),m.throwIfAborted(a)):(this._areaTexture=c,this._searchTexture=b)})};g.enable=function(d){if(this._isEnabled)return!0;
if(!this._edgeDetectTechnique||!this._blendWeights||!this._blur){const a=new q.SMAATechniqueConfiguration,c=(b,e)=>this._techniqueRep.releaseAndAcquire(q.SMAATechnique,b,e);a.output=0;this._edgeDetectTechnique=c(a,this._edgeDetectTechnique);a.output=1;this._blendWeights=c(a,this._blendWeights);a.output=2;this._blur=c(a,this._blur)}if(!this._loadResources(d))return!1;this._vao=y.createScreenSizeTriangleVAO(this.rctx);this._edges=new r(this.rctx,{colorTarget:0,depthStencilTarget:0},{target:3553,pixelFormat:6407,
dataType:5121,samplingMode:9729,wrapMode:33071,width:4,height:4});this._blend=new r(this.rctx,{colorTarget:0,depthStencilTarget:0},{target:3553,pixelFormat:6408,dataType:5121,samplingMode:9729,wrapMode:33071,width:4,height:4});return this._isEnabled=!0};g.disable=function(){this._isEnabled&&(this._vao=f.disposeMaybe(this._vao),this._areaTexture=f.disposeMaybe(this._areaTexture),this._searchTexture=f.disposeMaybe(this._searchTexture),this._blend=f.disposeMaybe(this._blend),this._edges=f.disposeMaybe(this._edges),
this._isEnabled=!1)};g.render=function(d){if(this._isEnabled){var a=this.rctx,c=a.getBoundFramebufferObject(),b=d.descriptor.width,e=d.descriptor.height;a.bindVAO(this._vao);this._edgeDetectTechnique.bindPipelineState(a);a.setViewport(0,0,b,e);this._edges.resize(b,e);a.bindFramebuffer(this._edges);a.setClearColor(0,0,0,1);a.clear(16384);a.useProgram(this._edgeDetectTechnique.program);this._edgeDetectTechnique.program.bindTexture(d,"tColor");this._edgeDetectTechnique.program.setUniform4f("uResolution",
1/b,1/e,b,e);a.drawArrays(4,0,3);this._blend.resize(b,e);a.bindFramebuffer(this._blend);a.setClearColor(0,0,1,1);a.clear(16384);a.useProgram(this._blendWeights.program);this._blendWeights.program.setUniform4f("uResolution",1/b,1/e,b,e);this._blendWeights.program.bindTexture(this._searchTexture,"tSearch");this._blendWeights.program.bindTexture(this._areaTexture,"tArea");this._blendWeights.program.bindTexture(this._edges.colorTexture,"tEdges");a.drawArrays(4,0,3);a.bindFramebuffer(c);a.setClearColor(0,
1,0,1);a.clear(16384);a.useProgram(this._blur.program);this._blur.program.setUniform4f("uResolution",1/b,1/e,b,e);this._blur.program.bindTexture(d,"tColor");this._blur.program.bindTexture(this._blend.colorTexture,"tBlendWeights");a.drawArrays(4,0,3)}};g.loadTextureFromBase64=function(d,a,c){const b=new z(this.rctx,{pixelFormat:c,dataType:5121,wrapMode:33071,samplingMode:a},null);return x.requestImage(d).then(e=>{b.resize(e.width,e.height);b.setData(e);return b})};n._createClass(k,[{key:"updating",
get:function(){return f.isSome(this._abortController)}}]);return k}(v);l.__decorate([p.property()],h.SmaaRenderPass.prototype,"_abortController",void 0);l.__decorate([p.property({readOnly:!0})],h.SmaaRenderPass.prototype,"updating",null);h.SmaaRenderPass=l.__decorate([w.subclass("esri.views.3d.webgl-engine.lib.SmaaRenderPass")],h.SmaaRenderPass);Object.defineProperty(h,"__esModule",{value:!0})});