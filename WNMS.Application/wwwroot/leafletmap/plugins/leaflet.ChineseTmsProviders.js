L.TileLayer.ChinaProvider = L.TileLayer.extend({

    initialize: function(type, options) { // (type, Object)
        var providers = L.TileLayer.ChinaProvider.providers;

        var parts = type.split('.');

        var providerName = parts[0];
        var mapName = parts[1];
        var mapType = parts[2];

        var url = providers[providerName][mapName][mapType];
        options.subdomains = providers[providerName].Subdomains;

        L.TileLayer.prototype.initialize.call(this, url, options);
    }
});
//http://t{s}.tianditu.gov.cn/img_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5
L.TileLayer.ChinaProvider.providers = {
    TianDiTuNew2: {
        Normal: {
            Map: "http://t{s}.tianditu.gov.cn/vec_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/cva_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Satellite: {
            Map: "http://t{s}.tianditu.gov.cn/img_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/cia_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Terrain: {
            Map: "http://t{s}.tianditu.gov.cn/ter_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/cta_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Subdomains: ['0']//, '1', '2', '3', '4', '5', '6', '7'
    },
    
    TianDiTuNew: {
        Normal: {
            Map: "http://t{s}.tianditu.gov.cn/vec_w/wmts?Service=WMTS&Request=GetTile&Version=1.0.0&layer=img&Style=Default&serviceMode=KVP&TileMatrixSet=w&Format=tiles&TileMatrix={z}&TileRow={x}&TileCol={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/cva_w/wmts?Service=WMTS&Request=GetTile&Version=1.0.0&layer=img&Style=Default&serviceMode=KVP&TileMatrixSet=w&Format=tiles&TileMatrix={z}&TileRow={x}&TileCol={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Satellite: {
            Map: "http://t{s}.tianditu.gov.cn/img_w/wmts?Service=WMTS&Request=GetTile&Version=1.0.0&layer=img&Style=Default&serviceMode=KVP&TileMatrixSet=w&Format=tiles&TileMatrix={z}&TileRow={x}&TileCol={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/cia_w/wmts?Service=WMTS&Request=GetTile&Version=1.0.0&layer=img&Style=Default&serviceMode=KVP&TileMatrixSet=w&Format=tiles&TileMatrix={z}&TileRow={x}&TileCol={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Terrain: {
            Map: "http://t{s}.tianditu.gov.cn/ter_w/wmts?Service=WMTS&Request=GetTile&Version=1.0.0&layer=img&Style=Default&serviceMode=KVP&TileMatrixSet=w&Format=tiles&TileMatrix={z}&TileRow={x}&TileCol={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/cta_w/wmts?Service=WMTS&Request=GetTile&Version=1.0.0&layer=img&Style=Default&serviceMode=KVP&TileMatrixSet=w&Format=tiles&TileMatrix={z}&TileRow={x}&TileCol={y}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Subdomains: ['0']//, '1', '2', '3', '4', '5', '6', '7'
    },
    
    TianDiTu: {
        Normal: {
            Map: "http://t{s}.tianditu.gov.cn/DataServer?T=vec_w&X={x}&Y={y}&L={z}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/DataServer?T=cva_w&X={x}&Y={y}&L={z}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Satellite: {
            Map: "http://t{s}.tianditu.gov.cn/DataServer?T=img_w&X={x}&Y={y}&L={z}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/DataServer?T=cia_w&X={x}&Y={y}&L={z}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Terrain: {
            Map: "http://t{s}.tianditu.gov.cn/DataServer?T=ter_w&X={x}&Y={y}&L={z}&tk=99d26ddc8c9b0e2c2e2d7150207269c5",
            Annotion: "http://t{s}.tianditu.gov.cn/DataServer?T=cta_w&X={x}&Y={y}&L={z}&tk=99d26ddc8c9b0e2c2e2d7150207269c5"
        },
        Subdomains: ['0', '1', '2', '3', '4', '5', '6', '7']
    },


    BdMap: {
        Normal: {
//            Map: 'http://online{s}.map.bdimg.com/tile/?qt=tile&x={x}&y={y}&z={z}&styles=pl&udt=20150518'
            Map: 'http://shangetu{s}.map.bdimg.com/it/u=x={x};y={y};z={z};v=017;type=web&fm=44'
        },
        Traffic: {
            Map: 'http://its.map.baidu.com:8002/traffic/TrafficTileService?x={x}&y={y}&level={z}&time=' + (new Date().getTime()) + '&label=web2D&v=017'
        },
        Satellite: {
            Map: 'http://shangetu{s}.map.bdimg.com/it/u=x={x};y={y};z={z};v=009;type=sate&fm=46',
            Annotion: 'http://online{s}.map.bdimg.com/tile/?qt=tile&x={x}&y={y}&z={z}&styles=sh&v=020'
        },
        Subdomains: ["0", "1", "2", "3", "4"]
    },


    GaoDe: {
        Normal: {
            Map: 'http://webrd0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={x}&y={y}&z={z}'
        },
        Satellite: {
            Map: 'http://webst0{s}.is.autonavi.com/appmaptile?style=6&x={x}&y={y}&z={z}',
            Annotion: 'http://webst0{s}.is.autonavi.com/appmaptile?style=8&x={x}&y={y}&z={z}'
        },
        Subdomains: ["1", "2", "3", "4"]
    },

    Google: {
        Normal: {
            Map: "http://www.google.cn/maps/vt?lyrs=m@189&gl=cn&x={x}&y={y}&z={z}"
        },
        Satellite: {
            Map: "http://www.google.cn/maps/vt?lyrs=s@189&gl=cn&x={x}&y={y}&z={z}"
        },
        Subdomains: []
    },

    Geoq: {
        Normal: {
            Map: "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineCommunity/MapServer/tile/{z}/{y}/{x}",
            Color: "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetColor/MapServer/tile/{z}/{y}/{x}",
            PurplishBlue: "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetPurplishBlue/MapServer/tile/{z}/{y}/{x}",
            Gray: "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetGray/MapServer/tile/{z}/{y}/{x}",
            Warm: "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetWarm/MapServer/tile/{z}/{y}/{x}",
            Cold: "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetCold/MapServer/tile/{z}/{y}/{x}"
        },
        Subdomains: []

    }
};

L.tileLayer.chinaProvider = function(type, options) {
    return new L.TileLayer.ChinaProvider(type, options);
};
