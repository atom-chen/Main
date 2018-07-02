
//当打开界面时：page1的内容同时出现

//入口
$(document).ready(function()
{
	$("#page1").animate({opacity:1},{duration:700,complete:function()
	{
	//出场光环
	//1、标题文字淡入，标题问题从上方进入
	$("#MyMain").animate({top:10+"%",opacity:1},{duration:700});
	//2、文字淡入
	$("#write1").animate({top:27+"%",opacity:1},{duration:700});
	$("#write2").animate({top:34+"%",opacity:1},{duration:700});
	//3、图片淡入
	$("#page1 img").animate({top:50+"%",opacity:1},{duration:700});
	
	}
	});

})
//标记,记录滚动方向
var down = false;

//监听页面的滚动
$(window).scroll(function(){
	//如果看见了中间字体
	if($(this).scrollTop()>200)
	{
		$("#mid p").animate({top:0,opacity:1},{dufation:1000});
	}
//	如果看见了海南
	if($(this).scrollTop()>500)
	{
		$("#page2").animate({opacity:1},{duration:700,complete:function()
			{
			//海南两个字
			$("#MyHainan").animate({top:0,opacity:1},{dufation:700});
			//下面部分的字体
			$("#page2 #TitleHainan #write1").animate({top: 14+"%",opacity:1},{duration:700});
			$("#page2 #TitleHainan #write2").animate({top: 22+"%",opacity:1},
		       {duration:700,complete:function()
		       	//在字体显示完毕后
		       {
		       		//下面部分的图片
		       		$("#page2 #hainan3").animate({left: 65+"%",opacity:1},{duration:700});
		       		$("#page2 #hainan2").animate({bottom: 10+"%",top: 36+"%",opacity:1},{duration:700});
		       		$("#page2 #hainan").animate({left: 5+"%",opacity:1},{duration:700});
		       }
		       })
		    }
		})
	}
})
	

