#pragma once
#include "ggl.h"
#include "RenderAble.h"

class FullScreenQuad:public RenderAble
{
public:
	void Init();
	void Draw();
	void DrawToLeftTop();
	void DrawToRightTop();
	void DrawToLeftBottom();
	void DrawToRightBottom();
protected:
private:
};