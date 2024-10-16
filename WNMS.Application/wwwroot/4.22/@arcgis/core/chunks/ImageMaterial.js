/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{o as e,D as t,P as a}from"./ScreenSpacePass.js";import{G as s}from"./Texture2.js";import{S as r,T as i,g as n,q as o,R as l,A as p,H as u,C as c,p as d,a as h,b as m,c as g,P as f,D as v,h as b,L as T,Q as y,V as P,E as x,W as C,G as E,a1 as _,a4 as O,a0 as S}from"./StencilUtils.js";import{a as F,m as w,O as D,f as q,g as H,c as j,i as G,d as A,j as I,k as $}from"./OrderIndependentTransparency.js";import{_ as W}from"./tslib.es6.js";const z=Object.freeze({__proto__:null,build:function(e){const t=new r;return t.include(i,{linearDepth:!1}),t.vertex.uniforms.add("proj","mat4").add("view","mat4"),t.attributes.add("position","vec3"),t.attributes.add("uv0","vec2"),t.varyings.add("vpos","vec3"),e.multipassTerrainEnabled&&t.varyings.add("depth","float"),t.vertex.uniforms.add("textureCoordinateScaleFactor","vec2"),t.vertex.code.add(n`
    void main(void) {
      vpos = position;
      ${e.multipassTerrainEnabled?"depth = (view * vec4(vpos, 1.0)).z;":""}
      vTexCoord = uv0 * textureCoordinateScaleFactor;
      gl_Position = transformPosition(proj, view, vpos);
    }
  `),t.include(o,e),e.multipassTerrainEnabled&&(t.fragment.include(l),t.include(p,e)),t.fragment.uniforms.add("tex","sampler2D"),t.fragment.uniforms.add("opacity","float"),t.varyings.add("vTexCoord","vec2"),7===e.output?t.fragment.code.add(n`
    void main() {
      discardBySlice(vpos);
      ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}

      float alpha = texture2D(tex, vTexCoord).a * opacity;
      if (alpha  < ${n.float(u)}) {
        discard;
      }

      gl_FragColor = vec4(alpha);
    }
    `):(t.fragment.include(c),t.fragment.code.add(n`
    void main() {
      discardBySlice(vpos);
      ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}
      gl_FragColor = texture2D(tex, vTexCoord) * opacity;

      if (gl_FragColor.a < ${n.float(u)}) {
        discard;
      }

      gl_FragColor = highlightSlice(gl_FragColor, vpos);
      ${e.OITEnabled?"gl_FragColor = premultiplyAlpha(gl_FragColor);":""}
    }
    `)),t}});class U extends g{initializeProgram(e){const t=U.shader.get(),a=this.configuration,s=t.build({output:a.output,slicePlaneEnabled:a.slicePlaneEnabled,sliceHighlightDisabled:!1,sliceEnabledForVertexPrograms:!1,OITEnabled:0===a.transparencyPassType,multipassTerrainEnabled:a.multipassTerrainEnabled,cullAboveGround:a.cullAboveGround});return new f(e.rctx,s,v)}bindPass(e,t){b(this.program,t.camera.projectionMatrix),this.program.setUniform1f("opacity",e.opacity),t.multipassTerrainEnabled&&(this.program.setUniform2fv("cameraNearFar",t.camera.nearFar),this.program.setUniform2fv("inverseViewport",t.inverseViewport),T(this.program,t))}bindDraw(e){y(this.program,e),P(this.program,this.configuration,e),this.program.rebindTextures()}setPipelineState(e,t){const a=this.configuration,s=3===e,r=2===e;return w({blending:0!==a.output&&7!==a.output||!a.transparent?null:s?V:D(e),culling:q(a.cullFace),depthTest:{func:H(e)},depthWrite:s?a.writeDepth&&j:G(e),colorWrite:A,stencilWrite:a.sceneHasOcludees?x:null,stencilTest:a.sceneHasOcludees?t?C:E:null,polygonOffset:s||r?null:I(a.enableOffset)})}initializePipeline(){return this._occludeePipelineState=this.setPipelineState(this.configuration.transparencyPassType,!0),this.setPipelineState(this.configuration.transparencyPassType,!1)}getPipelineState(e,t){return t?this._occludeePipelineState:super.getPipelineState(e,t)}}U.shader=new h(z,(()=>Promise.resolve().then((()=>z))));const V=F(1,771);class B extends m{constructor(){super(...arguments),this.output=0,this.cullFace=0,this.slicePlaneEnabled=!1,this.transparent=!1,this.enableOffset=!0,this.writeDepth=!0,this.sceneHasOcludees=!1,this.transparencyPassType=3,this.multipassTerrainEnabled=!1,this.cullAboveGround=!1}}W([d({count:8})],B.prototype,"output",void 0),W([d({count:3})],B.prototype,"cullFace",void 0),W([d()],B.prototype,"slicePlaneEnabled",void 0),W([d()],B.prototype,"transparent",void 0),W([d()],B.prototype,"enableOffset",void 0),W([d()],B.prototype,"writeDepth",void 0),W([d()],B.prototype,"sceneHasOcludees",void 0),W([d({count:4})],B.prototype,"transparencyPassType",void 0),W([d()],B.prototype,"multipassTerrainEnabled",void 0),W([d()],B.prototype,"cullAboveGround",void 0);class L extends _{constructor(e){super(e,k),this.supportsEdges=!0,this.techniqueConfig=new B}getTechniqueConfig(e,t){return this.techniqueConfig.output=e,this.techniqueConfig.cullFace=this.parameters.cullFace,this.techniqueConfig.slicePlaneEnabled=this.parameters.slicePlaneEnabled,this.techniqueConfig.transparent=this.parameters.transparent,this.techniqueConfig.writeDepth=this.parameters.writeDepth,this.techniqueConfig.sceneHasOcludees=this.parameters.sceneHasOcludees,this.techniqueConfig.transparencyPassType=t.transparencyPassType,this.techniqueConfig.enableOffset=t.camera.relativeElevation<$,this.techniqueConfig.multipassTerrainEnabled=t.multipassTerrainEnabled,this.techniqueConfig.cullAboveGround=t.cullAboveGround,this.techniqueConfig}intersect(e,t,a,s,r,i,n){O(e,t,s,r,i,void 0,n)}requiresSlot(t,a){if(20===t)return!0;if(4===e(a))return 2===t;return t===(this.parameters.transparent?this.parameters.writeDepth?4:7:2)}createGLMaterial(e){return 0===e.output||7===e.output||4===e.output?new M(e):void 0}createBufferWriter(){return new t(a)}}class M extends s{constructor(e){super({...e,...e.material.parameters})}updateParameters(e){const t=this._material.parameters;return this.updateTexture(t.textureId),this.ensureTechnique(U,e)}_updateOccludeeState(e){e.hasOccludees!==this._material.parameters.sceneHasOcludees&&(this._material.setParameters({sceneHasOcludees:e.hasOccludees}),this.updateParameters(e))}beginSlot(e){return 0!==this._output&&7!==this._output||this._updateOccludeeState(e),this.updateParameters(e)}bind(e,t){this.bindTextures(t.program),this.bindTextureScale(t.program),t.bindPass(this._material.parameters,e)}}const k={transparent:!1,writeDepth:!0,slicePlaneEnabled:!1,cullFace:0,sceneHasOcludees:!1,opacity:1,textureId:null,initTextureTransparent:!0,...S};export{L as I};
