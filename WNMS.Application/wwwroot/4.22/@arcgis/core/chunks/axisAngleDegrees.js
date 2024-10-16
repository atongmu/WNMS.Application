/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{r,m as t,o as a}from"./mathUtils.js";import{g as n,s,m as o}from"./quat.js";import{a as u}from"./quatf64.js";function f(r=j){return[r[0],r[1],r[2],r[3]]}function i(r,a,n=f()){return t(n,r),n[3]=a,n}function m(t,a,u=f()){return s(q,t,p(t)),s(b,a,p(a)),o(q,b,q),i=u,m=r(n(u,q)),i[3]=m,i;var i,m}function c(r){return r}function e(r){return r[3]}function p(r){return a(r[3])}const j=[0,0,1,0],q=u(),b=u();f();export{m as a,c as b,f as c,p as d,e,i as f};
