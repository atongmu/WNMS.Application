// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require exports ../../../chunks/_rollupPluginBabelHelpers ../../../chunks/NoiseTextureAtlas.glsl ../webgl-engine/core/shaderTechnique/ReloadableShaderModule ../webgl-engine/core/shaderTechnique/ShaderTechnique ../webgl-engine/lib/DefaultVertexAttributeLocations ../webgl-engine/lib/Program ../../webgl/renderState".split(" "),function(g,e,h,k,l,b,m,n,d){b=function(c){function a(){return c.apply(this,arguments)||this}h._inheritsLoose(a,c);var f=a.prototype;f.initializeProgram=function(p){const q=
a.shader.get().build();return new n.Program(p.rctx,q,m.Default3D)};f.initializePipeline=function(){return d.makePipelineState({blending:d.simpleBlendingParams(1,0),depthTest:{func:515},colorWrite:d.defaultColorWriteParams})};return a}(b.ShaderTechnique);b.shader=new l.ReloadableShaderModule(k.NoiseTextureAtlasShader,()=>new Promise((c,a)=>g(["./NoiseTextureAtlas.glsl"],c,a)));e.NoiseTextureAtlasTechnique=b;Object.defineProperty(e,"__esModule",{value:!0})});