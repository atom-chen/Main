#pragma once
#include "ggl.h"
#include "WinApp.h"

class Time
{
friend class EngineBehavior;
public:
	static float DeltaTime();
protected:
private:
	static void SetDeltaTime();
private:
	static unsigned long lastTime , timeSinceComputerStar;//�ϴ���Ⱦʱ�䣬
	static unsigned long frameTime;
};