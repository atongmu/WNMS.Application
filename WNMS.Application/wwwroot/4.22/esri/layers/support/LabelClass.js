// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../symbols ../../core/jsonMap ../../core/JSONSupport ../../core/lang ../../core/screenUtils ../../core/accessorSupport/decorators/property ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/reader ../../core/accessorSupport/decorators/subclass ../../core/accessorSupport/decorators/writer ./LabelExpressionInfo ./labelUtils ../../symbols/support/defaults ../../symbols/support/jsonUtils".split(" "),function(B,
e,v,n,c,w,C,g,G,D,E,q,x,p,F,y){function t(h){return"map-image"===(null==h?void 0:h.type)}function z(h){var k,d;return t(h)?!(null==(k=h.capabilities)||null==(d=k.exportMap)||!d.supportsArcadeExpressionForLabeling):!1}var u;n=new n.JSONMap({esriServerPointLabelPlacementAboveCenter:"above-center",esriServerPointLabelPlacementAboveLeft:"above-left",esriServerPointLabelPlacementAboveRight:"above-right",esriServerPointLabelPlacementBelowCenter:"below-center",esriServerPointLabelPlacementBelowLeft:"below-left",
esriServerPointLabelPlacementBelowRight:"below-right",esriServerPointLabelPlacementCenterCenter:"center-center",esriServerPointLabelPlacementCenterLeft:"center-left",esriServerPointLabelPlacementCenterRight:"center-right",esriServerLinePlacementAboveAfter:"above-after",esriServerLinePlacementAboveAlong:"above-along",esriServerLinePlacementAboveBefore:"above-before",esriServerLinePlacementAboveStart:"above-start",esriServerLinePlacementAboveEnd:"above-end",esriServerLinePlacementBelowAfter:"below-after",
esriServerLinePlacementBelowAlong:"below-along",esriServerLinePlacementBelowBefore:"below-before",esriServerLinePlacementBelowStart:"below-start",esriServerLinePlacementBelowEnd:"below-end",esriServerLinePlacementCenterAfter:"center-after",esriServerLinePlacementCenterAlong:"center-along",esriServerLinePlacementCenterBefore:"center-before",esriServerLinePlacementCenterStart:"center-start",esriServerLinePlacementCenterEnd:"center-end",esriServerPolygonPlacementAlwaysHorizontal:"always-horizontal"},
{ignoreUnknown:!0});c=u=function(h){function k(a){a=h.call(this,a)||this;a.type="label";a.name=null;a.allowOverrun=!1;a.deconflictionStrategy="static";a.labelExpression=null;a.labelExpressionInfo=null;a.labelPlacement=null;a.labelPosition="curved";a.maxScale=0;a.minScale=0;a.repeatLabel=!0;a.repeatLabelDistance=null;a.symbol=F.defaultTextSymbol2D;a.useCodedValues=void 0;a.where=null;return a}B._inheritsLoose(k,h);k.evaluateWhere=function(a,f){const m=function(b,r,l){switch(r){case "\x3d":return b==
l?!0:!1;case "\x3c\x3e":return b!=l?!0:!1;case "\x3e":return b>l?!0:!1;case "\x3e\x3d":return b>=l?!0:!1;case "\x3c":return b<l?!0:!1;case "\x3c\x3d":return b<=l?!0:!1}return!1};try{if(null==a)return!0;const b=a.split(" ");if(3===b.length)return m(f[b[0]],b[1],b[2]);if(7===b.length){const r=m(f[b[0]],b[1],b[2]),l=b[3],A=m(f[b[4]],b[5],b[6]);switch(l){case "AND":return r&&A;case "OR":return r||A}}return!1}catch(b){console.log("Error.: can't parse \x3d "+a)}};var d=k.prototype;d.readLabelExpression=
function(a,f){f=f.labelExpressionInfo;if(!f||!f.value&&!f.expression)return a};d.writeLabelExpression=function(a,f,m){if(this.labelExpressionInfo)if(null!=this.labelExpressionInfo.value)a=p.templateStringToSql(this.labelExpressionInfo.value);else if(null!=this.labelExpressionInfo.expression){const b=p.getSingleFieldArcadeExpression(this.labelExpressionInfo.expression);b&&(a="["+b+"]")}null!=a&&(f[m]=a)};d.writeLabelExpressionInfo=function(a,f,m,b){if(null==a&&null!=this.labelExpression&&(b?"service"===
b.origin?0:!t(b.layer):1))a=new x({expression:this.getLabelExpressionArcade()});else if(!a)return;a=a.toJSON(b);a.expression&&(f[m]=a)};d.writeMaxScale=function(a,f){if(a||this.minScale)f.maxScale=a};d.writeMinScale=function(a,f){if(a||this.maxScale)f.minScale=a};d.getLabelExpression=function(){return p.getLabelExpression(this)};d.getLabelExpressionArcade=function(){return p.getLabelExpressionArcade(this)};d.getLabelExpressionSingleField=function(){return p.getLabelExpressionSingleField(this)};d.hash=
function(){return JSON.stringify(this)};d.clone=function(){return new u({allowOverrun:this.allowOverrun,deconflictionStrategy:this.deconflictionStrategy,labelExpression:this.labelExpression,labelExpressionInfo:w.clone(this.labelExpressionInfo),labelPosition:this.labelPosition,labelPlacement:this.labelPlacement,maxScale:this.maxScale,minScale:this.minScale,name:this.name,repeatLabel:this.repeatLabel,repeatLabelDistance:this.repeatLabelDistance,symbol:w.clone(this.symbol),where:this.where,useCodedValues:this.useCodedValues})};
return k}(c.JSONSupport);e.__decorate([g.property({type:String,json:{write:!0}})],c.prototype,"name",void 0);e.__decorate([g.property({type:Boolean,json:{write:!0,default:!1,origins:{"web-scene":{write:!1}}}})],c.prototype,"allowOverrun",void 0);e.__decorate([g.property({type:String,json:{write:!0,default:"static",origins:{"web-scene":{write:!1}}}})],c.prototype,"deconflictionStrategy",void 0);e.__decorate([g.property({type:String,json:{write:{overridePolicy(h,k,d){return this.labelExpressionInfo&&
"service"===(null==d?void 0:d.origin)&&z(d.layer)?{enabled:!1}:{allowNull:!0}}}}})],c.prototype,"labelExpression",void 0);e.__decorate([D.reader("labelExpression")],c.prototype,"readLabelExpression",null);e.__decorate([q.writer("labelExpression")],c.prototype,"writeLabelExpression",null);e.__decorate([g.property({type:x,json:{write:{overridePolicy(h,k,d){return(d?"service"===d.origin?0:!t(d.layer):1)||z(d.layer)?{allowNull:!0}:{enabled:!1}}}}})],c.prototype,"labelExpressionInfo",void 0);e.__decorate([q.writer("labelExpressionInfo")],
c.prototype,"writeLabelExpressionInfo",null);e.__decorate([g.property({type:n.apiValues,json:{type:n.jsonValues,read:n.read,write:n.write}})],c.prototype,"labelPlacement",void 0);e.__decorate([g.property({type:["curved","parallel"],json:{write:!0,origins:{"web-map":{write:!1},"web-scene":{write:!1},"portal-item":{write:!1}}}})],c.prototype,"labelPosition",void 0);e.__decorate([g.property({type:Number})],c.prototype,"maxScale",void 0);e.__decorate([q.writer("maxScale")],c.prototype,"writeMaxScale",
null);e.__decorate([g.property({type:Number})],c.prototype,"minScale",void 0);e.__decorate([q.writer("minScale")],c.prototype,"writeMinScale",null);e.__decorate([g.property({type:Boolean,json:{write:!0,origins:{"web-scene":{write:!1},"portal-item":{write:!1}}}})],c.prototype,"repeatLabel",void 0);e.__decorate([g.property({type:Number,cast:C.toPt,json:{write:!0,origins:{"web-scene":{write:!1}}}})],c.prototype,"repeatLabelDistance",void 0);e.__decorate([g.property({types:v.symbolTypesLabel,json:{origins:{"web-scene":{types:v.symbolTypesLabel3D,
write:y.write,default:null}},write:y.write,default:null}})],c.prototype,"symbol",void 0);e.__decorate([g.property({type:Boolean,json:{write:!0}})],c.prototype,"useCodedValues",void 0);e.__decorate([g.property({type:String,json:{write:!0}})],c.prototype,"where",void 0);return c=u=e.__decorate([E.subclass("esri.layers.support.LabelClass")],c)});