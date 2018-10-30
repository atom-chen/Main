#include "FullScreenQuad.h"
#include "SceneManager.h"

void FullScreenQuad::Init(const char* vertShader, const char* fragShader)
{
	if (m_IsInit)
	{
		return;
	}
	m_IsInit = true;
	m_VertexBuf.Init(4);
	m_VertexBuf.SetTexcoord(0, 0, 0);
	m_VertexBuf.SetTexcoord(1, 1, 0);
	m_VertexBuf.SetTexcoord(2, 0, 1);
	m_VertexBuf.SetTexcoord(3, 1, 1);
	m_Options.DrawType = DRAW_TRIANGLES_STRIP;
	glm::mat4 identityMat(
		1, 0, 0, 0,
		0, 1, 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1
		);
	m_Shader.Init(vertShader, fragShader);
	m_Shader.SetMatrix("ModelMatrix", identityMat);
	m_Shader.SetMatrix("ViewMatrix", identityMat);
	m_Shader.SetMatrix("ProjectionMatrix", identityMat);
	this->SetColor(COLOR_WHITE);
}
void FullScreenQuad::Draw()
{
	INIT_TEST_VOID
	m_VertexBuf.SetPosition(0, -1, -1, 0);
	m_VertexBuf.SetPosition(1, 1, -1, 0);
	m_VertexBuf.SetPosition(2, -1, 1, 0);
	m_VertexBuf.SetPosition(3, 1, 1, 0);
	SceneManager::DrawGameObject(this);
}
void FullScreenQuad::DrawToLeftTop()
{
	INIT_TEST_VOID
	m_VertexBuf.SetPosition(0, -1, 0, -1);
	m_VertexBuf.SetPosition(1, 0, 0, -1);
	m_VertexBuf.SetPosition(2, -1, 1, -1);
	m_VertexBuf.SetPosition(3, 0, 1, -1);
	SceneManager::DrawGameObject(this);
}
void FullScreenQuad::DrawToLeftBottom()
{
	INIT_TEST_VOID
	m_VertexBuf.SetPosition(0, -1, -1, -1);
	m_VertexBuf.SetPosition(1, 0, -1, -1);
	m_VertexBuf.SetPosition(2, -1, 0, -1);
	m_VertexBuf.SetPosition(3, 0, 0, -1);
	SceneManager::DrawGameObject(this);
}
void FullScreenQuad::DrawToRightTop()
{
	INIT_TEST_VOID
	m_VertexBuf.SetPosition(0, 0, 0, -1);
	m_VertexBuf.SetPosition(1, 1, 0, -1);
	m_VertexBuf.SetPosition(2, 0, 1, -1);
	m_VertexBuf.SetPosition(3, 1, 1, -1);
	SceneManager::DrawGameObject(this);
}

void FullScreenQuad::DrawToRightBottom()
{
	INIT_TEST_VOID
	m_VertexBuf.SetPosition(0, 0, -1, -1);
	m_VertexBuf.SetPosition(1, 1, -1, -1);
	m_VertexBuf.SetPosition(2, 0, 0, -1);
	m_VertexBuf.SetPosition(3, 1, 0, -1);
	SceneManager::DrawGameObject(this);
}
