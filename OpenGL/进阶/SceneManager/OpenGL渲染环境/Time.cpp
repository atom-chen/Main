#include "Time.h"


unsigned long Time::lastTime = 0, Time::timeSinceComputerStar = 0;//上次渲染时间
unsigned long Time::frameTime;
float Time::DeltaTime()
{
	return float(frameTime) / 1000.0f;
}
void Time::SetDeltaTime()
{
	timeSinceComputerStar = timeGetTime();//当前时间
	frameTime = lastTime == 0 ? 0 : timeSinceComputerStar - lastTime;
	//如果是第一次启动
	if (lastTime != 0)
	{
		frameTime = timeSinceComputerStar - lastTime;
	}
	lastTime = timeSinceComputerStar;
}