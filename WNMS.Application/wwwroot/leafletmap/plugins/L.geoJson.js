
/*
 * @class GeoJSON
 * @aka L.GeoJSON
 * @inherits FeatureGroup
 *
 * Represents a GeoJSON object or an array of GeoJSON objects. Allows you to parse
 * GeoJSON data and display it on the map. Extends `FeatureGroup`.
 *
 * @example
 *
 * ```js
 * L.geoJSON(data, {
 * 	style: function (feature) {
 * 		return {color: feature.properties.color};
 * 	}
 * }).bindPopup(function (layer) {
 * 	return layer.feature.properties.description;
 * }).addTo(map);
 * ```
 */

var GeoJSON = L.FeatureGroup.extend({

	/* @section
	 * @aka GeoJSON options
	 *
	 * @option pointToLayer: Function = *
	 * A `Function` defining how GeoJSON points spawn Leaflet layers. It is internally
	 * called when data is added, passing the GeoJSON point feature and its `LatLng`.
	 * The default is to spawn a default `Marker`:
	 * ```js
	 * function(geoJsonPoint, latlng) {
	 * 	return L.marker(latlng);
	 * }
	 * ```
	 *
	 * @option style: Function = *
	 * A `Function` defining the `Path options` for styling GeoJSON lines and polygons,
	 * called internally when data is added.
	 * The default value is to not override any defaults:
	 * ```js
	 * function (geoJsonFeature) {
	 * 	return {}
	 * }
	 * ```
	 *
	 * @option onEachFeature: Function = *
	 * A `Function` that will be called once for each created `Feature`, after it has
	 * been created and styled. Useful for attaching events and popups to features.
	 * The default is to do nothing with the newly created layers:
	 * ```js
	 * function (feature, layer) {}
	 * ```
	 *
	 * @option filter: Function = *
	 * A `Function` that will be used to decide whether to include a feature or not.
	 * The default is to include all features:
	 * ```js
	 * function (geoJsonFeature) {
	 * 	return true;
	 * }
	 * ```
	 * Note: dynamically changing the `filter` option will have effect only on newly
	 * added data. It will _not_ re-evaluate already included features.
	 *
	 * @option coordsToLatLng: Function = *
	 * A `Function` that will be used for converting GeoJSON coordinates to `LatLng`s.
	 * The default is the `coordsToLatLng` static method.
	 */

	initialize: function (geojson, options) {
		L.setOptions(this, options);
//		alert(options);
		this._layers = {};
		this._geometryType = "";

		if (geojson) {
			this.addData(geojson);
		}
	},

	// @method addData( <GeoJSON> data ): this
	// Adds a GeoJSON object to the layer.
	addData: function (geojson) {
		var features = isArray(geojson) ? geojson : geojson.features,
		    i, len, feature;
		if(this._geometryType=="")
				this._geometryType = geojson.geometryType?geojson.geometryType:this._geometryType;

		if (features) {
			for (i = 0, len = features.length; i < len; i++) {
				// only add this if geometry or geometries are set and not null
				feature = features[i];
				if (feature.geometries || feature.geometry || feature.features || feature.coordinates) {
					this.addData(feature);
				}
			}
			return this;
		}

		var options = this.options;

		if (options.filter && !options.filter(geojson)) { return this; }
		
		options._geometryType = this._geometryType;

		var layer = geometryToLayer(geojson, options);
		if (!layer) {
			return this;
		}
//		layer.feature = asFeature(geojson);

		layer.defaultOptions = layer.options;
		this.resetStyle(layer);

		if (options.onEachFeature) {
			options.onEachFeature(geojson, layer);
		}

		return this.addLayer(layer);
	},

	// @method resetStyle( <Path> layer ): this
	// Resets the given vector layer's style to the original GeoJSON style, useful for resetting style after hover events.
	resetStyle: function (layer) {
		// reset any custom styles
		layer.options = L.extend({}, layer.defaultOptions);
		this._setLayerStyle(layer, this.options.style);
		return this;
	},

	// @method setStyle( <Function> style ): this
	// Changes styles of GeoJSON vector layers with the given style function.
	setStyle: function (style) {
		return this.eachLayer(function (layer) {
			this._setLayerStyle(layer, style);
		}, this);
	},

	_setLayerStyle: function (layer, style) {
		if (typeof style === 'function') {
			style = style(layer);
			//style = style(layer.feature);
		}
		if (layer.setStyle) {
			layer.setStyle(style);
		}
	}
});

