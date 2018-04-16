#include "Camera_2D.h"
#include "Utils.h"
#include "Resource.h"
#include "RenderOptions.h"
#include "SceneManager.h"

Camera_2D::Camera_2D()
{
	
}

void Camera_2D::Init(FrameBuffer &fbo, const char* bufferName)
{
	//×ø±ê×ª»»
	m_VertexBuf.Init(4);
	this->SetPosition(-1, -1, 2, 2);
	m_Shader.Init("res/fullScreenQuad.vert", "res/gray.frag");
	m_Texture = fbo.GetBuffer(bufferName);
	m_Shader.SetTexture2D(m_Texture);
}
void Camera_2D::Destory()
{
	m_Shader.Destory();
	m_VertexBuf.Destory();
}
void Camera_2D::Draw()
{
	AlphaBlendInfo Info;
	Info.AlphaBlend = 1;
	Info.Type = ALPHA_BLEND_ONE_MINUS_SRC_ALPHA;
	SceneManager::SetBlendState(Info);
	BEGIN
		glDrawArrays(GL_TRIANGLE_STRIP, 0, VBO_NAME.GetLenth());
	END
}

void Camera_2D::SetPosition(float x, float y, float width, float height)
{
	VBO_NAME.SetPosition(0, x, y, -0.2f);
	VBO_NAME.SetTexcoord(0, 0, 0);
	VBO_NAME.SetColor(0, 1, 1, 1);

	VBO_NAME.SetPosition(1, x + width, y, -0.2f);
	VBO_NAME.SetTexcoord(1, 1, 0);
	VBO_NAME.SetColor(1, 1, 1, 1);

	VBO_NAME.SetPosition(2, x, y + height, -0.2f);
	VBO_NAME.SetTexcoord(2, 0, 1);
	VBO_NAME.SetColor(2, 1, 1, 1);

	VBO_NAME.SetPosition(3, x + width, y + height, -0.2f);
	VBO_NAME.SetTexcoord(3, 1, 1);
	VBO_NAME.SetColor(3, 1, 1, 1);
}