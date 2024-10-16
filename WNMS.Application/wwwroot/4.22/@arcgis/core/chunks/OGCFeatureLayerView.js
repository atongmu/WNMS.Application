/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"./tslib.es6.js";import r from"../core/Error.js";import{property as s}from"../core/accessorSupport/decorators/property.js";import"../core/lang.js";import"./ensureType.js";import{subclass as t}from"../core/accessorSupport/decorators/subclass.js";const a=a=>{let i=class extends a{initialize(){const{layer:e,view:s}=this;e.source.supportsSpatialReference(s.spatialReference)||this.addResolvingPromise(Promise.reject(new r("layerview:spatial-reference-incompatible","The spatial references supported by this OGC layer are incompatible with the spatial reference of the view",{layer:e})))}get availableFields(){return this.layer.fieldsIndex.fields.map((e=>e.name))}};return e([s()],i.prototype,"layer",void 0),e([s({readOnly:!0})],i.prototype,"availableFields",null),i=e([t("esri.views.layers.OGCFeatureLayerView")],i),i};export{a as O};
