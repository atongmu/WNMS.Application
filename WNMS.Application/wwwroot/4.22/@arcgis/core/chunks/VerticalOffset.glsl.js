/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{g as e,t as r}from"./StencilUtils.js";function a(r){r.vertex.code.add(e`float screenSizePerspectiveMinSize(float size, vec4 factor) {
float nonZeroSize = 1.0 - step(size, 0.0);
return (
factor.z * (
1.0 +
nonZeroSize *
2.0 * factor.w / (
size + (1.0 - nonZeroSize)
)
)
);
}`),r.vertex.code.add(e`float screenSizePerspectiveViewAngleDependentFactor(float absCosAngle) {
return absCosAngle * absCosAngle * absCosAngle;
}`),r.vertex.code.add(e`vec4 screenSizePerspectiveScaleFactor(float absCosAngle, float distanceToCamera, vec4 params) {
return vec4(
min(params.x / (distanceToCamera - params.y), 1.0),
screenSizePerspectiveViewAngleDependentFactor(absCosAngle),
params.z,
params.w
);
}`),r.vertex.code.add(e`float applyScreenSizePerspectiveScaleFactorFloat(float size, vec4 factor) {
return max(mix(size * factor.x, size, factor.y), screenSizePerspectiveMinSize(size, factor));
}`),r.vertex.code.add(e`float screenSizePerspectiveScaleFloat(float size, float absCosAngle, float distanceToCamera, vec4 params) {
return applyScreenSizePerspectiveScaleFactorFloat(
size,
screenSizePerspectiveScaleFactor(absCosAngle, distanceToCamera, params)
);
}`),r.vertex.code.add(e`vec2 applyScreenSizePerspectiveScaleFactorVec2(vec2 size, vec4 factor) {
return mix(size * clamp(factor.x, screenSizePerspectiveMinSize(size.y, factor) / size.y, 1.0), size, factor.y);
}`),r.vertex.code.add(e`vec2 screenSizePerspectiveScaleVec2(vec2 size, float absCosAngle, float distanceToCamera, vec4 params) {
return applyScreenSizePerspectiveScaleFactorVec2(size, screenSizePerspectiveScaleFactor(absCosAngle, distanceToCamera, params));
}`)}function t(e,a){if(a.screenSizePerspective){r(a.screenSizePerspective,e,"screenSizePerspective");const t=a.screenSizePerspectiveAlignment||a.screenSizePerspective;r(t,e,"screenSizePerspectiveAlignment")}}function c(r,t){const c=r.vertex.code;t.verticalOffsetEnabled?(r.vertex.uniforms.add("verticalOffset","vec4"),t.screenSizePerspectiveEnabled&&(r.include(a),r.vertex.uniforms.add("screenSizePerspectiveAlignment","vec4")),c.add(e`
    vec3 calculateVerticalOffset(vec3 worldPos, vec3 localOrigin) {
      float viewDistance = length((view * vec4(worldPos, 1.0)).xyz);
      ${1===t.viewingMode?e`vec3 worldNormal = normalize(worldPos + localOrigin);`:e`vec3 worldNormal = vec3(0.0, 0.0, 1.0);`}
      ${t.screenSizePerspectiveEnabled?e`
          float cosAngle = dot(worldNormal, normalize(worldPos - camPos));
          float verticalOffsetScreenHeight = screenSizePerspectiveScaleFloat(verticalOffset.x, abs(cosAngle), viewDistance, screenSizePerspectiveAlignment);`:e`
          float verticalOffsetScreenHeight = verticalOffset.x;`}
      // Screen sized offset in world space, used for example for line callouts
      float worldOffset = clamp(verticalOffsetScreenHeight * verticalOffset.y * viewDistance, verticalOffset.z, verticalOffset.w);
      return worldNormal * worldOffset;
    }

    vec3 addVerticalOffset(vec3 worldPos, vec3 localOrigin) {
      return worldPos + calculateVerticalOffset(worldPos, localOrigin);
    }
    `)):c.add(e`vec3 addVerticalOffset(vec3 worldPos, vec3 localOrigin) { return worldPos; }`)}function s(e,r,a){if(!r.verticalOffset)return;const t=function(e,r,a,t=i){return t.screenLength=e.screenLength,t.perDistance=Math.tan(.5*r)/(.5*a),t.minWorldLength=e.minWorldLength,t.maxWorldLength=e.maxWorldLength,t}(r.verticalOffset,a.camera.fovY,a.camera.fullViewport[3]),c=a.camera.pixelRatio||1;e.setUniform4f("verticalOffset",t.screenLength*c,t.perDistance,t.minWorldLength,t.maxWorldLength)}const i={screenLength:0,perDistance:0,minWorldLength:0,maxWorldLength:0};export{a as S,c as V,t as a,s as b};
