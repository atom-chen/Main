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
	static unsigned long lastTime , timeSinceComputerStar;//上次渲染时间，
	static unsigned long frameTime;//当前帧消耗时间
};