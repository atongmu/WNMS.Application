"use strict";

var windowWidth = $(window).width(),
	windowHeight = $(window).height(),
	timeWidthDhmRange = 0; // 天时分宽度

$(function() {
	/**
	 * 设置时间框宽度
	 */
	if($('.time-value-content').length > 0) {
		timeWidthDhmRange = $('.time-width-dhm-range').outerWidth();
	}
});

/**
 * 选项卡切换
 * @param {Object} callback
 */
function tabNavClick(callback) {
	$('.tab-nav .item').click(function() {
		var index = $(this).index();
		$(this).addClass('active').siblings().removeClass('active');
		$(this).parent().parent().find('.tab-content-item').eq(index).show().siblings().hide();
		
		callback && callback(index);
	});
}

/**
 * 日期插件初始化
 */
function advanceDateInit() {
	$('.advanceDate').each(function() {
		var $this = $(this),
			datetype = $(this).attr('data-datetype'),
			datetype = datetype ? datetype : 'date',
			dateformat = $(this).attr('data-dateformat'),
			dateformat = dateformat ? dateformat : 'yy-mm-dd';
		
		switch(datetype) {
			case 'date':
				$this.mobiscroll().date({
					theme: 'android-holo-light',
					lang: 'zh',
					dateFormat : dateformat,
					display: 'center',
					max: new Date(),
					defaultValue: [new Date(), new Date()]
				});
				break;
				
			case 'range':
				$this.mobiscroll().range({
					theme: 'android-holo-light',
					lang: 'zh',
					dateFormat : 'yyyy-mm-dd',
					display: 'center',
					controls: ['datetime'],
					max: new Date(),
					defaultValue: [new Date(), new Date()]
				});
				break;
		}
	});
}