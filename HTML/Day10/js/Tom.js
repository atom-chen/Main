window.onload=start();
var timer;//播放动画的计时器

function start()
{
	document.getElementById("cymbal").onclick=function()
	{
		getAnimation("cymbal",13);
	};
	document.getElementById("eat").onclick=function()
	{
		getAnimation("eat",40);
	};	
	document.getElementById("drink").onclick=function()
	{
		getAnimation("drink",81);
	};	
	document.getElementById("pie").onclick=function()
	{
		getAnimation("pie",24);
	};	
	document.getElementById("scratch").onclick=function()
	{
		getAnimation("scratch",56);
	};
		document.getElementById("fart").onclick=function()
	{
		getAnimation("fart",28);
	};
}
//切换图片动画方法(参数 动画名,图片数量)
function getAnimation(name,count)
{
	//在执行之前 清空计时器
	clearInterval(timer);
	var index=0;//初始化下标为0
	
	//获取图片对象
	var img=document.getElementById("cat");
	
	//计时器
	timer=setInterval(function()
	{
		if(++index<count)
		{
			//切换路径
			img.src=getImages(name,index);
		}
		else
		{
			clearInterval(timer);
		}
	},80);
}

//图片路径处理方法
function getImages(name,index)
{
	return "img/Animations/"+name+"/"+name+"_"+getName(index)+".jpg";
}

//图片命名处理方法
function getName(index)
{
	//如果1输出01，如果12就输出12
	if(index<10)
	{
		//输出0x
		return "0"+index;
	}
	else
	{
		//输出下标
		return index;
	}
}
