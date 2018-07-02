
//让js知道外面的世界
//1、通过id获取元素
var div = document.getElementById("demo");
//2、通过class获取元素
var elec=document.getElementsByClassName("demo");
//3、通过标签获取元素(参数是标签名)
var elet=document.getElementsByTagName("input");

//set元素属性
div.abc=10;
//set数组
div.arr=["1","2","3","4","5"];
//set元素名字
div.className="???";
//set元素指定属性 (key,value)
div.setAttribute("id","sss");

//向某元素添加显示内容<div>XXXXXXX<div>
div.innerHTML="不想显示";
//获取元素的CSS属性引用
div.style.background="wheat";
div.style.width="300px";
div.style.height="300px";
div.style.font="微软雅黑";

//创建元素
var Ele1=document.createElement("div");
var Ele2=document.createElement("p");
//把参数插入到body里面
document.body.appendChild(Ele1);

















