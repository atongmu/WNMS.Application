/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import e from"../request.js";import o from"../core/Error.js";import{i as r}from"../core/lang.js";async function n(n,t,s,a,i,d){let l=null;if(r(s)){const o=`${n}/nodepages/`,t=o+Math.floor(s.rootIndex/s.nodesPerPage);try{return{type:"page",rootPage:(await e(t,{query:{f:"json",token:a},responseType:"json",signal:d})).data,rootIndex:s.rootIndex,pageSize:s.nodesPerPage,lodMetric:s.lodSelectionMetricType,urlPrefix:o}}catch(e){r(i)&&i.warn("#fetchIndexInfo()","Failed to load root node page. Falling back to node documents.",t,e),l=e}}if(!t)return null;const p=`${n}/nodes/`,c=p+(t&&t.split("/").pop());try{return{type:"node",rootNode:(await e(c,{query:{f:"json",token:a},responseType:"json",signal:d})).data,urlPrefix:p}}catch(e){throw new o("sceneservice:root-node-missing","Root node missing.",{pageError:l,nodeError:e,url:c})}}export{n as f};