// @section
// There are several static functions which can be called without instantiating L.GeoJSON:

// @function geometryToLayer(featureData: Object, options?: GeoJSON options): Layer
// Creates a `Layer` from a given GeoJSON feature. Can use a custom
// [`pointToLayer`](#geojson-pointtolayer) and/or [`coordsToLatLng`](#geojson-coordstolatlng)
// functions if provided as options.
function geometryToLayer(geojson, options) {

	var geometry = (geojson.type === 'Feature' || geojson.attributes) ? geojson.geometry : geojson,
			attributes = geojson.attributes ? geojson.attributes : null,
	    coords = geometry ? geometry.coordinates : null,
	    layers = [],
	    pointToLayer = options && options.pointToLayer,
	    _coordsToLatLng = options && options.coordsToLatLng || coordsToLatLng,
	    latlng, latlngs, i, len;

	if (!geometry) {
		return null;
	}
	
	var geotype = geometry.type?geometry.type:options._geometryType;
	
	options["attributes"] = attributes;

	switch (geotype) {
	//  EsriJson
	case 'esriGeometryPoint':
		latlng = geometryToLatLng(geometry, options);
		return pointToLayer ? pointToLayer(esrijson, latlng) : new L.Marker(latlng, options);
	case 'esriGeometryPolyline':
		latlngs = geometryToLatLngs(geometry.paths, 1, options);
		return new L.Polyline(latlngs, options);
	case 'esriGeometryPolygon':
		latlngs = geometryToLatLngs(geometry.rings, 1, options);
		return new L.Polygon(latlngs, options);

	//  GeoJson
	case 'Point':
		latlng = _coordsToLatLng(coords);
		return pointToLayer ? pointToLayer(geojson, latlng) : new L.Marker(latlng);

	case 'MultiPoint':
		for (i = 0, len = coords.length; i < len; i++) {
			latlng = _coordsToLatLng(coords[i]);
			layers.push(pointToLayer ? pointToLayer(geojson, latlng) : new L.Marker(latlng, options));
		}
		return new FeatureGroup(layers);

	case 'LineString':
	case 'MultiLineString':
		latlngs = coordsToLatLngs(coords, geometry.type === 'LineString' ? 0 : 1, _coordsToLatLng);
		return new L.Polyline(latlngs, options);

	case 'Polygon':
	case 'MultiPolygon':
		latlngs = coordsToLatLngs(coords, geometry.type === 'Polygon' ? 1 : 2, _coordsToLatLng);
		return new L.Polygon(latlngs, options);

	case 'GeometryCollection':
		for (i = 0, len = geometry.geometries.length; i < len; i++) {
			var layer = geometryToLayer({
				geometry: geometry.geometries[i],
				type: 'Feature',
				properties: geojson.properties
			}, options);
	
			if (layer) {
				layers.push(layer);
			}
		}
		return new L.FeatureGroup(layers);

	default:
		throw new Error('Invalid GeoJSON object.');
	}
}
function geometryToLatLng(geom, options) {
	var p = null;
	//  添加坐标系纠偏
	if(options && options.coordtransfunc){
			if(isArray(geom))
			{
				var p = options.coordtransfunc(geom[0], geom[1]);
				return new L.LatLng(p[1], p[0], geom.length>2?geom[2]:null);
			}
			else
			{
				var p = options.coordtransfunc(geom.x, geom.y);
				return new L.LatLng(p[1], p[0], geom.z);
			}
	}
	else if(options && options.offset)
	{
			if(isArray(geom))
				return new L.LatLng(geom[1]+options.offset.y, geom[0]+options.offset.x, geom.length>2?geom[2]:null);
			else
				return new L.LatLng(geom.y+options.offset.y, geom.x+options.offset.x, geom.z);
	}
	else{
		if(isArray(geom))
			return new L.LatLng(geom[1], geom[0], geom.length>2?geom[2]:null);
		else
			return new L.LatLng(geom.y, geom.x, geom.z);
}
}
function geometryToLatLngs(pnts, levelsDeep, options) {
	var latlngs = [];

	for (var i = 0, len = pnts.length, latlng; i < len; i++) {
		latlng = levelsDeep ?
			geometryToLatLngs(pnts[i], levelsDeep - 1, options) :
			geometryToLatLng(pnts[i], options);

		latlngs.push(latlng);
	}

	return latlngs;
}

