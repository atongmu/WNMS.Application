// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["../../../core/Collection","../../../core/Logger","../../../core/watchUtils","../../../chunks/vec3","./RenderContext"],function(h,f,k,g,l){function e(d,c,a){if("function"===typeof d[c])d[c](a)}const m=f.getLogger("esri.views.3d.externalRenderers.ExternalRendererStore");f=function(){function d(){this._renderers=new h}var c=d.prototype;c.add=function(a,b){this._findOrCreateStageRenderer(a).add(b)};c.remove=function(a,b){(a=this._findStageRenderer(a))&&a.remove(b);0===a.renderers.length&&(a.destroy(),
this._renderers.remove(a))};c._findStageRenderer=function(a){return this._renderers.find(b=>b.view===a)};c._findOrCreateStageRenderer=function(a){let b=this._findStageRenderer(a);b||(b=new n(a),this._renderers.add(b));return b};return d}();let n=function(){function d(a){this.view=a;this.canRender=!0;this.renderers=new h;this._readyWatchHandle=k.init(a,"ready",b=>{b?(this.context=new l(this.view),this.view._stage.addRenderPlugin([3,5],this)):(this.renderers.forEach(p=>e(p,"dispose",this.context)),
this.context=null)})}var c=d.prototype;c.destroy=function(){this.renderers.drain(a=>{this.context&&e(a,"dispose",this.context)});this.view._stage&&this.view._stage.removeRenderPlugin(this);this._readyWatchHandle&&(this._readyWatchHandle.remove(),this._readyWatchHandle=null);this.context=null};c.add=function(a){-1!==this.renderers.indexOf(a)?m.warn("add(): Cannot add external renderer: renderer has already been added"):(this.renderers.add(a),this.context&&(this._webglStateReset(),e(a,"setup",this.context),
this.view._stage.renderView.requestRender()))};c.remove=function(a){const b=this.renderers.indexOf(a);-1!==b&&(this.renderers.removeAt(b),this.context&&(e(a,"dispose",this.context),this.view._stage.renderView.requestRender()))};c.initializeRenderContext=function(a){this.context.gl=a.renderContext.rctx.gl;this.context.rctx=a.renderContext.rctx;0<this.renderers.length&&this._webglStateReset();this.renderers.forEach(b=>e(b,"setup",this.context))};c.uninitializeRenderContext=function(){};c.render=function(a){if(0===
this.renderers.length||0!==a.pass||3!==a.slot&&5!==a.slot)return!1;this._updateContext(a);this._webglStateReset();this.renderers.forEach(b=>{switch(a.slot){case 3:e(b,"render",this.context);break;case 5:e(b,"renderTransparent",this.context)}});return!0};c._updateContext=function(a){this.context.camera.copyFrom(a.camera);g.copy(this.context.sunLight.direction,a.scenelightingData.old.direction);g.copy(this.context.sunLight.diffuse.color,a.scenelightingData.old.diffuse.color);g.copy(this.context.sunLight.ambient.color,
a.scenelightingData.old.ambient.color);this.context._renderTargetHelper=a.offscreenRenderingHelper};c._webglStateReset=function(){var a;this.context.rctx.resetState();null==(a=this.context._renderTargetHelper)?void 0:a.bindFramebuffer()};return d}();return f});