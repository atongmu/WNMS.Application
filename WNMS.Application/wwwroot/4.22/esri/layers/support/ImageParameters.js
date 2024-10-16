// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../../chunks/_rollupPluginBabelHelpers ../../chunks/tslib.es6 ../../TimeExtent ../../core/Accessor ../../core/accessorSupport/decorators/property ../../core/arrayUtils ../../core/has ../../core/accessorSupport/ensureType ../../core/accessorSupport/decorators/subclass ../../geometry/Extent ../../geometry/SpatialReference ./LayerTimeOptions ./layerUtils".split(" "),function(t,c,u,b,d,z,A,B,v,w,x,y,m){b=function(n){function h(a){a=n.call(this,a)||this;a.extent=null;a.width=null;a.height=null;
a.dpi=null;a.format=null;a.imageSpatialReference=null;a.layerDefinitions=[];a.layerOption=null;a.layerIds=null;a.transparent=!0;a.timeExtent=null;a.layerTimeOptions=null;return a}t._inheritsLoose(h,n);h.prototype.toJSON=function(){let a=this.extent;a=a&&a.clone()._normalize(!0);let p;if(this.timeExtent){var f=this.timeExtent.toJSON();const {start:e,end:g}=f;if(null!=e||null!=g)p=e===g?""+e:`${null==e?"null":e},${null==g?"null":g}`}f=this.layerOption;const q=a?a.spatialReference.wkid||JSON.stringify(a.spatialReference.toJSON()):
null,k=this.imageSpatialReference,l={dpi:this.dpi,format:this.format,transparent:this.transparent,size:null!==this.width&&null!==this.height?this.width+","+this.height:null,bbox:a?a.xmin+","+a.ymin+","+a.xmax+","+a.ymax:null,bboxSR:q,layers:f?f+":"+this.layerIds.join(","):null,layerDefs:m.serializeLayerDefinitions(this.layerDefinitions),layerTimeOptions:m.serializeTimeOptions(this.layerTimeOptions),imageSR:k?k.wkid||JSON.stringify(k.toJSON()):q,time:p},r={};Object.keys(l).filter(e=>null!==l[e]).forEach(e=>
r[e]=l[e]);return r};return h}(b);c.__decorate([d.property({type:w})],b.prototype,"extent",void 0);c.__decorate([d.property()],b.prototype,"width",void 0);c.__decorate([d.property()],b.prototype,"height",void 0);c.__decorate([d.property()],b.prototype,"dpi",void 0);c.__decorate([d.property()],b.prototype,"format",void 0);c.__decorate([d.property({type:x})],b.prototype,"imageSpatialReference",void 0);c.__decorate([d.property()],b.prototype,"layerDefinitions",void 0);c.__decorate([d.property()],b.prototype,
"layerOption",void 0);c.__decorate([d.property()],b.prototype,"layerIds",void 0);c.__decorate([d.property()],b.prototype,"transparent",void 0);c.__decorate([d.property({type:u})],b.prototype,"timeExtent",void 0);c.__decorate([d.property({type:[y]})],b.prototype,"layerTimeOptions",void 0);return b=c.__decorate([v.subclass("esri.layers.support.ImageParameters")],b)});