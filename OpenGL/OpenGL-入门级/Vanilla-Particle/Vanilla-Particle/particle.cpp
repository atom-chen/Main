#include "particle.h"

Particle::Particle(bool bRoot/* =false */) :mbRoot(bRoot)
{

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
		mPos.y += 10.0f*deltaTime;
	}

	if (mNext != nullptr)
	{
		Next<Particle>()->Update(deltaTime);
	}
}