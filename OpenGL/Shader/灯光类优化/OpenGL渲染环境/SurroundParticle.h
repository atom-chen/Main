#pragma once
#include "ParticleSystem.h"

class SurroundParticle:public ParticleSystem
{
public:
	virtual void Init(const vec3& position, const int& maxNum = 180, const char* picPath = "res/test.bmp");
public:
	virtual void Update();
private:
	float angle = 0.0f;
};

