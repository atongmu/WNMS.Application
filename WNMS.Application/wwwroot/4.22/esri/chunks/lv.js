// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./_commonjsHelpers","./moment"],function(p,m,q){var h={};(function(k,l){(function(f,b){"function"===typeof m.commonjsRequire?b(q.moment$1.exports):b(f.moment)})(m.commonjsGlobal,function(f){function b(c,d,g){return g?1===d%10&&11!==d%100?c[2]:c[3]:1===d%10&&11!==d%100?c[0]:c[1]}function a(c,d,g){return c+" "+b(n[g],c,d)}function e(c,d,g){return b(n[g],c,d)}var n={ss:["sekundes","sekund\u0113m","sekunde","sekundes"],m:["min\u016btes","min\u016bt\u0113m","min\u016bte","min\u016btes"],
mm:["min\u016btes","min\u016bt\u0113m","min\u016bte","min\u016btes"],h:["stundas","stund\u0101m","stunda","stundas"],hh:["stundas","stund\u0101m","stunda","stundas"],d:["dienas","dien\u0101m","diena","dienas"],dd:["dienas","dien\u0101m","diena","dienas"],M:["m\u0113ne\u0161a","m\u0113ne\u0161iem","m\u0113nesis","m\u0113ne\u0161i"],MM:["m\u0113ne\u0161a","m\u0113ne\u0161iem","m\u0113nesis","m\u0113ne\u0161i"],y:["gada","gadiem","gads","gadi"],yy:["gada","gadiem","gads","gadi"]};return f.defineLocale("lv",
{months:"janv\u0101ris febru\u0101ris marts apr\u012blis maijs j\u016bnijs j\u016blijs augusts septembris oktobris novembris decembris".split(" "),monthsShort:"jan feb mar apr mai j\u016bn j\u016bl aug sep okt nov dec".split(" "),weekdays:"sv\u0113tdiena pirmdiena otrdiena tre\u0161diena ceturtdiena piektdiena sestdiena".split(" "),weekdaysShort:"Sv P O T C Pk S".split(" "),weekdaysMin:"Sv P O T C Pk S".split(" "),weekdaysParseExact:!0,longDateFormat:{LT:"HH:mm",LTS:"HH:mm:ss",L:"DD.MM.YYYY.",LL:"YYYY. [gada] D. MMMM",
LLL:"YYYY. [gada] D. MMMM, HH:mm",LLLL:"YYYY. [gada] D. MMMM, dddd, HH:mm"},calendar:{sameDay:"[\u0160odien pulksten] LT",nextDay:"[R\u012bt pulksten] LT",nextWeek:"dddd [pulksten] LT",lastDay:"[Vakar pulksten] LT",lastWeek:"[Pag\u0101ju\u0161\u0101] dddd [pulksten] LT",sameElse:"L"},relativeTime:{future:"p\u0113c %s",past:"pirms %s",s:function(c,d){return d?"da\u017eas sekundes":"da\u017e\u0101m sekund\u0113m"},ss:a,m:e,mm:a,h:e,hh:a,d:e,dd:a,M:e,MM:a,y:e,yy:a},dayOfMonthOrdinalParse:/\d{1,2}\./,
ordinal:"%d.",week:{dow:1,doy:4}})})})();h=Object.freeze(function(k,l){for(var f=0;f<l.length;f++){const b=l[f];if("string"!==typeof b&&!Array.isArray(b))for(const a in b)if("default"!==a&&!(a in k)){const e=Object.getOwnPropertyDescriptor(b,a);e&&Object.defineProperty(k,a,e.get?e:{enumerable:!0,get:()=>b[a]})}}return Object.freeze(k)}({__proto__:null,default:h},[h]));p.lv=h});