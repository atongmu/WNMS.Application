// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../../../../chunks/_rollupPluginBabelHelpers","../../../../../core/Error","../enums","../mesh/templates/meshUtils"],function(k,h,z,f,A){function x(d,b){const a=f.WGLVVFlag.SIZE_FIELD_STOPS|f.WGLVVFlag.SIZE_MINMAX_VALUE|f.WGLVVFlag.SIZE_SCALE_STOPS|f.WGLVVFlag.SIZE_UNIT_VALUE;d=(d&(f.WGLVVTarget.FIELD_TARGETS_OUTLINE|f.WGLVVTarget.MINMAX_TARGETS_OUTLINE|f.WGLVVTarget.SCALE_TARGETS_OUTLINE|f.WGLVVTarget.UNIT_TARGETS_OUTLINE))>>>4;return b.isOutline||b.isOutlinedFill?a&d:a&~d}let l=
function(){function d(a){this._data=0;this._data=a}d.load=function(a){const c=this.shared;c.data=a;return c};var b=d.prototype;b.setBit=function(a,c){a=1<<a;this._data=c?this._data|a:this._data&~a};b.bit=function(a){return(this._data&1<<a)>>a};b.setBits=function(a,c,e){for(let g=c,m=0;g<e;g++,m++)this.setBit(g,0!==(a&1<<m))};b.bits=function(a,c){let e=0;for(let g=a,m=0;g<c;g++,m++)e|=this.bit(g)<<m;return e};b.hasVV=function(){return!1};b.setVV=function(a,c){};b.getVariation=function(){return{mapAligned:this.mapAligned,
pattern:this.pattern,sdf:this.sdf}};b.getVariationHash=function(){return this._data&~(7&this.textureBinding)};h._createClass(d,[{key:"data",get:function(){return this._data},set:function(a){this._data=a}},{key:"geometryType",get:function(){return this.bits(8,11)},set:function(a){this.setBits(a,8,11)}},{key:"mapAligned",get:function(){return!!this.bit(20)},set:function(a){this.setBit(20,a)}},{key:"sdf",get:function(){return!!this.bit(11)},set:function(a){this.setBit(11,a)}},{key:"pattern",get:function(){return!!this.bit(12)},
set:function(a){this.setBit(12,a)}},{key:"textureBinding",get:function(){return this.bits(0,8)},set:function(a){this.setBits(a,0,8)}},{key:"geometryTypeString",get:function(){switch(this.geometryType){case f.WGLGeometryType.FILL:return"fill";case f.WGLGeometryType.MARKER:return"marker";case f.WGLGeometryType.LINE:return"line";case f.WGLGeometryType.TEXT:return"text";case f.WGLGeometryType.LABEL:return"label";default:throw new z(`Unable to handle unknown geometryType: ${this.geometryType}`);}}}]);
return d}();l.shared=new l(0);const n=d=>function(b){function a(){return b.apply(this,arguments)||this}h._inheritsLoose(a,b);var c=a.prototype;c.hasVV=function(){return b.prototype.hasVV.call(this)||this.vvSizeMinMaxValue||this.vvSizeScaleStops||this.vvSizeFieldStops||this.vvSizeUnitValue};c.setVV=function(e,g){b.prototype.setVV.call(this,e,g);e&=x(e,g);this.vvSizeMinMaxValue=!!(e&f.WGLVVFlag.SIZE_MINMAX_VALUE);this.vvSizeFieldStops=!!(e&f.WGLVVFlag.SIZE_FIELD_STOPS);this.vvSizeUnitValue=!!(e&f.WGLVVFlag.SIZE_UNIT_VALUE);
this.vvSizeScaleStops=!!(e&f.WGLVVFlag.SIZE_SCALE_STOPS)};h._createClass(a,[{key:"vvSizeMinMaxValue",get:function(){return 0!==this.bit(16)},set:function(e){this.setBit(16,e)}},{key:"vvSizeScaleStops",get:function(){return 0!==this.bit(17)},set:function(e){this.setBit(17,e)}},{key:"vvSizeFieldStops",get:function(){return 0!==this.bit(18)},set:function(e){this.setBit(18,e)}},{key:"vvSizeUnitValue",get:function(){return 0!==this.bit(19)},set:function(e){this.setBit(19,e)}}]);return a}(d),y=d=>function(b){function a(){return b.apply(this,
arguments)||this}h._inheritsLoose(a,b);var c=a.prototype;c.hasVV=function(){return b.prototype.hasVV.call(this)||this.vvRotation};c.setVV=function(e,g){b.prototype.setVV.call(this,e,g);this.vvRotation=!g.isOutline&&!!(e&f.WGLVVFlag.ROTATION)};h._createClass(a,[{key:"vvRotation",get:function(){return 0!==this.bit(15)},set:function(e){this.setBit(15,e)}}]);return a}(d),v=d=>function(b){function a(){return b.apply(this,arguments)||this}h._inheritsLoose(a,b);var c=a.prototype;c.hasVV=function(){return b.prototype.hasVV.call(this)||
this.vvColor};c.setVV=function(e,g){b.prototype.setVV.call(this,e,g);this.vvColor=!g.isOutline&&!!(e&f.WGLVVFlag.COLOR)};h._createClass(a,[{key:"vvColor",get:function(){return 0!==this.bit(13)},set:function(e){this.setBit(13,e)}}]);return a}(d),w=d=>function(b){function a(){return b.apply(this,arguments)||this}h._inheritsLoose(a,b);var c=a.prototype;c.hasVV=function(){return b.prototype.hasVV.call(this)||this.vvOpacity};c.setVV=function(e,g){b.prototype.setVV.call(this,e,g);this.vvOpacity=!g.isOutline&&
!!(e&f.WGLVVFlag.OPACITY)};h._createClass(a,[{key:"vvOpacity",get:function(){return 0!==this.bit(14)},set:function(e){this.setBit(14,e)}}]);return a}(d);let p=function(d){function b(){return d.apply(this,arguments)||this}h._inheritsLoose(b,d);b.load=function(a){const c=this.shared;c.data=a;return c};b.from=function(a){const c=this.load(0);c.geometryType=f.WGLGeometryType.FILL;c.dotDensity="dot-density"===a.stride.fill;c.simple="simple"===a.stride.fill;c.outlinedFill=a.isOutlinedFill;c.dotDensity||
c.setVV(a.vvFlags,a);return c.data};b.prototype.getVariation=function(){return{...d.prototype.getVariation.call(this),dotDensity:this.dotDensity,outlinedFill:this.outlinedFill,simple:this.simple,vvColor:this.vvColor,vvOpacity:this.vvOpacity,vvSizeFieldStops:this.vvSizeFieldStops,vvSizeMinMaxValue:this.vvSizeMinMaxValue,vvSizeScaleStops:this.vvSizeScaleStops,vvSizeUnitValue:this.vvSizeUnitValue}};h._createClass(b,[{key:"dotDensity",get:function(){return!!this.bit(15)},set:function(a){this.setBit(15,
a)}},{key:"simple",get:function(){return!!this.bit(22)},set:function(a){this.setBit(22,a)}},{key:"outlinedFill",get:function(){return!!this.bit(21)},set:function(a){this.setBit(21,a)}}]);return b}(v(w(n(l))));p.shared=new p(0);let q=function(d){function b(){return d.apply(this,arguments)||this}h._inheritsLoose(b,d);b.load=function(a){const c=this.shared;c.data=a;return c};b.from=function(a){const c=this.load(0);c.geometryType=f.WGLGeometryType.MARKER;c.setVV(a.vvFlags,a);return c.data};b.prototype.getVariation=
function(){return{...d.prototype.getVariation.call(this),vvColor:this.vvColor,vvRotation:this.vvRotation,vvOpacity:this.vvOpacity,vvSizeFieldStops:this.vvSizeFieldStops,vvSizeMinMaxValue:this.vvSizeMinMaxValue,vvSizeScaleStops:this.vvSizeScaleStops,vvSizeUnitValue:this.vvSizeUnitValue}};return b}(v(w(y(n(l)))));q.shared=new q(0);let r=function(d){function b(){return d.apply(this,arguments)||this}h._inheritsLoose(b,d);b.load=function(a){const c=this.shared;c.data=a;return c};b.from=function(a){const c=
this.load(0);c.geometryType=f.WGLGeometryType.LINE;c.setVV(a.vvFlags,a);return c.data};b.prototype.getVariation=function(){return{...d.prototype.getVariation.call(this),vvColor:this.vvColor,vvOpacity:this.vvOpacity,vvSizeFieldStops:this.vvSizeFieldStops,vvSizeMinMaxValue:this.vvSizeMinMaxValue,vvSizeScaleStops:this.vvSizeScaleStops,vvSizeUnitValue:this.vvSizeUnitValue}};return b}(v(w(n(l))));r.shared=new r(0);let t=function(d){function b(){return d.apply(this,arguments)||this}h._inheritsLoose(b,d);
b.load=function(a){const c=this.shared;c.data=a;return c};b.from=function(a){const c=this.load(0);c.geometryType=f.WGLGeometryType.TEXT;c.setVV(a.vvFlags,a);return c.data};b.prototype.getVariation=function(){return{...d.prototype.getVariation.call(this),vvColor:this.vvColor,vvOpacity:this.vvOpacity,vvRotation:this.vvRotation,vvSizeFieldStops:this.vvSizeFieldStops,vvSizeMinMaxValue:this.vvSizeMinMaxValue,vvSizeScaleStops:this.vvSizeScaleStops,vvSizeUnitValue:this.vvSizeUnitValue}};return b}(v(w(y(n(l)))));
t.shared=new t(0);let u=function(d){function b(){return d.apply(this,arguments)||this}h._inheritsLoose(b,d);b.load=function(a){const c=this.shared;c.data=a;return c};b.from=function(a){const c=this.load(0);c.geometryType=f.WGLGeometryType.LABEL;c.setVV(a.vvFlags,a);c.mapAligned=A.isMapAligned(a.placement);return c.data};b.prototype.getVariation=function(){return{...d.prototype.getVariation.call(this),vvSizeFieldStops:this.vvSizeFieldStops,vvSizeMinMaxValue:this.vvSizeMinMaxValue,vvSizeScaleStops:this.vvSizeScaleStops,
vvSizeUnitValue:this.vvSizeUnitValue}};return b}(n(l));u.shared=new u(0);k.FillMaterialKey=p;k.LabelMaterialKey=u;k.LineMaterialKey=r;k.MarkerMaterialKey=q;k.MaterialKeyBase=l;k.TextMaterialKey=t;k.createMaterialKey=function(d,b){switch(d){case f.WGLGeometryType.FILL:return p.from(b);case f.WGLGeometryType.LINE:return r.from(b);case f.WGLGeometryType.MARKER:return q.from(b);case f.WGLGeometryType.TEXT:return t.from(b);case f.WGLGeometryType.LABEL:return u.from(b);default:throw Error(`Unable to createMaterialKey for unknown geometryType ${d}`);
}};k.getSizeFlagsMask=x;k.hydrateMaterialKey=function(d){switch(l.load(d).geometryType){case f.WGLGeometryType.MARKER:return new q(d);case f.WGLGeometryType.FILL:return new p(d);case f.WGLGeometryType.LINE:return new r(d);case f.WGLGeometryType.TEXT:return new t(d);case f.WGLGeometryType.LABEL:return new u(d)}};Object.defineProperty(k,"__esModule",{value:!0})});