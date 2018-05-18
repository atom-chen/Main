#pragma once
#include "ImageSprite.h"

class Particle:public ImageSprite
{
public:
	bool mbRoot;
	float mLifeTime, mExistTime;
	Vector3f mOriginalPos;
	Particle(bool bRoot=false);
	void Update(float deltaTime);
	void Draw();
};