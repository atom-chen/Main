#pragma once
#include "ImageSprite.h"

class Particle:public ImageSprite
{
public:
	bool mbRoot;
	Particle(bool bRoot=false);
	void Update(float deltaTime);
	void Draw();
};