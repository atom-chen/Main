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
protected:
	float m_StartLiftTime=15;//ÿ�����Ӵ��ʱ��
	int32_t m_MaxPar;//���������
	float angle = 0.0f;
};
