// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../chunks/_rollupPluginBabelHelpers ../../request ../utils ./utils ../support/AddressCandidate ../support/AddressToLocationsParameters".split(" "),function(h,m,n,f,k,p,q){function g(){g=m._asyncToGenerator(function*(b,a,c){a=q.from(a);b=f.parseUrl(b);const {address:e,...d}=a.toJSON();a=f.encode({...b.query,...e,...d,f:"json"});c=f.asValidOptions(a,c);return n(`${b.path}/findAddressCandidates`,c).then(r)});return g.apply(this,arguments)}function r({data:b}){if(!b)return[];const {candidates:a,
spatialReference:c}=b;return a?a.map(e=>{if(e){var {extent:d,location:l}=e,t=d?k.isValidExtent(d):!0;if(k.isValidLocation(l)&&t)return d&&(d.spatialReference=c),l.spatialReference=c,p.fromJSON(e)}}):[]}h.addressToLocations=function(b,a,c){return g.apply(this,arguments)};Object.defineProperty(h,"__esModule",{value:!0})});