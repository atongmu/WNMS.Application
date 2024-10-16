// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../chunks/tslib.es6 ../../../../geometry ../../../../core/Handles ../../../../core/accessorSupport/decorators/property ../../../../core/arrayUtils ../../../../core/has ../../../../core/accessorSupport/ensureType ../../../../core/accessorSupport/decorators/subclass ../../../support/TileTreeDebugger ../../../support/WatchUpdatingTracking ../../../../geometry/Polygon".split(" "),function(a,l,d,v,m,h,w,x,y,n,p,q,r){a.FeatureTileTree3DDebugger=
function(k){function e(b){b=k.call(this,b)||this;b.watchUpdatingTracking=new q.WatchUpdatingTracking;b.handles=new m;return b}l._inheritsLoose(e,k);var f=e.prototype;f.initialize=function(){this.handles.add(this.view.featureTiles.addClient());this.watchUpdatingTracking.addOnCollectionPropertyChange(this.view.featureTiles,"tiles",()=>this.update(),2)};f.destroy=function(){this.handles&&(this.handles.destroy(),this.handles=null);this.watchUpdatingTracking.destroy()};f.getTiles=function(){const b=c=>
{const [g,t,u]=c.lij;return r.fromExtent(this.view.featureTiles.tilingScheme.getExtentGeometry(g,t,u))};return this.view.featureTiles.tiles.toArray().sort((c,g)=>c.loadPriority-g.loadPriority).map(c=>({...c,geometry:b(c)}))};return e}(p.TileTreeDebugger);d.__decorate([h.property({readOnly:!0})],a.FeatureTileTree3DDebugger.prototype,"watchUpdatingTracking",void 0);d.__decorate([h.property({readOnly:!0,aliasOf:"watchUpdatingTracking.updating"})],a.FeatureTileTree3DDebugger.prototype,"updating",void 0);
a.FeatureTileTree3DDebugger=d.__decorate([n.subclass("esri.views.3d.layers.support.FeatureTileTree3DDebugger")],a.FeatureTileTree3DDebugger);a.default=a.FeatureTileTree3DDebugger;Object.defineProperty(a,"__esModule",{value:!0})});