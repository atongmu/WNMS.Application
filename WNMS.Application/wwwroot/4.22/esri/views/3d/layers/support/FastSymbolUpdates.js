// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../core/has ../../../../core/mathUtils ../../../../chunks/mat3 ../../../../chunks/mat3f64 ../../../../chunks/mat4 ../../../../chunks/mat4f64 ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../../../../renderers/support/lengthUtils ../../support/debugFlags".split(" "),function(p,C,D,H,I,k,y,J,K,E,L){function u(a){return null!==a&&void 0!==a}function q(a){return"number"===typeof a}function w(a){return"string"===typeof a}function g(a,b){a&&a.push(b)}function M(a,b,c,d=y.create()){a=
a||0;b=b||0;c=c||0;0!==a&&k.rotateZ(d,d,-a/180*Math.PI);0!==b&&k.rotateX(d,d,b/180*Math.PI);0!==c&&k.rotateY(d,d,c/180*Math.PI);return d}function n(a,b,c,d,e){const f=a.minSize,h=a.maxSize;if(a.expression)return g(e,"Could not convert size info: expression not supported"),!1;if(a.useSymbolValue)return a=d.symbolSize[c],b.minSize[c]=a,b.maxSize[c]=a,b.offset[c]=b.minSize[c],b.factor[c]=0,b.type[c]=1,!0;if(u(a.field)){if(u(a.stops)){if(2===a.stops.length&&q(a.stops[0].size)&&q(a.stops[1].size))return F(a.stops[0].size,
a.stops[1].size,a.stops[0].value,a.stops[1].value,b,c),b.type[c]=1,!0;g(e,"Could not convert size info: stops only supported with 2 elements");return!1}if(q(f)&&q(h)&&u(a.minDataValue)&&u(a.maxDataValue))return F(f,h,a.minDataValue,a.maxDataValue,b,c),b.type[c]=1,!0;if(null!=E.meterIn[a.valueUnit])return b.minSize[c]=-Infinity,b.maxSize[c]=Infinity,b.offset[c]=0,b.factor[c]=1/E.meterIn[a.valueUnit],b.type[c]=1,!0;if("unknown"===a.valueUnit)return g(e,"Could not convert size info: proportional size not supported"),
!1;g(e,"Could not convert size info: scale-dependent size not supported");return!1}if(!u(a.field)){if(a.stops&&a.stops[0]&&q(a.stops[0].size))return b.minSize[c]=a.stops[0].size,b.maxSize[c]=a.stops[0].size,b.offset[c]=b.minSize[c],b.factor[c]=0,b.type[c]=1,!0;if(q(f))return b.minSize[c]=f,b.maxSize[c]=f,b.offset[c]=f,b.factor[c]=0,b.type[c]=1,!0}g(e,"Could not convert size info: unsupported variant of sizeInfo");return!1}function F(a,b,c,d,e,f){d=0<Math.abs(d-c)?(b-a)/(d-c):0;e.minSize[f]=0<d?a:
b;e.maxSize[f]=0<d?b:a;e.offset[f]=a-c*d;e.factor[f]=d}function N(a,b,c,d){if(a.normalizationField||a.valueRepresentation)return g(d,"Could not convert size info: unsupported property"),null;var e=a.field;if(null!=e&&!w(e))return g(d,"Could not convert size info: field is not a string"),null;if(!b.size)b.size={field:a.field,minSize:[0,0,0],maxSize:[0,0,0],offset:[0,0,0],factor:[0,0,0],type:[0,0,0]};else if(a.field)if(!b.size.field)b.size.field=a.field;else if(a.field!==b.size.field)return g(d,"Could not convert size info: multiple fields in use"),
null;switch(a.axis){case "width":return(e=n(a,b.size,0,c,d))?b:null;case "height":return(e=n(a,b.size,2,c,d))?b:null;case "depth":return(e=n(a,b.size,1,c,d))?b:null;case "width-and-depth":return(e=n(a,b.size,0,c,d))&&n(a,b.size,1,c,d),e?b:null;case null:case void 0:case "all":return(e=(e=(e=n(a,b.size,0,c,d))&&n(a,b.size,1,c,d))&&n(a,b.size,2,c,d))?b:null;default:return g(d,`Could not convert size info: unknown axis "${a.axis}""`),null}}function O(a,b,c){for(let d=0;3>d;++d){let e=b.unitInMeters;
1===a.type[d]&&(e*=b.modelSize[d],a.type[d]=2);a.minSize[d]/=e;a.maxSize[d]/=e;a.offset[d]/=e;a.factor[d]/=e}if(0!==a.type[0])b=0;else if(0!==a.type[1])b=1;else if(0!==a.type[2])b=2;else return g(c,"No size axis contains a valid size or scale"),!1;for(c=0;3>c;++c)0===a.type[c]&&(a.minSize[c]=a.minSize[b],a.maxSize[c]=a.maxSize[b],a.offset[c]=a.offset[b],a.factor[c]=a.factor[b],a.type[c]=a.type[b]);return!0}function G(a,b,c){a[4*b]=c.r/255;a[4*b+1]=c.g/255;a[4*b+2]=c.b/255;a[4*b+3]=c.a}function P(a,
b,c){if(a.normalizationField)return g(c,"Could not convert color info: unsupported property"),null;if(w(a.field))if(a.stops){if(8<a.stops.length)return g(c,"Could not convert color info: too many color stops"),null;b.color={field:a.field,values:[0,0,0,0,0,0,0,0],colors:[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]};a=a.stops;for(c=0;8>c;++c){const d=a[Math.min(c,a.length-1)];b.color.values[c]=d.value;G(b.color.colors,c,d.color)}}else return g(c,"Could not convert color info: missing stops or colors"),
null;else if(a.stops&&0<=a.stops.length)for(a=a.stops&&0<=a.stops.length&&a.stops[0].color,b.color={field:null,values:[0,0,0,0,0,0,0,0],colors:[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]},c=0;8>c;c++)b.color.values[c]=Infinity,G(b.color.colors,c,a);else return g(c,"Could not convert color info: no field and no colors/stops"),null;return b}function Q(a,b,c){if(a.normalizationField)return g(c,"Could not convert opacity info: unsupported property"),null;if(w(a.field))if(a.stops){if(8<
a.stops.length)return g(c,"Could not convert opacity info: too many opacity stops"),null;b.opacity={field:a.field,values:[0,0,0,0,0,0,0,0],opacityValues:[0,0,0,0,0,0,0,0]};a=a.stops;for(c=0;8>c;++c){const d=a[Math.min(c,a.length-1)];b.opacity.values[c]=d.value;b.opacity.opacityValues[c]=d.opacity}}else return g(c,"Could not convert opacity info: missing stops or opacities"),null;else if(a.stops&&0<=a.stops.length)for(a=a.stops&&0<=a.stops.length&&a.stops[0].opacity,b.opacity={field:null,values:[0,
0,0,0,0,0,0,0],opacityValues:[0,0,0,0,0,0,0,0]},c=0;8>c;c++)b.opacity.values[c]=Infinity,b.opacity.opacityValues[c]=a;else return g(c,"Could not convert opacity info: no field and no opacities/stops"),null;return b}function z(a,b,c){a=2===c&&"arithmetic"===a.rotationType;b.offset[c]=a?90:0;b.factor[c]=a?-1:1;b.type[c]=1}function R(a,b,c){if(!w(a.field))return g(c,"Could not convert rotation info: field is not a string"),null;if(!b.rotation)b.rotation={field:a.field,offset:[0,0,0],factor:[1,1,1],type:[0,
0,0]};else if(a.field)if(!b.rotation.field)b.rotation.field=a.field;else if(a.field!==b.rotation.field)return g(c,"Could not convert rotation info: multiple fields in use"),null;switch(a.axis){case "tilt":return z(a,b.rotation,0),b;case "roll":return z(a,b.rotation,1),b;case null:case void 0:case "heading":return z(a,b.rotation,2),b;default:return g(c,`Could not convert rotation info: unknown axis "${a.axis}""`),null}}function A(a,b,c){if(!a)return null;const d=!b.supportedTypes||!!b.supportedTypes.size,
e=!b.supportedTypes||!!b.supportedTypes.color,f=!b.supportedTypes||!!b.supportedTypes.rotation,h=!!b.supportedTypes&&!!b.supportedTypes.opacity,m=a.reduce((l,r)=>{if(!l)return l;if(r.valueExpression)return g(c,"Could not convert visual variables: arcade expressions not supported"),null;switch(r.type){case "size":return d?N(r,l,b,c):l;case "color":return e?P(r,l,c):l;case "opacity":return h?Q(r,l,c):null;case "rotation":return f?R(r,l,c):l;default:return null}},{size:null,color:null,opacity:null,rotation:null});
return 0<a.length&&m&&!m.size&&!m.color&&!m.opacity&&!m.rotation||m&&m.size&&!O(m.size,b,c)?null:m}function x(a,b,c){if(!!a!==!!b||a&&a.field!==b.field)return!1;if(a&&"rotation"===c)for(c=0;3>c;c++)if(a.type[c]!==b.type[c]||a.offset[c]!==b.offset[c]||a.factor[c]!==b.factor[c])return!1;return!0}function B(a,b){const c={vvSizeEnabled:!1,vvSizeMinSize:null,vvSizeMaxSize:null,vvSizeOffset:null,vvSizeFactor:null,vvSizeValue:null,vvColorEnabled:!1,vvColorValues:null,vvColorColors:null,vvOpacityEnabled:!1,
vvOpacityValues:null,vvOpacityOpacities:null,vvSymbolAnchor:null,vvSymbolRotationMatrix:null},d=a&&null!=a.size;a&&a.size?(c.vvSizeEnabled=!0,c.vvSizeMinSize=a.size.minSize,c.vvSizeMaxSize=a.size.maxSize,c.vvSizeOffset=a.size.offset,c.vvSizeFactor=a.size.factor):a&&d&&(c.vvSizeValue=b.transformation.scale);a&&d&&(c.vvSymbolAnchor=b.transformation.anchor,c.vvSymbolRotationMatrix=I.create(),k.identity(v),M(b.transformation.rotation[2],b.transformation.rotation[0],b.transformation.rotation[1],v),H.fromMat4(c.vvSymbolRotationMatrix,
v));a&&a.color&&(c.vvColorEnabled=!0,c.vvColorValues=a.color.values,c.vvColorColors=a.color.colors);a&&a.opacity&&(c.vvOpacityEnabled=!0,c.vvOpacityValues=a.opacity.values,c.vvOpacityOpacities=a.opacity.opacityValues);return c}var t;(function(a){const b=y.create(),c=K.create();a.evaluateModelTransform=function(d,e,f){if(!d.vvSizeEnabled)return f;k.copy(b,f);f=d.vvSymbolRotationMatrix;k.set(v,f[0],f[1],f[2],0,f[3],f[4],f[5],0,f[6],f[7],f[8],0,0,0,0,1);k.multiply(b,b,v);for(f=0;3>f;++f)c[f]=D.clamp(d.vvSizeOffset[f]+
e[0]*d.vvSizeFactor[f],d.vvSizeMinSize[f],d.vvSizeMaxSize[f]);k.scale(b,b,c);k.translate(b,b,d.vvSymbolAnchor);return b};a.evaluateModelTransformScale=function(d,e,f){if(!e.vvSizeEnabled)return J.set(d,1,1,1);for(let h=0;3>h;++h)d[h]=D.clamp(e.vvSizeOffset[h]+f[0]*e.vvSizeFactor[h],e.vvSizeMinSize[h],e.vvSizeMaxSize[h]);return d}})(t||(t={}));const v=y.create();C=t.evaluateModelTransform;t=t.evaluateModelTransformScale;p.convertVisualVariables=A;p.evaluateModelTransform=C;p.evaluateModelTransformScale=
t;p.getMaterialParams=B;p.initFastSymbolUpdatesState=function(a,b){return!a||L.TESTS_DISABLE_FAST_UPDATES?{enabled:!1}:(a=A(a.visualVariables,b))?{enabled:!0,visualVariables:a,materialParameters:B(a,b),requiresShaderTransformation:a&&null!=a.size}:{enabled:!1}};p.updateFastSymbolUpdatesState=function(a,b,c){if(!b||!a.enabled)return!1;const d=a.visualVariables;b=A(b.visualVariables,c);if(!(b&&x(d.size,b.size,"size")&&x(d.color,b.color,"color")&&x(d.rotation,b.rotation,"rotation")&&x(d.opacity,b.opacity,
"opacity")))return!1;a.visualVariables=b;a.materialParameters=B(b,c);a.requiresShaderTransformation=b&&null!=b.size;return!0};Object.defineProperty(p,"__esModule",{value:!0})});