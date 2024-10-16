// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./componentsUtils","./dom","./form","./label"],function(f,b,c,h,d){function g(){["calcite-checkbox"].forEach(a=>{switch(a){case "calcite-checkbox":customElements.get(a)||customElements.define(a,e)}})}let e=class extends HTMLElement{constructor(){super();this.__registerHost();this.__attachShadow();this.calciteInternalCheckboxBlur=b.createEvent(this,"calciteInternalCheckboxBlur",7);this.calciteCheckboxChange=b.createEvent(this,"calciteCheckboxChange",7);this.calciteInternalCheckboxFocus=
b.createEvent(this,"calciteInternalCheckboxFocus",7);this.indeterminate=this.hovered=this.disabled=this.checked=!1;this.name="";this.scale="m";this.checkedPath="M5.5 12L2 8.689l.637-.636L5.5 10.727l8.022-7.87.637.637z";this.indeterminatePath="M13 8v1H3V8z";this.focused=!1;this.getPath=()=>this.indeterminate?this.indeterminatePath:this.checked?this.checkedPath:"";this.toggle=()=>{this.disabled||(this.checked=!this.checked,c.focusElement(this.input),this.indeterminate=!1,this.calciteCheckboxChange.emit())};
this.clickHandler=()=>{this.toggle()};this.formResetHandler=()=>{this.checked=this.initialChecked}}checkedWatcher(a){this.input.checked=a}disabledChanged(a){this.input.disabled=a}nameChanged(a){this.input.name=a}async setFocus(){c.focusElement(this.input)}mouseenter(){this.hovered=!0}mouseleave(){this.hovered=!1}onInputBlur(){this.focused=!1;this.calciteInternalCheckboxBlur.emit(!1)}onInputFocus(){this.focused=!0;this.calciteInternalCheckboxFocus.emit(!0)}onLabelClick(){this.toggle()}connectedCallback(){this.guid=
this.el.id||`calcite-checkbox-${c.guid()}`;this.initialChecked=this.checked;this.renderHiddenCheckboxInput();const a=c.closestElementCrossShadowBoundary(this.el,"form");a&&a.addEventListener("reset",this.formResetHandler);d.connectLabel(this)}componentDidLoad(){this.input.setAttribute("aria-label",d.getLabelText(this))}disconnectedCallback(){this.input.parentNode.removeChild(this.input);const a=c.closestElementCrossShadowBoundary(this.el,"form");a&&a.removeEventListener("reset",this.formResetHandler);
d.disconnectLabel(this)}renderHiddenCheckboxInput(){this.input=document.createElement("input");this.checked&&this.input.setAttribute("checked","");this.input.disabled=this.disabled;this.input.id=`${this.guid}-input`;this.input.name=this.name;this.input.onblur=this.onInputBlur.bind(this);this.input.onfocus=this.onInputFocus.bind(this);this.input.style.cssText=h.hiddenInputStyle;this.input.type="checkbox";this.input.setAttribute("aria-label",d.getLabelText(this));this.value&&(this.input.value=null!=
this.value?this.value.toString():"");this.el.appendChild(this.input)}render(){return b.h(b.Host,{onClick:this.clickHandler},b.h("div",{class:{focused:this.focused}},b.h("svg",{class:"check-svg",viewBox:"0 0 16 16"},b.h("path",{d:this.getPath()})),b.h("slot",null)))}get el(){return this}static get watchers(){return{checked:["checkedWatcher"],disabled:["disabledChanged"],name:["nameChanged"]}}static get style(){return"@-webkit-keyframes in{0%{opacity:0}100%{opacity:1}}@keyframes in{0%{opacity:0}100%{opacity:1}}@-webkit-keyframes in-down{0%{opacity:0;-webkit-transform:translate3D(0, -5px, 0);transform:translate3D(0, -5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@keyframes in-down{0%{opacity:0;-webkit-transform:translate3D(0, -5px, 0);transform:translate3D(0, -5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@-webkit-keyframes in-up{0%{opacity:0;-webkit-transform:translate3D(0, 5px, 0);transform:translate3D(0, 5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@keyframes in-up{0%{opacity:0;-webkit-transform:translate3D(0, 5px, 0);transform:translate3D(0, 5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@-webkit-keyframes in-scale{0%{opacity:0;-webkit-transform:scale3D(0.95, 0.95, 1);transform:scale3D(0.95, 0.95, 1)}100%{opacity:1;-webkit-transform:scale3D(1, 1, 1);transform:scale3D(1, 1, 1)}}@keyframes in-scale{0%{opacity:0;-webkit-transform:scale3D(0.95, 0.95, 1);transform:scale3D(0.95, 0.95, 1)}100%{opacity:1;-webkit-transform:scale3D(1, 1, 1);transform:scale3D(1, 1, 1)}}:root{--calcite-animation-timing:300ms}.calcite-animate{opacity:0;-webkit-animation-fill-mode:both;animation-fill-mode:both;-webkit-animation-duration:var(--calcite-animation-timing);animation-duration:var(--calcite-animation-timing)}.calcite-animate__in{-webkit-animation-name:in;animation-name:in}.calcite-animate__in-down{-webkit-animation-name:in-down;animation-name:in-down}.calcite-animate__in-up{-webkit-animation-name:in-up;animation-name:in-up}.calcite-animate__in-scale{-webkit-animation-name:in-scale;animation-name:in-scale}:root{--calcite-popper-transition:150ms ease-in-out}:host([hidden]){display:none}:host([scale\x3ds]){--calcite-checkbox-size:0.75rem}:host([scale\x3dm]){--calcite-checkbox-size:var(--calcite-font-size--1)}:host([scale\x3dl]){--calcite-checkbox-size:1rem}:host{display:-ms-inline-flexbox;display:inline-flex;cursor:pointer;position:relative;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;-webkit-tap-highlight-color:transparent}:host .check-svg{overflow:hidden;display:block;background-color:var(--calcite-ui-foreground-1);pointer-events:none;-webkit-box-sizing:border-box;box-sizing:border-box;-webkit-transition-property:all;transition-property:all;-webkit-transition-duration:150ms;transition-duration:150ms;-webkit-transition-timing-function:cubic-bezier(0.4, 0, 0.2, 1);transition-timing-function:cubic-bezier(0.4, 0, 0.2, 1);stroke:currentColor;stroke-width:1;fill:currentColor;width:var(--calcite-checkbox-size);height:var(--calcite-checkbox-size);-webkit-box-shadow:inset 0 0 0 1px var(--calcite-ui-border-input);box-shadow:inset 0 0 0 1px var(--calcite-ui-border-input);color:var(--calcite-ui-background)}:host ::slotted(input){width:var(--calcite-checkbox-size);height:var(--calcite-checkbox-size)}:host(:hover) .check-svg,:host([hovered]) .check-svg{-webkit-box-shadow:inset 0 0 0 2px var(--calcite-ui-brand);box-shadow:inset 0 0 0 2px var(--calcite-ui-brand)}:host([checked]) .check-svg,:host([indeterminate]) .check-svg{background-color:var(--calcite-ui-brand);-webkit-box-shadow:inset 0 0 0 1px var(--calcite-ui-brand);box-shadow:inset 0 0 0 1px var(--calcite-ui-brand)}:host .focused .check-svg{-webkit-box-shadow:inset 0 0 0 1px var(--calcite-ui-brand), 0 0 0 2px var(--calcite-ui-foreground-1), 0 0 0 4px var(--calcite-ui-brand);box-shadow:inset 0 0 0 1px var(--calcite-ui-brand), 0 0 0 2px var(--calcite-ui-foreground-1), 0 0 0 4px var(--calcite-ui-brand);-webkit-transition:150ms ease-in-out;transition:150ms ease-in-out}:host([disabled]){cursor:default;opacity:var(--calcite-ui-opacity-disabled);pointer-events:none}"}};
e=b.proxyCustomElement(e,[1,"calcite-checkbox",{checked:[1540],disabled:[516],guid:[1537],hovered:[1540],indeterminate:[1540],label:[1],name:[513],scale:[513],value:[8],focused:[32],setFocus:[64]},[[1,"mouseenter","mouseenter"],[1,"mouseleave","mouseleave"]]]);g();f.CalciteCheckbox=e;f.defineCustomElement=g});