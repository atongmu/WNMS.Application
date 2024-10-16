/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{q as e,c as t}from"./dom.js";
/*!
 * All material copyright ESRI, All Rights Reserved, unless otherwise specified.
 * See https://github.com/Esri/calcite-components/blob/master/LICENSE.md for details.
 */const l=new WeakMap,n=l=>{const{id:n}=l,a=n&&e(l,`calcite-label[for="${n}"]`);if(a)return a;const i=t(l,"calcite-label");return!i||function(e,t){let l;const n="custom-element-ancestor-check",a=n=>{n.stopImmediatePropagation();const a=n.composedPath();l=a.slice(a.indexOf(t),a.indexOf(e))};e.addEventListener(n,a,{once:!0}),t.dispatchEvent(new CustomEvent(n,{composed:!0,bubbles:!0})),e.removeEventListener(n,a);return l.filter((l=>l!==t&&l!==e)).filter((e=>{var t;return null===(t=e.tagName)||void 0===t?void 0:t.includes("-")})).length>0}(i,l)?null:i};function a(e){const t=n(e.el);if(!t||l.has(t))return;e.labelEl=t;const a=c.bind(e);l.set(e.labelEl,a),e.labelEl.addEventListener("calciteInternalLabelClick",a)}function i(e){if(!e.labelEl)return;const t=l.get(e.labelEl);e.labelEl.removeEventListener("calciteInternalLabelClick",t),l.delete(e.labelEl)}function o(e){var t,l;return e.label||(null===(l=null===(t=e.labelEl)||void 0===t?void 0:t.textContent)||void 0===l?void 0:l.trim())||""}function c(e){this.el.contains(e.detail.sourceEvent.target)||this.onLabelClick(e)}export{a as c,i as d,o as g};
