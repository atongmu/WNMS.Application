/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import e from"../Color.js";import{t,a as s}from"./colorUtils2.js";import{b as r}from"./vec4f64.js";import{d as i,R as o,E as a}from"./lineStippleUtils.js";import{E as n}from"./Evented.js";import{i as l,b as h,u as c}from"../core/lang.js";import{u as d,w as u,e as p,g as m,f as _,z as f,b as g}from"./mathUtils.js";import{f as y,a as w}from"./vec4f32.js";import{projectBuffer as v}from"../geometry/projection.js";import{c as C,b,n as R}from"./aaBoundingBox.js";import{m as O}from"./dehydratedFeatures.js";import{f as G}from"./elevationInfoUtils.js";import{E as P}from"./ElevationInfo.js";import{_ as A}from"./tslib.es6.js";import j from"../core/Accessor.js";import{I as D}from"./Identifiable.js";import{property as F}from"../core/accessorSupport/decorators/property.js";import"./ensureType.js";import{subclass as S}from"../core/accessorSupport/decorators/subclass.js";import{L as W}from"./LaserlineVisualElement.js";import{init as E}from"../core/watchUtils.js";import{W as x,O as M}from"./ScreenSpacePass.js";import{y as z,E as T,B as V,z as I}from"./lineUtils.js";import{c as B}from"./geometryDataUtils.js";class U{constructor(e){this.resourceFactory=e,this._resources=null,this._visible=!0,this._attached=!1}destroy(){this._destroyResources()}get object(){return l(this._resources)?this._resources.object:null}get resources(){return l(this._resources)?this._resources.external:null}get visible(){return this._visible}set visible(e){e!==this._visible&&(this._visible=e,this._syncVisible())}get attached(){return this._attached}set attached(e){e!==this._attached&&(this._attached=e,this._createOrDestroyResources())}recreate(){this.attached&&this._createResources()}recreateGeometry(){if(!this.resourceFactory.recreateGeometry)return void this.recreate();const e=this.resourceFactory.view._stage;if(h(this._resources)||!e)return;const t=this._resources.object;this._resources.external.forEach((t=>{2===t.type&&e.remove(t)})),t.removeAllGeometries();const s=this.resourceFactory.recreateGeometry(this._resources.external,t,this._resources.layer);e.addMany(s)}_createOrDestroyResources(){this._attached?this._resources||this._createResources():this._destroyResources()}_createResources(){this._destroyResources();const e=this.resourceFactory.view._stage;if(!e)return;const t=new x({isPickable:!1,updatePolicy:1});e.add(t);const s=new M({castShadow:!1}),r=this.resourceFactory.createResources(s,t);r.forEach((t=>e.add(t))),e.add(s),t.add(s),this._syncVisible();const i=this.resourceFactory.cameraChanged?E(this.resourceFactory.view.state,"camera",(e=>this.resourceFactory.cameraChanged(e))):null;this._resources={layer:t,object:s,external:r,cameraHandle:i}}_destroyResources(){if(h(this._resources))return;const e=this.resourceFactory.view._stage;null==e||e.remove(this._resources.object),null==e||e.remove(this._resources.layer),this._resources.external.forEach((t=>{null==e||e.remove(t),"dispose"in t&&t.dispose()})),this._resources.object.dispose(),this._resources.cameraHandle&&this._resources.cameraHandle.remove(),this._resources=null}_syncVisible(){h(this._resources)||this._resources.object.setVisible(this._visible)}}const H={main:new e([255,127,0]),selected:new e([255,255,255]),staged:new e([12,207,255]),outline:new e([0,0,0,.5]),selectedOutline:new e([255,255,255])};function L(e,t){const s=e.clone();return s.a*=t,s}function q(e,r){const i=e.clone(),o=t(i);o.s*=r;const a=s(o);return i.r=a.r,i.g=a.g,i.b=a.b,i}function k(e,t){if(t)for(const s in t)e[s]=t[s]}class J{constructor(e){this.color=H.main,this.height=90,this.coneHeight=40,this.coneWidth=23,this.width=3,this.renderOccluded=16,k(this,e)}}class K{constructor(e){this.size=11,this.outlineSize=1,this.collisionPadding=9,this.color=H.main,this.outlineColor=H.outline,this.renderOccluded=16,this.hoverOutlineColor=H.selectedOutline,k(this,e)}apply(e,t){const s=this[e];t.setParameters({color:re(s),transparent:"color"!==e||s.a<1,renderOccluded:this.renderOccluded})}}class N{constructor(e){this.size=40,this.height=.2,this.offset=.25,this.collisionPadding=2,this.color=L(H.main,.5),this.hoverColor=H.main,this.renderOccluded=2,this.minSquaredEdgeLength=900,k(this,e)}apply(e,t){const s=this[e];t.setParameters({color:re(s),transparent:s.a<1,renderOccluded:this.renderOccluded})}}class Q{constructor(e){this.vertex=new K({color:H.main,outlineColor:H.outline}),this.edge=new K({color:q(L(H.main,2/3),.5),outlineColor:L(H.outline,.5),size:8,collisionPadding:8}),this.selected=new K({color:H.selected,outlineColor:H.outline}),this.edgeOffset=new N,k(this,e)}}class X{constructor(e){this.color=H.selected,this.width=1.5,this.stipplePattern=i(5),this.stippleOffColor=H.outline,this.falloff=0,this.innerWidth=1.5,this.innerColor=H.selected,this.renderOccluded=4,k(this,e)}apply(e){e.color=re(this.color),e.width=this.width,e.stipplePattern=this.stipplePattern,e.stippleOffColor=re(this.stippleOffColor),e.falloff=this.falloff,e.innerWidth=this.innerWidth,e.innerColor=re(this.innerColor),e.renderOccluded=this.renderOccluded}}class Y{constructor(e){this.color=H.selected,this.size=4,this.outlineSize=1,this.outlineColor=H.outline,this.shape="square",k(this,e)}apply(e){e.color=re(this.color),e.size=this.size,e.outlineSize=this.outlineSize,e.outlineColor=re(this.outlineColor),e.primitive=this.shape}}class Z{constructor(e){this.innerColor=H.selected,this.innerWidth=1,this.glowColor=H.main,this.glowWidth=8,this.glowFalloff=8,this.globalAlpha=.3,this.globalAlphaContrastBoost=1.5,this.radius=3,this.heightFillColor=H.main,k(this,e)}apply(t,s=1){const r={glowColor:e.toUnitRGB(this.glowColor),glowFalloff:this.glowFalloff,glowWidth:this.glowWidth,innerColor:e.toUnitRGB(this.innerColor),innerWidth:this.innerWidth,globalAlpha:this.globalAlpha*s,globalAlphaContrastBoost:this.globalAlphaContrastBoost,intersectsLineRadius:this.radius};"style"in t?t.style=r:t.laserlineStyle=r}}class ${constructor(e){this.outline=new X({color:H.outline,renderOccluded:8,stippleOffColor:H.selected,stipplePattern:i(5),width:1.5,innerWidth:0}),this.staged=new X({color:H.selected,renderOccluded:8,innerColor:H.staged,stippleOffColor:H.outline,stipplePattern:i(5),width:1.5}),this.shadowStyle=new Z({globalAlpha:.3,glowColor:H.main,glowFalloff:8,glowWidth:8,innerColor:H.selected,innerWidth:1}),k(this,e)}}class ee{constructor(e){this.outline=new Y({color:H.selected,outlineColor:H.outline,outlineSize:1,shape:"circle",size:4}),this.shadowStyle=new Z({globalAlpha:.3,glowColor:H.main,glowFalloff:1.5,glowWidth:6,innerColor:H.selected,innerWidth:1,radius:2}),k(this,e)}}class te extends X{constructor(e){super(),this.extensionType=2,k(this,e)}}class se{constructor(e){this.lineGraphics=new $,this.pointGraphics=new ee,this.heightPlane=new Z({globalAlpha:.3,glowColor:H.main,glowFalloff:8,glowWidth:8,innerColor:H.selected,innerWidth:1}),this.heightBox=new Z({globalAlpha:.3,glowColor:H.main,glowFalloff:8,glowWidth:8,innerColor:H.selected,innerWidth:0,heightFillColor:H.main}),this.zVerticalLine=new te({color:L(H.main,.5),falloff:2,innerColor:L(H.selected,0),renderOccluded:4,stipplePattern:null,width:5,extensionType:2}),this.laserlineAlphaMultiplier=1.5,this.heightPlaneAngleCutoff=20,k(this,e)}}function re(t){return r(e.toUnitRGBA(t))}const ie=new class{constructor(e){this.visualElements=new se,this.reshapeManipulators=new Q,this.zManipulator=new J,k(this,e)}colorToVec4(e){return re(e)}};class oe{constructor(e){this.resourceFactory=e,this._resources=null,this._visible=!0,this._attached=!1}destroy(){this._destroyResources()}get resources(){return l(this._resources)?this._resources.external:null}get visible(){return this._visible}set visible(e){e!==this._visible&&(this._visible=e,this._syncGeometriesToRenderer())}get attached(){return this._attached}set attached(e){e!==this._attached&&(this._attached=e,this._createOrDestroyResources())}recreate(){this.attached&&this._createResources()}recreateGeometry(){this.resourceFactory.recreateGeometry?h(this._resources)||(this.ensureRenderGeometriesRemoved(),this.resourceFactory.recreateGeometry(this._resources.external),this._syncGeometriesToRenderer()):this.recreate()}_createOrDestroyResources(){this._attached?h(this._resources)&&this._createResources():this._destroyResources()}_createResources(){var e;this._destroyResources();const t=this.resourceFactory.createResources();this._resources={layerView:new ae({view:this.resourceFactory.view}),external:t,geometriesAdded:!1},this._syncGeometriesToRenderer();const s=null==(e=this.resourceFactory.view.basemapTerrain)?void 0:e.overlayManager;s&&s.registerDrapeSource(this._resources.layerView)}_destroyResources(){var e;if(h(this._resources))return;this.ensureRenderGeometriesRemoved();const t=null==(e=this.resourceFactory.view.basemapTerrain)?void 0:e.overlayManager;t&&t.unregisterDrapeSource(this._resources.layerView),this._resources=null}_syncGeometriesToRenderer(){this._visible?this.ensureRenderGeometriesAdded():this.ensureRenderGeometriesRemoved()}ensureRenderGeometriesRemoved(){var e;if(h(this._resources)||null==(e=this.resourceFactory.view)||!e.basemapTerrain)return;if(!this._resources.geometriesAdded)return;this.resourceFactory.view.basemapTerrain.overlayManager.renderer.removeGeometries(this._resources.external.geometries,this._resources.layerView,2),this._resources.geometriesAdded=!1}ensureRenderGeometriesAdded(){if(h(this._resources))return;if(this._resources.geometriesAdded)return;this.resourceFactory.view.basemapTerrain.overlayManager.renderer.addGeometries(this._resources.external.geometries,this._resources.layerView,2),this._resources.geometriesAdded=!0}}let ae=class extends(D(j)){constructor(e){super(e),this.drapeSourceType=1,this.updatePolicy=1}};A([F({constructOnly:!0})],ae.prototype,"view",void 0),A([F({readOnly:!0})],ae.prototype,"drapeSourceType",void 0),ae=A([S("DrapedVisualElementLayerView")],ae);class ne{constructor(e){this.view=null,this._attachmentOrigin=O(0,0,0,null),this._attachmentOriginDirty=!0,this.events=new n,this._isDraped=!1,this._geometry=null,this._width=1,this._color=y(1,0,1,1),this._innerWidth=0,this._innerColor=y(1,1,1,1),this._stipplePattern=null,this._stippleOffColor=null,this._falloff=0,this._elevationInfo=null,this._laserlineStyle=null,this._laserlineEnabled=!1,this._renderOccluded=8,this.resources=new U({view:e.view,createResources:e=>this.createResources(e),recreateGeometry:(e,t)=>(e.geometries.length=0,this.recreateGeometry(t,e.material,e.geometries),e.geometries)}),this._attachmentOrigin.spatialReference=e.view.spatialReference,this.drapedResources=new oe({view:e.view,createResources:()=>this._createDrapedResources(),recreateGeometry:e=>{e.geometries=this._createRenderGeometriesDraped(e.material),this.attachmentOriginChanged()}});let t=!0;this._laserline=new W({view:e.view});for(const s in e)s in this?"attached"===s?t=e[s]:this[s]=e[s]:console.error("Cannot set unknown property",s);this.attached=t}destroy(){this.resources.destroy(),this.drapedResources.destroy(),this._laserline.destroy()}get isDraped(){return this._isDraped}set isDraped(e){e!==this._isDraped&&(this._isDraped=e,this.updateAttached(this.attached),this._laserline.attached=this.laserlineAttached)}get laserlineAttached(){return this.attached&&this.visible&&l(this._laserlineStyle)&&!this.isDraped&&this.laserlineEnabled}get visible(){return this.resources.visible}set visible(e){this.resources.visible=e,this.drapedResources.visible=e,this._laserline.attached=this.laserlineAttached}get attached(){return this.resources.attached||this.drapedResources.attached}set attached(e){this.updateAttached(e)}updateAttached(e){this.resources.attached=!this.isDraped&&e,this.drapedResources.attached=this.isDraped&&e,this._laserline.attached=this.laserlineAttached,this.attached&&this.attachmentOriginChanged()}get geometry(){return this._geometry}set geometry(e){this._geometry=e,this.resources.recreateGeometry(),this.drapedResources.recreateGeometry()}get width(){return this._width}set width(e){e!==this._width&&(this._width=e,this.updateMaterial())}get color(){return this._color}set color(e){d(e,this._color)||(u(this._color,e),this.updateMaterial())}get innerWidth(){return this._innerWidth}set innerWidth(e){e!==this._innerWidth&&(this._innerWidth=e,this.updateMaterial())}get innerColor(){return this._innerColor}set innerColor(e){d(e,this._innerColor)||(u(this._innerColor,e),this.updateMaterial())}get stipplePattern(){return this._stipplePattern}set stipplePattern(e){const t=l(e)!==l(this._stipplePattern);this._stipplePattern=e,t?this.resources.recreate():this.updateMaterial()}get stippleOffColor(){return this._stippleOffColor}set stippleOffColor(e){e&&this._stippleOffColor&&d(e,this._stippleOffColor)||(this._stippleOffColor=e?w(e):null,this.updateMaterial())}get falloff(){return this._falloff}set falloff(e){e!==this._falloff&&(this._falloff=e,this.updateMaterial())}get elevationInfo(){return this._elevationInfo}set elevationInfo(e){this._elevationInfo=e,this.resources.recreateGeometry()}get laserlineStyle(){return this._laserlineStyle}set laserlineStyle(e){this._laserlineStyle=e,this._laserline.attached=this.laserlineAttached,l(e)&&(this._laserline.style=e)}get laserlineEnabled(){return this._laserlineEnabled}set laserlineEnabled(e){this._laserlineEnabled!==e&&(this._laserlineEnabled=e,this._laserline.attached=this.laserlineAttached)}get renderOccluded(){return this._renderOccluded}set renderOccluded(e){e!==this._renderOccluded&&(this._renderOccluded=e,this.updateMaterial())}get attachmentOrigin(){if(!this._attachmentOriginDirty)return this._attachmentOrigin;const e=l(this.resources.resources)?this.resources.resources.geometries:null;if(!e||0===e.length)return null;p(ce,0,0,0);let t=0;for(const s of e){const e=s.vertexAttributes.get("position"),r=s.indices.get("position"),i=c(this.resources.resources).material.isClosed(e.data,r);B(e,r,i,he)&&(m(ce,ce,he),t++)}return 0===t?null:(_(ce,ce,1/t),this.view.renderCoordsHelper.fromRenderCoords(ce,this._attachmentOrigin),this._attachmentOriginDirty=!1,this._attachmentOrigin)}updateMaterial(){l(this.resources.resources)&&this.resources.resources.material.setParameters(this.materialParameters),l(this.drapedResources.resources)&&this.drapedResources.resources.material.setParameters(this.materialParameters)}get isClosed(){return l(this.geometry)&&"polygon"===this.geometry.type}get materialParameters(){return{width:this._width,color:this._color,stippleOffColor:this._stippleOffColor,stipplePattern:this._stipplePattern,stipplePreferContinuous:!1,isClosed:this.isClosed,falloff:this._falloff,innerColor:this._innerColor,innerWidth:this._innerWidth,join:"round",polygonOffset:!0,renderOccluded:this.normalizedRenderOccluded}}get normalizedRenderOccluded(){return this.isDraped&&8===this._renderOccluded?4:this._renderOccluded}recreateGeometry(e,t,s){const r=this.createRenderGeometries();for(const i of r)e.addGeometry(i,t),s.push(i);this.attachmentOriginChanged()}attachmentOriginChanged(){this._attachmentOriginDirty=!0,this.events.emit("attachment-origin-changed")}createResources(e){const t=new z(this.materialParameters),s=[];return this.recreateGeometry(e,t,s),{material:t,geometries:s,forEach:e=>{e(t),s.forEach(e)}}}_createDrapedResources(){const e=new z(this.materialParameters);return{material:e,geometries:this._createRenderGeometriesDraped(e)}}_createRenderGeometriesDraped(e){const t=this.geometry;if(h(t))return[];const s=T(t,this.view.basemapTerrain.spatialReference),r=[];for(const{position:t}of s.lines){const s={overlayInfo:{spatialReference:this.view.basemapTerrain.spatialReference,renderCoordsHelper:this.view.renderCoordsHelper},attributeData:{position:t},removeDuplicateStartEnd:this.isClosed?1:0},i=new o(V(s),e),a=b(le);R(a,t),f(i.boundingSphere,.5*(a[0]+a[3]),.5*(a[1]+a[4]),0,.5*Math.sqrt((a[3]-a[0])*(a[3]-a[0])+(a[4]-a[1])*(a[4]-a[1]))),r.push(i)}return r}calculateMapBounds(e){if(h(this.resources.resources))return!1;const t=this.view.renderCoordsHelper;for(const s of this.resources.resources.geometries){const r=s.vertexAttributes.get("position"),i=new Float64Array(r.data.length);v(r.data,t.spatialReference,0,i,this.view.spatialReference,0,r.data.length/3),R(e,i)}return!0}createRenderGeometries(){var e;const t=this.geometry;if(h(t))return[];const s=I(t,this.view.elevationProvider,this.view.renderCoordsHelper,a.fromElevationInfo(null!=(e=this.elevationInfo)?e:new P({mode:G(t,null)}))),r=[],i=[];for(const{position:e,mapPosition:t}of s.lines){const s={overlayInfo:null,attributeData:{position:e,mapPosition:t},removeDuplicateStartEnd:this.isClosed?1:0};r.push(V(s)),i.push(e)}return this._laserline.pathVerticalPlane=i,r}}const le=C(),he=g(),ce=g();export{ne as O,U as V,H as c,ie as s};
