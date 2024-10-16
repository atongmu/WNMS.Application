// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("require exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/RealisticTree.glsl ../core/shaderLibrary/util/DoublePrecision.glsl ../core/shaderTechnique/ReloadableShaderModule ../lib/DefaultVertexAttributeLocations ../lib/Program ./DefaultMaterialTechnique".split(" "),function(h,g,k,l,m,n,p,q,c){c=function(d){function b(){return d.apply(this,arguments)||this}k._inheritsLoose(b,d);b.prototype.initializeProgram=function(e){var f=b.shader.get();const a=this.configuration;f=
f.build({OITEnabled:0===a.transparencyPassType,output:a.output,viewingMode:e.viewingMode,receiveShadows:a.receiveShadows,slicePlaneEnabled:a.slicePlaneEnabled,sliceHighlightDisabled:a.sliceHighlightDisabled,sliceEnabledForVertexPrograms:!1,symbolColor:a.symbolColors,vvSize:a.vvSize,vvColor:a.vvColor,vvInstancingEnabled:!0,instanced:a.instanced,instancedColor:a.instancedColor,instancedDoublePrecision:a.instancedDoublePrecision,pbrMode:a.usePBR?1:0,hasMetalnessAndRoughnessTexture:!1,hasEmissionTexture:!1,
hasOcclusionTexture:!1,hasNormalTexture:!1,hasColorTexture:a.hasColorTexture,receiveAmbientOcclusion:a.receiveAmbientOcclusion,useCustomDTRExponentForWater:!1,normalType:0,doubleSidedMode:2,vertexTangents:!1,attributeTextureCoordinates:a.hasColorTexture?1:0,textureAlphaPremultiplied:a.textureAlphaPremultiplied,attributeColor:a.vertexColors,screenSizePerspectiveEnabled:a.screenSizePerspective,verticalOffsetEnabled:a.verticalOffset,offsetBackfaces:a.offsetBackfaces,doublePrecisionRequiresObfuscation:m.doublePrecisionRequiresObfuscation(e.rctx),
alphaDiscardMode:a.alphaDiscardMode,supportsTextureAtlas:!1,multipassTerrainEnabled:a.multipassTerrainEnabled,cullAboveGround:a.cullAboveGround});return new q.Program(e.rctx,f,p.Default3D)};return b}(c.DefaultMaterialTechnique);c.shader=new n.ReloadableShaderModule(l.RealisticTreeShader,()=>new Promise((d,b)=>h(["../core/shaderLibrary/default/RealisticTree.glsl"],d,b)));g.RealisticTreeTechnique=c;Object.defineProperty(g,"__esModule",{value:!0})});