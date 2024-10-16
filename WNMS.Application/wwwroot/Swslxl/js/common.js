"use strict";

var windowHeight = $(window).height(),
	fullHeight = 0;

$(function() {
	/**
	 * 根据屏幕设置全屏高度的最小高度
	 * negative负向偏移，由main的padding-top + border-box的上外边距
	 */
	var windowHeightNegative = parseFloat($('.window-height').data('negative'));
	fullHeight = windowHeightNegative ? (windowHeight - windowHeightNegative * baseSize) : windowHeight;
	fullHeight = (fullHeight < 500) ? 500: fullHeight;
	$('.window-height').height(parseInt(fullHeight));
	
	/**
	 * 设置时间框宽度
	 */
	if($('.time-value-content').length > 0) {
		var timeWidthDhmsRange = $('.time-width-d-range').width();
		$('.time-d-range').width(timeWidthDhmsRange);
		
		var timeWidthDRange = $('.time-width-d-range').width();
		$('.time-d-range').width(timeWidthDRange);
		
		var timeWidthD = $('.time-width-d').width();
		$('.time-d').width(timeWidthD);
	}

	/**
		* 侧边栏折叠
		*/
    $('.btn-aside-fold').click(function() {
        var active = $(this).hasClass('active');
        if (active) {
            $(this).removeClass('active');
            $('.ztree-container').removeClass('active');
            $('.has-left-menu').css('padding-left', 0);
        }
        else {
            $(this).addClass('active');
            $('.ztree-container').addClass('active');
            $('.has-left-menu').css('padding-left', 290);
		}

        setTimeout(function() {
            echartsApp.forEach(function (item) {
                console.log(item);
                item.resize();
            });
		}, .3 * 1000);
	});
});

/**
 * 检测屏幕尺寸变化
 */
$(window).resize(function(){
    location.reload();
});

/**
 * 获取当前时间
 */
function formatTime(date, format, join, mode, difference) {
	date = date.toString();
	date = new Date(date)
	
	if(difference !== undefined) {
		switch(format) {
			case 'y':
				date.setFullYear(date.getFullYear() + difference);
				break;
			case 'm':
				date.setMonth(date.getMonth() + difference);
				break;
			case 'd':
				date.setDate(date.getDate() + difference);
				break;
		}
	}

	var Y = date.getFullYear(),
		M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1),
		D = (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()),
		h = (date.getHours() < 10 ? '0' + date.getHours() : date.getHours()) + ':',
		m = (date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes()) + ':',
		s = (date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds()),
		result = null;
	
	if(join == '年') {
		Y = Y + '年';
		M = M + '月';
		D = D + '日';
	}
	else {
		Y = Y;
		M = '-' + M;
		D = '-' + D;
	}
	
	if(mode == 'dawn') {
		// 黎明
		h = '00:';
		m = '00:';
		s = '00';
	}
	else if(mode == 'midnight') {
		// 午夜
		h = '23:';
		m = '59:';
		s = '59';
	}
	
	switch(format) {
		case 'y':
			result = Y;
			break;
		case 'm':
			result = Y + M;
			break;
		case 'd':
			result = Y + M + D;
			break;
		case 'HM':
			result =  h + m.replace(':',"");
			break;
		case 'DHM':
			result = Y + M + D + ' ' + h + m.replace(':',"");
			break;
		case 'DHMSTRONG':
			result = Y + M + D + ' <strong>' + h + '</strong><strong>' + m.replace(':',"") + '</strong>';
			break;
		default:
			result = Y + M + D + ' ' + h + m + s;
	}
	
	return result;
}

/**
 * 单行文本长度省略
 * @param {String} 	text	要转换的文字
 * @param {Num} 	num		单行文本长度
 * @param {String} 	join	省略符
 */
function textSingleLineFormat(text, num, join) {
	join = join ? join : '...';
	
	if (!text) return '';
	
	if (text.length > num) {
		text = text.slice(0, num) + join;
	}
	return text;
}
			
/**
 * 单行长文本转多行折行文本
 * @param {String} 	text	要转换的文字
 * @param {Num} 	num		一行的分隔长度
 * @param {String} 	join	转换后的换行符
 */
function textLengthFormat(text, num, join) {
	var textArr = text.replace(/(^\s*)|(\s*$)/g, "").split(''),
		result = [];
	for (var i = 0, length = textArr.length; i < length;) {
		var currentText = textArr.slice(i, i += num);
		result.push(currentText.join(''));
	}
	
	result = result.join(join);
	
	return result;
}

/**
 * 接收页面传递的参数
 */
function getQueryString(name) {
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
	var r = window.location.search.substr(1).match(reg); 
	if (r != null) return decodeURI(r[2]); return null;
}

/**
 * 数字转百分比
 * @param {any} num
 */
function toPercent(num) {
	// 扩大100倍
	num = num * 10000 / 100;
	
	// 向下保留两位小数
	num = Math.floor(num * 100) / 100;
	
	return num + '%';
}

/**
 * 千分位格式化
 * @param {any} num
 */
function formatThousands(num) {
	return (num + '').replace(/(\d{1,3})(?=(\d{3})+(?:$|\.))/g, '$1,');
}

/**
 * 图表滑块切换
 */
function intEchartsTabs(callback) {
	// 初始化滑块位置
	setEchartsSlider();
	
	$('body').on('click', '.echarts-tabs .item', function() {
		var type = $(this).data('type');
		$(this).addClass('active').siblings().removeClass('active');
		
		// 刷新位置
		setEchartsSlider();
		
		// 回调函数
		callback && callback(type);
	});
}
/**
 * 设置图表滑块位置
 */
function setEchartsSlider() {
	var echartsTabsActive = $('.echarts-tabs .item.active');
	$('.echarts-slider').css({
		'width': echartsTabsActive.outerWidth(),
		'left': echartsTabsActive.position().left
	});
}

