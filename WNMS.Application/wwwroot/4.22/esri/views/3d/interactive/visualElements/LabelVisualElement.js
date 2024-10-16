// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../chunks/_rollupPluginBabelHelpers ../../../../core/Handles ../../../../core/maybe ../../../../core/screenUtils ../../../../chunks/vec2 ../../../../chunks/vec3 ../../../../chunks/vec3f64 ../measurementTools/support/viewUtils ./VisualElement ../../../overlay/LineOverlayItem ../../../overlay/TextOverlayItem".split(" "),function(v,B,J,C,k,e,K,w,L,x,M,N){function y(n,p,f,a){n.eval(p,z,D);K.add(E,z,D);f.projectToRenderScreen(z,F);f.projectToRenderScreen(E,G);e.subtract(a,O,P);
e.normalize(a,a)}x=function(n){function p(a){var c=n.call(this,a.view)||this;c._handles=new J;c._textItem=null;c._calloutItem=null;c._showCallout=!0;c._showText=!0;c._geometry=null;c._text=null;c._fontSize=14;c._distance=25;c._anchor="right";c.applyProps(a);return c}B._inheritsLoose(p,n);var f=p.prototype;f.overlaps=function(a){return this.attached?this.textItem.visible&&a.textItem.visible&&this.view.overlay.overlaps(this._textItem,a.textItem):!1};f._updateLabelPosition=function(){if(this.attached){this._showCallout=
this._showText=!1;if(C.isSome(this.geometry)&&this.view._stage)switch(this.geometry.type){case "point":if(this._computeLabelPositionFromPoint(this.geometry.point,h)){if(this.geometry.callout){const a=this.view.state.camera,c=this.geometry.callout.distance;e.add(d,d,[0,this.geometry.callout.offset]);a.renderToScreen(d,h);e.set(b,0,1);e.scale(b,b,c*a.pixelRatio);e.add(b,b,d);a.renderToScreen(b,q);this._showCallout=this._updatePosition(h,q)}else this._textItem.position=[h[0],h[1]],this._textItem.anchor=
"center";this._showText=!0}break;case "corner":this._computeLabelPositionFromCorner(this.geometry,this._distance,h,q)&&(this._showCallout=this._updatePosition(h,q),this._showText=!0);break;case "segment":this._computeLabelPositionFromSegment(this.geometry,this._distance,this._anchor,h,q)&&(this._showText=!0,this._showCallout=this._updatePosition(h,q))}this.updateVisibility(this.visible)}};f._computeLabelPositionFromPoint=function(a,c){const g=this.view.state.camera;g.projectToRenderScreen(a,d);if(0>
d[2]||1<d[2])return!1;g.renderToScreen(d,c);return!0};f._computeLabelPositionFromCorner=function(a,c,g,r){if(!a)return!1;const l=this.view.state.camera;y(a.left,1,l,m);e.negate(m,m);y(a.right,0,l,H);e.add(b,m,H);e.negate(b,b);e.normalize(b,b);l.projectToRenderScreen(a.left.endRenderSpace,d);if(0>d[2]||1<d[2])return!1;l.renderToScreen(d,g);e.scale(b,b,c*l.pixelRatio);e.add(b,b,d);l.renderToScreen(b,r);return!0};f._computeLabelPositionFromSegment=function(a,c,g,r,l){if(!a)return!1;const A=a.segment,
t=this.view.state.camera;L.screenSpaceTangent(A.startRenderSpace,A.endRenderSpace,m,t);e.set(b,-m[1],m[0]);let u=!1;switch(g){case "top":u=0>b[1];break;case "bottom":u=0<b[1];break;case "left":u=0<b[0];break;case "right":u=0>b[0]}u&&e.negate(b,b);if(0===e.length(b))switch(g){case "top":b[1]=1;break;case "bottom":b[1]=-1;break;case "left":b[0]=-1;break;case "right":b[0]=1}A.eval(Q[a.sampleLocation],I);t.projectToRenderScreen(I,d);if(0>d[2]||1<d[2])return!1;t.renderToScreen(d,r);e.scale(b,b,c*t.pixelRatio);
e.add(b,b,d);t.renderToScreen(b,l);return!0};f._updatePosition=function(a,c){if(c){const g=c[0]-a[0],r=c[1]-a[1];this._textItem.position=[c[0],c[1]];this._textItem.anchor=Math.abs(g)>Math.abs(r)?0<g?"left":"right":0<r?"top":"bottom";this._calloutItem.startPosition=[a[0],a[1]];this._calloutItem.endPosition=[c[0],c[1]];return!0}this._textItem.position=[a[0],a[1]];this._textItem.anchor="center";return!1};f.createResources=function(){this._textItem=new N({visible:!0});this._textItem.text=C.unwrap(this._text);
this._textItem.fontSize=this._fontSize;this._calloutItem=new M({visible:!0,width:2});this._updateLabelPosition();this.view.overlay.items.addMany([this._textItem,this._calloutItem]);this._handles.add(this.view.state.watch("camera",()=>this._updateLabelPosition()))};f.destroyResources=function(){this.view.overlay&&!this.view.overlay.destroyed&&this.view.overlay.items.removeMany([this._textItem,this._calloutItem]);this._handles.removeAll()};f.updateVisibility=function(a){this._textItem.visible=this._showText&&
a;this._calloutItem.visible=this._showCallout&&a};B._createClass(p,[{key:"geometry",get:function(){return this._geometry},set:function(a){this._geometry=a;this._updateLabelPosition()}},{key:"textItem",get:function(){return this._textItem}},{key:"text",get:function(){return this._text},set:function(a){this._text=a;this.attached&&(this._textItem.text=this._text)}},{key:"fontSize",get:function(){return this._fontSize},set:function(a){this._fontSize=a;this.attached&&(this._textItem.fontSize=this._fontSize)}},
{key:"distance",get:function(){return this._distance},set:function(a){this._distance!==a&&(this._distance=a,this._updateLabelPosition())}},{key:"anchor",get:function(){return this._anchor},set:function(a){this._anchor!==a&&(this._anchor=a,this._updateLabelPosition())}}]);return p}(x.VisualElement);const z=w.create(),E=w.create(),D=w.create(),m=k.createRenderScreenPointArray(),H=k.createRenderScreenPointArray(),b=k.createRenderScreenPointArray(),I=w.create(),d=k.createRenderScreenPointArray3(),h=k.createScreenPointArray(),
q=k.createScreenPointArray(),F=k.createRenderScreenPointArray3(),P=F,G=k.createRenderScreenPointArray3(),O=G,Q={start:0,center:.5,end:1};v.LabelVisualElement=x;v.mirrorPosition=function(n){switch(n){case "top":return"bottom";case "right":return"left";case "bottom":return"top";case "left":return"right"}};v.screenSpaceTangent=y;Object.defineProperty(v,"__esModule",{value:!0})});