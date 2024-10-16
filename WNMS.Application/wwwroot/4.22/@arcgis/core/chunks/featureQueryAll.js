/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{b as e}from"../core/lang.js";import r from"../rest/support/Query.js";async function t(r,t,s){t=t.clone(),r.capabilities.query.supportsMaxRecordCountFactor&&(t.maxRecordCountFactor=o(r));const n=a(r),u=r.capabilities.query.supportsPagination;t.start=0,t.num=n;let i=null;for(;;){const a=await r.source.queryFeaturesJSON(t,s);if(e(i)?i=a:i.features=i.features.concat(a.features),i.exceededTransferLimit=a.exceededTransferLimit,!u||!a.exceededTransferLimit)break;t.start+=n}return i}function a(e){return o(e)*function(e){return e.capabilities.query.maxRecordCount||2e3}(e)}function o(e){return e.capabilities.query.supportsMaxRecordCountFactor?r.MAX_MAX_RECORD_COUNT_FACTOR:1}export{a as g,t as q};
