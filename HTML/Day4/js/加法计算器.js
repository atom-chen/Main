//注册监听
//①获取文本框的内容
var JLabel1=document.getElementById("num1");
var JLabel2=document.getElementById("num2");
var JButton=document.getElementById("sum");
var resLab=document.getElementById("res");

//②点击事件：点击sum触发什么效果
JButton.onclick=function()
{
	//弹框！
	alert("听到了！");
	//获取文本框里的浮点值,标签.value,如不可解析返回NaN
	var num1=parseFloat(JLabel1.value);
	var num2=parseFloat(JLabel2.value);
	
	resLab.innerHTML=num1+num2;
	
}