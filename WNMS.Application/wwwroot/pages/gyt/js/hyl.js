//wave
var wave1 = $('#feel-the-wave,#feel-the-wave2').wavify({
	  height: 70,
	  bones: 4,
	  amplitude: 50,
	  color: '#7BA8EB',
	  speed: .15
});
var wave2 = $('#feel-the-wave-two,#feel-the-wave-two2').wavify({
	  height: 60,
	  bones: 3,
	  amplitude: 40,
	  color: '#4782DC',
	  speed: .25
});

//switch
layui.use('form', function(){
  var form = layui.form;
  	form.on('switch(switchTest)', function(data){
	  var elems = data.elem.checked;
	  if( elems == false){
	  	$(".path1").addClass("hidden");
	  	$(".txt4 span:first-child").removeClass("xz");
	  	new NoticeJs({										//notice
			    text: '阀门关闭.....',
			    position: 'bottomRight',
			    animation: {
			        open: 'animated bounceIn',
			        close: 'animated bounceOut'
			    }
			}).show();	
	  }else{
	  	$(".txt4 span:first-child").addClass("xz");
	  	$(".path1").removeClass("hidden");
	  	new NoticeJs({										//notice
			    text: '阀门开启.....',
			    position: 'bottomRight',
			    animation: {
			        open: 'animated bounceIn',
			        close: 'animated bounceOut'
			    }
			}).show();	
	  }
	  
	});  
});

//tobig
	$("#big").click(function(){
		var w = $(".ibox").width();
		var h = $(".ibox").height();
		var ssjw = $(".jingwater").width();
		var ssjh = $(".jingwater").height();
		var scw = $(".scwater").width();
		var sch = $(".scwater").height();
		var mtop = h / 2;	
		var size = w * 0.9;		
		var posleft1 = $(".jingwater").position().left;
		var posleft2 = $(".scwater").position().left;			
		if( size >= 1307){
			new NoticeJs({										//notice
			    text: '达到最大尺寸.....',
			    position: 'bottomRight',
			    animation: {
			        open: 'animated bounceIn',
			        close: 'animated bounceOut'
			    }
			}).show();	
			return false;
		}else if(size < 1598){
			$(".ibox").css("margin-top",-mtop - 50 +'px');						//修改负边距		
			$(".ibox,.ditu,.bg1,.bg2").width(w * 1.1).height(h * 1.1);
			$(".jingwater").width(ssjw * 1.1).height(h * 0.8).css("left", posleft1 * 1.1);
			$(".scwater").width(scw * 1.1).height(sch * 1.1).css("left", posleft2 * 1.1);
		}
	});
	//tosmall
	$("#small").click(function(){
		var w = $(".ibox").width();
		var h = $(".ibox").height();
		var ssjw = $(".jingwater").width();
		var ssjh = $(".jingwater").height();
		var scw = $(".scwater").width();
		var sch = $(".scwater").height();
		var posleft1 = $(".jingwater").position().left;
		var posleft2 = $(".scwater").position().left;	
		var mtop = h / 2;
		var size = w * 0.9;		
		if( size <= 1081){			
			new NoticeJs({										//notice
			    text: '达到最小尺寸.....',
			    position: 'bottomRight',
			    animation: {
			        open: 'animated bounceIn',
			        close: 'animated bounceOut'
			    }
			}).show();	
			return false;
		}else if(size > 1080){
			$(".ibox").css("margin-top",-mtop  +'px');
			$(".ibox,.ditu,.bg1,.bg2").width(w * 0.9).height(h * 0.9);
			$(".jingwater").width(ssjw * 0.9).height(h * 0.7).css("left", posleft1 * 0.9);
			$(".scwater").width(scw * 0.9).height(sch * 0.9).css("left", posleft2 * 0.9);
		}		
	});	
	//tonormal
	$("#normal").click(function(){
		if($(".ibox").width() == 1200){
			new NoticeJs({										//notice
				    text: '工艺图还原到初始尺寸.....',
				    position: 'bottomRight',
				    animation: {
				        open: 'animated bounceIn',
				        close: 'animated bounceOut'
				    }
				}).show();	
				return false;
		}else{
			new NoticeJs({										//notice
				    text: '工艺图还原到初始尺寸.....',
				    position: 'bottomRight',
				    animation: {
				        open: 'animated bounceIn',
				        close: 'animated bounceOut'
				    }
				}).show();	
				
			$(".ibox").css("margin-top",-340 +'px');
			$(".ibox,.ditu,.bg1,.bg2").width(1200).height(680);
			$(".jingwater").width(244).height(536).css("left", 196);
			$(".scwater").width(245).height(430).css("left", 808);
		}	
	})