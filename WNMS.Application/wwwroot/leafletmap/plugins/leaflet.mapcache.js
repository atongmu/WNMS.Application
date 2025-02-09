﻿
/** 
 * 继承自TileLayer 
 * @param {Object} tomcat中映射该切片目录url 
 * @param {Object} options 
 */  

L.TileLayer.TileLoad = L.TileLayer.extend({  
    initialize: function (url, options) {  
        options = L.setOptions(this, options);  
        this.url = url + "/{z}/{x}/{y}.png";  
//        console.log(url);
        L.TileLayer.prototype.initialize.call(this, this.url, options);  
    }  
});  

/** 
 * 重写TileLayer中获取切片url方法 
 * @param {Object} tilePoint 
 */  

L.TileLayer.prototype.getTileUrl = function(tilePoint) {  
    if(this._url.indexOf("_alllayers")<0)
    {
		    return L.Util.template(this._url, L.extend({  
		        s: this._getSubdomain(tilePoint),  
		        z: tilePoint.z,  
		        x: tilePoint.x,  
		        y: tilePoint.y 
		    }));  
    }
    else
    {
	    return L.Util.template(this._url, L.extend({  
	        s: this._getSubdomain(tilePoint),  
	        z: function() {  
//	        	console.log(tilePoint);
	            var value = (tilePoint.z).toString(16);  
	            return "L" + pad(value, 2);  
	        },  
	        x: function() {  
	        	
	            var value = (tilePoint.y).toString(16);  
	            return "R" + pad(value, 8);  
	        },  
	        y: function() {  
	        	
	            var value = (tilePoint.x).toString(16);  
	            return "C" + pad(value, 8);  
	        }  
	    }));  
  }
};


/** 
 * 高位补全方法 
 * @param {Object} 数字类型字符串 
 * @param {Object} 总位数，不足则高位补0 
 */  
var pad = function(numStr, n) {  
    var len = numStr.length;  
    while(len < n) {  
        numStr = "0" + numStr;  
        len++;    
    }  
    return numStr;  
};  
