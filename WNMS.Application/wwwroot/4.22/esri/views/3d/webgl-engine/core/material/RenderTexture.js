// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../../../../core/maybe"],function(f,b){let k=function(){function g(a,e){this._textureRep=a;this._disposed=!1;a=this._textureRep.acquire(e);a.then(c=>{this._disposed?b.releaseMaybe(c):this._textureRef=c});this.loadPromise=a}var h=g.prototype;h.dispose=function(){this._textureRef=b.releaseMaybe(this._textureRef);this._disposed=!0};h.bind=function(a,e,c){const d=b.isSome(this._textureRef)?this._textureRef.glTexture:null;b.isSome(d)&&(a.bindTexture(d,e),a.setUniform2f(c,d.descriptor.width,
d.descriptor.height))};return g}();f.RenderTexture=k;Object.defineProperty(f,"__esModule",{value:!0})});