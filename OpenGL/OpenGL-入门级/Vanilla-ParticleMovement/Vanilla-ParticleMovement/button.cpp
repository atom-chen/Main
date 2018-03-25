#include "button.h"


Button::Button() :mOffsetY(0.0f),mClickHandler(nullptr)
{

}

Button*Button::OnTouchBegin(int x, int y)
{
	if (x>mMinX&&x<mMaxX&&
		y>mMinY&&y<mMaxY)
	{
		mOffsetY = -4.0f;
		return this;
	}
	Button*ret = nullptr;
	if (mNext!=nullptr)
	{
		ret=Next<Button>()->OnTouchBegin(x, y);
	}
	return ret;
}

Button* Button::OnTouchEnd(int x, int y)
{
	if (x > mMinX&&x<mMaxX&&
		y>mMinY&&y < mMaxY)
	{
		if (mClickHandler != nullptr)
		{
			mClickHandler();
		}
		return this;
	}
	Button*ret = nullptr;
	if (mNext != nullptr)
	{
		ret = Next<Button>()->OnTouchEnd(x, y);
	}
	return ret;
}

void Button::ResetState()
{
	mOffsetY = 0.0f;//
	if (mNext != nullptr)
	{
		Next<Button>()->ResetState();
	}
}

void Button::SetOnClick(std::function<void()> foo)
{
	mClickHandler = foo;
}

void Button::SetRect(float x, float y, float width, float height)
{
	mMinX = x - width / 2;
	mMaxX = x + width / 2;
	mMinY = y - height / 2;
	mMaxY = y + height / 2;
	mDefaultSprite->SetRect(x, y, width, height);
}

void Button::SetDefaultSprite(ImageSprite*sprite)
{
	mDefaultSprite = sprite;
}

void Button::Draw()
{
	//draw
	glPushMatrix();
	glTranslatef(0.0f, mOffsetY, 0.0f);
	mDefaultSprite->Draw();
	glPopMatrix();
	RenderableObject::Draw();
}