// @function coordsToLatLng(coords: Array): LatLng
// Creates a `LatLng` object from an array of 2 numbers (longitude, latitude)
// or 3 numbers (longitude, latitude, altitude) used in GeoJSON for points.
function coordsToLatLng(coords) {
	return new L.LatLng(coords[1], coords[0], coords[2]);
}

// @function coordsToLatLngs(coords: Array, levelsDeep?: Number, coordsToLatLng?: Function): Array
// Creates a multidimensional array of `LatLng`s from a GeoJSON coordinates array.
// `levelsDeep` specifies the nesting level (0 is for an array of points, 1 for an array of arrays of points, etc., 0 by default).
// Can use a custom [`coordsToLatLng`](#geojson-coordstolatlng) function.
function coordsToLatLngs(coords, levelsDeep, _coordsToLatLng) {
	var latlngs = [];

	for (var i = 0, len = coords.length, latlng; i < len; i++) {
		latlng = levelsDeep ?
			coordsToLatLngs(coords[i], levelsDeep - 1, _coordsToLatLng) :
			(_coordsToLatLng || coordsToLatLng)(coords[i]);

		latlngs.push(latlng);
	}

	return latlngs;
}

// @function latLngToCoords(latlng: LatLng, precision?: Number): Array
// Reverse of [`coordsToLatLng`](#geojson-coordstolatlng)
function latLngToCoords(latlng, precision) {
	precision = typeof precision === 'number' ? precision : 6;
	return latlng.alt !== undefined ?
		[formatNum(latlng.lng, precision), formatNum(latlng.lat, precision), formatNum(latlng.alt, precision)] :
		[formatNum(latlng.lng, precision), formatNum(latlng.lat, precision)];
}

// @function latLngsToCoords(latlngs: Array, levelsDeep?: Number, closed?: Boolean): Array
// Reverse of [`coordsToLatLngs`](#geojson-coordstolatlngs)
// `closed` determines whether the first point should be appended to the end of the array to close the feature, only used when `levelsDeep` is 0. False by default.
function latLngsToCoords(latlngs, levelsDeep, closed, precision) {
	var coords = [];

	for (var i = 0, len = latlngs.length; i < len; i++) {
		coords.push(levelsDeep ?
			latLngsToCoords(latlngs[i], levelsDeep - 1, closed, precision) :
			latLngToCoords(latlngs[i], precision));
	}

	if (!levelsDeep && closed) {
		coords.push(coords[0]);
	}

	return coords;
}

function getFeature(layer, newGeometry) {
	return layer.feature ?
		extend({}, layer.feature, {geometry: newGeometry}) :
		asFeature(newGeometry);
}

// @function asFeature(geojson: Object): Object
// Normalize GeoJSON geometries/features into GeoJSON features.
function asFeature(geojson) {
	if (geojson.type === 'Feature' || geojson.type === 'FeatureCollection' || geojson.attributes) {
		return geojson;
	}

	return {
		type: 'Feature',
		properties: {},
		geometry: geojson
	};
}

var PointToGeoJSON = {
	toGeoJSON: function (precision) {
		return getFeature(this, {
			type: 'Point',
			coordinates: latLngToCoords(this.getLatLng(), precision)
		});
	}
};

// @namespace Marker
// @method toGeoJSON(): Object
// Returns a [`GeoJSON`](http://en.wikipedia.org/wiki/GeoJSON) representation of the marker (as a GeoJSON `Point` Feature).
L.Marker.include(PointToGeoJSON);

