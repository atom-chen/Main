#include "FullScreenQuad.h"
#include "SceneManager.h"


void FullScreenQuad::Init()
{
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
	m_Shader.Init("res/fullScreenQuad.vert", "res/fullScreenQuad.frag");
	m_Shader.SetMatrix("ModelMatrix", identityMat);
	m_Shader.SetMatrix("ViewMatrix", identityMat);
	m_Shader.SetMatrix("ProjectionMatrix", identityMat);
}
void FullScreenQuad::Draw()
{
	m_VertexBuf.SetPosition(0, -1, -1, 0);
	m_VertexBuf.SetPosition(1, 1, -1, 0);
	m_VertexBuf.SetPosition(2, -1, 1, 0);
	m_VertexBuf.SetPosition(3, 1, 1, 0);
	SceneManager::DrawGameObject(this);
}
void FullScreenQuad::DrawToLeftTop()
{
	m_VertexBuf.SetPosition(0, -1, 0, -1);
	m_VertexBuf.SetPosition(1, 0, 0, -1);
	m_VertexBuf.SetPosition(2, -1, 1, -1);
	m_VertexBuf.SetPosition(3, 0, 1, -1);
	SceneManager::DrawGameObject(this);
}
void FullScreenQuad::DrawToLeftBottom()
{
	m_VertexBuf.SetPosition(0, -1, -1, -1);
	m_VertexBuf.SetPosition(1, 0, -1, -1);
	m_VertexBuf.SetPosition(2, -1, 0, -1);
	m_VertexBuf.SetPosition(3, 0, 0, -1);
	SceneManager::DrawGameObject(this);
}
void FullScreenQuad::DrawToRightTop()
{
	m_VertexBuf.SetPosition(0, 0, 0, -1);
	m_VertexBuf.SetPosition(1, 1, 0, -1);
	m_VertexBuf.SetPosition(2, 0, 1, -1);
	m_VertexBuf.SetPosition(3, 1, 1, -1);
	SceneManager::DrawGameObject(this);
}

void FullScreenQuad::DrawToRightBottom()
{
	m_VertexBuf.SetPosition(0, 0, -1, -1);
	m_VertexBuf.SetPosition(1, 1, -1, -1);
	m_VertexBuf.SetPosition(2, 0, 0, -1);
	m_VertexBuf.SetPosition(3, 1, 0, -1);
	SceneManager::DrawGameObject(this);
}