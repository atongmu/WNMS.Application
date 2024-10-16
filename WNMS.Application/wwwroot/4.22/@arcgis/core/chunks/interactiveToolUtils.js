/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{b as t}from"../core/lang.js";function e(t){return[t.on("after-add",(t=>{const e=t.item;e.view&&e.view.ready&&!e.attached&&e.attach()})),t.on("after-remove",(t=>{const e=t.item;e.active&&(e.view.activeTool=null),e.attached&&e.detach()}))]}function a(t){return t.visible&&t.getEditableFlag(0)&&t.getEditableFlag(1)}function n(e){return t(e)?{}:"function"==typeof e?e():e}export{a,n as e,e as g};
