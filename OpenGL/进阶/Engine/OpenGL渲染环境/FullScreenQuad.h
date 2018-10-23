#pragma once
#include "ggl.h"
#include "RenderAble.h"

class FullScreenQuad:public RenderAble
{
public:
	void Init(const char* vertShader = SHADER_ROOT"fullScreenQuad.vert", const char* fragShader = SHADER_ROOT"fullScreenQuad.frag");
	void Draw();
	void DrawToLeftTop();
	void DrawToRightTop();
	void DrawToLeftBottom();
	void DrawToRightBottom();
protected:
private:
};