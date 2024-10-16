// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../shaderModules/interfaces"],function(d,b){d.TerrainTexture=function(a,c){a.varyings.add("vtc","vec2");a.vertex.uniforms.add("texOffsetAndScale","vec4");a.fragment.uniforms.add("tex","sampler2D");a.fragment.uniforms.add("textureOpacities","vec3");c.textureFadingEnabled&&(a.vertex.uniforms.add("nextTexOffsetAndScale","vec4"),a.varyings.add("nvtc","vec2"),a.fragment.uniforms.add("texNext","sampler2D"),a.fragment.uniforms.add("nextTexOpacities","vec3"),a.fragment.uniforms.add("fadeFactor",
"float"));a.vertex.code.add(b.glsl`
  void forwardTextureCoordinates(in vec2 uv) {
    vtc = uv * texOffsetAndScale.zw + texOffsetAndScale.xy;
    ${c.textureFadingEnabled?b.glsl`nvtc = uv * nextTexOffsetAndScale.zw + nextTexOffsetAndScale.xy;`:b.glsl``}
  }`);c.useGrid?(a.fragment.code.add(b.glsl`float lineFactorAtPosition(float value) {
float pos = value * 129.0;
if(pos < 0.5 || pos > 128.5) {
return 1.0;
}
pos = pos + 0.5;
float modulo = mod(pos, 16.0);
return modulo <= 2.0 ?  1.0 - abs(modulo - 1.0) : 0.0;
}
float lineFactorAtUV(vec2 uv) {
return max(lineFactorAtPosition(uv.x), lineFactorAtPosition(uv.y));
}
float lineFactor(vec2 uv) {
vec2 offset = fwidth(uv) * 0.25;
return (lineFactorAtUV(vec2(uv.x + offset.x, uv.y + offset.y)) +
lineFactorAtUV(vec2(uv.x - offset.x, uv.y + offset.y)) +
lineFactorAtUV(vec2(uv.x + offset.x, uv.y - offset.y)) +
lineFactorAtUV(vec2(uv.x - offset.x, uv.y - offset.y))) / 4.0;
}
vec4 gridColor(vec2 uv) {
float line = lineFactor(uv) * 0.1 + 0.9;
float backgroundOpacity = textureOpacities.y;
return vec4(1.0, 0.972, 0.918, 1.0) * line * backgroundOpacity;
}`),a.fragment.code.add(b.glsl`vec4 getColor(vec4 color, vec2 uv, vec3 opacities) {
if (opacities.y <= 0.0) {
return color * opacities.z * opacities.x;
}
vec4 grid = gridColor(uv);
float alpha = opacities.z * color.a;
return mix(grid, color, alpha) * opacities.x;
}`)):c.hasBackgroundColor?(a.fragment.uniforms.add("backgroundColor","vec3"),a.fragment.code.add(b.glsl`vec4 getColor(vec4 color, vec2 uv, vec3 opacities) {
if (opacities.y <= 0.0) {
return color * opacities.z * opacities.x;
}
float alpha = opacities.z * color.a;
float backgroundOpacity = opacities.y;
return mix(vec4(backgroundColor, 1.0) * backgroundOpacity, color, alpha) * opacities.x;
}`)):a.fragment.code.add(b.glsl`vec4 getColor(vec4 color, vec2 uv, vec3 opacities) {
return color;
}`);c.textureFadingEnabled?a.fragment.code.add(b.glsl`vec4 getTileColor() {
vec4 color = getColor(texture2D(tex, vtc), vtc, textureOpacities);
if (fadeFactor >= 1.0) {
return color;
}
vec4 nextColor = getColor(texture2D(texNext, nvtc), nvtc, nextTexOpacities);
return mix(nextColor, color, fadeFactor);
}`):a.fragment.code.add(b.glsl`vec4 getTileColor() {
return getColor(texture2D(tex, vtc), vtc, textureOpacities);
}`)};Object.defineProperty(d,"__esModule",{value:!0})});