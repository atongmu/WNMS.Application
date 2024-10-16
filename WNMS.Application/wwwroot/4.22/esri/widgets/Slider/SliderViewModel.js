// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../core/Accessor ../../core/Error ../../core/Logger ../../core/maybe ../../core/accessorSupport/decorators/property ../../core/arrayUtils ../../core/has ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass".split(" "),function(p,g,e,r,t,l,h,w,x,y,u){const v=t.getLogger("esri.widgets.Slider.SliderViewModel");e=function(q){function n(a){a=q.call(this,a)||this;a.precision=4;a.thumbsConstrained=!0;
return a}p._inheritsLoose(n,q);var k=n.prototype;k.toPrecision=function(a){return parseFloat(a.toFixed(this.precision))};k.defaultLabelFormatFunction=function(a){const {max:c,min:b,precision:d}=this;return parseFloat(a.toFixed(10<c-b?2:d)).toString()};k.defaultInputFormatFunction=function(a){return a.toString()};k.defaultInputParseFunction=function(a){return parseFloat(a)};k.getBoundsForValueAtIndex=function(a){const {thumbsConstrained:c,max:b,min:d,values:f}=this;if(c){const m=a-1;a+=1;return{min:l.isSome(f[m])?
f[m]:d,max:l.isSome(f[a])?f[a]:b}}return{min:d,max:b}};k.getLabelForValue=function(a,c,b){return l.isSome(a)?this.labelFormatFunction?this.labelFormatFunction(a,c,b):this.defaultLabelFormatFunction(a):null};k.setMax=function(a){const {max:c,values:b}=this;if(isNaN(a))this._logError("slider:invalid-value","Property 'max' must of type 'number'.");else if(null===a)this._set("max",null);else if(a=this.toPrecision(a),c!==a&&(this._set("max",a),b&&b.length))for(let d=0;d<b.length;d++)a<b[d]&&this.setValue(d,
a)};k.setMin=function(a){const {min:c,values:b}=this;if(isNaN(a))this._logError("slider:invalid-value","Property 'min' must of type 'number'.");else if(null===a)this._set("min",null);else if(a=this.toPrecision(a),c!==a&&(this._set("min",a),b&&b.length))for(let d=0;d<b.length;d++)a>b[d]&&this.setValue(d,a)};k.setValue=function(a,c){if(isNaN(c))this._logError("slider:invalid-value","Members of property 'values' must of type 'number'.");else{var {values:b}=this,d=b[a];c=this.toPrecision(c);d!==c&&(b=
[...b],b[a]=c,this._set("values",b),this.notifyChange("labels"))}};k._logError=function(a,c,b){v.error(new r(a,c,b))};p._createClass(n,[{key:"labelFormatFunction",set:function(a){this._set("labelFormatFunction",a)}},{key:"inputFormatFunction",set:function(a){this._set("inputFormatFunction",a)}},{key:"inputParseFunction",set:function(a){this._set("inputParseFunction",a)}},{key:"labels",get:function(){const {max:a,min:c,values:b}=this,d=b&&b.length?b.map((f,m)=>this.getLabelForValue(f,"value",m)):[];
return{max:this.getLabelForValue(a,"max"),min:this.getLabelForValue(c,"min"),values:d}}},{key:"max",set:function(a){this.setMax(a)}},{key:"min",set:function(a){this.setMin(a)}},{key:"state",get:function(){const {max:a,min:c}=this;return l.isSome(a)&&l.isSome(c)&&a>c?"ready":"disabled"}},{key:"values",set:function(a){const {max:c,min:b}=this,d=this.values;d&&a&&d.length===a.length&&d.every((f,m)=>f===a[m])||(this._set("values",null),a&&a.length&&(l.isSome(b)&&a.some(f=>f<b)&&(this.min=Math.min(...a)),
l.isSome(c)&&a.some(f=>f>c)&&(this.max=Math.max(...a))),this._set("values",a))}}]);return n}(e);g.__decorate([h.property()],e.prototype,"labelFormatFunction",null);g.__decorate([h.property()],e.prototype,"inputFormatFunction",null);g.__decorate([h.property()],e.prototype,"inputParseFunction",null);g.__decorate([h.property({readOnly:!0})],e.prototype,"labels",null);g.__decorate([h.property()],e.prototype,"max",null);g.__decorate([h.property()],e.prototype,"min",null);g.__decorate([h.property()],e.prototype,
"precision",void 0);g.__decorate([h.property({readOnly:!0})],e.prototype,"state",null);g.__decorate([h.property()],e.prototype,"thumbsConstrained",void 0);g.__decorate([h.property()],e.prototype,"values",null);return e=g.__decorate([u.subclass("esri.widgets.Slider.SliderViewModel")],e)});