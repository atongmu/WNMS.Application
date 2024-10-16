// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../Color ../../core/accessorSupport/decorators/aliasOf ../../core/arrayUtils ../../core/has ../../core/accessorSupport/decorators/cast ../../core/accessorSupport/decorators/property ../../core/accessorSupport/decorators/subclass ./SmartMappingSliderBase ./OpacitySlider/OpacitySliderViewModel ../support/widgetUtils ../support/decorators/messageBundle ../../core/Logger ../support/jsxFactory".split(" "),function(w,d,c,t,C,D,x,l,
y,z,A,E,B,F,h){var p;const u={trackFillColor:new c([0,121,193])};c=p=function(v){function m(a,b){a=v.call(this,a,b)||this;a._bgFillId=null;a._rampFillId=null;a.label=void 0;a.messages=null;a.stops=null;a.style={...u};a.viewModel=new A;a.zoomOptions=null;a._rampFillId=`${a.id}-ramp-fill`;a._bgFillId=`${a.id}-bg-fill`;return a}w._inheritsLoose(m,v);var n=m.prototype;n.castStyle=function(a){return{...u,...a}};m.fromVisualVariableResult=function(a,b){const {visualVariable:{stops:f},statistics:e}=a,{avg:g,
max:k,min:q,stddev:r}=e;return new p({max:k,min:q,stops:f,histogramConfig:{average:g,standardDeviation:r,bins:b?b.bins:[]}})};n.updateFromVisualVariableResult=function(a,b){const {visualVariable:{stops:f},statistics:e}=a,{avg:g,max:k,min:q,stddev:r}=e;this.set({max:k,min:q,stops:f,histogramConfig:{average:g,standardDeviation:r,bins:b?b.bins:[]}})};n.render=function(){const {state:a,label:b,visibleElements:f}=this,e="disabled"===a,g=this.classes("esri-opacity-slider","esri-widget","esri-widget--panel",
{["esri-disabled"]:e,["esri-opacity-slider--interactive-track"]:f.interactiveTrack});return h.tsx("div",{"aria-label":b,class:g},e?null:this.renderContent(this.renderRamp(),"esri-opacity-slider__slider-container","esri-opacity-slider__histogram-container"))};n.renderRamp=function(){const {_bgFillId:a,_rampFillId:b,style:{trackFillColor:f},viewModel:e,zoomOptions:g}=this,k=e.getStopInfo(f);return h.tsx("div",{class:"esri-opacity-slider__ramp"},h.tsx("svg",{xmlns:"http://www.w3.org/2000/svg"},h.tsx("defs",
null,this.renderRampFillDefinition(b,k),this.renderBackgroundFillDefinition(a)),h.tsx("rect",{x:"0",y:"0",fill:`url(#${a})`,height:"100%",width:"100%"}),h.tsx("rect",{x:"0",y:"0",fill:`url(#${b})`,height:"100%",width:"100%"})),g?this.renderZoomCaps():null)};return m}(z.SmartMappingSliderBase);d.__decorate([l.property({aliasOf:{source:"messages.widgetLabel",overridable:!0}})],c.prototype,"label",void 0);d.__decorate([l.property(),B.messageBundle("esri/widgets/smartMapping/OpacitySlider/t9n/OpacitySlider")],
c.prototype,"messages",void 0);d.__decorate([t.aliasOf("viewModel.stops")],c.prototype,"stops",void 0);d.__decorate([l.property()],c.prototype,"style",void 0);d.__decorate([x.cast("style")],c.prototype,"castStyle",null);d.__decorate([l.property()],c.prototype,"viewModel",void 0);d.__decorate([t.aliasOf("viewModel.zoomOptions")],c.prototype,"zoomOptions",void 0);return c=p=d.__decorate([y.subclass("esri.widgets.smartMapping.OpacitySlider")],c)});