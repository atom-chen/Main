#include "particle.h"
#include "utils.h"

Particle::Particle(bool bRoot/* =false */) :mbRoot(bRoot)
{
	mLifeTime = 4.0f + 2.0f*randf();
	mExistTime = 0.0f;
	mOriginalPos.x = 640.0f*srandf();//-640.0f~640.0f
	mOriginalPos.y = -360.0f + 20.0f*randf();//-360.0f~-340.0f
	mPos = mOriginalPos;
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
		mPos.y += 40.0f*deltaTime;
		mExistTime += deltaTime;
		if (mExistTime>mLifeTime)
		{
			mPos = mOriginalPos;
			mExistTime = 0.0f;
		}
	}

	if (mNext != nullptr)
	{
		Next<Particle>()->Update(deltaTime);
	}
}