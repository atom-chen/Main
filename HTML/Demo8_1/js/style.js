//JQuery基本语法： $(selecter).antion();
//1、使用美元符号定义JQuery函数
//2、selecter为选择器(兼容4种选择器id class 标签 派生)
//3、action()为要执行的函数

var nowpage=0;//记录当前页面
//JQuery的入口
$(document).ready(function()
{
	//获取手机宽和高
	var CellphoneWidth=window.innerWidth;
	var CellphoneHeight=window.innerHeight;
	
	//设置顶层容器div的宽、高
	$(".content").width(CellphoneWidth);
	$(".content").height(CellphoneHeight*4);
	
	//设置每个页面的宽高
	$(".page").width(CellphoneWidth);
	$(".page").height(CellphoneHeight);
	
	//出场光环
	//1、楼房淡入
	$(".page1_building").fadeIn(2000,
		function()
		{
			//小人渐大
			$(".page1_avatar").animate({width:70+"%"},{duration:2500})
		}
		);
	
	//触碰监听：事件：触屏  事件源：手机屏幕
	$(".content").swipe(
		{
		//event；事件本身
		//direction:滑动方向
		//distance:距离(像素)
		//duration:滑动的过程时间
		//fingerCount:触碰点
		swipe:function(event,direction,distance,duration,fingerCount)
		{
			//处理向某个方向滑动
			if(direction=="up")
			{
				//如果页面到顶
				if(nowpage>=5)
				{
					nowpage=5;
				}else
				{
					nowpage+=1;
				}
				
			}else if(direction=="down")
			{
				if(nowpage==0)
				{
					nowpage=0;
				}
				else
				{
					nowpage-=1;
				}
			}
			//播放动画：设置切换：高度变换，切换时间
			$(".content").animate({top:nowpage*-100+"%"},
			{duration:500,complete:ans()})
		}
	});
});
//页面切换处理方法
function ans()
{
	//如果滑到界面2
	if(nowpage==1)
	{
		$(".page2_bg").fadeIn(2000,
		function()
		{
			$(".page2_farm").fadeIn(1000,function(){$(".page2_it").fadeIn(1000)})
		}
		);
	}//如果切到界面3
		//如果滑到第三个页面
	else if(nowpage == 2){
		$(".page3_bus").animate({left:"-100%"}, {duration:2000});
		$(".page3_person").animate({right:"50%"}, {duration:3000,
			complete:function(){
				$(".page3_eT,.page3_Bt,.page3_station,.page3_person").fadeOut("slow",function(){
					//控制页面三的第二个场景
					//1.楼房出现
					$(".page3-wall").fadeIn(1000,function(){
						//2.人的出现
						$(".page3-tAvatar").fadeIn(1000,function(){
							//3.左侧文字出现
							$(".page3-space").animate({width:"40%"},{duration:1000,complete:function(){
								//4.“我的团队在哪儿”变宽
								$(".page3-where").animate({width:"50%"},{duration:1000});
							}});
						});
					});
				});
			}});
		$(".page3-title").fadeIn(1000,function(){
			$(".page3-btitle").fadeIn(1000);
		});
	}
	//第五页
	else if(nowpage==4)
	{
		//人从底部渐大
		$(".page5_bg").fadeIn(1000,function()
		{
			$(".page5_person").animate({bottom:"20%"},{duration:1000});
			$(".page5_person").animate({width:"40%"},{duration:1000,
			complete:function()
			{
				
				$(".page5_Ta").fadeIn(1000,function()
				{
					$(".page5_Txt").fadeIn(1000);
				})
			}
		});
		});
		
	}
	else if(nowpage==5)
	{
		$(".page6_bg").fadeIn(500);
		$(".page6_person").animate({left:'10%'},{duration:1000});
		$(".page6_person").animate({top:'60%',width:'40%'},{duration:2000,
		complete:function()
		{
			$(".page6_runTa").fadeIn(1000,function()
			{
				$(".page6_runTxt").fadeIn(1000);
			});
			
		}
		})
	}
}
//点击灯：切换到新界面
function start(img)
{
	img.src="images/lightOn.png";
	$(".page4_bg,.page4_title,.page4_guide").fadeOut("slow",function()
	{
		$(".page4_onBg").fadeIn(1000,function()
		{
			$(".page4_you").fadeIn(1000);
		});
	});

}
function playPause(img)
{
	//获取音乐
	var music=document.getElementById("musicplayer");
	if(music.paused)
	{
		//播放音乐
		music.play();
		img.src="images/music.png";		
	}
	else
	{
		//暂停音乐
		music.pause();
		img.src="images/stop.png"	
	}
}
