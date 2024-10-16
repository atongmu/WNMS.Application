// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../chunks/vec4f64 ../core/shaderLibrary/ScreenSizeScaling.glsl ../core/shaderLibrary/Slice.glsl ../core/shaderLibrary/shading/MultipassTerrainTest.glsl ../core/shaderLibrary/util/View.glsl ../core/shaderTechnique/ReloadableShaderModule ../core/shaderTechnique/ShaderTechnique ../core/shaderTechnique/ShaderTechniqueConfiguration ../lib/OrderIndependentTransparency ../lib/Program ../../../../chunks/ShadedColorMaterial.glsl ../../../webgl/renderState".split(" "),
function(w,n,u,g,x,y,z,A,t,e,p,f,q,B,C,r){p=function(m){function h(){return m.apply(this,arguments)||this}u._inheritsLoose(h,m);var d=h.prototype;d.initializeProgram=function(b){var a=h.shader.get();const c=this.configuration;a=a.build({output:c.output,slicePlaneEnabled:c.slicePlaneEnabled,sliceHighlightDisabled:!1,sliceEnabledForVertexPrograms:!1,screenSizeEnabled:c.screenSizeEnabled,shadingEnabled:c.shadingEnabled,OITEnabled:0===c.transparencyPassType,multipassTerrainEnabled:c.multipassTerrainEnabled,
cullAboveGround:c.cullAboveGround});return new B.Program(b.rctx,a,v)};d.bindPass=function(b,a){const {screenSizeEnabled:c,shadingEnabled:k}=this.configuration,{color:l,shadingTint:D,shadingDirection:E}=b;t.bindProjectionMatrix(this.program,a.camera.projectionMatrix);this.program.setUniform4fv("color",l);a.multipassTerrainEnabled&&(this.program.setUniform2fv("cameraNearFar",a.camera.nearFar),this.program.setUniform2fv("inverseViewport",a.inverseViewport),A.bindMultipassTerrainTexture(this.program,
a));k&&(this.program.setUniform4fv("shadedColor",this.blendColor(F,D,l)),this.program.setUniform3fv("shadingDirection",E),this.program.setUniformMatrix4fv("viewNormal",a.camera.viewInverseTransposeMatrix));c&&y.bindScreenSizeScalingUniforms(this.program,b,a)};d.bindDraw=function(b){t.bindView(this.program,b);t.bindCameraPosition(this.program,b.origin,b.camera.viewInverseTransposeMatrix);z.bindSliceUniformsWithOrigin(this.program,this.configuration,b);this.program.rebindTextures()};d.blendColor=function(b,
a,c){const k=1-a[3],l=a[3]+c[3]*k;if(0===l)return b[3]=l,b;b[0]=(a[0]*a[3]+c[0]*c[3]*k)/l;b[1]=(a[1]*a[3]+c[1]*c[3]*k)/l;b[2]=(a[2]*a[3]+c[2]*c[3]*k)/l;b[3]=c[3];return b};d.setPipelineState=function(b){const a=this.configuration,c=3===b,k=2===b;return r.makePipelineState({blending:0!==a.output&&7!==a.output||!a.transparent?null:c?q.blendingDefault:q.OITBlending(b),culling:r.cullingParams(a.cullFace),depthTest:{func:k?513:a.shadingEnabled?515:513},depthWrite:c?a.writeDepth&&r.defaultDepthWriteParams:
q.OITDepthWrite(b),colorWrite:r.defaultColorWriteParams,polygonOffset:c||k?null:q.OITPolygonOffset})};d.initializePipeline=function(){return this.setPipelineState(this.configuration.transparencyPassType)};return h}(p.ShaderTechnique);p.shader=new e.ReloadableShaderModule(C.ShadedColorMaterialShader,()=>new Promise((m,h)=>w(["./ShadedColorMaterial.glsl"],m,h)));e=function(m){function h(){var d=m.apply(this,arguments)||this;d.output=0;d.cullFace=0;d.slicePlaneEnabled=!1;d.transparent=!1;d.writeDepth=
!0;d.screenSizeEnabled=!0;d.shadingEnabled=!0;d.transparencyPassType=3;d.multipassTerrainEnabled=!1;d.cullAboveGround=!1;return d}u._inheritsLoose(h,m);return h}(f.ShaderTechniqueConfiguration);g.__decorate([f.parameter({count:8})],e.prototype,"output",void 0);g.__decorate([f.parameter({count:3})],e.prototype,"cullFace",void 0);g.__decorate([f.parameter()],e.prototype,"slicePlaneEnabled",void 0);g.__decorate([f.parameter()],e.prototype,"transparent",void 0);g.__decorate([f.parameter()],e.prototype,
"writeDepth",void 0);g.__decorate([f.parameter()],e.prototype,"screenSizeEnabled",void 0);g.__decorate([f.parameter()],e.prototype,"shadingEnabled",void 0);g.__decorate([f.parameter({count:4})],e.prototype,"transparencyPassType",void 0);g.__decorate([f.parameter()],e.prototype,"multipassTerrainEnabled",void 0);g.__decorate([f.parameter()],e.prototype,"cullAboveGround",void 0);const v=new Map([["position",0],["normal",1],["offset",2]]),F=x.create();n.ShadedColorMaterialTechnique=p;n.ShadedColorMaterialTechniqueConfiguration=
e;n.ShadedColorMaterialVertexAttrLocations=v;Object.defineProperty(n,"__esModule",{value:!0})});