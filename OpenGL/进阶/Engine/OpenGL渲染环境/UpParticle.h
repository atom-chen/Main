#pragma once
#include "ParticleSystem.h"

#define DELAYTIME 0.02f

//index ��ʾǰindex*4->index*4+3��������Ҫ������
class UpParticle :public ParticleSystem
{
public:
	UpParticle();
	void Init(const vec3& pos);
	void Update();
	//void Draw();
	//void Destory();
private:
	float m_StartLiftTime=15;//ÿ�����Ӵ��ʱ��
	int m_MaxPar=1550;//���������
	float lastTime = 0;
	float m_LastChangeTime = 0;
	int m_InLife = 0;

};
