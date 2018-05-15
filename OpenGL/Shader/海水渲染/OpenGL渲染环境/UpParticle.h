#pragma once
#include "ParticleSystem.h"

class UpParticle :public ParticleSystem
{
public:
public:
	UpParticle();
public:
	//void Init(GLubyte r, GLubyte g, GLubyte b, GLubyte a, float life);
	void SetHalfSize(float halfSize);
	void Update();
protected:
public:
	float m_RotateSpeed = 100;//ËÙ¶È
private:
	float m_HalfSize = 0;//Á£×Ó°ë¾¶
	vec4 m_Color;
protected:
private:
	
};