/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{e,j as t,n as r,s as o,f as i,m as a,l as s,E as n,t as l,d as c,c as d,g as p,b as u,k as f}from"./mathUtils.js";import{i as v}from"../core/lang.js";import{d as m}from"./screenUtils.js";import{f as g}from"./mat3.js";import{c as h}from"./quatf64.js";import{a as x}from"./mat4.js";import{c as b,I as y}from"./mat4f64.js";import{c as P}from"./vec2.js";import{c as S,f as C}from"./vec2f64.js";import{a as O}from"./aaBoundingRect.js";import{n as w}from"./InterleavedLayout.js";import{b as z}from"./geometryDataUtils.js";import{o as D,O as A,c as j}from"./ScreenSpacePass.js";import{G as T}from"./Texture2.js";import{g as E,R as F,f as U,S as V,q as M,C as H,z as R,H as G,r as q,p as B,a as $,b as _,c as I,P as N,D as k,h as L,a5 as W,L as Q,B as Y,Q as Z,U as J,V as K,a1 as X,ag as ee,i as te,ah as re,af as oe,a0 as ie,a6 as ae,a7 as se,ac as ne,a8 as le}from"./StencilUtils.js";import{d as ce}from"./Util2.js";import{i as de}from"./utils14.js";import{Z as pe}from"./vec4f64.js";import{S as ue,b as fe,a as ve}from"./VerticalOffset.glsl.js";import{V as me,g as ge}from"./PhysicallyBasedRendering.glsl.js";import{_ as he}from"./tslib.es6.js";import{a as xe,c as be,m as ye,O as Pe,d as Se}from"./OrderIndependentTransparency.js";import{projectPointToVector as Ce}from"../geometry/projection.js";import{r as Oe}from"./aaBoundingBox.js";import{d as we,a as ze}from"../geometry/Polygon.js";import{m as De}from"./dehydratedFeatures.js";import{u as Ae,L as je}from"./lineUtils.js";import{p as Te}from"./floatRGBA.js";function Ee(e){const t=E`vec4 alignToPixelCenter(vec4 clipCoord, vec2 widthHeight) {
vec2 xy = vec2(0.500123) + 0.5 * clipCoord.xy / clipCoord.w;
vec2 pixelSz = vec2(1.0) / widthHeight;
vec2 ij = (floor(xy * widthHeight) + vec2(0.5)) * pixelSz;
vec2 result = (ij * 2.0 - vec2(1.0)) * clipCoord.w;
return vec4(result, clipCoord.zw);
}`,r=E`vec4 alignToPixelOrigin(vec4 clipCoord, vec2 widthHeight) {
vec2 xy = vec2(0.5) + 0.5 * clipCoord.xy / clipCoord.w;
vec2 pixelSz = vec2(1.0) / widthHeight;
vec2 ij = floor((xy + 0.5 * pixelSz) * widthHeight) * pixelSz;
vec2 result = (ij * 2.0 - vec2(1.0)) * clipCoord.w;
return vec4(result, clipCoord.zw);
}`;e.vertex.code.add(t),e.vertex.code.add(r),e.fragment.code.add(t),e.fragment.code.add(r)}function Fe(e){return function(e){return e instanceof Float32Array&&e.length>=16}(e)||function(e){return Array.isArray(e)&&e.length>=16}(e)}function Ue(e,t){const r=e;r.include(ue),r.attributes.add("position","vec3"),r.attributes.add("normal","vec3"),r.attributes.add("auxpos1","vec4"),r.vertex.uniforms.add("proj","mat4"),r.vertex.uniforms.add("view","mat4"),r.vertex.uniforms.add("viewNormal","mat4"),r.vertex.uniforms.add("viewport","vec4"),r.vertex.uniforms.add("camPos","vec3"),r.vertex.uniforms.add("polygonOffset","float"),r.vertex.uniforms.add("cameraGroundRelative","float"),r.vertex.uniforms.add("pixelRatio","float"),r.vertex.uniforms.add("perDistancePixelRatio","float"),r.vertex.uniforms.add("uRenderTransparentlyOccludedHUD","float"),t.verticalOffsetEnabled&&r.vertex.uniforms.add("verticalOffset","vec4"),t.screenSizePerspectiveEnabled&&r.vertex.uniforms.add("screenSizePerspectiveAlignment","vec4"),r.vertex.uniforms.add("hudVisibilityTexture","sampler2D"),r.vertex.constants.add("smallOffsetAngle","float",.984807753012208),r.vertex.code.add(E`struct ProjectHUDAux {
vec3 posModel;
vec3 posView;
vec3 vnormal;
float distanceToCamera;
float absCosAngle;
};`),r.vertex.code.add(E`float applyHUDViewDependentPolygonOffset(float pointGroundDistance, float absCosAngle, inout vec3 posView) {
float pointGroundSign = sign(pointGroundDistance);
if (pointGroundSign == 0.0) {
pointGroundSign = cameraGroundRelative;
}
float groundRelative = cameraGroundRelative * pointGroundSign;
if (polygonOffset > .0) {
float cosAlpha = clamp(absCosAngle, 0.01, 1.0);
float tanAlpha = sqrt(1.0 - cosAlpha * cosAlpha) / cosAlpha;
float factor = (1.0 - tanAlpha / viewport[2]);
if (groundRelative > 0.0) {
posView *= factor;
}
else {
posView /= factor;
}
}
return groundRelative;
}`),t.isDraped||r.vertex.code.add(E`void applyHUDVerticalGroundOffset(vec3 normalModel, inout vec3 posModel, inout vec3 posView) {
float distanceToCamera = length(posView);
float pixelOffset = distanceToCamera * perDistancePixelRatio * 0.5;
vec3 modelOffset = normalModel * cameraGroundRelative * pixelOffset;
vec3 viewOffset = (viewNormal * vec4(modelOffset, 1.0)).xyz;
posModel += modelOffset;
posView += viewOffset;
}`),r.vertex.code.add(E`
    vec4 projectPositionHUD(out ProjectHUDAux aux) {
      // centerOffset is in view space and is used to implement world size offsetting
      // of labels with respect to objects. It also pulls the label towards the viewer
      // so that the label is visible in front of the object.
      vec3 centerOffset = auxpos1.xyz;

      // The pointGroundDistance is the distance of the geometry to the ground and is
      // negative if the point is below the ground, or positive if the point is above
      // ground.
      float pointGroundDistance = auxpos1.w;

      aux.posModel = position;
      aux.posView = (view * vec4(aux.posModel, 1.0)).xyz;
      aux.vnormal = normal;
      ${t.isDraped?"":"applyHUDVerticalGroundOffset(aux.vnormal, aux.posModel, aux.posView);"}

      // Screen sized offset in world space, used for example for line callouts
      // Note: keep this implementation in sync with the CPU implementation, see
      //   - MaterialUtil.verticalOffsetAtDistance
      //   - HUDMaterial.applyVerticalOffsetTransformation

      aux.distanceToCamera = length(aux.posView);

      vec3 viewDirObjSpace = normalize(camPos - aux.posModel);
      float cosAngle = dot(aux.vnormal, viewDirObjSpace);

      aux.absCosAngle = abs(cosAngle);

      ${t.screenSizePerspectiveEnabled&&(t.verticalOffsetEnabled||1===t.screenCenterOffsetUnitsEnabled)?"vec4 perspectiveFactor = screenSizePerspectiveScaleFactor(aux.absCosAngle, aux.distanceToCamera, screenSizePerspectiveAlignment);":""}

      ${t.verticalOffsetEnabled?t.screenSizePerspectiveEnabled?"float verticalOffsetScreenHeight = applyScreenSizePerspectiveScaleFactorFloat(verticalOffset.x, perspectiveFactor);":"float verticalOffsetScreenHeight = verticalOffset.x;":""}

      ${t.verticalOffsetEnabled?E`
            float worldOffset = clamp(verticalOffsetScreenHeight * verticalOffset.y * aux.distanceToCamera, verticalOffset.z, verticalOffset.w);
            vec3 modelOffset = aux.vnormal * worldOffset;
            aux.posModel += modelOffset;
            vec3 viewOffset = (viewNormal * vec4(modelOffset, 1.0)).xyz;
            aux.posView += viewOffset;
            // Since we elevate the object, we need to take that into account
            // in the distance to ground
            pointGroundDistance += worldOffset;`:""}

      float groundRelative = applyHUDViewDependentPolygonOffset(pointGroundDistance, aux.absCosAngle, aux.posView);

      ${1!==t.screenCenterOffsetUnitsEnabled?E`
            // Apply x/y in view space, but z in screen space (i.e. along posView direction)
            aux.posView += vec3(centerOffset.x, centerOffset.y, 0.0);

            // Same material all have same z != 0.0 condition so should not lead to
            // branch fragmentation and will save a normalization if it's not needed
            if (centerOffset.z != 0.0) {
              aux.posView -= normalize(aux.posView) * centerOffset.z;
            }
          `:""}

      vec4 posProj = proj * vec4(aux.posView, 1.0);

      ${1===t.screenCenterOffsetUnitsEnabled?t.screenSizePerspectiveEnabled?"float centerOffsetY = applyScreenSizePerspectiveScaleFactorFloat(centerOffset.y, perspectiveFactor);":"float centerOffsetY = centerOffset.y;":""}

      ${1===t.screenCenterOffsetUnitsEnabled?"posProj.xy += vec2(centerOffset.x, centerOffsetY) * pixelRatio * 2.0 / viewport.zw * posProj.w;":""}

      // constant part of polygon offset emulation
      posProj.z -= groundRelative * polygonOffset * posProj.w;
      return posProj;
    }
  `),r.vertex.code.add(E`bool testVisibilityHUD(vec4 posProj) {
vec4 posProjCenter = alignToPixelCenter(posProj, viewport.zw);
vec4 occlusionPixel = texture2D(hudVisibilityTexture, .5 + .5 * posProjCenter.xy / posProjCenter.w);
if (uRenderTransparentlyOccludedHUD > 0.5) {
return occlusionPixel.r * occlusionPixel.g > 0.0 && occlusionPixel.g * uRenderTransparentlyOccludedHUD < 1.0;
}
return occlusionPixel.r * occlusionPixel.g > 0.0 && occlusionPixel.g == 1.0;
}`)}function Ve(e,t){e.setUniform1f("uRenderTransparentlyOccludedHUD",0===t.renderTransparentlyOccludedHUD?1:1===t.renderTransparentlyOccludedHUD?0:.75)}function Me(e){e.include(F),e.uniforms.add("geometryDepthTexture","sampler2D"),e.uniforms.add("cameraNearFar","vec2"),e.code.add(E`bool geometryDepthTest(vec2 pos, float elementDepth) {
float geometryDepth = linearDepthFromTexture(geometryDepthTexture, pos, cameraNearFar);
return (elementDepth < (geometryDepth - 1.0));
}`)}function He(e,t){t.multipassGeometryEnabled&&t.geometryLinearDepthTexture&&e.bindTexture(t.geometryLinearDepthTexture,"geometryDepthTexture")}function Re(e,t){t.multipassGeometryEnabled&&e.vertex.include(Me),t.multipassTerrainEnabled&&e.varyings.add("depth","float"),e.vertex.code.add(E`
  void main(void) {
    vec4 posProjCenter;
    if (dot(position, position) > 0.0) {
      // Render single point to center of the pixel to avoid subpixel
      // filtering to affect the marker color
      ProjectHUDAux projectAux;
      vec4 posProj = projectPositionHUD(projectAux);
      posProjCenter = alignToPixelCenter(posProj, viewport.zw);

      ${t.multipassGeometryEnabled?E`
        // Don't draw vertices behind geometry
        if(geometryDepthTest(.5 + .5 * posProjCenter.xy / posProjCenter.w, projectAux.posView.z)){
          posProjCenter = vec4(1e038, 1e038, 1e038, 1.0);
        }`:""}

      ${t.multipassTerrainEnabled?"depth = projectAux.posView.z;":""}
      vec3 vpos = projectAux.posModel;
      if (rejectBySlice(vpos)) {
        // Project out of clip space
        posProjCenter = vec4(1e038, 1e038, 1e038, 1.0);
      }

    } else {
      // Project out of clip space
      posProjCenter = vec4(1e038, 1e038, 1e038, 1.0);
    }

    gl_Position = posProjCenter;
    gl_PointSize = 1.0;
  }
  `),t.multipassTerrainEnabled&&e.fragment.include(F),e.fragment.uniforms.add("terrainDepthTexture","sampler2D"),e.fragment.uniforms.add("cameraNearFar","vec2"),e.fragment.uniforms.add("inverseViewport","vec2"),e.fragment.include(U),e.fragment.code.add(E`
  void main() {
    gl_FragColor = vec4(1, 1, 1, 1);
    ${t.multipassTerrainEnabled?E`

          vec2 uv = gl_FragCoord.xy * inverseViewport;

          //Read the rgba data from the texture linear depth
          vec4 terrainDepthData = texture2D(terrainDepthTexture, uv);

          float terrainDepth = linearDepthFromFloat(rgba2float(terrainDepthData), cameraNearFar);

          //If HUD vertex is behind terrain and the terrain depth is not the initialize value (e.g. we are not looking at the sky)
          //Mark the HUD vertex as occluded by transparent terrain
          if(depth < terrainDepth && terrainDepthData != vec4(0,0,0,1)){
            gl_FragColor.g = 0.5;
          }`:""}
  }
  `)}function Ge(e,t,r){e.setUniform4fv("materialColor",t.color),t.textureIsSignedDistanceField&&(t.outlineColor[3]<=0||t.outlineSize<=0?(e.setUniform4fv("outlineColor",pe),e.setUniform1f("outlineSize",0)):(e.setUniform4fv("outlineColor",t.outlineColor),e.setUniform1f("outlineSize",t.outlineSize))),e.setUniform2f("screenOffset",2*t.screenOffset[0]*r,2*t.screenOffset[1]*r),e.setUniform2fv("anchorPos",qe(t))}function qe(e,t=Be){var r,o,i;return e.textureIsSignedDistanceField?(r=e.anchorPos,o=e.distanceFieldBoundingBox,(i=t)[0]=r[0]*(o[2]-o[0])+o[0],i[1]=r[1]*(o[3]-o[1])+o[1]):P(t,e.anchorPos),t}const Be=S(),$e=Object.freeze({__proto__:null,build:function(e){const t=new V,r=e.signedDistanceFieldEnabled;if(t.include(Ee),t.include(Ue,e),t.include(M,e),6===e.output)return t.include(Re,e),t;t.include(ue),t.fragment.include(U),t.fragment.include(H),t.include(me,e),t.varyings.add("vcolor","vec4"),t.varyings.add("vtc","vec2"),t.varyings.add("vsize","vec2"),e.binaryHighlightOcclusionEnabled&&t.varyings.add("voccluded","float"),t.vertex.uniforms.add("screenOffset","vec2").add("anchorPos","vec2").add("textureCoordinateScaleFactor","vec2").add("materialColor","vec4"),r&&t.vertex.uniforms.add("outlineColor","vec4"),e.screenSizePerspectiveEnabled&&t.vertex.uniforms.add("screenSizePerspective","vec4"),(e.debugDrawBorder||e.binaryHighlightOcclusionEnabled)&&t.varyings.add("debugBorderCoords","vec4"),t.attributes.add("uv0","vec2"),t.attributes.add("color","vec4"),t.attributes.add("size","vec2"),t.attributes.add("auxpos2","vec4"),t.vertex.code.add(E`
    void main(void) {
      ProjectHUDAux projectAux;
      vec4 posProj = projectPositionHUD(projectAux);

      if (rejectBySlice(projectAux.posModel)) {
        // Project outside of clip plane
        gl_Position = vec4(1e038, 1e038, 1e038, 1.0);
        return;
      }
      vec2 inputSize;
      ${e.screenSizePerspectiveEnabled?E`
      inputSize = screenSizePerspectiveScaleVec2(size, projectAux.absCosAngle, projectAux.distanceToCamera, screenSizePerspective);
      vec2 screenOffsetScaled = screenSizePerspectiveScaleVec2(screenOffset, projectAux.absCosAngle, projectAux.distanceToCamera, screenSizePerspectiveAlignment);
         `:E`
      inputSize = size;
      vec2 screenOffsetScaled = screenOffset;`}

      ${e.vvSize?"inputSize *= vvScale(auxpos2).xx;":""}

      vec2 combinedSize = inputSize * pixelRatio;
      vec4 quadOffset = vec4(0.0);

      ${e.occlusionTestEnabled||e.binaryHighlightOcclusionEnabled?"bool visible = testVisibilityHUD(posProj);":""}

      ${e.binaryHighlightOcclusionEnabled?"voccluded = visible ? 0.0 : 1.0;":""}
    `);const o=E`vec2 uv01 = floor(uv0);
vec2 uv = uv0 - uv01;
quadOffset.xy = ((uv01 - anchorPos) * 2.0 * combinedSize + screenOffsetScaled) / viewport.zw * posProj.w;`,i=e.pixelSnappingEnabled?r?E`posProj = alignToPixelOrigin(posProj, viewport.zw) + quadOffset;`:E`posProj += quadOffset;
if (inputSize.x == size.x) {
posProj = alignToPixelOrigin(posProj, viewport.zw);
}`:E`posProj += quadOffset;`;t.vertex.code.add(E`
      ${e.occlusionTestEnabled?"if (visible) {":""}
      ${o}
      ${e.vvColor?"vcolor = vvGetColor(auxpos2, vvColorValues, vvColorColors) * materialColor;":"vcolor = color / 255.0 * materialColor;"}

      bool alphaDiscard = vcolor.a < ${E.float(R)};
      ${r?`alphaDiscard = alphaDiscard && outlineColor.a < ${E.float(R)};`:""}
      if (alphaDiscard) {
        // "early discard" if both symbol color (= fill) and outline color (if applicable) are transparent
        gl_Position = vec4(1e38, 1e38, 1e38, 1.0);
        return;
      } else {
        ${i}
        gl_Position = posProj;
      }

      vtc = uv * textureCoordinateScaleFactor;

      ${e.debugDrawBorder?"debugBorderCoords = vec4(uv01, 1.5 / combinedSize);":""}
      vsize = inputSize;
      ${e.occlusionTestEnabled?E`} else { vtc = vec2(0.0);
        ${e.debugDrawBorder?"debugBorderCoords = vec4(0.5, 0.5, 1.5 / combinedSize);}":"}"}`:""}
    }
    `),t.fragment.uniforms.add("tex","sampler2D"),r&&(t.fragment.uniforms.add("outlineColor","vec4"),t.fragment.uniforms.add("outlineSize","float"));const a=e.debugDrawBorder?E`(isBorder > 0.0 ? 0.0 : ${E.float(G)})`:E.float(G),s=E`
    ${e.debugDrawBorder?E`
      float isBorder = float(any(lessThan(debugBorderCoords.xy, debugBorderCoords.zw)) || any(greaterThan(debugBorderCoords.xy, 1.0 - debugBorderCoords.zw)));`:""}

    ${r?E`
      vec4 fillPixelColor = vcolor;

      // Attempt to sample texel centers to avoid that thin cross outlines
      // disappear with large symbol sizes.
      // see: https://devtopia.esri.com/WebGIS/arcgis-js-api/issues/7058#issuecomment-603041
      const float txSize = 128.0;
      const float texelSize = 1.0 / txSize;
      // Calculate how much we have to add/subtract to/from each texel to reach the size of an onscreen pixel
      vec2 scaleFactor = (vsize - txSize) * texelSize;
      vec2 samplePos = vtc + (vec2(1.0, -1.0) * texelSize) * scaleFactor;

      // Get distance and map it into [-0.5, 0.5]
      float d = rgba2float(texture2D(tex, samplePos)) - 0.5;

      // Distance in output units (i.e. pixels)
      float dist = d * vsize.x;

      // Create smooth transition from the icon into its outline
      float fillAlphaFactor = clamp(0.5 - dist, 0.0, 1.0);
      fillPixelColor.a *= fillAlphaFactor;

      if (outlineSize > 0.25) {
        vec4 outlinePixelColor = outlineColor;
        float clampedOutlineSize = min(outlineSize, 0.5*vsize.x);

        // Create smooth transition around outline
        float outlineAlphaFactor = clamp(0.5 - (abs(dist) - 0.5*clampedOutlineSize), 0.0, 1.0);
        outlinePixelColor.a *= outlineAlphaFactor;

        if (
          outlineAlphaFactor + fillAlphaFactor < ${a} ||
          fillPixelColor.a + outlinePixelColor.a < ${E.float(R)}
        ) {
          discard;
        }

        // perform un-premultiplied over operator (see https://en.wikipedia.org/wiki/Alpha_compositing#Description)
        float compositeAlpha = outlinePixelColor.a + fillPixelColor.a * (1.0 - outlinePixelColor.a);
        vec3 compositeColor = vec3(outlinePixelColor) * outlinePixelColor.a +
          vec3(fillPixelColor) * fillPixelColor.a * (1.0 - outlinePixelColor.a);

        gl_FragColor = vec4(compositeColor, compositeAlpha);
      } else {
        if (fillAlphaFactor < ${a}) {
          discard;
        }

        gl_FragColor = premultiplyAlpha(fillPixelColor);
      }

      // visualize SDF:
      // gl_FragColor = vec4(clamp(-dist/vsize.x*2.0, 0.0, 1.0), clamp(dist/vsize.x*2.0, 0.0, 1.0), 0.0, 1.0);
      `:E`
          vec4 texColor = texture2D(tex, vtc, -0.5);
          if (texColor.a < ${a}) {
            discard;
          }
          gl_FragColor = texColor * premultiplyAlpha(vcolor);
          `}

    ${e.debugDrawBorder?E`gl_FragColor = mix(gl_FragColor, vec4(1.0, 0.0, 1.0, 1.0), isBorder);`:""}
  `;return 7===e.output&&t.fragment.code.add(E`
      void main() {
        ${s}
        gl_FragColor = vec4(gl_FragColor.a);
      }
      `),0===e.output&&t.fragment.code.add(E`
    void main() {
      ${s}
      ${e.FrontFacePass?"gl_FragColor.rgb /= gl_FragColor.a;":""}
    }
    `),4===e.output&&(t.include(q),t.fragment.code.add(E`
    void main() {
      ${s}
      ${e.binaryHighlightOcclusionEnabled?E`
          if (voccluded == 1.0) {
            gl_FragColor = vec4(1.0, 1.0, 0.0, 1.0);
          } else {
            gl_FragColor = vec4(1.0, 0.0, 1.0, 1.0);
          }`:"outputHighlight();"}
    }
    `)),t},bindHUDMaterialUniforms:Ge,calculateAnchorPosForRendering:qe});class _e extends I{initializeProgram(e){const t=_e.shader.get(),r=this.configuration,o=t.build({output:r.output,FrontFacePass:2===r.transparencyPassType,viewingMode:e.viewingMode,occlusionTestEnabled:r.occlusionTestEnabled,signedDistanceFieldEnabled:r.sdf,slicePlaneEnabled:r.slicePlaneEnabled,sliceHighlightDisabled:!1,sliceEnabledForVertexPrograms:!0,debugDrawBorder:r.debugDrawBorder,binaryHighlightOcclusionEnabled:r.binaryHighlightOcclusion,screenCenterOffsetUnitsEnabled:r.screenCenterOffsetUnitsEnabled,screenSizePerspectiveEnabled:r.screenSizePerspective,verticalOffsetEnabled:r.verticalOffset,pixelSnappingEnabled:r.pixelSnappingEnabled,vvSize:r.vvSize,vvColor:r.vvColor,vvInstancingEnabled:!1,isDraped:r.isDraped,multipassGeometryEnabled:r.multipassGeometryEnabled,multipassTerrainEnabled:r.multipassTerrainEnabled,cullAboveGround:r.cullAboveGround});return new N(e.rctx,o,k)}bindPass(e,t){L(this.program,t.camera.projectionMatrix),this.program.setUniform1f("cameraGroundRelative",t.camera.aboveGround?1:-1),this.program.setUniform1f("perDistancePixelRatio",Math.tan(t.camera.fovY/2)/(t.camera.fullViewport[2]/2)),this.program.setUniformMatrix4fv("viewNormal",t.camera.viewInverseTransposeMatrix),this.program.setUniform1f("polygonOffset",e.shaderPolygonOffset),fe(this.program,e,t),ve(this.program,e),this.program.setUniform1f("pixelRatio",t.camera.pixelRatio||1),W(this.program,t),6===this.configuration.output?(this.program.setUniform2fv("cameraNearFar",t.camera.nearFar),this.program.setUniform2fv("inverseViewport",t.inverseViewport),He(this.program,t),Q(this.program,t)):(Ve(this.program,t),Ge(this.program,e,t.camera.pixelRatio||1),ge(this.program,e),this.configuration.occlusionTestEnabled&&this.program.bindTexture(t.hudVisibilityTexture,"hudVisibilityTexture")),4===this.configuration.output&&Y(this.program,t)}bindDraw(e){Z(this.program,e),J(this.program,e.origin,e.camera.viewInverseTransposeMatrix),K(this.program,this.configuration,e),this.program.rebindTextures()}setPipelineState(e){const t=this.configuration,r=3===e,o=2===e,i=this.configuration.polygonOffsetEnabled&&Ie,a=(r||o)&&4!==t.output?(t.depthEnabled||6===t.output)&&be:null;return ye({blending:0===t.output||7===t.output||4===t.output?r?Ne:Pe(e):null,depthTest:{func:515},depthWrite:a,colorWrite:Se,polygonOffset:i})}initializePipeline(){return this.setPipelineState(this.configuration.transparencyPassType)}get primitiveType(){return 6===this.configuration.output?0:4}}_e.shader=new $($e,(()=>Promise.resolve().then((()=>$e))));const Ie={factor:0,units:-4},Ne=xe(1,771);class ke extends _{constructor(){super(...arguments),this.output=0,this.occlusionTestEnabled=!0,this.sdf=!1,this.vvSize=!1,this.vvColor=!1,this.verticalOffset=!1,this.screenSizePerspective=!1,this.screenCenterOffsetUnitsEnabled=0,this.debugDrawBorder=!1,this.binaryHighlightOcclusion=!0,this.slicePlaneEnabled=!1,this.polygonOffsetEnabled=!1,this.depthEnabled=!0,this.transparencyPassType=3,this.pixelSnappingEnabled=!0,this.isDraped=!1,this.multipassGeometryEnabled=!1,this.multipassTerrainEnabled=!1,this.cullAboveGround=!1}}he([B({count:8})],ke.prototype,"output",void 0),he([B()],ke.prototype,"occlusionTestEnabled",void 0),he([B()],ke.prototype,"sdf",void 0),he([B()],ke.prototype,"vvSize",void 0),he([B()],ke.prototype,"vvColor",void 0),he([B()],ke.prototype,"verticalOffset",void 0),he([B()],ke.prototype,"screenSizePerspective",void 0),he([B({count:2})],ke.prototype,"screenCenterOffsetUnitsEnabled",void 0),he([B()],ke.prototype,"debugDrawBorder",void 0),he([B()],ke.prototype,"binaryHighlightOcclusion",void 0),he([B()],ke.prototype,"slicePlaneEnabled",void 0),he([B()],ke.prototype,"polygonOffsetEnabled",void 0),he([B()],ke.prototype,"depthEnabled",void 0),he([B({count:4})],ke.prototype,"transparencyPassType",void 0),he([B()],ke.prototype,"pixelSnappingEnabled",void 0),he([B()],ke.prototype,"isDraped",void 0),he([B()],ke.prototype,"multipassGeometryEnabled",void 0),he([B()],ke.prototype,"multipassTerrainEnabled",void 0),he([B()],ke.prototype,"cullAboveGround",void 0);class Le extends X{constructor(e){super(e,ft),this.techniqueConfig=new ke}getTechniqueConfig(e,t){return this.techniqueConfig.output=e,this.techniqueConfig.slicePlaneEnabled=this.parameters.slicePlaneEnabled,this.techniqueConfig.verticalOffset=!!this.parameters.verticalOffset,this.techniqueConfig.screenSizePerspective=!!this.parameters.screenSizePerspective,this.techniqueConfig.screenCenterOffsetUnitsEnabled="screen"===this.parameters.centerOffsetUnits?1:0,this.techniqueConfig.polygonOffsetEnabled=this.parameters.polygonOffset,this.techniqueConfig.isDraped=this.parameters.isDraped,this.techniqueConfig.occlusionTestEnabled=this.parameters.occlusionTest,this.techniqueConfig.pixelSnappingEnabled=this.parameters.pixelSnappingEnabled,this.techniqueConfig.sdf=this.parameters.textureIsSignedDistanceField,this.techniqueConfig.vvSize=!!this.parameters.vvSizeEnabled,this.techniqueConfig.vvColor=!!this.parameters.vvColorEnabled,0===e&&(this.techniqueConfig.debugDrawBorder=!!this.parameters.debugDrawBorder),4===e&&(this.techniqueConfig.binaryHighlightOcclusion=this.parameters.binaryHighlightOcclusion),this.techniqueConfig.depthEnabled=this.parameters.depthEnabled,this.techniqueConfig.transparencyPassType=t.transparencyPassType,this.techniqueConfig.multipassGeometryEnabled=t.multipassGeometryEnabled,this.techniqueConfig.multipassTerrainEnabled=t.multipassTerrainEnabled,this.techniqueConfig.cullAboveGround=t.cullAboveGround,this.techniqueConfig}intersect(e,t,r,o,i,a,s,n,l){v(l)?this.intersectDrapedHudGeometry(e,a,s,n,l):this.intersectHudGeometry(e,t,r,o,s,n)}intersectDrapedHudGeometry(e,t,r,o,i){const a=e.vertexAttributes.get("position"),s=e.vertexAttributes.get("size"),n=this.parameters,l=qe(n);let c=1,d=1;if(v(o)){const e=o(lt);c=e[0],d=e[5]}c*=e.screenToWorldRatio,d*=e.screenToWorldRatio;const p=dt*e.screenToWorldRatio;for(let o=0;o<a.data.length/a.size;o++){const u=o*a.size,f=a.data[u],v=a.data[u+1],m=o*s.size;let g;pt[0]=s.data[m]*c,pt[1]=s.data[m+1]*d,n.textureIsSignedDistanceField&&(g=n.outlineSize*e.screenToWorldRatio/2),Ye(t,f,v,pt,p,g,n,l)&&r(i.dist,i.normal,-1,!0)}}intersectHudGeometry(l,c,d,p,f,m){if(!p.options.selectionMode||!p.options.hud||de(c))return;const h=this.parameters;let b=1,y=1;if(g(ot,d),v(m)){const e=m(lt);b=e[0],y=e[5],function(e){const t=e[0],r=e[1],o=e[2],i=e[3],a=e[4],s=e[5],n=e[6],l=e[7],c=e[8],d=1/Math.sqrt(t*t+r*r+o*o),p=1/Math.sqrt(i*i+a*a+s*s),u=1/Math.sqrt(n*n+l*l+c*c);e[0]=t*d,e[1]=r*d,e[2]=o*d,e[3]=i*p,e[4]=a*p,e[5]=s*p,e[6]=n*u,e[7]=l*u,e[8]=c*u}(ot)}const P=l.vertexAttributes.get("position"),S=l.vertexAttributes.get("size"),C=l.vertexAttributes.get("normal"),O=l.vertexAttributes.get("auxpos1");ce(P.size>=3);const w=p.point,z=p.camera,D=qe(h);b*=z.pixelRatio,y*=z.pixelRatio;const A="screen"===this.parameters.centerOffsetUnits;for(let l=0;l<P.data.length/P.size;l++){const c=l*P.size;e(Ke,P.data[c],P.data[c+1],P.data[c+2]),t(Ke,Ke,d);const v=l*S.size;pt[0]=S.data[v]*b,pt[1]=S.data[v+1]*y,t(Ke,Ke,z.viewMatrix);const m=l*O.size;if(e(st,O.data[m+0],O.data[m+1],O.data[m+2]),!A&&(Ke[0]+=st[0],Ke[1]+=st[1],0!==st[2])){const e=st[2];r(st,Ke),o(Ke,Ke,i(st,st,e))}const g=l*C.size;if(e(Xe,C.data[g],C.data[g+1],C.data[g+2]),this.normalAndViewAngle(Xe,ot,z,nt),this.applyVerticalOffsetTransformationView(Ke,nt,z,Ze),z.applyProjection(Ke,et),et[0]>-1){let e=Math.floor(et[0])+this.parameters.screenOffset[0],r=Math.floor(et[1])+this.parameters.screenOffset[1];A&&(e+=st[0],0!==st[1]&&(r+=ee(st[1],Ze.factorAlignment))),te(pt,Ze.factor,pt);const o=ct*z.pixelRatio;let l;if(h.textureIsSignedDistanceField&&(l=h.outlineSize*z.pixelRatio/2),Ye(w,e,r,pt,o,l,h,D)){const e=p.ray;if(t(rt,Ke,x(at,z.viewMatrix)),et[0]=w[0],et[1]=w[1],z.unprojectFromRenderScreen(et,Ke)){const t=u();a(t,e.direction);const r=1/s(t);i(t,t,r);f(n(e.origin,Ke)*r,t,-1,!0,1,rt)}}}}}computeAttachmentOrigin(e,t){const r=e.vertexAttributes;if(!r)return!1;const o=r.get("position"),i=e.indices.get("position");return z(o,i,t)}createBufferWriter(){return new mt(this)}normalAndViewAngle(e,r,o,i){return Fe(r)&&(r=g(it,r)),l(i.normal,e,r),t(i.normal,i.normal,o.viewInverseTransposeMatrix),i.cosAngle=c(tt,ut),i}updateScaleInfo(e,t,r){const o=this.parameters;o.screenSizePerspective?re(r,t,o.screenSizePerspective,e.factor):(e.factor.scale=1,e.factor.factor=0,e.factor.minPixelSize=0,e.factor.paddingPixels=0),o.screenSizePerspectiveAlignment?re(r,t,o.screenSizePerspectiveAlignment,e.factorAlignment):(e.factorAlignment.factor=e.factor.factor,e.factorAlignment.scale=e.factor.scale,e.factorAlignment.minPixelSize=e.factor.minPixelSize,e.factorAlignment.paddingPixels=e.factor.paddingPixels)}applyShaderOffsetsView(e,t,r,o,i,a,s){const n=this.normalAndViewAngle(t,r,i,nt);return this.applyVerticalGroundOffsetView(e,n,i,s),this.applyVerticalOffsetTransformationView(s,n,i,a),this.applyPolygonOffsetView(s,n,o[3],i,s),this.applyCenterOffsetView(s,o,s),s}applyShaderOffsetsNDC(e,t,r,o,i){return this.applyCenterOffsetNDC(e,t,r,o),v(i)&&a(i,o),this.applyPolygonOffsetNDC(o,t,r,o),o}applyPolygonOffsetView(e,t,r,o,s){const n=o.aboveGround?1:-1;let l=Math.sign(r);0===l&&(l=n);const c=n*l;if(this.parameters.shaderPolygonOffset<=0)return a(s,e);const p=d(Math.abs(t.cosAngle),.01,1),u=1-Math.sqrt(1-p*p)/p/o.viewport[2];return i(s,e,c>0?u:1/u),s}applyVerticalGroundOffsetView(e,t,r,o){const a=s(e),n=r.aboveGround?1:-1,l=.5*r.computeRenderPixelSizeAtDist(a),c=i(Ke,t.normal,n*l);return p(o,e,c),o}applyVerticalOffsetTransformationView(e,t,r,o){const a=this.parameters;if(!a.verticalOffset||!a.verticalOffset.screenLength){if(a.screenSizePerspective||a.screenSizePerspectiveAlignment){const r=s(e);this.updateScaleInfo(o,r,t.cosAngle)}else o.factor.scale=1,o.factorAlignment.scale=1;return e}const n=s(e),l=a.screenSizePerspectiveAlignment||a.screenSizePerspective,c=oe(r,n,a.verticalOffset,t.cosAngle,l);return this.updateScaleInfo(o,n,t.cosAngle),i(t.normal,t.normal,c),p(e,e,t.normal)}applyCenterOffsetView(e,t,o){const s="screen"!==this.parameters.centerOffsetUnits;return o!==e&&a(o,e),s&&(o[0]+=t[0],o[1]+=t[1],t[2]&&(r(Xe,o),p(o,o,i(Xe,Xe,t[2])))),o}applyCenterOffsetNDC(e,t,r,o){const i="screen"!==this.parameters.centerOffsetUnits;return o!==e&&a(o,e),i||(o[0]+=t[0]/r.fullWidth*2,o[1]+=t[1]/r.fullHeight*2),o}applyPolygonOffsetNDC(e,t,r,o){const i=this.parameters.shaderPolygonOffset;if(e!==o&&a(o,e),i){const e=r.aboveGround?1:-1,a=e*Math.sign(t[3]);o[2]-=(a||e)*i}return o}requiresSlot(e,t){const r=D(t);if(0===r||7===r){if(20===e)return!0;const t=this.parameters.drawInSecondSlot?17:16;return this.parameters.occlusionTest&&11===e||e===t}return e===(this.parameters.drawInSecondSlot?17:16)||20===e}createGLMaterial(e){return 0===e.output||7===e.output?new Qe(e):4===e.output?new We(e):null}calculateRelativeScreenBounds(e,t,r=O()){return function(e,t,r,o=Je){P(o,e.anchorPos),o[0]*=-t[0],o[1]*=-t[1],o[0]+=e.screenOffset[0]*r,o[1]+=e.screenOffset[1]*r}(this.parameters,e,t,r),r[2]=r[0]+e[0],r[3]=r[1]+e[1],r}}class We extends T{constructor(e){super({...e,...e.material.parameters})}updateParameters(e){return this.updateTexture(this._material.parameters.textureId),this.selectProgram(e)}selectProgram(e){return this.ensureTechnique(_e,e)}beginSlot(e){return this.updateParameters(e)}bind(e,t){this.bindTextures(t.program),this.bindTextureScale(t.program),t.bindPass(this._material.parameters,e)}}class Qe extends We{isOcclusionSlot(e){return 11===e.slot&&this._material.parameters.occlusionTest&&(0===this._output||7===this._output)}selectProgram(e){return this.ensureTechnique(_e,e,this.isOcclusionSlot(e)?6:this._output)}bind(e,t){this.isOcclusionSlot(e)||(this.bindTextures(t.program),this.bindTextureScale(t.program)),t.bindPass(this._material.parameters,e)}}function Ye(e,t,r,o,i,a,s,n){let l=t-i-(n[0]>0?o[0]*n[0]:0),c=l+o[0]+2*i,d=r-i-(n[1]>0?o[1]*n[1]:0),p=d+o[1]+2*i;if(s.textureIsSignedDistanceField){const e=s.distanceFieldBoundingBox;l+=o[0]*e[0],d+=o[1]*e[1],c-=o[0]*(1-e[2]),p-=o[1]*(1-e[3]),l-=a,c+=a,d-=a,p+=a}return e[0]>l&&e[0]<c&&e[1]>d&&e[1]<p}const Ze={factor:{scale:0,factor:0,minPixelSize:0,paddingPixels:0},factorAlignment:{scale:0,factor:0,minPixelSize:0,paddingPixels:0}},Je=S(),Ke=u(),Xe=u(),et=m(),tt=u(),rt=u(),ot=h(),it=h(),at=b(),st=u(),nt={normal:tt,cosAngle:0},lt=b(),ct=1,dt=2,pt=[0,0],ut=f(0,0,1),ft={texCoordScale:[1,1],occlusionTest:!0,binaryHighlightOcclusion:!0,drawInSecondSlot:!1,color:[1,1,1,1],outlineColor:[1,1,1,1],outlineSize:0,textureIsSignedDistanceField:!1,distanceFieldBoundingBox:null,vvSizeEnabled:!1,vvSizeMinSize:[1,1,1],vvSizeMaxSize:[100,100,100],vvSizeOffset:[0,0,0],vvSizeFactor:[1,1,1],vvColorEnabled:!1,vvColorValues:[0,0,0,0,0,0,0,0],vvColorColors:[1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0],screenOffset:[0,0],verticalOffset:null,screenSizePerspective:null,screenSizePerspectiveAlignment:null,slicePlaneEnabled:!1,anchorPos:C(.5,.5),shaderPolygonOffset:1e-5,polygonOffset:!1,textureId:null,centerOffsetUnits:"world",depthEnabled:!0,pixelSnappingEnabled:!0,debugDrawBorder:!1,isDraped:!1,...ie},vt=w().vec3f("position").vec3f("normal").vec2f("uv0").vec4u8("color").vec2f("size").vec4f("auxpos1").vec4f("auxpos2");class mt{constructor(e){this.material=e,this.vertexBufferLayout=vt}allocate(e){return this.vertexBufferLayout.createBuffer(e)}elementCount(e){return 6*e.indices.get("position").length}write(e,t,r,o){ae(t.indices.get("position"),t.vertexAttributes.get("position").data,e.transformation,r.position,o,6),se(t.indices.get("normal"),t.vertexAttributes.get("normal").data,e.invTranspTransformation,r.normal,o,6);{const e=t.vertexAttributes.get("uv0").data;let i,a,s,n;if(null==e||e.length<4){const e=this.material.parameters;i=0,a=0,s=e.texCoordScale[0],n=e.texCoordScale[1]}else i=e[0],a=e[1],s=e[2],n=e[3];s=Math.min(1.99999,s+1),n=Math.min(1.99999,n+1);const l=t.indices.get("position").length,c=r.uv0;let d=o;for(let e=0;e<l;++e)c.set(d,0,i),c.set(d,1,a),d+=1,c.set(d,0,s),c.set(d,1,a),d+=1,c.set(d,0,s),c.set(d,1,n),d+=1,c.set(d,0,s),c.set(d,1,n),d+=1,c.set(d,0,i),c.set(d,1,n),d+=1,c.set(d,0,i),c.set(d,1,a),d+=1}ne(t.indices.get("color"),t.vertexAttributes.get("color").data,4,r.color,o,6);{const e=t.indices.get("size"),i=t.vertexAttributes.get("size").data,a=e.length,s=r.size;let n=o;for(let t=0;t<a;++t){const r=i[2*e[t]],o=i[2*e[t]+1];for(let e=0;e<6;++e)s.set(n,0,r),s.set(n,1,o),n+=1}}t.indices.get("auxpos1")&&t.vertexAttributes.get("auxpos1")&&le(t.indices.get("auxpos1"),t.vertexAttributes.get("auxpos1").data,r.auxpos1,o,6),t.indices.get("auxpos2")&&t.vertexAttributes.get("auxpos2")&&le(t.indices.get("auxpos2"),t.vertexAttributes.get("auxpos2").data,r.auxpos2,o,6)}}const gt=u();function ht(e,t,r,o,i,a,s,n){const l=r?r.length:0,c=e.clippingExtent;if(Ce(t,gt,e.elevationProvider.spatialReference),v(c)&&!Oe(c,gt))return null;const d=e.renderCoordsHelper.spatialReference;Ce(t,gt,d);const p=e.localOriginFactory.getOrigin(gt),u=new A({castShadow:!1,metadata:{layerUid:a,graphicUid:s,usesVerticalDistanceToGround:!0}});for(let e=0;e<l;e++){const t=y;u.addGeometry(r[e],o[e],t,p,n)}return{object:u,sampledElevation:je(u,t,e.elevationProvider,e.renderCoordsHelper,i)}}function xt(e,t,r){const o=e.elevationContext,i=r.spatialReference;Ce(t,gt,i),o.centerPointInElevationSR=De(gt[0],gt[1],t.hasZ?gt[2]:0,i)}function bt(e){switch(e.type){case"point":return e;case"polygon":case"extent":return j(e);case"polyline":{const t=e.paths[0];if(!t||0===t.length)return null;const r=we(t,ze(t)/2);return De(r[0],r[1],r[2],e.spatialReference)}case"mesh":return e.origin}return null}function yt(e,t,r,o,i){const a=new Float64Array(3*e.length),s=new Float64Array(a.length);e.forEach(((e,t)=>{a[3*t+0]=e[0],a[3*t+1]=e[1],a[3*t+2]=e.length>2?e[2]:0}));const n=Ae(a,t,0,s,0,a,0,a.length/3,r,o,i),l=null!=n;return{numVertices:e.length,position:a,mapPosition:s,projectionSuccess:l,sampledElevation:n}}function Pt(e,t,r){switch(e){case"circle":default:return St(t,r);case"square":return Ct(t,r);case"cross":return wt(t,r);case"x":return zt(t,r);case"kite":return Ot(t,r);case"triangle":return Dt(t,r)}}function St(e,t){const r=e,o=new Uint8Array(4*r*r),i=r/2-.5,a=t/2;for(let t=0;t<r;t++)for(let s=0;s<r;s++){const n=s+r*t,l=s-i,c=t-i;let d=Math.sqrt(l*l+c*c)-a;d=d/e+.5,Te(d,o,4*n)}return o}function Ct(e,t){return At(e,t,!1)}function Ot(e,t){return At(e,t,!0)}function wt(e,t){return jt(e,t,!1)}function zt(e,t){return jt(e,t,!0)}function Dt(e,t){const r=new Uint8Array(4*e*e),o=-.5,i=Math.sqrt(1.25),a=(e-t)/2;for(let s=0;s<e;s++)for(let n=0;n<e;n++){const l=s*e+n,c=(n-a)/t,d=(s-a+.75)/t,p=-(1*c+o*d)/i,u=(1*(c-1)+o*-d)/i,f=-d,v=Math.max(p,u,f);Te(v*t/e+.5,r,4*l)}return r}function At(e,t,r){r&&(t/=Math.SQRT2);const o=new Uint8Array(4*e*e);for(let i=0;i<e;i++)for(let a=0;a<e;a++){let s=a-.5*e+.25,n=.5*e-i-.75;const l=i*e+a;if(r){const e=(s+n)/Math.SQRT2;n=(n-s)/Math.SQRT2,s=e}let c=Math.max(Math.abs(s),Math.abs(n))-.5*t;c=c/e+.5,Te(c,o,4*l)}return o}function jt(e,t,r){r&&(t*=Math.SQRT2);const o=.5*t,i=new Uint8Array(4*e*e);for(let t=0;t<e;t++)for(let a=0;a<e;a++){let s=a-.5*e,n=.5*e-t-1;const l=t*e+a;if(r){const e=(s+n)/Math.SQRT2;n=(n-s)/Math.SQRT2,s=e}let c;s=Math.abs(s),n=Math.abs(n),c=s>n?s>o?Math.sqrt((s-o)*(s-o)+n*n):n:n>o?Math.sqrt(s*s+(n-o)*(n-o)):s,c=c/e+.5,Te(c,i,4*l)}return i}export{Ee as A,Le as H,Ue as a,Ve as b,He as c,ht as d,xt as e,Dt as f,zt as g,wt as h,Ot as i,Ct as j,St as k,Pt as l,Me as m,yt as n,bt as p};
