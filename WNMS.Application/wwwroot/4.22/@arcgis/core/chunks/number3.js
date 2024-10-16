/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{e}from"./string.js";import{g as n}from"./locale.js";const t={ar:[".",","],bg:[","," "],bs:[",","."],ca:[",","."],cs:[","," "],da:[",","."],de:[",","."],"de-ch":[".","’"],el:[",","."],en:[".",","],"en-au":[".",","],es:[",","."],"es-mx":[".",","],et:[","," "],fi:[","," "],fr:[","," "],"fr-ch":[","," "],he:[".",","],hi:[".",",","#,##,##0.###"],hr:[",","."],hu:[","," "],id:[",","."],it:[",","."],"it-ch":[".","’"],ja:[".",","],ko:[".",","],lt:[","," "],lv:[","," "],mk:[",","."],nb:[","," "],nl:[",","."],pl:[","," "],pt:[",","."],"pt-pt":[","," "],ro:[",","."],ru:[","," "],sk:[","," "],sl:[",","."],sr:[",","."],sv:[","," "],th:[".",","],tr:[",","."],uk:[","," "],vi:[",","."],zh:[".",","]};function r(e){e||(e=n());let r=e in t;if(!r){const n=e.split("-");n.length>1&&n[0]in t&&(e=n[0],r=!0),r||(e="en")}const[s,o,i="#,##0.###"]=t[e];return{decimal:s,group:o,pattern:i}}function s(e,n){const t=r((n={...n}).locale);n.customs=t;const s=n.pattern||t.pattern;return isNaN(e)||Math.abs(e)===1/0?null:function(e,n,t){const r=(t=t||{}).customs.group,s=t.customs.decimal,i=n.split(";"),a=i[0];if(-1!==(n=i[e<0?1:0]||"-"+a).indexOf("%"))e*=100;else if(-1!==n.indexOf("‰"))e*=1e3;else{if(-1!==n.indexOf("¤"))throw new Error("currency notation not supported");if(-1!==n.indexOf("E"))throw new Error("exponential notation not supported")}const l=o,c=a.match(l);if(!c)throw new Error("unable to find a number expression in pattern: "+n);!1===t.fractional&&(t.places=0);return n.replace(l,function(e,n,t){!0===(t=t||{}).places&&(t.places=0);t.places===1/0&&(t.places=6);const r=n.split("."),s="string"==typeof t.places&&t.places.indexOf(",");let o=t.places;s?o=t.places.substring(s+1):o>=0||(o=(r[1]||[]).length);t.round<0||(e=Number(e.toFixed(Number(o))));const i=String(Math.abs(e)).split("."),a=i[1]||"";if(r[1]||t.places){s&&(t.places=t.places.substring(0,s));const e=void 0!==t.places?t.places:r[1]&&r[1].lastIndexOf("0")+1;e>a.length&&(i[1]=a.padEnd(Number(e),"0")),o<a.length&&(i[1]=a.substr(0,Number(o)))}else i[1]&&i.pop();const l=r[0].replace(",","");let c=l.indexOf("0");-1!==c&&(c=l.length-c,c>i[0].length&&(i[0]=i[0].padStart(c,"0")),-1===l.indexOf("#")&&(i[0]=i[0].substr(i[0].length-c)));let p,u,f=r[0].lastIndexOf(",");if(-1!==f){p=r[0].length-f-1;const e=r[0].substr(0,f);f=e.lastIndexOf(","),-1!==f&&(u=e.length-f-1)}const d=[];for(let e=i[0];e;){const n=e.length-p;d.push(n>0?e.substr(n):e),e=n>0?e.slice(0,n):"",u&&(p=u,u=void 0)}return i[0]=d.reverse().join(t.group||","),i.join(t.decimal||".")}(e,c[0],{decimal:s,group:r,places:t.places,round:t.round}))}(e,s,n)}const o=/[#0,]*[#0](?:\.0*#*)?/;function i(n){const t=r((n=n||{}).locale),s=n.pattern||t.pattern,i=t.group,a=t.decimal;let p=1;if(-1!==s.indexOf("%"))p/=100;else if(-1!==s.indexOf("‰"))p/=1e3;else if(-1!==s.indexOf("¤"))throw new Error("currency notation not supported");const u=s.split(";");1===u.length&&u.push("-"+u[0]);const f=c(u,(function(t){return(t="(?:"+e(t,".")+")").replace(o,(function(e){const t={signed:!1,separator:n.strict?i:[i,""],fractional:n.fractional,decimal:a,exponent:!1},r=e.split(".");let s=n.places;1===r.length&&1!==p&&(r[1]="###"),1===r.length||0===s?t.fractional=!1:(void 0===s&&(s=n.pattern?r[1].lastIndexOf("0")+1:1/0),s&&null==n.fractional&&(t.fractional=!0),!n.places&&s<r[1].length&&(s+=","+r[1].length),t.places=s);const o=r[0].split(",");return o.length>1&&(t.groupSize=o.pop().length,o.length>1&&(t.groupSize2=o.pop().length)),"("+function(e){"places"in(e=e||{})||(e.places=1/0);"string"!=typeof e.decimal&&(e.decimal=".");"fractional"in e&&!/^0/.test(String(e.places))||(e.fractional=[!0,!1]);"exponent"in e||(e.exponent=[!0,!1]);"eSigned"in e||(e.eSigned=[!0,!1]);const n=l(e),t=c(e.fractional,(function(n){let t="";return n&&0!==e.places&&(t="\\"+e.decimal,e.places===1/0?t="(?:"+t+"\\d+)?":t+="\\d{"+e.places+"}"),t}),!0),r=c(e.exponent,(function(n){return n?"([eE]"+l({signed:e.eSigned})+")":""}));let s=n+t;t&&(s="(?:(?:"+s+")|(?:"+t+"))");return s+r}(t)+")"}))}),!0);return{regexp:f.replace(/[\xa0 ]/g,"[\\s\\xa0]"),group:i,decimal:a,factor:p}}function a(e,n){const t=i(n),r=new RegExp("^"+t.regexp+"$").exec(e);if(!r)return NaN;let s=r[1];if(!r[1]){if(!r[2])return NaN;s=r[2],t.factor*=-1}return s=s.replace(new RegExp("["+t.group+"\\s\\xa0]","g"),"").replace(t.decimal,"."),Number(s)*t.factor}function l(n){"signed"in(n=n||{})||(n.signed=[!0,!1]),"separator"in n?"groupSize"in n||(n.groupSize=3):n.separator="";return c(n.signed,(function(e){return e?"[-+]":""}),!0)+c(n.separator,(function(t){if(!t)return"(?:\\d+)";" "===(t=e(t))?t="\\s":" "===t&&(t="\\s\\xa0");const r=n.groupSize,s=n.groupSize2;if(s){const e="(?:0|[1-9]\\d{0,"+(s-1)+"}(?:["+t+"]\\d{"+s+"})*["+t+"]\\d{"+r+"})";return r-s>0?"(?:"+e+"|(?:0|[1-9]\\d{0,"+(r-1)+"}))":e}return"(?:0|[1-9]\\d{0,"+(r-1)+"}(?:["+t+"]\\d{"+r+"})*)"}),!0)}const c=function(e,n,t){if(!(e instanceof Array))return n(e);const r=[];for(let t=0;t<e.length;t++)r.push(n(e[t]));return p(r.join("|"),t)},p=function(e,n){return"("+(n?"?:":"")+e+")"};export{i as _,s as f,r as g,a as p};
