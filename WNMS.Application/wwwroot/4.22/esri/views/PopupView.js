// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ../core/arrayUtils ../core/maybe ../core/promiseUtils ../core/Logger ../core/accessorSupport/ensureType ../core/has ../core/accessorSupport/set ../core/accessorSupport/decorators/subclass".split(" "),function(q,l,t,u,k,v,y,z,A,B,w){q.PopupView=m=>{m=function(r){function n(){return r.apply(this,arguments)||this}l._inheritsLoose(n,r);var g=n.prototype;g.fetchPopupFeatures=function(){var b=l._asyncToGenerator(function*(a,c){yield this.when();
const {location:e,queryArea:d,layerViewsAndGraphics:f,clientOnlyGraphics:h}=yield this._prepareFetchPopupFeatures(a,c);a=Promise.resolve(h);c=this._queryLayerPopupFeatures(d,f,c);const p=c.map(x=>x.promise);a=v.eachAlwaysValues([a,...p]).then(u.flatten);return{location:e,clientOnlyGraphics:h,allGraphicsPromise:a,promisesPerLayerView:c}});return function(a,c){return b.apply(this,arguments)}}();g._queryLayerPopupFeatures=function(b,a,c){return a.map(({layerView:e,graphics:d})=>{d={clientGraphics:d,
event:k.isSome(c)?c.event:null,signal:k.isSome(c)?c.signal:null,defaultPopupTemplateEnabled:k.isSome(c)?!!c.defaultPopupTemplateEnabled:!1};d=e.fetchPopupFeatures(b,d);return{layerView:e,promise:d}})};g._isValidPopupGraphic=function(b,a){return b&&!!b.getEffectivePopupTemplate(k.isSome(a)&&a.defaultPopupTemplateEnabled)};g._prepareFetchPopupFeatures=function(){var b=l._asyncToGenerator(function*(a,c){const {clientGraphics:e,queryArea:d,location:f}=yield this._popupHitTestGraphics(a,c);a=this._getFetchPopupLayerViews();
const {layerViewsAndGraphics:h,clientOnlyGraphics:p}=this._graphicsPerFetchPopupLayerView(e,a);return{clientOnlyGraphics:p,layerViewsAndGraphics:h,queryArea:d,location:f}});return function(a,c){return b.apply(this,arguments)}}();g._popupHitTestGraphics=function(){var b=l._asyncToGenerator(function*(a,c){const {results:e,mapPoint:d}=yield this.popupHitTest(a);a=e.filter(h=>this._isValidPopupGraphic(h.graphic,c));const f=a.length?a[0].mapPoint:null;return{clientGraphics:a.map(h=>h.graphic),queryArea:d,
location:d||f}});return function(a,c){return b.apply(this,arguments)}}();g._getFetchPopupLayerViews=function(){const b=[];this.allLayerViews.forEach(a=>{this._isValidPopupLayerView(a)&&b.push(a)});k.isSome(this.graphicsView)&&this._isValidPopupLayerView(this.graphicsView)&&b.push(this.graphicsView);return b.reverse()};g._isValidPopupLayerView=function(b){return k.isSome(b)&&(!("layer"in b)||!b.suspended)&&"fetchPopupFeatures"in b};g._graphicsPerFetchPopupLayerView=function(b,a){const c=[],e=new Map;
a=a.map(d=>{const f=[];"layer"in d?e.set(d.layer,f):e.set(d.graphics,f);return{layerView:d,graphics:f}});for(const d of b)(b=e.get(d.layer)||e.get(d.sourceLayer)||null)?b.push(d):c.push(d);return{layerViewsAndGraphics:a,clientOnlyGraphics:c}};return n}(m);return m=t.__decorate([w.subclass("esri.views.PopupView")],m)};Object.defineProperty(q,"__esModule",{value:!0})});