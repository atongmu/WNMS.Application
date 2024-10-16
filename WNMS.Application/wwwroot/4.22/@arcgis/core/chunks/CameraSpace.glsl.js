/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{g as o}from"./StencilUtils.js";function e(e){e.fragment.uniforms.add("projInfo","vec4"),e.fragment.uniforms.add("zScale","vec2"),e.fragment.code.add(o`vec3 reconstructPosition(vec2 fragCoord, float depth) {
return vec3((fragCoord * projInfo.xy + projInfo.zw) * (zScale.x * depth + zScale.y), depth);
}`)}export{e as C};
