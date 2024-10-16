// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["../../../../../chunks/_rollupPluginBabelHelpers","../enums","../Utils","./WGLGeometryBrush","../materialKey/MaterialKey"],function(r,t,u,v,w){return function(l){function d(){return l.apply(this,arguments)||this}r._inheritsLoose(d,l);var e=d.prototype;e.dispose=function(){};e.getGeometryType=function(){return t.WGLGeometryType.TEXT};e.drawGeometry=function(f,m,g,a){const {context:c,painter:n,rendererInfo:h,state:p}=f,b=w.TextMaterialKey.load(g.materialKey),{bufferLayouts:q,attributes:k}=u.createProgramDescriptor(b.data,
{geometry:[{location:0,name:"a_pos",count:2,type:5122},{location:1,name:"a_id",count:4,type:5121},{location:2,name:"a_color",count:4,type:5121,normalized:!0},{location:3,name:"a_haloColor",count:4,type:5121,normalized:!0},{location:4,name:"a_texFontSize",count:4,type:5121},{location:5,name:"a_aux",count:4,type:5120},{location:6,name:"a_zoomRange",count:2,type:5123},{location:7,name:"a_vertexOffset",count:2,type:5122},{location:8,name:"a_texCoords",count:2,type:5123}]});a=n.materialManager.getMaterialProgram(f,
b,"materials/text",k,a);c.useProgram(a);this._setSharedUniforms(a,f,m);n.textureManager.bindTextures(c,a,b);a.setUniformMatrix3fv("u_displayMat3",p.displayMat3);a.setUniformMatrix3fv("u_displayViewMat3",p.displayViewMat3);this._setSizeVVUniforms(b,a,h,m);this._setColorAndOpacityVVUniforms(b,a,h);this._setRotationVVUniforms(b,a,h);a.setUniform1f("u_isHalo",1);g.draw(c,q,k);a.setUniform1f("u_isHalo",0);g.draw(c,q,k)};return d}(v)});