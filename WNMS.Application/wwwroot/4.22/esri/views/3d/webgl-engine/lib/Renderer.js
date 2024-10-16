// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/Accessor ../../../../core/Handles ../../../../core/MapUtils ../../../../core/maybe ../../../../core/watchUtils ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../../chunks/mat4 ../../../../chunks/mat4f64 ../../../../chunks/vec2f64 ../../support/debugFlags ../core/renderPasses/RenderPassManager ./BoundingInfo ./depthRange ./depthRangeUtils ./glUtil3D ./HighlightHelper ./OffscreenRendering ./RenderContext ./rendererUtils ./RenderPluginManager ./ShadowAccumulator ./ShadowHighlightHelper ./ShadowMap ./SliceHelper ./SmaaRenderPass ./SSAOHelper ./edgeRendering/EdgeView ../lighting/SceneLighting ../materials/renderers/MergedRenderer ../shaders/ShadowHighlightTechnique ../statistics/RendererPerformanceInfo ../../../webgl/Measurement".split(" "),
function(n,E,t,L,M,B,l,F,u,ma,na,oa,N,p,r,O,P,Q,R,G,S,T,U,V,W,X,Y,Z,aa,ba,ca,da,ea,fa,ha,ia,H,ja,ka){n.Renderer=function(I){function z(a,c,d,g,e,h,k,m,q){var b=I.call(this,{})||this;b._materialRepository=a;b._shaderTechniqueRepository=d;b._rctx=g;b._compositingHelper=e;b._magnifierHelper=h;b._requestRender=k;b._stage=q;b._materialRenderers=new Map;b._hasHighlights=!1;b._hasOccludees=!1;b._hasWater=!1;b._hasOverlayWater=!1;b._content=new Map;b._isRendering=!1;b._fallbackDepthStencilTexture=null;b.performanceInfo=
new ja.RendererPerformanceInfo;b._antialiasing=1;b._oitEnabled=!1;b._multipassTerrain=!0;b._opaqueTerrain=!0;b._lighting=new ha.SceneLighting;b._handles=new M;b.renderHiddenTransparentEdges=()=>{};b._smaaPass=new da.SmaaRenderPass(b._rctx,d);b._oitEnabled=b._hasOITSupport;b._requestRender();b._offscreenRendering=new V.OffscreenRendering(b._rctx,b._compositingHelper);b._sliceHelper=new ca;b._shadowMap=new ba(b._rctx,q.viewingMode,2);b._ssaoHelper=new ea.SSAOHelper(d,b._rctx,()=>b._requestRender());
b._highlightHelper=new U(d,b._rctx);b._shadowHighlightHelper=new aa.ShadowHighlightHelper(b._rctx,q.viewingMode);b._shadowAccumulator=new Z.ShadowAccumulator(b._rctx,d,q,(v,C)=>{b.renderPassManager.shadowCastingEnabled=!0;b._renderPlugins.prepareRender(v,C);b.renderPassManager.shadowCastingEnabled=b._shadowMap.enabled},(v,C,D,la)=>{v.start(D,C,la);b.renderShadowCascades(4,v);D.setGLViewport(b._rctx);b.ensureCameraBindParameters(D)},()=>b._requestRender());b._bindParameters={slot:null,camera:null,
inverseViewport:O.create(),shadowMap:b._shadowMap,shadowMappingEnabled:b._shadowMap.enabled,ssaoHelper:b._ssaoHelper,ssaoEnabled:b._ssaoHelper.enabled,origin:null,screenToWorldRatio:null,screenToPCSRatio:null,slicePlane:b._sliceHelper.plane,highlightDepthTexture:null,hasOccludees:!1,linearDepthTexture:null,terrainLinearDepthTexture:null,geometryLinearDepthTexture:null,multipassTerrainEnabled:!1,multipassGeometryEnabled:!1,cullAboveGround:!1,lastFrameColorTexture:null,reprojectionMatrix:r.IDENTITY,
ssrEnabled:!1,lighting:b._lighting,highlightColorTexture:null};b._renderContext=new W.RenderContext(b._rctx,b._offscreenRendering,b._lighting,b._shadowMap,b._ssaoHelper,b._sliceHelper);b._renderContext.multipassTerrainParams={camera:null,multipassTerrainEnabled:!1,cullAboveGround:!1,terrainLinearDepthTexture:null};b._renderContext.multipassGeometryParams={multipassGeometryEnabled:!1,geometryLinearDepthTexture:null};b._renderContext.ssrParams={camera:null,linearDepthTexture:null,lastFrameColorTexture:null,
reprojectionMatrix:r.IDENTITY,ssrEnabled:!1};b._renderPlugins=new Y.RenderPluginManager({renderContext:b._renderContext,shaderTechniqueRep:d,textureRep:c,materialRep:b._materialRepository,requestRender:b._requestRender,schedule:m});b.renderPassManager=new Q.RenderPassManager(b._rctx,b._shaderTechniqueRepository);b._renderPlugins.add(b.renderPassManager.slots(),b.renderPassManager);b._handles.add([F.init(P,"EDGES_SHOW_HIDDEN_TRANSPARENT_EDGES",v=>{b.renderHiddenTransparentEdges=v?()=>b.renderEdges(1):
()=>{};b._requestRender()}),F.init(b._stage,"camera",()=>b._requestRender(),!0)]);return b}E._inheritsLoose(z,I);var f=z.prototype;f.normalizeCtorArgs=function(){return{}};f.dispose=function(){this._handles.destroy();this._smaaPass.dispose();this._materialRenderers.forEach(a=>a.dispose());this._materialRenderers.clear();this._edgeView=l.destroyMaybe(this._edgeView);this._offscreenRendering.dispose();this._fallbackDepthStencilTexture=l.disposeMaybe(this._fallbackDepthStencilTexture);this._shadowMap.enabled=
!1;this._shadowMap.dispose();this._ssaoHelper.enabled=!1;this._ssaoHelper.dispose();this._highlightHelper.dispose();this._shadowHighlightHelper.dispose();this._shadowAccumulator.dispose();R.BoundingInfo.prune();this._content.clear()};f.ensureEdgeView=function(){l.isNone(this._edgeView)&&(this._edgeView=new fa.EdgeView({rctx:this._rctx,renderSR:this._stage.renderSR,techniqueRepository:this._shaderTechniqueRepository,setNeedsRender:()=>this._requestRender(),schedule:a=>this._stage.resourceController.schedule(a)}),
this._handles.add(this._edgeView.watch("updating",()=>this._requestRender(),!0)),this._requestRender());return this._edgeView};f.setRenderParameters=function(a){const {renderPassManager:c,_shadowMap:d,_ssaoHelper:g}=this;void 0!==a.screenSpaceReflectionsEnabled&&c.screenSpaceReflectionsEnabled!==a.screenSpaceReflectionsEnabled&&(c.screenSpaceReflectionsEnabled=a.screenSpaceReflectionsEnabled,this._requestRender());void 0===a.shadowMap||d.enabled===a.shadowMap&&c.shadowCastingEnabled===a.shadowMap||
(d.enabled=a.shadowMap,c.shadowCastingEnabled=a.shadowMap,this._requestRender());void 0!==a.shadowMapMaxCascades&&d.maxCascades!==a.shadowMapMaxCascades&&(d.maxCascades=a.shadowMapMaxCascades,this._requestRender());void 0!==a.ssao&&g.enabled!==a.ssao&&(g.enabled=a.ssao,this._requestRender());a.background&&this._offscreenRendering.background!==a.background&&(this._offscreenRendering.background=a.background,this._requestRender());const e=a.antialiasingEnabled?1:0;void 0!==a.antialiasingEnabled&&this._antialiasing!==
e&&(this._antialiasing=e,this._requestRender());void 0!==a.highQualityTransparency&&this._multipassTerrain!==a.highQualityTransparency&&(this._oitEnabled=(this._multipassTerrain=a.highQualityTransparency)&&this._hasOITSupport,this._requestRender());void 0!==a.defaultHighlightOptions&&(this._highlightHelper.setDefaultOptions(a.defaultHighlightOptions),this._shadowHighlightHelper.setDefaultOptions(a.defaultHighlightOptions),this._requestRender());void 0!==a.slicePlane&&this._sliceHelper.plane!==a.slicePlane&&
(this._sliceHelper.plane=l.unwrap(a.slicePlane),this._requestRender());void 0!==a.waterReflectionEnabled&&this._waterReflectionEnabled!==a.waterReflectionEnabled&&(this._waterReflectionEnabled=a.waterReflectionEnabled,this._requestRender());void 0!==a.opaqueTerrain&&this._opaqueTerrain!==a.opaqueTerrain&&(this._opaqueTerrain=a.opaqueTerrain,this._requestRender());void 0!==a.hasOverlayWater&&this._hasOverlayWater!==a.hasOverlayWater&&(this._hasOverlayWater=a.hasOverlayWater,this._requestRender());
void 0!==a.shadowCastOptions&&this._shadowAccumulator.setOptions(a.shadowCastOptions)};f.modify=function(a){this._isRendering&&console.warn("Renderer.modify called while rendering");const {adds:c,removes:d,updates:g}=a;if(0!==c.length||0!==d.length||0!==g.length){d.forAll(({id:h})=>this._content.delete(h));c.forAll(h=>this._content.set(h.id,h));var e=!1;X.splitRenderGeometryChangeSetByMaterial(a).forEach((h,k)=>{let m=this._materialRenderers.get(k);if(!m)if(0<h.adds.length)m=new ia.MergedRenderer(this._rctx,
this._materialRepository,k),this._materialRenderers.set(k,m);else return;m.modify(h);m.isEmpty&&(e=!0)});e&&this._materialRenderers.forEach((h,k)=>{h.isEmpty&&(this._materialRenderers.delete(k),h.dispose())});this._hasHighlights=B.someMap(this._materialRenderers,h=>h.hasHighlights);this._hasOccludees=B.someMap(this._materialRenderers,h=>h.hasOccludees);this._hasWater=B.someMap(this._materialRenderers,h=>h.hasWater);this._requestRender()}};f.updateLogic=function(a){let c=!1;this._materialRenderers.forEach(d=>
c=d.updateLogic(a)||c);return c};f.updateLightSources=function(a,c,d){this._lighting.groundLightingFactor=c;this._lighting.globalFactor=d;this._lighting.set(a)};f.render=function(a,c,d,g){this._isRendering=!0;this.performanceInfo.prerender(this._rctx);this._bindParameters.lighting=this._lighting;this._renderContext.hasOccludees=this._hasOccludees;this._renderContext.transparencyPassType=3;this._bindParameters.transparencyPassType=3;var e=this._offscreenRendering;e.needLastFrameColorTexture=this.hasWaterReflection;
e.advanceCurrentRenderTarget();const h=this._sliceHelper.plane;0===g&&(this._sliceHelper.plane=null);this._rctx.bindFramebuffer(a);c.setGLViewport(this._rctx);this.needsShadowCast&&(this.renderPassManager.shadowCastingEnabled=!0);this._renderPlugins.prepareRender(c,d);this.performanceInfo.advance(0);var k=this.computeDepthRange(c);this.renderShadowMap(a,c,this._lighting.lightingMainDirection,k);this.performanceInfo.advance(1);e.initializeFrame(c);this.ensureBindParameters(c);this.renderLinearDepth();
this.performanceInfo.advance(2);this.accumulateShadows(k,c,d);this.renderNormal();this.performanceInfo.advance(3);this.ensureBindParametersSSR();this._ssaoHelper.computeSSAO(c,e.linearDepthTexture,e.normalTexture);this.performanceInfo.advance(4);this._renderContext.pass=0;e.bindFramebuffer();this.renderOpaqueGeometry();this.performanceInfo.advance(5);c=this._multipassTerrain&&!this._opaqueTerrain;this.renderTerrainLinearDepth(c);this.setMultipassFlags(c);this.setTerrainCulling(c);this.renderEdges(2);
this.performanceInfo.advance(6);this.renderHiddenTransparentEdges();this._oitEnabled?this.renderOrderIndependentTransparency(()=>this.renderTransparentGeometry(),!1):this.renderTransparentGeometry();this.performanceInfo.advance(7);this.renderGeometryLinearDepth(c);k=d=!1;d=this.renderHUDVisibility();c||this.renderInternalSlot(18);this.performanceInfo.advance(9);this.renderEdges(1,c);this.performanceInfo.advance(8);this.renderSlot(22);(k=this.renderTransparentTerrain())&&d&&(c?this.renderLineCallouts(0):
e.compositeTransparentTerrainOntoHUDVisibility(),this.renderHUD(0,e.framebuffer),this.performanceInfo.advance(16));this.performanceInfo.advance(10);this.setTerrainCulling(!1);k&&(e.compositeTransparentTerrainOntoMain(),c&&(this.renderEdges(2),this.performanceInfo.advance(6),this._oitEnabled?this.renderOrderIndependentTransparency(()=>this.renderTransparentGeometry(),!1):this.renderTransparentGeometry(),this.performanceInfo.advance(7),this.renderEdges(1),this.performanceInfo.advance(8)));c&&this.renderLineCallouts(1);
this.setMultipassFlags(!1);this._shadowAccumulator.render();e.renderToTargets(()=>{this.renderInternalSlot(7);this.renderSlot(13);this.renderSlot(14)},e.currentColorTarget,e.mainDepth);this.performanceInfo.advance(11);this._renderPlugins.needsLaserlineWithContrastControl&&e.renderTmpAndCompositeToMain(()=>this.renderSlot(15),2);this.performanceInfo.advance(12);this.renderOccluded();this.performanceInfo.advance(13);e=(g=1===g&&this._magnifierHelper.enabled)&&l.isNone(a)?this._offscreenRendering.getFramebuffer(this._offscreenRendering.tmpColor,
this._offscreenRendering.tmpDepth):a;this._rctx.bindFramebuffer(e);c=this._offscreenRendering.colorTexture;!this.renderAntiAliasing(this._antialiasing,c)&&l.isSome(c)&&this._compositingHelper.composite(c,0);this.performanceInfo.advance(14);this.renderHUD(1,e);this.performanceInfo.advance(17);this.renderHighlights(e,this._bindParameters);this.performanceInfo.advance(15);g&&this._magnifierHelper.render(this._rctx,this._bindParameters);e!==a&&(this._rctx.bindFramebuffer(a),this._compositingHelper.composite(this._offscreenRendering.tmpColorTexture,
0));this._renderContext.lastFrameCamera.copyFrom(this._renderContext.camera);this._sliceHelper.plane=h;this._isRendering=!1;if(this.onPostRender)this.onPostRender();this.performanceInfo.postrender()};f.readDepthPixels=function(a,c,d,g,e,h){const k=this._offscreenRendering.bindTarget(this._offscreenRendering.linearDepth,this._offscreenRendering.tmpDepth);this._needsLinearDepth||(this.ensureBindParameters(a),this._renderContext.camera.setGLViewport(this._rctx),this._rctx.setClearColor(0,0,0,0),this._rctx.clearSafe(17664),
this.renderGeometryAndTransparentTerrainPass(2));k.readPixels(c,d,g,e,6408,5121,h)};f.readAccumulatedShadow=function(a,c){return this._shadowAccumulator.readAccumulatedShadow(a,c)};f.setMultipassFlags=function(a){this._renderContext.multipassTerrainParams.multipassTerrainEnabled=this._bindParameters.multipassTerrainEnabled=a;this._renderContext.multipassGeometryParams.multipassGeometryEnabled=this._bindParameters.multipassGeometryEnabled=a};f.setTerrainCulling=function(a){this._renderContext.multipassTerrainParams.cullAboveGround=
this._bindParameters.cullAboveGround=a};f.renderSlot=function(a){this._renderContext.slot=a;return this._renderPlugins.render()};f.renderEdges=function(a,c=!1){const d=this._edgeView;l.isSome(d)&&d.shouldRender()&&this._offscreenRendering.renderTmpAndCompositeToMain(()=>d.render(this._bindParameters,a),1,c)};f.renderShadowMap=function(a,c,d,g){const e=this._shadowMap;e.enabled&&(e.start(c,d,g),this.needsShadowHighlight?(this.renderShadowCascades(7,this._shadowMap,h=>e.takeCascadeSnapshotTo(h,H.HighlightShadowMapSlot)),
e.clear(),this.renderShadowCascades(6,this._shadowMap,h=>{e.takeCascadeSnapshotTo(h,H.DefaultSnapshotSlot);this.renderGeometryAndTransparentTerrainPass(7)})):this.renderShadowCascades(4),e.finish(a),c.setGLViewport(this._rctx))};f.renderShadowCascades=function(a,c=this._shadowMap,d=g=>{}){for(const g of c.getCascades())g.camera.setGLViewport(this._rctx),this.ensureCameraBindParameters(g.camera),this.renderGeometryAndTransparentTerrainPass(a),d(g)};f.renderLinearDepth=function(){this._needsLinearDepth?
this._offscreenRendering.renderToTargets(()=>this.renderGeometryAndTransparentTerrainPass(2),this._offscreenRendering.linearDepth,this._offscreenRendering.tmpDepth,[0,0,0,0],!0,!0):this._offscreenRendering.disposeTarget(this._offscreenRendering.linearDepth);this._renderContext.ssrParams.linearDepthTexture=this._bindParameters.linearDepthTexture=this._offscreenRendering.linearDepthTexture};f.renderTerrainLinearDepth=function(a){a?(a=this._renderContext.pass,this._renderContext.pass=2,this._offscreenRendering.renderToTargets(()=>
this.renderTransparentTerrain(),this._offscreenRendering.terrainLinearDepth,this._offscreenRendering.tmpDepth,[-1E10,-1E10,-1E10,1],!0,!0),this._renderContext.pass=a):this._offscreenRendering.disposeTarget(this._offscreenRendering.terrainLinearDepth);this._renderContext.multipassTerrainParams.terrainLinearDepthTexture=this._bindParameters.terrainLinearDepthTexture=this._offscreenRendering.terrainLinearDepthTexture};f.renderGeometryLinearDepth=function(a){a?(a=this._renderContext.pass,this._offscreenRendering.renderToTargets(()=>
this.renderGeometryPass(2),this._offscreenRendering.geometryLinearDepth,this._offscreenRendering.tmpDepth,[1,1,1,1],!0,!0),this._renderContext.pass=a):this._offscreenRendering.disposeTarget(this._offscreenRendering.geometryLinearDepth);this._renderContext.multipassGeometryParams.geometryLinearDepthTexture=this._bindParameters.geometryLinearDepthTexture=this._offscreenRendering.geometryLinearDepthTexture};f.renderNormal=function(){this.needsNormal?this._offscreenRendering.renderToTargets(()=>{this.renderGeometryAndTransparentTerrainPass(3)},
this._offscreenRendering.normal,this._offscreenRendering.tmpDepth,[0,0,0,0],!0,!0):this._offscreenRendering.disposeTarget(this._offscreenRendering.normal)};f.computeDepthRange=function(a){if(!this.needsDepthRange)return G.ZERO;const c=S.depthRangeFromScene(a,this._content,this._stage.layers);G.union(c,this.renderPlugins.queryDepthRange(a));c.near=Math.max(a.near,c.near);c.far=Math.min(a.far,c.far);return c};f.renderGeometryAndTransparentTerrainPass=function(a){this._renderContext.pass=a;this.renderGeometryPass(a);
this.renderTransparentTerrain()};f.renderGeometryPass=function(a){this._renderContext.pass=a;this.renderOpaqueGeometry();this.renderTransparentGeometry()};f.renderOpaqueGeometry=function(){this.renderSlot(0);this.renderSlot(1);this.renderInternalSlot(2);this.renderSlot(3);this.renderSlot(12)};f.renderTransparentGeometry=function(){this.renderInternalSlot(4);this.renderSlot(5)};f.renderTransparentTerrain=function(){return this.renderSlot(6)};f.renderHUDVisibility=function(){let a=!1;this._renderContext.offscreenRenderingHelper&&
this._renderContext.offscreenRenderingHelper.renderHUDVisibility(()=>{this._bindParameters.hudVisibilityTexture=this._renderContext.offscreenRenderingHelper?this._renderContext.offscreenRenderingHelper.hudVisibilityTexture:null;a=this.renderInternalSlot(11)},this._multipassTerrain&&!this._opaqueTerrain);return a};f.renderLineCallouts=function(a){this._bindParameters.renderTransparentlyOccludedHUD=a;0===a?this._offscreenRendering.renderToTargets(()=>this.renderInternalSlot(18),this._offscreenRendering.currentColorTarget,
this._offscreenRendering.tmpDepth,void 0,!0,!0):this.renderInternalSlot(18)};f.renderHUD=function(a,c){this._oitEnabled?(this.renderOrderIndependentTransparency(()=>this.renderHUDElements(a),!0),this._rctx.bindFramebuffer(c),this._compositingHelper.composite(this._offscreenRendering.hudColorTexture,2)):0===a?this._offscreenRendering.renderToTargets(()=>this.renderHUDElements(0),this._offscreenRendering.currentColorTarget,this._offscreenRendering.tmpDepth,void 0,!0,!0):this.renderHUDElements(a)};f.renderHUDElements=
function(a){this._bindParameters.renderTransparentlyOccludedHUD=a;this.renderInternalSlot(19);this.renderInternalSlot(16);this.renderInternalSlot(17)};f.renderHighlights=function(a,c){if(this.needsHighlight){var d=this._highlightHelper,g=d.profilingCallback&&ka.startMeasurement(this._renderContext.rctx);this._renderContext.highlightDepthTexture=c.highlightDepthTexture;var e=this._offscreenRendering.renderToTargets(()=>{this.renderGeometryAndTransparentTerrainPass(5);this._rctx.clearSafe(256);this.renderHUDElements(2)},
this._offscreenRendering.highlight,this._offscreenRendering.tmpDepth,[0,0,0,0],!0,!0);this._bindParameters.highlightColorTexture=e.colorTexture;this.needsShadowHighlight&&this._shadowHighlightHelper.render(c,a);d.render(this._renderContext.camera,e,a);l.isSome(g)&&d.profilingCallback&&g.stop(h=>{d.profilingCallback&&d.profilingCallback(h)})}else this._offscreenRendering.disposeTarget(this._offscreenRendering.highlight)};f.accumulateShadows=function(a,c,d){this.needsShadowCast&&(this._shadowAccumulator.setAccumulationDependencies(this._offscreenRendering.linearDepthTexture,
a,c,d),this._shadowAccumulator.accumulateFixedSamples(),this.renderPassManager.shadowCastingEnabled=this._shadowMap.enabled)};f.renderOrderIndependentTransparency=function(a,c){const d=e=>{this._renderContext.transparencyPassType=e;this._bindParameters.transparencyPassType=this._renderContext.transparencyPassType;this._offscreenRendering.renderOITPass(()=>a(),e,c)},g=this._renderContext.pass;this._renderContext.pass=1;d(1);this._renderContext.pass=0;d(0);d(2);this._offscreenRendering.compositeTransparentOntoOpaque(c);
this._renderContext.transparencyPassType=3;this._bindParameters.transparencyPassType=this._renderContext.transparencyPassType;this._renderContext.pass=g};f.renderOccluded=function(){let a=0;this._materialRenderers.forEach((h,k)=>{k&&k.isVisible()&&8===k.renderOccluded&&(a|=k.renderOccluded,x.push(k))});const c=this._offscreenRendering,d=(h,k,m,q,b)=>{0!==(a&k)&&(c.renderToTargets(m,c.tmpColor,c.mainDepth,[0,0,0,0],q,b),c.compositeOccludedOntoMain(h))};0!==x.length&&(this.renderInternalSlot(9,x),d(.5,
8,()=>this.renderInternalSlot(10,x),!1,!1),x.length=0);this._materialRenderers.forEach((h,k)=>{k&&k.isVisible()&&(4===k.renderOccluded||2===k.renderOccluded||16===k.renderOccluded)&&(a|=k.renderOccluded,y.push(k))});const g=this._renderPlugins.renderOccludedFlags;if(a|=g){var e=h=>{this._renderContext.renderOccludedMask=h;1<g&&this.renderSlot(8);this.renderInternalSlot(2,y);this.renderInternalSlot(4,y);this.renderInternalSlot(7,y);this._renderContext.resetRenderOccludedMask()};this._renderContext.pass=
0;d(.5,4,()=>e(4),!0,2);d(.5,2,()=>e(2),!0,2);d(1,16,()=>e(16),!0,2);y.length=0}};f.renderAntiAliasing=function(a,c){if(1===a){if(this._smaaPass.enable(()=>this._requestRender())&&l.isSome(c))return this._smaaPass.render(c),!0}else this._smaaPass.disable();return!1};f.ensureCameraBindParameters=function(a){this._renderContext.camera=a;this._bindParameters.camera=this._renderContext.camera;this._bindParameters.inverseViewport[0]=1/this._renderContext.camera.fullViewport[2];this._bindParameters.inverseViewport[1]=
1/this._renderContext.camera.fullViewport[3]};f.ensureBindParameters=function(a){var c;this.ensureCameraBindParameters(a);a=this._renderContext.offscreenRenderingHelper;this._bindParameters.shadowMap=this._renderContext.shadowMap;this._bindParameters.shadowMappingEnabled=this._renderContext.shadowMap.enabled;this._bindParameters.ssaoHelper=this._renderContext.ssaoHelper;this._bindParameters.ssaoEnabled=this._renderContext.ssaoHelper.enabled;this._bindParameters.slicePlane=this._renderContext.sliceHelper.plane;
this._bindParameters.hasOccludees=this._renderContext.hasOccludees;this._renderContext.multipassTerrainParams.camera=this._renderContext.camera;this._bindParameters.hudVisibilityTexture=a.hudVisibilityTexture;this._bindParameters.highlightDepthTexture=null!=(c=a.depthTexture)?c:this.getFallbackDepthTexture()};f.ensureBindParametersSSR=function(){this.hasWaterReflection?(this._renderContext.lastFrameCamera.equals(this._renderContext.camera)?this._renderContext.ssrParams.reprojectionMatrix=this._reprojectionMatrix=
r.IDENTITY:(p.translate(J,this._renderContext.lastFrameCamera.viewMatrix,this._bindParameters.origin?this._bindParameters.origin:[0,0,0]),p.translate(A,this._renderContext.camera.viewMatrix,this._bindParameters.origin?this._bindParameters.origin:[0,0,0]),p.invert(A,A),p.invert(K,this._renderContext.camera.projectionMatrix),p.multiply(w,A,K),p.multiply(w,J,w),p.multiply(w,this._renderContext.lastFrameCamera.projectionMatrix,w),this._renderContext.ssrParams.reprojectionMatrix=this._reprojectionMatrix=
w),this._renderContext.ssrParams.camera=this._renderContext.camera,this._renderContext.ssrParams.lastFrameColorTexture=this._bindParameters.lastFrameColorTexture=this._offscreenRendering.lastFrameColorTexture):this._renderContext.ssrParams.lastFrameColorTexture=this._bindParameters.lastFrameColorTexture=null;this._renderContext.ssrParams.ssrEnabled=this._bindParameters.ssrEnabled=this.hasWaterReflection};f.renderInternalSlot=function(a,c){let d=!1;this._bindParameters.slot=a;l.isSome(c)?c.forEach(g=>
{g.shouldRender(this._renderContext)&&(g=this._materialRenderers.get(g),l.isSome(g)&&(d=g.render(a,this._renderContext.pass,this._bindParameters)||d))}):this._materialRenderers.forEach((g,e)=>{e.shouldRender(this._renderContext)&&(d=g.render(a,this._renderContext.pass,this._bindParameters)||d)});return d};f.getFallbackDepthTexture=function(){this._fallbackDepthStencilTexture||(this._fallbackDepthStencilTexture=T.createEmptyDepthTexture(this._rctx));return this._fallbackDepthStencilTexture};E._createClass(z,
[{key:"hasWater",get:function(){return this._hasWater||this._hasOverlayWater}},{key:"hasWaterReflection",get:function(){return this.hasWater&&this._waterReflectionEnabled}},{key:"updating",get:function(){return 1===this._antialiasing&&this._smaaPass.updating||l.isSome(this._edgeView)&&this._edgeView.updating||this._shadowAccumulator.running||!this.isCameraFinal}},{key:"edgeView",get:function(){return this._edgeView}},{key:"isCameraFinal",get:function(){return null===this._bindParameters.reprojectionMatrix||
p.equals(this._bindParameters.reprojectionMatrix,r.IDENTITY)}},{key:"_reprojectionMatrix",set:function(a){p.equals(this._bindParameters.reprojectionMatrix,a)||(this._bindParameters.reprojectionMatrix=a,this.notifyChange("isCameraFinal"))}},{key:"hasShadowsEnabled",get:function(){var a;return!(null==(a=this._shadowMap)||!a.enabled)}},{key:"hasSlicePlane",get:function(){return!!this._sliceHelper.plane}},{key:"renderPlugins",get:function(){return this._renderPlugins}},{key:"_hasOITSupport",get:function(){return this._rctx.driverTest.floatBufferBlendWorking}},
{key:"_needsLinearDepth",get:function(){return this._ssaoHelper.enabled||this._renderPlugins.needsLinearDepth||this._hasWater&&this._waterReflectionEnabled||this.needsShadowHighlight||this.needsShadowCast}},{key:"needsNormal",get:function(){return this._ssaoHelper.enabled}},{key:"needsDepthRange",get:function(){return this._shadowMap.enabled||this.needsShadowCast}},{key:"needsHighlight",get:function(){return this._hasHighlights||this._renderPlugins.needsHighlight}},{key:"needsShadowHighlight",get:function(){return this._shadowMap.enabled&&
this._shadowHighlightHelper.isVisible&&this.needsHighlight}},{key:"needsShadowCast",get:function(){return this._shadowAccumulator.isAccumulating}},{key:"gpuMemoryUsage",get:function(){var a,c,d,g,e,h,k,m,q,b;return{offscreen:null!=(a=null==(c=this._offscreenRendering)?void 0:c.gpuMemoryUsage)?a:0,highlights:(null!=(d=null==(g=this._highlightHelper)?void 0:g.gpuMemoryUsage)?d:0)+(null!=(e=null==(h=this._shadowHighlightHelper)?void 0:h.gpuMemoryUsage)?e:0),shadows:null!=(k=null==(m=this._shadowMap)?
void 0:m.gpuMemoryUsage)?k:0,ssao:null!=(q=null==(b=this._ssaoHelper)?void 0:b.gpuMemoryUsage)?q:0}}},{key:"test",get:function(){const a=this;return{offscreen:this._offscreenRendering,shadowMap:this._shadowMap,ssao:this._ssaoHelper,highlight:this._highlightHelper,lighting:this._lighting,materialRenderers:this._materialRenderers,shadowAccumulator:this._shadowAccumulator,getFramebufferTexture:c=>{var d;switch(c){case 0:return a._offscreenRendering.colorTexture;case 1:return a._offscreenRendering.linearDepthTexture;
case 2:return a._offscreenRendering.normalTexture;case 3:return null==(d=a._shadowMap)?void 0:d.test.depthTexture;case 4:return a._offscreenRendering.hudVisibilityTexture;case 5:return a._offscreenRendering.highlightTexture}}}}}]);return z}(L);t.__decorate([u.property()],n.Renderer.prototype,"_shadowAccumulator",void 0);t.__decorate([u.property()],n.Renderer.prototype,"_smaaPass",void 0);t.__decorate([u.property()],n.Renderer.prototype,"_antialiasing",void 0);t.__decorate([u.property()],n.Renderer.prototype,
"_edgeView",void 0);t.__decorate([u.property()],n.Renderer.prototype,"updating",null);t.__decorate([u.property()],n.Renderer.prototype,"isCameraFinal",null);n.Renderer=t.__decorate([N.subclass("esri.views.3d.webgl-engine.lib.Renderer")],n.Renderer);const y=[],x=[],J=r.create(),K=r.create(),A=r.create(),w=r.create();Object.defineProperty(n,"__esModule",{value:!0})});