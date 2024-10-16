// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./geometryEngineBase","../geometry/geometryAdapters/json"],function(d,e,f){function m(b){return e.GeometryEngineApi.extendedSpatialReferenceInfo(b)}function n(b,a,c){return e.GeometryEngineApi.clip(f.jsonAdapter,b,a,c)}function p(b,a,c){return e.GeometryEngineApi.cut(f.jsonAdapter,b,a,c)}function q(b,a,c){return e.GeometryEngineApi.contains(f.jsonAdapter,b,a,c)}function r(b,a,c){return e.GeometryEngineApi.crosses(f.jsonAdapter,b,a,c)}function t(b,a,c,g){return e.GeometryEngineApi.distance(f.jsonAdapter,
b,a,c,g)}function u(b,a,c){return e.GeometryEngineApi.equals(f.jsonAdapter,b,a,c)}function v(b,a,c){return e.GeometryEngineApi.intersects(f.jsonAdapter,b,a,c)}function w(b,a,c){return e.GeometryEngineApi.touches(f.jsonAdapter,b,a,c)}function x(b,a,c){return e.GeometryEngineApi.within(f.jsonAdapter,b,a,c)}function y(b,a,c){return e.GeometryEngineApi.disjoint(f.jsonAdapter,b,a,c)}function z(b,a,c){return e.GeometryEngineApi.overlaps(f.jsonAdapter,b,a,c)}function A(b,a,c,g){return e.GeometryEngineApi.relate(f.jsonAdapter,
b,a,c,g)}function B(b,a){return e.GeometryEngineApi.isSimple(f.jsonAdapter,b,a)}function C(b,a){return e.GeometryEngineApi.simplify(f.jsonAdapter,b,a)}function D(b,a,c=!1){return e.GeometryEngineApi.convexHull(f.jsonAdapter,b,a,c)}function E(b,a,c){return e.GeometryEngineApi.difference(f.jsonAdapter,b,a,c)}function F(b,a,c){return e.GeometryEngineApi.symmetricDifference(f.jsonAdapter,b,a,c)}function G(b,a,c){return e.GeometryEngineApi.intersect(f.jsonAdapter,b,a,c)}function H(b,a,c=null){return e.GeometryEngineApi.union(f.jsonAdapter,
b,a,c)}function I(b,a,c,g,h,k,l){return e.GeometryEngineApi.offset(f.jsonAdapter,b,a,c,g,h,k,l)}function J(b,a,c,g,h=!1){return e.GeometryEngineApi.buffer(f.jsonAdapter,b,a,c,g,h)}function K(b,a,c,g,h,k,l){return e.GeometryEngineApi.geodesicBuffer(f.jsonAdapter,b,a,c,g,h,k,l)}function L(b,a,c,g=!0){return e.GeometryEngineApi.nearestCoordinate(f.jsonAdapter,b,a,c,g)}function M(b,a,c){return e.GeometryEngineApi.nearestVertex(f.jsonAdapter,b,a,c)}function N(b,a,c,g,h){return e.GeometryEngineApi.nearestVertices(f.jsonAdapter,
b,a,c,g,h)}function O(b,a,c,g){if(null==a||null==g)throw Error("Illegal Argument Exception");a=e.GeometryEngineApi.rotate(a,c,g);a.spatialReference=b;return a}function P(b,a,c){if(null==a||null==c)throw Error("Illegal Argument Exception");a=e.GeometryEngineApi.flipHorizontal(a,c);a.spatialReference=b;return a}function Q(b,a,c){if(null==a||null==c)throw Error("Illegal Argument Exception");a=e.GeometryEngineApi.flipVertical(a,c);a.spatialReference=b;return a}function R(b,a,c,g,h){return e.GeometryEngineApi.generalize(f.jsonAdapter,
b,a,c,g,h)}function S(b,a,c,g){return e.GeometryEngineApi.densify(f.jsonAdapter,b,a,c,g)}function T(b,a,c,g,h=0){return e.GeometryEngineApi.geodesicDensify(f.jsonAdapter,b,a,c,g,h)}function U(b,a,c){return e.GeometryEngineApi.planarArea(f.jsonAdapter,b,a,c)}function V(b,a,c){return e.GeometryEngineApi.planarLength(f.jsonAdapter,b,a,c)}function W(b,a,c,g){return e.GeometryEngineApi.geodesicArea(f.jsonAdapter,b,a,c,g)}function X(b,a,c,g){return e.GeometryEngineApi.geodesicLength(f.jsonAdapter,b,a,c,
g)}const Y=Object.freeze({__proto__:null,extendedSpatialReferenceInfo:m,clip:n,cut:p,contains:q,crosses:r,distance:t,equals:u,intersects:v,touches:w,within:x,disjoint:y,overlaps:z,relate:A,isSimple:B,simplify:C,convexHull:D,difference:E,symmetricDifference:F,intersect:G,union:H,offset:I,buffer:J,geodesicBuffer:K,nearestCoordinate:L,nearestVertex:M,nearestVertices:N,rotate:O,flipHorizontal:P,flipVertical:Q,generalize:R,densify:S,geodesicDensify:T,planarArea:U,planarLength:V,geodesicArea:W,geodesicLength:X});
d.buffer=J;d.clip=n;d.contains=q;d.convexHull=D;d.crosses=r;d.cut=p;d.densify=S;d.difference=E;d.disjoint=y;d.distance=t;d.equals=u;d.extendedSpatialReferenceInfo=m;d.flipHorizontal=P;d.flipVertical=Q;d.generalize=R;d.geodesicArea=W;d.geodesicBuffer=K;d.geodesicDensify=T;d.geodesicLength=X;d.geometryEngineJSON=Y;d.intersect=G;d.intersects=v;d.isSimple=B;d.nearestCoordinate=L;d.nearestVertex=M;d.nearestVertices=N;d.offset=I;d.overlaps=z;d.planarArea=U;d.planarLength=V;d.relate=A;d.rotate=O;d.simplify=
C;d.symmetricDifference=F;d.touches=w;d.union=H;d.within=x});