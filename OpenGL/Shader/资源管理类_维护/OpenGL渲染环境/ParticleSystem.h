#pragma once
#include "RenderAble.h"

#define DELAYTIME 0.1f


class ParticleSystem:public RenderAble
{
public:
	virtual void Init(const vec3& position, const int& maxNum = 180, const char* picPath = "res/test.bmp");
public:
	virtual void Update();
	void Draw();
	virtual void Destory();
protected:
	float m_StartLiftTime=15;//每个粒子存活时间
	int32_t m_MaxPar;//最大粒子数
	float angle = 0.0f;
};
