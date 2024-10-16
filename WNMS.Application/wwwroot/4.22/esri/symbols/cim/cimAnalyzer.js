// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../chunks/_rollupPluginBabelHelpers ../../Color ../../core/Logger ../../core/maybe ../../core/screenUtils ../../core/string ../../support/arcadeOnDemand ./CIMSymbolHelper ./SDFHelper ./utils ./effects/CIMEffectHelper ../../views/2d/arcade/callExpressionWithFeature".split(" "),function(R,ka,la,ma,Y,na,A,S,C,Q,u,Z,aa){function T(a){switch(a){case "Butt":return 0;case "Square":return 2;default:return 1}}function U(a){switch(a){case "Bevel":return 0;case "Miter":return 2;default:return 1}}
function oa(a){switch(a){default:return"left";case "Right":return"right";case "Center":return"center";case "Justify":return"justify"}}function pa(a){switch(a){default:return"top";case "Center":return"middle";case "Baseline":return"baseline";case "Bottom":return"bottom"}}function qa(a){let g="",c="";a&&(a=a.toLowerCase(),-1!==a.indexOf("italic")?g="italic":-1!==a.indexOf("oblique")&&(g="oblique"),-1!==a.indexOf("bold")?c="bold":-1!==a.indexOf("light")&&(c="lighter"));return{style:g,weight:c}}function ba(a,
g,c,d){let b;a[g]?b=a[g]:(b={},a[g]=b);b[c]=d}function ca(a){return(a=a.markerPlacement)&&a.angleToLine?1:0}function V(){V=ka._asyncToGenerator(function*(a,g,c,d,b){d=null!=d?d:[];if(!a)return d;let e;const r={};if("CIMSymbolReference"===a.type){if(e=a.symbol,a=a.primitiveOverrides){var k=[];for(const l of a){var n=l.valueExpressionInfo;n&&g?(n=S.createRendererExpression(n.expression,g.spatialReference,g.fields).then(q=>{q&&ba(r,l.primitiveName,l.propertyName,q)}),k.push(n)):null!=l.value&&ba(r,l.primitiveName,
l.propertyName,l.value)}0<k.length&&(yield Promise.all(k))}}else return da.error("Expect cim type to be 'CIMSymbolReference'"),d;k=[];ea(e,c,k);0<k.length&&(yield Promise.all(k));switch(e.type){case "CIMPointSymbol":case "CIMLineSymbol":case "CIMPolygonSymbol":ra(e,a,r,g,d,c,b)}return d});return V.apply(this,arguments)}function ra(a,g,c,d,b,e,r){if(a){var k=a.symbolLayers;if(k){var n=a.effects,l,q=C.CIMSymbolHelper.getSize(a);"CIMPointSymbol"===a.type&&"Map"===a.angleAlignment&&(l=1);for(var p=k.length;p--;){var f=
k[p];if(!f||!1===f.enable)continue;let y;n&&n.length&&(y=[...n]);var t=f.effects;t&&t.length&&(n?y.push(...t):y=[...t]);var m=[];C.OverrideHelper.findApplicableOverrides(f,g,m);switch(f.type){case "CIMSolidFill":sa(f,y,c,m,d,b);break;case "CIMPictureFill":ta(f,y,c,m,d,e,b);break;case "CIMHatchFill":ua(f,y,c,m,d,b);break;case "CIMGradientFill":{t=y;var v=c,w=d,z=b;const [E,G]=K(m,f.primitiveName);var x=A.numericHash(JSON.stringify(f)+G).toString();z.push({type:"fill",templateHash:x,materialHash:E?
P(x,v,m,w):x,cim:f,materialOverrides:null,colorLocked:f.colorLocked,effects:t,color:{r:128,g:128,b:128,a:1},height:0,angle:0,offsetX:0,offsetY:0,scaleX:1})}break;case "CIMSolidStroke":va(f,y,c,m,d,b,"CIMPolygonSymbol"===a.type,q);break;case "CIMPictureStroke":wa(f,y,c,m,d,b,"CIMPolygonSymbol"===a.type,q);break;case "CIMGradientStroke":{t=y;v=c;w=d;z=b;x="CIMPolygonSymbol"===a.type;var D=q,B=f.primitiveName,F=void 0!==f.width?f.width:4,H=T(f.capStyle),I=U(f.joinStyle),J=f.miterLimit;const [E,G]=K(m,
B),L=A.numericHash(JSON.stringify(f)+G).toString();z.push({type:"line",templateHash:L,materialHash:E?P(L,v,m,w):L,cim:f,materialOverrides:null,isOutline:x,colorLocked:f.colorLocked,effects:t,color:{r:128,g:128,b:128,a:1},width:h(B,v,"Width",w,F),cap:h(B,v,"CapStyle",w,H),join:h(B,v,"JoinStyle",w,I),miterLimit:h(B,v,"MiterLimit",w,J),referenceWidth:D,zOrder:W(f.name),dashTemplate:null,scaleDash:!1})}break;case "CIMCharacterMarker":X(f,y,c,m,d,b);break;case "CIMPictureMarker":if(X(f,y,c,m,d,b))break;
"CIMLineSymbol"===a.type&&(l=ca(f));xa(f,y,c,m,d,e,b,l,q);break;case "CIMVectorMarker":if(X(f,y,c,m,d,b))break;"CIMLineSymbol"===a.type&&(l=ca(f));t=y;v=c;w=d;z=b;x=e;D=l;B=q;F=r;if(I=f.markerGraphics){H=0;f.scaleSymbolsProportionally&&(J=f.frame)&&(H=J.ymax-J.ymin);for(const E of I)if(E&&(I=E.symbol))switch(I.type){case "CIMPointSymbol":case "CIMLineSymbol":case "CIMPolygonSymbol":ya(f,t,E,m,v,w,z,x,D,B,H,F);break;case "CIMTextSymbol":za(f,t,E,v,m,w,z,D,B,H)}}break;default:da.error("Cannot analyze CIM layer",
f.type)}}}}}function sa(a,g,c,d,b,e){const r=a.primitiveName,k=u.fromCIMColor(a.color),[n,l]=K(d,r),q=A.numericHash(JSON.stringify(a)+l).toString();e.push({type:"fill",templateHash:q,materialHash:n?()=>q:q,cim:a,materialOverrides:null,colorLocked:a.colorLocked,color:h(r,c,"Color",b,k,N),height:0,angle:0,offsetX:0,offsetY:0,scaleX:1,effects:g})}function ta(a,g,c,d,b,e,r){const k=a.primitiveName,n=a.tintColor?u.fromCIMColor(a.tintColor):{r:255,g:255,b:255,a:1},[l,q]=K(d,k);d=A.numericHash(JSON.stringify(a)+
q).toString();const p=A.numericHash(`${a.url}${JSON.stringify(a.colorSubstitutions)}`).toString();let f=u.getValue(a.scaleX);if("width"in a){const t=a.width;let m=1;e=e.getResource(a.url);Y.isSome(e)&&(m=e.width/e.height);f/=a.height/t*m}r.push({type:"fill",templateHash:d,materialHash:l?()=>p:p,cim:a,materialOverrides:null,colorLocked:a.colorLocked,effects:g,color:h(k,c,"TintColor",b,n,N),height:h(k,c,"Height",b,a.height),scaleX:h(k,c,"ScaleX",b,f),angle:h(k,c,"Rotation",b,u.getValue(a.rotation)),
offsetX:h(k,c,"OffsetX",b,u.getValue(a.offsetX)),offsetY:h(k,c,"OffsetY",b,u.getValue(a.offsetY)),url:a.url})}function ua(a,g,c,d,b,e){const r=["Rotation","OffsetX","OffsetY"],k=d.filter(f=>f.primitiveName!==a.primitiveName&&-1===r.indexOf(f.propertyName)),n=a.primitiveName,[l,q]=K(d,n);d=A.numericHash(JSON.stringify(a)+q).toString();const p=A.numericHash(`${a.separation}${JSON.stringify(a.lineSymbol)}`).toString();e.push({type:"fill",templateHash:d,materialHash:l?P(p,c,k,b):p,cim:a,materialOverrides:k,
colorLocked:a.colorLocked,effects:g,color:{r:255,g:255,b:255,a:1},height:h(n,c,"Separation",b,a.separation),scaleX:1,angle:h(n,c,"Rotation",b,u.getValue(a.rotation)),offsetX:h(n,c,"OffsetX",b,u.getValue(a.offsetX)),offsetY:h(n,c,"OffsetY",b,u.getValue(a.offsetY))})}function va(a,g,c,d,b,e,r,k){const n=a.primitiveName,l=u.fromCIMColor(a.color),q=void 0!==a.width?a.width:4,p=T(a.capStyle),f=U(a.joinStyle),t=a.miterLimit,[m,v]=K(d,n),w=A.numericHash(JSON.stringify(a)+v).toString();let z;if(g&&0<g.length&&
(d=g[g.length-1],"CIMGeometricEffectDashes"===d.type&&"NoConstraint"===d.lineDashEnding)){g=[...a.effects];var x=g.pop();z=x.dashTemplate;x=x.scaleDash}e.push({type:"line",templateHash:w,materialHash:m?()=>w:w,cim:a,materialOverrides:null,isOutline:r,colorLocked:a.colorLocked,effects:g,color:h(n,c,"Color",b,l,N),width:h(n,c,"Width",b,q),cap:h(n,c,"CapStyle",b,p),join:h(n,c,"JoinStyle",b,f),miterLimit:h(n,c,"MiterLimit",b,t),referenceWidth:k,zOrder:W(a.name),dashTemplate:z,scaleDash:x})}function wa(a,
g,c,d,b,e,r,k){const n=A.numericHash(`${a.url}${JSON.stringify(a.colorSubstitutions)}`).toString(),l=a.primitiveName,q=u.fromCIMColor(a.tintColor),p=void 0!==a.width?a.width:4,f=T(a.capStyle),t=U(a.joinStyle),m=a.miterLimit,[v,w]=K(d,l);d=A.numericHash(JSON.stringify(a)+w).toString();e.push({type:"line",templateHash:d,materialHash:v?()=>n:n,cim:a,materialOverrides:null,isOutline:r,colorLocked:a.colorLocked,effects:g,color:h(l,c,"TintColor",b,q,N),width:h(l,c,"Width",b,p),cap:h(l,c,"CapStyle",b,f),
join:h(l,c,"JoinStyle",b,t),miterLimit:h(l,c,"MiterLimit",b,m),referenceWidth:k,zOrder:W(a.name),dashTemplate:null,scaleDash:!1,url:a.url})}function X(a,g,c,d,b,e){const r=a.markerPlacement;if(!r||"CIMMarkerPlacementInsidePolygon"!==r.type)return!1;const k=["Rotation","OffsetX","OffsetY"],n=d.filter(v=>v.primitiveName!==a.primitiveName&&-1===k.indexOf(v.propertyName)),l="url"in a?a.url:null,[q,p]=K(d,r.primitiveName);d=A.numericHash(JSON.stringify(a)+p).toString();let f=r.stepY,t=null,m=1;r.shiftOddRows&&
(f*=2,t=function(v){return v?2*v:0},m=.5);e.push({type:"fill",templateHash:d,materialHash:q?P(d,c,n,b):d,cim:a,materialOverrides:n,colorLocked:a.colorLocked,effects:g,color:{r:255,g:255,b:255,a:1},height:h(r.primitiveName,c,"StepY",b,f,t),scaleX:m,angle:h(r.primitiveName,c,"GridAngle",b,r.gridAngle),offsetX:h(r.primitiveName,c,"OffsetX",b,u.getValue(r.offsetX)),offsetY:h(r.primitiveName,c,"OffsetY",b,u.getValue(r.offsetY)),url:l});return!0}function xa(a,g,c,d,b,e,r,k,n){var l;const q=a.primitiveName,
p=u.getValue(a.size);let f=u.getValue(a.scaleX);const t=u.getValue(a.rotation),m=u.getValue(a.offsetX),v=u.getValue(a.offsetY),w=a.tintColor?u.fromCIMColor(a.tintColor):{r:255,g:255,b:255,a:1},z=A.numericHash(`${a.url}${JSON.stringify(a.colorSubstitutions)}`).toString(),[x,D]=K(d,q);d=A.numericHash(JSON.stringify(a)+D).toString();const B=null!=(l=a.anchorPoint)?l:{x:0,y:0};if("width"in a){l=a.width;let F=1;e=e.getResource(a.url);Y.isSome(e)&&(F=e.width/e.height);f/=p/l*F}r.push({type:"marker",templateHash:d,
materialHash:x?()=>z:z,cim:a,materialOverrides:null,colorLocked:a.colorLocked,effects:g,scaleSymbolsProportionally:!1,alignment:k,size:h(q,c,"Size",b,p),scaleX:h(q,c,"ScaleX",b,f),rotation:h(q,c,"Rotation",b,t),offsetX:h(q,c,"OffsetX",b,m),offsetY:h(q,c,"OffsetY",b,v),color:h(q,c,"TintColor",b,w,N),anchorPoint:{x:B.x,y:-B.y},isAbsoluteAnchorPoint:"Relative"!==a.anchorPointUnits,outlineColor:{r:0,g:0,b:0,a:0},outlineWidth:0,frameHeight:0,rotateClockwise:a.rotateClockwise,referenceSize:n,sizeRatio:1,
markerPlacement:a.markerPlacement,url:a.url})}function za(a,g,c,d,b,e,r,k,n,l){C.OverrideHelper.findApplicableOverrides(c,b,[]);var q=c.geometry;if("x"in q&&"y"in q){var p=c.symbol;var f=p.underline?"underline":p.strikethrough?"line-through":"none";var t=qa(p.fontStyleName),m=u.toKebabCase(p.fontFamilyName);p.font={family:m,decoration:f,...t};var v=a.frame,w=q.x-.5*(v.xmin+v.xmax),z=q.y-.5*(v.ymin+v.ymax),x=a.size/l;l=a.primitiveName;q=u.getValue(p.height)*x;v=u.getValue(p.angle);w=(u.getValue(p.offsetX)+
w)*x;z=(u.getValue(p.offsetY)+z)*x;var D=u.fromCIMColor(C.CIMSymbolHelper.getFillColor(p)),B=u.fromCIMColor(C.CIMSymbolHelper.getStrokeColor(p)),F=C.CIMSymbolHelper.getStrokeWidth(p);F||(B=u.fromCIMColor(C.CIMSymbolHelper.getFillColor(p.haloSymbol)),F=p.haloSize*x);var [H,I]=K(b,l);b=JSON.stringify(a.effects)+Number(a.colorLocked)+JSON.stringify(a.anchorPoint)+a.anchorPointUnits+JSON.stringify(a.markerPlacement);b=A.numericHash(JSON.stringify(c)+b+I).toString();var J=A.numericHash(JSON.stringify(p.font)).toString(),
y=h(c.primitiveName,d,"TextString",e,c.textString,u._adjustTextCase,p.textCase);({fontStyleName:c}=p);m+=c?"-"+c.toLowerCase():"-regular";r.push({type:"text",templateHash:b,materialHash:H||"function"===typeof y||y.match(/\[([\w]+)\]/)?(E,G,L)=>J+"-"+u.evaluateValueOrFunction(y,E,G,L):J+"-"+A.numericHash(y),cim:p,materialOverrides:null,colorLocked:a.colorLocked,effects:g,alignment:k,anchorPoint:{x:a.anchorPoint?a.anchorPoint.x:0,y:a.anchorPoint?a.anchorPoint.y:0},isAbsoluteAnchorPoint:"Relative"!==
a.anchorPointUnits,fontName:m,decoration:f,weight:h(l,d,"Weight",e,t.weight),style:h(l,d,"Size",e,t.style),size:h(l,d,"Size",e,q),angle:h(l,d,"Rotation",e,v),offsetX:h(l,d,"OffsetX",e,w),offsetY:h(l,d,"OffsetY",e,z),horizontalAlignment:oa(p.horizontalAlignment),verticalAlignment:pa(p.verticalAlignment),text:y,color:D,outlineColor:B,outlineSize:F,referenceSize:n,sizeRatio:1,markerPlacement:a.markerPlacement})}}function ya(a,g,c,d,b,e,r,k,n,l,q,p){var f=c.symbol;const t=f.symbolLayers;if(t)if(p)fa(a,
g,c,b,d,e,r,k,n,l,q);else if(p=t.length,t&&2===t.length&&t[0].enable&&t[1].enable&&"CIMSolidStroke"===t[0].type&&"CIMSolidFill"===t[1].type&&!t[0].effects&&!t[1].effects)Aa(a,c,t,d,b,e,r,n,l,q);else if(f=Z.CIMEffectHelper.applyEffects(f.effects,c.geometry))for(;p--;){var m=t[p];if(m&&!1!==m.enable)switch(m.type){case "CIMSolidFill":case "CIMSolidStroke":{var v,w=Z.CIMEffectHelper.applyEffects(m.effects,f),z=Q.getExtent(w);if(!z)continue;const [B,F,H]=Q.getSDFMetrics(z,a.frame,a.size,a.anchorPoint,
"Relative"!==a.anchorPointUnits);var x="CIMSolidFill"===m.type;w={type:"sdf",geom:w,asFill:x};var D=a.primitiveName;z=null!=(v=u.getValue(a.size))?v:10;const I=u.getValue(a.rotation),J=u.getValue(a.offsetX),y=u.getValue(a.offsetY),E=m.path,G=m.primitiveName,L=x?u.fromCIMColor(C.CIMSymbolHelper.getFillColor(m)):u.fromCIMColor(C.CIMSymbolHelper.getStrokeColor(m)),M=x?{r:0,g:0,b:0,a:0}:u.fromCIMColor(C.CIMSymbolHelper.getStrokeColor(m)),ha=C.CIMSymbolHelper.getStrokeWidth(m);if(!x&&!ha)break;x=!1;let ia=
"";for(const O of d)if(O.primitiveName===G||O.primitiveName===D)void 0!==O.value?ia+=`-${O.primitiveName}-${O.propertyName}-${JSON.stringify(O.value)}`:O.valueExpressionInfo&&(x=!0);D=JSON.stringify({...a,markerGraphics:null});const ja=A.numericHash(JSON.stringify(w)+E).toString();m={type:"marker",templateHash:A.numericHash(JSON.stringify(c)+JSON.stringify(m)+D+ia).toString(),materialHash:x?()=>ja:ja,cim:w,materialOverrides:null,colorLocked:a.colorLocked,effects:g,scaleSymbolsProportionally:a.scaleSymbolsProportionally,
alignment:n,anchorPoint:{x:F,y:H},isAbsoluteAnchorPoint:!1,size:h(a.primitiveName,b,"Size",e,z),rotation:h(a.primitiveName,b,"Rotation",e,I),offsetX:h(a.primitiveName,b,"OffsetX",e,J),offsetY:h(a.primitiveName,b,"OffsetY",e,y),scaleX:1,frameHeight:q,rotateClockwise:a.rotateClockwise,referenceSize:l,sizeRatio:B,color:h(G,b,"Color",e,L,N),outlineColor:h(G,b,"Color",e,M,N),outlineWidth:h(G,b,"Width",e,ha),markerPlacement:a.markerPlacement,path:E};r.push(m);break}default:fa(a,g,c,b,d,e,r,k,n,l,q)}}}function Aa(a,
g,c,d,b,e,r,k,n,l){var q=g.geometry;const p=c[0];c=c[1];var f=Q.getExtent(q);if(f){var [t,m,v]=Q.getSDFMetrics(f,a.frame,a.size,a.anchorPoint,"Relative"!==a.anchorPointUnits);q={type:"sdf",geom:q,asFill:!0};var w=a.primitiveName;f=u.getValue(a.size);var z=u.getValue(a.rotation),x=u.getValue(a.offsetX),D=u.getValue(a.offsetY),B=c.path,F=c.primitiveName,H=p.primitiveName,I=u.fromCIMColor(C.CIMSymbolHelper.getFillColor(c)),J=u.fromCIMColor(C.CIMSymbolHelper.getStrokeColor(p)),y=C.CIMSymbolHelper.getStrokeWidth(p),
E=!1,G="";for(const M of d)if(M.primitiveName===F||M.primitiveName===H||M.primitiveName===w)void 0!==M.value?G+=`-${M.primitiveName}-${M.propertyName}-${JSON.stringify(M.value)}`:M.valueExpressionInfo&&(E=!0);d=JSON.stringify({...a,markerGraphics:null});var L=A.numericHash(JSON.stringify(q)+B).toString();a={type:"marker",templateHash:A.numericHash(JSON.stringify(g)+JSON.stringify(c)+JSON.stringify(p)+d+G).toString(),materialHash:E?()=>L:L,cim:q,materialOverrides:null,colorLocked:a.colorLocked,effects:a.effects,
scaleSymbolsProportionally:a.scaleSymbolsProportionally,alignment:k,anchorPoint:{x:m,y:v},isAbsoluteAnchorPoint:!1,size:h(a.primitiveName,b,"Size",e,f),rotation:h(a.primitiveName,b,"Rotation",e,z),offsetX:h(a.primitiveName,b,"OffsetX",e,x),offsetY:h(a.primitiveName,b,"OffsetY",e,D),scaleX:1,frameHeight:l,rotateClockwise:a.rotateClockwise,referenceSize:n,sizeRatio:t,color:h(F,b,"Color",e,I,N),outlineColor:h(H,b,"Color",e,J,N),outlineWidth:h(H,b,"Width",e,y),markerPlacement:a.markerPlacement,path:B};
r.push(a)}}function fa(a,g,c,d,b,e,r,k,n,l,q){c={type:a.type,enable:!0,name:a.name,colorLocked:a.colorLocked,primitiveName:a.primitiveName,anchorPoint:a.anchorPoint,anchorPointUnits:a.anchorPointUnits,offsetX:0,offsetY:0,rotateClockwise:a.rotateClockwise,rotation:0,size:a.size,billboardMode3D:a.billboardMode3D,depth3D:a.depth3D,frame:a.frame,markerGraphics:[c],scaleSymbolsProportionally:a.scaleSymbolsProportionally,respectFrame:a.respectFrame,clippingPath:a.clippingPath};let p=[];const f=["Rotation",
"OffsetX","OffsetY"];p=b.filter(D=>D.primitiveName!==a.primitiveName||-1===f.indexOf(D.propertyName));var t="";for(var m of b)void 0!==m.value&&(t+=`-${m.primitiveName}-${m.propertyName}-${JSON.stringify(m.value)}`);const [v,w,z]=C.CIMSymbolHelper.getTextureAnchor(c,k);b=a.primitiveName;k=u.getValue(a.rotation);m=u.getValue(a.offsetX);const x=u.getValue(a.offsetY);t=A.numericHash(JSON.stringify(c)+t).toString();g={type:"marker",templateHash:t,materialHash:0===p.length?t:P(t,d,p,e),cim:c,materialOverrides:p,
colorLocked:a.colorLocked,effects:g,scaleSymbolsProportionally:a.scaleSymbolsProportionally,alignment:n,anchorPoint:{x:v,y:w},isAbsoluteAnchorPoint:!1,size:a.size,rotation:h(b,d,"Rotation",e,k),offsetX:h(b,d,"OffsetX",e,m),offsetY:h(b,d,"OffsetY",e,x),color:{r:255,g:255,b:255,a:1},outlineColor:{r:0,g:0,b:0,a:0},outlineWidth:0,scaleX:1,frameHeight:q,rotateClockwise:a.rotateClockwise,referenceSize:l,sizeRatio:z/na.pt2px(a.size),markerPlacement:a.markerPlacement};r.push(g)}function W(a){return a&&0===
a.indexOf("Level_")?parseInt(a.substr(6),10):0}function N(a){if(!a||0===a.length)return null;a=(new la(a)).toRgba();return{r:a[0],g:a[1],b:a[2],a:a[3]}}function h(a,g,c,d,b,e,r){if(a=g[a]){const k=a[c];if("string"===typeof k||"number"===typeof k||k instanceof Array)return e?e.call(null,k,r):k;if(null!=k&&k instanceof S.ArcadeExpression)return(n,l,q)=>{n=aa(k,n,{$view:q},d.geometryType,l);null!==n&&e&&(n=e.call(null,n,r));return null!==n?n:b}}return b}function P(a,g,c,d){for(const b of c)if(b.valueExpressionInfo){const e=
g[b.primitiveName]&&g[b.primitiveName][b.propertyName];e instanceof S.ArcadeExpression&&(b.fn=(r,k,n)=>aa(e,r,{$view:n},d.geometryType,k))}return(b,e,r)=>{for(const k of c)k.fn&&(k.value=k.fn(b,e,r));return A.numericHash(a+C.OverrideHelper.buildOverrideKey(c)).toString()}}function K(a,g){let c=!1,d="";for(const b of a)b.primitiveName===g&&(void 0!==b.value?d+=`-${b.primitiveName}-${b.propertyName}-${JSON.stringify(b.value)}`:b.valueExpressionInfo&&(c=!0));return[c,d]}function ea(a,g,c){if(a&&g)switch(a.type){case "CIMPointSymbol":case "CIMLineSymbol":case "CIMPolygonSymbol":if(a=
a.symbolLayers)for(const d of a)switch(d.type){case "CIMPictureFill":case "CIMHatchFill":case "CIMGradientFill":case "CIMPictureStroke":case "CIMGradientStroke":case "CIMCharacterMarker":case "CIMPictureMarker":"url"in d&&d.url&&c.push(g.fetchResource(d.url,null));break;case "CIMVectorMarker":if(a=d.markerGraphics)for(const b of a)b&&(a=b.symbol)&&ea(a,g,c)}}}const da=ma.getLogger("esri.symbols.cim.cimAnalyzer");R.analyzeCIMResource=function(a,g){if(!g||0===g.length)return a;a=JSON.parse(JSON.stringify(a));
C.OverrideHelper.applyOverrides(a,g);return a};R.analyzeCIMSymbol=function(a,g,c,d,b){return V.apply(this,arguments)};Object.defineProperty(R,"__esModule",{value:!0})});