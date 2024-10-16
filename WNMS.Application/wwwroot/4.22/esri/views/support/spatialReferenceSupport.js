// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports"],function(b){b.isSpatialReferenceSupported=function(a,c){return null==a?!1:null==c?!0:2===c?a.isGeographic?a.isWGS84||4490===a.wkid:!0:a.isWebMercator||a.isWGS84||4490===a.wkid||104971===a.wkid||104905===a.wkid||104903===a.wkid};Object.defineProperty(b,"__esModule",{value:!0})});