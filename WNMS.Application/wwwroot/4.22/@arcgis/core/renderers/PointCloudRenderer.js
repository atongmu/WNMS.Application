/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as o}from"../chunks/tslib.es6.js";import{s as e,J as t}from"../chunks/jsonMap.js";import{a as r}from"../chunks/JSONSupport.js";import{clone as s}from"../core/lang.js";import{property as i}from"../core/accessorSupport/decorators/property.js";import"../chunks/ensureType.js";import{subclass as n}from"../core/accessorSupport/decorators/subclass.js";import{P as l,a as p}from"../chunks/PointSizeSplatAlgorithm.js";import{e as u}from"../chunks/enumeration.js";import"../chunks/object.js";import"../core/Accessor.js";import"../chunks/deprecate.js";import"../chunks/Logger.js";import"../config.js";import"../chunks/string.js";import"../chunks/metadata.js";import"../chunks/handleUtils.js";import"../chunks/ArrayPool.js";import"../core/scheduling.js";import"../chunks/nextTick.js";import"../core/promiseUtils.js";import"../core/Error.js";var a;let c=a=class extends r{constructor(){super(...arguments),this.field=null,this.minValue=0,this.maxValue=255}clone(){return new a({field:this.field,minValue:this.minValue,maxValue:this.maxValue})}};o([i({type:String,json:{write:!0}})],c.prototype,"field",void 0),o([i({type:Number,nonNullable:!0,json:{write:!0}})],c.prototype,"minValue",void 0),o([i({type:Number,nonNullable:!0,json:{write:!0}})],c.prototype,"maxValue",void 0),c=a=o([n("esri.renderers.support.pointCloud.ColorModulation")],c);const d=c;var m;let h=m=class extends l{constructor(){super(...arguments),this.type="fixed-size",this.size=0,this.useRealWorldSymbolSizes=null}clone(){return new m({size:this.size,useRealWorldSymbolSizes:this.useRealWorldSymbolSizes})}};o([u({pointCloudFixedSizeAlgorithm:"fixed-size"})],h.prototype,"type",void 0),o([i({type:Number,nonNullable:!0,json:{write:!0}})],h.prototype,"size",void 0),o([i({type:Boolean,json:{write:!0}})],h.prototype,"useRealWorldSymbolSizes",void 0),h=m=o([n("esri.renderers.support.pointCloud.PointSizeFixedSizeAlgorithm")],h);const y={key:"type",base:l,typeMap:{"fixed-size":h,splat:p}},j=e()({pointCloudClassBreaksRenderer:"point-cloud-class-breaks",pointCloudRGBRenderer:"point-cloud-rgb",pointCloudStretchRenderer:"point-cloud-stretch",pointCloudUniqueValueRenderer:"point-cloud-unique-value"});let b=class extends r{constructor(o){super(o),this.type=void 0,this.pointSizeAlgorithm=null,this.colorModulation=null,this.pointsPerInch=10}clone(){return console.warn(".clone() is not implemented for "+this.declaredClass),null}cloneProperties(){return{pointSizeAlgorithm:s(this.pointSizeAlgorithm),colorModulation:s(this.colorModulation),pointsPerInch:s(this.pointsPerInch)}}};o([i({type:j.apiValues,readOnly:!0,nonNullable:!0,json:{type:j.jsonValues,read:!1,write:j.write}})],b.prototype,"type",void 0),o([i({types:y,json:{write:!0}})],b.prototype,"pointSizeAlgorithm",void 0),o([i({type:d,json:{write:!0}})],b.prototype,"colorModulation",void 0),o([i({json:{write:!0},nonNullable:!0,type:Number})],b.prototype,"pointsPerInch",void 0),b=o([n("esri.renderers.PointCloudRenderer")],b),function(o){o.fieldTransformTypeKebabDict=new t({none:"none",lowFourBit:"low-four-bit",highFourBit:"high-four-bit",absoluteValue:"absolute-value",moduloTen:"modulo-ten"})}(b||(b={}));const f=b;export{f as default};
