#pragma once
#include "ParticleSystem.h"

class SurroundParticle:public ParticleSystem
{
public:
	virtual void Init(const vec3& position, const int& maxNum = 180, const char* picPath = "res/test.bmp");
public:
	virtual void Update();
	inline void SetPointSize(float size)
	{
		m_Shader.SetFloat("U_PointSize", size);
	}
private:
	float angle = 0.0f;
	float m_PointSize = 20;
	int m_MaxPar;//最大粒子数
};

