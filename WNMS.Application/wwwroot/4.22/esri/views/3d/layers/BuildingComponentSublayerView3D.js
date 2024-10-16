// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../../chunks/_rollupPluginBabelHelpers ../../../chunks/tslib.es6 ../../../Graphic ../../../core/Error ../../../core/Logger ../../../core/maybe ../../../core/accessorSupport/decorators/property ../../../core/arrayUtils ../../../core/has ../../../core/accessorSupport/ensureType ../../../core/accessorSupport/decorators/subclass ../../../core/sql/WhereClause ../../../layers/buildingSublayers/BuildingComponentSublayer ../../../layers/support/FeatureFilter ../../../layers/support/fieldUtils ../../../rest/support/Query ./BuildingSublayerView3D ./I3SMeshView3D ./i3s/BuildingFilterUtil ./i3s/I3SGeometryUtil ./i3s/I3SMeshViewFilter ./i3s/I3SQueryEngine ./i3s/I3SQueryFeatureAdapter ./i3s/I3SQueryFeatureStore ./i3s/I3SUtil ./support/DefinitionExpressionSceneLayerView ../../layers/BuildingComponentSublayerView ../../layers/support/popupUtils ../../support/Scheduler".split(" "),
function(q,g,B,w,e,h,k,O,P,Q,C,D,R,E,n,x,F,G,y,H,z,I,J,K,A,L,M,v,N){const r=e.getLogger("esri.views.3d.layers.BuildingComponentSublayerView3D");e=function(t){function u(){var a=t.apply(this,arguments)||this;a.type="building-component-sublayer-3d";a.layerView=null;a._elevationContext="scene";a._isIntegratedMesh=!1;a._supportsLabeling=!1;a.lodFactor=1;a.progressiveLoadFactor=1;a._queryEngine=null;return a}q._inheritsLoose(u,t);var d=u.prototype;d.initialize=function(){this.updatingHandles.add(this,
"sublayer.renderer,definitionExpressionFields,filterExpressionFields",()=>this._updateRequiredFields());this.updatingHandles.add(this.sublayer,"renderer",a=>this._rendererChange(a),2);for(const a of["parsedDefinitionExpression","filter","viewFilter.parsedWhereClause","viewFilter.parsedGeometry","viewFilter.sortedObjectIds"])this.updatingHandles.add(this,a,()=>this._filterChange());this.updatingHandles.add(this,"parsedFilterExpressions",()=>this._updateSymbologyOverride(),2);this.addResolvingPromise(this._updateRequiredFields())};
d._updateSymbologyOverride=function(){const a=this.parsedFilterExpressions;0<a.length?this._setSymbologyOverride((b,c)=>{for(const [f,l]of a)try{if(f.testFeature(b))return y.applyFilterMode(c,l)}catch(m){this.logError(m)}return y.applyFilterMode(c,null)},this.filterExpressionFields):this._setSymbologyOverride(null,null)};d._createLayerGraphic=function(a){a=new B(null,null,a);a.layer=this.sublayer.layer;a.sourceLayer=this.sublayer;return a};d.canResume=function(){return t.prototype.canResume.call(this)&&
(!this._controller||this._controller.rootNodeVisible)};d.fetchPopupFeatures=function(){var a=q._asyncToGenerator(function*(b,c){if(b=this._validateFetchPopupFeatures(c))return Promise.reject(b);if(!h.isSome(c)||!c.clientGraphics||0===c.clientGraphics.length)return[];const f=[],l=[];b=h.isSome(this.sublayer.associatedLayer)?yield this.sublayer.associatedLayer.load():this.sublayer;b=n.unpackFieldNames(this.sublayer.fieldsIndex,yield v.getRequiredFields(b,v.getFetchPopupTemplate(this.sublayer,c)));const m=
new Set;for(const p of c.clientGraphics)n.populateMissingFields(b,p,m)?l.push(p):f.push(p);if(0===l.length)return Promise.resolve(f);h.isSome(this.sublayer.associatedLayer)&&(yield this.sublayer.associatedLayer.load().catch(()=>r.warn("Failed to load associated feature layer. Displaying popup attributes from cached attributes.")));return this.whenGraphicAttributes(l,Array.from(m)).catch(()=>l).then(p=>f.concat(p))});return function(b,c){return a.apply(this,arguments)}}();d._updateRequiredFields=function(){var a=
q._asyncToGenerator(function*(){const b=n.fixFields(this.sublayer.fieldsIndex,[...this.sublayer.renderer?yield this.sublayer.renderer.getRequiredFields(this.sublayer.fieldsIndex):[],...this.definitionExpressionFields||[],...this.filterExpressionFields||[]]);this._set("requiredFields",b)});return function(){return a.apply(this,arguments)}}();d._validateFetchPopupFeatures=function(a){const {sublayer:b}=this,{popupEnabled:c}=b;if(!c)return new w("buildingscenelayerview3d:fetchPopupFeatures","Popups are disabled",
{sublayer:b});if(!v.getFetchPopupTemplate(b,a))return new w("buildingscenelayerview3d:fetchPopupFeatures","Layer does not define a popup template",{sublayer:b})};d.getFilters=function(){const a=t.prototype.getFilters.call(this);this.addSqlFilter(a,this.parsedDefinitionExpression,this.logError);h.isSome(this.viewFilter)&&this.viewFilter.addFilters(a,this.view,this._controller.crsIndex,this._collection);return a};d.createQuery=function(){const a={outFields:["*"],returnGeometry:!1,outSpatialReference:this.view.spatialReference};
return h.isSome(this.filter)?this.filter.createQuery(a):new x(a)};d.queryExtent=function(a,b){return this._ensureQueryEngine().executeQueryForExtent(this._ensureQuery(a),null==b?void 0:b.signal)};d.queryFeatureCount=function(a,b){return this._ensureQueryEngine().executeQueryForCount(this._ensureQuery(a),null==b?void 0:b.signal)};d.queryFeatures=function(a,b){return this._ensureQueryEngine().executeQuery(this._ensureQuery(a),null==b?void 0:b.signal).then(c=>{if(null==c||!c.features)return c;const f=
this.sublayer,l=f.layer;for(const m of c.features)m.layer=l,m.sourceLayer=f;return c})};d.queryObjectIds=function(a,b){return this._ensureQueryEngine().executeQueryForIds(this._ensureQuery(a),null==b?void 0:b.signal)};d._ensureQueryEngine=function(){h.isNone(this._queryEngine)&&(this._queryEngine=this._createQueryEngine());return this._queryEngine};d._createQueryEngine=function(){const a=H.createGetFeatureExtent(this.view.spatialReference,this.view.renderSpatialReference,this._collection);return new I.default({layerView:this,
priority:N.TaskPriority.FEATURE_QUERY_ENGINE,spatialIndex:new K.default({featureAdapter:new J.I3SQueryFeatureAdapter({objectIdField:this.sublayer.objectIdField,attributeStorageInfo:this.sublayer.attributeStorageInfo,getFeatureExtent:a}),forAllFeatures:(b,c)=>this._forAllFeatures((f,l,m)=>b({id:f,index:l,meta:m}),c,2),getFeatureExtent:a,sourceSpatialReference:A.getIndexCrs(this.sublayer),viewSpatialReference:this.view.spatialReference})})};d._ensureQuery=function(a){return this._addDefinitionExpressionToQuery(h.isNone(a)?
this.createQuery():x.from(a))};q._createClass(u,[{key:"layerUid",get:function(){return this.sublayer.layer.uid}},{key:"sublayerUid",get:function(){return this.sublayer.uid}},{key:"parsedFilterExpressions",get:function(){return"Overview"===this.sublayer.modelName?[]:this.layerView.filterExpressions.map(([a,b])=>{let c;try{c=D.WhereClause.create(a,this.sublayer.fieldsIndex)}catch(f){return r.error("Failed to parse filterExpression: "+f),null}if(!c.isStandardized)return r.error("filterExpression is using non standard function"),
null;a=[];A.findFieldsCaseInsensitive(c.fieldNames,this.sublayer.fields,{missingFields:a});return 0<a.length?(r.error(`filterExpression references unknown fields: ${a.join(", ")}`),null):[c,b]}).filter(a=>null!=a)}},{key:"filter",get:function(){return h.isSome(this.viewFilter)?this.viewFilter.filter:null},set:function(a){h.isNone(a)||!z.I3SMeshViewFilter.checkSupport(a)?this.viewFilter=null:h.isSome(this.viewFilter)?this.viewFilter.filter=a:this.viewFilter=new z.I3SMeshViewFilter({filter:a,layerFieldsIndex:this.sublayer.fieldsIndex,
loadAsyncModule:b=>this._loadAsyncModule(b),applyFilters:()=>this._applyFilters(!0),addSqlFilter:(b,c)=>this.addSqlFilter(b,c,this.logError)})}},{key:"filterExpressionFields",get:function(){return n.fixFields(this.sublayer.fieldsIndex,this.parsedFilterExpressions.reduce((a,[b])=>a.concat(b.fieldNames),[]))}},{key:"availableFields",get:function(){var a=this.sublayer;const b=a.fieldsIndex;let c=this.requiredFields;if(a.outFields||a.layer.outFields)a=[...a.outFields||[],...a.layer.outFields||[]],c=[...n.unpackFieldNames(b,
a),...c];return n.fixFields(b,c)}}]);return u}(L.DefinitionExpressionSceneLayerView(G.I3SMeshView3D(F.BuildingSublayerView3DMixin(M))));g.__decorate([k.property({aliasOf:"sublayer"})],e.prototype,"i3slayer",void 0);g.__decorate([k.property()],e.prototype,"layerView",void 0);g.__decorate([k.property()],e.prototype,"suspended",void 0);g.__decorate([k.property({readOnly:!0,aliasOf:"view.qualitySettings.sceneService.3dObject.lodFactor"})],e.prototype,"lodFactor",void 0);g.__decorate([k.property({readOnly:!0})],
e.prototype,"parsedFilterExpressions",null);g.__decorate([k.property({type:E})],e.prototype,"filter",null);g.__decorate([k.property()],e.prototype,"viewFilter",void 0);g.__decorate([k.property({type:[String],readOnly:!0})],e.prototype,"filterExpressionFields",null);g.__decorate([k.property({type:[String],readOnly:!0})],e.prototype,"requiredFields",void 0);g.__decorate([k.property({type:[String],readOnly:!0})],e.prototype,"availableFields",null);return e=g.__decorate([C.subclass("esri.views.3d.layers.BuildingComponentSublayerView3D")],
e)});