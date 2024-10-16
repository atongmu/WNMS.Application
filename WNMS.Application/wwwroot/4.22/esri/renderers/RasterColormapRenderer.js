// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ../core/JSONSupport ../core/accessorSupport/decorators/property ../core/arrayUtils ../core/has ../core/accessorSupport/ensureType ../core/accessorSupport/decorators/enumeration ../core/accessorSupport/decorators/subclass ./support/ColormapInfo ./support/colorRampUtils".split(" "),function(n,g,c,p,v,w,x,q,r,h,t){var d;c=d=function(k){function e(a){a=k.call(this,a)||this;a.colormapInfos=null;a.type="raster-colormap";return a}n._inheritsLoose(e,
k);e.createFromColormap=function(a,f){if(!a)return null;const u=5===a[0].length;a=[...a].sort(b=>b[0][0]-b[1][0]).map(b=>{var l;return h.fromJSON({value:b[0],color:u?b.slice(1,5):b.slice(1,4).concat([255]),label:f?null!=(l=f[b[0]])?l:"":b[0]})});return new d({colormapInfos:a})};e.createFromColorramp=function(a){a=t.convertColorRampToColormap(a,256);return d.createFromColormap(a)};var m=e.prototype;m.clone=function(){return new d({colormapInfos:this.colormapInfos.map(a=>a.toJSON())})};m.extractColormap=
function(){return this.colormapInfos.map(a=>[a.value,a.color.r,a.color.g,a.color.b,1<a.color.a?a.color.a:255*a.color.a&255]).sort((a,f)=>a[0]-f[0])};return e}(c.JSONSupport);g.__decorate([p.property({type:[h],json:{write:!0}})],c.prototype,"colormapInfos",void 0);g.__decorate([q.enumeration({rasterColormap:"raster-colormap"})],c.prototype,"type",void 0);return c=d=g.__decorate([r.subclass("esri.renderers.RasterColormapRenderer")],c)});