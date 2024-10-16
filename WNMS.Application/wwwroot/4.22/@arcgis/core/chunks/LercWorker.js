/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{d as e}from"./LercCodec.js";import"./mathUtils.js";import"./common.js";class r{_decode(r){const t=e(r.buffer,r.options);return Promise.resolve({result:t,transferList:[t.pixelData.buffer]})}}function t(){return new r}export{t as default};