// @namespace CircleMarker
// @method toGeoJSON(): Object
// Returns a [`GeoJSON`](http://en.wikipedia.org/wiki/GeoJSON) representation of the circle marker (as a GeoJSON `Point` Feature).
L.Circle.include(PointToGeoJSON);
L.CircleMarker.include(PointToGeoJSON);


// @namespace Polyline
// @method toGeoJSON(): Object
// Returns a [`GeoJSON`](http://en.wikipedia.org/wiki/GeoJSON) representation of the polyline (as a GeoJSON `LineString` or `MultiLineString` Feature).
L.Polyline.include({
	toGeoJSON: function (precision) {
		var multi = !isFlat(this._latlngs);

		var coords = latLngsToCoords(this._latlngs, multi ? 1 : 0, false, precision);

		return getFeature(this, {
			type: (multi ? 'Multi' : '') + 'LineString',
			coordinates: coords
		});
	}
});

// @namespace Polygon
// @method toGeoJSON(): Object
// Returns a [`GeoJSON`](http://en.wikipedia.org/wiki/GeoJSON) representation of the polygon (as a GeoJSON `Polygon` or `MultiPolygon` Feature).
L.Polygon.include({
	toGeoJSON: function (precision) {
		var holes = !isFlat(this._latlngs),
		    multi = holes && !isFlat(this._latlngs[0]);

		var coords = latLngsToCoords(this._latlngs, multi ? 2 : holes ? 1 : 0, true, precision);

		if (!holes) {
			coords = [coords];
		}

		return getFeature(this, {
			type: (multi ? 'Multi' : '') + 'Polygon',
			coordinates: coords
		});
	}
});


// @namespace LayerGroup
L.LayerGroup.include({
	toMultiPoint: function (precision) {
		var coords = [];

		this.eachLayer(function (layer) {
			coords.push(layer.toGeoJSON(precision).geometry.coordinates);
		});

		return getFeature(this, {
			type: 'MultiPoint',
			coordinates: coords
		});
	},

	// @method toGeoJSON(): Object
	// Returns a [`GeoJSON`](http://en.wikipedia.org/wiki/GeoJSON) representation of the layer group (as a GeoJSON `FeatureCollection`, `GeometryCollection`, or `MultiPoint`).
	toGeoJSON: function (precision) {

		var type = this.feature && this.feature.geometry && this.feature.geometry.type;

		if (type === 'MultiPoint') {
			return this.toMultiPoint(precision);
		}

		var isGeometryCollection = type === 'GeometryCollection',
		    jsons = [];

		this.eachLayer(function (layer) {
			if (layer.toGeoJSON) {
				var json = layer.toGeoJSON(precision);
				if (isGeometryCollection) {
					jsons.push(json.geometry);
				} else {
					var feature = asFeature(json);
					// Squash nested feature collections
					if (feature.type === 'FeatureCollection') {
						jsons.push.apply(jsons, feature.features);
					} else {
						jsons.push(feature);
					}
				}
			}
		});

		if (isGeometryCollection) {
			return getFeature(this, {
				geometries: jsons,
				type: 'GeometryCollection'
			});
		}

		return {
			type: 'FeatureCollection',
			features: jsons
		};
	}
});

//@function isArray(obj): Boolean
//Compatibility polyfill for [Array.isArray](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Array/isArray)
var isArray = Array.isArray || function (obj) {
	return (Object.prototype.toString.call(obj) === '[object Array]');
};

//@function indexOf(array: Array, el: Object): Number
//Compatibility polyfill for [Array.prototype.indexOf](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Array/indexOf)
function indexOf(array, el) {
	for (var i = 0; i < array.length; i++) {
		if (array[i] === el) { return i; }
	}
	return -1;
}

// @namespace GeoJSON
// @factory L.geoJSON(geojson?: Object, options?: GeoJSON options)
// Creates a GeoJSON layer. Optionally accepts an object in
// [GeoJSON format](http://geojson.org/geojson-spec.html) to display on the map
// (you can alternatively add it later with `addData` method) and an `options` object.
function geoJSON(geojson, options) {
	return new GeoJSON(geojson, options);
}

