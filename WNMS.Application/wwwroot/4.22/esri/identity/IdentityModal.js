// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("../chunks/_rollupPluginBabelHelpers ../chunks/tslib.es6 ../core/handleUtils ../core/accessorSupport/decorators/property ../core/arrayUtils ../core/has ../core/accessorSupport/ensureType ../core/accessorSupport/decorators/subclass ../widgets/Widget ../widgets/support/widget ../widgets/support/decorators/messageBundle ../widgets/support/jsxFactory".split(" "),function(Y,x,Z,A,L,ra,sa,aa,r,ba,ca,u){function N(a,c){var d=Object.keys(a);if(Object.getOwnPropertySymbols){var e=Object.getOwnPropertySymbols(a);
c&&(e=e.filter(function(g){return Object.getOwnPropertyDescriptor(a,g).enumerable}));d.push.apply(d,e)}return d}function da(a){for(var c=1;c<arguments.length;c++){var d=null!=arguments[c]?arguments[c]:{};c%2?N(Object(d),!0).forEach(function(e){var g=d[e];e in a?Object.defineProperty(a,e,{value:g,enumerable:!0,configurable:!0,writable:!0}):a[e]=g}):Object.getOwnPropertyDescriptors?Object.defineProperties(a,Object.getOwnPropertyDescriptors(d)):N(Object(d)).forEach(function(e){Object.defineProperty(a,
e,Object.getOwnPropertyDescriptor(d,e))})}return a}L='input select textarea a[href] button [tabindex] audio[controls] video[controls] [contenteditable]:not([contenteditable\x3d"false"]) details\x3esummary:first-of-type details'.split(" ");var O=L.join(","),H="undefined"===typeof Element?function(){}:Element.prototype.matches||Element.prototype.msMatchesSelector||Element.prototype.webkitMatchesSelector,ea=function(a,c,d){var e=Array.prototype.slice.apply(a.querySelectorAll(O));c&&H.call(a,O)&&e.unshift(a);
return e=e.filter(d)},P=function(a){var c=parseInt(a.getAttribute("tabindex"),10);return isNaN(c)?"true"===a.contentEditable||("AUDIO"===a.nodeName||"VIDEO"===a.nodeName||"DETAILS"===a.nodeName)&&null===a.getAttribute("tabindex")?0:a.tabIndex:c},fa=function(a,c){return a.tabIndex===c.tabIndex?a.documentOrder-c.documentOrder:a.tabIndex-c.tabIndex},ha=function(a){return"DETAILS"===a.tagName&&Array.prototype.slice.apply(a.children).some(function(c){return"SUMMARY"===c.tagName})},ia=function(a,c){for(var d=
0;d<a.length;d++)if(a[d].checked&&a[d].form===c)return a[d]},ja=function(a){if(!a.name)return!0;var c=a.form||a.ownerDocument,d=function(g){return c.querySelectorAll('input[type\x3d"radio"][name\x3d"'+g+'"]')};if("undefined"!==typeof window&&"undefined"!==typeof window.CSS&&"function"===typeof window.CSS.escape)var e=d(window.CSS.escape(a.name));else try{e=d(a.name)}catch(g){return console.error("Looks like you have a radio button with a name attribute containing invalid CSS selector characters and need the CSS.escape polyfill: %s",
g.message),!1}d=ia(e,a.form);return!d||d===a},ka=function(a,c){if("hidden"===getComputedStyle(a).visibility)return!0;var d=H.call(a,"details\x3esummary:first-of-type")?a.parentElement:a;if(H.call(d,"details:not([open]) *"))return!0;if(!c||"full"===c)for(;a;){if("none"===getComputedStyle(a).display)return!0;a=a.parentElement}else if("non-zero-area"===c)return a=a.getBoundingClientRect(),c=a.height,0===a.width&&0===c;return!1},Q=function(a,c){var d;(d=c.disabled)||(d="INPUT"===c.tagName&&"hidden"===
c.type);if(!(a=d||ka(c,a.displayCheck)||ha(c)))a:{if("INPUT"===c.tagName||"SELECT"===c.tagName||"TEXTAREA"===c.tagName||"BUTTON"===c.tagName)for(a=c.parentElement;a;){if("FIELDSET"===a.tagName&&a.disabled){for(d=0;d<a.children.length;d++){var e=a.children.item(d);if("LEGEND"===e.tagName){if(e.contains(c)){a=!1;break a}break}}a=!0;break a}a=a.parentElement}a=!1}return a?!1:!0},la=function(a,c){(a=!Q(a,c))||(a="INPUT"===c.tagName&&"radio"===c.type&&!ja(c));return a||0>P(c)?!1:!0},ma=function(a,c){c=
c||{};var d=[],e=[];ea(a,c.includeContainer,la.bind(null,c)).forEach(function(g,h){var t=P(g);0===t?d.push(g):e.push({documentOrder:h,tabIndex:t,node:g})});return e.sort(fa).map(function(g){return g.node}).concat(d)},na=L.concat("iframe").join(","),oa=function(a,c){c=c||{};if(!a)throw Error("No node provided");return!1===H.call(a,na)?!1:Q(c,a)},R=function(){var a=[];return{activateTrap:function(c){if(0<a.length){var d=a[a.length-1];d!==c&&d.pause()}d=a.indexOf(c);-1!==d&&a.splice(d,1);a.push(c)},
deactivateTrap:function(c){c=a.indexOf(c);-1!==c&&a.splice(c,1);0<a.length&&a[a.length-1].unpause()}}}(),M=function(a,c){var d=-1;a.every(function(e,g){return c(e)?(d=g,!1):!0});return d},B=function(a){for(var c=arguments.length,d=Array(1<c?c-1:0),e=1;e<c;e++)d[e-1]=arguments[e];return"function"===typeof a?a.apply(void 0,d):a},I=function(a){return a.target.shadowRoot&&"function"===typeof a.composedPath?a.composedPath()[0]:a.target},qa=function(a,c){var d=(null===c||void 0===c?void 0:c.document)||
document,e=da({returnFocusOnDeactivate:!0,escapeDeactivates:!0,delayInitialFocus:!0},c),g=[],h=[],t=null,y=null,n=!1,v=!1,C=void 0,q=function(k,b,f){return k&&void 0!==k[b]?k[b]:e[f||b]},z=function(k){return!(!k||!g.some(function(b){return b.contains(k)}))},D=function(k){var b=e[k];if("function"===typeof b){for(var f=arguments.length,l=Array(1<f?f-1:0),m=1;m<f;m++)l[m-1]=arguments[m];b=b.apply(void 0,l)}if(!b){if(void 0===b||!1===b)return b;throw Error("`".concat(k,"` was specified but was not a node, or did not return a node"));
}f=b;if("string"===typeof b&&(f=d.querySelector(b),!f))throw Error("`".concat(k,"` as selector refers to no known node"));return f},J=function(){var k=D("initialFocus");if(!1===k)return!1;void 0===k&&(k=z(d.activeElement)?d.activeElement:(k=h[0])&&k.firstTabbableNode||D("fallbackFocus"));if(!k)throw Error("Your focus-trap needs to have at least one focusable element");return k},E=function(){h=g.map(function(k){var b=ma(k);if(0<b.length)return{container:k,firstTabbableNode:b[0],lastTabbableNode:b[b.length-
1]}}).filter(function(k){return!!k});if(0>=h.length&&!D("fallbackFocus"))throw Error("Your focus-trap must have at least one container with at least one tabbable node in it at all times");},F=function f(b){!1!==b&&b!==d.activeElement&&(b&&b.focus?(b.focus({preventScroll:!!e.preventScroll}),y=b,b.tagName&&"input"===b.tagName.toLowerCase()&&"function"===typeof b.select&&b.select()):f(J()))},S=function(b){var f=D("setReturnFocus",b);return f?f:!1===f?!1:b},K=function(b){var f=I(b);z(f)||(B(e.clickOutsideDeactivates,
b)?w.deactivate({returnFocus:e.returnFocusOnDeactivate&&!oa(f)}):B(e.allowOutsideClick,b)||b.preventDefault())},T=function(b){var f=I(b),l=z(f);l||f instanceof Document?l&&(y=f):(b.stopImmediatePropagation(),F(y||J()))},pa=function(b){var f=I(b);E();var l=null;if(0<h.length){var m=M(h,function(G){return G.container.contains(f)});if(0>m)l=b.shiftKey?h[h.length-1].lastTabbableNode:h[0].firstTabbableNode;else if(b.shiftKey){var p=M(h,function(G){return f===G.firstTabbableNode});0>p&&h[m].container===
f&&(p=m);0<=p&&(l=h[0===p?h.length-1:p-1].lastTabbableNode)}else p=M(h,function(G){return f===G.lastTabbableNode}),0>p&&h[m].container===f&&(p=m),0<=p&&(l=h[p===h.length-1?0:p+1].firstTabbableNode)}else l=D("fallbackFocus");l&&(b.preventDefault(),F(l))},U=function(b){"Escape"!==b.key&&"Esc"!==b.key&&27!==b.keyCode||!1===B(e.escapeDeactivates,b)?"Tab"!==b.key&&9!==b.keyCode||pa(b):(b.preventDefault(),w.deactivate())},V=function(b){if(!B(e.clickOutsideDeactivates,b)){var f=I(b);z(f)||B(e.allowOutsideClick,
b)||(b.preventDefault(),b.stopImmediatePropagation())}},W=function(){if(n)return R.activateTrap(w),C=e.delayInitialFocus?setTimeout(function(){F(J())},0):F(J()),d.addEventListener("focusin",T,!0),d.addEventListener("mousedown",K,{capture:!0,passive:!1}),d.addEventListener("touchstart",K,{capture:!0,passive:!1}),d.addEventListener("click",V,{capture:!0,passive:!1}),d.addEventListener("keydown",U,{capture:!0,passive:!1}),w},X=function(){if(n)return d.removeEventListener("focusin",T,!0),d.removeEventListener("mousedown",
K,!0),d.removeEventListener("touchstart",K,!0),d.removeEventListener("click",V,!0),d.removeEventListener("keydown",U,!0),w};var w={activate:function(b){if(n)return this;var f=q(b,"onActivate"),l=q(b,"onPostActivate"),m=q(b,"checkCanFocusTrap");m||E();n=!0;v=!1;t=d.activeElement;f&&f();b=function(){m&&E();W();l&&l()};if(m)return m(g.concat()).then(b,b),this;b();return this},deactivate:function(b){if(!n)return this;clearTimeout(C);C=void 0;X();v=n=!1;R.deactivateTrap(w);var f=q(b,"onDeactivate"),l=
q(b,"onPostDeactivate"),m=q(b,"checkCanReturnFocus");f&&f();var p=q(b,"returnFocus","returnFocusOnDeactivate");b=function(){setTimeout(function(){p&&F(S(t));l&&l()},0)};if(p&&m)return m(S(t)).then(b,b),this;b();return this},pause:function(){if(v||!n)return this;v=!0;X();return this},unpause:function(){if(!v||!n)return this;v=!1;E();W();return this},updateContainerElements:function(b){g=[].concat(b).filter(Boolean).map(function(f){return"string"===typeof f?d.querySelector(f):f});n&&E();return this}};
w.updateContainerElements(a);return w};r=function(a){function c(e,g){var h=a.call(this,e,g)||this;h.container=document.createElement("div");h.content=null;h.open=!1;h._close=()=>{h.open=!1};document.body.appendChild(h.container);h.own(h.watch("open",()=>h._toggleFocusTrap()));return h}Y._inheritsLoose(c,a);var d=c.prototype;d.destroy=function(){this._destroyFocusTrap()};d.render=function(){var e=this.id;const {open:g,content:h,title:t,messages:y}=this;var n=g&&!!h;const v={["esri-identity-modal--open"]:n,
["esri-identity-modal--closed"]:!n},C=u.tsx("button",{class:"esri-identity-modal__close-button","aria-label":y.close,title:y.close,bind:this,onclick:this._close},u.tsx("span",{"aria-hidden":"true",class:"esri-icon-close"})),q=`${e}_title`;e=`${e}_content`;const z=t?u.tsx("h1",{id:q,class:"esri-identity-modal__title"},t):null;n=n?u.tsx("div",{bind:this,class:"esri-identity-modal__dialog",role:"dialog","aria-labelledby":q,"aria-describedby":e,afterCreate:this._createFocusTrap},C,z,this._renderContent(e)):
null;return u.tsx("div",{tabIndex:-1,class:this.classes("esri-identity-modal",v)},n)};d._destroyFocusTrap=function(){var e;null==(e=this._focusTrap)?void 0:e.deactivate({onDeactivate:null});this._focusTrap=null};d._toggleFocusTrap=function(){const {_focusTrap:e,open:g}=this;e&&(g?e.activate():e.deactivate())};d._createFocusTrap=function(e){this._destroyFocusTrap();const g=requestAnimationFrame(()=>{this._focusTrap=qa(e,{initialFocus:"input",onDeactivate:this._close});this._toggleFocusTrap()});this.own(Z.makeHandle(()=>
cancelAnimationFrame(g)))};d._renderContent=function(e){const g=this.content;return"string"===typeof g?u.tsx("div",{class:"esri-identity-modal__content",id:e,innerHTML:g}):ba.isWidget(g)?u.tsx("div",{class:"esri-identity-modal__content",id:e},g.render()):g instanceof HTMLElement?u.tsx("div",{class:"esri-identity-modal__content",id:e,bind:g,afterCreate:this._attachToNode}):null};d._attachToNode=function(e){e.appendChild(this)};return c}(r);x.__decorate([A.property({readOnly:!0})],r.prototype,"container",
void 0);x.__decorate([A.property()],r.prototype,"content",void 0);x.__decorate([A.property()],r.prototype,"open",void 0);x.__decorate([A.property(),ca.messageBundle("esri/t9n/common")],r.prototype,"messages",void 0);x.__decorate([A.property({aliasOf:"messages.auth.signIn"})],r.prototype,"title",void 0);return r=x.__decorate([aa.subclass("esri.identity.IdentityModal")],r)});