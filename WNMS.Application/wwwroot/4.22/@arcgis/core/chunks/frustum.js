/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{O as r,s as t,a as n}from"./vectorStacks.js";import{m as a,a as s}from"./mat4.js";import{g as o,f as c,l as e,J as f,e as u,b as i}from"./mathUtils.js";import{f as m}from"./vec4f64.js";import{c as l,a as p,f as g}from"./ray.js";import{c as y,a as j,f as b,i as A,b as d,s as h,d as v}from"./plane.js";function U(r){return r?{ray:l(r.ray),c0:r.c0,c1:r.c1}:{ray:l(),c0:0,c1:Number.MAX_VALUE}}function k(r,t=U()){return p(r,t.ray),t.c0=0,t.c1=Number.MAX_VALUE,t}function w(r,t){return L(r,r.c0,t)}function E(r,t){return L(r,r.c1,t)}function L(r,t,n){return o(n,r.ray.origin,c(n,r.ray.direction,t))}function M(r){return r?[y(r[0]),y(r[1]),y(r[2]),y(r[3]),y(r[4]),y(r[5])]:[y(),y(),y(),y(),y(),y()]}function N(){return[i(),i(),i(),i(),i(),i(),i(),i()]}function V(r,t=M()){for(let n=0;n<6;n++)j(r[n],t[n])}function X(r,o,c,e=G){const i=a(t.get(),o,r);s(i,i);for(let r=0;r<8;++r){const t=f(n.get(),D[r],i);u(e[r],t[0]/t[3],t[1]/t[3],t[2]/t[3])}_(c,e)}function _(r,t){b(t[4],t[0],t[3],r[0]),b(t[1],t[5],t[6],r[1]),b(t[4],t[5],t[1],r[2]),b(t[3],t[2],t[6],r[3]),b(t[0],t[1],t[2],r[4]),b(t[5],t[4],t[7],r[5])}function x(r,t){for(let n=0;n<6;n++){const a=r[n];if(a[0]*t[0]+a[1]*t[1]+a[2]*t[2]+a[3]>=t[3])return!1}return!0}function J(r,t){return B(r,k(t,F.get()))}function O(r,t){for(let n=0;n<6;n++){const a=r[n];if(!d(a,t))return!1}return!0}function S(r,t,n){return B(r,function(r,t,n=U()){const a=e(r.vector);return g(r.origin,t,n.ray),n.c0=0,n.c1=a,n}(t,n,F.get()))}function q(r,t){for(let n=0;n<6;n++){if(h(r[n],t)>0)return!1}return!0}function z(r,t){for(let n=0;n<6;n++)if(A(r[n],t))return!1;return!0}function B(r,t){for(let n=0;n<6;n++)if(!v(r[n],t))return!1;return!0}new r((()=>({c0:0,c1:0,ray:null})));const C={bottom:[5,1,0,4],near:[0,1,2,3],far:[5,4,7,6],right:[1,5,6,2],left:[4,0,3,7],top:[7,3,2,6]},D=[m(-1,-1,-1,1),m(1,-1,-1,1),m(1,1,-1,1),m(-1,1,-1,1),m(-1,-1,1,1),m(1,-1,1,1),m(1,1,1,1),m(-1,1,1,1)],F=new r(U),G=N();export{V as a,N as b,M as c,_ as d,J as e,X as f,S as g,q as h,x as i,z as j,U as k,k as l,O as m,w as n,E as o,C as p};
