#include "Time.h"


unsigned long Time::lastTime = 0, Time::timeSinceComputerStar = 0;//�ϴ���Ⱦʱ��
unsigned long Time::frameTime;
float Time::DeltaTime()
{
	return float(frameTime) / 1000.0f;
}
void Time::SetDeltaTime()
{
	timeSinceComputerStar = timeGetTime();//��ǰʱ��
	frameTime = lastTime == 0 ? 0 : timeSinceComputerStar - lastTime;
	//����ǵ�һ������
	if (lastTime != 0)
	{
		frameTime = timeSinceComputerStar - lastTime;
	}
	lastTime = timeSinceComputerStar;
}