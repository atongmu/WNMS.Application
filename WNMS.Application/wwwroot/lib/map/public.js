if (typeof(console) == "undefined") {
    console = {};
    console.log = function() {}
}
window.onerror = function() {}; (function() {
    var i = window.DomReady = {};
    var h = navigator.userAgent.toLowerCase();
    var c = {
        version: (h.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1],
        safari: /webkit/.test(h),
        opera: /opera/.test(h),
        msie: (/msie/.test(h)) && (!/opera/.test(h)),
        mozilla: (/mozilla/.test(h)) && (!/(compatible|webkit)/.test(h))
    };
    var d = false;
    var e = false;
    var g = [];
    function a() {
        if (!e) {
            e = true;
            if (g) {
                for (var j = 0; j < g.length; j++) {
                    g[j].call(window, [])
                }
                g = []
            }
        }
    }
    function f(j) {
        var k = window.onload;
        if (typeof window.onload != "function") {
            window.onload = j
        } else {
            window.onload = function() {
                if (k) {
                    k()
                }
                j()
            }
        }
    }
    function b() {
        if (d) {
            return
        }
        d = true;
        if (document.addEventListener && !c.opera) {
            document.addEventListener("DOMContentLoaded", a, false)
        }
        if (c.msie && window == top) { (function() {
                if (e) {
                    return
                }
                try {
                    document.documentElement.doScroll("left")
                } catch(k) {
                    setTimeout(arguments.callee, 0);
                    return
                }
                a()
            })()
        }
        if (c.opera) {
            document.addEventListener("DOMContentLoaded",
            function() {
                if (e) {
                    return
                }
                for (var k = 0; k < document.styleSheets.length; k++) {
                    if (document.styleSheets[k].disabled) {
                        setTimeout(arguments.callee, 0);
                        return
                    }
                }
                a()
            },
            false)
        }
        if (c.safari) {
            var j; (function() {
                if (e) {
                    return
                }
                if (document.readyState != "loaded" && document.readyState != "complete") {
                    setTimeout(arguments.callee, 0);
                    return
                }
                if (j === undefined) {
                    var l = document.getElementsByTagName("link");
                    for (var m = 0; m < l.length; m++) {
                        if (l[m].getAttribute("rel") == "stylesheet") {
                            j++
                        }
                    }
                    var k = document.getElementsByTagName("style");
                    j += k.length
                }
                if (document.styleSheets.length != j) {
                    setTimeout(arguments.callee, 0);
                    return
                }
                a()
            })()
        }
        f(a)
    }
    i.ready = function(k, j) {
        b();
        if (e) {
            k.call(window, [])
        } else {
            g.push(function() {
                return k.call(window, [])
            })
        }
    };
    b()
})();
var Fe = Fe || {
    version: "20080809",
    emptyFn: function() {}
};
Fe._log = [];
var counter = 0;
var instances = {};
Fe.BaseClass = function(a) {
    instances[(this.hashCode = (a || Fe.BaseClass.guid()))] = this
};
Fe.BaseClass.guid = function() {
    return "mz_" + (counter++).toString(36)
};
Fe.BaseClass.create = function() {
    var a = new Fe.BaseClass();
    a.decontrol();
    return a
};
window.Instance = Fe.instance = Fe.I = function(a) {
    return instances[a]
};
Fe.BaseClass.prototype.dispose = function() {
    if (this.hashCode) {
        delete instances[this.hashCode]
    }
    for (var a in this) {
        if (typeof this[a] != "function") {
            delete this[a]
        }
    }
};
Fe.BaseClass.prototype.getHashCode = function() {
    if (!this.hashCode) {
        instances[(this.hashCode = Fe.BaseClass.guid())] = this
    }
    return this.hashCode
};
Fe.BaseClass.prototype.decontrol = function() {
    delete instances[this.hashCode]
};
Fe.BaseClass.prototype.toString = function() {
    return "[object " + (this._className || "Object") + "]"
};
Fe.BaseClass.prototype._wlog = function(c, d) {
    var b = Fe._log;
    if (b.length > 100) {
        b.reverse().length = 50;
        b.reverse()
    }
    b[b.length] = "[" + c + "][" + (this._className || "Object") + " " + this.hashCode + "] " + d
};
Fe.extend = function(d, b) {
    if (d && b && typeof(b) == "object") {
        for (var c in b) {
            d[c] = b[c]
        }
        var a = ["constructor", "hasOwnProperty", "isPrototypeOf", "propertyIsEnumerable", "toLocaleString", "toString", "valueOf"];
        for (var e = 0,
        f; e < a.length; e++) {
            f = a[e];
            if (Object.prototype.hasOwnProperty.call(b, f)) {
                d[f] = b[f]
            }
        }
    }
    return d
};
Fe.on = function(a, c, b) {
    if (! (a = Fe.G(a))) {
        return a
    }
    c = c.replace(/^on/, "").toLowerCase();
    if (a.attachEvent) {
        a[c + b] = function() {
            b.call(a, window.event)
        };
        a.attachEvent("on" + c, a[c + b])
    } else {
        a.addEventListener(c, b, false)
    }
    return a
};
Fe.un = function(a, c, b) {
    if (! (a = Fe.G(a))) {
        return a
    }
    c = c.replace(/^on/, "").toLowerCase();
    if (a.attachEvent) {
        a.detachEvent("on" + c, a[c + b]);
        a[c + b] = null
    } else {
        a.removeEventListener(c, b, false)
    }
    return a
};
Fe.G = function() {
    for (var b = [], c = arguments.length - 1; c > -1; c--) {
        var d = arguments[c];
        b[c] = null;
        if (typeof d == "object" && d && d.dom) {
            b[c] = d.dom
        } else {
            if ((typeof d == "object" && d && d.tagName) || d == window || d == document) {
                b[c] = d
            } else {
                if (typeof d == "string" && (d = document.getElementById(d))) {
                    b[c] = d
                }
            }
        }
    }
    return b.length < 2 ? b[0] : b
};
Fe.copy = function(a) {};
function beforeEndHTML(b, a) {
    b.insertAdjacentHTML("beforeEnd", a);
    return b.lastChild
}
function getClientSize() {
    if (window.innerHeight) {
        return {
            width: window.innerWidth,
            height: window.innerHeight
        }
    } else {
        if (document.documentElement && document.documentElement.clientHeight) {
            return {
                width: document.documentElement.clientWidth,
                height: document.documentElement.clientHeight
            }
        } else {
            return {
                width: document.body.clientWidth,
                height: document.body.clientHeight
            }
        }
    }
}
function foucs_(a, b, c) {
    if (!c) {
        c = ""
    }
    a.style.color = "#000"; (a.value != b) ? null: a.value = c
}
function blur_(a, b, c) {
    a.style.color = "#8c8c8c"; (a.value == c || a.value.length <= 0) ? a.value = b: null
}
function loadBody() {
    createCopyBt()
    initMapSize();
    initMap();
    addMapControls();
    setTimeout(function() {
        mapResize()
    },
    200);
}
function initMapSize() {
    var a = Fe.G("MapHolder");
    var b = getClientSize().height - 140;
    b = b < 0 ? 0 : b;
    a.style.height = b + "px";
    var e = Fe.G("MapInfo");
    var d = b + 2;
    d = d < 0 ? 0 : d;
    e.style.height = d + "px";
    var c = Fe.G("shad_v");
    var f = b;
    f = f < 0 ? 0 : f;
    c.style.height = f + "px"
}
function initMap() {
    window.map = new BMap.Map("MapHolder", {enableMapClick: false});
    window.projection = new BMap.MercatorProjection();
    var a = new BMap.Point(116.395645, 39.929986);
    map.addEventListener("load",
    function(b) {
        getCurrentCityName()
    });
    map.addEventListener("moveend",
    function(b) {
        getCurrentCityName()
    });
    map.addEventListener("dragend",
    function(b) {
        getCurrentCityName()
    });
    map.addEventListener("zoomend",
    function(b) {
        getCurrentCityName()
    });
    map.addEventListener("click",
    function(c) {
        var b = c.point;
        if (c.overlay && c.overlay instanceof BMap.Marker) {
            b = c.overlay.point
        }
        setInputPoint(b)
    });
    map.addEventListener("mousemove",
    function(c) {
        if (!temp.mouseLabel) {
            createMouseMoveLabel(c.point)
        }
        if (!temp.mouseLabel.isVisible()) {
            temp.mouseLabel.show()
        }
        var k = temp.mouseLabel;
        var h = map.getContainer();
        var g = h.clientWidth;
        var f = h.clientHeight;
        var j = 132;
        var i = 19;
        var n = map.pointToPixel(c.point).x + 13;
        var l = map.pointToPixel(c.point).y + 20;
        var m = map.pixelToPoint(new BMap.Pixel(g - j - 13, f - i - 20));
        var b = map.pixelToPoint(new BMap.Pixel(n - j - 33, f - i - 20));
        var d = c.point;
        if (g - n < j) {
            d = new BMap.Point(m.lng, d.lat)
        }
        if (f - l < i) {
            d = new BMap.Point(d.lng, m.lat)
        }
        if (g - n - 16 < j && f - l < i) {
            d = b
        }
        k.setPosition(d);
        k.setContent(c.point.lng + "," + c.point.lat)
    });
    Fe.on(document.body, "mousemove",
    function(c) {
        var c = window.event || c;
        var b = c.srcElement || c.target;
        if (b.className != "BMap_mask" && temp.mouseLabel && temp.mouseLabel.isVisible()) {
            temp.mouseLabel.hide()
        }
    });
    Fe.on(document.body, "mouseout",
    function(c) {
        var c = window.event || c;
        var b = c.srcElement || c.target;
        if (b.className == "BMap_mask" && temp.mouseLabel && temp.mouseLabel.isVisible()) {
            temp.mouseLabel.hide()
        }
    });
    mapInfo.centerPoint = a;
    map.centerAndZoom(a, 12);
    map.enableScrollWheelZoom();
    map.setDefaultCursor("default");
    map.setDraggingCursor("default")
}
function setInputPoint(a) {
    var value = a.lng + "," + a.lat;
    Fe.G("pointInput").value = value;
    Fe.G('pointInput').setAttribute('data-clipboard-text', value); 
    $(parent.$('iframe')[0].contentDocument).find('#Lng').val(a.lng);
    $(parent.$('iframe')[0].contentDocument).find('#Lat').val(a.lat);
    $(parent.$('iframe')[1].contentDocument).find('#Lng').val();
    $(parent.$('iframe')[1].contentDocument).find('#Lat').val(a.lat); 
    //填写泵房省市区

    var point = new BMap.Point(a.lng, a.lat);
    var mk = new BMap.Marker(point);
    map.clearOverlays();
    map.addOverlay(mk);

    var geoc = new BMap.Geocoder();
    geoc.getLocation(point, function (rs) { 
        var addComp = rs.addressComponents;
        parent.$('iframe')[0].contentWindow.bindAreaAll(addComp.province,addComp.city,addComp.district);
        //$(parent.$('iframe')[0].contentDocument).find('#StationPosition').val(rs.address); 

        //parent.$('iframe')[2].contentWindow.bindAreaAll(addComp.province, addComp.city, addComp.district);
        //$(parent.$('iframe')[2].contentDocument).find('#StationPosition').val(rs.address); 
    }); 
    //parent.$('#Lng').val(a.lng);
    //parent.$('#Lat').val(a.lat);
   // aas();
    //var aa = parent.$('iframe');
    //var bb = aa[0];
    //var cc = bb.$("Lng").value;
     
    //console.log(cc);
   // parent.$('iframe')[0].$('Lng').val("aaaaaa");

    //("#Lng", parent.frames[0].document).val(a.lng);
    //("#Lat", parent.frames[1].document).val(a.lat);
    //parent.$('#Lng').val(a.lng);
    //parent.$('#Lat').val(a.lat);
}
function createMouseMoveLabel(c) {
    var a = map.pixelToPoint(new BMap.Pixel(0, 0));
    var b = c.lng + "," + c.lat;
    var d = new BMap.Label(b, {
        point: a,
        offset: new BMap.Size(13, 20),
        enableMassClear: false
    });
    d.setStyle({
        background: "#fff",
        border: "#999 solid 1px",
        zIndex: 10000000
    });
    map.addOverlay(d);
    temp.mouseLabel = d
}
function addMapControls() {
    window.stdMapCtrl = new BMap.NavigationControl();
    map.addControl(window.stdMapCtrl);
    window.scaleCtrl = new BMap.ScaleControl();
    map.addControl(window.scaleCtrl);
    window.overviewCtrl = new BMap.OverviewMapControl();
    map.addControl(window.overviewCtrl);
    var a = new BMap.CopyrightControl();
    map.addControl(a)
}
function mapResize() {
    var a = Fe.G("MapHolder");
    var b = Fe.G("shad_v");
    if (window._resizeTimer) {
        return
    }
    window._resizeTimer = setTimeout(function() {
        var c = getClientSize().height - 140;
        var d = c - 20;
        c = c < 0 ? 0 : c;
        d = d < 0 ? 0 : d;
        a.style.height = c + "px";
        Fe.G("MapInfo").style.height = d + "px";
        // if (overviewCtrl.getDom() == null) {
        //     var e = parseInt(a.style.height)
        // } else {
        //     var e = parseInt(a.style.height) - parseInt(overviewCtrl.getDom().style.height)
        // }
        // e = e < 0 ? 0 : e;
        // b.style.height = e + "px";
        window._resizeTimer = null
    },
    100)
}
var mapInfo = {
    cityName: "",
    cityCode: "",
    centerPoint: null
};
var temp = {
    pt: [],
    mk: [],
    iw: [],
    iwOpenIndex: null,
    mouseLabel: null,
    poiSearchMark: null,
    geoCoder: null
};
var eventTemp = {};
function clearLastResult() {
    Fe.G("txtPanel").innerHTML = "";
    temp.pt = [];
    temp.mk = [];
    temp.iw = [];
    temp.iwOpenInde = null;
    map.clearOverlays();
    if (temp.poiSearchMark) {
        temp.poiSearchMark.hide()
    }
}
function localsearch() {
    var a = [];
    var c = Fe.G("localvalue").value;
    var d = {
        onSearchComplete: function(w) {
            clearLastResult();
            if (b.getStatus() == BMAP_STATUS_SUCCESS) {
                var l = w.getCurrentNumPois();
                var k = w.getCurrentNumPois();
                var e = w.getNumPois();
                var p = "";

                if (l == 1 && typeof w.getPoi(0).city == "undefined") {
                    var f = 4;
                    // if (b && b._json && b._json.content && b._json.content.length == 2) {
                    if (b && w && w.getNumPois() == 2) {
                        f = w.getPoi(1);
                        p = '<b style="font-size:14px;">' + w.keyword + "</b>"
                    } else {
                        if (b && w && w.city) {
                            // f = b._json.current_city.level;
                            // if (b._json.current_city.name == "全国") {
                            //     f = 4
                            // }
                            p = '<span style="color:#00c">已切换至' + w.city + "</span>";
                            Fe.G("resultNum").innerHTML = ""
                        }
                    }
                    map.centerAndZoom(w.getPoi(0).point, 12);
                    Fe.G("txtPanel").innerHTML = p
                } else {
                    // var o = _res_sta_i = b._json.content.length - k;
                    p = '<ul class="local_s">';
                    for (var z = 0; z < l; z++) {
                        var h = w.getPoi(z);
                        var q = h.title;
                        var u = h.address;
                        var n = h.phoneNumber;
                        var v = h.point;
                        var A = v.lng + "," + v.lat;
                        var x = q;
                        var j = h.type;
                        var t = "地址";
                        if (x.length > 20) {
                            x = x.substring(0, 17) + "..."
                        }
                        if (j == 1) {
                            t = "途径公交车"
                        }
                        if (j == 3) {
                            t = "途径地铁"
                        }
                        p += '<li id="no' + z + '">';
                        p += '<span id="mk_' + z + '"></span>';
                        p += '  <div id="no_' + z + '">';
                        p += '      <a href="javascript:void(0)" title="' + q + '">' + x + "</a>";
                        p += "      <p>" + t + "：" + u;
                        n ? p += "      <br/>电话：" + n: null;
                        p += "      <br/>坐标：" + A;
                        p += "      </p>";
                        p += "  </div>";
                        p += "</li>";
                        a.push(v);
                        temp.pt.push(v);
                        addMarker(z);
                        createIw({
                            tit: q,
                            add: u,
                            tel: n,
                            poi: v,
                            type: j
                        })
                    }
                    p += "</ul>";
                    p += '<div id="result_page_c"></div>';
                    map.setViewport(a);
                    a = [];
                    Fe.G("txtPanel").innerHTML = p;
                    Fe.G("resultNum").innerHTML = "共找到" + e + "条结果";
                    var m = new Page("result_page_c",
                    function(i) {
                        Fe.G("MapInfo").scrollTop = 0;
                        b.gotoPage(i - 1)
                    },
                    {
                        page: w.getPageIndex() + 1,
                        totalCount: e,
                        pageCount: Math.ceil(e / 10)
                    });
                    if (m.pageCount <= 1) {
                        Fe.G("result_page_c").innerHTML = ""
                    }
                    bindEvent()
                }
            } else {
                var y = "";
                Fe.G("resultNum").innerHTML = "";
                if (w.city != "全国") {
                    y = "在<b>" + w.city + "</b>及全国没有找到相关的地点。"
                } else {
                    y = "在全国没有找到相关的地点。"
                }
                if (w.getCityList().length > 0) {
                    if (w.city != "全国") {
                        y = "在<b>" + w.city + "</b>没有找到相关的地点。"
                    } else {
                        y = "在以下城市有结果，请您选择：<br />"
                    }
                    var g = w.getCityList();
                    if (g.length > 0) {
                        y += '<p style="margin-top:10px;">在以下城市找到结果，请选择城市：</p><ul class="SearchList" id="cityList" style="height:40px;overflow:hidden;">';
                        for (var z = 0; z < g.length; z++) {
                            y += "<li><a onclick=\"searchInthisCity('" + g[z].city + '\')" href="javascript:void(0)">' + g[z].city + "</a>(";
                            y += g[z].numResults + ")</li>"
                        }
                        y += "</ul>";
                        if (g.length > 6) {
                            y += '<div id="moreCityPop"><a onclick="showMoreCity()" class="resultMore">更多城市</a></div>'
                        }
                    }
                }
                Fe.G("txtPanel").innerHTML = y
            }
        }
    };
    var b = new BMap.LocalSearch(map, d);
    window.l_local = b;
    Fe.G("localsearch").onclick = function() {
        beginsearch(b)
    }
}
function setCurrentMapInfo(c, d) {
    var a = mapInfo;
    var b = map.getZoom();
    a.cityName = c;
    a.cityCode = d;
    a.centerPoint = map.getCenter();
    Fe.G("curCity").innerHTML = c;
    Fe.G("ZoomNum").innerHTML = b
}
function getCurrentCityName() {
    var a = map.getZoom();
    var c;
    var h = 10000;
    if (a <= 7) {
        c = a;
        setCurrentMapInfo("全国");
        return
    }
    var i = function() {
        var m = map.getBounds();
        var j = projection.lngLatToPoint(m.getSouthWest());
        var l = projection.lngLatToPoint(m.getNorthEast());
        var k = function(n) {
            return parseInt(n / 1000) * 1000
        };
        return k(j.x) + "," + k(j.y) + ";" + k(l.x) + "," + k(l.y)
    };
    var e = mapInfo.centerPoint;
    var d = map.getCenter();
    var f = Math.sqrt((e.lng - d.lng) * (e.lng - d.lng) + (e.lat - d.lat) * (e.lat - d.lat));
    if (f > h || a != c) {
        c = a;
        var b = "https://map.baidu.com/?newmap=1&qt=cen&b=" + i() + "&l=" + a;
        scriptRequest(b, g, "_MAP_CENTER_", "gbk")
    }
    function g() {
        if (typeof _mapCenter == "undefined") {
            return
        }
        var j = _mapCenter;
        var k = j.content;
        if (!k) {
            return
        }
        setCurrentMapInfo(_mapCenter.content.name, _mapCenter.content.uid)
    }
}
function bindEvent() {
    var d = temp.mk;
    var a = temp.iw;
    var b = function(g) {
        if (temp.iwOpenIndex == g) {
            return
        }
        temp.iwOpenIndex = g;
        d[g].openInfoWindow(a[g]);
        setInputPoint(d[g].point)
    };
    var c = function(g) {
        if (temp.iwOpenIndex == g) {
            return
        }
        var i = d[g];
        var h = i.getIcon();
        h.setImageOffset(new BMap.Size(0, -250 - g * 25));
        i.setIcon(h);
        i.setTop(true, 1000100);
        Fe.G("no_" + g) ? Fe.G("no_" + g).className = "hover": null
    };
    var e = function(g) {
        var i = d[g];
        if (temp.iwOpenIndex == g) {
            i.setTop(true);
            return
        }
        var h = i.getIcon();
        h.setImageOffset(new BMap.Size(0, -g * 25));
        i.setIcon(h);
        i.setTop(false);
        Fe.G("no_" + g) ? Fe.G("no_" + g).className = "": null
    };
    for (var f = 0; f < d.length; f++) { (function() {
            var h = f;
            var i = d[h];
            var g = a[h];
            d[h].addEventListener("click",
            function() {
                b(h)
            });
            d[h].addEventListener("mouseover",
            function() {
                c(h)
            });
            d[h].addEventListener("mouseout",
            function() {
                e(h)
            });
            g.addEventListener("open",
            function() {
                temp.iwOpenIndex = h
            });
            g.addEventListener("close",
            function() {
                temp.iwOpenIndex = null;
                e(h)
            });
            Fe.on(Fe.G("no_" + h), "click",
            function() {
                b(h)
            });
            Fe.on(Fe.G("no_" + h), "mouseover",
            function() {
                c(h)
            });
            Fe.on(Fe.G("no_" + h), "mouseout",
            function() {
                e(h)
            });
            Fe.on(Fe.G("mk_" + h), "click",
            function() {
                b(h)
            });
            Fe.on(Fe.G("mk_" + h), "mouseover",
            function() {
                c(h)
            });
            Fe.on(Fe.G("mk_" + h), "mouseout",
            function() {
                e(h)
            })
        })()
    }
}
function addMarker(a) {
    var b = temp.pt[a];
    var d = new BMap.Icon("../../Content/newself/map/markers.png", new BMap.Size(23, 25), {
        offset: new BMap.Size(10, 25),
        imageOffset: new BMap.Size(0, 0 - a * 25),
        infoWindowAnchor: new BMap.Size(12, 0)
    });
    var c = new BMap.Marker(b, {
        icon: d
    });
    map.addOverlay(c);
    temp.mk.push(c);
    return c
}
function createIw(a) {
    var e = a.tit;
    var h = a.add;
    var d = a.tel;
    var i = a.poi.lng + "," + a.poi.lat;
    var j = '<p class="iwContent">';
    var f = a.type;
    var c = "地址";
    if (f == 1) {
        c = "途径公交车"
    }
    if (f == 3) {
        c = "途径地铁"
    }
    j += "<em>" + c + "：</em>" + h + "<br/>";
    d ? j += "<em>电话：</em>" + d + "<br/>": null;
    j += "<em>坐标：</em>" + i + "";
    j += "</p>";
    var g = e;
    if (g.length > 15) {
        g = g.substring(0, 12) + "..."
    }
    var b = new BMap.InfoWindow(j, {
        title: '<span class="iwTitle" title="' + e + '">' + g + "</span>",
        width: 250
    });
    temp.iw.push(b);
    return b
}
function showMoreCity() {
    Fe.G("cityList").style.height = "auto";
    Fe.G("moreCityPop").style.display = "none"
}
function goCity(c) {
    var b = c.tagName.toLowerCase() == "input" ? "value": "innerHTML";
    var a = c[b];
    getCityPoint(a)
}
function getCityPoint(b) {
    b = encodeURIComponent(b);
    var a = "https://map.baidu.com/?newmap=1&qt=cur&callback=setCurrentCity&ie=utf-8&wd=" + b + "&oue=1&res=jc";
    scriptRequest(a, "null")
}
function setCurrentCity(d) {
    if (!d.content || d.content.error == 0) {
        setTimeout(function() {
            Fe.G("selCityMessage").style.display = "block";
            Fe.G("selCityMessage").innerHTML = "请输入正确的中文城市名称"
        },
        0)
    } else {
        hidePop();
        var b = (((d.content.geo).split("|")[2]).split(";")[0]).split(",")[0];
        var a = (((d.content.geo).split("|")[2]).split(";")[0]).split(",")[1];
        var c = projection.pointToLngLat(new BMap.Pixel(b, a));
        if (d.content.cname == "全国") {
            map.centerAndZoom(new BMap.Point(c.lng, c.lat), 5)
        } else {
            map.centerAndZoom(new BMap.Point(c.lng, c.lat), d.content.level)
        }
        clearLastResult();
        Fe.G("resultNum").innerHTML = "";
        Fe.G("txtPanel").innerHTML = '<span style="color:#00c">已切换至' + d.content.cname + "</span>"
    }
}
function hidePop() {
    if (Fe.G("selCityInput")) {
        Fe.G("selCityInput").value = "请输入城市名";
        Fe.G("selCityMessage").style.display = "none";
        Fe.G("map_popup").style.display = "none"
    }
    if (eventTemp.cityPop.length > 0) {
        var a = eventTemp.cityPop;
        for (var b = 0; b < a.length; b++) {
            Fe.un(a[b].dom, a[b].type, a[b].fun)
        }
        eventTemp.cityPop = []
    }
}
function showPop() {
    if (Fe.G("map_popup").style.display == "block") {
        return
    }
    Fe.G("map_popup").style.display = "block";
    var a = function(c) {
        var b = c.srcElement || c.target;
        while (b) {
            if (b == Fe.G("map_popup") || b == Fe.G("curCityText")) {
                return
            }
            if (b == Fe.G("selCity")) {
                Fe.G("selCityMessage").style.display = "none";
                return
            }
            if (b == document.body) {
                hidePop();
                return
            }
            b = b.parentNode
        }
    };
    Fe.on(document.body, "mousedown", a);
    eventTemp.cityPop = [];
    eventTemp.cityPop.push({
        dom: document.body,
        type: "mousedown",
        fun: a
    })
}
var TimerSM;
function showMessage(b) {
    var a = Fe.G("searchTip");
    if (TimerSM) {
        clearTimeout(TimerSM)
    }
    a.innerHTML = b;
    TimerSM = setTimeout(function() {
        a.innerHTML = ""
    },
    1000)
}
function createCopyBt() {
    // M.fe.copy({
    //     copyBtnId: "copyPoint",
    //     copytextId: "pointInput",
    //     copyTag: "value",
        // callback: function(d) {
        //     var c = Fe.G("copyMessage");
        //     c.style.display = "inline-block";
        //     setTimeout(function() {
        //         c.style.display = "none"
        //     },
        //     1000)
        // }
    // });
    // var a = setInterval(function() {
    //     if (Fe.G("ZeroClipboardMovie_1")) {
    //         var c = getAbsPoint(Fe.G("copyButton"));
    //         if (baidu.browser.ie == 8) {
    //             baidu.dom.getParent(Fe.G("ZeroClipboardMovie_1")).style.top = (c.y + 25) + "px";
    //             baidu.dom.getParent(Fe.G("ZeroClipboardMovie_1")).style.left = (c.x + 170) + "px"
    //         } else {
    //             if (baidu.browser.ie < 8) {
    //                 baidu.dom.getParent(Fe.G("ZeroClipboardMovie_1")).style.top = (c.y - 10) + "px";
    //                 baidu.dom.getParent(Fe.G("ZeroClipboardMovie_1")).style.left = (c.x + 10) + "px"
    //             } else {
    //                 baidu.dom.getParent(Fe.G("ZeroClipboardMovie_1")).style.top = (c.y + 6) + "px";
    //                 baidu.dom.getParent(Fe.G("ZeroClipboardMovie_1")).style.left = (c.x + 11) + "px"
    //             }
    //         }
    //         clearInterval(a)
    //     }
    // },
    // 1000);
    if (!document.all) {
        Fe.G("copyButton").style.top = "-9px";
        Fe.G("copyButton").style.left = "5px"
    }
    if (baidu.browser.ie >= 8) {
        Fe.G("copyButton").style.top = "-22px";
        Fe.G("copyButton").style.left = "5px"
    }
    var b = navigator.userAgent.toLowerCase();
    if ((/mozilla/.test(b)) && (!/(compatible|webkit)/.test(b))) {
        Fe.G("copyButton").style.top = "-22px";
        Fe.G("copyButton").style.left = "5px"
    }

    // 绑定复制功能
    // Fe.G("copyButton").addEventListener('click', function() {
    //     console.log('-0-0-0-0-0-0')
    //     copyTextByCB();
    // });
    copyTextByCB();
}

