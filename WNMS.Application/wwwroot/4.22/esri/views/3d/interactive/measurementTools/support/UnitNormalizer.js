// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["../../../../../chunks/_rollupPluginBabelHelpers","../../../../../core/unitUtils"],function(e,d){return function(){function c(a=null){this.spatialReference=a}var b=c.prototype;b.normalizeDistance=function(a){return a*this._metersPerDistanceUnit};b.normalizeElevation=function(a){return a*this._metersPerElevationUnit};b.normalizeArea=function(a){return a*this._squareMetersPerAreaUnit};b._updateNormalizationFactors=function(){this._metersPerDistanceUnit=d.getMetersPerUnitForSR(this._spatialReference,
1);this._metersPerElevationUnit=d.getMetersPerUnitForSR(this._spatialReference,1);this._squareMetersPerAreaUnit=this._metersPerDistanceUnit*this._metersPerDistanceUnit};e._createClass(c,[{key:"spatialReference",get:function(){return this._spatialReference},set:function(a){a!==this._spatialReference&&(this._spatialReference=a,this._updateNormalizationFactors())}}]);return c}()});