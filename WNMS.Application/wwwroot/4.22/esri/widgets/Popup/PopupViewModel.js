// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../geometry ../../Graphic ../../symbols ../../core/Collection ../../core/Error ../../core/Handles ../../core/Logger ../../core/maybe ../../core/watchUtils ../../core/accessorSupport/decorators/property ../../core/arrayUtils ../../core/has ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass ../../geometry/support/webMercatorUtils ../../layers/Layer ../../support/actions/ActionBase ../../support/actions/ActionButton ../../support/actions/ActionToggle ../../symbols/support/symbolUtils ../../views/input/InputManager ../../views/support/layerViewUtils ../Feature/FeatureViewModel ./actions ./actionUtils ../support/AnchorElementViewModel ../support/GoTo ../../geometry/Point ../../symbols/SimpleFillSymbol".split(" "),
function(t,d,c,D,W,E,z,F,G,A,q,e,X,Y,Z,H,I,J,K,L,M,N,O,P,Q,x,u,R,S,T,U){const v=E.ofType({key:"type",defaultKeyValue:"button",base:K,typeMap:{button:L,toggle:M}}),B=G.getLogger("esri.widgets.Popup.PopupViewModel");c=function(C){function y(a){a=C.call(this,a)||this;a._handles=new F;a._pendingPromises=new Set;a._fetchFeaturesController=null;a._selectedClusterFeature=null;a.featurePage=null;a.actions=new v;a.defaultPopupTemplateEnabled=!1;a.autoCloseEnabled=!1;a.autoOpenEnabled=!0;a.browseClusterEnabled=
!1;a.content=null;a.featuresPerPage=20;a.featureViewModelAbilities=null;a.featureViewModels=[];a.highlightEnabled=!0;a.includeDefaultActions=!0;a.selectedClusterBoundaryFeature=new D({symbol:new U({outline:{width:1.5,color:"cyan"},style:"none"})});a.title=null;a.updateLocationEnabled=!1;a.view=null;a.visible=!1;a.zoomFactor=4;a.zoomToLocation=null;return a}t._inheritsLoose(y,C);var f=y.prototype;f.initialize=function(){this._handles.add([q.init(this,["autoOpenEnabled","view"],()=>this._autoOpenEnabledChange()),
this.on("view-change",()=>this._autoClose()),q.watch(this,["highlightEnabled","selectedFeature","visible","view"],()=>this._highlightFeature()),q.watch(this,"view.animation.state",a=>this._animationStateChange(a)),q.watch(this,"location",a=>this._locationChange(a)),q.watch(this,"selectedFeature",a=>this._selectedFeatureChange(a)),q.watch(this,["selectedFeatureIndex","featureCount","featuresPerPage"],()=>this._selectedFeatureIndexChange()),q.watch(this,["featurePage","selectedFeatureIndex","featureCount",
"featuresPerPage, featureViewModels"],()=>this._setGraphicOnFeatureViewModels()),q.watch(this,"featureViewModels",()=>this._featureViewModelsChange()),this.on("trigger-action",a=>u.triggerAction({event:a,view:this.view})),q.whenFalse(this,"waitingForResult",()=>this._waitingForResultChange(),!0),q.watch(this,["features","view","view.map","view.spatialReference"],()=>this._updateFeatureVMs()),q.watch(this,["view.scale"],this._viewScaleChange),q.whenFalse(this,"visible",()=>this.browseClusterEnabled=
!1),q.watch(this,"browseClusterEnabled",a=>a?this.enableClusterBrowsing():this.disableClusterBrowsing())])};f.destroy=function(){this._cancelFetchingFeatures();this._handles.destroy();this._handles=null;this._pendingPromises.clear();this.browseClusterEnabled=!1;this.view=null};f.centerAtLocation=function(){var {view:a}=this;const b=u.getSelectedTarget(this);return b?this.callGoTo({target:{target:b,scale:a.scale}}):(a=new z("center-at-location:invalid-target-or-view","Cannot center at a location without a target and view.",
{target:b,view:a}),B.error(a),Promise.reject(a))};f.clear=function(){this.set({promises:[],features:[],content:null,title:null,location:null})};f.fetchFeatures=function(a,b){const {view:g}=this;return g&&a?g.fetchPopupFeatures(a,{event:b&&b.event,defaultPopupTemplateEnabled:this.defaultPopupTemplateEnabled,signal:b&&b.signal}):(a=new z("fetch-features:invalid-screenpoint-or-view","Cannot fetch features without a screenPoint and view.",{screenPoint:a,view:g}),B.error(a),Promise.reject(a))};f.open=
function(a){a={updateLocationEnabled:!1,promises:[],fetchFeatures:!1,...a,visible:!0};const {fetchFeatures:b}=a;delete a.fetchFeatures;b&&this._setFetchFeaturesPromises(a.location);this.set(a)};f.triggerAction=function(a){(a=this.allActions.getItemAt(a))&&!a.disabled&&this.emit("trigger-action",{action:a})};f.next=function(){this.selectedFeatureIndex+=1;return this};f.previous=function(){--this.selectedFeatureIndex;return this};f.disableClusterBrowsing=function(){u.removeClusteredFeaturesForBrowsing(this);
this._clearBrowsedClusterGraphics()};f.enableClusterBrowsing=function(){var a=t._asyncToGenerator(function*(){yield u.displayClusterExtent(this);yield u.browseAggregateFeatures(this)});return function(){return a.apply(this,arguments)}}();f._animationStateChange=function(a){this.zoomToLocation||(x.zoomToFeature.disabled="waiting-for-target"===a)};f._clearBrowsedClusterGraphics=function(){var a;const b=null==(a=this.view)?void 0:a.graphics;b&&(b.remove(this.selectedClusterBoundaryFeature),b.remove(this._selectedClusterFeature));
this._selectedClusterFeature=null;this.selectedClusterBoundaryFeature.geometry=null};f._viewScaleChange=function(){var a;if(null!=(a=this.selectedFeature)&&a.isAggregate||this.browseClusterEnabled)this.visible=this.browseClusterEnabled=!1};f._locationChange=function(a){const {selectedFeature:b,updateLocationEnabled:g}=this;g&&a&&(!b||b.geometry)&&this.centerAtLocation()};f._selectedFeatureIndexChange=function(){this.featurePage=1<this.featureCount?Math.floor(this.selectedFeatureIndex/this.featuresPerPage)+
1:null};f._featureViewModelsChange=function(){this.featurePage=1<this.featureCount?1:null};f._setGraphicOnFeatureViewModels=function(){const {features:a,featureCount:b,featurePage:g,featuresPerPage:k,featureViewModels:l}=this;if(null!==g){var h=((g-1)*k+b)%b;l.slice(h,h+k).forEach((n,m)=>{n&&!n.graphic&&(n.graphic=a[h+m])})}};f._selectedFeatureChange=function(){var a=t._asyncToGenerator(function*(b){if(b){var {location:g,updateLocationEnabled:k,view:l}=this;this.browseClusterEnabled?(this._selectedClusterFeature&&
(l.graphics.remove(this._selectedClusterFeature),this._selectedClusterFeature=null),b.isAggregate||(b.symbol=yield N.getDisplayedSymbol(b),this._selectedClusterFeature=b,l.graphics.add(this._selectedClusterFeature))):!k&&g||!b.geometry?k&&!b.geometry&&this.centerAtLocation().then(()=>{this.location=l.center.clone()}):this.location=A.unwrap(this._getPointFromGeometry(b.geometry))}});return function(b){return a.apply(this,arguments)}}();f._waitingForResultChange=function(){!this.featureCount&&this.promises&&
(this.visible=!1)};f._setFetchFeaturesPromises=function(a){return this._fetchFeaturesWithController(this._getScreenPoint(a||this.location)).then(b=>{const {clientOnlyGraphics:g,promisesPerLayerView:k}=b;b=Promise.resolve(g);const l=k.map(h=>h.promise);this.promises=[b,...l]})};f._destroyFeatureVMs=function(){this.featureViewModels.forEach(a=>a&&!a.destroyed&&a.destroy());this._set("featureViewModels",[])};f._updateFeatureVMs=function(){const {selectedFeature:a,features:b,featureViewModels:g}=this;
null!=a&&a.isAggregate||(this.browseClusterEnabled=!1);this._destroyFeatureVMs();if(b&&b.length){var k=g.slice(0),l=[];b.forEach((h,n)=>{if(h){var m=null;k.some((w,V)=>{w&&w.graphic===h&&(m=w,k.splice(V,1));return!!m});if(m)l[n]=m;else{var p,r;const w=new Q({abilities:this.featureViewModelAbilities,defaultPopupTemplateEnabled:this.defaultPopupTemplateEnabled,spatialReference:null==(p=this.view)?void 0:p.spatialReference,graphic:h===a?h:null,map:null==(r=this.view)?void 0:r.map,view:this.view});l[n]=
w}}});k.forEach(h=>h&&!h.destroyed&&h.destroy());this._set("featureViewModels",l)}};f._getScreenPoint=function(a){const {view:b}=this;return b&&a&&"function"===typeof b.toScreen?b.toScreen(a):null};f._autoOpenEnabledChange=function(){const {_handles:a,autoOpenEnabled:b}=this;a.remove("auto-fetch-features");if(b&&this.view){const g=this.view.on("click",k=>{"mouse"===k.pointerType&&0!==k.button||this._fetchFeaturesAndOpen(k)},O.ViewEventPriorities.WIDGET);a.add(g,"auto-fetch-features")}};f._cancelFetchingFeatures=
function(){const a=this._fetchFeaturesController;a&&a.abort();this._fetchFeaturesController=null;this.notifyChange("waitingForResult")};f._fetchFeaturesWithController=function(a,b){this._cancelFetchingFeatures();const g=new AbortController,{signal:k}=g;this._fetchFeaturesController=g;this.notifyChange("waitingForResult");a=this.fetchFeatures(a,{signal:k,event:b});a.catch(()=>{}).then(()=>{this._fetchFeaturesController=null;this.notifyChange("waitingForResult")});return a};f._fetchFeaturesAndOpen=
function(a){const {screenPoint:b,mapPoint:g}=a,{view:k}=this;this._fetchFeaturesWithController(b,a).then(l=>{const {clientOnlyGraphics:h,promisesPerLayerView:n,location:m}=l,p=[Promise.resolve(h),...n.map(r=>r.promise)];k.popup.open({location:m||g,promises:p});return l})};f._updatePendingPromises=function(a){a&&this._pendingPromises.has(a)&&(this._pendingPromises.delete(a),this.notifyChange("pendingPromisesCount"))};f._autoClose=function(){this.autoCloseEnabled&&(this.visible=!1)};f._getPointFromGeometry=
function(a){return A.isNone(a)?null:"point"===a.type?a:"extent"===a.type?a.center:"polygon"===a.type?a.centroid:"multipoint"===a.type||"polyline"===a.type?a.extent.center:null};f._getLayerView=function(){var a=t._asyncToGenerator(function*(b,g){yield b.when();return b.whenLayerView(g)});return function(b,g){return a.apply(this,arguments)}}();f._highlightFeature=function(){var a=t._asyncToGenerator(function*(){this._handles.remove("highlight");const {selectedFeature:b,highlightEnabled:g,view:k,visible:l}=
this;if(b&&k&&g&&l){var {layer:h,sourceLayer:n}=b;if("map-notes"===(null==n?void 0:n.type)||"subtype-group"===(null==n?void 0:n.type))h=n;if(h&&h instanceof J){var m=this._getLayerView(k,h);this._highlightPromise=m;var p=yield m;if(p&&P.highlightsSupported(p)&&this._highlightPromise===m&&this.selectedFeature&&this.highlightEnabled&&this.visible){m="objectIdField"in h&&h.objectIdField;var r=b.attributes;p=p.highlight(r&&m&&r[m]||b);this._handles.add(p,"highlight")}}}});return function(){return a.apply(this,
arguments)}}();f._updateFeatures=function(a){const {features:b}=this;a&&a.length&&(b.length?(a=a.filter(g=>-1===b.indexOf(g)),this.features=b.concat(a)):this.features=a)};t._createClass(y,[{key:"isLoadingFeature",get:function(){return this.featureViewModels.some(a=>a.waitingForContent)}},{key:"active",get:function(){return!(!this.visible||this.waitingForResult)}},{key:"allActions",get:function(){const a=this._get("allActions")||new v;a.removeAll();const {actions:b,defaultActions:g,defaultPopupTemplateEnabled:k,
includeDefaultActions:l,selectedFeature:h}=this;var n=l?g.concat(b):b;const m=h&&("function"===typeof h.getEffectivePopupTemplate&&h.getEffectivePopupTemplate(k)||h.popupTemplate),p=m&&m.actions;(n=m&&m.overwriteActions?p:p?p.concat(n):n)&&n.filter(Boolean).forEach(r=>a.add(r));return a}},{key:"defaultActions",get:function(){var a;const b=this._get("defaultActions")||new v;b.removeAll();b.addMany(null!=(a=this.selectedFeature)&&a.isAggregate?[x.zoomToClusteredFeatures.clone(),x.browseClusteredFeatures.clone()]:
[x.zoomToFeature.clone()]);return b}},{key:"featureCount",get:function(){return this.features.length}},{key:"features",get:function(){return this._get("features")||[]},set:function(a){a=a||[];this._set("features",a);const {pendingPromisesCount:b,promiseCount:g,selectedFeatureIndex:k}=this,l=g&&a.length;l&&b&&-1===k?this.selectedFeatureIndex=0:l&&-1!==k||(this.selectedFeatureIndex=a.length?0:-1)}},{key:"location",get:function(){return this._get("location")||null},set:function(a){const b=this.get("view.spatialReference.isWebMercator");
a&&a.get("spatialReference.isWGS84")&&b&&(a=I.geographicToWebMercator(a));this._set("location",a)}},{key:"pendingPromisesCount",get:function(){return this._pendingPromises.size}},{key:"waitingForResult",get:function(){return!(!(this._fetchFeaturesController||0<this.pendingPromisesCount)||0!==this.featureCount)}},{key:"promiseCount",get:function(){return this.promises.length}},{key:"promises",get:function(){return this._get("promises")||[]},set:function(a){this._pendingPromises.clear();this.features=
[];Array.isArray(a)&&a.length?(this._set("promises",a),a=a.slice(0),a.forEach(b=>{this._pendingPromises.add(b);b.then(g=>{this._pendingPromises.has(b)&&this._updateFeatures(g);this._updatePendingPromises(b)},()=>this._updatePendingPromises(b))})):this._set("promises",[]);this.notifyChange("pendingPromisesCount")}},{key:"selectedFeature",get:function(){const {features:a,selectedFeatureIndex:b}=this;return-1===b?null:a[b]||null}},{key:"selectedFeatureIndex",get:function(){const a=this._get("selectedFeatureIndex");
return"number"===typeof a?a:-1},set:function(a){const {featureCount:b}=this;a=isNaN(a)||-1>a||!b?-1:(a+b)%b;this._set("selectedFeatureIndex",a)}},{key:"selectedFeatureViewModel",get:function(){return this.featureViewModels[this.selectedFeatureIndex]||null}},{key:"state",get:function(){return this.get("view.ready")?"ready":"disabled"}}]);return y}(S.GoToMixin(R));d.__decorate([e.property()],c.prototype,"featurePage",void 0);d.__decorate([e.property()],c.prototype,"isLoadingFeature",null);d.__decorate([e.property({type:v})],
c.prototype,"actions",void 0);d.__decorate([e.property({readOnly:!0})],c.prototype,"active",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"allActions",null);d.__decorate([e.property({type:Boolean})],c.prototype,"defaultPopupTemplateEnabled",void 0);d.__decorate([e.property()],c.prototype,"autoCloseEnabled",void 0);d.__decorate([e.property()],c.prototype,"autoOpenEnabled",void 0);d.__decorate([e.property()],c.prototype,"browseClusterEnabled",void 0);d.__decorate([e.property()],c.prototype,
"content",void 0);d.__decorate([e.property({type:v,readOnly:!0})],c.prototype,"defaultActions",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"featureCount",null);d.__decorate([e.property()],c.prototype,"features",null);d.__decorate([e.property()],c.prototype,"featuresPerPage",void 0);d.__decorate([e.property()],c.prototype,"featureViewModelAbilities",void 0);d.__decorate([e.property({readOnly:!0})],c.prototype,"featureViewModels",void 0);d.__decorate([e.property()],c.prototype,"highlightEnabled",
void 0);d.__decorate([e.property()],c.prototype,"includeDefaultActions",void 0);d.__decorate([e.property({type:T})],c.prototype,"location",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"pendingPromisesCount",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"selectedClusterBoundaryFeature",void 0);d.__decorate([e.property({readOnly:!0})],c.prototype,"waitingForResult",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"promiseCount",null);d.__decorate([e.property()],
c.prototype,"promises",null);d.__decorate([e.property({value:null,readOnly:!0})],c.prototype,"selectedFeature",null);d.__decorate([e.property({value:-1})],c.prototype,"selectedFeatureIndex",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"selectedFeatureViewModel",null);d.__decorate([e.property({readOnly:!0})],c.prototype,"state",null);d.__decorate([e.property()],c.prototype,"title",void 0);d.__decorate([e.property()],c.prototype,"updateLocationEnabled",void 0);d.__decorate([e.property()],
c.prototype,"view",void 0);d.__decorate([e.property()],c.prototype,"visible",void 0);d.__decorate([e.property()],c.prototype,"zoomFactor",void 0);d.__decorate([e.property()],c.prototype,"zoomToLocation",void 0);d.__decorate([e.property()],c.prototype,"centerAtLocation",null);return c=d.__decorate([H.subclass("esri.widgets.Popup.PopupViewModel")],c)});