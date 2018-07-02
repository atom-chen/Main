/*
 * 不支持平稳退化
 */


var time=0;
var lastIndex=0;

var contentDiv = document.getElementById("gameDiv");//获取整个草坪
/*若按下开始按钮*/
function start(obj)
{
	//设置计时器(点击开始后，开始计时)
	contentDiv.timmer=setInterval(timer,1000);
}

/*计时器的封装*/
function timer()
{
	//	随机产生一个0-8的数字
	var index = Math.round(Math.random()*8);
	/*	取整
	var index = Math.round(temp);*/
	
	//找到要冒出地鼠的草坪
	var child = contentDiv.children[index];
	//替换子类图片,将草坪替换为地鼠
	child.src = "img/2.gif";
	
	//需要一个全局变量lastIndex判断上一次随机数
	if(lastIndex != index)
	{
		var lastChild = contentDiv.children[lastIndex];
		lastChild.src = "img/5.jpg";
	}
	lastIndex = index;
	//修改时间
	time++;
	document.getElementById("time").innerHTML="时间："+time+"s";
	if(time%5==0)
	{
		alert("时间到！游戏结束！");
		//关闭窗口
		clearInterval(contentDiv.timmer);
	}
}

//点击草坪事件：判断该草坪是否是地鼠
var x = 0;
function beat(img){
	//取出点击到图片路径
	var srcstr = img.src;
	
	//判断此图片为草坪还是地鼠，若该图片路径倒数第五位是"2"，那么就说这是一个地鼠
	if(srcstr.charAt(srcstr.length-5) == "2")
	{	
		img.src = "img/5.jpg";
		x++;
		
		//将分数显示在积分框
		document.getElementById("score").innerHTML = "得分："+x;
		
	}
}
