#pragma once
#include "ggl.h"
#include "SceneManager.h"

class Time
{
friend class SceneManager;
public:
	static float DeltaTime();
protected:
private:
	Time(){}
	static void SetDeltaTime();
private:
	static unsigned long lastTime , timeSinceComputerStar;//�ϴ���Ⱦʱ�䣬
	static unsigned long frameTime;//��ǰ֡����ʱ��
};