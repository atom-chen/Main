#include "particle.h"
#include "utils.h"

static Vector3f speeds[100];

void Particle::InitSpeeds()
{
	for (int i=0;i<100;++i)
	{
		speeds[i] = Vector3f(srandf(),randf(),0.0f);
	}
}

Particle::Particle(bool bRoot/* =false */) :mbRoot(bRoot)
{
	mLifeTime = 4.0f + 2.0f*randf();
	mDelayTime = 4.0f*randf();
	mExistTime = 0.0f;
	mOriginalPos.x = 640.0f*srandf();//-640.0f~640.0f
	mOriginalPos.y = -360.0f + 20.0f*randf();//-360.0f~-340.0f
	mPos = mOriginalPos;
	mR = 120;
	mG = 80;
	mB = 25;
	mDstFactor = GL_ONE;
	mAlpha = 0;
}

void Particle::Draw()
{
	if (!mbRoot)
	{
		ImageSprite::Draw();
	}
	if (mNext!=nullptr)
	{
		Next<Particle>()->Draw();
	}
}

void Particle::Update(float deltaTime)
{
	if (!mbRoot)
	{
		mExistTime += deltaTime;
		if (mExistTime>mDelayTime)
		{
			mAlpha = 255;
			int speedIndex = int(100 * randf());
			speedIndex = speedIndex >= 100 ? 99 : speedIndex;
			mPos = mPos + speeds[speedIndex] * deltaTime*100.0f;
			if (mExistTime > (mLifeTime+mDelayTime))
			{
				mPos = mOriginalPos;
				mExistTime = 0.0f;
			}
		}
	}

	if (mNext != nullptr)
	{
		Next<Particle>()->Update(deltaTime);
	}
}