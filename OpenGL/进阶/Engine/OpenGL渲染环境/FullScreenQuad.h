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
	void SetColor(const vec4& color)
	{
		INIT_TEST_VOID
			m_VertexBuf.SetColor(0, color);
		m_VertexBuf.SetColor(1, color);
		m_VertexBuf.SetColor(2, color);
		m_VertexBuf.SetColor(3, color);
	}
public:

protected:
private:
};