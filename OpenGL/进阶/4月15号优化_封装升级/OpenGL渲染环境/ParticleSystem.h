#pragma once
#include "RenderAble.h"

#define DELAYTIME 0.1f


class ParticleSystem:public RenderAble
{
public:
	void Init(const vec3& position);
public:
	void Update();
	void Draw();
private:
	float m_StartLiftTime=15;//每个粒子存活时间
	int32_t m_MaxPar=180;//最大粒子数
	vec3 m_Position;//粒子系统的位置
	GLuint m_Texture;//粒子系统所用纹理
};
