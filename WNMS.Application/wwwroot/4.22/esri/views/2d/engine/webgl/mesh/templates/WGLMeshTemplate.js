// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["../../../../../../core/maybe","../../../../../../symbols/cim/effects/CIMEffectHelper"],function(h,e){return function(){function g(){}var f=g.prototype;f.bindFeature=function(d,b,c){};f.write=function(d,b,c){var a;if(h.isNone(this._effects)||0===(null==(a=this._effects)?void 0:a.length))return this._write(d,b);c=e.CIMEffectHelper.executeEffects(this._effects,b.readLegacyGeometryForDisplay());for(a=e.CIMEffectHelper.next(c);a;)this._write(d,b,a),a=e.CIMEffectHelper.next(c)};f._write=function(d,
b,c){};return g}()});