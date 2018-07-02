window.onload=start();
//计算滑动时间
var clearTime=null;
//获取当前显示的图片
var $index=0;
//前一张图片
var $last_index=0;
function start()
{
	//鼠标经过
	$("#list li").mouseover(function()
	{
		//关掉计时器
		clearInterval(clearTime);
		//获取当前鼠标停留对应的图片序列号
		$index=$(this).index();
		scrollPlay();
		$last_index=$index;
	}).mouseout(function()
	{
		//鼠标移开
		aotoplay();
		
	})
	$("#box .btmRight").click(function()
	{
		$index++;
		if($index>7)
		{
			$index=0;
		}
		aotoplay();
		$last_index=$index;
		clearInterval(clearTime);
	})
	$("#box .btmLeft").click(function()
	{
		$index--;
		if($index<0)
		{
			$index=7;
		}
		aotoplay();
		$last_index=$index;
		clearInterval(clearTime);
	})
}

//功能：滑动的动画
function scrollPlay()
{
	//小方格变色
	//siblings()：过滤，通过选择器选择我们要的
	//removeClsss()：清空该对象
	$("#list li").eq($index).addClass("hover").siblings().removeClass("hover");
	//首先处理向左移动
	if($index>$last_index)
	{
		//eq()：选择器选取带有指定索引的元素
		//stop()：参数表示要关闭附带效果
		$("#imgbox img").eq($last_index).stop(true,true).animate({"left":"-720px"});
		$("#imgbox img").eq($index).css("left","720px").stop(true,true).animate({"left":"0"});
	}
	else if($index<$last_index)
	{
		$("#imgbox").eq($last_index).stop(true,true).animate({"left":"720px"});
		$("#imgbox").eq($index).css("left","-720px").stop(true,true).animate({"left":"0"})
	}
}
//功能：自动滚动
function aotoplay()
{
	clearTime=setInterval(aotu,2000);
}
//功能：页码切换
function aotu()
{
	$index++;
	if($index>7)
	{
		$index=0;
	}
	scrollPlay();
	$last_index=$index;
}

