// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../views/3d/webgl-engine/core/shaderLibrary/shading/EvaluateAmbientLighting.glsl ../views/3d/webgl-engine/core/shaderLibrary/shading/EvaluateMainLighting.glsl ../views/3d/webgl-engine/core/shaderLibrary/util/CloudsParallaxShading.glsl ../views/3d/webgl-engine/core/shaderLibrary/util/ColorConversion.glsl ../views/3d/webgl-engine/core/shaderLibrary/util/RgbaFloatEncoding.glsl ../views/3d/webgl-engine/core/shaderModules/interfaces ../views/3d/webgl-engine/core/shaderModules/ShaderBuilder".split(" "),
function(b,e,f,g,h,k,c,l){function d(){const a=new l.ShaderBuilder;a.attributes.add("position","vec2");a.varyings.add("worldRay","vec3");a.vertex.uniforms.add("inverseProjectionMatrix","mat4");a.vertex.uniforms.add("inverseViewMatrix","mat4");a.vertex.code.add(c.glsl`void main(void) {
vec3 posViewNear = (inverseProjectionMatrix * vec4(position, -1.0, 1.0)).xyz;
worldRay = (inverseViewMatrix * vec4(posViewNear, 0.0)).xyz;
gl_Position = vec4(position, 1.0, 1.0);
}`);a.fragment.uniforms.add("lightingMainDirection","vec3").add("cubeMap","samplerCube");a.fragment.include(h.ColorConversion);a.fragment.include(k.RgbaFloatEncoding);a.include(f.EvaluateMainLighting);a.include(e.EvaluateAmbientLighting,{pbrMode:0,lightingSphericalHarmonicsOrder:2});a.include(g.CloudsParallaxShading);a.fragment.code.add(c.glsl`void main() {
vec4 cloudsColor = crossFade == 0 ? renderCloud(normalize(worldRay)) : renderCloudCrossFade(normalize(worldRay));
gl_FragColor = vec4((1.0 - totalFadeInOut) * cloudsColor.rgb, cloudsColor.a);
}`);return a}const m=Object.freeze({__proto__:null,build:d});b.CloudsCompositionShader=m;b.build=d});