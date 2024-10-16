// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./SpatialReference","./support/Ellipsoid","./support/spatialReferenceUtils"],function(c,k,b,d){function g(a){return new k({wkt:`GEOCCS["Spherical geocentric",\n    DATUM["Not specified",\n      SPHEROID["Sphere",${a.radius},0]],\n    PRIMEM["Greenwich",0.0,\n      AUTHORITY["EPSG","8901"]],\n    UNIT["m",1.0],\n    AXIS["Geocentric X",OTHER],\n    AXIS["Geocentric Y",EAST],\n    AXIS["Geocentric Z",NORTH]\n  ]`})}const h=g(b.earth),e=g(b.mars),f=g(b.moon),l=new k({wkt:`GEOCCS["WGS 84",\n  DATUM["WGS_1984",\n    SPHEROID["WGS 84",${b.earth.radius},298.257223563,\n      AUTHORITY["EPSG","7030"]],\n    AUTHORITY["EPSG","6326"]],\n  PRIMEM["Greenwich",0,\n    AUTHORITY["EPSG","8901"]],\n  UNIT["m",1.0,\n    AUTHORITY["EPSG","9001"]],\n  AXIS["Geocentric X",OTHER],\n  AXIS["Geocentric Y",OTHER],\n  AXIS["Geocentric Z",NORTH],\n  AUTHORITY["EPSG","4978"]\n]`});
c.SphericalECEFSpatialReference=h;c.SphericalPCPFMars=e;c.SphericalPCPFMoon=f;c.WGS84ECEFSpatialReference=l;c.createSphericalPCPF=g;c.getReferenceEllipsoid=function(a){return a&&(d.isMars(a)||a===e)?b.mars:a&&(d.isMoon(a)||a===f)?b.moon:b.earth};c.getReferenceEllipsoidFromWKID=function(a){return d.isWKIDFromMars(a)?b.mars:d.isWKIDFromMoon(a)?b.moon:b.earth};c.getSphericalPCPF=function(a){return a&&(d.isMars(a)||a===e)?e:a&&(d.isMoon(a)||a===f)?f:h};c.getSphericalPCPFForEllipsoid=function(a){return a&&
a===b.mars?e:a&&a===b.moon?f:h};Object.defineProperty(c,"__esModule",{value:!0})});