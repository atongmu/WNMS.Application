/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"./tslib.es6.js";import{E as t}from"./Evented.js";import i from"../core/Handles.js";import{i as o}from"../core/lang.js";import{watch as n,pausable as s}from"../core/watchUtils.js";import{property as r}from"../core/accessorSupport/decorators/property.js";import"./ensureType.js";import{subclass as a}from"../core/accessorSupport/decorators/subclass.js";let c=class extends t.EventedAccessor{constructor(e){super(e),this._anchorHandles=new i,this.location=null,this.screenLocation=null,this.screenLocationEnabled=!1,this.view=null,this._anchorHandles.add([n(this,["screenLocationEnabled","location","view.size","view.stationary"],(()=>this._updateScreenPointAndHandle())),n(this,["view","view.ready"],(()=>this._wireUpView()))])}destroy(){this.view=null,this._anchorHandles&&this._anchorHandles.destroy(),this._anchorHandles=null,this._viewpointHandle=null}_wireUpView(){const e="view";this._anchorHandles.remove(e),this._viewpointHandle=null;if(!this.get("view.ready"))return;this._setScreenLocation();const{view:t}=this,i="3d"===t.type?"camera":"viewpoint",o=s(t,i,(()=>this._viewpointChange()));this._anchorHandles.add(o,e),this._viewpointHandle=o,this._toggleWatchingViewpoint()}_viewpointChange(){this._setScreenLocation(),this.emit("view-change")}_updateScreenPointAndHandle(){this._setScreenLocation(),this._toggleWatchingViewpoint()}_toggleWatchingViewpoint(){const{_viewpointHandle:e,location:t,screenLocationEnabled:i}=this;if(!e)return;t&&i?e.resume():e.pause()}_setScreenLocation(){const{location:e,view:t,screenLocationEnabled:i}=this,n=this.get("view.ready"),s=i&&n&&o(e)?t.toScreen(e):null;this._set("screenLocation",s)}};e([r()],c.prototype,"location",void 0),e([r({readOnly:!0})],c.prototype,"screenLocation",void 0),e([r()],c.prototype,"screenLocationEnabled",void 0),e([r()],c.prototype,"view",void 0),c=e([a("esri.widgets.support.AnchorElementViewModel")],c);const l=c;export{l as A};
