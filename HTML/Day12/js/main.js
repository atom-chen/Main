
//鼠标点击,变换颜色
$(".navBar nav a").click(function(){
	$(this).addClass("currentPage").siblings().removeClass("currentPage");
})

//鼠标经过,变化颜色
$(".navBar nav a").mouseover(function(){
	$(this).addClass("mouse").siblings().removeClass("mouse");
}).mouseout(function(){
	$(this).removeClass("mouse");
});

//标记,记录滚动方向
var down = false;

//监听页面的滚动
$(window).scroll(function(){
//	看不见导航栏
	if($(this).scrollTop()>100){
//		让导航栏从上至下弹下来
		$(".navBar").addClass("fixed");
		if(!down){
			$(".navBar").css({"top":"-40px"});
			$(".navBar").animate({"top":"0"},500);
			down = true;
		}
	}else{
		$(".navBar").removeClass("fixed");
		down = false;
	}
});








