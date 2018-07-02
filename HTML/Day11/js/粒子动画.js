window.onload=function()
{	animates();
	
}
function animates()
{	//创建canvas标签(画布)
	var canvas=document.createElement("canvas");
	//将canvas插入body文档
	document.body.appendChild(canvas);
	//canvas背景颜色
	canvas.style.backgroundColor="black";
	//为canvas设置宽高
	canvas.width=window.innerWidth;
	canvas.height=window.innerHeight;
	//取出画笔
	var context=canvas.getContext("2d");
	//创建一个数组，用于保存粒子
	var particles=[];
	loop();
	//定义一个随机产生粒子的方法
	function loop()
	{	//计时器
		setInterval(function(){
			//清空画布
			context.clearRect(0,0,canvas.width,canvas.height);
			//随机产生一个原型的位置
			var part=new Particle(canvas.width/2,canvas.height/2);
			//把对象存入数组
			particles.push(part);
			for(var i=0;i<particles.length;i++)
			{	//更新粒子
				particles[i].upDate();
				
			}
		},30);
	}
	//画一个粒子
	function Particle(xPos,yPos)
	{	//当前粒子圆心的x坐标，y坐标
		this.xPos=xPos;
		this.yPos=yPos;
		//y方向的变化量
		this.yVal=-7;
		this.xVal=Math.random()*6-3;
		//定义重力
		this.gravity=0.1;
		//画圆
		this.draw=function()
		{	//开始路径
			context.beginPath();
			context.arc(this.xPos,this.yPos,5,0,2*Math.PI);
			//结束路径
			context.closePath();
			//填充颜色
			context.fill();
		}
		//更新圆的位置和颜色
		this.upDate=function()
		{	//更新圆心位置
			this.yPos+=this.yVal;
			this.xPos+=this.xVal;
			this.yVal+=this.gravity;
			//画笔颜色
			context.fillStyle=getRandomColor();
			//每次重新画
			this.draw()
		}
	}
	//封装颜色
	function getRandomColor()
	{	//将十进制颜色转化为十六进制
		return "#"+Math.floor(Math.random()*16777215).toString(16);
		
	}
}
