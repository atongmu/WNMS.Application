// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["../../../../chunks/_rollupPluginBabelHelpers"],function(d){return function(){function b(a){this._material=a.material;this._techniqueRep=a.techniqueRep;this._output=a.output}var c=b.prototype;c.dispose=function(){this._techniqueRep.release(this._technique)};c.ensureTechnique=function(a,e,f=this._output){return this._technique=this._techniqueRep.releaseAndAcquire(a,this._material.getTechniqueConfig(f,e),this._technique)};c.ensureResources=function(a){return 2};d._createClass(b,[{key:"technique",
get:function(){return this._technique}}]);return b}()});