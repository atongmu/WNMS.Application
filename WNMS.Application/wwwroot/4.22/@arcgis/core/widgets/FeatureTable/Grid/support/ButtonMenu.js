/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"../../../../chunks/tslib.es6.js";import{e as t}from"../../../../core/promiseUtils.js";import{HandleOwnerMixin as s}from"../../../../core/HandleOwner.js";import{i as o}from"../../../../core/lang.js";import{watch as n}from"../../../../core/watchUtils.js";import{aliasOf as i}from"../../../../core/accessorSupport/decorators/aliasOf.js";import"../../../../chunks/ensureType.js";import{property as r}from"../../../../core/accessorSupport/decorators/property.js";import{subclass as a}from"../../../../core/accessorSupport/decorators/subclass.js";import l from"../../../Widget.js";import u from"./ButtonMenuViewModel.js";import{P as c}from"../../../../chunks/Popover.js";import{i as d,h as m}from"../../../../chunks/widgetUtils.js";import"../../../../chunks/Logger.js";import{t as p}from"../../../../chunks/jsxFactory.js";import"../../../../core/Error.js";import"../../../../chunks/object.js";import"../../../../config.js";import"../../../../chunks/string.js";import"../../../../core/Accessor.js";import"../../../../chunks/deprecate.js";import"../../../../chunks/metadata.js";import"../../../../chunks/handleUtils.js";import"../../../../chunks/ArrayPool.js";import"../../../../core/scheduling.js";import"../../../../chunks/nextTick.js";import"../../../../core/Handles.js";import"../../../../core/Collection.js";import"../../../../chunks/Evented.js";import"../../../../chunks/shared.js";import"../../../../chunks/reactiveUtils.js";import"../../../../intl.js";import"../../../../chunks/number.js";import"../../../../chunks/jsonMap.js";import"../../../../chunks/locale.js";import"../../../../chunks/messages.js";import"../../../../request.js";import"../../../../kernel.js";import"../../../../core/urlUtils.js";import"../../../../chunks/assets.js";import"../../../../chunks/domUtils.js";import"../../../../chunks/Promise.js";import"../../../../chunks/uuid.js";import"../../../../core/accessorSupport/decorators/cast.js";import"../../../../chunks/projector.js";import"../../../../chunks/jsxWidgetSupport.js";import"./ButtonMenuItem.js";const h="esri-button-menu",_="esri-button-menu__content",b="esri-button-menu__item-wrapper",j="esri-button-menu__icon",k="esri-button-menu__item",v="esri-button-menu__item-label",f="esri-button-menu__item-label-content",C="esri-button-menu__item--selectable",M="esri-button-menu__item--selected",g="esri-button-menu__checkbox",y="esri-button-menu__embedded-content-wrapper",w="esri-button-menu__button",I="esri-button-menu__button--selected",N="esri-icon-menu",x="esri-widget";let $=class extends(s(l)){constructor(e,t){super(e,t),this._menuContentNode=null,this._popover=null,this._rootNode=null,this.iconClass=null,this.items=null,this.label=null,this.open=!1,this.viewModel=new u,this._handleOutsideClick=this._handleOutsideClick.bind(this)}postInitialize(){this._popover=new c({owner:this,open:!!this.open,placement:d(this.container)?"bottom-start":"bottom-end",renderContentFunction:this.renderMenuContent,anchorElement:this._rootNode}),this.handles.add([n(this,"open",(e=>this._popover.set("open",e)))]),document.addEventListener("click",this._handleOutsideClick)}destroy(){var e;null==(e=this._popover)||e.destroy(),this._popover=null,document.removeEventListener("click",this._handleOutsideClick)}_handleOutsideClick(e){var t,s;if(!this.open||!this._rootNode||!this._menuContentNode)return;const o=e.target;null!=(t=this._menuContentNode)&&t.contains(o)||null!=(s=this._rootNode)&&s.contains(o)||(this.open=!1)}render(){return p("div",{afterCreate:this._afterRootNodeCreate,bind:this,"data-node-ref":"_rootNode",class:this.classes(h,x),key:"menu"},this.renderMenuButton())}renderMenuButton(){const{iconClass:e,id:t,label:s,open:o}=this,n=this.classes([w,e||N,o?I:null]);return p("button",{"aria-pressed":o.toString(),"aria-controls":`${t}-menu`,"aria-expanded":o.toString(),"aria-haspopup":"true","aria-label":s,bind:this,class:n,id:`${t}-button`,title:s,selected:o,onclick:this._toggleOpen,tabindex:"0",type:"button"})}renderMenuContent(){var e;const{id:t,open:s}=this;return p("div",{afterCreate:this._afterMenuContentNodeCreate,bind:this,class:_,"data-node-ref":"_menuContentNode",key:"esri-button-menu-content",hidden:!s},p("ul",{"aria-labelledby":`${t}-button`,bind:this,class:b,id:`${t}-menu`,role:"menu"},(null==(e=this.items)?void 0:e.length)&&this.renderItems()))}renderItems(){var e;return null==(e=this.items)?void 0:e.map(((e,t)=>this.renderItem(e,t)))}renderItem(e,t,s){var n;const i=o(s)?`${this.id}-menu-item-${t}-${s}`:`${this.id}-menu-item-${t}`,r=`${i}-label`,a=this.classes(k,e.selectionEnabled?C:null,e.selectionEnabled&&e.selected?M:null);return p("li",{afterCreate:this._afterMenuItemCreate,bind:this,class:a,"data-item-index":t,"data-item-subIndex":s,for:i,key:i,onkeydown:t=>this._handleMenuItemKeydown(t,e),onclick:t=>this._handleMenuItemInteraction(t,e),role:"menuitem",tabindex:"0"},p("input",{disabled:!0,checked:e.selected,class:g,id:i,name:i,tabindex:"-1",type:"checkbox"}),p("label",{bind:this,class:this.classes(w,v),for:i,id:r},p("span",{class:this.classes(j,e.iconClass),"aria-hidden":"true"}),p("span",{class:f},e.label)),p("ul",{"aria-labelledby":r,class:y,id:`${this.id}-submenu`,role:"menu"},null==e||null==(n=e.items)?void 0:n.map(((e,s)=>this.renderItem(e,t,s)))))}_afterRootNodeCreate(e){var t;this._rootNode=e,null==(t=this._popover)||t.set("anchorElement",(()=>e))}_handleMenuItemInteraction(e,t){t.selected=!t.selected,t.open=!(!t.selected||!t.items),t.autoCloseMenu&&this.set("open",!1),t.clickFunction&&t.clickFunction({event:e,item:t}),e.stopPropagation()}_handleMenuItemKeydown(e,s){const o=t(e);m(o)&&this._handleMenuItemInteraction(e,s),"Escape"===o&&(this.open=!1,e.preventDefault(),e.stopPropagation())}_afterMenuContentNodeCreate(e){this._menuContentNode=e,e.focus()}_toggleOpen(){this.open=!this.open}_afterMenuItemCreate(e){0===e["data-item-index"]&&e.focus()}};e([r()],$.prototype,"iconClass",void 0),e([i("viewModel.items")],$.prototype,"items",void 0),e([r()],$.prototype,"label",void 0),e([i("viewModel.open")],$.prototype,"open",void 0),e([r()],$.prototype,"viewModel",void 0),$=e([a("esri.widgets.FeatureTable.Grid.support.ButtonMenu")],$);const E=$;export{E as default};
