var designSize = 1920, // 设计图尺寸
	html = document.documentElement,
	wW = html.clientWidth,
	baseSize = wW * 100 / designSize;

document.documentElement.style.fontSize = baseSize + 'px';
