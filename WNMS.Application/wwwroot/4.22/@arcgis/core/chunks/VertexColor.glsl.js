/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{g as o}from"./StencilUtils.js";function r(r,e){e.attributeColor?(r.attributes.add("color","vec4"),r.varyings.add("vColor","vec4"),r.vertex.code.add(o`void forwardVertexColor() { vColor = color; }`),r.vertex.code.add(o`void forwardNormalizedVertexColor() { vColor = color * 0.003921568627451; }`)):r.vertex.code.add(o`void forwardVertexColor() {}
void forwardNormalizedVertexColor() {}`)}export{r as V};