function copyTextByCB() {

    function messageTips(text) {
        var c = Fe.G("copyMessage");
        c.innerHTML = text;
        c.style.display = "inline-block";
        setTimeout(function() {
            c.style.display = "none"
        }, 1000);
    }

    var clipboard = new Clipboard('#copyPoint');

    clipboard.on('success', function(e) {
        e.clearSelection();
        if (e.text !== '') {
            messageTips('复制成功');
        }
        // console.info('Action:', e.action);
        // console.info('Text:', e.text);
        // console.info('Trigger:', e.trigger);
    });

    clipboard.on('error', function(e) {
        // console.error('Action2:', e.action);
        // console.error('Trigger2:', e.trigger);
        messageTips('请手动复制');
    });
    // var inpObj = document.getElementById('pointInput');
    // function messageTips(text) {
    //     var c = Fe.G("copyMessage");
    //     c.innerHTML = text;
    //     c.style.display = "inline-block";
    //     setTimeout(function() {
    //         c.style.display = "none"
    //     }, 1000);
    // }

    // var clipboard = new Clipboard(document.getElementById('copyPoint'), {
    //     text: function(target) {
    //         return inpObj.value || '';
    //     }
    // });

    // clipboard.on('success', function(e) {
    //     e.clearSelection();
    //     messageTips('复制成功');
    // });

    // clipboard.on('error', function(e) {
    //     messageTips('请手动复制');
    // });


}

