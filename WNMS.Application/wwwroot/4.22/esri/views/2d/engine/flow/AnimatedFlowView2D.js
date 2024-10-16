// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../core/Accessor ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ./dataUtils ./FlowContainer ./FlowStrategy ./FlowStyle".split(" "),function(f,c,b,e,v,w,x,n,p,q,r,t){b=function(m){function g(){var a=m.apply(this,arguments)||this;a._loadImagery=(h,k,l,u)=>p.loadImagery(a.layer,
h,k,l,u);a._createStreamlinesMesh=(h,k,l)=>a.layer.createStreamlinesMesh({flowData:k,rendererSettings:h},{signal:l});a.container=null;a.layer=null;a.type="rasterAnimatedFlow";a.redrawOrRefetch=f._asyncToGenerator(function*(){a._rendererChanged()});return a}f._inheritsLoose(g,m);var d=g.prototype;d.update=function(a){a.stationary?this._strategy.update(a):this.layerView.requestUpdate()};d.install=function(a){this.container=new q;a.addChild(this.container);this._strategy=new r({flowContainer:this.container});
this._rendererChanged()};d.uninstall=function(){this._strategy.destroy();this.container.removeAllChildren();this.container.remove()};d.moveEnd=function(){};d.doRefresh=function(){var a=f._asyncToGenerator(function*(){});return function(){return a.apply(this,arguments)}}();d.attach=function(){};d._rendererChanged=function(){if("animated-flow"===this.layer.renderer.type){var a=new t(this._loadImagery,this._createStreamlinesMesh,this.layer.renderer);this.container.flowStyle=a;this.layerView.requestUpdate()}};
f._createClass(g,[{key:"updating",get:function(){return!this._strategy||this._strategy.updating}}]);return g}(b);c.__decorate([e.property()],b.prototype,"_strategy",void 0);c.__decorate([e.property()],b.prototype,"container",void 0);c.__decorate([e.property()],b.prototype,"layer",void 0);c.__decorate([e.property()],b.prototype,"layerView",void 0);c.__decorate([e.property()],b.prototype,"type",void 0);c.__decorate([e.property()],b.prototype,"updating",null);return b=c.__decorate([n.subclass("esri.views.2d.engine.flow.AnimatedFlowView2D")],
b)});