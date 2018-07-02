//入口：m1,m2淡入
$(document).ready(function()
{
	//m1,m2淡入
	$("#m1,#m2").fadeIn(1500);
})

//点赞
function love(img)
{
	//获取图片
	var pi=document.getElementsByClassName("love");
	var addres=pi.src;
	if(addres="img/anxin.jpg")
	{
		img.src="img/liangxin.jpg";
	}
	else if(addres="img/liangxin.jpg"){
		img.src="img/anxin.jpg";
	}
}

//鼠标经过,变化颜色
$("#connect #m1 #w1,#connect #m2 #w2,#connect #m3 #w3,#connect #m4 #w4,#connect #m5 #w5,#connect #m6 #w6,#connect #m7 #w7").mouseover(function(){
	$(this).addClass("write").siblings().removeClass("write");
}).mouseout(function(){
	$(this).removeClass("write");
});

var nowpage=1;
//点击右箭头，向后翻页
function paging_right()
{
	
	nowpage++;
	if(nowpage==4)
	{
		nowpage=1;
	}
	if(nowpage==1)
	{
		
		//此时前一页是第三页
		//m5,m6从左边出去
		$("#m5,#m6").animate({left: -100+"%"},{duration:1000,complete:function()
			{
				//m1,m2从右边进来
				$("#m1,#m2").animate({left:10+"%"},{duration:1000});
			}
		});
	}else if(nowpage==2)
	{
		//此时前一页是第一页
		//m1,m2从左边出去
		$("#m1,#m2").animate({left: -100+"%"},{duration:1000,complete:function()
			{
				//m3,m4从右边进来
				$("#m3,#m4").animate({left:10+"%"},{duration:1000});

			}
		});
	}else if(nowpage==3)
	{
		//此时前一页是第二页
		//m3,m4从左边出去
		$("#m3,#m4").animate({left: -100+"%"},{duration:1000,complete:function()
			{
				//m5,m6从右边进来
				$("#m5,#m6").animate({left:10+"%"},{duration:1000});
			}
		});
	}
}
function paging_left()
{
	nowpage--;
	if(nowpage==0)
	{
		nowpage=3;
	}
	if(nowpage==1)
	{
		//此时前一页是第2页
		//m3,m4从右边出去
		$("#m3,#m4").animate({left: -100+"%"},{duration:1000,complete:function()
			{
				//m1,m2从右边进来
				$("#m1,#m2").animate({left:10+"%"},{duration:1000});
			}
		});
	}else if(nowpage==2)
	{
		//此时前一页是第3页
		//m5,m6从左边出去
		$("#m5,#m6").animate({left: -100+"%"},{duration:1000,complete:function()
			{
				//m3,m4从右边进来
				$("#m3,#m4").animate({left:10+"%"},{duration:1000});

			}
		});
	}else if(nowpage==3)
	{
		//此前一页是第1页
		//m1,m2从左边出去
		$("#m1,#m2").animate({left: -100+"%"},{duration:1000,complete:function()
			{
				//m5,m6从右边进来
				$("#m5,#m6").animate({left:10+"%"},{duration:1000});
			}
		});
	}
}
