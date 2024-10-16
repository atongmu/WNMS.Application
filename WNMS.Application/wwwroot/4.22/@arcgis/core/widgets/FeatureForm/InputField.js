/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"../../chunks/tslib.es6.js";import{substitute as t}from"../../intl.js";import i from"../../core/Accessor.js";import{property as r}from"../../core/accessorSupport/decorators/property.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import{subclass as o}from"../../core/accessorSupport/decorators/subclass.js";import s from"../../layers/support/CodedValueDomain.js";import"../../layers/support/Domain.js";import"../../layers/support/InheritedDomain.js";import"../../layers/support/RangeDomain.js";import{isNumericField as n,isStringField as l,isDateField as a,v as u,TypeValidationError as p,validateFieldValue as d,getFeatureEditFields as m,getFeatureGeometryFields as c,D as h,NumericRangeValidationError as y,g,getFieldRange as f}from"../../layers/support/fieldUtils.js";import"../../chunks/number.js";import"../../chunks/jsonMap.js";import"../../chunks/object.js";import"../../chunks/locale.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/string.js";import"../../chunks/messages.js";import"../../core/Error.js";import"../../core/promiseUtils.js";import"../../request.js";import"../../kernel.js";import"../../core/urlUtils.js";import"../../chunks/assets.js";import"../../chunks/deprecate.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../chunks/enumeration.js";import"../../chunks/JSONSupport.js";import"../../chunks/arcadeOnDemand.js";import"../../geometry.js";import"../../geometry/Extent.js";import"../../geometry/Geometry.js";import"../../chunks/reader.js";import"../../geometry/SpatialReference.js";import"../../chunks/writer.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../geometry/support/webMercatorUtils.js";import"../../chunks/Ellipsoid.js";import"../../geometry/Multipoint.js";import"../../chunks/zmUtils.js";import"../../geometry/Polygon.js";import"../../chunks/extentUtils.js";import"../../geometry/Polyline.js";import"../../chunks/typeUtils.js";import"../../geometry/support/jsonUtils.js";const v={type:"number"},j={type:"number",intlOptions:{notation:"scientific"}},b={type:"date",intlOptions:{day:"2-digit",month:"2-digit",year:"numeric",hour:"numeric",minute:"numeric",second:"numeric"}};let _=class extends i{constructor(e){super(e),this.arcade=null,this.config=null,this.feature=null,this.field=null,this.initialFeature=null,this.layer=null,this.description=null,this.editorType=null,this.error=null,this.format=null,this.group=null,this.hint=null,this.messages=null,this.name=null}get compiledFunc(){var e;const{arcade:t}=this;return t&&t.arcadeUtils.createFunction(null==(e=this.config)?void 0:e.visibilityExpression)}get compiledRequiredFunc(){var e;const{arcade:t}=this;return t&&t.arcadeUtils.createFunction(null==(e=this.config)?void 0:e.requiredExpression)}get evaluatedRequirement(){const e=this.compiledRequiredFunc;if(!e)return null;const{arcade:t}=this;return t.arcadeUtils.executeFunction(e,t.arcadeUtils.createExecContext(this.feature))}get evaluatedVisibility(){const e=this.compiledFunc;if(!e)return null;const{arcade:t}=this;return t.arcadeUtils.executeFunction(e,t.arcadeUtils.createExecContext(this.feature))}get domain(){var e,t;const{typeIdField:i}=this.layer,r=i===this.name,o=null==(e=this.field)?void 0:e.domain;if(r&&!o)return new s({name:"__internal-type-based-coded-value-domain__",codedValues:this.layer.types.map((({id:e,name:t})=>({code:e,name:t})))});const{feature:n}=this,l=i&&this.layer.getFieldDomain(this.name,{feature:n})||o,a=null==(t=this.config)?void 0:t.domain;return this._isDomainCompatible(a)?a:l}get editable(){var e,t;const{config:i,field:r,layer:o,name:s}=this,n=o.capabilities.operations.supportsEditing&&r.editable,l=null==(e=o.popupTemplate)||null==(t=e.fieldInfos)?void 0:t.find((({fieldName:e})=>e===s)),a=i?!1!==i.editable:!1!==(null==l?void 0:l.isEditable);return n&&a}get errorMessage(){return this._toErrorMessage()}get includeTime(){var e;const t=null==(e=this.config)?void 0:e.includeTime;return void 0===t||t}get label(){var e;return(null==(e=this.config)?void 0:e.label)||this.field.alias||this.field.name}get maxLength(){var e,t;if("date"===this.type)return-1;const i=null==(e=this.field)?void 0:e.length,r=null==(t=this.config)?void 0:t.maxLength;return!isNaN(r)&&r>=-1&&(-1===i||r<=i)?r:i}get minLength(){var e,t;if("date"===this.type)return-1;const i=null==(e=this.field)?void 0:e.length,r=null==(t=this.config)?void 0:t.minLength;return!isNaN(r)&&r>=-1&&(-1===i||r<=i)?r:-1}get required(){var e,t;if(!this.editable)return!1;if(!(null==(e=this.field)?void 0:e.nullable))return!0;if("boolean"==typeof this.evaluatedRequirement)return this.evaluatedRequirement;return!0===(null==(t=this.config)?void 0:t.required)}get type(){const{field:e}=this;return n(e)?"number":l(e)?"text":a(e)?"date":"unsupported"}get valid(){const e=this.editable?this._validate():null;return this._set("error",e),null===e}get value(){return this._get("value")}set value(e){this.notifyChange("evaluatedVisibility"),this._set("value",e)}get visible(){if(this._isEditorField())return!1;return"boolean"==typeof this.evaluatedVisibility?this.evaluatedVisibility:!!this.config||this._shownByDefault()}_isDomainCompatible(e){const{field:t}=this;if("coded-value"===(null==e?void 0:e.type)){const i=typeof e.codedValues[0].code;if("string"===i&&l(t)||"number"===i&&n(t))return!0}return!!("range"===(null==e?void 0:e.type)&&n(t)||a(t))}_validate(){const{domain:e,field:t,initialFeature:i,minLength:r,required:o,type:s,valid:n,value:l}=this,a=o&&null===l,m=void 0!==n;return!a&&i.getAttribute(t.name)===l&&m?null:"text"===s&&r>-1&&"string"==typeof l&&l.length<r?"length-validation-error::too-short":e?null!==l||o?u(e,l):null:a?p.INVALID_TYPE:d(t,l)}_shownByDefault(){var e;const t=null==(e=this.field)?void 0:e.type;return"oid"!==t&&"global-id"!==t&&!this._isGeometryField()}_isEditorField(){return m(this.layer).indexOf(this.name)>-1}_isGeometryField(){return c(this.layer).indexOf(this.name)>-1}_toErrorMessage(){const{domain:e,field:i,messages:r,minLength:o,value:s,required:n,type:l}=this,a=this.error;if(n&&null===s)return r.validationErrors.cannotBeNull;if(a===h.VALUE_OUT_OF_RANGE||a===y.OUT_OF_RANGE){const o=g(e)||f(i);return t(r.validationErrors.outsideRange,o,{format:{max:"date"===l?b:o.isInteger?v:j,min:"date"===l?b:o.isInteger?v:j}})}return a===h.INVALID_CODED_VALUE?r.validationErrors.invalidCodedValue:a===p.INVALID_TYPE?r.validationErrors.invalidType:"length-validation-error::too-short"===a?t(r.validationErrors.tooShort,{min:o}):null}};e([r()],_.prototype,"arcade",void 0),e([r()],_.prototype,"compiledFunc",null),e([r()],_.prototype,"compiledRequiredFunc",null),e([r()],_.prototype,"config",void 0),e([r()],_.prototype,"evaluatedRequirement",null),e([r()],_.prototype,"evaluatedVisibility",null),e([r()],_.prototype,"feature",void 0),e([r()],_.prototype,"field",void 0),e([r()],_.prototype,"initialFeature",void 0),e([r()],_.prototype,"layer",void 0),e([r({aliasOf:"config.description"})],_.prototype,"description",void 0),e([r()],_.prototype,"domain",null),e([r()],_.prototype,"editable",null),e([r({aliasOf:"config.editorType"})],_.prototype,"editorType",void 0),e([r({readOnly:!0})],_.prototype,"error",void 0),e([r({dependsOn:["error","messages","value"]})],_.prototype,"errorMessage",null),e([r({aliasOf:"config.format"})],_.prototype,"format",void 0),e([r()],_.prototype,"group",void 0),e([r({aliasOf:"config.hint"})],_.prototype,"hint",void 0),e([r()],_.prototype,"includeTime",null),e([r()],_.prototype,"label",null),e([r()],_.prototype,"maxLength",null),e([r()],_.prototype,"minLength",null),e([r()],_.prototype,"messages",void 0),e([r({aliasOf:"field.name"})],_.prototype,"name",void 0),e([r()],_.prototype,"required",null),e([r()],_.prototype,"type",null),e([r()],_.prototype,"valid",null),e([r({value:null})],_.prototype,"value",null),e([r()],_.prototype,"visible",null),_=e([o("esri.widgets.FeatureForm.InputField")],_);const k=_;export{k as default};
