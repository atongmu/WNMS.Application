/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import"../core/lang.js";import{c as e}from"./screenUtils.js";function t(t){return e(t.x,t.y)}function n(t,n){const c=(t instanceof HTMLElement?t:t.surface).getBoundingClientRect();return e(n.clientX-c.left,n.clientY-c.top)}function c(e,c){return c instanceof Event?n(e,c):t(c)}function i(e){if(e instanceof Event)return!0;if("object"==typeof e&&"type"in e){switch(e.type){case"click":case"double-click":case"pointer-down":case"pointer-drag":case"pointer-enter":case"pointer-leave":case"pointer-up":case"pointer-move":case"immediate-click":case"immediate-double-click":case"hold":case"drag":case"mouse-wheel":return!0;default:return!1}}return!1}export{c as a,n as b,t as c,i};
