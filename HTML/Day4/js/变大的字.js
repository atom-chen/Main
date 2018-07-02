//获取html上的对象
var JButton1=document.getElementById("bigger");
var JButton2=document.getElementById("litter");

//获取p标签的字
var text1=document.getElementById("text");
//确定字号
var size=20;
text1.style.fontSize=size+"px";

//如果点击了加号
JButton1.onclick=function()
{
	size+=5;
	text1.style.fontSize=size+"px";
	if(size>=50)
	{
		text1.style.color="cornflowerblue";
	}else
	{
		text1.style.color="black"
	}
}

//如果点击了减号
JButton2.onclick=function()
{
	size-=1;
	text1.style.fontSize=size+"px";
	if(size<=10)
	{
		text1.style.color="red";
	}
	else
	{
		text1.style.color="black"
	}
}
