/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{a as e}from"./devEnvironmentUtils.js";import{h as t,i as o,u as a,j as r,b as i}from"../core/lang.js";import{n}from"./mat3.js";import{c as s}from"./quatf64.js";import{a as l}from"./mat4.js";import{c as d}from"./mat4f64.js";import{b as c,k as u,e as m,m as v,n as p,s as f,l as h,f as g,d as x,t as b,U as y,j as C,aa as T,h as M}from"./mathUtils.js";import{b as w,u as S}from"./aaBoundingBox.js";import{a as A,g as _,e as P,b as O,l as R,m as E,n as L}from"./BufferView.js";import{t as F,a as D,s as z,c as B}from"./vec3.js";import{l as N,D as I,C as V,c as G,t as $,n as H,s as U,a as W,f as j,b as k,d as q,e as X}from"./DefaultMaterial_COLOR_GAMMA.js";import Q from"../request.js";import{r as K}from"./asyncUtils.js";import J from"../core/Error.js";import{L as Y}from"./Logger.js";import{throwIfAbortError as Z}from"../core/promiseUtils.js";import{V as ee}from"./Version.js";import{r as te}from"./requestImageUtils.js";import{g as oe,T as ae,O as re,q as ie,y as ne,r as se,C as le,S as de,z as ce,R as ue,A as me,p as ve,a as pe,b as fe,c as he,P as ge,D as xe,h as be,L as ye,a9 as Ce,B as Te,t as Me,ae as we,U as Se,u as Ae,E as _e,W as Pe,G as Oe,a1 as Re,af as Ee,a4 as Le,H as Fe,a0 as De,ab as ze,a3 as Be}from"./StencilUtils.js";import{G as Ne,T as Ie}from"./Texture2.js";import{n as Ve}from"./InterleavedLayout.js";import{m as Ge,e as $e,O as He,f as Ue,g as We,c as je,d as ke,j as qe,k as Xe}from"./OrderIndependentTransparency.js";import{a as Qe}from"./utils14.js";import{_ as Ke}from"./tslib.es6.js";import{a as Je}from"./doublePrecisionUtils.js";import{V as Ye,b as Ze}from"./VerticalOffset.glsl.js";import{f as et}from"./vec3f32.js";import{V as tt,a as ot,R as at,P as rt,F as it,e as nt,f as st}from"./PhysicallyBasedRendering.glsl.js";import{V as lt}from"./VertexColor.glsl.js";import{w as dt}from"./Texture.js";function ct(e,t){1===t.attributeTextureCoordinates&&(e.attributes.add("uv0","vec2"),e.varyings.add("vuv0","vec2"),e.vertex.code.add(oe`void forwardTextureCoordinates() {
vuv0 = uv0;
}`)),2===t.attributeTextureCoordinates&&(e.attributes.add("uv0","vec2"),e.varyings.add("vuv0","vec2"),e.attributes.add("uvRegion","vec4"),e.varyings.add("vuvRegion","vec4"),e.vertex.code.add(oe`void forwardTextureCoordinates() {
vuv0 = uv0;
vuvRegion = uvRegion;
}`)),0===t.attributeTextureCoordinates&&e.vertex.code.add(oe`void forwardTextureCoordinates() {}`)}function ut(e,t){const o=e.fragment,a=void 0!==t.lightingSphericalHarmonicsOrder?t.lightingSphericalHarmonicsOrder:2;0===a?(o.uniforms.add("lightingAmbientSH0","vec3"),o.code.add(oe`vec3 calculateAmbientIrradiance(vec3 normal, float ambientOcclusion) {
vec3 ambientLight = 0.282095 * lightingAmbientSH0;
return ambientLight * (1.0 - ambientOcclusion);
}`)):1===a?(o.uniforms.add("lightingAmbientSH_R","vec4"),o.uniforms.add("lightingAmbientSH_G","vec4"),o.uniforms.add("lightingAmbientSH_B","vec4"),o.code.add(oe`vec3 calculateAmbientIrradiance(vec3 normal, float ambientOcclusion) {
vec4 sh0 = vec4(
0.282095,
0.488603 * normal.x,
0.488603 * normal.z,
0.488603 * normal.y
);
vec3 ambientLight = vec3(
dot(lightingAmbientSH_R, sh0),
dot(lightingAmbientSH_G, sh0),
dot(lightingAmbientSH_B, sh0)
);
return ambientLight * (1.0 - ambientOcclusion);
}`)):2===a&&(o.uniforms.add("lightingAmbientSH0","vec3"),o.uniforms.add("lightingAmbientSH_R1","vec4"),o.uniforms.add("lightingAmbientSH_G1","vec4"),o.uniforms.add("lightingAmbientSH_B1","vec4"),o.uniforms.add("lightingAmbientSH_R2","vec4"),o.uniforms.add("lightingAmbientSH_G2","vec4"),o.uniforms.add("lightingAmbientSH_B2","vec4"),o.code.add(oe`vec3 calculateAmbientIrradiance(vec3 normal, float ambientOcclusion) {
vec3 ambientLight = 0.282095 * lightingAmbientSH0;
vec4 sh1 = vec4(
0.488603 * normal.x,
0.488603 * normal.z,
0.488603 * normal.y,
1.092548 * normal.x * normal.y
);
vec4 sh2 = vec4(
1.092548 * normal.y * normal.z,
0.315392 * (3.0 * normal.z * normal.z - 1.0),
1.092548 * normal.x * normal.z,
0.546274 * (normal.x * normal.x - normal.y * normal.y)
);
ambientLight += vec3(
dot(lightingAmbientSH_R1, sh1),
dot(lightingAmbientSH_G1, sh1),
dot(lightingAmbientSH_B1, sh1)
);
ambientLight += vec3(
dot(lightingAmbientSH_R2, sh2),
dot(lightingAmbientSH_G2, sh2),
dot(lightingAmbientSH_B2, sh2)
);
return ambientLight * (1.0 - ambientOcclusion);
}`),1!==t.pbrMode&&2!==t.pbrMode||o.code.add(oe`const vec3 skyTransmittance = vec3(0.9, 0.9, 1.0);
vec3 calculateAmbientRadiance(float ambientOcclusion)
{
vec3 ambientLight = 1.2 * (0.282095 * lightingAmbientSH0) - 0.2;
return ambientLight *= (1.0 - ambientOcclusion) * skyTransmittance;
}`))}function mt(e){const t=e.fragment;t.uniforms.add("lightingMainDirection","vec3"),t.uniforms.add("lightingMainIntensity","vec3"),t.uniforms.add("lightingFixedFactor","float"),t.code.add(oe`vec3 evaluateMainLighting(vec3 normal_global, float shadowing) {
float dotVal = clamp(dot(normal_global, lightingMainDirection), 0.0, 1.0);
dotVal = mix(dotVal, 1.0, lightingFixedFactor);
return lightingMainIntensity * ((1.0 - shadowing) * dotVal);
}`)}function vt({code:e},t){t.doublePrecisionRequiresObfuscation?e.add(oe`vec3 dpPlusFrc(vec3 a, vec3 b) {
return mix(a, a + b, vec3(notEqual(b, vec3(0))));
}
vec3 dpMinusFrc(vec3 a, vec3 b) {
return mix(vec3(0), a - b, vec3(notEqual(a, b)));
}
vec3 dpAdd(vec3 hiA, vec3 loA, vec3 hiB, vec3 loB) {
vec3 t1 = dpPlusFrc(hiA, hiB);
vec3 e = dpMinusFrc(t1, hiA);
vec3 t2 = dpMinusFrc(hiB, e) + dpMinusFrc(hiA, dpMinusFrc(t1, e)) + loA + loB;
return t1 + t2;
}`):e.add(oe`vec3 dpAdd(vec3 hiA, vec3 loA, vec3 hiB, vec3 loB) {
vec3 t1 = hiA + hiB;
vec3 e = t1 - hiA;
vec3 t2 = ((hiB - e) + (hiA - (t1 - e))) + loA + loB;
return t1 + t2;
}`)}function pt(e){return!!t("force-double-precision-obfuscation")||e.driverTest.doublePrecisionRequiresObfuscation}function ft(e,t){t.instanced&&t.instancedDoublePrecision&&(e.attributes.add("modelOriginHi","vec3"),e.attributes.add("modelOriginLo","vec3"),e.attributes.add("model","mat3"),e.attributes.add("modelNormal","mat3")),t.instancedDoublePrecision&&(e.vertex.include(vt,t),e.vertex.uniforms.add("viewOriginHi","vec3"),e.vertex.uniforms.add("viewOriginLo","vec3"));const o=[oe`
    vec3 calculateVPos() {
      ${t.instancedDoublePrecision?"return model * localPosition().xyz;":"return localPosition().xyz;"}
    }
    `,oe`
    vec3 subtractOrigin(vec3 _pos) {
      ${t.instancedDoublePrecision?oe`
          vec3 originDelta = dpAdd(viewOriginHi, viewOriginLo, -modelOriginHi, -modelOriginLo);
          return _pos - originDelta;`:"return vpos;"}
    }
    `,oe`
    vec3 dpNormal(vec4 _normal) {
      ${t.instancedDoublePrecision?"return normalize(modelNormal * _normal.xyz);":"return normalize(_normal.xyz);"}
    }
    `,oe`
    vec3 dpNormalView(vec4 _normal) {
      ${t.instancedDoublePrecision?"return normalize((viewNormal * vec4(modelNormal * _normal.xyz, 1.0)).xyz);":"return normalize((viewNormal * _normal).xyz);"}
    }
    `,t.vertexTangents?oe`
    vec4 dpTransformVertexTangent(vec4 _tangent) {
      ${t.instancedDoublePrecision?"return vec4(modelNormal * _tangent.xyz, _tangent.w);":"return _tangent;"}

    }
    `:oe``];e.vertex.code.add(o[0]),e.vertex.code.add(o[1]),e.vertex.code.add(o[2]),2===t.output&&e.vertex.code.add(o[3]),e.vertex.code.add(o[4])}!function(e){e.Uniforms=class{},e.bindCustomOrigin=function(e,t){Je(t,ht,gt,3),e.setUniform3fv("viewOriginHi",ht),e.setUniform3fv("viewOriginLo",gt)}}(ft||(ft={}));const ht=c(),gt=c();function xt(e){e.extensions.add("GL_EXT_shader_texture_lod"),e.extensions.add("GL_OES_standard_derivatives"),e.fragment.code.add(oe`#ifndef GL_EXT_shader_texture_lod
float calcMipMapLevel(const vec2 ddx, const vec2 ddy) {
float deltaMaxSqr = max(dot(ddx, ddx), dot(ddy, ddy));
return max(0.0, 0.5 * log2(deltaMaxSqr));
}
#endif
vec4 textureAtlasLookup(sampler2D texture, vec2 textureSize, vec2 textureCoordinates, vec4 atlasRegion) {
vec2 atlasScale = atlasRegion.zw - atlasRegion.xy;
vec2 uvAtlas = fract(textureCoordinates) * atlasScale + atlasRegion.xy;
float maxdUV = 0.125;
vec2 dUVdx = clamp(dFdx(textureCoordinates), -maxdUV, maxdUV) * atlasScale;
vec2 dUVdy = clamp(dFdy(textureCoordinates), -maxdUV, maxdUV) * atlasScale;
#ifdef GL_EXT_shader_texture_lod
return texture2DGradEXT(texture, uvAtlas, dUVdx, dUVdy);
#else
vec2 dUVdxAuto = dFdx(uvAtlas);
vec2 dUVdyAuto = dFdy(uvAtlas);
float mipMapLevel = calcMipMapLevel(dUVdx * textureSize, dUVdy * textureSize);
float autoMipMapLevel = calcMipMapLevel(dUVdxAuto * textureSize, dUVdyAuto * textureSize);
return texture2D(texture, uvAtlas, mipMapLevel - autoMipMapLevel);
#endif
}`)}function bt(e,t){e.include(ct,t),e.fragment.code.add(oe`
  struct TextureLookupParameter {
    vec2 uv;
    ${t.supportsTextureAtlas?"vec2 size;":""}
  } vtc;
  `),1===t.attributeTextureCoordinates&&e.fragment.code.add(oe`vec4 textureLookup(sampler2D tex, TextureLookupParameter params) {
return texture2D(tex, params.uv);
}`),2===t.attributeTextureCoordinates&&(e.include(xt),e.fragment.code.add(oe`vec4 textureLookup(sampler2D tex, TextureLookupParameter params) {
return textureAtlasLookup(tex, params.size, params.uv, vuvRegion);
}`))}const yt=et(0,.6,.2);function Ct(e,t){const o=e.fragment,a=t.hasMetalnessAndRoughnessTexture||t.hasEmissionTexture||t.hasOcclusionTexture;1===t.pbrMode&&a&&e.include(bt,t),2!==t.pbrMode?(0===t.pbrMode&&o.code.add(oe`float getBakedOcclusion() { return 1.0; }`),1===t.pbrMode&&(o.uniforms.add("emissionFactor","vec3"),o.uniforms.add("mrrFactors","vec3"),o.code.add(oe`vec3 mrr;
vec3 emission;
float occlusion;`),t.hasMetalnessAndRoughnessTexture&&(o.uniforms.add("texMetallicRoughness","sampler2D"),t.supportsTextureAtlas&&o.uniforms.add("texMetallicRoughnessSize","vec2"),o.code.add(oe`void applyMetallnessAndRoughness(TextureLookupParameter params) {
vec3 metallicRoughness = textureLookup(texMetallicRoughness, params).rgb;
mrr[0] *= metallicRoughness.b;
mrr[1] *= metallicRoughness.g;
}`)),t.hasEmissionTexture&&(o.uniforms.add("texEmission","sampler2D"),t.supportsTextureAtlas&&o.uniforms.add("texEmissionSize","vec2"),o.code.add(oe`void applyEmission(TextureLookupParameter params) {
emission *= textureLookup(texEmission, params).rgb;
}`)),t.hasOcclusionTexture?(o.uniforms.add("texOcclusion","sampler2D"),t.supportsTextureAtlas&&o.uniforms.add("texOcclusionSize","vec2"),o.code.add(oe`void applyOcclusion(TextureLookupParameter params) {
occlusion *= textureLookup(texOcclusion, params).r;
}
float getBakedOcclusion() {
return occlusion;
}`)):o.code.add(oe`float getBakedOcclusion() { return 1.0; }`),o.code.add(oe`
    void applyPBRFactors() {
      mrr = mrrFactors;
      emission = emissionFactor;
      occlusion = 1.0;
      ${a?"vtc.uv = vuv0;":""}
      ${t.hasMetalnessAndRoughnessTexture?t.supportsTextureAtlas?"vtc.size = texMetallicRoughnessSize; applyMetallnessAndRoughness(vtc);":"applyMetallnessAndRoughness(vtc);":""}
      ${t.hasEmissionTexture?t.supportsTextureAtlas?"vtc.size = texEmissionSize; applyEmission(vtc);":"applyEmission(vtc);":""}
      ${t.hasOcclusionTexture?t.supportsTextureAtlas?"vtc.size = texOcclusionSize; applyOcclusion(vtc);":"applyOcclusion(vtc);":""}
    }
  `))):o.code.add(oe`const vec3 mrr = vec3(0.0, 0.6, 0.2);
const vec3 emission = vec3(0.0);
float occlusion = 1.0;
void applyPBRFactors() {}
float getBakedOcclusion() { return 1.0; }`)}function Tt(e,t,o=!1){o||(e.setUniform3fv("mrrFactors",t.mrrFactors),e.setUniform3fv("emissionFactor",t.emissiveFactor))}function Mt(e){e.vertex.code.add(oe`vec4 offsetBackfacingClipPosition(vec4 posClip, vec3 posWorld, vec3 normalWorld, vec3 camPosWorld) {
vec3 camToVert = posWorld - camPosWorld;
bool isBackface = dot(camToVert, normalWorld) > 0.0;
if (isBackface) {
posClip.z += 0.0000003 * posClip.w;
}
return posClip;
}`)}function wt(e){const t=oe`vec3 decodeNormal(vec2 f) {
float z = 1.0 - abs(f.x) - abs(f.y);
return vec3(f + sign(f) * min(z, 0.0), z);
}`;e.fragment.code.add(t),e.vertex.code.add(t)}function St(e,t){0===t.normalType&&(e.attributes.add("normal","vec3"),e.vertex.code.add(oe`vec3 normalModel() {
return normal;
}`)),1===t.normalType&&(e.include(wt),e.attributes.add("normalCompressed","vec2"),e.vertex.code.add(oe`vec3 normalModel() {
return decodeNormal(normalCompressed);
}`)),3===t.normalType&&(e.extensions.add("GL_OES_standard_derivatives"),e.fragment.code.add(oe`vec3 screenDerivativeNormal(vec3 positionView) {
return normalize(cross(dFdx(positionView), dFdy(positionView)));
}`))}function At(e){e.attributes.add("position","vec3"),e.vertex.code.add(oe`vec3 positionModel() { return position; }`)}function _t(e){e.vertex.code.add(oe`
    vec4 decodeSymbolColor(vec4 symbolColor, out int colorMixMode) {
      float symbolAlpha = 0.0;

      const float maxTint = 85.0;
      const float maxReplace = 170.0;
      const float scaleAlpha = 3.0;

      if (symbolColor.a > maxReplace) {
        colorMixMode = ${oe.int(1)};
        symbolAlpha = scaleAlpha * (symbolColor.a - maxReplace);
      } else if (symbolColor.a > maxTint) {
        colorMixMode = ${oe.int(3)};
        symbolAlpha = scaleAlpha * (symbolColor.a - maxTint);
      } else if (symbolColor.a > 0.0) {
        colorMixMode = ${oe.int(4)};
        symbolAlpha = scaleAlpha * symbolColor.a;
      } else {
        colorMixMode = ${oe.int(1)};
        symbolAlpha = 0.0;
      }

      return vec4(symbolColor.r, symbolColor.g, symbolColor.b, symbolAlpha);
    }
  `)}function Pt(e,t){t.symbolColor?(e.include(_t),e.attributes.add("symbolColor","vec4"),e.varyings.add("colorMixMode","mediump float")):e.fragment.uniforms.add("colorMixMode","int"),t.symbolColor?e.vertex.code.add(oe`int symbolColorMixMode;
vec4 getSymbolColor() {
return decodeSymbolColor(symbolColor, symbolColorMixMode) * 0.003921568627451;
}
void forwardColorMixMode() {
colorMixMode = float(symbolColorMixMode) + 0.5;
}`):e.vertex.code.add(oe`vec4 getSymbolColor() { return vec4(1.0); }
void forwardColorMixMode() {}`)}function Ot(e,t){e.include(At),e.vertex.include(vt,t),e.varyings.add("vPositionWorldCameraRelative","vec3"),e.varyings.add("vPosition_view","vec3"),e.vertex.uniforms.add("uTransform_WorldFromModel_RS","mat3"),e.vertex.uniforms.add("uTransform_WorldFromModel_TH","vec3"),e.vertex.uniforms.add("uTransform_WorldFromModel_TL","vec3"),e.vertex.uniforms.add("uTransform_WorldFromView_TH","vec3"),e.vertex.uniforms.add("uTransform_WorldFromView_TL","vec3"),e.vertex.uniforms.add("uTransform_ViewFromCameraRelative_RS","mat3"),e.vertex.uniforms.add("uTransform_ProjFromView","mat4"),e.vertex.code.add(oe`vec3 positionWorldCameraRelative() {
vec3 rotatedModelPosition = uTransform_WorldFromModel_RS * positionModel();
vec3 transform_CameraRelativeFromModel = dpAdd(
uTransform_WorldFromModel_TL,
uTransform_WorldFromModel_TH,
-uTransform_WorldFromView_TL,
-uTransform_WorldFromView_TH
);
return transform_CameraRelativeFromModel + rotatedModelPosition;
}
vec3 position_view() {
return uTransform_ViewFromCameraRelative_RS * positionWorldCameraRelative();
}
void forwardPosition() {
vPositionWorldCameraRelative = positionWorldCameraRelative();
vPosition_view = position_view();
gl_Position = uTransform_ProjFromView * vec4(vPosition_view, 1.0);
}
vec3 positionWorld() {
return uTransform_WorldFromView_TL + vPositionWorldCameraRelative;
}`),e.fragment.uniforms.add("uTransform_WorldFromView_TL","vec3"),e.fragment.code.add(oe`vec3 positionWorld() {
return uTransform_WorldFromView_TL + vPositionWorldCameraRelative;
}`)}function Rt(e,t){0===t.normalType||1===t.normalType?(e.include(St,t),e.varyings.add("vNormalWorld","vec3"),e.varyings.add("vNormalView","vec3"),e.vertex.uniforms.add("uTransformNormal_GlobalFromModel","mat3"),e.vertex.uniforms.add("uTransformNormal_ViewFromGlobal","mat3"),e.vertex.code.add(oe`void forwardNormal() {
vNormalWorld = uTransformNormal_GlobalFromModel * normalModel();
vNormalView = uTransformNormal_ViewFromGlobal * vNormalWorld;
}`)):2===t.normalType?(e.include(Ot,t),e.varyings.add("vNormalWorld","vec3"),e.vertex.code.add(oe`
    void forwardNormal() {
      vNormalWorld = ${1===t.viewingMode?oe`normalize(vPositionWorldCameraRelative);`:oe`vec3(0.0, 0.0, 1.0);`}
    }
    `)):e.vertex.code.add(oe`void forwardNormal() {}`)}function Et(e,t){const o=e.vertex.code,a=e.fragment.code;1!==t.output&&3!==t.output||(e.include(ae,{linearDepth:!0}),e.include(ct,t),e.include(tt,t),e.include(re,t),e.include(ie,t),e.vertex.uniforms.add("cameraNearFar","vec2"),e.varyings.add("depth","float"),t.hasColorTexture&&e.fragment.uniforms.add("tex","sampler2D"),o.add(oe`void main(void) {
vpos = calculateVPos();
vpos = subtractOrigin(vpos);
vpos = addVerticalOffset(vpos, localOrigin);
gl_Position = transformPositionWithDepth(proj, view, vpos, cameraNearFar, depth);
forwardTextureCoordinates();
}`),e.include(ne,t),a.add(oe`
      void main(void) {
        discardBySlice(vpos);
        ${t.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        discardOrAdjustAlpha(texColor);`:""}
        outputDepth(depth);
      }
    `)),2===t.output&&(e.include(ae,{linearDepth:!1}),e.include(St,t),e.include(Rt,t),e.include(ct,t),e.include(tt,t),t.hasColorTexture&&e.fragment.uniforms.add("tex","sampler2D"),e.vertex.uniforms.add("viewNormal","mat4"),e.varyings.add("vPositionView","vec3"),o.add(oe`
      void main(void) {
        vpos = calculateVPos();
        vpos = subtractOrigin(vpos);
        ${0===t.normalType?oe`
        vNormalWorld = dpNormalView(vvLocalNormal(normalModel()));`:""}
        vpos = addVerticalOffset(vpos, localOrigin);
        gl_Position = transformPosition(proj, view, vpos);
        forwardTextureCoordinates();
      }
    `),e.include(ie,t),e.include(ne,t),a.add(oe`
      void main() {
        discardBySlice(vpos);
        ${t.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        discardOrAdjustAlpha(texColor);`:""}

        ${3===t.normalType?oe`
            vec3 normal = screenDerivativeNormal(vPositionView);`:oe`
            vec3 normal = normalize(vNormalWorld);
            if (gl_FrontFacing == false) normal = -normal;`}
        gl_FragColor = vec4(vec3(0.5) + 0.5 * normal, 1.0);
      }
    `)),4===t.output&&(e.include(ae,{linearDepth:!1}),e.include(ct,t),e.include(tt,t),t.hasColorTexture&&e.fragment.uniforms.add("tex","sampler2D"),o.add(oe`void main(void) {
vpos = calculateVPos();
vpos = subtractOrigin(vpos);
vpos = addVerticalOffset(vpos, localOrigin);
gl_Position = transformPosition(proj, view, vpos);
forwardTextureCoordinates();
}`),e.include(ie,t),e.include(ne,t),e.include(se),a.add(oe`
      void main() {
        discardBySlice(vpos);
        ${t.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        discardOrAdjustAlpha(texColor);`:""}
        outputHighlight();
      }
    `))}function Lt(e,t){const o=e.fragment;t.vertexTangents?(e.attributes.add("tangent","vec4"),e.varyings.add("vTangent","vec4"),2===t.doubleSidedMode?o.code.add(oe`mat3 computeTangentSpace(vec3 normal) {
float tangentHeadedness = gl_FrontFacing ? vTangent.w : -vTangent.w;
vec3 tangent = normalize(gl_FrontFacing ? vTangent.xyz : -vTangent.xyz);
vec3 bitangent = cross(normal, tangent) * tangentHeadedness;
return mat3(tangent, bitangent, normal);
}`):o.code.add(oe`mat3 computeTangentSpace(vec3 normal) {
float tangentHeadedness = vTangent.w;
vec3 tangent = normalize(vTangent.xyz);
vec3 bitangent = cross(normal, tangent) * tangentHeadedness;
return mat3(tangent, bitangent, normal);
}`)):(e.extensions.add("GL_OES_standard_derivatives"),o.code.add(oe`mat3 computeTangentSpace(vec3 normal, vec3 pos, vec2 st) {
vec3 Q1 = dFdx(pos);
vec3 Q2 = dFdy(pos);
vec2 stx = dFdx(st);
vec2 sty = dFdy(st);
float det = stx.t * sty.s - sty.t * stx.s;
vec3 T = stx.t * Q2 - sty.t * Q1;
T = T - normal * dot(normal, T);
T *= inversesqrt(max(dot(T,T), 1.e-10));
vec3 B = sign(det) * cross(normal, T);
return mat3(T, B, normal);
}`)),0!==t.attributeTextureCoordinates&&(e.include(bt,t),o.uniforms.add("normalTexture","sampler2D"),o.uniforms.add("normalTextureSize","vec2"),o.code.add(oe`
    vec3 computeTextureNormal(mat3 tangentSpace, vec2 uv) {
      vtc.uv = uv;
      ${t.supportsTextureAtlas?"vtc.size = normalTextureSize;":""}
      vec3 rawNormal = textureLookup(normalTexture, vtc).rgb * 2.0 - 1.0;
      return tangentSpace * rawNormal;
    }
  `))}function Ft(e,t){const o=e.fragment;t.receiveAmbientOcclusion?(o.uniforms.add("ssaoTex","sampler2D"),o.uniforms.add("viewportPixelSz","vec4"),o.code.add(oe`float evaluateAmbientOcclusion() {
return 1.0 - texture2D(ssaoTex, (gl_FragCoord.xy - viewportPixelSz.xy) * viewportPixelSz.zw).a;
}
float evaluateAmbientOcclusionInverse() {
float ssao = texture2D(ssaoTex, (gl_FragCoord.xy - viewportPixelSz.xy) * viewportPixelSz.zw).a;
return viewportPixelSz.z < 0.0 ? 1.0 : ssao;
}`)):o.code.add(oe`float evaluateAmbientOcclusion() { return 0.0; }
float evaluateAmbientOcclusionInverse() { return 1.0; }`)}function Dt(e,t){const o=e.fragment;e.include(mt),e.include(Ft,t),0!==t.pbrMode&&e.include(ot,t),e.include(ut,t),t.receiveShadows&&e.include(at,t),o.uniforms.add("lightingGlobalFactor","float"),o.uniforms.add("ambientBoostFactor","float"),e.include(rt),o.code.add(oe`
    const float GAMMA_SRGB = 2.1;
    const float INV_GAMMA_SRGB = 0.4761904;
    ${0===t.pbrMode?"":"const vec3 GROUND_REFLECTANCE = vec3(0.2);"}
  `),o.code.add(oe`
    float additionalDirectedAmbientLight(vec3 vPosWorld) {
      float vndl = dot(${1===t.viewingMode?oe`normalize(vPosWorld)`:oe`vec3(0.0, 0.0, 1.0)`}, lightingMainDirection);
      return smoothstep(0.0, 1.0, clamp(vndl * 2.5, 0.0, 1.0));
    }
  `),o.code.add(oe`vec3 evaluateAdditionalLighting(float ambientOcclusion, vec3 vPosWorld) {
float additionalAmbientScale = additionalDirectedAmbientLight(vPosWorld);
return (1.0 - ambientOcclusion) * additionalAmbientScale * ambientBoostFactor * lightingGlobalFactor * lightingMainIntensity;
}`),0===t.pbrMode||4===t.pbrMode?o.code.add(oe`vec3 evaluateSceneLighting(vec3 normalWorld, vec3 albedo, float shadow, float ssao, vec3 additionalLight)
{
vec3 mainLighting = evaluateMainLighting(normalWorld, shadow);
vec3 ambientLighting = calculateAmbientIrradiance(normalWorld, ssao);
vec3 albedoLinear = pow(albedo, vec3(GAMMA_SRGB));
vec3 totalLight = mainLighting + ambientLighting + additionalLight;
totalLight = min(totalLight, vec3(PI));
vec3 outColor = vec3((albedoLinear / PI) * totalLight);
return pow(outColor, vec3(INV_GAMMA_SRGB));
}`):1!==t.pbrMode&&2!==t.pbrMode||(o.code.add(oe`const float fillLightIntensity = 0.25;
const float horizonLightDiffusion = 0.4;
const float additionalAmbientIrradianceFactor = 0.02;
vec3 evaluateSceneLightingPBR(vec3 normal, vec3 albedo, float shadow, float ssao, vec3 additionalLight, vec3 viewDir, vec3 normalGround, vec3 mrr, vec3 _emission, float additionalAmbientIrradiance)
{
vec3 viewDirection = -viewDir;
vec3 mainLightDirection = lightingMainDirection;
vec3 h = normalize(viewDirection + mainLightDirection);
PBRShadingInfo inputs;
inputs.NdotL = clamp(dot(normal, mainLightDirection), 0.001, 1.0);
inputs.NdotV = clamp(abs(dot(normal, viewDirection)), 0.001, 1.0);
inputs.NdotH = clamp(dot(normal, h), 0.0, 1.0);
inputs.VdotH = clamp(dot(viewDirection, h), 0.0, 1.0);
inputs.NdotNG = clamp(dot(normal, normalGround), -1.0, 1.0);
vec3 reflectedView = normalize(reflect(viewDirection, normal));
inputs.RdotNG = clamp(dot(reflectedView, normalGround), -1.0, 1.0);
inputs.albedoLinear = pow(albedo, vec3(GAMMA_SRGB));
inputs.ssao = ssao;
inputs.metalness = mrr[0];
inputs.roughness = clamp(mrr[1] * mrr[1], 0.001, 0.99);`),o.code.add(oe`inputs.f0 = (0.16 * mrr[2] * mrr[2]) * (1.0 - inputs.metalness) + inputs.albedoLinear * inputs.metalness;
inputs.f90 = vec3(clamp(dot(inputs.f0, vec3(50.0 * 0.33)), 0.0, 1.0));
inputs.diffuseColor = inputs.albedoLinear * (vec3(1.0) - inputs.f0) * (1.0 - inputs.metalness);`),o.code.add(oe`vec3 ambientDir = vec3(5.0 * normalGround[1] - normalGround[0] * normalGround[2], - 5.0 * normalGround[0] - normalGround[2] * normalGround[1], normalGround[1] * normalGround[1] + normalGround[0] * normalGround[0]);
ambientDir = ambientDir != vec3(0.0)? normalize(ambientDir) : normalize(vec3(5.0, -1.0, 0.0));
inputs.NdotAmbDir = abs(dot(normal, ambientDir));
vec3 mainLightIrradianceComponent = inputs.NdotL * (1.0 - shadow) * lightingMainIntensity;
vec3 fillLightsIrradianceComponent = inputs.NdotAmbDir * lightingMainIntensity * fillLightIntensity;
vec3 ambientLightIrradianceComponent = calculateAmbientIrradiance(normal, ssao) + additionalLight;
inputs.skyIrradianceToSurface = ambientLightIrradianceComponent + mainLightIrradianceComponent + fillLightsIrradianceComponent ;
inputs.groundIrradianceToSurface = GROUND_REFLECTANCE * ambientLightIrradianceComponent + mainLightIrradianceComponent + fillLightsIrradianceComponent ;`),o.code.add(oe`vec3 horizonRingDir = inputs.RdotNG * normalGround - reflectedView;
vec3 horizonRingH = normalize(viewDirection + horizonRingDir);
inputs.NdotH_Horizon = dot(normal, horizonRingH);
vec3 mainLightRadianceComponent = normalDistribution(inputs.NdotH, inputs.roughness) * lightingMainIntensity * (1.0 - shadow);
vec3 horizonLightRadianceComponent = normalDistribution(inputs.NdotH_Horizon, min(inputs.roughness + horizonLightDiffusion, 1.0)) * lightingMainIntensity * fillLightIntensity;
vec3 ambientLightRadianceComponent = calculateAmbientRadiance(ssao) + additionalLight;
inputs.skyRadianceToSurface = ambientLightRadianceComponent + mainLightRadianceComponent + horizonLightRadianceComponent;
inputs.groundRadianceToSurface = GROUND_REFLECTANCE * (ambientLightRadianceComponent + horizonLightRadianceComponent) + mainLightRadianceComponent;
inputs.averageAmbientRadiance = ambientLightIrradianceComponent[1] * (1.0 + GROUND_REFLECTANCE[1]);`),o.code.add(oe`
        vec3 reflectedColorComponent = evaluateEnvironmentIllumination(inputs);
        vec3 additionalMaterialReflectanceComponent = inputs.albedoLinear * additionalAmbientIrradiance;
        vec3 emissionComponent = pow(_emission, vec3(GAMMA_SRGB));
        vec3 outColorLinear = reflectedColorComponent + additionalMaterialReflectanceComponent + emissionComponent;
        ${2===t.pbrMode?oe`vec3 outColor = pow(max(vec3(0.0), outColorLinear - 0.005 * inputs.averageAmbientRadiance), vec3(INV_GAMMA_SRGB));`:oe`vec3 outColor = pow(blackLevelSoftCompression(outColorLinear, inputs), vec3(INV_GAMMA_SRGB));`}
        return outColor;
      }
    `))}function zt(e,t){const o=e.fragment;o.code.add(oe`struct ShadingNormalParameters {
vec3 normalView;
vec3 viewDirection;
} shadingParams;`),1===t.doubleSidedMode?o.code.add(oe`vec3 shadingNormal(ShadingNormalParameters params) {
return dot(params.normalView, params.viewDirection) > 0.0 ? normalize(-params.normalView) : normalize(params.normalView);
}`):2===t.doubleSidedMode?o.code.add(oe`vec3 shadingNormal(ShadingNormalParameters params) {
return gl_FrontFacing ? normalize(params.normalView) : normalize(-params.normalView);
}`):o.code.add(oe`vec3 shadingNormal(ShadingNormalParameters params) {
return normalize(params.normalView);
}`)}function Bt(e,t){const o=oe`
  /*
  *  ${t.name}
  *  ${0===t.output?"RenderOutput: Color":1===t.output?"RenderOutput: Depth":3===t.output?"RenderOutput: Shadow":2===t.output?"RenderOutput: Normal":4===t.output?"RenderOutput: Highlight":""}
  */
  `;dt()&&(e.fragment.code.add(o),e.vertex.code.add(o))}function Nt(e){e.include(le),e.code.add(oe`
    vec3 mixExternalColor(vec3 internalColor, vec3 textureColor, vec3 externalColor, int mode) {
      // workaround for artifacts in OSX using Intel Iris Pro
      // see: https://devtopia.esri.com/WebGIS/arcgis-js-api/issues/10475
      vec3 internalMixed = internalColor * textureColor;
      vec3 allMixed = internalMixed * externalColor;

      if (mode == ${oe.int(1)}) {
        return allMixed;
      }
      else if (mode == ${oe.int(2)}) {
        return internalMixed;
      }
      else if (mode == ${oe.int(3)}) {
        return externalColor;
      }
      else {
        // tint (or something invalid)
        float vIn = rgb2v(internalMixed);
        vec3 hsvTint = rgb2hsv(externalColor);
        vec3 hsvOut = vec3(hsvTint.x, hsvTint.y, vIn * hsvTint.z);
        return hsv2rgb(hsvOut);
      }
    }

    float mixExternalOpacity(float internalOpacity, float textureOpacity, float externalOpacity, int mode) {
      // workaround for artifacts in OSX using Intel Iris Pro
      // see: https://devtopia.esri.com/WebGIS/arcgis-js-api/issues/10475
      float internalMixed = internalOpacity * textureOpacity;
      float allMixed = internalMixed * externalOpacity;

      if (mode == ${oe.int(2)}) {
        return internalMixed;
      }
      else if (mode == ${oe.int(3)}) {
        return externalOpacity;
      }
      else {
        // multiply or tint (or something invalid)
        return allMixed;
      }
    }
  `)}!function(e){e.ModelTransform=class{constructor(){this.worldFromModel_RS=s(),this.worldFromModel_TH=c(),this.worldFromModel_TL=c()}};e.ViewProjectionTransform=class{constructor(){this.worldFromView_TH=c(),this.worldFromView_TL=c(),this.viewFromCameraRelative_RS=s(),this.projFromView=d()}},e.bindModelTransform=function(e,t){e.setUniformMatrix3fv("uTransform_WorldFromModel_RS",t.worldFromModel_RS),e.setUniform3fv("uTransform_WorldFromModel_TH",t.worldFromModel_TH),e.setUniform3fv("uTransform_WorldFromModel_TL",t.worldFromModel_TL)},e.bindViewProjTransform=function(e,t){e.setUniform3fv("uTransform_WorldFromView_TH",t.worldFromView_TH),e.setUniform3fv("uTransform_WorldFromView_TL",t.worldFromView_TL),e.setUniformMatrix4fv("uTransform_ProjFromView",t.projFromView),e.setUniformMatrix3fv("uTransform_ViewFromCameraRelative_RS",t.viewFromCameraRelative_RS)}}(Ot||(Ot={})),function(e){e.bindUniforms=function(e,t){e.setUniformMatrix4fv("viewNormal",t)}}(Rt||(Rt={}));const It=Object.freeze({__proto__:null,build:function(e){const t=new de,o=t.vertex.code,a=t.fragment.code;return t.include(Bt,{name:"Default Material Shader",output:e.output}),t.vertex.uniforms.add("proj","mat4").add("view","mat4").add("camPos","vec3").add("localOrigin","vec3"),t.include(At),t.varyings.add("vpos","vec3"),t.include(tt,e),t.include(ft,e),t.include(Ye,e),0!==e.output&&7!==e.output||(t.include(St,e),t.include(ae,{linearDepth:!1}),0===e.normalType&&e.offsetBackfaces&&t.include(Mt),t.include(Lt,e),t.include(Rt,e),e.instancedColor&&t.attributes.add("instanceColor","vec4"),t.varyings.add("localvpos","vec3"),t.include(ct,e),t.include(it,e),t.include(Pt,e),t.include(lt,e),t.vertex.uniforms.add("externalColor","vec4"),t.varyings.add("vcolorExt","vec4"),e.multipassTerrainEnabled&&t.varyings.add("depth","float"),o.add(oe`
      void main(void) {
        forwardNormalizedVertexColor();
        vcolorExt = externalColor;
        ${e.instancedColor?"vcolorExt *= instanceColor;":""}
        vcolorExt *= vvColor();
        vcolorExt *= getSymbolColor();
        forwardColorMixMode();

        if (vcolorExt.a < ${oe.float(ce)}) {
          gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
        }
        else {
          vpos = calculateVPos();
          localvpos = vpos - view[3].xyz;
          vpos = subtractOrigin(vpos);
          ${0===e.normalType?oe`
          vNormalWorld = dpNormal(vvLocalNormal(normalModel()));`:""}
          vpos = addVerticalOffset(vpos, localOrigin);
          ${e.vertexTangents?"vTangent = dpTransformVertexTangent(tangent);":""}
          gl_Position = transformPosition(proj, view, vpos);
          ${0===e.normalType&&e.offsetBackfaces?"gl_Position = offsetBackfacingClipPosition(gl_Position, vpos, vNormalWorld, camPos);":""}
        }

        ${e.multipassTerrainEnabled?"depth = (view * vec4(vpos, 1.0)).z;":""}
        forwardLinearDepth();
        forwardTextureCoordinates();
      }
    `)),7===e.output&&(t.include(ie,e),t.include(ne,e),e.multipassTerrainEnabled&&(t.fragment.include(ue),t.include(me,e)),t.fragment.uniforms.add("camPos","vec3").add("localOrigin","vec3").add("opacity","float").add("layerOpacity","float"),e.hasColorTexture&&t.fragment.uniforms.add("tex","sampler2D"),t.fragment.include(Nt),a.add(oe`
      void main() {
        discardBySlice(vpos);
        ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}
        ${e.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        ${e.textureAlphaPremultiplied?"texColor.rgb /= texColor.a;":""}
        discardOrAdjustAlpha(texColor);`:oe`vec4 texColor = vec4(1.0);`}
        ${e.attributeColor?oe`
        float opacity_ = layerOpacity * mixExternalOpacity(vColor.a * opacity, texColor.a, vcolorExt.a, int(colorMixMode));`:oe`
        float opacity_ = layerOpacity * mixExternalOpacity(opacity, texColor.a, vcolorExt.a, int(colorMixMode));
        `}
        gl_FragColor = vec4(opacity_);
      }
    `)),0===e.output&&(t.include(ie,e),t.include(Dt,e),t.include(Ft,e),t.include(ne,e),e.receiveShadows&&t.include(at,e),e.multipassTerrainEnabled&&(t.fragment.include(ue),t.include(me,e)),t.fragment.uniforms.add("camPos","vec3").add("localOrigin","vec3").add("ambient","vec3").add("diffuse","vec3").add("opacity","float").add("layerOpacity","float"),e.hasColorTexture&&t.fragment.uniforms.add("tex","sampler2D"),t.include(Ct,e),t.include(ot,e),t.fragment.include(Nt),t.include(zt,e),a.add(oe`
      void main() {
        discardBySlice(vpos);
        ${e.multipassTerrainEnabled?"terrainDepthTest(gl_FragCoord, depth);":""}
        ${e.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        ${e.textureAlphaPremultiplied?"texColor.rgb /= texColor.a;":""}
        discardOrAdjustAlpha(texColor);`:oe`vec4 texColor = vec4(1.0);`}
        shadingParams.viewDirection = normalize(vpos - camPos);
        ${3===e.normalType?oe`
        vec3 normal = screenDerivativeNormal(localvpos);`:oe`
        shadingParams.normalView = vNormalWorld;
        vec3 normal = shadingNormal(shadingParams);`}
        ${1===e.pbrMode?"applyPBRFactors();":""}
        float ssao = evaluateAmbientOcclusionInverse();
        ssao *= getBakedOcclusion();

        float additionalAmbientScale = additionalDirectedAmbientLight(vpos + localOrigin);
        vec3 additionalLight = ssao * lightingMainIntensity * additionalAmbientScale * ambientBoostFactor * lightingGlobalFactor;
        ${e.receiveShadows?"float shadow = readShadowMap(vpos, linearDepth);":1===e.viewingMode?"float shadow = lightingGlobalFactor * (1.0 - additionalAmbientScale);":"float shadow = 0.0;"}
        vec3 matColor = max(ambient, diffuse);
        ${e.attributeColor?oe`
        vec3 albedo_ = mixExternalColor(vColor.rgb * matColor, texColor.rgb, vcolorExt.rgb, int(colorMixMode));
        float opacity_ = layerOpacity * mixExternalOpacity(vColor.a * opacity, texColor.a, vcolorExt.a, int(colorMixMode));`:oe`
        vec3 albedo_ = mixExternalColor(matColor, texColor.rgb, vcolorExt.rgb, int(colorMixMode));
        float opacity_ = layerOpacity * mixExternalOpacity(opacity, texColor.a, vcolorExt.a, int(colorMixMode));
        `}
        ${e.hasNormalTexture?oe`
              mat3 tangentSpace = ${e.vertexTangents?"computeTangentSpace(normal);":"computeTangentSpace(normal, vpos, vuv0);"}
              vec3 shadedNormal = computeTextureNormal(tangentSpace, vuv0);`:"vec3 shadedNormal = normal;"}
        ${1===e.pbrMode||2===e.pbrMode?1===e.viewingMode?oe`vec3 normalGround = normalize(vpos + localOrigin);`:oe`vec3 normalGround = vec3(0.0, 0.0, 1.0);`:oe``}
        ${1===e.pbrMode||2===e.pbrMode?oe`
            float additionalAmbientIrradiance = additionalAmbientIrradianceFactor * lightingMainIntensity[2];
            vec3 shadedColor = evaluateSceneLightingPBR(shadedNormal, albedo_, shadow, 1.0 - ssao, additionalLight, shadingParams.viewDirection, normalGround, mrr, emission, additionalAmbientIrradiance);`:"vec3 shadedColor = evaluateSceneLighting(shadedNormal, albedo_, shadow, 1.0 - ssao, additionalLight);"}
        gl_FragColor = highlightSlice(vec4(shadedColor, opacity_), vpos);
        ${e.OITEnabled?"gl_FragColor = premultiplyAlpha(gl_FragColor);":""}
      }
    `)),t.include(Et,e),t}});class Vt extends he{initializeProgram(e){const t=Vt.shader.get(),o=this.configuration,a=t.build({OITEnabled:0===o.transparencyPassType,output:o.output,viewingMode:e.viewingMode,receiveShadows:o.receiveShadows,slicePlaneEnabled:o.slicePlaneEnabled,sliceHighlightDisabled:o.sliceHighlightDisabled,sliceEnabledForVertexPrograms:!1,symbolColor:o.symbolColors,vvSize:o.vvSize,vvColor:o.vvColor,vvInstancingEnabled:!0,instanced:o.instanced,instancedColor:o.instancedColor,instancedDoublePrecision:o.instancedDoublePrecision,pbrMode:o.usePBR?o.isSchematic?2:1:0,hasMetalnessAndRoughnessTexture:o.hasMetalnessAndRoughnessTexture,hasEmissionTexture:o.hasEmissionTexture,hasOcclusionTexture:o.hasOcclusionTexture,hasNormalTexture:o.hasNormalTexture,hasColorTexture:o.hasColorTexture,receiveAmbientOcclusion:o.receiveAmbientOcclusion,useCustomDTRExponentForWater:!1,normalType:o.normalsTypeDerivate?3:0,doubleSidedMode:o.doubleSidedMode,vertexTangents:o.vertexTangents,attributeTextureCoordinates:o.hasMetalnessAndRoughnessTexture||o.hasEmissionTexture||o.hasOcclusionTexture||o.hasNormalTexture||o.hasColorTexture?1:0,textureAlphaPremultiplied:o.textureAlphaPremultiplied,attributeColor:o.vertexColors,screenSizePerspectiveEnabled:o.screenSizePerspective,verticalOffsetEnabled:o.verticalOffset,offsetBackfaces:o.offsetBackfaces,doublePrecisionRequiresObfuscation:pt(e.rctx),alphaDiscardMode:o.alphaDiscardMode,supportsTextureAtlas:!1,multipassTerrainEnabled:o.multipassTerrainEnabled,cullAboveGround:o.cullAboveGround});return new ge(e.rctx,a,xe)}bindPass(e,t){var o,a;be(this.program,t.camera.projectionMatrix);const r=this.configuration.output;(1===this.configuration.output||t.multipassTerrainEnabled||3===r)&&this.program.setUniform2fv("cameraNearFar",t.camera.nearFar),t.multipassTerrainEnabled&&(this.program.setUniform2fv("inverseViewport",t.inverseViewport),ye(this.program,t)),7===r&&(this.program.setUniform1f("opacity",e.opacity),this.program.setUniform1f("layerOpacity",e.layerOpacity),this.program.setUniform4fv("externalColor",e.externalColor),this.program.setUniform1i("colorMixMode",Ce[e.colorMixMode])),0===r?(t.lighting.setUniforms(this.program,!1),this.program.setUniform3fv("ambient",e.ambient),this.program.setUniform3fv("diffuse",e.diffuse),this.program.setUniform4fv("externalColor",e.externalColor),this.program.setUniform1i("colorMixMode",Ce[e.colorMixMode]),this.program.setUniform1f("opacity",e.opacity),this.program.setUniform1f("layerOpacity",e.layerOpacity),this.configuration.usePBR&&Tt(this.program,e,this.configuration.isSchematic)):4===r&&Te(this.program,t),nt(this.program,e),Ze(this.program,e,t),Me(e.screenSizePerspective,this.program,"screenSizePerspectiveAlignment"),2!==e.textureAlphaMode&&3!==e.textureAlphaMode||this.program.setUniform1f("textureAlphaCutoff",e.textureAlphaCutoff),null==(o=t.shadowMap)||o.bind(this.program),null==(a=t.ssaoHelper)||a.bind(this.program,t.camera)}bindDraw(e){const t=this.configuration.instancedDoublePrecision?u(e.camera.viewInverseTransposeMatrix[3],e.camera.viewInverseTransposeMatrix[7],e.camera.viewInverseTransposeMatrix[11]):e.origin;we(this.program,t,e.camera.viewMatrix),this.program.rebindTextures(),(0===this.configuration.output||7===this.configuration.output||1===this.configuration.output&&this.configuration.screenSizePerspective||2===this.configuration.output&&this.configuration.screenSizePerspective||4===this.configuration.output&&this.configuration.screenSizePerspective)&&Se(this.program,t,e.camera.viewInverseTransposeMatrix),2===this.configuration.output&&this.program.setUniformMatrix4fv("viewNormal",e.camera.viewInverseTransposeMatrix),this.configuration.instancedDoublePrecision&&ft.bindCustomOrigin(this.program,t),Ae(this.program,this.configuration,e.slicePlane,t),0===this.configuration.output&&st(this.program,e,t)}setPipeline(e,t){const o=this.configuration,a=3===e,r=2===e;return Ge({blending:0!==o.output&&7!==o.output||!o.transparent?null:a?$e:He(e),culling:Gt(o)&&Ue(o.cullFace),depthTest:{func:We(e)},depthWrite:a||r?o.writeDepth&&je:null,colorWrite:ke,stencilWrite:o.sceneHasOcludees?_e:null,stencilTest:o.sceneHasOcludees?t?Pe:Oe:null,polygonOffset:a||r?null:qe(o.enableOffset)})}initializePipeline(){return this._occludeePipelineState=this.setPipeline(this.configuration.transparencyPassType,!0),this.setPipeline(this.configuration.transparencyPassType,!1)}getPipelineState(e,t){return t?this._occludeePipelineState:super.getPipelineState(e,t)}}function Gt(e){return e.cullFace?0!==e.cullFace:!e.slicePlaneEnabled&&(!e.transparent&&!e.doubleSidedMode)}Vt.shader=new pe(It,(()=>Promise.resolve().then((()=>It))));class $t extends fe{constructor(){super(...arguments),this.output=0,this.alphaDiscardMode=1,this.doubleSidedMode=0,this.isSchematic=!1,this.vertexColors=!1,this.offsetBackfaces=!1,this.symbolColors=!1,this.vvSize=!1,this.vvColor=!1,this.verticalOffset=!1,this.receiveShadows=!1,this.slicePlaneEnabled=!1,this.sliceHighlightDisabled=!1,this.receiveAmbientOcclusion=!1,this.screenSizePerspective=!1,this.textureAlphaPremultiplied=!1,this.hasColorTexture=!1,this.usePBR=!1,this.hasMetalnessAndRoughnessTexture=!1,this.hasEmissionTexture=!1,this.hasOcclusionTexture=!1,this.hasNormalTexture=!1,this.instanced=!1,this.instancedColor=!1,this.instancedDoublePrecision=!1,this.vertexTangents=!1,this.normalsTypeDerivate=!1,this.writeDepth=!0,this.sceneHasOcludees=!1,this.transparent=!1,this.enableOffset=!0,this.cullFace=0,this.transparencyPassType=3,this.multipassTerrainEnabled=!1,this.cullAboveGround=!1}}Ke([ve({count:8})],$t.prototype,"output",void 0),Ke([ve({count:4})],$t.prototype,"alphaDiscardMode",void 0),Ke([ve({count:3})],$t.prototype,"doubleSidedMode",void 0),Ke([ve()],$t.prototype,"isSchematic",void 0),Ke([ve()],$t.prototype,"vertexColors",void 0),Ke([ve()],$t.prototype,"offsetBackfaces",void 0),Ke([ve()],$t.prototype,"symbolColors",void 0),Ke([ve()],$t.prototype,"vvSize",void 0),Ke([ve()],$t.prototype,"vvColor",void 0),Ke([ve()],$t.prototype,"verticalOffset",void 0),Ke([ve()],$t.prototype,"receiveShadows",void 0),Ke([ve()],$t.prototype,"slicePlaneEnabled",void 0),Ke([ve()],$t.prototype,"sliceHighlightDisabled",void 0),Ke([ve()],$t.prototype,"receiveAmbientOcclusion",void 0),Ke([ve()],$t.prototype,"screenSizePerspective",void 0),Ke([ve()],$t.prototype,"textureAlphaPremultiplied",void 0),Ke([ve()],$t.prototype,"hasColorTexture",void 0),Ke([ve()],$t.prototype,"usePBR",void 0),Ke([ve()],$t.prototype,"hasMetalnessAndRoughnessTexture",void 0),Ke([ve()],$t.prototype,"hasEmissionTexture",void 0),Ke([ve()],$t.prototype,"hasOcclusionTexture",void 0),Ke([ve()],$t.prototype,"hasNormalTexture",void 0),Ke([ve()],$t.prototype,"instanced",void 0),Ke([ve()],$t.prototype,"instancedColor",void 0),Ke([ve()],$t.prototype,"instancedDoublePrecision",void 0),Ke([ve()],$t.prototype,"vertexTangents",void 0),Ke([ve()],$t.prototype,"normalsTypeDerivate",void 0),Ke([ve()],$t.prototype,"writeDepth",void 0),Ke([ve()],$t.prototype,"sceneHasOcludees",void 0),Ke([ve()],$t.prototype,"transparent",void 0),Ke([ve()],$t.prototype,"enableOffset",void 0),Ke([ve({count:3})],$t.prototype,"cullFace",void 0),Ke([ve({count:4})],$t.prototype,"transparencyPassType",void 0),Ke([ve()],$t.prototype,"multipassTerrainEnabled",void 0),Ke([ve()],$t.prototype,"cullAboveGround",void 0);const Ht=Object.freeze({__proto__:null,build:function(e){const t=new de,o=t.vertex.code,a=t.fragment.code;return t.vertex.uniforms.add("proj","mat4").add("view","mat4").add("camPos","vec3").add("localOrigin","vec3"),t.include(At),t.varyings.add("vpos","vec3"),t.include(tt,e),t.include(ft,e),t.include(Ye,e),0!==e.output&&7!==e.output||(t.include(St,e),t.include(ae,{linearDepth:!1}),e.offsetBackfaces&&t.include(Mt),e.instancedColor&&t.attributes.add("instanceColor","vec4"),t.varyings.add("vNormalWorld","vec3"),t.varyings.add("localvpos","vec3"),e.multipassTerrainEnabled&&t.varyings.add("depth","float"),t.include(ct,e),t.include(it,e),t.include(Pt,e),t.include(lt,e),t.vertex.uniforms.add("externalColor","vec4"),t.varyings.add("vcolorExt","vec4"),o.add(oe`
        void main(void) {
          forwardNormalizedVertexColor();
          vcolorExt = externalColor;
          ${e.instancedColor?"vcolorExt *= instanceColor;":""}
          vcolorExt *= vvColor();
          vcolorExt *= getSymbolColor();
          forwardColorMixMode();

          if (vcolorExt.a < ${oe.float(ce)}) {
            gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
          }
          else {
            vpos = calculateVPos();
            localvpos = vpos - view[3].xyz;
            vpos = subtractOrigin(vpos);
            vNormalWorld = dpNormal(vvLocalNormal(normalModel()));
            vpos = addVerticalOffset(vpos, localOrigin);
            gl_Position = transformPosition(proj, view, vpos);
            ${e.offsetBackfaces?"gl_Position = offsetBackfacingClipPosition(gl_Position, vpos, vNormalWorld, camPos);":""}
          }
          ${e.multipassTerrainEnabled?oe`depth = (view * vec4(vpos, 1.0)).z;`:""}
          forwardLinearDepth();
          forwardTextureCoordinates();
        }
      `)),7===e.output&&(t.include(ie,e),t.include(ne,e),e.multipassTerrainEnabled&&(t.fragment.include(ue),t.include(me,e)),t.fragment.uniforms.add("camPos","vec3").add("localOrigin","vec3").add("opacity","float").add("layerOpacity","float"),t.fragment.uniforms.add("view","mat4"),e.hasColorTexture&&t.fragment.uniforms.add("tex","sampler2D"),t.fragment.include(Nt),a.add(oe`
      void main() {
        discardBySlice(vpos);
        ${e.multipassTerrainEnabled?oe`terrainDepthTest(gl_FragCoord, depth);`:""}
        ${e.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        ${e.textureAlphaPremultiplied?"texColor.rgb /= texColor.a;":""}
        discardOrAdjustAlpha(texColor);`:oe`vec4 texColor = vec4(1.0);`}
        ${e.attributeColor?oe`
        float opacity_ = layerOpacity * mixExternalOpacity(vColor.a * opacity, texColor.a, vcolorExt.a, int(colorMixMode));`:oe`
        float opacity_ = layerOpacity * mixExternalOpacity(opacity, texColor.a, vcolorExt.a, int(colorMixMode));
        `}

        gl_FragColor = vec4(opacity_);
      }
    `)),0===e.output&&(t.include(ie,e),t.include(Dt,e),t.include(Ft,e),t.include(ne,e),e.receiveShadows&&t.include(at,e),e.multipassTerrainEnabled&&(t.fragment.include(ue),t.include(me,e)),t.fragment.uniforms.add("camPos","vec3").add("localOrigin","vec3").add("ambient","vec3").add("diffuse","vec3").add("opacity","float").add("layerOpacity","float"),t.fragment.uniforms.add("view","mat4"),e.hasColorTexture&&t.fragment.uniforms.add("tex","sampler2D"),t.include(Ct,e),t.include(ot,e),t.fragment.include(Nt),a.add(oe`
      void main() {
        discardBySlice(vpos);
        ${e.multipassTerrainEnabled?oe`terrainDepthTest(gl_FragCoord, depth);`:""}
        ${e.hasColorTexture?oe`
        vec4 texColor = texture2D(tex, vuv0);
        ${e.textureAlphaPremultiplied?"texColor.rgb /= texColor.a;":""}
        discardOrAdjustAlpha(texColor);`:oe`vec4 texColor = vec4(1.0);`}
        vec3 viewDirection = normalize(vpos - camPos);
        ${1===e.pbrMode?"applyPBRFactors();":""}
        float ssao = evaluateAmbientOcclusionInverse();
        ssao *= getBakedOcclusion();

        float additionalAmbientScale = additionalDirectedAmbientLight(vpos + localOrigin);
        vec3 additionalLight = ssao * lightingMainIntensity * additionalAmbientScale * ambientBoostFactor * lightingGlobalFactor;
        ${e.receiveShadows?"float shadow = readShadowMap(vpos, linearDepth);":1===e.viewingMode?"float shadow = lightingGlobalFactor * (1.0 - additionalAmbientScale);":"float shadow = 0.0;"}
        vec3 matColor = max(ambient, diffuse);
        ${e.attributeColor?oe`
        vec3 albedo_ = mixExternalColor(vColor.rgb * matColor, texColor.rgb, vcolorExt.rgb, int(colorMixMode));
        float opacity_ = layerOpacity * mixExternalOpacity(vColor.a * opacity, texColor.a, vcolorExt.a, int(colorMixMode));`:oe`
        vec3 albedo_ = mixExternalColor(matColor, texColor.rgb, vcolorExt.rgb, int(colorMixMode));
        float opacity_ = layerOpacity * mixExternalOpacity(opacity, texColor.a, vcolorExt.a, int(colorMixMode));
        `}
        ${oe`
        vec3 shadedNormal = normalize(vNormalWorld);
        albedo_ *= 1.2;
        vec3 viewForward = vec3(view[0][2], view[1][2], view[2][2]);
        float alignmentLightView = clamp(dot(viewForward, -lightingMainDirection), 0.0, 1.0);
        float transmittance = 1.0 - clamp(dot(viewForward, shadedNormal), 0.0, 1.0);
        float treeRadialFalloff = vColor.r;
        float backLightFactor = 0.5 * treeRadialFalloff * alignmentLightView * transmittance * (1.0 - shadow);
        additionalLight += backLightFactor * lightingMainIntensity;`}
        ${1===e.pbrMode||2===e.pbrMode?1===e.viewingMode?oe`vec3 normalGround = normalize(vpos + localOrigin);`:oe`vec3 normalGround = vec3(0.0, 0.0, 1.0);`:oe``}
        ${1===e.pbrMode||2===e.pbrMode?oe`
            float additionalAmbientIrradiance = additionalAmbientIrradianceFactor * lightingMainIntensity[2];
            vec3 shadedColor = evaluateSceneLightingPBR(shadedNormal, albedo_, shadow, 1.0 - ssao, additionalLight, viewDirection, normalGround, mrr, emission, additionalAmbientIrradiance);`:"vec3 shadedColor = evaluateSceneLighting(shadedNormal, albedo_, shadow, 1.0 - ssao, additionalLight);"}
        gl_FragColor = highlightSlice(vec4(shadedColor, opacity_), vpos);
        ${e.OITEnabled?"gl_FragColor = premultiplyAlpha(gl_FragColor);":""}
      }
    `)),t.include(Et,e),t}});class Ut extends Vt{initializeProgram(e){const t=Ut.shader.get(),o=this.configuration,a=t.build({OITEnabled:0===o.transparencyPassType,output:o.output,viewingMode:e.viewingMode,receiveShadows:o.receiveShadows,slicePlaneEnabled:o.slicePlaneEnabled,sliceHighlightDisabled:o.sliceHighlightDisabled,sliceEnabledForVertexPrograms:!1,symbolColor:o.symbolColors,vvSize:o.vvSize,vvColor:o.vvColor,vvInstancingEnabled:!0,instanced:o.instanced,instancedColor:o.instancedColor,instancedDoublePrecision:o.instancedDoublePrecision,pbrMode:o.usePBR?1:0,hasMetalnessAndRoughnessTexture:!1,hasEmissionTexture:!1,hasOcclusionTexture:!1,hasNormalTexture:!1,hasColorTexture:o.hasColorTexture,receiveAmbientOcclusion:o.receiveAmbientOcclusion,useCustomDTRExponentForWater:!1,normalType:0,doubleSidedMode:2,vertexTangents:!1,attributeTextureCoordinates:o.hasColorTexture?1:0,textureAlphaPremultiplied:o.textureAlphaPremultiplied,attributeColor:o.vertexColors,screenSizePerspectiveEnabled:o.screenSizePerspective,verticalOffsetEnabled:o.verticalOffset,offsetBackfaces:o.offsetBackfaces,doublePrecisionRequiresObfuscation:pt(e.rctx),alphaDiscardMode:o.alphaDiscardMode,supportsTextureAtlas:!1,multipassTerrainEnabled:o.multipassTerrainEnabled,cullAboveGround:o.cullAboveGround});return new ge(e.rctx,a,xe)}}Ut.shader=new pe(Ht,(()=>Promise.resolve().then((()=>Ht))));class Wt extends Re{constructor(e){super(e,kt),this.supportsEdges=!0,this.techniqueConfig=new $t,this.vertexBufferLayout=function(e){const t=e.textureId||e.normalTextureId||e.metallicRoughnessTextureId||e.emissiveTextureId||e.occlusionTextureId,o=Ve().vec3f("position").vec3f("normal");e.vertexTangents&&o.vec4f("tangent");t&&o.vec2f("uv0");e.vertexColors&&o.vec4u8("color");e.symbolColors&&o.vec4u8("symbolColor");return o}(this.parameters),this.instanceBufferLayout=e.instanced?Xt(this.parameters):null}isVisibleInPass(e){return 4!==e&&6!==e&&7!==e||this.parameters.castShadows}isVisible(){const e=this.parameters;if(!super.isVisible()||0===e.layerOpacity)return!1;const t=e.instanced,o=e.vertexColors,a=e.symbolColors,r=!!t&&t.indexOf("color")>-1,i=e.vvColorEnabled,n="replace"===e.colorMixMode,s=e.opacity>0,l=e.externalColor&&e.externalColor[3]>0;return o&&(r||i||a)?!!n||s:o?n?l:s:r||i||a?!!n||s:n?l:s}getTechniqueConfig(e,t){return this.techniqueConfig.output=e,this.techniqueConfig.hasNormalTexture=!!this.parameters.normalTextureId,this.techniqueConfig.hasColorTexture=!!this.parameters.textureId,this.techniqueConfig.vertexTangents=this.parameters.vertexTangents,this.techniqueConfig.instanced=!!this.parameters.instanced,this.techniqueConfig.instancedDoublePrecision=this.parameters.instancedDoublePrecision,this.techniqueConfig.vvSize=this.parameters.vvSizeEnabled,this.techniqueConfig.verticalOffset=null!==this.parameters.verticalOffset,this.techniqueConfig.screenSizePerspective=null!==this.parameters.screenSizePerspective,this.techniqueConfig.slicePlaneEnabled=this.parameters.slicePlaneEnabled,this.techniqueConfig.sliceHighlightDisabled=this.parameters.sliceHighlightDisabled,this.techniqueConfig.alphaDiscardMode=this.parameters.textureAlphaMode,this.techniqueConfig.normalsTypeDerivate="screenDerivative"===this.parameters.normals,this.techniqueConfig.transparent=this.parameters.transparent,this.techniqueConfig.writeDepth=this.parameters.writeDepth,this.techniqueConfig.sceneHasOcludees=this.parameters.sceneHasOcludees,this.techniqueConfig.cullFace=this.parameters.slicePlaneEnabled?0:this.parameters.cullFace,this.techniqueConfig.multipassTerrainEnabled=t.multipassTerrainEnabled,this.techniqueConfig.cullAboveGround=t.cullAboveGround,0!==e&&7!==e||(this.techniqueConfig.vertexColors=this.parameters.vertexColors,this.techniqueConfig.symbolColors=this.parameters.symbolColors,this.parameters.treeRendering?this.techniqueConfig.doubleSidedMode=2:this.techniqueConfig.doubleSidedMode=this.parameters.doubleSided&&"normal"===this.parameters.doubleSidedType?1:this.parameters.doubleSided&&"winding-order"===this.parameters.doubleSidedType?2:0,this.techniqueConfig.instancedColor=!!this.parameters.instanced&&this.parameters.instanced.indexOf("color")>-1,this.techniqueConfig.receiveShadows=this.parameters.receiveShadows&&this.parameters.shadowMappingEnabled,this.techniqueConfig.receiveAmbientOcclusion=!!t.ssaoEnabled&&this.parameters.receiveSSAO,this.techniqueConfig.vvColor=this.parameters.vvColorEnabled,this.techniqueConfig.textureAlphaPremultiplied=!!this.parameters.textureAlphaPremultiplied,this.techniqueConfig.usePBR=this.parameters.usePBR,this.techniqueConfig.hasMetalnessAndRoughnessTexture=!!this.parameters.metallicRoughnessTextureId,this.techniqueConfig.hasEmissionTexture=!!this.parameters.emissiveTextureId,this.techniqueConfig.hasOcclusionTexture=!!this.parameters.occlusionTextureId,this.techniqueConfig.offsetBackfaces=!(!this.parameters.transparent||!this.parameters.offsetTransparentBackfaces),this.techniqueConfig.isSchematic=this.parameters.usePBR&&this.parameters.isSchematic,this.techniqueConfig.transparencyPassType=t.transparencyPassType,this.techniqueConfig.enableOffset=t.camera.relativeElevation<Xe),this.techniqueConfig}intersect(e,t,o,a,r,i,n){if(null!==this.parameters.verticalOffset){const e=a.camera;m(eo,o[12],o[13],o[14]);let t=null;switch(a.viewingMode){case 1:t=p(Yt,eo);break;case 2:t=v(Yt,Jt)}let n=0;if(null!==this.parameters.verticalOffset){const o=f(to,eo,e.eye),a=h(o),r=g(o,o,1/a);let i=null;this.parameters.screenSizePerspective&&(i=x(t,r)),n+=Ee(e,a,this.parameters.verticalOffset,i,this.parameters.screenSizePerspective)}g(t,t,n),b(Zt,t,a.transform.inverseRotation),r=f(Qt,r,Zt),i=f(Kt,i,Zt)}Le(e,t,a,r,i,Qe(a.verticalOffset),n)}requiresSlot(e){return e===(this.parameters.transparent?this.parameters.writeDepth?4:7:2)||20===e}createGLMaterial(e){return 0===e.output||7===e.output||1===e.output||2===e.output||3===e.output||4===e.output?new jt(e):null}createBufferWriter(){return new qt(this.vertexBufferLayout,this.instanceBufferLayout)}}class jt extends Ne{constructor(e){super({...e,...e.material.parameters})}updateParameters(e){const t=this._material.parameters;return this.updateTexture(t.textureId),this.ensureTechnique(t.treeRendering?Ut:Vt,e)}_updateShadowState(e){e.shadowMappingEnabled!==this._material.parameters.shadowMappingEnabled&&this._material.setParameters({shadowMappingEnabled:e.shadowMappingEnabled})}_updateOccludeeState(e){e.hasOccludees!==this._material.parameters.sceneHasOcludees&&this._material.setParameters({sceneHasOcludees:e.hasOccludees})}beginSlot(e){return 0!==this._output&&7!==this._output||(this._updateShadowState(e),this._updateOccludeeState(e)),this.updateParameters(e)}bind(e,t){t.bindPass(this._material.parameters,e),this.bindTextures(t.program)}}const kt={textureId:void 0,initTextureTransparent:!1,isSchematic:!1,usePBR:!1,normalTextureId:void 0,vertexTangents:!1,occlusionTextureId:void 0,emissiveTextureId:void 0,metallicRoughnessTextureId:void 0,emissiveFactor:[0,0,0],mrrFactors:[0,1,.5],ambient:[.2,.2,.2],diffuse:[.8,.8,.8],externalColor:[1,1,1,1],colorMixMode:"multiply",opacity:1,layerOpacity:1,vertexColors:!1,symbolColors:!1,doubleSided:!1,doubleSidedType:"normal",cullFace:2,instanced:void 0,instancedDoublePrecision:!1,normals:"default",receiveSSAO:!0,receiveShadows:!0,castShadows:!0,shadowMappingEnabled:!1,verticalOffset:null,screenSizePerspective:null,slicePlaneEnabled:!1,sliceHighlightDisabled:!1,offsetTransparentBackfaces:!1,vvSizeEnabled:!1,vvSizeMinSize:[1,1,1],vvSizeMaxSize:[100,100,100],vvSizeOffset:[0,0,0],vvSizeFactor:[1,1,1],vvSizeValue:[1,1,1],vvColorEnabled:!1,vvColorValues:[0,0,0,0,0,0,0,0],vvColorColors:[1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0],vvSymbolAnchor:[0,0,0],vvSymbolRotationMatrix:s(),transparent:!1,writeDepth:!0,textureAlphaMode:0,textureAlphaCutoff:Fe,textureAlphaPremultiplied:!1,sceneHasOcludees:!1,...De};class qt{constructor(e,t){this.vertexBufferLayout=e,this.instanceBufferLayout=t}allocate(e){return this.vertexBufferLayout.createBuffer(e)}elementCount(e){return e.indices.get("position").length}write(e,t,o,a){ze(t,this.vertexBufferLayout,e.transformation,e.invTranspTransformation,o,a)}}function Xt(e){let t=Ve();return t=e.instancedDoublePrecision?t.vec3f("modelOriginHi").vec3f("modelOriginLo").mat3f("model").mat3f("modelNormal"):t.mat4f("model").mat4f("modelNormal"),e.instanced&&e.instanced.indexOf("color")>-1&&(t=t.vec4f("instanceColor")),e.instanced&&e.instanced.indexOf("featureAttribute")>-1&&(t=t.vec4f("instanceFeatureAttribute")),t}const Qt=c(),Kt=c(),Jt=u(0,0,1),Yt=c(),Zt=c(),eo=c(),to=c(),oo=Y.getLogger("esri.views.3d.layers.graphics.objectResourceUtils");async function ao(e,t){const r=await async function(e,t){const r=o(t)&&t.streamDataRequester;if(r)return async function(e,t,o){const a=await K(t.request(e,"json",o));if(!0===a.ok)return a.value;return Z(a.error),void ro(a.error.details.url)}(e,r,t);const i=await K(Q(e,a(t)));if(!0===i.ok)return i.value.data;return Z(i.error),void ro(i.error)}(e,t),i=await async function(e,t){const a=[];for(const r in e){const i=e[r],n=i.images[0].data;if(!n){oo.warn("Externally referenced texture data is not yet supported");continue}const s=i.encoding+";base64,"+n,l="/textureDefinitions/"+r,d="rgba"===i.channels?i.alphaChannelUsage||"transparency":"none",c={noUnpackFlip:!0,wrap:{s:10497,t:10497},preMultiplyAlpha:1!==so(d)},u=o(t)&&t.disableTextures?Promise.resolve(null):te(s,t);a.push(u.then((e=>({refId:l,image:e,params:c,alphaChannelUsage:d}))))}const r=await Promise.all(a),i={};for(const e of r)i[e.refId]=e;return i}(r.textureDefinitions,t);return{resource:r,textures:i}}function ro(e){throw new J("",`Request for object resource failed: ${e}`)}function io(e){const t=e.params,o=t.topology;let a=!0;switch(t.vertexAttributes||(oo.warn("Geometry must specify vertex attributes"),a=!1),t.topology){case"PerAttributeArray":break;case"Indexed":case null:case void 0:{const e=t.faces;if(e){if(t.vertexAttributes)for(const o in t.vertexAttributes){const t=e[o];t&&t.values?(null!=t.valueType&&"UInt32"!==t.valueType&&(oo.warn(`Unsupported indexed geometry indices type '${t.valueType}', only UInt32 is currently supported`),a=!1),null!=t.valuesPerElement&&1!==t.valuesPerElement&&(oo.warn(`Unsupported indexed geometry values per element '${t.valuesPerElement}', only 1 is currently supported`),a=!1)):(oo.warn(`Indexed geometry does not specify face indices for '${o}' attribute`),a=!1)}}else oo.warn("Indexed geometries must specify faces"),a=!1;break}default:oo.warn(`Unsupported topology '${o}'`),a=!1}e.params.material||(oo.warn("Geometry requires material"),a=!1);const r=e.params.vertexAttributes;for(const e in r){r[e].values||(oo.warn("Geometries with externally defined attributes are not yet supported"),a=!1)}return a}function no(e){const t=w();return e.forEach((e=>{const a=e.boundingInfo;o(a)&&(S(t,a.getBBMin()),S(t,a.getBBMax()))})),t}function so(e){switch(e){case"mask":return 2;case"maskAndTransparency":return 3;case"none":return 1;default:return 0}}function lo(e){const t=e.params;return{id:1,material:t.material,texture:t.texture,region:t.texture}}const co=new ee(1,2,"wosr");async function uo(t,a){const n=mo(e(t));if("wosr"===n.fileType){const e=await(a.cache?a.cache.loadWOSR(n.url,a):ao(n.url,a)),t=function(e,t){const a=[],r=[],i=[],n=[],s=e.resource,l=ee.parse(s.version||"1.0","wosr");co.validate(l);const d=s.model.name,c=s.model.geometries,u=s.materialDefinitions,m=e.textures;let v=0;const p=new Map;for(let e=0;e<c.length;e++){const s=c[e];if(!io(s))continue;const l=lo(s),d=s.params.vertexAttributes,f=[];for(const e in d){const t=d[e],o=t.values;f.push([e,{data:o,size:t.valuesPerElement,exclusive:!0}])}const h=[];if("PerAttributeArray"!==s.params.topology){const e=s.params.faces;for(const t in e)h.push([t,new Uint32Array(e[t].values)])}const g=m&&m[l.texture];if(g&&!p.has(l.texture)){const{image:e,params:t}=g,o=new Ie(e,t);n.push(o),p.set(l.texture,o)}const x=p.get(l.texture),b=x?x.id:void 0;let C=i[l.material]?i[l.material][l.texture]:null;if(!C){const e=u[l.material.substring(l.material.lastIndexOf("/")+1)].params;1===e.transparency&&(e.transparency=0);const a=g&&g.alphaChannelUsage,r=e.transparency>0||"transparency"===a||"maskAndTransparency"===a,n=g?so(g.alphaChannelUsage):void 0,s={ambient:y(e.diffuse),diffuse:y(e.diffuse),opacity:1-(e.transparency||0),transparent:r,textureAlphaMode:n,textureAlphaCutoff:.33,textureId:b,initTextureTransparent:!0,doubleSided:!0,cullFace:0,colorMixMode:e.externalColorMixMode||"tint",textureAlphaPremultiplied:!!g&&!!g.params.preMultiplyAlpha};o(t)&&t.materialParamsMixin&&Object.assign(s,t.materialParamsMixin),C=new Wt(s),i[l.material]||(i[l.material]={}),i[l.material][l.texture]=C}r.push(C);const T=new Be(f,h);v+=h.position?h.position.length:0,a.push(T)}return{name:d,stageResources:{textures:n,materials:r,geometries:a},pivotOffset:s.model.pivotOffset,boundingBox:no(a),numberOfVertices:v,lodThreshold:null}}(e,a);return{lods:[t],referenceBoundingBox:t.boundingBox,isEsriSymbolResource:!1,isWosr:!0,remove:e.remove}}const s=await(a.cache?a.cache.loadGLTF(n.url,a,a.usePBR):N(new I(a.streamDataRequester),n.url,a,a.usePBR)),u=r(s.model.meta,"ESRI_proxyEllipsoid");s.meta.isEsriSymbolResource&&o(u)&&-1!==s.meta.uri.indexOf("/RealisticTrees/")&&function(e,t){for(let o=0;o<e.model.lods.length;++o){const a=e.model.lods[o];e.customMeta.esriTreeRendering=!0;for(const r of a.parts){const a=r.attributes.normal;if(i(a))return;const n=r.attributes.position,s=n.count,u=c(),m=c(),v=c(),g=G(O,s),x=G(A,s),b=l(d(),r.transform);for(let i=0;i<s;i++){n.getVec(i,m),a.getVec(i,u),C(m,m,r.transform),f(v,m,t.center),T(v,v,t.radius);const s=v[2],l=h(v),d=Math.min(.45+.55*l*l,1);T(v,v,t.radius),C(v,v,b),p(v,v),o+1!==e.model.lods.length&&e.model.lods.length>1&&M(v,v,u,s>-1?.2:Math.min(-4*s-3.8,1)),x.setVec(i,v),g.set(i,0,255*d),g.set(i,1,255*d),g.set(i,2,255*d),g.set(i,3,255)}r.attributes.normal=x,r.attributes.color=g}}}(s,u);const m=s.meta.isEsriSymbolResource?{usePBR:a.usePBR,isSchematic:!1,treeRendering:s.customMeta.esriTreeRendering,mrrFactors:[0,1,.2]}:{usePBR:a.usePBR,isSchematic:!1,mrrFactors:[0,1,.5]},v={...a.materialParamsMixin,treeRendering:s.customMeta.esriTreeRendering};if(null!=n.specifiedLodIndex){const e=vo(s,m,v,n.specifiedLodIndex);let t=e[0].boundingBox;if(0!==n.specifiedLodIndex){t=vo(s,m,v,0)[0].boundingBox}return{lods:e,referenceBoundingBox:t,isEsriSymbolResource:s.meta.isEsriSymbolResource,isWosr:!1,remove:s.remove}}const g=vo(s,m,v);return{lods:g,referenceBoundingBox:g[0].boundingBox,isEsriSymbolResource:s.meta.isEsriSymbolResource,isWosr:!1,remove:s.remove}}function mo(e){const t=e.match(/(.*\.(gltf|glb))(\?lod=([0-9]+))?$/);if(t)return{fileType:"gltf",url:t[1],specifiedLodIndex:null!=t[4]?Number(t[4]):null};return e.match(/(.*\.(json|json\.gz))$/)?{fileType:"wosr",url:e,specifiedLodIndex:null}:{fileType:"unknown",url:e,specifiedLodIndex:null}}function vo(e,t,a,r){const i=e.model,l=s(),d=new Array,c=new Map,u=new Map;return i.lods.forEach(((e,s)=>{if(void 0!==r&&s!==r)return;const m={name:e.name,stageResources:{textures:new Array,materials:new Array,geometries:new Array},lodThreshold:o(e.lodThreshold)?e.lodThreshold:null,pivotOffset:[0,0,0],numberOfVertices:0,boundingBox:w()};d.push(m),e.parts.forEach((e=>{const r=e.material+(e.attributes.normal?"_normal":"")+(e.attributes.color?"_color":"")+(e.attributes.texCoord0?"_texCoord0":"")+(e.attributes.tangent?"_tangent":""),s=i.materials.get(e.material),d=o(e.attributes.texCoord0),v=o(e.attributes.normal),p=function(e){switch(e){case"BLEND":return 0;case"MASK":return 2;case"OPAQUE":case null:case void 0:return 1}}(s.alphaMode);if(!c.has(r)){if(d){if(o(s.textureColor)&&!u.has(s.textureColor)){const e=i.textures.get(s.textureColor),t={...e.parameters,preMultiplyAlpha:1!==p};u.set(s.textureColor,new Ie(e.data,t))}if(o(s.textureNormal)&&!u.has(s.textureNormal)){const e=i.textures.get(s.textureNormal);u.set(s.textureNormal,new Ie(e.data,e.parameters))}if(o(s.textureOcclusion)&&!u.has(s.textureOcclusion)){const e=i.textures.get(s.textureOcclusion);u.set(s.textureOcclusion,new Ie(e.data,e.parameters))}if(o(s.textureEmissive)&&!u.has(s.textureEmissive)){const e=i.textures.get(s.textureEmissive);u.set(s.textureEmissive,new Ie(e.data,e.parameters))}if(o(s.textureMetallicRoughness)&&!u.has(s.textureMetallicRoughness)){const e=i.textures.get(s.textureMetallicRoughness);u.set(s.textureMetallicRoughness,new Ie(e.data,e.parameters))}}const n=s.color[0]**(1/V),l=s.color[1]**(1/V),m=s.color[2]**(1/V),f=s.emissiveFactor[0]**(1/V),h=s.emissiveFactor[1]**(1/V),g=s.emissiveFactor[2]**(1/V),x=o(s.textureColor)&&d?u.get(s.textureColor):null;c.set(r,new Wt({...t,transparent:0===p,textureAlphaMode:p,textureAlphaCutoff:s.alphaCutoff,diffuse:[n,l,m],ambient:[n,l,m],opacity:s.opacity,doubleSided:s.doubleSided,doubleSidedType:"winding-order",cullFace:s.doubleSided?0:2,vertexColors:!!e.attributes.color,vertexTangents:!!e.attributes.tangent,normals:v?"default":"screenDerivative",castShadows:!0,receiveSSAO:!0,textureId:o(x)?x.id:void 0,colorMixMode:s.colorMixMode,normalTextureId:o(s.textureNormal)&&d?u.get(s.textureNormal).id:void 0,textureAlphaPremultiplied:o(x)&&!!x.params.preMultiplyAlpha,occlusionTextureId:o(s.textureOcclusion)&&d?u.get(s.textureOcclusion).id:void 0,emissiveTextureId:o(s.textureEmissive)&&d?u.get(s.textureEmissive).id:void 0,metallicRoughnessTextureId:o(s.textureMetallicRoughness)&&d?u.get(s.textureMetallicRoughness).id:void 0,emissiveFactor:[f,h,g],mrrFactors:[s.metallicFactor,s.roughnessFactor,t.mrrFactors[2]],isSchematic:!1,...a}))}const f=function(e,t){switch(t){case 4:return X(e);case 5:return q(e);case 6:return k(e)}}(e.indices||e.attributes.position.count,e.primitiveType),h=e.attributes.position.count,g=G(A,h);F(g,e.attributes.position,e.transform);const x=[["position",{data:g.typedBuffer,size:g.elementCount,exclusive:!0}]],b=[["position",f]];if(o(e.attributes.normal)){const t=G(A,h);n(l,e.transform),D(t,e.attributes.normal,l),x.push(["normal",{data:t.typedBuffer,size:t.elementCount,exclusive:!0}]),b.push(["normal",f])}if(o(e.attributes.tangent)){const t=G(_,h);n(l,e.transform),$(t,e.attributes.tangent,l),x.push(["tangent",{data:t.typedBuffer,size:t.elementCount,exclusive:!0}]),b.push(["tangent",f])}if(o(e.attributes.texCoord0)){const t=G(P,h);H(t,e.attributes.texCoord0),x.push(["uv0",{data:t.typedBuffer,size:t.elementCount,exclusive:!0}]),b.push(["uv0",f])}if(o(e.attributes.color)){const t=G(O,h);if(4===e.attributes.color.elementCount)e.attributes.color instanceof _?U(t,e.attributes.color,255):e.attributes.color instanceof O?W(t,e.attributes.color):e.attributes.color instanceof R&&U(t,e.attributes.color,1/256);else{j(t,255,255,255,255);const o=new E(t.buffer,0,4);e.attributes.color instanceof A?z(o,e.attributes.color,255):e.attributes.color instanceof E?B(o,e.attributes.color):e.attributes.color instanceof L&&z(o,e.attributes.color,1/256)}x.push(["color",{data:t.typedBuffer,size:t.elementCount,exclusive:!0}]),b.push(["color",f])}const y=new Be(x,b);m.stageResources.geometries.push(y),m.stageResources.materials.push(c.get(r)),d&&(o(s.textureColor)&&m.stageResources.textures.push(u.get(s.textureColor)),o(s.textureNormal)&&m.stageResources.textures.push(u.get(s.textureNormal)),o(s.textureOcclusion)&&m.stageResources.textures.push(u.get(s.textureOcclusion)),o(s.textureEmissive)&&m.stageResources.textures.push(u.get(s.textureEmissive)),o(s.textureMetallicRoughness)&&m.stageResources.textures.push(u.get(s.textureMetallicRoughness))),m.numberOfVertices+=h;const C=y.boundingInfo;o(C)&&(S(m.boundingBox,C.getBBMin()),S(m.boundingBox,C.getBBMax()))}))})),d}const po=Object.freeze({__proto__:null,fetch:uo,parseUrl:mo,gltfToEngineResources:vo});export{Lt as C,_t as D,mt as E,Bt as H,Nt as M,zt as N,Ct as P,ct as T,Ot as V,ut as a,Dt as b,Rt as c,bt as d,xt as e,Tt as f,pt as g,vt as h,Wt as i,Xt as j,uo as k,ao as l,At as m,Ft as n,yt as o,po as p};
