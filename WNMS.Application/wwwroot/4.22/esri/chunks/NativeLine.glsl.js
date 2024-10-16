// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../views/3d/webgl-engine/core/shaderLibrary/Slice.glsl ../views/3d/webgl-engine/core/shaderLibrary/Transform.glsl ../views/3d/webgl-engine/core/shaderLibrary/attributes/VertexColor.glsl ../views/3d/webgl-engine/core/shaderLibrary/output/OutputHighlight.glsl ../views/3d/webgl-engine/core/shaderLibrary/shading/LineStipple.glsl ../views/3d/webgl-engine/core/shaderLibrary/util/AlphaDiscard.glsl ../views/3d/webgl-engine/core/shaderModules/interfaces ../views/3d/webgl-engine/core/shaderModules/ShaderBuilder".split(" "),
function(d,f,g,h,k,l,m,b,n){function e(c){const a=new n.ShaderBuilder;a.include(g.Transform,{linearDepth:!1});a.include(h.VertexColor,c);a.include(l.LineStipple,{...c,stippleRequiresStretchMeasure:!1});a.vertex.uniforms.add("proj","mat4").add("view","mat4");c.stippleEnabled&&(a.vertex.uniforms.add("ndcToPixel","vec2"),a.attributes.add("uv0","vec2"),a.attributes.add("auxpos1","vec3"));a.attributes.add("position","vec3");a.varyings.add("vpos","vec3");a.vertex.code.add(b.glsl`void main(void) {
vpos = position;
forwardNormalizedVertexColor();
gl_Position = transformPosition(proj, view, vpos);`);c.stippleEnabled&&(a.vertex.code.add(b.glsl`vec4 vpos2 = transformPosition(proj, view, auxpos1);
float lineSegmentPixelSize = length((vpos2.xy / vpos2.w - gl_Position.xy / gl_Position.w) * ndcToPixel);`),c.draped||a.vertex.code.add(b.glsl`vec3 segmentCenter = (position + auxpos1) * 0.5;
float worldToScreenRatio = computeWorldToScreenRatio(segmentCenter);`),a.vertex.code.add(b.glsl`float discreteWorldToScreenRatio = discretizeWorldToScreenRatio(worldToScreenRatio);`),c.draped?a.vertex.code.add(b.glsl`float startPseudoScreen = uv0.y * discreteWorldToScreenRatio - mix(0.0, lineSegmentPixelSize, uv0.x);
float segmentLengthPseudoScreen = lineSegmentPixelSize;`):a.vertex.code.add(b.glsl`float segmentLengthRender = length(position - auxpos1);
float startPseudoScreen = mix(uv0.y, uv0.y - segmentLengthRender, uv0.x) * discreteWorldToScreenRatio;
float segmentLengthPseudoScreen = segmentLengthRender * discreteWorldToScreenRatio;`),a.vertex.code.add(b.glsl`vec2 stippleDistanceLimits = computeStippleDistanceLimits(startPseudoScreen, segmentLengthPseudoScreen, lineSegmentPixelSize, stipplePatternPixelSize);
vStippleDistance = mix(stippleDistanceLimits.x, stippleDistanceLimits.y, uv0.x);
vStippleDistance *= gl_Position.w;`));a.vertex.code.add(b.glsl`}`);4===c.output&&a.include(k.OutputHighlight);a.include(f.Slice,c);a.fragment.uniforms.add("constantColor","vec4").add("alphaCoverage","float");a.fragment.code.add(b.glsl`
  void main() {
    discardBySlice(vpos);

    vec4 color = ${c.attributeColor?"vColor":"constantColor"};

    float stippleAlpha = getStippleAlpha();
    discardByStippleAlpha(stippleAlpha, stippleAlphaColorDiscard);

    vec4 finalColor = blendStipple(vec4(color.rgb, color.a * alphaCoverage), stippleAlpha);

    if (finalColor.a < ${b.glsl.float(m.symbolAlphaCutoff)}) {
      discard;
    }

    ${0===c.output?b.glsl`gl_FragColor = highlightSlice(finalColor, vpos);`:""}
    ${4===c.output?b.glsl`outputHighlight();`:""}
  }
  `);return a}const p=Object.freeze({__proto__:null,build:e});d.NativeLineShader=p;d.build=e});