//控制轮播的函数

//计算华东的时间
var clearTime = null;
//获取当前是哪张图片
var nowPic = 0;
//获取前一站图片
var lastPic = 0;

//封装一个自动滚动的函数
function autoPlay(){
	clearTime = setInterval(function(){
		//每次序列号+1
		nowPic++;
		if(nowPic > 6){
			nowPic = 0;
		}
		scrollPlay();
		lastPic = nowPic;
	},4000);
}

//封装一个滑动的动画
function scrollPlay(){
	//silibings()过滤，通过选择器选择可选项
	
	
	if(nowPic > lastPic){
		$(".img_box img").eq(lastPic).stop(true,true).animate({"left":"-720px"});
		$(".img_box img").eq(nowPic).css("left","720px").stop(true,true).animate({"left":"0"});
		
	}else if(nowPic < lastPic){
		$(".img_box img").eq(lastPic).stop(true,true).animate({"left":"720px"});
		$(".img_box img").eq(nowPic).css("left","-720px").stop(true,true).animate({"left":"0"});
		
	}
	
}
addLoadEvent(autoPlay());


//鼠标经过显示元素
function show(){
	$(".bottom_team").mouseover(function(){
		$(".bottom_leftPic").fadeIn(1000);
	});
}
addLoadEvent(show);



