function getAbsPoint(c) {
    var b = c.offsetLeft;
    var a = c.offsetTop;
    while (c = c.offsetParent) {
        b += c.offsetLeft;
        a += c.offsetTop
    }
    return {
        x: b,
        y: a
    }
}
// function CreateFlash(a, e, f, b, d) {
//     var c = '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" width="' + f + '" height="' + b + '" id="' + a + '" align="middle">';
//     c += '<param name="allowScriptAccess" value="always">';
//     c += '<param name="quality" value="high">';
//     c += '<param name="movie" value="' + e + '">';
//     c += '<param name="flashvars" value="' + d + '">';
//     c += '<embed src="' + e + '" flashvars="' + d + '" quality="high" width="' + f + '" height="' + b + '" name="' + a + '" align="middle" allowScriptAccess="always" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer">';
//     c += "</object>";
//     return c
// }
// function ieCopy() {
//     var a = Fe.G("pointInput");
//     var c = a.value;
//     if (!c) {
//         return
//     }
//     if (window.clipboardData) {
//         window.clipboardData.clearData();
//         window.clipboardData.setData("Text", c);
//         if (Fe.G("copyMessage")) {
//             Fe.G("copyMessage").style.display = ""
//         }
//         setTimeout(function() {
//             if (Fe.G("copyMessage")) {
//                 Fe.G("copyMessage").style.display = "none"
//             }
//         },
//         1000)
//     } else {
//         try {
//             Fe.copy(c);
//             if (Fe.G("copyMessage")) {
//                 Fe.G("copyMessage").style.display = ""
//             }
//             setTimeout(function() {
//                 if (Fe.G("copyMessage")) {
//                     Fe.G("copyMessage").style.display = "none"
//                 }
//             },
//             1000);
//             return c
//         } catch(b) {}
//     }
// }
function filtQuery(a) {
    a = a || "";
    return a.replace(/[\uac00-\ud7a3]/g, "").replace(/\u2022|\u2027|\u30FB/g, String.fromCharCode(183)).replace(/^\s*|\s*$/g, "")
}
function beginsearch(b, a) {
    var c = filtQuery(Fe.G("localvalue").value);
    // 处理特殊城市
    if (isInArray(c)) {
        trickCity(c);
        return;
    }

    if (!c || c == "请输入关键字进行搜索") {
        return
    }
    if (Fe.G("pointLabel").checked) {
        searchByPoint(c)
    } else {
        if (!a) {
            b.setLocation(map)
        }
        b.search(c)
    }
}
// 判断是否市特殊城市
function isInArray(value){
    var arr = ['钓鱼岛','赤尾屿','台北','高雄','台湾','台湾省','台北市','高雄市'];
    for(var i = 0; i < arr.length; i++){
        if(value === arr[i]){
            return true;
        }
    }
    return false;
}
// 处理特殊城市的状态
function trickCity(cityName) {
    Fe.G("txtPanel").innerHTML = '<span style="color:#00c">已切换至' + cityName + "</span>";
    switch (cityName) {
        case '钓鱼岛':
            var point = new BMap.Point(123.480329,25.748826);
        break;
        case '赤尾屿':
            var point = new BMap.Point(124.582155,25.91351);
        break;
        case '台北':
        case '台北市':
            var point = new BMap.Point(121.546943,25.045762);
        break;
        case '高雄':
        case '高雄市':
            var point = new BMap.Point(120.326972,22.672297);
        break;
        case '台湾':
        case '台湾省':
            var point = new BMap.Point(120.985825,23.773814 );
        break;

    }
    map.centerAndZoom(point, 10);
}

