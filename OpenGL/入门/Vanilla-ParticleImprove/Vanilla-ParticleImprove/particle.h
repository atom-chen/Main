#pragma once
#include "ImageSprite.h"

class Particle:public ImageSprite
{
public:
	bool mbRoot;
	float mLifeTime, mExistTime;
	float mDelayTime;
	Vector3f mOriginalPos;
	Particle(bool bRoot=false);
	void Update(float deltaTime);
	void Draw();
	static void InitSpeeds();
};