#pragma once
#include "ParticleSystem.h"

class ScatteredParticle:public ParticleSystem
{
public:
	void Init(const char* picPath);
	virtual	void Update();
protected:
private:
};