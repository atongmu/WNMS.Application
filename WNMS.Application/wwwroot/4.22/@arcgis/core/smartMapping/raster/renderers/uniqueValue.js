/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import s from"../../../core/Error.js";import{i as r}from"../../../core/lang.js";import{j as o,k as t,l as e}from"../../../chunks/rasterRendererHelper.js";import{processRasterRendererParameters as i}from"../support/utils.js";import"../../../chunks/Logger.js";import"../../../config.js";import"../../../chunks/object.js";import"../../../chunks/string.js";import"../../../Color.js";import"../../../chunks/colorUtils.js";import"../../../chunks/mathUtils.js";import"../../../chunks/common.js";import"../../../chunks/ensureType.js";import"../../../rasterRenderers.js";import"../../../core/accessorSupport/decorators/subclass.js";import"../../../chunks/metadata.js";import"../../../chunks/handleUtils.js";import"../../../renderers/AnimatedFlowRenderer.js";import"../../../chunks/tslib.es6.js";import"../../../chunks/JSONSupport.js";import"../../../core/Accessor.js";import"../../../chunks/deprecate.js";import"../../../core/accessorSupport/decorators/property.js";import"../../../chunks/ArrayPool.js";import"../../../core/scheduling.js";import"../../../chunks/nextTick.js";import"../../../core/promiseUtils.js";import"../../../chunks/screenUtils.js";import"../../../chunks/enumeration.js";import"../../../chunks/jsonMap.js";import"../../../renderers/ClassBreaksRenderer.js";import"../../../symbols.js";import"../../../symbols/CIMSymbol.js";import"../../../chunks/reader.js";import"../../../chunks/writer.js";import"../../../layers/support/fieldUtils.js";import"../../../chunks/arcadeOnDemand.js";import"../../../geometry.js";import"../../../geometry/Extent.js";import"../../../geometry/Geometry.js";import"../../../geometry/SpatialReference.js";import"../../../geometry/Point.js";import"../../../core/accessorSupport/decorators/cast.js";import"../../../geometry/support/webMercatorUtils.js";import"../../../chunks/Ellipsoid.js";import"../../../geometry/Multipoint.js";import"../../../chunks/zmUtils.js";import"../../../geometry/Polygon.js";import"../../../chunks/extentUtils.js";import"../../../geometry/Polyline.js";import"../../../chunks/typeUtils.js";import"../../../geometry/support/jsonUtils.js";import"../../../symbols/Symbol.js";import"../../../symbols/ExtrudeSymbol3DLayer.js";import"../../../symbols/Symbol3DLayer.js";import"../../../chunks/utils.js";import"../../../symbols/edges/Edges3D.js";import"../../../chunks/materialUtils.js";import"../../../chunks/opacityUtils.js";import"../../../symbols/edges/SketchEdges3D.js";import"../../../symbols/edges/SolidEdges3D.js";import"../../../chunks/Symbol3DMaterial.js";import"../../../symbols/FillSymbol.js";import"../../../symbols/SimpleLineSymbol.js";import"../../../symbols/LineSymbol.js";import"../../../symbols/LineSymbolMarker.js";import"../../../symbols/FillSymbol3DLayer.js";import"../../../symbols/patterns/LineStylePattern3D.js";import"../../../symbols/patterns/StylePattern3D.js";import"../../../chunks/utils2.js";import"../../../chunks/colors.js";import"../../../chunks/symbolLayerUtils3D.js";import"../../../chunks/aaBoundingBox.js";import"../../../chunks/aaBoundingRect.js";import"../../../symbols/Font.js";import"../../../symbols/IconSymbol3DLayer.js";import"../../../core/urlUtils.js";import"../../../chunks/persistableUrlUtils.js";import"../../../symbols/LabelSymbol3D.js";import"../../../core/Collection.js";import"../../../chunks/Evented.js";import"../../../chunks/shared.js";import"../../../symbols/Symbol3D.js";import"../../../chunks/collectionUtils.js";import"../../../portal/Portal.js";import"../../../kernel.js";import"../../../request.js";import"../../../chunks/Loadable.js";import"../../../chunks/Promise.js";import"../../../chunks/locale.js";import"../../../portal/PortalQueryParams.js";import"../../../portal/PortalQueryResult.js";import"../../../portal/PortalUser.js";import"../../../portal/PortalFolder.js";import"../../../portal/PortalGroup.js";import"../../../symbols/LineSymbol3DLayer.js";import"../../../symbols/ObjectSymbol3DLayer.js";import"../../../symbols/PathSymbol3DLayer.js";import"../../../symbols/TextSymbol3DLayer.js";import"../../../symbols/WaterSymbol3DLayer.js";import"../../../chunks/Thumbnail.js";import"../../../chunks/Symbol3DVerticalOffset.js";import"../../../symbols/callouts/Callout3D.js";import"../../../symbols/callouts/LineCallout3D.js";import"../../../symbols/LineSymbol3D.js";import"../../../symbols/MarkerSymbol.js";import"../../../symbols/MeshSymbol3D.js";import"../../../symbols/PictureFillSymbol.js";import"../../../chunks/urlUtils.js";import"../../../symbols/PictureMarkerSymbol.js";import"../../../symbols/PointSymbol3D.js";import"../../../symbols/PolygonSymbol3D.js";import"../../../symbols/SimpleFillSymbol.js";import"../../../symbols/SimpleMarkerSymbol.js";import"../../../symbols/TextSymbol.js";import"../../../symbols/WebStyleSymbol.js";import"../../../renderers/Renderer.js";import"../../../renderers/support/AuthoringInfo.js";import"../../../renderers/support/AuthoringInfoVisualVariable.js";import"../../../chunks/colorRamps.js";import"../../../rest/support/AlgorithmicColorRamp.js";import"../../../rest/support/ColorRamp.js";import"../../../rest/support/MultipartColorRamp.js";import"../../../chunks/VisualVariablesMixin.js";import"../../../renderers/visualVariables/ColorVariable.js";import"../../../renderers/visualVariables/VisualVariable.js";import"../../../chunks/LegendOptions.js";import"../../../renderers/visualVariables/support/ColorStop.js";import"../../../renderers/visualVariables/OpacityVariable.js";import"../../../renderers/visualVariables/support/OpacityStop.js";import"../../../renderers/visualVariables/RotationVariable.js";import"../../../renderers/visualVariables/SizeVariable.js";import"../../../renderers/visualVariables/support/SizeStop.js";import"../../../chunks/sizeVariableUtils.js";import"../../../chunks/visualVariableUtils.js";import"../../../Graphic.js";import"../../../PopupTemplate.js";import"../../../popup/content.js";import"../../../popup/content/AttachmentsContent.js";import"../../../popup/content/Content.js";import"../../../popup/content/CustomContent.js";import"../../../popup/content/ExpressionContent.js";import"../../../popup/ElementExpressionInfo.js";import"../../../popup/content/FieldsContent.js";import"../../../popup/FieldInfo.js";import"../../../popup/support/FieldInfoFormat.js";import"../../../chunks/date.js";import"../../../chunks/number.js";import"../../../popup/content/MediaContent.js";import"../../../popup/content/BarChartMediaInfo.js";import"../../../chunks/chartMediaInfoUtils.js";import"../../../chunks/MediaInfo.js";import"../../../popup/content/support/ChartMediaInfoValue.js";import"../../../popup/content/support/ChartMediaInfoValueSeries.js";import"../../../popup/content/ColumnChartMediaInfo.js";import"../../../popup/content/ImageMediaInfo.js";import"../../../popup/content/support/ImageMediaInfoValue.js";import"../../../popup/content/LineChartMediaInfo.js";import"../../../popup/content/PieChartMediaInfo.js";import"../../../popup/content/TextContent.js";import"../../../popup/ExpressionInfo.js";import"../../../popup/LayerOptions.js";import"../../../popup/RelatedRecordsInfo.js";import"../../../popup/support/RelatedRecordsInfoFieldOrder.js";import"../../../support/actions/ActionBase.js";import"../../../chunks/Identifiable.js";import"../../../support/actions/ActionButton.js";import"../../../support/actions/ActionToggle.js";import"../../../chunks/compilerUtils.js";import"../../../chunks/lengthUtils.js";import"../../../chunks/unitUtils.js";import"../../../chunks/projectionEllipsoid.js";import"../../../renderers/support/ClassBreakInfo.js";import"../../../chunks/commonProperties2.js";import"../../../symbols/support/jsonUtils.js";import"../../../chunks/symbolConversion.js";import"../../../renderers/RasterColormapRenderer.js";import"../../../renderers/support/ColormapInfo.js";import"../../../chunks/colorRampUtils.js";import"../../../chunks/colorUtils2.js";import"../../../renderers/RasterShadedReliefRenderer.js";import"../../../renderers/RasterStretchRenderer.js";import"../../../renderers/UniqueValueRenderer.js";import"../../../chunks/diffUtils.js";import"../../../renderers/support/UniqueValueInfo.js";import"../../../chunks/styleUtils.js";import"../../../renderers/VectorFieldRenderer.js";import"../../../geometry/support/normalizeUtils.js";import"../../../chunks/normalizeUtilsCommon.js";import"../../../chunks/vectorFieldUtils.js";import"../../../layers/support/PixelBlock.js";import"../../../chunks/pixelUtils.js";import"../../../chunks/utils4.js";import"../../../chunks/asyncUtils.js";import"../../../chunks/jsonUtils.js";import"../../../chunks/parser.js";import"../../../chunks/mat4.js";import"../../../chunks/_commonjsHelpers.js";import"../../../chunks/assets.js";import"../../../chunks/ItemCache.js";import"../../../chunks/MemCache.js";import"../../../symbols/support/cimSymbolUtils.js";import"../../../chunks/utils5.js";import"../../../layers/support/Field.js";import"../../../chunks/domains.js";import"../../../layers/support/CodedValueDomain.js";import"../../../layers/support/Domain.js";import"../../../layers/support/InheritedDomain.js";import"../../../layers/support/RangeDomain.js";import"../../../chunks/fieldType.js";import"../../../layers/support/RasterInfo.js";import"../../../chunks/generateRendererUtils.js";import"../../../rest/support/ImageHistogramParameters.js";import"../../../TimeExtent.js";import"../../../chunks/timeUtils.js";import"../../../layers/support/MosaicRule.js";import"../../../layers/support/DimensionalDefinition.js";import"../../../layers/support/RasterFunction.js";async function p(p){p=await async function(r){var o;r=await i(r);const{rasterInfo:p}=r.layer;if(p.bandCount>1)throw new s("raster-class-breaks-renderer:not-supported","Multiband raster is not supported");if(null===p.attributeTable&&!t(p))throw new s("raster-unique-value-renderer:not-supported","The source raster does not have an attribute table");const m=null==(o=r.classFieldName)?void 0:o.toLowerCase();if(m&&!p.attributeTable.fields.some((s=>s.name.toLowerCase()===m)))throw new s("raster-unique-value-renderer:invalid-parameters","A valid 'classfieldName' is required");return m||(r.classFieldName=e(p.attributeTable).name),r}(p);const{classFieldName:m,colors:n,colorRamp:l}=p,a=o(p.layer.rasterInfo,m,n,l);if(!r(a))throw new s("raster-unique-value-renderer:not-supported","UniqueValueRenderer is not supported on the provided data source");return{renderer:a,classFieldName:m}}export{p as createRenderer};
