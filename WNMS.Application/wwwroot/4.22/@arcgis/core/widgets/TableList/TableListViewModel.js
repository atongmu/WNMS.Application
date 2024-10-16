/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"../../chunks/tslib.es6.js";import e from"../../core/Collection.js";import{E as s}from"../../chunks/Evented.js";import i from"../../core/Handles.js";import{debounce as o,throwIfNotAbortError as r}from"../../core/promiseUtils.js";import{init as a,on as l,watch as c}from"../../core/watchUtils.js";import{property as n}from"../../core/accessorSupport/decorators/property.js";import"../../core/lang.js";import"../../chunks/ensureType.js";import{subclass as m}from"../../core/accessorSupport/decorators/subclass.js";import{b as p}from"../../chunks/layerListUtils.js";import d from"./ListItem.js";import"../../chunks/ArrayPool.js";import"../../core/scheduling.js";import"../../chunks/nextTick.js";import"../../core/Error.js";import"../../chunks/Logger.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../chunks/shared.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../core/HandleOwner.js";import"../../chunks/reactiveUtils.js";import"../../chunks/Identifiable.js";import"../../core/accessorSupport/decorators/aliasOf.js";import"../../support/actions/ActionBase.js";import"../../support/actions/ActionButton.js";import"../../chunks/ActionSlider.js";import"../../support/actions/ActionToggle.js";const h="map",u="tables",_="layer-list-mode",j=e.ofType(d);let b=class extends s.EventedAccessor{constructor(t){super(t),this._handles=new i,this.listItemCreatedFunction=null,this.tableItems=new j,this.map=null,this._callListItemsCreatedDebounced=o((async()=>this._callListItemsCreated()),300)}initialize(){this._handles.add([a(this,["map","map.loaded"],(()=>this._mapHandles()))],h)}destroy(){this._handles.destroy(),this._handles=null,this.map=null,this.tableItems.removeAll()}get state(){var t;const e=null==(t=this.map)?void 0:t.loadStatus;return"string"==typeof e?"loaded"===e?"ready":"loading"===e?"loading":"disabled":"disabled"}triggerAction(t,e){t&&!t.disabled&&this.emit("trigger-action",{action:t,item:e})}_mapHandles(){const{_handles:t,map:e}=this;t.remove(u),this._compileList(),e&&t.add([l(this,"map.allTables","change",(()=>this._compileList())),a(this,"map.allTables",(()=>this._compileList())),a(this,"listItemCreatedFunction",(()=>this._compileList()))],u)}_callListItemCreated(t){if("function"==typeof this.listItemCreatedFunction){const e={item:t};this.listItemCreatedFunction.call(null,e)}}_removeAllItems(){this.tableItems.removeAll()}_getViewableTables(t){if(t)return t.filter((t=>"hide"!==p(t)))}_watchTablesListMode(t){const{_handles:e}=this;e.remove(_),t&&t.forEach((t=>{e.add(c(t,"listMode",(()=>this._compileList())),_)}))}_compileList(){var t;const e=null==(t=this.map)?void 0:t.tables;this._watchTablesListMode(e);const s=this._getViewableTables(e);s&&s.length?(this._createNewItems(s),this._removeItems(s),this._sortItems(s),this._beforeCallListItemsCreated()):this._removeAllItems()}async _beforeCallListItemsCreated(){return this._callListItemsCreatedDebounced().catch(r)}async _callListItemsCreated(){var t;await(null==(t=this.map)?void 0:t.when()),this.tableItems.forEach((t=>this._callListItemCreated(t)))}_createNewItems(t){const{tableItems:e}=this;t.forEach((t=>{e.find((e=>e.layer===t))||e.add(new d({layer:t}))}))}_removeItems(t){const{tableItems:e}=this;e.forEach((s=>{if(!s)return;t&&t.find((t=>s.layer===t))||e.remove(s)}))}_sortItems(t){const{tableItems:e}=this;e.sort(((e,s)=>{const i=t.indexOf(e.layer),o=t.indexOf(s.layer);return i>o?-1:i<o?1:0}))}};t([n()],b.prototype,"listItemCreatedFunction",void 0),t([n({type:j})],b.prototype,"tableItems",void 0),t([n()],b.prototype,"map",void 0),t([n({readOnly:!0})],b.prototype,"state",null),b=t([m("esri.widgets.TableList.TableListViewModel")],b);const f=b;export{f as default};
