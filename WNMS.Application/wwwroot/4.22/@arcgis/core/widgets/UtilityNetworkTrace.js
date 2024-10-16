/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{_ as e}from"../chunks/tslib.es6.js";import{HandleOwnerMixin as t}from"../core/HandleOwner.js";import{whenFalse as s}from"../core/watchUtils.js";import{aliasOf as i}from"../core/accessorSupport/decorators/aliasOf.js";import"../core/lang.js";import"../chunks/ensureType.js";import{property as o}from"../core/accessorSupport/decorators/property.js";import{subclass as r}from"../core/accessorSupport/decorators/subclass.js";import l from"./Widget.js";import{s as a}from"../chunks/widgetUtils.js";import{m as n}from"../chunks/messageBundle.js";import"../chunks/Logger.js";import{t as c}from"../chunks/jsxFactory.js";import h,{G as p}from"./UtilityNetworkTrace/UtilityNetworkTraceViewModel.js";import"../core/Accessor.js";import"../chunks/deprecate.js";import"../chunks/metadata.js";import"../chunks/handleUtils.js";import"../chunks/ArrayPool.js";import"../core/scheduling.js";import"../chunks/nextTick.js";import"../core/promiseUtils.js";import"../core/Error.js";import"../chunks/object.js";import"../config.js";import"../chunks/string.js";import"../core/Handles.js";import"../core/Collection.js";import"../chunks/Evented.js";import"../chunks/shared.js";import"../chunks/reactiveUtils.js";import"../intl.js";import"../chunks/number.js";import"../chunks/jsonMap.js";import"../chunks/locale.js";import"../chunks/messages.js";import"../request.js";import"../kernel.js";import"../core/urlUtils.js";import"../chunks/assets.js";import"../chunks/domUtils.js";import"../chunks/Promise.js";import"../chunks/uuid.js";import"../core/accessorSupport/decorators/cast.js";import"../chunks/projector.js";import"../chunks/jsxWidgetSupport.js";import"../geometry/Point.js";import"../chunks/reader.js";import"../chunks/writer.js";import"../geometry/Geometry.js";import"../chunks/JSONSupport.js";import"../geometry/SpatialReference.js";import"../geometry/support/webMercatorUtils.js";import"../chunks/Ellipsoid.js";import"../chunks/featureQueryAll.js";import"../rest/support/Query.js";import"../geometry.js";import"../geometry/Extent.js";import"../geometry/Multipoint.js";import"../chunks/zmUtils.js";import"../geometry/Polygon.js";import"../chunks/extentUtils.js";import"../geometry/Polyline.js";import"../chunks/typeUtils.js";import"../geometry/support/jsonUtils.js";import"../TimeExtent.js";import"../chunks/timeUtils.js";import"../chunks/enumeration.js";import"../chunks/DataLayerSource.js";import"../layers/support/Field.js";import"../chunks/domains.js";import"../layers/support/CodedValueDomain.js";import"../layers/support/Domain.js";import"../layers/support/InheritedDomain.js";import"../layers/support/RangeDomain.js";import"../chunks/fieldType.js";import"../rest/support/StatisticDefinition.js";import"../rest/support/FeatureSet.js";import"../Graphic.js";import"../PopupTemplate.js";import"../layers/support/fieldUtils.js";import"../chunks/arcadeOnDemand.js";import"../popup/content.js";import"../popup/content/AttachmentsContent.js";import"../popup/content/Content.js";import"../popup/content/CustomContent.js";import"../popup/content/ExpressionContent.js";import"../popup/ElementExpressionInfo.js";import"../popup/content/FieldsContent.js";import"../popup/FieldInfo.js";import"../popup/support/FieldInfoFormat.js";import"../chunks/date.js";import"../popup/content/MediaContent.js";import"../popup/content/BarChartMediaInfo.js";import"../chunks/chartMediaInfoUtils.js";import"../chunks/MediaInfo.js";import"../popup/content/support/ChartMediaInfoValue.js";import"../popup/content/support/ChartMediaInfoValueSeries.js";import"../popup/content/ColumnChartMediaInfo.js";import"../popup/content/ImageMediaInfo.js";import"../popup/content/support/ImageMediaInfoValue.js";import"../popup/content/LineChartMediaInfo.js";import"../popup/content/PieChartMediaInfo.js";import"../popup/content/TextContent.js";import"../popup/ExpressionInfo.js";import"../popup/LayerOptions.js";import"../popup/RelatedRecordsInfo.js";import"../popup/support/RelatedRecordsInfoFieldOrder.js";import"../support/actions/ActionBase.js";import"../chunks/Identifiable.js";import"../support/actions/ActionButton.js";import"../support/actions/ActionToggle.js";import"../symbols.js";import"../symbols/CIMSymbol.js";import"../symbols/Symbol.js";import"../Color.js";import"../chunks/colorUtils.js";import"../chunks/mathUtils.js";import"../chunks/common.js";import"../symbols/ExtrudeSymbol3DLayer.js";import"../symbols/Symbol3DLayer.js";import"../chunks/utils.js";import"../symbols/edges/Edges3D.js";import"../chunks/screenUtils.js";import"../chunks/materialUtils.js";import"../chunks/opacityUtils.js";import"../symbols/edges/SketchEdges3D.js";import"../symbols/edges/SolidEdges3D.js";import"../chunks/Symbol3DMaterial.js";import"../symbols/FillSymbol.js";import"../symbols/SimpleLineSymbol.js";import"../symbols/LineSymbol.js";import"../symbols/LineSymbolMarker.js";import"../symbols/FillSymbol3DLayer.js";import"../symbols/patterns/LineStylePattern3D.js";import"../symbols/patterns/StylePattern3D.js";import"../chunks/utils2.js";import"../chunks/colors.js";import"../chunks/symbolLayerUtils3D.js";import"../chunks/aaBoundingBox.js";import"../chunks/aaBoundingRect.js";import"../symbols/Font.js";import"../symbols/IconSymbol3DLayer.js";import"../chunks/persistableUrlUtils.js";import"../symbols/LabelSymbol3D.js";import"../symbols/Symbol3D.js";import"../chunks/collectionUtils.js";import"../portal/Portal.js";import"../chunks/Loadable.js";import"../portal/PortalQueryParams.js";import"../portal/PortalQueryResult.js";import"../portal/PortalUser.js";import"../portal/PortalFolder.js";import"../portal/PortalGroup.js";import"../symbols/LineSymbol3DLayer.js";import"../symbols/ObjectSymbol3DLayer.js";import"../symbols/PathSymbol3DLayer.js";import"../symbols/TextSymbol3DLayer.js";import"../symbols/WaterSymbol3DLayer.js";import"../chunks/Thumbnail.js";import"../chunks/Symbol3DVerticalOffset.js";import"../symbols/callouts/Callout3D.js";import"../symbols/callouts/LineCallout3D.js";import"../symbols/LineSymbol3D.js";import"../symbols/MarkerSymbol.js";import"../symbols/MeshSymbol3D.js";import"../symbols/PictureFillSymbol.js";import"../chunks/urlUtils.js";import"../symbols/PictureMarkerSymbol.js";import"../symbols/PointSymbol3D.js";import"../symbols/PolygonSymbol3D.js";import"../symbols/SimpleFillSymbol.js";import"../symbols/SimpleMarkerSymbol.js";import"../symbols/TextSymbol.js";import"../symbols/WebStyleSymbol.js";import"../rest/networks/trace.js";import"../chunks/utils3.js";import"../chunks/scaleUtils.js";import"../chunks/unitUtils.js";import"../chunks/projectionEllipsoid.js";import"../chunks/floorFilterUtils.js";import"../rest/networks/support/TraceResult.js";import"../rest/networks/support/AggregatedGeometry.js";import"../rest/networks/support/FunctionResult.js";import"../rest/networks/support/NetworkElement.js";import"../rest/networks/support/TraceLocation.js";import"../rest/networks/support/TraceParameters.js";import"../networks/support/TraceConfiguration.js";import"../chunks/typeUtils2.js";import"../geometry/geometryEngine.js";import"../chunks/geometryEngineBase.js";import"../chunks/hydrated.js";import"../geometry/projection.js";import"../chunks/mat4.js";import"../chunks/pe.js";import"../chunks/geodesicConstants.js";import"../geometry/support/GeographicTransformation.js";import"../geometry/support/GeographicTransformationStep.js";import"../chunks/zscale.js";const u="esri-utility-trace-network",d="esri-utility-trace-network__loader-container",m="esri-utility-trace-network__loader",g="esri-utility-trace-network__add-button-container",b="esri-utility-trace-network__notice-container",y="esri-utility-trace-network__list-container",w="esri-utility-trace-network__results-container",k="esri-utility-trace-network__flow",j="esri-widget",v="esri-widget--panel",_="esri-widget--disabled",S="esri-match-height",T="esri-icon-UtilityNetworkTrace";function R(e){return{height:e+"px"}}let C=class extends(t(l)){constructor(e,t){super(e,t),this._tracesExists=!0,this._graphicHandler=null,this._selectToolActive=!1,this._activeTrace=null,this._activeSwatch="",this._traceHeaderForFlow="",this._assetGroupHeader="",this._assetTypeHeader="",this._traceResultsFunctions=[],this._traceResultsAssetGroup=[],this._traceResultsAssetType=[],this._traceResultsIndividual=[],this._showTraceResultFunctions=!1,this._showTraceResultAssetGroup=!1,this._showTraceResultAssetType=!1,this._showIndividualRecords=!1,this._activeTab="input",this._flagViewType="starting-point",this._alertRemoveModal=!1,this._warningNoFlag=!1,this._warningNoTraceSelected=!1,this._warningNoStartAssetFound=!1,this._warningNoBarrierAssetFound=!1,this._confirmReset=!1,this._showResultOptions=!1,this._resultDisplayField="objectid",this._resultSortField="objectid",this._resultSortOrder="desc",this._swatchNode=null,this._individualResultNode=null,this.disabled=!0,this.flags=[],this.gdbVersion="sde.DEFAULT",this.goToOverride=null,this.iconClass=T,this.label=void 0,this.messages=null,this.messagesCommon=null,this.selectedTraces=[],this.selectOnComplete=!0,this.showGraphicsOnComplete=!0,this.showSelectionAttributes=!0,this.view=null,this.viewModel=new h}initialize(){this._UtilityNetworkTraceInitialized(),this._graphicHandler=new p}async checkCanTrace(){this._confirmReset=!1;const e=this.viewModel.checkCanTrace();e.status?(this._warningNoFlag=!1,this._warningNoTraceSelected=!1,this._warningNoTraceSelected=!1,this._showTraceResultFunctions=!1,this._showTraceResultAssetGroup=!1,this._showTraceResultAssetType=!1,this._showIndividualRecords=!1,this.switchTab("result"),this.viewModel._activeProgress=!0,await this.viewModel.callTrace(),this.viewModel._activeProgress=!1):e.issues.forEach((e=>{"noStart"===e?this._warningNoFlag=!0:this._warningNoTraceSelected=!0})),this.scheduleRender()}confirmReset(){this._confirmReset=!0}render(){const{state:e}=this.viewModel,t="loading"===e?this.renderLoading():this.renderUtilityNetworkTrace();return c("div",{class:this.classes(u,j,v,{[_]:this.disabled})},t)}loadDependencies(){return Promise.all([import("../chunks/calcite-action.js"),import("../chunks/calcite-action-group.js"),import("../chunks/calcite-action-pad.js"),import("../chunks/calcite-checkbox.js"),import("../chunks/calcite-color-picker-swatch.js"),import("../chunks/calcite-combobox.js"),import("../chunks/calcite-combobox-item.js"),import("../chunks/calcite-block.js"),import("../chunks/calcite-block-section.js"),import("../chunks/calcite-button.js"),import("../chunks/calcite-flow.js"),import("../chunks/calcite-icon.js"),import("../chunks/calcite-label.js"),import("../chunks/calcite-list.js"),import("../chunks/calcite-list-item.js"),import("../chunks/calcite-modal.js"),import("../chunks/calcite-notice.js"),import("../chunks/calcite-option.js"),import("../chunks/calcite-panel.js"),import("../chunks/calcite-popover.js"),import("../chunks/calcite-popover-manager.js"),import("../chunks/calcite-select.js"),import("../chunks/calcite-tabs.js"),import("../chunks/calcite-tab.js"),import("../chunks/calcite-tab-nav.js"),import("../chunks/calcite-tab-title.js")])}switchTab(e){this._activeTab=e,this.scheduleRender()}switchToFunctions(e,t){this._traceResultsFunctions=e,this._showTraceResultFunctions=t,this.scheduleRender()}switchToAssetGroup(e,t,s){this._traceHeaderForFlow=t,this._traceResultsAssetGroup=e,this._showTraceResultAssetGroup=s,this.scheduleRender()}switchToAssetType(e,t,s){this._assetGroupHeader=t,this._traceResultsAssetType=e,this._showTraceResultAssetType=s,this.scheduleRender()}switchToIndividualRecords(e,t,s){this._assetTypeHeader=t,this._traceResultsIndividual=e,this._showIndividualRecords=s,this.scheduleRender()}renderLoading(){return c("div",{class:d,key:"loader"},c("div",{class:m}))}renderUtilityNetworkTrace(){const{messages:e}=this;let t=c("calcite-tabs",{position:"above",layout:"center",class:S},c("calcite-tab-nav",{slot:"tab-nav"},c("calcite-tab-title",{active:"input"===this._activeTab,onclick:()=>{this.switchTab("input")}},e.inputsStrings.headerTabInputs),c("calcite-tab-title",{active:"result"===this._activeTab,onclick:()=>{this.switchTab("result")}},e.resultsStrings.headerTabResults)),c("calcite-tab",{active:"input"===this._activeTab,class:S},this.renderInputPanel()),c("calcite-tab",{active:"result"===this._activeTab,class:S},this.viewModel._activeProgress?c("calcite-loader",{active:!0,label:e.alertsStrings.traceExecuting,text:e.alertsStrings.traceExecuting,type:"indeterminate"}):this.viewModel._traceResults.length>0?this.renderResultPanel():this.renderWarningMessage("noTraceExecuted",!1),c("calcite-modal",{active:this._confirmReset,color:"blue",scale:"m",width:"s","intl-close":"Close",onCalciteModalClose:()=>{this._confirmReset=!1}},c("h3",{slot:"header"},e.resultsStrings.startOverButton),c("div",{slot:"content"},e.resultsStrings.startOverValidation),c("calcite-button",{slot:"secondary",width:"full",appearance:"outline",onclick:()=>{this._confirmReset=!1}},e.globalStrings.cancel),c("calcite-button",{slot:"primary",width:"full",onclick:()=>{this._confirmReset=!1,this.viewModel.reset(),this.switchTab("input")}},e.globalStrings.ok))));return this._tracesExists||(t=c("calcite-panel",null,this.renderWarningMessage("noTraceConfig",!1))),t}renderInputPanel(){const{messages:e}=this;return c("calcite-flow",{class:k},c("calcite-panel",null,this._warningNoFlag?this.renderWarningMessage("flag",!0):null,this._warningNoTraceSelected?this.renderWarningMessage("trace",!0):null,this.renderTraceSelectorContainer(),this.renderStartFlagsContainer(),this.renderBarriersFlagsContainer(),this._warningNoFlag&&this._warningNoTraceSelected?c("div",{styles:R(10)}):null,c("calcite-button",{slot:"footer",scale:"m",color:"blue",width:"full",onclick:()=>{this.checkCanTrace()}},e.tracingStrings.runTrace)),this._selectToolActive?this.renderActiveTool():null)}renderResultPanel(){return c("div",{class:this.classes(S,w)},c("calcite-flow",null,this.renderTraceResults(),this._showTraceResultFunctions?this.renderTraceResultFunctions():null,this._showTraceResultAssetGroup?this.renderTraceResultByAssetGroup():null,this._showTraceResultAssetType?this.renderTraceResultByAssetType():null,this._showIndividualRecords?this.renderTraceResultIndividual():null))}renderStartFlagsContainer(){const{messages:e}=this,t=[];let s=[];return s=this.viewModel._flags.filter((e=>"starting-point"===e.type)),s.forEach((e=>{e.displayValue&&t.push(this.renderFlagRow(e,"start"))})),c("calcite-block",{heading:e.inputsStrings.headerStartingPoint+" ("+s.length+")",open:!0,collapsible:!0},c("div",{slot:"icon"},c("calcite-icon",{icon:"pin",scale:"s"})),c("div",null,e.inputsStrings.startingPointHint),c("div",{class:y},t),this._warningNoStartAssetFound?this.renderWarningMessage("noStartAsset",!0):null,c("div",{class:g},c("calcite-button",{alignment:"center",appearance:"outline","icon-start":"plus",scale:"m",href:"",label:e.inputsStrings.addPointOption,onclick:()=>{this._flagViewType="starting-point",this._selectToolActive=!0,this._warningNoStartAssetFound=!1,this.viewModel.addFlagByHit("starting-point").then((e=>{e||(this._warningNoStartAssetFound=!0),this._selectToolActive=!1,this.scheduleRender()}))},round:!0})))}renderBarriersFlagsContainer(){const{messages:e}=this,t=[];let s=[];return s=this.viewModel._flags.filter((e=>"barrier"===e.type)),s.forEach((e=>{e.displayValue&&t.push(this.renderFlagRow(e,"barrier"))})),c("calcite-block",{heading:e.inputsStrings.headerBarrier+" ("+s.length+")",open:!1,collapsible:!0},c("div",{slot:"icon"},c("calcite-icon",{icon:"x-circle-f",scale:"s"})),c("div",null,e.inputsStrings.barrierPointHint),c("div",{class:y},t),this._warningNoBarrierAssetFound?this.renderWarningMessage("noBarrierAsset",!0):null,c("div",{class:g},c("calcite-button",{alignment:"center",appearance:"outline",color:"blue","icon-start":"plus",scale:"m",href:"",round:!0,label:e.inputsStrings.addPointOption,onclick:()=>{this._flagViewType="barrier",this._selectToolActive=!0,this._warningNoBarrierAssetFound=!1,this.viewModel.addFlagByHit("barrier").then((e=>{e||(this._warningNoBarrierAssetFound=!0),this._selectToolActive=!1,this.scheduleRender()}))}})))}renderFlagRow(e,t){const{messages:s}=this,i=[];let o=!1;return null!==e.allTerminals&&void 0!==e.allTerminals&&e.allTerminals.terminals.length>0&&(o=!0,e.allTerminals.terminals.forEach((t=>{let s=!1;e.selectedTerminals.find((e=>e===t.id))&&(s=!0),i.push(c("calcite-combobox-item",{key:t.name,selected:s,value:t.id,"text-label":t.name,onclick:t=>{this.viewModel.addTerminal(t.target.value,e)}}))}))),c("calcite-block",{key:"pop"+e.globalId+e.type+e.id+t,heading:e.displayValue.value,collapsible:null!==e.allTerminals||"barrier"===e.type},c("calcite-action",{textEnabled:!0,slot:"header-menu-actions",text:s.globalStrings.remove,label:s.globalStrings.remove,onCalciteListItemChange:()=>{this.viewModel.removeFlag(e)},onclick:()=>{this.viewModel.removeFlag(e)},scale:"s",icon:"trash"}),c("calcite-action",{textEnabled:!0,slot:"header-menu-actions",text:s.globalStrings.zoomToFeature,label:s.globalStrings.zoomToFeature,onCalciteListItemChange:()=>{this.viewModel.zoomToAsset(e.details.geometry)},onclick:()=>{this.viewModel.zoomToAsset(e.details.geometry)},scale:"s",icon:"zoom-to-object"}),"barrier"===e.type?c("calcite-label",{scale:"s",layout:"inline"},c("calcite-checkbox",{checked:e.isFilterBarrier,scale:"s",onclick:()=>{this.viewModel.manageFilterBarrier(!e.isFilterBarrier,e)}}),s.inputsStrings.barrierFilter):null,o?c("calcite-combobox",{label:s.globalStrings.selectTerminalPlaceholder,placeholder:s.globalStrings.selectTerminalPlaceholder,"selection-mode":"multi",scale:"s",maxItems:0,onCalciteComboboxChipDismiss:t=>{this.viewModel.removeTerminal(t.target.value,e)}},i):null)}renderActiveTool(){const{messages:e}=this;return c("calcite-panel",{"height-scale":"s",onCalcitePanelBackClick:()=>{this.viewModel._clickHandler&&(this.viewModel._clickHandler.remove(),this.view.popup.autoOpenEnabled=!0),this._selectToolActive=!1},heading:e.inputsStrings.addPointOption},c("calcite-block",{open:!0,collapsible:!1,heading:""},c("div",{styles:{textAlign:"center"}},c("calcite-icon",{icon:"starting-point"===this._flagViewType?"pin":"x-circle-f",scale:"s"})),c("div",{styles:{textAlign:"center"}},e.inputsStrings.addPointHint)))}renderTraceSelectorContainer(){const{messages:e}=this,t=[];return this.viewModel._traces.length>0&&this.viewModel._traces.forEach((e=>{t.push(c("calcite-combobox-item",{key:e.globalId,selected:e.selected,value:e.globalId,"text-label":e.title,onCalciteComboboxItemChange:t=>{const s=t.target;this.viewModel.selectTraces(s.selected,e.globalId),this.viewModel._traces.length>0&&(this._warningNoTraceSelected=!1)}}))})),c("calcite-block",{heading:e.tracingStrings.traceOperation,open:!0,collapsible:!0},c("calcite-combobox",{label:e.inputsStrings.selectTraces,placeholder:e.inputsStrings.selectTraces,"selection-mode":"multi",scale:"s",maxItems:0,onCalciteComboboxChipDismiss:e=>{this.viewModel.selectTraces(!1,e.target.value),this.viewModel._traces.length>0&&(this._warningNoTraceSelected=!1)}},t))}renderStartOverContainer(){const{messages:e}=this;return c("calcite-button",{slot:"footer",scale:"m",color:"blue",width:"full",onclick:()=>{this.confirmReset()}},e.resultsStrings.startOverButton)}renderWarningMessage(e,t,s){const{messages:i}=this;let o=i.alertsStrings.NoRunAlertHeader,r=i.alertsStrings.noResultsInfo;switch(e){case"flag":o=i.alertsStrings.startingPointAlertHeader,r=i.alertsStrings.startingPointAlert;break;case"trace":o=i.alertsStrings.selectTraceAlertHeader,r=i.alertsStrings.selectTraceAlert;break;case"noTraceExecuted":o=i.alertsStrings.NoRunAlertHeader,r=i.alertsStrings.noResultsInfo;break;case"noBarrierAsset":case"noStartAsset":o=i.alertsStrings.noAssetsFoundHeader,r=i.alertsStrings.noAssetFound;break;case"noTraceConfig":o="",r=i.alertsStrings.noTraceSupported;break;default:o=i.alertsStrings.genericResultHeader,r=s||""}return c("div",{class:b,key:e},c("calcite-notice",{open:!0,key:e,active:!0,dismissible:t,icon:!0,scale:"s",width:"auto",color:"red",onCalciteNoticeClose:()=>{switch(e){case"flag":this._warningNoFlag=!1;break;case"trace":this._warningNoTraceSelected=!1;break;case"noStartAsset":this._warningNoStartAssetFound=!1;break;case"noBarrierAsset":this._warningNoBarrierAssetFound=!1}}},c("div",{slot:"title"},o),c("div",{slot:"message"},r)))}renderRemoveTraceContainer(e){const{messages:t}=this;return c("calcite-action",{textEnabled:!0,slot:"header-menu-actions",text:t.globalStrings.clearResults,label:t.globalStrings.clearResults,onCalciteListItemChange:()=>{this._alertRemoveModal=!0,this._activeTrace=e.trace},onclick:()=>{this._alertRemoveModal=!0,this._activeTrace=e.trace},scale:"s",icon:"trash"})}renderHighlightColorPicker(e,t){const{messages:s}=this;return c("calcite-action-pad",{position:"start",layout:"grid",expandDisabled:!0},c("calcite-action-group",{layout:"grid",columns:4},c("calcite-action",{text:s.resultsStrings.graphicColor,label:s.resultsStrings.graphicColor,scale:"s",onclick:()=>{this.viewModel.changeResultGraphicColor(this._graphicHandler.getHighlightColor(0),t)}},c("calcite-color-picker-swatch",{scale:"s",color:this._graphicHandler.getHighlightColor(0).hex,active:e.color===this._graphicHandler.getHighlightColor(0).color})),c("calcite-action",{text:"",scale:"s",onclick:()=>{this.viewModel.changeResultGraphicColor(this._graphicHandler.getHighlightColor(1),t)}},c("calcite-color-picker-swatch",{scale:"s",color:this._graphicHandler.getHighlightColor(1).hex,active:e.color===this._graphicHandler.getHighlightColor(1).color})),c("calcite-action",{text:"",scale:"s",onclick:()=>{this.viewModel.changeResultGraphicColor(this._graphicHandler.getHighlightColor(2),t)}},c("calcite-color-picker-swatch",{scale:"s",color:this._graphicHandler.getHighlightColor(2).hex,active:e.color===this._graphicHandler.getHighlightColor(2).color})),c("calcite-action",{text:"",scale:"s",onclick:()=>{this.viewModel.changeResultGraphicColor(this._graphicHandler.getHighlightColor(3),t)}},c("calcite-color-picker-swatch",{scale:"s",color:this._graphicHandler.getHighlightColor(3).hex,active:e.color===this._graphicHandler.getHighlightColor(3).color})),c("calcite-action",{text:"",scale:"s",onclick:()=>{this.viewModel.changeResultGraphicColor(this._graphicHandler.getHighlightColor(4),t)}},c("calcite-color-picker-swatch",{scale:"s",color:this._graphicHandler.getHighlightColor(4).hex,active:e.color===this._graphicHandler.getHighlightColor(4).color}))))}renderTraceResults(){const{messages:e}=this,t=this.viewModel._traceResults,s=[];return t.forEach(((t,i)=>{let o=[],r=[],l=!1,a=!1,n=!1;null!==t.results&&t.results.hasOwnProperty("elements")?(null!==t.results.aggregatedGeometry&&(a=!0),null!==t.results.globalFunctionResults&&t.results.globalFunctionResults.length>0&&(r=t.results.globalFunctionResults,r.length>0&&(n=!0)),t.results.elements.length>0&&(o=t.results.elements,o.length>0&&(l=!0)),s.push(c("calcite-block",{key:t.trace.title,heading:t.trace.title,summary:t.trace.description,collapsible:!1,open:!0,intlOptions:e.resultsStrings.ellipsesOptions},this.renderRemoveTraceContainer(t),c("calcite-list",null,n?c("calcite-list-item",{label:e.resultsStrings.functionHeader+" ("+r.length+")",onclick:()=>{this.switchToFunctions(r,!0)}},c("calcite-icon",{icon:"chevron-right",scale:"s",slot:"content-end"})):null,l&&this.viewModel.showSelectionAttributes?c("calcite-list-item",{label:e.resultsStrings.viewFeatures+" ("+o.length+")",onclick:()=>{this.switchToAssetGroup(this._groupResultsByAssetGroup(t),t.trace.title+" ("+o.length+")",!0)}},c("calcite-icon",{icon:"chevron-right",scale:"s",slot:"content-end"})):this.viewModel.showSelectionAttributes?c("calcite-label",{layout:"inline",scale:"s"},e.resultsStrings.noSelectionResults):null),c("div",{styles:R(10)}),l?c("calcite-label",{layout:"inline",scale:"s",onclick:e=>{e.preventDefault(),e.stopPropagation(),this.viewModel.mergeSelection(!t.selectionEnabled,t.trace)}},c("calcite-checkbox",{checked:t.selectionEnabled,scale:"m",onclick:e=>{e.preventDefault(),e.stopPropagation(),this.viewModel.mergeSelection(!t.selectionEnabled,t.trace)}}),this.viewModel.showSelectionAttributes?e.resultsStrings.selectFeatures:e.resultsStrings.selectFeatures+" ("+o.length+")"):null,a?c("calcite-block-section",{"toggle-display":"switch",text:e.resultsStrings.highlightTrace,open:t.graphicEnabled,onCalciteBlockSectionToggle:e=>{e.target.open?this.viewModel.changeResultGraphicColor(t.graphicColor,t):this.viewModel.removeResultGraphicFromView(t)}},c("calcite-popover-manager",{key:"aggGeom_"+t.trace.globalId,autoClose:!0},c("calcite-action",{text:e.resultsStrings.graphicColor,label:e.resultsStrings.graphicColor,scale:"s",onclick:e=>{const t=e.target;this._swatchNode=t,this._activeSwatch="aggGeom_"+i,this.scheduleRender()}},c("calcite-color-picker-swatch",{scale:"s",color:t.graphicColor.hex})),c("calcite-popover",{label:e.resultsStrings.graphicColor,text:e.resultsStrings.graphicColor,referenceElement:this._swatchNode,placement:"auto",offsetDistance:5,offsetSkidding:0,open:this._activeSwatch==="aggGeom_"+i,onCalcitePopoverClose:()=>{this._swatchNode=null,this._activeSwatch=null}},this.renderHighlightColorPicker(t.graphicColor,t)))):null))):s.push(c("calcite-block",{key:"error-"+i,heading:t.trace.title,collapsible:!1,open:!0},this.renderRemoveTraceContainer(t),this.renderWarningMessage("noController",!1,t.status)))})),c("calcite-panel",{key:"traceResults"},s,this.renderStartOverContainer(),c("calcite-modal",{active:this._alertRemoveModal,color:"blue",scale:"m",width:"s","intl-close":"Close",onCalciteModalClose:()=>{this._alertRemoveModal=!1,this.scheduleRender()}},c("h3",{slot:"header"},e.globalStrings.clearResults),c("div",{slot:"content"},e.alertsStrings.clearResultsAlert),c("calcite-button",{slot:"secondary",width:"full",appearance:"outline",onclick:()=>{this._alertRemoveModal=!1,this.scheduleRender()}},e.globalStrings.cancel),c("calcite-button",{slot:"primary",width:"full",onclick:()=>{this.viewModel.clearResult(this._activeTrace),this._alertRemoveModal=!1,0===this.viewModel._traceResults.length?this.switchTab("input"):(this._activeTrace=this.viewModel._traceResults[0].trace,this._showTraceResultFunctions=!1,this._showTraceResultAssetGroup=!1,this._showTraceResultAssetType=!1,this._showIndividualRecords=!1),this.scheduleRender()}},e.globalStrings.ok)))}renderTraceResultFunctions(){const{messages:e}=this,t=this._traceResultsFunctions,s=[];return t.forEach((e=>{s.push(this.renderResultRowFunctions(e))})),c("calcite-panel",{key:"functionResultMultiple",onCalcitePanelBackClick:()=>{this.switchToFunctions([],!1)},heading:e.resultsStrings.functionHeader},s,this.renderStartOverContainer())}renderTraceResultByAssetGroup(){const e=this._traceResultsAssetGroup,t=[];for(const s in e){const i=e[s];for(const e in i)t.push(this.renderResultRowAssetGroup(i[e]))}return c("calcite-panel",{key:"assetGroupResultMultiple",onCalcitePanelBackClick:()=>{this.switchToAssetGroup([],"",!1)},heading:this._traceHeaderForFlow},c("calcite-list",null,t),this.renderStartOverContainer())}renderTraceResultByAssetType(){const e=this._traceResultsAssetType,t=[];for(const s in e)e[s].length>0&&t.push(this.renderResultRowAssetType(e[s]));return c("calcite-panel",{key:"assetTypeResult",onCalcitePanelBackClick:()=>{this.switchToAssetType([],"",!1)},heading:this._assetGroupHeader},t,this.renderStartOverContainer())}renderTraceResultIndividual(){const{messages:e}=this,t=this._traceResultsIndividual;t.sort(this._compare(this._resultSortField,this._resultSortOrder));const s=[],i=[],o=[];return t.length>0&&(t[0].details.fields.forEach((e=>{i.push(c("calcite-option",{key:"display"+e.name,label:e.alias,value:e.name,selected:e.name===this._resultDisplayField})),o.push(c("calcite-option",{key:"sort"+e.name,label:e.alias,value:e.name,selected:e.name===this._resultSortField}))})),t.forEach((e=>{s.push(this.renderResultRowIndividual(e))}))),c("calcite-popover-manager",{class:S,key:"popIndividualResult",autoClose:!0},c("calcite-panel",{key:"individualResult",onCalcitePanelBackClick:()=>{this._showResultOptions=!1,this.switchToIndividualRecords([],"",!1)},heading:this._assetTypeHeader},c("calcite-action",{"text-enabled":!0,slot:"header-actions-end",text:e.resultsStrings.displayAttribute,onCalciteListItemChange:()=>{this._showResultOptions=!0},onclick:()=>{this._showResultOptions=!0},scale:"s",icon:"gear",label:e.resultsStrings.displayAttribute,id:"field_options",afterCreate:a,bind:this,"data-node-ref":"_individualResultNode"}),s,this.renderStartOverContainer()),c("calcite-popover",{label:e.resultsStrings.displayAttribute,referenceElement:this._individualResultNode,placement:"auto",offsetDistance:5,offsetSkidding:0,open:this._showResultOptions,styles:(r=.75*this.domNode.clientWidth,{width:r+"px"})},c("calcite-block",{heading:e.resultsStrings.resultsListOptions,collapsible:!1,open:!0},c("calcite-label",{scale:"s"},e.resultsStrings.displayAttribute,c("calcite-select",{label:e.resultsStrings.displayAttribute,onCalciteSelectChange:e=>{const t=e.target;this._resultDisplayField=t.selectedOption.value,this._showResultOptions=!1}},i)),c("calcite-label",null,e.resultsStrings.sortBy,c("calcite-select",{label:e.resultsStrings.sortBy,onCalciteSelectChange:e=>{const s=e.target;this._resultSortField=s.selectedOption.value,t.sort(this._compare(this._resultSortField,this._resultSortOrder)),this._traceResultsIndividual=t,this._showResultOptions=!1}},o)))));var r}renderResultRow(e){let t=e[0].assetGroupCode;const s=this.viewModel.getValidSources().filter((t=>t.sourceId===e[0].networkSourceId));if(s.length>0){const i=s[0].assetGroups.filter((t=>t.assetGroupCode===e[0].assetGroupCode));i.length>0&&(t=i[0].assetGroupName)}return c("calcite-list-item",{label:t+" ("+e.length+")",onclick:()=>{this.switchToAssetType(this._groupResultsByAssetType(e),t+" ("+e.length+")",!0)}},c("calcite-icon",{icon:"chevron-right",scale:"s",slot:"content-end"}))}renderResultRowFunctions(e){return c("calcite-block",{heading:e.functionType+" = "+e.result,collapsible:!1})}renderResultRowAssetGroup(e){let t=e[0].assetGroupCode;const s=e[0].assetGroupCode,i=this.viewModel.getValidSources().filter((t=>t.sourceId===e[0].networkSourceId));if(i.length>0){const e=i[0].assetGroups.filter((e=>e.assetGroupCode===s));e.length>0&&(t=e[0].assetGroupName)}return c("calcite-list-item",{label:t+" ("+e.length+")",onclick:()=>{this.switchToAssetType(this._groupResultsByAssetType(e),t+" ("+e.length+")",!0)}},c("calcite-icon",{slot:"content-end",icon:"chevron-right",scale:"s"}))}renderResultRowAssetType(e){let t=e[0].assetTypeCode;const s=e[0].assetGroupCode,i=this.viewModel.getValidSources().filter((t=>t.sourceId===e[0].networkSourceId));if(i.length>0){const o=i[0].assetGroups.filter((e=>e.assetGroupCode===s));if(o.length>0){const s=o[0].assetTypes.filter((t=>t.assetTypeCode===e[0].assetTypeCode));s.length>0&&(t=s[0].assetTypeName)}}return c("calcite-list-item",{label:t+" ("+e.length+")",onclick:()=>{this.viewModel.queryFeaturesById(e).then((s=>{if(null!==s){const i=this._appendAttributes(e,s.featureSet,"objectId");this.switchToIndividualRecords(i,t+" ("+e.length+")",!0)}else this.switchToIndividualRecords(e,t+" ("+e.length+")",!0)}))}},c("calcite-icon",{slot:"content-end",icon:"chevron-right",scale:"s"}))}renderResultRowIndividual(e){const{messages:t}=this;let s=e.details.attributes[this._resultDisplayField];return s&&""!==s&&null!==s||(s=t.resultsStrings.noValue),c("calcite-list-item",{label:s,onclick:()=>{this.viewModel.zoomToAsset(e.details.geometry)}},c("calcite-icon",{icon:"zoom-to-object","text-label":t.globalStrings.zoomToFeature,scale:"s",slot:"content-end"}))}_UtilityNetworkTraceInitialized(){return this.viewModel.loadUtilityNetwork().then((e=>e?(this.viewModel.selectTracesOnLoad(),this.viewModel._traces.length<=0&&(this._tracesExists=!1),this.viewModel.flags.length>0?this.view.when().then((()=>{const e=s(this.view,"updating",(async()=>{this._warningNoFlag=!1,this._warningNoBarrierAssetFound=!1,this._warningNoStartAssetFound=!1;const t=await this.viewModel.addFlagsOnLoad();t.includes("barrier")&&(this._warningNoBarrierAssetFound=!0),t.includes("starting-point")&&(this._warningNoStartAssetFound=!0),this.disabled=!1,e.remove(),this.scheduleRender()}))})):this.view.when().then((()=>{const e=s(this.view,"updating",(()=>{this.disabled=!1,e.remove()}))}))):(this._tracesExists=!1,this.view.when().then((()=>{const e=s(this.view,"updating",(()=>{this.disabled=!1,e.remove()}))})))))}_groupResultsByAssetGroup(e){const t=[],s=e.results.elements,i=this._groupBy(s,"networkSourceId");for(const e in i)t.push(this._groupBy(i[e],"assetGroupCode"));return t}_groupResultsByAssetType(e){return this._groupBy(e,"assetTypeCode")}_appendAttributes(e,t,s){const i=[];return e.forEach((e=>{t.features.forEach((o=>{e[s]===o.attributes[s.toLowerCase()]&&(e.details=o,e.details.fields=t.fields,i.push(e))}))})),i}_compare(e,t){return(s,i)=>{let o=0,r=s.details.attributes[e],l=i.details.attributes[e];return isNaN(r)&&(r=r.toLowerCase()),isNaN(l)&&(l=l.toLowerCase()),"desc"===t?r>l?o=1:r<l&&(o=-1):r<l?o=1:r>l&&(o=-1),o}}_groupBy(e,t){return e.reduce((function(e,s){return(e[s[t]]=e[s[t]]||[]).push(s),e}),{})}};e([o()],C.prototype,"_confirmReset",void 0),e([o()],C.prototype,"disabled",void 0),e([i("viewModel.flags")],C.prototype,"flags",void 0),e([i("viewModel.gdbVersion")],C.prototype,"gdbVersion",void 0),e([i("viewModel.goToOverride")],C.prototype,"goToOverride",void 0),e([o()],C.prototype,"iconClass",void 0),e([o({aliasOf:{source:"messages.widgetLabel",overridable:!0}})],C.prototype,"label",void 0),e([o(),n("esri/widgets/UtilityNetworkTrace/t9n/UtilityNetworkTrace")],C.prototype,"messages",void 0),e([o(),n("esri/t9n/common")],C.prototype,"messagesCommon",void 0),e([i("viewModel.selectedTraces")],C.prototype,"selectedTraces",void 0),e([i("viewModel.selectOnComplete")],C.prototype,"selectOnComplete",void 0),e([i("viewModel.showGraphicsOnComplete")],C.prototype,"showGraphicsOnComplete",void 0),e([i("viewModel.showSelectionAttributes")],C.prototype,"showSelectionAttributes",void 0),e([i("viewModel.view")],C.prototype,"view",void 0),e([o({type:h})],C.prototype,"viewModel",void 0),C=e([r("esri.widgets.UtilityNetworkTrace")],C);const f=C;export{f as default};
