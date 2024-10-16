// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../../chunks/vec3f64","../../../chunks/vec4f64"],function(b,e,f){let n=function(){function c(d,g,a,h,k,l,m){this.texture=d;this.type=g;d.retain();this.offsetAndScale=f.fromValues(a.offset[0],a.offset[1],a.scale,a.scale);this.opacities=e.fromValues(h,m?l:0,k)}c.prototype.destroy=function(){this.texture.release()};return c}();b.TextureReference=n;Object.defineProperty(b,"__esModule",{value:!0})});