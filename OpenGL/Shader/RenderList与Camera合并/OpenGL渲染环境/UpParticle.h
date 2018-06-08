#pragma once
#include "ParticleSystem.h"

#define DELAYTIME 0.02f

//index 表示前index*4->index*4+3个粒子需要被更新
class UpParticle :public ParticleSystem
{
public:
	UpParticle();
	void Init(const vec3& pos);
	void Update();
	//void Draw();
	//void Destory();
private:
	float m_StartLiftTime=15;//每个粒子存活时间
	int m_MaxPar=1550;//最大粒子数
	float lastTime = 0;
	float m_LastChangeTime = 0;
	int m_InLife = 0;

};
