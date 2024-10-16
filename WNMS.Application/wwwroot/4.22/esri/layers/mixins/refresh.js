// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../core/Collection","../../core/accessorSupport/watch","../../core/accessorSupport/trackingUtils"],function(g,n,r,p){function h(a){return a&&"object"===typeof a&&"refreshInterval"in a&&"refresh"in a}function l(a,b){return Number.isFinite(a)&&Number.isFinite(b)?0>=b?a:l(b,a%b):0}function m(){const a=Date.now();for(const c of d)if(c.refreshInterval){var b;const q=null!=(b=e.get(c))?b:0;a-q+5>=6E4*c.refreshInterval&&(e.set(c,a),c.refresh(a))}}const d=new n,e=new WeakMap;let k=0,
f=0;p.autorun(()=>{const a=Date.now();let b=0;for(const c of d)b=l(Math.round(6E4*c.refreshInterval),b),c.refreshInterval?e.get(c)||e.set(c,a):e.delete(c);b!==f&&(f=b,clearInterval(k),k=0===f?0:setInterval(m,f))});g.registerLayer=function(a){h(a)&&d.push(a)};g.test={get hasRefreshTimer(){return 0<k},get tickInterval(){return f},forceRefresh(){m()},hasLayer(a){return h(a)&&d.includes(a)},clear(){for(const a of d)e.delete(a);d.removeAll()}};g.unregisterLayer=function(a){h(a)&&d.includes(a)&&d.remove(a)};
Object.defineProperty(g,"__esModule",{value:!0})});