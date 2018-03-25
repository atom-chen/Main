#pragma once
#include "ImageSprite.h"
#include <functional>

class Button : public RenderableObject
{
public:
	Button();
	ImageSprite*mDefaultSprite;
	float mMinX, mMaxX, mMinY, mMaxY;
	float mOffsetY;
	std::function<void()> mClickHandler;
	Button* OnTouchBegin(int x,int y);
	Button* OnTouchEnd(int x,int y);
	void ResetState();
	void SetOnClick(std::function<void()> foo);
	void SetRect(float x,float y,float width,float height);
	void SetDefaultSprite(ImageSprite*sprite);
	void Draw();
};