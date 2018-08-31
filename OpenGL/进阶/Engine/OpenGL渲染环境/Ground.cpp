#include "Ground.h"
#include "SceneManager.h"

Ground::Ground()
{
	m_IsInit = 0;
	height = -10;
}
bool Ground::Init()
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		m_VertexBuf.Init(4);

		//起始坐标
		int zStart = 100;
		int xStart = -100;
		int length = 100;

		m_VertexBuf.SetPosition(0, xStart, height, zStart);
		m_VertexBuf.SetPosition(1, xStart + length, height, zStart);
		m_VertexBuf.SetPosition(2, xStart, height, zStart - length);
		m_VertexBuf.SetPosition(3, xStart + length, height, zStart - length);

		m_VertexBuf.SetNormal(0, 0, 1, 0);
		m_VertexBuf.SetNormal(1, 0, 1, 0);
		m_VertexBuf.SetNormal(2, 0, 1, 0);
		m_VertexBuf.SetNormal(3, 0, 1, 0);
		m_VertexBuf.SetColor(0, 0.4f, 0.4f, 0.4f);
		m_VertexBuf.SetColor(1, 0.4f, 0.4f, 0.4f);
		m_VertexBuf.SetColor(2, 0.4f, 0.4f, 0.4f);
		m_VertexBuf.SetColor(3, 0.4f, 0.4f, 0.4f);

		m_Shader.Init(SHADER_ROOT"ground.vert", SHADER_ROOT"ground.frag");
		SetAmbientMaterial(0.1f, 0.1f, 0.1f, 1);
		SetDiffuseMaterial(0.6f, 0.6f, 0.6f, 1);
		SetSpecularMaterial(0, 0, 0, 0);
	}
	return 1;
}
bool Ground::Init(const char* picName)
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		m_VertexBuf.Init(4);

		//起始坐标
		int size = 100;
		int rept = 100;

		m_VertexBuf.SetPosition(0, -size, -0.5f, size);
		m_VertexBuf.SetTexcoord(0, 0, 0);
		m_VertexBuf.SetPosition(1, size, -0.5f, size);
		m_VertexBuf.SetTexcoord(1, rept, 0);
		m_VertexBuf.SetPosition(2, -size, height, -size);
		m_VertexBuf.SetTexcoord(2, rept, rept);
		m_VertexBuf.SetPosition(3, size, height, -size);
		m_VertexBuf.SetTexcoord(3, 0, rept);


		m_VertexBuf.SetNormal(0, 0, 1, 0);
		m_VertexBuf.SetNormal(1, 0, 1, 0);
		m_VertexBuf.SetNormal(2, 0, 1, 0);
		m_VertexBuf.SetNormal(3, 0, 1, 0);
		m_VertexBuf.SetColor(0, 1, 1, 1, 1);
		m_VertexBuf.SetColor(1, 1, 1, 1, 1);
		m_VertexBuf.SetColor(2, 1, 1, 1, 1);
		m_VertexBuf.SetColor(3, 1, 1, 1, 1);
		
		m_Shader.Init(SHADER_ROOT"ground.vert", SHADER_ROOT"ground.frag");
		SetTexture2D(picName);
		SetAmbientMaterial(1, 1, 1);
		SetDiffuseMaterial(0.2f, 0.2f, 0.2f);
		SetSpecularMaterial(0, 0, 0);
		m_Options.DepthTest = 1;
		m_Options.DrawType = DRAW_TRIANGLES_STRIP;
	}
	return 1;
}


