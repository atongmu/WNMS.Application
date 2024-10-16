// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./componentsUtils","./dom"],function(f,e,g){function k(){["calcite-tab-nav"].forEach(a=>{switch(a){case "calcite-tab-nav":customElements.get(a)||customElements.define(a,f.CalciteTabNav)}})}f.CalciteTabNav=class extends HTMLElement{constructor(){super();this.__registerHost();this.__attachShadow();this.calciteTabChange=e.createEvent(this,"calciteTabChange",7);this.scale="m";this.layout="inline";this.position="below";this.bordered=!1;this.animationActiveDuration=.3;this.handleContainerScroll=
()=>{this.activeIndicatorEl.style.transitionDuration="0s";this.updateOffsetPosition()}}async selectedTabChanged(){localStorage&&this.storageId&&void 0!==this.selectedTab&&null!==this.selectedTab&&localStorage.setItem(`calcite-tab-nav-${this.storageId}`,JSON.stringify(this.selectedTab));this.calciteTabChange.emit({tab:this.selectedTab});this.selectedTabEl=await this.getTabTitleById(this.selectedTab)}selectedTabElChanged(){this.updateOffsetPosition();this.updateActiveWidth();this.activeIndicatorEl.style.transitionDuration=
`${this.animationActiveDuration}s`}componentWillLoad(){const a=`calcite-tab-nav-${this.storageId}`;localStorage&&this.storageId&&localStorage.getItem(a)&&(this.selectedTab=JSON.parse(localStorage.getItem(a)),this.calciteTabChange.emit({tab:this.selectedTab}))}componentWillRender(){var a,b,d,c;this.layout=null===(a=this.el.closest("calcite-tabs"))||void 0===a?void 0:a.layout;this.position=null===(b=this.el.closest("calcite-tabs"))||void 0===b?void 0:b.position;this.scale=null===(d=this.el.closest("calcite-tabs"))||
void 0===d?void 0:d.scale;this.bordered=null===(c=this.el.closest("calcite-tabs"))||void 0===c?void 0:c.bordered;this.selectedTabEl&&this.updateOffsetPosition()}componentDidRender(){this.tabTitles.length&&this.tabTitles.every(a=>!a.active)&&!this.selectedTab&&this.tabTitles[0].getTabIdentifier().then(a=>{this.calciteTabChange.emit({tab:a})})}render(){const a=g.getElementDir(this.el),b=`${this.indicatorWidth}px`,d=`${this.indicatorOffset}px`;return e.h(e.Host,{role:"tablist"},e.h("div",{class:"tab-nav",
onScroll:this.handleContainerScroll,ref:c=>this.tabNavEl=c},e.h("div",{class:"tab-nav-active-indicator-container",ref:c=>this.activeIndicatorContainerEl=c},e.h("div",{class:"tab-nav-active-indicator",ref:c=>this.activeIndicatorEl=c,style:"rtl"!==a?{width:b,left:d}:{width:b,right:d}})),e.h("slot",null)))}resizeHandler(){this.activeIndicatorEl.style.transitionDuration="0s";this.updateActiveWidth();this.updateOffsetPosition()}focusPreviousTabHandler(a){const b=this.getIndexOfTabTitle(a.target,this.enabledTabTitles);
(this.enabledTabTitles[b-1]||this.enabledTabTitles[this.enabledTabTitles.length-1]).focus();a.stopPropagation();a.preventDefault()}focusNextTabHandler(a){const b=this.getIndexOfTabTitle(a.target,this.enabledTabTitles);(this.enabledTabTitles[b+1]||this.enabledTabTitles[0]).focus();a.stopPropagation();a.preventDefault()}activateTabHandler(a){this.selectedTab=a.detail.tab?a.detail.tab:this.getIndexOfTabTitle(a.target);a.stopPropagation();a.preventDefault()}updateTabTitles(a){a.target.active&&(this.selectedTab=
a.detail)}globalTabChangeHandler(a){this.syncId&&a.target!==this.el&&a.target.syncId===this.syncId&&this.selectedTab!==a.detail.tab&&(this.selectedTab=a.detail.tab)}updateOffsetPosition(){var a,b,d,c,h;const l=g.getElementDir(this.el),m=null===(a=this.activeIndicatorContainerEl)||void 0===a?void 0:a.offsetWidth;a=null===(b=this.selectedTabEl)||void 0===b?void 0:b.offsetLeft;b=null===(d=this.selectedTabEl)||void 0===d?void 0:d.offsetWidth;this.indicatorOffset="rtl"!==l?a-(null===(c=this.tabNavEl)||
void 0===c?NaN:c.scrollLeft):m-(a+b)+(null===(h=this.tabNavEl)||void 0===h?void 0:h.scrollLeft)}updateActiveWidth(){var a;this.indicatorWidth=null===(a=this.selectedTabEl)||void 0===a?void 0:a.offsetWidth}getIndexOfTabTitle(a,b=this.tabTitles){return b.indexOf(a)}async getTabTitleById(a){return Promise.all(this.tabTitles.map(b=>b.getTabIdentifier())).then(b=>this.tabTitles[b.indexOf(a)])}get tabTitles(){return g.filterDirectChildren(this.el,"calcite-tab-title")}get enabledTabTitles(){return g.filterDirectChildren(this.el,
"calcite-tab-title:not([disabled])")}get el(){return this}static get watchers(){return{selectedTab:["selectedTabChanged"],selectedTabEl:["selectedTabElChanged"]}}static get style(){return"@-webkit-keyframes in{0%{opacity:0}100%{opacity:1}}@keyframes in{0%{opacity:0}100%{opacity:1}}@-webkit-keyframes in-down{0%{opacity:0;-webkit-transform:translate3D(0, -5px, 0);transform:translate3D(0, -5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@keyframes in-down{0%{opacity:0;-webkit-transform:translate3D(0, -5px, 0);transform:translate3D(0, -5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@-webkit-keyframes in-up{0%{opacity:0;-webkit-transform:translate3D(0, 5px, 0);transform:translate3D(0, 5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@keyframes in-up{0%{opacity:0;-webkit-transform:translate3D(0, 5px, 0);transform:translate3D(0, 5px, 0)}100%{opacity:1;-webkit-transform:translate3D(0, 0, 0);transform:translate3D(0, 0, 0)}}@-webkit-keyframes in-scale{0%{opacity:0;-webkit-transform:scale3D(0.95, 0.95, 1);transform:scale3D(0.95, 0.95, 1)}100%{opacity:1;-webkit-transform:scale3D(1, 1, 1);transform:scale3D(1, 1, 1)}}@keyframes in-scale{0%{opacity:0;-webkit-transform:scale3D(0.95, 0.95, 1);transform:scale3D(0.95, 0.95, 1)}100%{opacity:1;-webkit-transform:scale3D(1, 1, 1);transform:scale3D(1, 1, 1)}}:root{--calcite-animation-timing:300ms}.calcite-animate{opacity:0;-webkit-animation-fill-mode:both;animation-fill-mode:both;-webkit-animation-duration:var(--calcite-animation-timing);animation-duration:var(--calcite-animation-timing)}.calcite-animate__in{-webkit-animation-name:in;animation-name:in}.calcite-animate__in-down{-webkit-animation-name:in-down;animation-name:in-down}.calcite-animate__in-up{-webkit-animation-name:in-up;animation-name:in-up}.calcite-animate__in-scale{-webkit-animation-name:in-scale;animation-name:in-scale}:root{--calcite-popper-transition:150ms ease-in-out}:host([hidden]){display:none}:host{position:relative;display:-ms-flexbox;display:flex}:host([scale\x3ds]){min-height:1.5rem}:host([scale\x3dm]){min-height:2rem}:host([scale\x3dl]){min-height:2.75rem}.tab-nav{display:-ms-flexbox;display:flex;width:100%;overflow:auto;-ms-flex-pack:start;justify-content:flex-start;-webkit-overflow-scrolling:touch;padding:0.25rem;margin:-0.25rem}:host([layout\x3dcenter]) .tab-nav{-ms-flex-pack:center;justify-content:center}.tab-nav-active-indicator-container{width:100%;right:0;left:0;bottom:0;position:absolute;overflow:hidden;height:0.125rem}.tab-nav-active-indicator{position:absolute;bottom:0;display:block;-webkit-transition-property:all;transition-property:all;-webkit-transition-timing-function:cubic-bezier(0, 0, 0.2, 1);transition-timing-function:cubic-bezier(0, 0, 0.2, 1);background-color:var(--calcite-ui-brand);height:0.125rem}:host([position\x3dbelow]) .tab-nav-active-indicator{bottom:unset;top:0}:host([position\x3dbelow]) .tab-nav-active-indicator-container{bottom:unset;top:0}:host([bordered]) .tab-nav-active-indicator-container{bottom:unset}:host([bordered][position\x3dbelow]) .tab-nav-active-indicator-container{bottom:0;top:unset}"}};
f.CalciteTabNav=e.proxyCustomElement(f.CalciteTabNav,[1,"calcite-tab-nav",{storageId:[1,"storage-id"],syncId:[1,"sync-id"],scale:[1537],layout:[1537],position:[1537],bordered:[1540],indicatorOffset:[1026,"indicator-offset"],indicatorWidth:[1026,"indicator-width"],selectedTab:[32],selectedTabEl:[32]},[[9,"resize","resizeHandler"],[0,"calciteTabsFocusPrevious","focusPreviousTabHandler"],[0,"calciteTabsFocusNext","focusNextTabHandler"],[0,"calciteTabsActivate","activateTabHandler"],[0,"calciteTabTitleRegister",
"updateTabTitles"],[16,"calciteTabChange","globalTabChangeHandler"]]]);k();f.defineCustomElement=k});