#include "ImageSprite.h"

ImageSprite::ImageSprite():mAlpha(255),mbFadeIn(false),mbFadeOut(false)
{
	mR = 255;
	mG = 255;
	mB = 255;
	mDstFactor = GL_ONE_MINUS_SRC_ALPHA;
}

void ImageSprite::SetTexture(Texture*texture)
{
	mTexture = texture;
}

void ImageSprite::SetRect(float x, float y, float width, float height)
{
	float halfWidth = width / 2.0f;
	float halfHeight = height / 2.0f;
	mMesh[0].x = x - halfWidth;
	mMesh[0].y = y - halfHeight;

	mMesh[1].x = x + halfWidth;
	mMesh[1].y = y - halfHeight;

	mMesh[2].x = x + halfWidth;
	mMesh[2].y = y + halfHeight;

	mMesh[3].x = x - halfWidth;
	mMesh[3].y = y + halfHeight;
}

void ImageSprite::Draw()
{
	glEnable(GL_TEXTURE_2D);
	glDisable(GL_LIGHTING);
	glDisable(GL_DEPTH_TEST);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, mDstFactor);
	glPushMatrix();
	glTranslatef(mPos.x, mPos.y, mPos.z);
	glColor4ub(mR, mG, mB, mAlpha);
	glBindTexture(GL_TEXTURE_2D, mTexture->mTextureID);
	glBegin(GL_QUADS);
	glTexCoord2f(0.0f, 0.0f);
	glVertex3fv(mMesh[0].v);
	glTexCoord2f(1.0f, 0.0f);
	glVertex3fv(mMesh[1].v);
	glTexCoord2f(1.0f, 1.0f);
	glVertex3fv(mMesh[2].v);
	glTexCoord2f(0.0f, 1.0f);
	glVertex3fv(mMesh[3].v);
	glEnd();
	glPopMatrix();
	glDisable(GL_BLEND);
}

void ImageSprite::Update(float deltaTime)
{
	if (mbFadeIn)
	{
		int alpha = mAlpha;
		alpha -= int(mFadeSpeed*deltaTime);
		mAlpha = alpha < 0 ? 0 : alpha;
		if (mAlpha==0)
		{
			mbFadeIn = false;
		}
	}
	else if (mbFadeOut)
	{
		int alpha = mAlpha;
		alpha += int(mFadeSpeed*deltaTime);
		mAlpha = alpha >255 ? 255 : alpha;
		if (mAlpha==255)
		{
			mbFadeOut = false;
		}
	}
}

void ImageSprite::FadeIn(float duration)
{
	if (!(mbFadeIn||mbFadeOut))
	{
		mbFadeIn = true;
		mFadeSpeed = int(255.0f/duration);
	}
}

void ImageSprite::FadeOut(float duration)
{
	if (!(mbFadeIn || mbFadeOut))
	{
		mbFadeOut = true;
		mFadeSpeed = int(255.0f / duration);
	}
}