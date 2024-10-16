/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as t}from"../../chunks/tslib.es6.js";import{A as e}from"../../chunks/Analysis.js";import s from"../../core/Collection.js";import{c as i,r}from"../../chunks/collectionUtils.js";import{b as a,i as o,clone as n,k as l,r as p}from"../../core/lang.js";import{L as u}from"../../chunks/Logger.js";import{property as h}from"../../core/accessorSupport/decorators/property.js";import"../../chunks/ensureType.js";import{subclass as m}from"../../core/accessorSupport/decorators/subclass.js";import{R as c,r as d}from"../../chunks/RenderCoordsHelper.js";import{s as y,c as j,a as k,b as f,d as v,e as g,P as _,p as b,f as w,g as P,h as M,r as S,i as V,j as D,k as T,D as x,l as E,m as I,u as U,n as C,o as L,q as R,t as O,v as F,w as H,x as G,I as z}from"../../chunks/sliceToolUtils.js";import{i as A}from"../../chunks/spatialReferenceSupport.js";import B from"./SlicePlane.js";import q from"../../core/Handles.js";import{c as K,ignoreAbortErrors as Q}from"../../core/promiseUtils.js";import{whenTrueOnce as N}from"../../core/watchUtils.js";import{aliasOf as J}from"../../core/accessorSupport/decorators/aliasOf.js";import{addFrameTask as W}from"../../core/scheduling.js";import{c as $,s as Z,b as X}from"../../chunks/screenUtils.js";import{i as Y,r as tt,v as et,m as st}from"../../chunks/mat4.js";import{d as it,l as rt,m as at,f as ot,g as nt,j as lt,q as pt,n as ut,e as ht,b as mt}from"../../chunks/mathUtils.js";import{c as ct,a as dt,n as yt,f as jt}from"../../chunks/boundedPlane.js";import{s as kt,c as ft,h as vt,n as gt}from"../../chunks/plane.js";import{c as _t}from"../../chunks/ray.js";import{b as bt,s as wt,c as Pt}from"../../chunks/vectorStacks.js";import{b as Mt}from"../../chunks/manipulatorUtils.js";import{g as St,a as Vt}from"../../chunks/Factory.js";import{a as Dt}from"../../chunks/dragEventPipeline3D.js";import{a as Tt}from"../../chunks/ray2.js";import{n as xt}from"../../chunks/Intersector.js";import{I as Et,c as It}from"../../chunks/InteractiveToolBase.js";import{c as Ut}from"../../chunks/screenUtils2.js";import{I as Ct}from"../../chunks/InteractiveToolViewModel.js";import"../../core/Accessor.js";import"../../chunks/deprecate.js";import"../../chunks/metadata.js";import"../../chunks/handleUtils.js";import"../../chunks/ArrayPool.js";import"../../config.js";import"../../chunks/object.js";import"../../chunks/string.js";import"../../core/Error.js";import"../../chunks/nextTick.js";import"../../chunks/Identifiable.js";import"../../chunks/Evented.js";import"../../chunks/shared.js";import"../../chunks/unitUtils.js";import"../../chunks/jsonMap.js";import"../../chunks/projectionEllipsoid.js";import"../../geometry/SpatialReference.js";import"../../chunks/JSONSupport.js";import"../../chunks/writer.js";import"../../chunks/Ellipsoid.js";import"../../geometry/projection.js";import"../../geometry/Extent.js";import"../../geometry/Geometry.js";import"../../chunks/reader.js";import"../../geometry/Point.js";import"../../core/accessorSupport/decorators/cast.js";import"../../geometry/support/webMercatorUtils.js";import"../../geometry/Multipoint.js";import"../../chunks/zmUtils.js";import"../../chunks/pe.js";import"../../chunks/assets.js";import"../../request.js";import"../../kernel.js";import"../../core/urlUtils.js";import"../../geometry/Polygon.js";import"../../chunks/extentUtils.js";import"../../geometry/Polyline.js";import"../../chunks/aaBoundingRect.js";import"../../chunks/geodesicConstants.js";import"../../geometry/support/GeographicTransformation.js";import"../../geometry/support/GeographicTransformationStep.js";import"../../chunks/zscale.js";import"../../chunks/common.js";import"../../chunks/sphere.js";import"../../chunks/vec4f64.js";import"../../chunks/ElevationProvider.js";import"../../geometry.js";import"../../chunks/typeUtils.js";import"../../geometry/support/jsonUtils.js";import"../../chunks/compilerUtils.js";import"../../chunks/mat4f64.js";import"../../chunks/quat.js";import"../../chunks/LineVisualElement.js";import"../../chunks/vec4f32.js";import"../../chunks/VisualElement.js";import"../../chunks/ScreenSpacePass.js";import"../../chunks/vec2f64.js";import"../../chunks/aaBoundingBox.js";import"../../chunks/dehydratedFeatures.js";import"../../chunks/byteSizeEstimations.js";import"../../chunks/quantizationUtils.js";import"../../layers/support/Field.js";import"../../chunks/enumeration.js";import"../../chunks/domains.js";import"../../layers/support/CodedValueDomain.js";import"../../layers/support/Domain.js";import"../../layers/support/InheritedDomain.js";import"../../layers/support/RangeDomain.js";import"../../chunks/fieldType.js";import"../../Graphic.js";import"../../PopupTemplate.js";import"../../layers/support/fieldUtils.js";import"../../chunks/arcadeOnDemand.js";import"../../popup/content.js";import"../../popup/content/AttachmentsContent.js";import"../../popup/content/Content.js";import"../../popup/content/CustomContent.js";import"../../popup/content/ExpressionContent.js";import"../../popup/ElementExpressionInfo.js";import"../../popup/content/FieldsContent.js";import"../../popup/FieldInfo.js";import"../../popup/support/FieldInfoFormat.js";import"../../chunks/date.js";import"../../chunks/number.js";import"../../chunks/locale.js";import"../../popup/content/MediaContent.js";import"../../popup/content/BarChartMediaInfo.js";import"../../chunks/chartMediaInfoUtils.js";import"../../chunks/MediaInfo.js";import"../../popup/content/support/ChartMediaInfoValue.js";import"../../popup/content/support/ChartMediaInfoValueSeries.js";import"../../popup/content/ColumnChartMediaInfo.js";import"../../popup/content/ImageMediaInfo.js";import"../../popup/content/support/ImageMediaInfoValue.js";import"../../popup/content/LineChartMediaInfo.js";import"../../popup/content/PieChartMediaInfo.js";import"../../popup/content/TextContent.js";import"../../popup/ExpressionInfo.js";import"../../popup/LayerOptions.js";import"../../popup/RelatedRecordsInfo.js";import"../../popup/support/RelatedRecordsInfoFieldOrder.js";import"../../support/actions/ActionBase.js";import"../../support/actions/ActionButton.js";import"../../support/actions/ActionToggle.js";import"../../symbols.js";import"../../symbols/CIMSymbol.js";import"../../symbols/Symbol.js";import"../../Color.js";import"../../chunks/colorUtils.js";import"../../symbols/ExtrudeSymbol3DLayer.js";import"../../symbols/Symbol3DLayer.js";import"../../chunks/utils.js";import"../../symbols/edges/Edges3D.js";import"../../chunks/materialUtils.js";import"../../chunks/opacityUtils.js";import"../../symbols/edges/SketchEdges3D.js";import"../../symbols/edges/SolidEdges3D.js";import"../../chunks/Symbol3DMaterial.js";import"../../symbols/FillSymbol.js";import"../../symbols/SimpleLineSymbol.js";import"../../symbols/LineSymbol.js";import"../../symbols/LineSymbolMarker.js";import"../../symbols/FillSymbol3DLayer.js";import"../../symbols/patterns/LineStylePattern3D.js";import"../../symbols/patterns/StylePattern3D.js";import"../../chunks/utils2.js";import"../../chunks/colors.js";import"../../chunks/symbolLayerUtils3D.js";import"../../symbols/Font.js";import"../../symbols/IconSymbol3DLayer.js";import"../../chunks/persistableUrlUtils.js";import"../../symbols/LabelSymbol3D.js";import"../../symbols/Symbol3D.js";import"../../portal/Portal.js";import"../../chunks/Loadable.js";import"../../chunks/Promise.js";import"../../portal/PortalQueryParams.js";import"../../portal/PortalQueryResult.js";import"../../portal/PortalUser.js";import"../../portal/PortalFolder.js";import"../../portal/PortalGroup.js";import"../../symbols/LineSymbol3DLayer.js";import"../../symbols/ObjectSymbol3DLayer.js";import"../../symbols/PathSymbol3DLayer.js";import"../../symbols/TextSymbol3DLayer.js";import"../../symbols/WaterSymbol3DLayer.js";import"../../chunks/Thumbnail.js";import"../../chunks/Symbol3DVerticalOffset.js";import"../../symbols/callouts/Callout3D.js";import"../../symbols/callouts/LineCallout3D.js";import"../../symbols/LineSymbol3D.js";import"../../symbols/MarkerSymbol.js";import"../../symbols/MeshSymbol3D.js";import"../../symbols/PictureFillSymbol.js";import"../../chunks/urlUtils.js";import"../../symbols/PictureMarkerSymbol.js";import"../../symbols/PointSymbol3D.js";import"../../symbols/PolygonSymbol3D.js";import"../../symbols/SimpleFillSymbol.js";import"../../symbols/SimpleMarkerSymbol.js";import"../../symbols/TextSymbol.js";import"../../symbols/WebStyleSymbol.js";import"../../chunks/mathUtils2.js";import"../../chunks/StencilUtils.js";import"../../chunks/Texture.js";import"../../chunks/Program.js";import"../../chunks/parser.js";import"../../chunks/_commonjsHelpers.js";import"../../chunks/Util2.js";import"../../chunks/vec2.js";import"../../chunks/geometryDataUtils.js";import"../../chunks/triangle.js";import"../../chunks/lineSegment.js";import"../../chunks/utils14.js";import"../../chunks/mat3.js";import"../../chunks/quatf64.js";import"../../chunks/vec3f32.js";import"../../chunks/doublePrecisionUtils.js";import"../../chunks/BufferView.js";import"../../chunks/frustum.js";import"../../chunks/InterleavedLayout.js";import"../../chunks/types.js";import"../../chunks/lineUtils.js";import"../../chunks/triangulationUtils.js";import"../../chunks/earcut.js";import"../../chunks/deduplicate.js";import"../../chunks/vec2f32.js";import"../../chunks/FramebufferObject.js";import"../../chunks/Camera.js";import"../../chunks/PhysicallyBasedRendering.glsl.js";import"../../chunks/OrderIndependentTransparency.js";import"../../chunks/glUtil.js";import"../../chunks/MemCache.js";import"../../chunks/floatRGBA.js";import"../../chunks/Scheduler.js";import"../../chunks/reactiveUtils.js";import"../../chunks/debugFlags.js";import"../../chunks/ColorMaterial.js";import"../../chunks/VertexColor.glsl.js";import"../../chunks/ImageMaterial.js";import"../../chunks/Texture2.js";import"../../chunks/requestImageUtils.js";import"../../chunks/NativeLineMaterial.js";import"../../chunks/persistable.js";import"../../chunks/multiOriginJSONSupportUtils.js";import"../../chunks/uuid.js";import"../../chunks/elevationInfoUtils.js";import"../../chunks/unitConversionUtils.js";import"../../chunks/lengthUtils.js";import"../../chunks/drawUtils.js";import"../../chunks/DOMContainer.js";import"../../chunks/domUtils.js";import"../../chunks/projector.js";import"../../chunks/widgetUtils.js";import"../../core/HandleOwner.js";import"../Popup.js";import"../../intl.js";import"../../chunks/messages.js";import"../../chunks/throttle.js";import"../Feature.js";import"../Widget.js";import"../../chunks/jsxWidgetSupport.js";import"../Attachments.js";import"../Attachments/AttachmentsViewModel.js";import"../../rest/query/support/AttachmentInfo.js";import"../../rest/support/AttachmentQuery.js";import"../../chunks/messageBundle.js";import"../../chunks/jsxFactory.js";import"../Feature/FeatureViewModel.js";import"../../chunks/languageUtils.js";import"../../chunks/datetime.js";import"../../chunks/number3.js";import"../../rest/support/Query.js";import"../../TimeExtent.js";import"../../chunks/timeUtils.js";import"../../chunks/DataLayerSource.js";import"../../rest/support/StatisticDefinition.js";import"../../rest/support/RelationshipQuery.js";import"../../tasks/QueryTask.js";import"../../chunks/executeForTopCount.js";import"../../chunks/utils3.js";import"../../chunks/scaleUtils.js";import"../../chunks/floorFilterUtils.js";import"../../chunks/query.js";import"../../geometry/support/normalizeUtils.js";import"../../chunks/normalizeUtilsCommon.js";import"../../chunks/arcgisLayerUrl.js";import"../../chunks/pbfQueryUtils.js";import"../../chunks/pbf.js";import"../../chunks/OptimizedFeature.js";import"../../chunks/OptimizedFeatureSet.js";import"../../chunks/queryZScale.js";import"../../chunks/featureConversionUtils.js";import"../../rest/support/FeatureSet.js";import"../../rest/support/TopFeaturesQuery.js";import"../../rest/support/TopFilter.js";import"../../chunks/executeQueryJSON.js";import"../../tasks/Task.js";import"../../layers/FeatureLayer.js";import"../../renderers/ClassBreaksRenderer.js";import"../../renderers/Renderer.js";import"../../renderers/support/AuthoringInfo.js";import"../../renderers/support/AuthoringInfoVisualVariable.js";import"../../chunks/colorRamps.js";import"../../rest/support/AlgorithmicColorRamp.js";import"../../rest/support/ColorRamp.js";import"../../rest/support/MultipartColorRamp.js";import"../../chunks/VisualVariablesMixin.js";import"../../renderers/visualVariables/ColorVariable.js";import"../../renderers/visualVariables/VisualVariable.js";import"../../chunks/LegendOptions.js";import"../../renderers/visualVariables/support/ColorStop.js";import"../../renderers/visualVariables/OpacityVariable.js";import"../../renderers/visualVariables/support/OpacityStop.js";import"../../renderers/visualVariables/RotationVariable.js";import"../../renderers/visualVariables/SizeVariable.js";import"../../renderers/visualVariables/support/SizeStop.js";import"../../chunks/sizeVariableUtils.js";import"../../chunks/visualVariableUtils.js";import"../../renderers/support/ClassBreakInfo.js";import"../../chunks/commonProperties2.js";import"../../symbols/support/jsonUtils.js";import"../../chunks/symbolConversion.js";import"../../renderers/DictionaryRenderer.js";import"../../chunks/LRUCache.js";import"../../renderers/DotDensityRenderer.js";import"../../renderers/support/AttributeColorInfo.js";import"../../renderers/HeatmapRenderer.js";import"../../renderers/support/HeatmapColorStop.js";import"../../renderers/SimpleRenderer.js";import"../../renderers/UniqueValueRenderer.js";import"../../chunks/diffUtils.js";import"../../renderers/support/UniqueValueInfo.js";import"../../chunks/styleUtils.js";import"../../renderers/support/jsonUtils.js";import"../../chunks/MultiOriginJSONSupport.js";import"../../core/sql.js";import"../../form/FormTemplate.js";import"../../form/ExpressionInfo.js";import"../../form/elements/GroupElement.js";import"../../form/elements/FieldElement.js";import"../../form/elements/support/inputs.js";import"../../form/elements/inputs/BarcodeScannerInput.js";import"../../form/elements/inputs/TextInput.js";import"../../form/elements/inputs/Input.js";import"../../form/elements/inputs/ComboBoxInput.js";import"../../form/elements/inputs/DateTimePickerInput.js";import"../../form/elements/inputs/RadioButtonsInput.js";import"../../form/elements/inputs/SwitchInput.js";import"../../form/elements/inputs/TextAreaInput.js";import"../../form/elements/inputs/TextBoxInput.js";import"../../form/support/elements.js";import"../../geometry/HeightModelInfo.js";import"../../layers/Layer.js";import"../../chunks/FeatureIndex.js";import"../../core/workers/workers.js";import"../../core/workers/Connection.js";import"../../core/workers/RemoteClient.js";import"../../chunks/APIKeyMixin.js";import"../../chunks/ArcGISService.js";import"../../chunks/BlendLayer.js";import"../../chunks/jsonUtils.js";import"../../chunks/CustomParametersMixin.js";import"../../chunks/FeatureEffectLayer.js";import"../../layers/support/FeatureEffect.js";import"../../layers/support/FeatureFilter.js";import"../../chunks/OperationalLayer.js";import"../../chunks/commonProperties.js";import"../../support/timeUtils.js";import"../../chunks/ElevationInfo.js";import"../../chunks/OrderedLayer.js";import"../../chunks/PortalLayer.js";import"../../chunks/asyncUtils.js";import"../../portal/PortalItem.js";import"../../portal/PortalItemResource.js";import"../../portal/PortalRating.js";import"../../chunks/RefreshableLayer.js";import"../../chunks/ScaleRangeLayer.js";import"../../chunks/TemporalLayer.js";import"../../TimeInterval.js";import"../../layers/support/TimeInfo.js";import"../../chunks/featureReductionUtils.js";import"../../layers/support/FeatureReductionSelection.js";import"../../layers/support/FeatureReductionCluster.js";import"../../layers/support/LabelClass.js";import"../../chunks/labelUtils.js";import"../../chunks/defaultsJSON.js";import"../../layers/support/FeatureTemplate.js";import"../../layers/support/FeatureType.js";import"../../chunks/fieldProperties.js";import"../../layers/support/FieldsIndex.js";import"../../layers/support/GeometryFieldsInfo.js";import"../../chunks/labelingInfo.js";import"../../layers/support/LayerFloorInfo.js";import"../../layers/support/Relationship.js";import"../../chunks/styleUtils2.js";import"../../support/popupUtils.js";import"../../chunks/Heading.js";import"../support/widget.js";import"../../chunks/accessibleHandler.js";import"../../chunks/vmEvent.js";import"../Spinner/SpinnerViewModel.js";import"../../chunks/AnchorElementViewModel.js";import"../Popup/PopupViewModel.js";import"../../symbols/support/symbolUtils.js";import"../../chunks/utils4.js";import"../../chunks/ItemCache.js";import"../../symbols/support/cimSymbolUtils.js";import"../../chunks/utils5.js";import"../../chunks/InputManager.js";import"../../chunks/Queue.js";import"../../chunks/layerViewUtils.js";import"../../chunks/actions.js";import"../../chunks/GoTo.js";import"../../chunks/interactiveToolUtils.js";var Lt;const Rt=u.getLogger("esri.analysis.Slice");let Ot=Lt=class extends e{constructor(t){super(t),this.type="slice",this.tiltEnabled=!1,this.shape=null,this.excludeGroundSurface=!1}get extent(){if(a(this.shape)||a(this.shape.position))return null;const t=this.shape.position.spatialReference;let e=null,s=null;if(A(t,2)){const s=c.create(2,d(!1,t));e=y(this.shape,s,{tiltEnabled:this.tiltEnabled})}if(A(t,1)){const e=c.create(1,d(!0,t));s=y(this.shape,e,{tiltEnabled:this.tiltEnabled})}return o(e)&&o(s)?e.union(s):a(e)&&a(s)?(Rt.warnOnce(`Extent of slice analysis (title: '${null!=(i=this.title)?i:"no title"}', id: '${null!=(r=this.id)?r:"no id"}') could not be computed as the spatial reference of the shape (wkid: '${null!=(n=t.wkid)?n:"no wkid"}') is not supported by the view`),null):a(e)?s:e;var i,r,n}clone(){return new Lt({tiltEnabled:this.tiltEnabled,shape:n(this.shape),excludedLayers:this.excludedLayers.slice(),excludeGroundSurface:this.excludeGroundSurface,...this.cloneBaseAnalysisProperties()})}get excludedLayers(){return this._get("excludedLayers")||new s}set excludedLayers(t){this._set("excludedLayers",r(t,this._get("excludedLayers")))}};t([h({readOnly:!0})],Ot.prototype,"type",void 0),t([h({readOnly:!0})],Ot.prototype,"extent",null),t([h()],Ot.prototype,"tiltEnabled",void 0),t([h({types:{key:"type",base:null,typeMap:{plane:B},defaultKeyValue:"plane"}})],Ot.prototype,"shape",void 0),t([h({cast:i})],Ot.prototype,"excludedLayers",null),t([h({type:Boolean,nonNullable:!0})],Ot.prototype,"excludeGroundSurface",void 0),Ot=Lt=t([m("esri.analysis.Slice")],Ot);const Ft=Ot;let Ht=class extends Et{constructor(t){super(t),this.clock=K,this._previewPlaneOpacity=1,this.analysisView=null,this.layersMode="none",this.tiltEnabled=!1,this.shiftManipulator=null,this.rotateHeadingManipulator=null,this.rotateTiltManipulator=null,this.resizeManipulators=null,this.disableEngineLayers=!1,this._handles=new q,this._viewHandles=new q,this._frameTask=null,this._prevPointerMoveTimeout=null,this._previewPlaneGridVisualElement=null,this._previewPlaneOutlineVisualElement=null,this._startPlane=ct(),this._previewPlane=null,this._activeKeyModifiers={},this._lastCursorPosition=$(),this._resizeHandles=[{direction:[1,0]},{direction:[1,1]},{direction:[0,1]},{direction:[-1,1]},{direction:[-1,0]},{direction:[-1,-1]},{direction:[0,-1]},{direction:[1,-1]}],this._intersector=xt(t.view.state.viewingMode),this._intersector.options.store=0}onAttach(){this.created&&this.visible&&this._updateManipulators()}initialize(){this._initialize()}async _initialize(){const t=()=>{if("creating"===this.toolState||"created"===this.toolState)throw new Error("Unexpected toolState");return!this.destroyed};if(!t())return;if(this.analysisView=await this.view.whenAnalysisView(this.analysis),!t())return;if(null==this.analysis)throw new Error("SliceTool requires valid model, but null was provided.");if(a(this.analysisView.analysisViewData))throw new Error("expected internal object to be defined.");this.analysisViewData=this.analysisView.analysisViewData,this.rotateHeadingImage=St(this.view.toolViewManager.textures),this.rotateTiltImage=Vt(this.view.toolViewManager.textures);const e=this.watch("state",(t=>{switch(t){case"ready":case"slicing":this.finishToolCreation();break;case"sliced":this.complete()}}),!0);"sliced"===this.state?this.complete():this.startToolCreation(),this._handles.add([e,this.view.state.watch("camera",(()=>this._onCameraChange()))]);const s=t=>{this.updateManipulatorsInteractive(t),t.grabbing||(o(this.analysisViewData.plane)&&dt(this.analysisViewData.plane,this._startPlane),this.inputState=null)};this.shiftManipulator=j(this.view),this.manipulators.add(this.shiftManipulator),this.shiftManipulator.events.on("grab-changed",(t=>{this._onShiftGrab(t),s(this.shiftManipulator)})),this._handles.add(this._createShiftDragPipeline(this.shiftManipulator)),this.rotateHeadingManipulator=k(this.view,this.rotateHeadingImage.texture),this.manipulators.add(this.rotateHeadingManipulator),this.rotateHeadingManipulator.events.on("grab-changed",(t=>{this._onRotateHeadingGrab(t),s(this.rotateHeadingManipulator)})),this._handles.add(this._createRotateHeadingDragPipeline(this.rotateHeadingManipulator)),this.rotateTiltManipulator=k(this.view,this.rotateTiltImage.texture),this.manipulators.add(this.rotateTiltManipulator),this.rotateTiltManipulator.events.on("grab-changed",(t=>{this._onRotateTiltGrab(t),s(this.rotateTiltManipulator)})),this._handles.add(this._createRotateTiltDragPipeline(this.rotateTiltManipulator)),this.resizeManipulators=this._resizeHandles.map(((t,e)=>{const i=f(this.view,t);return i.events.on("grab-changed",(t=>{this._onResizeGrab(t,e),s(i)})),this._handles.add(this._createResizeDragPipeline(i)),i})),this.manipulators.addMany(this.resizeManipulators),this._previewPlaneGridVisualElement=v(this.view),this._previewPlaneOutlineVisualElement=g(this.view),this._previewPlaneOutlineVisualElement.width=_,this._handles.add(this.analysisViewData.watch("plane",(()=>this._updateManipulators()),!0)),this._updateManipulators(),this._updateVisibility(this.visible)}destroy(){if("pending"===this.toolState)return this._handles=l(this._handles),void(this._viewHandles=l(this._viewHandles));this.rotateHeadingImage.release(),this.rotateTiltImage.release(),this.attached&&this.detach(),this._handles=l(this._handles),this._viewHandles=l(this._viewHandles),this._removeFrameTask(),this._clearPointerMoveTimeout(),this._previewPlaneOutlineVisualElement=l(this._previewPlaneOutlineVisualElement),this._previewPlaneGridVisualElement=l(this._previewPlaneGridVisualElement)}get state(){const t=!!this.analysisViewData.plane,e=!!this.inputState;return t?t&&e?"slicing":t&&!e?"sliced":"ready":"ready"}get cursor(){return this._get("cursor")}set analysis(t){if(null==t)throw new Error("SliceTool requires valid model, but null was provided.");this._handles.remove("analysis"),this._set("analysis",t)}get inputState(){return this._get("inputState")}set inputState(t){this._set("inputState",t),this.analysisView.showGrid=o(t)&&"resize"===t.type,this._updateMaterials()}get _isPlacingSlicePlane(){return!this.inputState&&!this.analysisViewData.plane&&this.active}get _creatingPointerId(){return o(this.inputState)&&"shift"===this.inputState.type?this.inputState.creatingPointerId:null}enterExcludeLayerMode(){a(this.analysisViewData.plane)||(this._set("layersMode","exclude"),this.active||(this.view.activeTool=this))}exitExcludeLayerMode(){a(this.analysisViewData.plane)||(this._set("layersMode","none"),this.active&&(this.view.activeTool=null))}onDeactivate(){this._updateMouseCursor(),this._set("layersMode","none"),this._updatePreviewPlane(null)}onDetach(){this.created&&this._set("layersMode","none")}onShow(){this._updateVisibility(!0)}onHide(){this._updateVisibility(!1)}_updateVisibility(t){this.analysis.visible=t,this.created&&(this._updateMouseCursor(),this._updateManipulators(),t||this._clearPointerMoveTimeout())}onInputEvent(t){switch(t.type){case"pointer-drag":if(!zt(t))return;this._isPlacingSlicePlane?this._onClickPlacePlane(t)&&(t.stopPropagation(),this._updateMouseCursor()):this._onPointerDrag(t)&&t.stopPropagation();break;case"pointer-move":this._onPointerMove(t),this._updateMouseCursor();break;case"pointer-up":this._onPointerUp(t)&&t.stopPropagation();break;case"immediate-click":if(!zt(t))return;this._onClickPlacePlane(t)&&(t.stopPropagation(),this._updateMouseCursor());break;case"click":if(!zt(t))return;this._onClickExcludeLayer(t)&&t.stopPropagation();break;case"drag":this.inputState&&t.stopPropagation();break;case"key-down":this._onKeyDown(t)&&t.stopPropagation();break;case"key-up":this._onKeyUp(t)&&t.stopPropagation()}}onEditableChange(){this.analysisView&&(this.analysisView.editable=this.editable)}_onPointerDrag(t){const e=this.inputState;if(t.pointerId===this._creatingPointerId&&o(e)&&"shift"===e.type){const s=Ut(t);return this.shiftManipulator.events.emit("drag",{action:e.hasBeenDragged?"update":"start",pointerType:t.pointerType,start:s,screenPoint:s}),e.hasBeenDragged=!0,!0}return!1}_onPointerMove(t){this._lastCursorPosition.x=t.x,this._lastCursorPosition.y=t.y,this._resetPointerMoveTimeout(),"touch"!==t.pointerType&&this._updatePreviewPlane(Ut(t),this._activeKeyModifiers)}_onCameraChange(){this._updatePreviewPlane(this._lastCursorPosition,this._activeKeyModifiers),this._updateManipulators()}_onPointerUp(t){if(t.pointerId===this._creatingPointerId&&o(this.analysisViewData.plane)){const e=Ut(t);return this.shiftManipulator.events.emit("drag",{action:"end",start:e,screenPoint:e}),dt(this.analysisViewData.plane,this._startPlane),this.inputState=null,!0}return!1}_onClickPlacePlane(t){if("exclude"===this.layersMode)return!1;if(this._isPlacingSlicePlane){const e=Ut(t),s=ct();if(this._pickPlane(e,!1,this._activeKeyModifiers,s)){if(dt(s,this._startPlane),this.analysis.shape=b(s,this.view,this.view.spatialReference,new B),"pointer-drag"===t.type){const i=this._calculatePickRay(e);this.inputState=Gt(i,t.pointerId,s.origin,s)}return!0}}return!1}_onClickExcludeLayer(t){return!("exclude"!==this.layersMode||!this.created)&&(this.view.hitTest(Ut(t)).then((t=>{if(t.results.length){const e=t.results[0],s=e&&e.graphic;if(s){const t=s.sourceLayer||s.layer;t&&this.analysis.excludedLayers.push(t)}}else t.ground.layer?this.analysis.excludedLayers.push(t.ground.layer):this.analysis.excludeGroundSurface=!0})),this._set("layersMode","none"),this.active&&(this.view.activeTool=null),!0)}_onKeyDown(t){return(t.key===w||t.key===P)&&(this._activeKeyModifiers[t.key]=!0,o(this._previewPlane)&&this._updatePreviewPlane(this._lastCursorPosition,this._activeKeyModifiers),!0)}_onKeyUp(t){return!(t.key!==w&&t.key!==P||!this._activeKeyModifiers[t.key])&&(delete this._activeKeyModifiers[t.key],o(this._previewPlane)&&this._updatePreviewPlane(this._lastCursorPosition,this._activeKeyModifiers),!0)}_onShiftGrab(t){if("start"!==t.action||a(this.analysisViewData.plane))return;const e=this._calculatePickRay(t.screenPoint);dt(this.analysisViewData.plane,this._startPlane),this.inputState=Gt(e,null,this.shiftManipulator.renderLocation,this.analysisViewData.plane)}_createShiftDragPipeline(t){return It(t,((t,e,s)=>{const i=this.inputState;if(a(i)||"shift"!==i.type)return;const r=o(this.analysisViewData.plane)?dt(this.analysisViewData.plane,ct()):null;e.next(Dt(this.view,i.shiftPlane)).next(this._shiftDragAdjustSensitivity(i)).next(this._shiftDragUpdatePlane(i)),s.next((()=>{o(r)&&this._updateBoundedPlane(r)}))}))}_shiftDragAdjustSensitivity(t){return e=>{if(a(this.analysisViewData.plane))return null;const s=Math.min((1-Math.abs(it(yt(this.analysisViewData.plane),e.ray.direction)/rt(e.ray.direction)))/.001,1),i=-kt(this._startPlane.plane,e.renderEnd),r=-kt(this._startPlane.plane,t.startPoint);return t.depth=t.depth*(1-s)+i*s-r,e}}_shiftDragUpdatePlane(t){return()=>{if(a(this.analysisViewData.plane))return;const e=at(bt.get(),this._startPlane.origin),s=at(bt.get(),yt(this._startPlane));ot(s,s,-t.depth),nt(s,s,e);const i=jt(s,this.analysisViewData.plane.basis1,this.analysisViewData.plane.basis2,ct());this._updateBoundedPlane(i)}}_onRotateHeadingGrab(t){if("start"!==t.action||a(this.analysisViewData.plane))return;const e=M(this.analysisViewData.plane,this.view.renderCoordsHelper,1,ft()),s=this._calculatePickRay(t.screenPoint),i=mt();vt(e,s,i)&&(dt(this.analysisViewData.plane,this._startPlane),this.inputState={type:"rotate",rotatePlane:e,startPoint:i})}_createRotateHeadingDragPipeline(t){return It(t,((t,e,s)=>{const i=this.inputState;if(a(i)||"rotate"!==i.type)return;const r=o(this.analysisViewData.plane)?dt(this.analysisViewData.plane,ct()):null;e.next(Dt(this.view,i.rotatePlane)).next(this._rotateDragRenderPlaneToRotate(i)).next(this._rotateDragUpdatePlaneFromRotate()),s.next((()=>{o(r)&&this._updateBoundedPlane(r)}))}))}_onRotateTiltGrab(t){if("start"!==t.action||a(this.analysisViewData.plane))return;const e=M(this.analysisViewData.plane,this.view.renderCoordsHelper,2,ft()),s=this._calculatePickRay(t.screenPoint),i=mt();vt(e,s,i)&&(dt(this.analysisViewData.plane,this._startPlane),this.inputState={type:"rotate",rotatePlane:e,startPoint:i})}_createRotateTiltDragPipeline(t){return It(t,((t,e,s)=>{const i=this.inputState;if(a(i)||"rotate"!==i.type)return;const r=o(this.analysisViewData.plane)?dt(this.analysisViewData.plane,ct()):null;e.next(Dt(this.view,i.rotatePlane)).next(this._rotateDragRenderPlaneToRotate(i)).next(this._rotateDragUpdatePlaneFromRotate()),s.next((()=>{o(r)&&this._updateBoundedPlane(r)}))}))}_rotateDragRenderPlaneToRotate(t){return e=>{if(a(this.analysisViewData.plane))return null;const s=gt(t.rotatePlane),i=Mt(t.startPoint,e.renderEnd,this.analysisViewData.plane.origin,s);return{...e,rotateAxis:s,rotateAngle:i}}}_rotateDragUpdatePlaneFromRotate(){return t=>{if(a(this.analysisViewData.plane))return;const e=Y(wt.get());tt(e,e,t.rotateAngle,t.rotateAxis);const s=lt(bt.get(),this._startPlane.basis1,e),i=lt(bt.get(),this._startPlane.basis2,e),r=jt(this.analysisViewData.plane.origin,s,i,ct());this._updateBoundedPlane(r)}}_onResizeGrab(t,e){if("start"!==t.action||a(this.analysisViewData.plane))return;const s=this._calculatePickRay(t.screenPoint),i=bt.get();vt(this.analysisViewData.plane.plane,s,i)&&(dt(this.analysisViewData.plane,this._startPlane),this.inputState={type:"resize",activeHandleIdx:e,startPoint:pt(i)})}_createResizeDragPipeline(t){return It(t,((t,e,s)=>{const i=this.inputState;if(a(i)||"resize"!==i.type||a(this.analysisViewData.plane))return;const r=dt(this.analysisViewData.plane,ct());e.next(Dt(this.view,this.analysisViewData.plane.plane)).next(this._resizeDragUpdatePlane(i)),s.next((()=>{this._updateBoundedPlane(r)}))}))}_resizeDragUpdatePlane(t){return e=>{if(a(this.analysisViewData.plane))return;const s=this._resizeHandles[t.activeHandleIdx],i=S(s,t.startPoint,e.renderEnd,this.view.state.camera,this._startPlane,dt(this.analysisViewData.plane));this._updateBoundedPlane(i)}}_updateBoundedPlane(t){const e=this.analysisViewData;if(!o(e))throw new Error("valid internal object expected");e.plane=t}_updatePreviewPlane(t,e={}){let s=this._previewPlane;if(this._previewPlane=null,a(t))return this._removeFrameTask(),void this._updateManipulators();if(!this.analysisViewData.plane&&this.active){const i=o(s)?s:ct();if(s=o(s)?dt(s,At):null,this._pickPlane(t,!0,e,i)){const t=G;let e=!1;o(s)&&(e=it(s.plane,i.plane)<t||it(ut(bt.get(),s.basis1),ut(bt.get(),i.basis1))<t),e&&(this._previewPlaneOpacity=0),this._previewPlane=i}}o(this._previewPlane)&&a(this._frameTask)&&0===this._previewPlaneOpacity?this._frameTask=W({update:({deltaTime:t})=>{this._previewPlaneOpacity=Math.min(this._previewPlaneOpacity+t/(1e3*V),1),this._updateManipulators(),1===this._previewPlaneOpacity&&this._removeFrameTask()}}):a(this._previewPlane)&&o(this._frameTask)?this._removeFrameTask():o(this._previewPlane)&&this._updateManipulators()}_removeFrameTask(){this._frameTask=p(this._frameTask)}_calculatePickRay(t){const e=_t(),s=Z(t,Bt);return Tt(this.view.state.camera,s,e),ut(e.direction,e.direction),e}_pickMinResult(t){const e=Z(t,Pt.get());return this.view.sceneIntersectionHelper.intersectToolIntersectorScreen(e,this._intersector),this._intersector.results.min}_pickPlane(t,e,s,i){const r=this._pickMinResult(t),a=bt.get();if(!r.getIntersectionPoint(a))return!1;const o=r.getTransformedNormal(bt.get()),n=this.view.state.camera;it(o,n.viewForward)>0&&ot(o,o,-1);const l=D(a,n),p=(e?1:-1)*l*z,u=ot(bt.get(),o,p);nt(u,u,a);const h=this.tiltEnabled?3:0,m=s[w]?2:s[P]?1:h;return T(u,o,l,l,n,m,this.view.renderCoordsHelper,i),!0}_updateMouseCursor(){this._set("cursor",null),this._isPlacingSlicePlane||"exclude"===this.layersMode?this._set("cursor","crosshair"):o(this._creatingPointerId)&&this._set("cursor","grabbing")}_clearPointerMoveTimeout(){this._prevPointerMoveTimeout=p(this._prevPointerMoveTimeout)}_resetPointerMoveTimeout(){this._clearPointerMoveTimeout(),this.shiftManipulator.state|=x,this.rotateHeadingManipulator.state|=x,this.rotateTiltManipulator.state|=x,this._prevPointerMoveTimeout=this.clock.setTimeout((()=>{this.shiftManipulator.state&=~x,this.rotateHeadingManipulator.state&=~x,this.rotateTiltManipulator.state&=~x}),E)}_updateManipulators(){if(this.disableEngineLayers)return;let t=null,e=!1;if(o(this.analysisViewData.plane))t=this.analysisViewData.plane,e=!1;else{if(!o(this._previewPlane))return this.shiftManipulator.available=!1,this.rotateHeadingManipulator.available=!1,this.rotateTiltManipulator.available=!1,this.resizeManipulators.forEach((t=>t.available=!1)),this._previewPlaneOutlineVisualElement.visible=!1,void(this._previewPlaneGridVisualElement.visible=!1);t=this._previewPlane,e=!0}const s=I(t,wt.get());e?(this.shiftManipulator.available=!1,this.rotateHeadingManipulator.available=!1,this.rotateTiltManipulator.available=!1,this.resizeManipulators.forEach((t=>t.available=!1)),this._previewPlaneOutlineVisualElement.visible=!0,this._previewPlaneGridVisualElement.visible=!0):(this.shiftManipulator.available=!0,this.rotateHeadingManipulator.available=!0,this.rotateTiltManipulator.available=this.tiltEnabled,this.resizeManipulators.forEach((t=>t.available=!0)),U(this.shiftManipulator,s,t,this.view.state.camera),C(this.rotateHeadingManipulator,s,t,this.view.renderCoordsHelper),L(this.rotateTiltManipulator,s,t),this.resizeManipulators.forEach(((e,i)=>R(e,this._resizeHandles[i],s,t))),this._previewPlaneOutlineVisualElement.visible=!1,this._previewPlaneGridVisualElement.visible=!1);const i=ht(bt.get(),rt(t.basis1),rt(t.basis2),1),r=et(wt.get(),i),a=st(r,s,r);this._previewPlaneOutlineVisualElement.transform=a,this._previewPlaneGridVisualElement.transform=a,this._updateMaterials()}_updateMaterials(){const t=[O[0],O[1],O[2],O[3]*this._previewPlaneOpacity];this._previewPlaneOutlineVisualElement.color=t;const e=[F[0],F[1],F[2],F[3]*this._previewPlaneOpacity];this._previewPlaneGridVisualElement.backgroundColor=e,this._previewPlaneGridVisualElement.gridColor=[0,0,0,0]}updateManipulatorsInteractive(t){if(!t.grabbing)return this.shiftManipulator.interactive=!0,this.rotateHeadingManipulator.interactive=!0,this.rotateTiltManipulator.interactive=!0,void this.resizeManipulators.forEach((t=>{t.interactive=!0}));this.shiftManipulator.interactive=this.shiftManipulator===t,this.rotateHeadingManipulator.interactive=this.rotateHeadingManipulator===t,this.rotateTiltManipulator.interactive=this.rotateTiltManipulator===t,this.resizeManipulators.forEach((e=>{e.interactive=e===t}))}testData(){return{plane:o(this.analysisViewData.plane)?this.analysisViewData.plane:null}}};function Gt(t,e,s,i){const r=H(s,yt(i),t.direction,ft()),a=mt();return vt(r,t,a)?{type:"shift",creatingPointerId:e,hasBeenDragged:!1,shiftPlane:r,depth:0,startPoint:a}:null}function zt(t){return"mouse"!==t.pointerType||0===t.button}t([h()],Ht.prototype,"clock",void 0),t([h({constructOnly:!0})],Ht.prototype,"view",void 0),t([h()],Ht.prototype,"analysisView",void 0),t([h()],Ht.prototype,"analysisViewData",void 0),t([h({readOnly:!0})],Ht.prototype,"state",null),t([h({readOnly:!0,value:null})],Ht.prototype,"cursor",null),t([h()],Ht.prototype,"analysis",null),t([h({readOnly:!0})],Ht.prototype,"layersMode",void 0),t([h({aliasOf:"analysis.tiltEnabled"})],Ht.prototype,"tiltEnabled",void 0),t([h({value:null})],Ht.prototype,"inputState",null),Ht=t([m("esri.views.3d.interactive.analysisTools.slice.SliceTool")],Ht);const At=ct(),Bt=X(),qt=Ht,Kt=u.getLogger("esri.widgets.Slice.SliceViewModel"),Qt=new Set;let Nt=class extends Ct{constructor(t){super(t),this.analysisView=null,this.logger=Kt,this.supportedViewType="3d",this.unsupportedErrorMessage="SliceViewModel is only supported in 3D views.",this.handles=new q,this.analysis=new Ft,this.shape=null,this.tiltEnabled=!1,this.ready=!1,Qt.add(this)}initialize(){this.handles.add(this.watch("shape",((t,e)=>{a(e)&&o(t)?a(this.tool)&&!this.creatingTool&&this.createTool():o(e)&&a(t)&&this.removeTool()}),!0)),this.handles.add(this.watch("tiltEnabled",(t=>this._updateToolOrRecreate((e=>e.tiltEnabled=t))),!0))}destroy(){Qt.delete(this),this.handles=l(this.handles)}get state(){return this.disabled||!this.ready?"disabled":a(this.tool)||"pending"===this.tool.toolState?"ready":this.tool.state}get excludedLayers(){return this.analysis.excludedLayers}set excludedLayers(t){this.analysis.excludedLayers=t}get excludeGroundSurface(){return this.analysis.excludeGroundSurface}set excludeGroundSurface(t){this.analysis.excludeGroundSurface=t}async start(){return this.removeTool(),this.ready||await N(this,"ready"),Qt.forEach((t=>{t.view===this.view&&t!==this&&t.clear()})),this.createTool()}clear(){this.removeTool(),this.shape=null,this.excludeGroundSurface=!1,this.excludedLayers=new s}removeSliceAndStart(){return this.removeTool(),this.shape=null,this.start()}enterExcludeLayerMode(){o(this.tool)&&this.tool.enterExcludeLayerMode()}exitExcludeLayerMode(){o(this.tool)&&this.tool.exitExcludeLayerMode()}_updateToolOrRecreate(t){o(this.tool)?t(this.tool):this.creatingTool&&Q(this.createTool())}onConnectToAnalysisView(t){this.analysisView=t,this.ready=!0}onDisconnectFromAnalysisView(){this.ready=!1,this.analysisView=null}createToolParams(){return{toolConstructor:qt,constructorArguments:()=>({tiltEnabled:this.tiltEnabled})}}};t([h()],Nt.prototype,"analysisView",void 0),t([h()],Nt.prototype,"analysis",void 0),t([h({readOnly:!0})],Nt.prototype,"state",null),t([h({aliasOf:"analysis.shape"})],Nt.prototype,"shape",void 0),t([h({aliasOf:"analysis.tiltEnabled"})],Nt.prototype,"tiltEnabled",void 0),t([J("tool.layersMode")],Nt.prototype,"layersMode",void 0),t([h()],Nt.prototype,"excludedLayers",null),t([h({nonNullable:!0})],Nt.prototype,"excludeGroundSurface",null),t([h()],Nt.prototype,"ready",void 0),Nt=t([m("esri.widgets.slice.SliceViewModel")],Nt);const Jt=Nt;export{Jt as default};
