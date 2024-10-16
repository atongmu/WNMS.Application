/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{r as n}from"../core/lang.js";import{w as r}from"../core/Accessor.js";import"../core/accessorSupport/decorators/subclass.js";function s(n,r,s={}){return t(n,r,s,c)}function o(n,r,s={}){return t(n,r,s,i)}function t(s,o,t={},c){let i=null;const a=t.once?(r,s)=>{c(r)&&(n(i),o(r,s))}:(n,r)=>{c(n)&&o(n,r)};if(i=r(s,a,t.sync,t.equals),t.initial){const n=s();a(n,n)}return i}function c(n){return!0}function i(n){return!!n}const a={sync:!0},e={initial:!0},u={sync:!0,initial:!0};export{o as a,a as b,e as i,s as r,u as s};
