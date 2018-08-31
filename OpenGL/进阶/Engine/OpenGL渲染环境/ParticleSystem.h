#pragma once
#include "RenderAble.h"

#define DELAYTIME 0.1f


class ParticleSystem:public RenderAble
{
public:
	ParticleSystem();
public:
	virtual void Update();
protected:

};
