/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{c as t,h as e,H as i,p as s}from"../widgets/Widget.js";import{f as a,b as o,c as n}from"./dom.js";import{h as r}from"./form.js";import{c,g as l,d as h}from"./label.js";import"./tslib.es6.js";import"../intl.js";import"./number.js";import"./jsonMap.js";import"./object.js";import"../core/lang.js";import"./locale.js";import"./Logger.js";import"../config.js";import"./string.js";import"./messages.js";import"../core/Error.js";import"../core/promiseUtils.js";import"../request.js";import"../kernel.js";import"../core/urlUtils.js";import"./assets.js";import"./deprecate.js";import"./domUtils.js";import"./Evented.js";import"../core/Accessor.js";import"../core/accessorSupport/decorators/property.js";import"./ensureType.js";import"./metadata.js";import"./handleUtils.js";import"./ArrayPool.js";import"../core/accessorSupport/decorators/subclass.js";import"../core/scheduling.js";import"./nextTick.js";import"../core/Handles.js";import"../core/Collection.js";import"./shared.js";import"./Promise.js";import"./uuid.js";import"../core/watchUtils.js";import"../core/accessorSupport/decorators/cast.js";import"./projector.js";import"./widgetUtils.js";import"./jsxWidgetSupport.js";
/*!
 * All material copyright ESRI, All Rights Reserved, unless otherwise specified.
 * See https://github.com/Esri/calcite-components/blob/master/LICENSE.md for details.
 */let m=class extends HTMLElement{constructor(){super(),this.__registerHost(),this.__attachShadow(),this.calciteInternalCheckboxBlur=t(this,"calciteInternalCheckboxBlur",7),this.calciteCheckboxChange=t(this,"calciteCheckboxChange",7),this.calciteInternalCheckboxFocus=t(this,"calciteInternalCheckboxFocus",7),this.checked=!1,this.disabled=!1,this.hovered=!1,this.indeterminate=!1,this.name="",this.scale="m",this.checkedPath="M5.5 12L2 8.689l.637-.636L5.5 10.727l8.022-7.87.637.637z",this.indeterminatePath="M13 8v1H3V8z",this.focused=!1,this.getPath=()=>this.indeterminate?this.indeterminatePath:this.checked?this.checkedPath:"",this.toggle=()=>{this.disabled||(this.checked=!this.checked,a(this.input),this.indeterminate=!1,this.calciteCheckboxChange.emit())},this.clickHandler=()=>{this.toggle()},this.formResetHandler=()=>{this.checked=this.initialChecked}}checkedWatcher(t){this.input.checked=t}disabledChanged(t){this.input.disabled=t}nameChanged(t){this.input.name=t}async setFocus(){a(this.input)}mouseenter(){this.hovered=!0}mouseleave(){this.hovered=!1}onInputBlur(){this.focused=!1,this.calciteInternalCheckboxBlur.emit(!1)}onInputFocus(){this.focused=!0,this.calciteInternalCheckboxFocus.emit(!0)}onLabelClick(){this.toggle()}connectedCallback(){this.guid=this.el.id||`calcite-checkbox-${o()}`,this.initialChecked=this.checked,this.renderHiddenCheckboxInput();const t=n(this.el,"form");t&&t.addEventListener("reset",this.formResetHandler),c(this)}componentDidLoad(){this.input.setAttribute("aria-label",l(this))}disconnectedCallback(){this.input.parentNode.removeChild(this.input);const t=n(this.el,"form");t&&t.removeEventListener("reset",this.formResetHandler),h(this)}renderHiddenCheckboxInput(){this.input=document.createElement("input"),this.checked&&this.input.setAttribute("checked",""),this.input.disabled=this.disabled,this.input.id=`${this.guid}-input`,this.input.name=this.name,this.input.onblur=this.onInputBlur.bind(this),this.input.onfocus=this.onInputFocus.bind(this),this.input.style.cssText=r,this.input.type="checkbox",this.input.setAttribute("aria-label",l(this)),this.value&&(this.input.value=null!=this.value?this.value.toString():""),this.el.appendChild(this.input)}render(){return e(i,{onClick:this.clickHandler},e("div",{class:{focused:this.focused}},e("svg",{class:"check-svg",viewBox:"0 0 16 16"},e("path",{d:this.getPath()})),e("slot",null)))}get el(){return this}static get watchers(){return{checked:["checkedWatcher"],disabled:["disabledChanged"],name:["nameChanged"]}}static get style(){return"@-webkit-keyframes in{0%{opacity:0}100%{opacity:1}}@keyframes in{0%{opacity:0}100%{opacity:1}}@-webkit-keyframes in-down{0%{opacity:0;-webkit-transform:translate3D(0, -5px, 0);transform:translate3D(0, -5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@keyframes in-down{0%{opacity:0;-webkit-transform:translate3D(0, -5px, 0);transform:translate3D(0, -5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@-webkit-keyframes in-up{0%{opacity:0;-webkit-transform:translate3D(0, 5px, 0);transform:translate3D(0, 5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@keyframes in-up{0%{opacity:0;-webkit-transform:translate3D(0, 5px, 0);transform:translate3D(0, 5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@-webkit-keyframes in-scale{0%{opacity:0;-webkit-transform:scale3D(0.95, 0.95, 1);transform:scale3D(0.95, 0.95, 1)}100%{opacity:1;-webkit-transform:scale3D(1, 1, 1);transform:scale3D(1, 1, 1)}}@keyframes in-scale{0%{opacity:0;-webkit-transform:scale3D(0.95, 0.95, 1);transform:scale3D(0.95, 0.95, 1)}100%{opacity:1;-webkit-transform:scale3D(1, 1, 1);transform:scale3D(1, 1, 1)}}:root{--calcite-animation-timing:300ms}.calcite-animate{opacity:0;-webkit-animation-fill-mode:both;animation-fill-mode:both;-webkit-animation-duration:var(--calcite-animation-timing);animation-duration:var(--calcite-animation-timing)}.calcite-animate__in{-webkit-animation-name:in;animation-name:in}.calcite-animate__in-down{-webkit-animation-name:in-down;animation-name:in-down}.calcite-animate__in-up{-webkit-animation-name:in-up;animation-name:in-up}.calcite-animate__in-scale{-webkit-animation-name:in-scale;animation-name:in-scale}:root{--calcite-popper-transition:150ms ease-in-out}:host([hidden]){display:none}:host([scale=s]){--calcite-checkbox-size:0.75rem}:host([scale=m]){--calcite-checkbox-size:var(--calcite-font-size--1)}:host([scale=l]){--calcite-checkbox-size:1rem}:host{display:-ms-inline-flexbox;display:inline-flex;cursor:pointer;position:relative;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;-webkit-tap-highlight-color:transparent}:host .check-svg{overflow:hidden;display:block;background-color:var(--calcite-ui-foreground-1);pointer-events:none;-webkit-box-sizing:border-box;box-sizing:border-box;-webkit-transition-property:all;transition-property:all;-webkit-transition-duration:150ms;transition-duration:150ms;-webkit-transition-timing-function:cubic-bezier(0.4, 0, 0.2, 1);transition-timing-function:cubic-bezier(0.4, 0, 0.2, 1);stroke:currentColor;stroke-width:1;fill:currentColor;width:var(--calcite-checkbox-size);height:var(--calcite-checkbox-size);-webkit-box-shadow:inset 0 0 0 1px var(--calcite-ui-border-input);box-shadow:inset 0 0 0 1px var(--calcite-ui-border-input);color:var(--calcite-ui-background)}:host ::slotted(input){width:var(--calcite-checkbox-size);height:var(--calcite-checkbox-size)}:host(:hover) .check-svg,:host([hovered]) .check-svg{-webkit-box-shadow:inset 0 0 0 2px var(--calcite-ui-brand);box-shadow:inset 0 0 0 2px var(--calcite-ui-brand)}:host([checked]) .check-svg,:host([indeterminate]) .check-svg{background-color:var(--calcite-ui-brand);-webkit-box-shadow:inset 0 0 0 1px var(--calcite-ui-brand);box-shadow:inset 0 0 0 1px var(--calcite-ui-brand)}:host .focused .check-svg{-webkit-box-shadow:inset 0 0 0 1px var(--calcite-ui-brand), 0 0 0 2px var(--calcite-ui-foreground-1), 0 0 0 4px var(--calcite-ui-brand);box-shadow:inset 0 0 0 1px var(--calcite-ui-brand), 0 0 0 2px var(--calcite-ui-foreground-1), 0 0 0 4px var(--calcite-ui-brand);-webkit-transition:150ms ease-in-out;transition:150ms ease-in-out}:host([disabled]){cursor:default;opacity:var(--calcite-ui-opacity-disabled);pointer-events:none}"}};function d(){["calcite-checkbox"].forEach((t=>{if("calcite-checkbox"===t)customElements.get(t)||customElements.define(t,m)}))}m=s(m,[1,"calcite-checkbox",{checked:[1540],disabled:[516],guid:[1537],hovered:[1540],indeterminate:[1540],label:[1],name:[513],scale:[513],value:[8],focused:[32],setFocus:[64]},[[1,"mouseenter","mouseenter"],[1,"mouseleave","mouseleave"]]]),d();
/*!
 * All material copyright ESRI, All Rights Reserved, unless otherwise specified.
 * See https://github.com/Esri/calcite-components/blob/master/LICENSE.md for details.
 */
const p=m,u=d;export{p as CalciteCheckbox,u as defineCustomElement};