// Backward compatibility.
var geoJson = geoJSON;


GeoJSON.geometryToLayer = geometryToLayer;
GeoJSON.coordsToLatLng = coordsToLatLng;
GeoJSON.coordsToLatLngs = coordsToLatLngs;
GeoJSON.latLngToCoords = latLngToCoords;
GeoJSON.latLngsToCoords = latLngsToCoords;
GeoJSON.getFeature = getFeature;
GeoJSON.asFeature = asFeature;
GeoJSON.geometryToLatLng = geometryToLatLng;
GeoJSON.geometryToLatLngs = geometryToLatLngs;

L.GeoJSON = GeoJSON;
L.geoJSON = geoJSON;
L.geoJson = geoJson;


function getOffset(mkey)
{
	//return mkey=="dj"?{x:0.008, y:-0.0005}:(mkey=="sz"?{x:0.0055, y:-0.00017}:{x:0.0057, y:-0.003});
	//return {x:0.0, y:-0.0};
	return mkey=="dj"?{x:0.008, y:-0.0005}:(mkey=="sz"?{x:0.0045, y:-0.00017}:{x:0.0057, y:-0.003});
}

function getOffLntlng(mkey, lntlng)
{
	var offset = getOffset(mkey);
	return [lntlng[0]+offset.y, lntlng[1]+offset.x];
}

/**
 * 获取从蓝到绿到红的渐变色
 * **/
function getSymColor(index, total)
{
	if(index<20)
		index = index;
	var dc = 256.0*4 / total;
	var dc4 = Math.floor(total/4);
	var r = 0;
	var g = 0;
	var b = 0;
	if(index<total/4)
	{
		b = 255;
		g = Math.floor(index*dc);
		if(b<0)	b = 0;
		if(g<0)	g = 0;
	}
	else 	if(index>=total/4 & index<total/2)
	{
		var cc = index % dc4;
		b = Math.floor(255 - cc*dc);
		g = 255;
		if(b<0)	b = 0;
		if(g<0)	g = 0;
	}
	else	if(index>=3*total/4)
	{
		var cc = index % dc4;
		r = 255;
		g = Math.floor(255 - cc*dc);
		if(r<0)	b = 0;
		if(g<0)	g = 0;
	}
	else {
		var cc = index % dc4;
		
		g = 255;
		r = Math.floor(cc*dc);
		if(r<0)	r = 0;
		if(g<0)	g = 0;
	}
	//if(r<10 && g < 10 & b< 10)
	return rgbColor(r,g,b);
}


function rgbColor(r,g,b) {
	return "#"+toHex(r)+toHex(g)+toHex(b);
}

function ShowLegend(tmap, total, vmin, vmax, itemname, unit)
{
	if(tmap.options["legend"])
		tmap.options["legend"].remove();
	var legend = L.control({position: 'topright', backgroundColor: '#ffffff'});
	legend.onAdd = function (map) {
	var div = L.DomUtil.create('div', 'legend');
	div.style.backgroundColor = "rgba(255,255,255,0.7)";
	div.style.padding = "5px";
	//div.style.opacity = "0.5";
	div.innerHTML += "<div class=legendtitle>"+itemname+"<br>"+unit+"</div>"
	var d5 = Math.round(total/5);
    for (var i = total-1; i >= 0; i--) {
    	var v = Math.round(1000*(vmin + (i>0?(i+1):0)*(vmax-vmin)/total))/1000;
    	div.innerHTML +=
//            '<i title="'+v+'" style="background:' + getSymColor(i,total) + ';height:'+(140/total)+'px"></i> ' + ( (i==0||i==total-1||i%d5==0)?(""+ v):"" ) + (i >0 ? '<br>' : '');
        '<i title="'+v+'" style="background:' + getSymColor(i,total) + '"></i>' + ( (i==0||i==total-1||i%d5==0)?("&nbsp;"+ v):" &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;           " ) + (i >0 ? '<br>' : '<br>');
    }
    return div;
	};
	legend.addTo(tmap);
	tmap.options["legend"] = legend;
}