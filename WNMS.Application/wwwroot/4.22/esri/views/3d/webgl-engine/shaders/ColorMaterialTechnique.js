// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../core/shaderLibrary/Slice.glsl ../core/shaderLibrary/output/OutputHighlight.glsl ../core/shaderLibrary/shading/MultipassTerrainTest.glsl ../core/shaderLibrary/util/View.glsl ../core/shaderTechnique/ReloadableShaderModule ../core/shaderTechnique/ShaderTechnique ../core/shaderTechnique/ShaderTechniqueConfiguration ../lib/DefaultVertexAttributeLocations ../lib/OrderIndependentTransparency ../lib/Program ../lib/StencilUtils ../../../../chunks/ColorMaterial.glsl ../../../webgl/renderState".split(" "),
function(w,p,t,g,x,y,z,u,d,l,e,A,m,B,q,C,n){l=function(k){function h(){return k.apply(this,arguments)||this}t._inheritsLoose(h,k);var a=h.prototype;a.initializeProgram=function(f){var b=h.shader.get();const c=this.configuration;b=b.build({output:c.output,OITEnabled:0===c.transparencyPassType,attributeColor:c.vertexColors,slicePlaneEnabled:c.slicePlaneEnabled,sliceHighlightDisabled:!1,sliceEnabledForVertexPrograms:!1,multipassTerrainEnabled:c.multipassTerrainEnabled,cullAboveGround:c.cullAboveGround});
return new B.Program(f.rctx,b,A.Default3D)};a.bindPass=function(f,b){u.bindProjectionMatrix(this.program,b.camera.projectionMatrix);this.program.setUniform4fv("eColor",f.color);4===this.configuration.output&&y.bindOutputHighlight(this.program,b);(1===this.configuration.output||b.multipassTerrainEnabled)&&this.program.setUniform2fv("cameraNearFar",b.camera.nearFar);b.multipassTerrainEnabled&&(this.program.setUniform2fv("inverseViewport",b.inverseViewport),z.bindMultipassTerrainTexture(this.program,
b))};a.bindDraw=function(f){u.bindView(this.program,f);this.program.rebindTextures();x.bindSliceUniformsWithOrigin(this.program,this.configuration,f)};a.createPipeline=function(f,b){const c=this.configuration,r=3===f,v=2===f;return n.makePipelineState({blending:0!==c.output&&7!==c.output||!c.transparent?null:r?m.blendingDefault:m.OITBlending(f),culling:n.cullingParams(c.cullFace),depthTest:{func:m.OITDepthTest(f)},depthWrite:r||v?c.writeDepth&&n.defaultDepthWriteParams:null,colorWrite:n.defaultColorWriteParams,
stencilWrite:c.sceneHasOcludees?q.stencilWriteMaskOn:null,stencilTest:c.sceneHasOcludees?b?q.stencilToolMaskBaseParams:q.stencilBaseAllZerosParams:null,polygonOffset:r||v?c.polygonOffset&&D:m.getOITPolygonOffset(c.enableOffset)})};a.initializePipeline=function(){this._occludeePipelineState=this.createPipeline(this.configuration.transparencyPassType,!0);return this.createPipeline(this.configuration.transparencyPassType,!1)};a.getPipelineState=function(f,b){return b?this._occludeePipelineState:k.prototype.getPipelineState.call(this,
f,b)};return h}(l.ShaderTechnique);l.shader=new d.ReloadableShaderModule(C.ColorMaterialShader,()=>new Promise((k,h)=>w(["./ColorMaterial.glsl"],k,h)));const D={factor:1,units:1};d=function(k){function h(){var a=k.apply(this,arguments)||this;a.output=0;a.cullFace=0;a.slicePlaneEnabled=!1;a.vertexColors=!1;a.transparent=!1;a.polygonOffset=!1;a.enableOffset=!0;a.writeDepth=!0;a.sceneHasOcludees=!1;a.transparencyPassType=3;a.multipassTerrainEnabled=!1;a.cullAboveGround=!1;return a}t._inheritsLoose(h,
k);return h}(e.ShaderTechniqueConfiguration);g.__decorate([e.parameter({count:8})],d.prototype,"output",void 0);g.__decorate([e.parameter({count:3})],d.prototype,"cullFace",void 0);g.__decorate([e.parameter()],d.prototype,"slicePlaneEnabled",void 0);g.__decorate([e.parameter()],d.prototype,"vertexColors",void 0);g.__decorate([e.parameter()],d.prototype,"transparent",void 0);g.__decorate([e.parameter()],d.prototype,"polygonOffset",void 0);g.__decorate([e.parameter()],d.prototype,"enableOffset",void 0);
g.__decorate([e.parameter()],d.prototype,"writeDepth",void 0);g.__decorate([e.parameter()],d.prototype,"sceneHasOcludees",void 0);g.__decorate([e.parameter({count:4})],d.prototype,"transparencyPassType",void 0);g.__decorate([e.parameter()],d.prototype,"multipassTerrainEnabled",void 0);g.__decorate([e.parameter()],d.prototype,"cullAboveGround",void 0);p.ColorMaterialTechnique=l;p.ColorMaterialTechniqueConfiguration=d;Object.defineProperty(p,"__esModule",{value:!0})});