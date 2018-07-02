var vv=document.getElementById("box");
var time=0;
var size=200;

	vv.timer=setInterval(function()
	{
		if(size<=500)
		{
			size+=50;
			vv.style.weight=size+"px";
			vv.style.height=size+"px";
		}
		if(time>=10)
		{
			clearInterval(vv.timer);
		}
		if(time%2==0)
		{
			vv.style.backgroundColor="greenyellow";
			time++;
		}else
		{
			vv.style.backgroundColor="coral";
			time++;
		}
		
	}
	,1000);
		
		