function searchByPoint(c) {
    var d = c.split(",");
    var b;
    var g = "";
    var e = temp.poiSearchMark;
    var a = temp.geoCoder;
    if (d[0] && d[0].split(".")[0].length > 5 && d[1] && d[1].split(".")[0].length > 5) {
        var f = projection.pointToLngLat(new BMap.Pixel(d[0], d[1]));
        d = [f.lng, f.lat]
    }
    if (d[0] && d[1]) {
        clearLastResult();
        b = new BMap.Point(d[0], d[1]);
        if (!a) {
            a = new BMap.Geocoder();
            temp.geoCoder = a
        }
        a.getLocation(b,
        function(h) {
            if (h.address) {
                Fe.G("txtPanel").innerHTML = "<b>地址：</b>" + h.address
            }
        });
        if (e) {
            e.show();
            e.setPosition(b)
        } else {
            e = new BMap.Marker(b, {
                enableMassClear: false
            });
            map.addOverlay(e);
            temp.poiSearchMark = e
        }
        map.centerAndZoom(b, 15)
    } else {
        showMessage("请输入正确的坐标")
    }
}
function searchInthisCity(a) {
    l_local.setLocation(a);
    beginsearch(l_local, "cityList")
}
document.onkeydown = function(evt) {
    var evt = window.event ? window.event: evt;
    target = evt.target ? evt.target: evt.srcElement;
    fun = target.getAttribute("callback");
    if (fun && evt.keyCode == 13) {
        eval(fun)
    }
};
function scriptRequest(url, echo, id, charset) {
    var isIe = /msie/i.test(window.navigator.userAgent);
    if (isIe && Fe.G("_script_" + id)) {
        var script = Fe.G("_script_" + id)
    } else {
        if (Fe.G("_script_" + id)) {
            Fe.G("_script_" + id).parentNode.removeChild(Fe.G("_script_" + id))
        }
        var script = document.createElement("script");
        if (charset != null) {
            script.charset = charset
        }
        if (id != null && id != "") {
            script.setAttribute("id", "_script_" + id)
        }
        script.setAttribute("type", "text/javascript");
        document.body.appendChild(script)
    }
    var t = new Date();
    if (url.indexOf("?") > -1) {
        url += "&t=" + t.getTime()
    } else {
        url += "?t=" + t.getTime()
    }
    var _complete = function() {
        if (!script.readyState || script.readyState == "loaded" || script.readyState == "complete") {
            if (echo == "null") {
                return
            } else {
                if (typeof(echo) == "function") {
                    try {
                        echo()
                    } catch(e) {}
                } else {
                    eval(echo)
                }
            }
        }
    };
    if (isIe) {
        script.onreadystatechange = _complete
    } else {
        script.onload = _complete
    }
    script.setAttribute("src", url)
}
function Page(d, c, e) {
    Fe.BaseClass.call(this);
    if (!d) {
        return
    }
    this.container = (typeof(d) == "object") ? d: Fe.G(d);
    this.page = 1;
    this.pageCount = 100;
    this.argName = "pg";
    this.pagecap = 4;
    this.callback = c;
    this.update = true;
    var a = {
        page: 1,
        totalCount: 100,
        pageCount: 100,
        pagecap: 4,
        argName: "pg",
        update: true
    };
    if (!e) {
        e = a
    }
    for (var b in e) {
        if (typeof(e[b]) != "undefined") {
            this[b] = e[b]
        }
    }
    this.render()
}
Fe.extend(Page.prototype, {
    render: function() {
        this.initialize()
    },
    initialize: function() {
        this.checkPages();
        this.container.innerHTML = this.createHtml()
    },
    checkPages: function() {
        if (isNaN(parseInt(this.page))) {
            this.page = 1
        }
        if (isNaN(parseInt(this.pageCount))) {
            this.pageCount = 1
        }
        if (this.page < 1) {
            this.page = 1
        }
        if (this.pageCount < 1) {
            this.pageCount = 1
        }
        if (this.page > this.pageCount) {
            this.page = this.pageCount
        }
        this.page = parseInt(this.page);
        this.pageCount = parseInt(this.pageCount)
    },
    getPage: function() {
        var c = location.search;
        var a = new RegExp("[?&]?" + this.argName + "=([^&]*)[&$]?", "gi");
        var b = c.match(a);
        this.page = RegExp.$1
    },
    createHtml: function() {
        var b = [],
        f = this.page - 1,
        e = this.page + 1;
        b.push('<p class="page">');
        if (f < 1) {} else {
            if (this.page >= this.pagecap) {
                b.push('<span><a href="javascript:void(0)" onclick="Instance(\'' + this.hashCode + "').toPage(1);\">首页</a></span>")
            }
            b.push('<span><a href="javascript:void(0)" onclick="Instance(\'' + this.hashCode + "').toPage(" + f + ');">上一页</a></span>')
        }
        if (this.page < this.pagecap) {
            if (this.page % this.pagecap == 0) {
                var a = this.page - this.pagecap - 1
            } else {
                var a = this.page - this.page % this.pagecap + 1
            }
            var d = a + this.pagecap - 1
        } else {
            var c = Math.floor(this.pagecap / 2);
            var h = this.pagecap % 2 - 1;
            if (this.pageCount > this.page + c) {
                var d = this.page + c;
                var a = this.page - c - h
            } else {
                var d = this.pageCount;
                var a = this.page - c - h
            }
        }
        if (this.page > this.pageCount - this.pagecap && this.page >= this.pagecap) {
            var a = this.pageCount - this.pagecap + 1;
            var d = this.pageCount
        }
        for (var g = a; g <= d; g++) {
            if (g > 0) {
                if (g == this.page) {
                    b.push("<span>" + g + "</span>")
                } else {
                    if (g >= 1 && g <= this.pageCount) {
                        b.push('<span><a href="javascript:void(0)" onclick="Instance(\'' + this.hashCode + "').toPage(" + g + ');">[' + g + "]</a></span>")
                    }
                }
            }
        }
        if (e > this.pageCount) {} else {
            b.push('<span><a href="javascript:void(0)" onclick="Instance(\'' + this.hashCode + "').toPage(" + e + ');">下一页</a></span>')
        }
        b.push("</p>");
        return b.join("")
    },
    toPage: function(b) {
        var a = b ? b: 1;
        if (typeof(this.callback) == "function") {
            this.callback(a);
            this.page = a
        }
        if (this.update) {
            this.render()
        }
    }
});
function Popup(a) {
    Fe.BaseClass.call(this);
    this.visible = false;
    this.config = a;
    if (!this.config) {
        return
    }
    this.config.addDom = this.config.addDom ? Fe.G(this.config.addDom) : document.body;
    if (a.clickClose != null && a.clickClose == false) {
        this.config.clickClose = false
    } else {
        this.config.clickClose = true
    }
    this.connectDom = new Array()
}
Fe.extend(Popup.prototype, {
    render: function() {
        var b = this.config;
        this.main = beforeEndHTML(b.addDom, '<div class="map_popup" style="width:390px;display:none"></div>');
        var a = this.popBox = beforeEndHTML(this.main, '<div class="popup_main"></div>');
        if (b.isTitle != false) {
            this.title = beforeEndHTML(a, '<div class="title">系统信息</div>')
        }
        this.content = beforeEndHTML(a, '<div class="content"></div>');
        if ( !! this.config.closeButton) {
            this.button = beforeEndHTML(a, this.config.closeButton)
        } else {
            this.button = beforeEndHTML(a, '<button id="popup_close"></button>')
        }
        this.shadow = beforeEndHTML(this.main, '<div class="poput_shadow"></div>');
        this.addConnectDom(this.main);
        this.initialize()
    },
    initialize: function() {
        var c = this.config;
        this.setTitle(c.title);
        this.setContent(c.content);
        this.setWidth(c.width);
        this.setHeight(c.height);
        this.show();
        var a = this;
        var b = function(d) {
            var f = d.srcElement || d.target;
            while (f) {
                var e = a.connectDom;
                for (var g = 0; g < e.length; g++) {
                    if (f == e[g]) {
                        return
                    }
                }
                if (f == document.body) {
                    a.close();
                    return
                }
                f = f.parentNode
            }
        };
        if (this.config.clickClose) {
            Fe.on(document.body, "mousedown", b)
        }
        Fe.on(this.button, "click",
        function(d) {
            if (a.config.clickClose) {
                Fe.un(document.body, "mousedown", b)
            }
            if (a.config.closeEffect && typeof(a.config.closeEffect) == "function") {
                a.config.closeEffect()
            } else {
                a.main.parentNode.removeChild(a.main)
            }
            a.visible = false;
            if (a.config.close && typeof(a.config.close) == "function") {
                a.config.close()
            }
            if (this.resizeTimer) {
                window.clearInterval(this.resizeTimer);
                this.resizeTimer = null
            }
            if (Fe.G("imgLogo")) {
                Fe.G("imgLogo").style.display = "";
                Fe.G("imgLogo").style.display = "inline"
            }
        });
        if (c.open && typeof(c.open) == "function") {
            c.open()
        }
    },
    setTitle: function(a) {
        if (a && this.title) {
            this.title.innerHTML = a;
            this.config.title = a
        }
    },
    setContent: function(a) {
        if (a) {
            if (typeof(a) == "string") {
                this.content.innerHTML = a
            } else {
                this.content.innerHTML = "";
                this.content.appendChild(a)
            }
            this.config.content = a
        }
    },
    setWidth: function(a) {
        if (a) {
            this.main.style.width = (a - 8) + "px";
            this.config.width = a
        }
    },
    setHeight: function(a) {
        if (this.resizeTimer) {
            window.clearInterval(this.resizeTimer);
            this.resizeTimer = null
        }
        if (a) {
            this.main.style.height = this.shadow.style.height = (a - 9) + "px";
            this.config.height = a;
            if (this.config.isTitle == false) {
                this.content.style.height = (a - 2) + "px"
            } else {
                this.content.style.height = (a - 24 - 9) + "px"
            }
            this.content.style.overflowY = "auto"
        } else {
            this.content.style.height = "auto";
            this.resize()
        }
    },
    hide: function() {
        this.main.style.display = "none";
        this.visible = false
    },
    show: function() {
        this.main.style.display = "block";
        this.popBox.scrollTop = 0;
        this.visible = true
    },
    getDom: function() {
        return this.main
    },
    resize: function() {
        var a = this;
        var b = function() {
            if (a.config.isAddBottomHeight == false) {
                var c = a.content.offsetHeight
            } else {
                var c = a.content.offsetHeight + 24
            }
            if (a.mainHeight) {
                if (a.mainHeight != c) {
                    a.mainHeight = c
                }
            }
            a.popBox.style.height = a.shadow.style.height = a.main.style.height = c + "px";
            a.popBox.scrollTop = 0
        };
        if (this.resizeTimer) {
            window.clearInterval(this.resizeTimer);
            this.resizeTimer = null
        }
        this.resizeTimer = window.setInterval(b, 50)
    },
    close: function() {
        this.button.click()
    },
    addConnectDom: function(a) {
        this.connectDom.push(a)
    }
});
function stopBubble(a) {
    var a = window.event || a;
    a.stopPropagation ? a.stopPropagation() : a.cancelBubble = true
}
function preventDefault(a) {
    var a = window.event || a;
    a.preventDefault ? a.preventDefault() : a.returnValue = false;
    return false
};