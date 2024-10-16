var designSize = 856,
	html = document.documentElement,
	wW = html.clientWidth,
	baseSize = wW * 100 / designSize;
	
if (baseSize < 60) {
	baseSize = 60;
}
else if(baseSize > 80) {
	baseSize = 80;
}

html.style.fontSize = baseSize + 'px';
