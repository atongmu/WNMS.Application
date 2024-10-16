/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"../../chunks/tslib.es6.js";import o from"../../core/Accessor.js";import r from"../../core/Collection.js";import{HandleOwnerMixin as e}from"../../core/HandleOwner.js";import{I as s}from"../../chunks/Identifiable.js";import{init as i,watch as c}from"../../core/watchUtils.js";import{aliasOf as a}from"../../core/accessorSupport/decorators/aliasOf.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import{property as n}from"../../core/accessorSupport/decorators/property.js";import{subclass as p}from"../../core/accessorSupport/decorators/subclass.js";import l from"../../support/actions/ActionBase.js";import m from"../../support/actions/ActionButton.js";import{A as h}from"../../chunks/ActionSlider.js";import d from"../../support/actions/ActionToggle.js";import"../../chunks/deprecate.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/promiseUtils.js";import"../../core/Error.js";import"../../chunks/Evented.js";import"../../chunks/shared.js";import"../../core/Handles.js";import"../../chunks/reactiveUtils.js";var u;const j=r.ofType({key:"type",defaultKeyValue:"button",base:l,typeMap:{button:m,toggle:d,slider:h}}),y=r.ofType(j);let f=u=class extends(s(e(o))){constructor(t){super(t),this.actionsSections=new y,this.actionsOpen=!1,this.error=null,this.layer=null}initialize(){this.handles.add([i(this,"layer",(t=>this._watchLayerProperties(t)))])}get title(){const t=this.get("layer.layer");return(!t||t&&this.get("layer.layer.loaded"))&&this.get("layer.title")||""}set title(t){void 0!==t?this._override("title",t):this._clearOverride("title")}clone(){return new u({actionsSections:this.actionsSections.clone(),actionsOpen:this.actionsOpen,layer:this.layer,title:this.title})}_watchLayerProperties(t){this.handles&&(this.handles.remove("layer"),t&&this.handles.add(c(t,"listMode",(()=>this._watchLayerProperties(t))),"layer"))}};t([n({type:y})],f.prototype,"actionsSections",void 0),t([n()],f.prototype,"actionsOpen",void 0),t([a("layer.loadError?")],f.prototype,"error",void 0),t([n()],f.prototype,"layer",void 0),t([n()],f.prototype,"title",null),f=u=t([p("esri.widgets.TableList.ListItem")],f);const k=f;export{k as default};
