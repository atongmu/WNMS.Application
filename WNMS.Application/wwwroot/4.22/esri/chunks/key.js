// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports"],function(c){c.getKey=function(a,b){a={Up:"ArrowUp",Down:"ArrowDown",Left:"ArrowLeft",Right:"ArrowRight",Spacebar:" ",Esc:"Escape"}[a]||a;return(b="rtl"===b)&&"ArrowLeft"===a?"ArrowRight":b&&"ArrowRight"===a?"ArrowLeft":a};c.numberKeys="0123456789".split("")});