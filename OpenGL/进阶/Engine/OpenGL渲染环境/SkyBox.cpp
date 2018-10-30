#include "SkyBox.h"
#include "Utils.h"
#include "SceneManager.h"
SkyBox::SkyBox()
{
	
}

bool SkyBox::Init(const char* forwardPath, const char* backPath, const char* topPath, const char* bottomPath, const char* leftPath, const char* rightPath)
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		this->SetForward(forwardPath);
		this->SetBack(backPath);
		this->SetTop(topPath);
		this->SetBottom(bottomPath);
		this->SetLeft(leftPath);
		this->SetRight(rightPath);
	}
	return 1;
}
void SkyBox::SetForward(const char* picPath)
{
	if (m_IsInit)
	{
		int forwardIndex = 0;
		this->m_VertexBuf[forwardIndex].Init(4);
		this->m_VertexBuf[forwardIndex].SetPosition(0, -0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[forwardIndex].SetPosition(1, 0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[forwardIndex].SetPosition(2, 0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[forwardIndex].SetPosition(3, -0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(3, 0, 1);

		m_Shader[forwardIndex].Init(SHADER_ROOT"skybox.vert", SHADER_ROOT"skybox.frag");
		m_Shader[forwardIndex].SetTexture2D(picPath,0);
	}
}
void SkyBox::SetBack(const char* picPath)
{
	if (m_IsInit)
	{
		int backIndex = 1;
		this->m_VertexBuf[backIndex].Init(4);
		this->m_VertexBuf[backIndex].SetPosition(0, 0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[backIndex].SetPosition(1, -0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[backIndex].SetPosition(2, -0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[backIndex].SetPosition(3, 0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(3, 0, 1);

		m_Shader[backIndex].Init(SHADER_ROOT"skybox.vert", SHADER_ROOT"skybox.frag");
		m_Shader[backIndex].SetTexture2D(picPath,0);
	}
}

void SkyBox::SetLeft(const char* picPath)
{
	if (m_IsInit)
	{
		int leftIndex = 4;
		this->m_VertexBuf[leftIndex].Init(4);
		this->m_VertexBuf[leftIndex].SetPosition(0, -0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[leftIndex].SetPosition(1, -0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[leftIndex].SetPosition(2, -0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[leftIndex].SetPosition(3, -0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(3, 0, 1);

		m_Shader[leftIndex].Init(SHADER_ROOT"skybox.vert", SHADER_ROOT"skybox.frag");
		m_Shader[leftIndex].SetTexture2D(picPath,0);
	}
}
void SkyBox::SetRight(const char* picPath)
{
	if (m_IsInit)
	{
		int rightIndex = 5;
		this->m_VertexBuf[rightIndex].Init(4);
		this->m_VertexBuf[rightIndex].SetPosition(0, 0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[rightIndex].SetPosition(1, 0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[rightIndex].SetPosition(2, 0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[rightIndex].SetPosition(3, 0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(3, 0, 1);

		m_Shader[rightIndex].Init(SHADER_ROOT"skybox.vert", SHADER_ROOT"skybox.frag");
		m_Shader[rightIndex].SetTexture2D(picPath,0);
	}
}
void SkyBox::SetTop(const char* picPath)
{
	if (m_IsInit)
	{
		int bottomIndex = 2;
		this->m_VertexBuf[bottomIndex].Init(4);
		this->m_VertexBuf[bottomIndex].SetPosition(0, -0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(1, 0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(2, 0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[bottomIndex].SetPosition(3, -0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(3, 0, 1);

		m_Shader[bottomIndex].Init(SHADER_ROOT"skybox.vert", SHADER_ROOT"skybox.frag");
		m_Shader[bottomIndex].SetTexture2D(picPath,0);
	}
}
void SkyBox::SetBottom(const char* picPath)
{
	if (m_IsInit)
	{
		int bottomIndex = 3;
		this->m_VertexBuf[bottomIndex].Init(4);
		this->m_VertexBuf[bottomIndex].SetPosition(0, -0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(1, 0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(2, 0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[bottomIndex].SetPosition(3, -0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(3, 0, 1);

		m_Shader[bottomIndex].Init(SHADER_ROOT"skybox.vert", SHADER_ROOT"skybox.frag");
		m_Shader[bottomIndex].SetTexture2D(picPath,0);
	}
}

void SkyBox::Update(const vec3& cameraPos)
{
	if (m_IsInit)
	{
		this->m_ModelMatrix = translate(cameraPos);
	}
}
void SkyBox::Draw()
{
	if (m_IsInit)
	{
		for (int i = 0; i < ARRLEN(m_VertexBuf); i++)
		{
			RenderDomain domain(m_VertexBuf[i], m_Shader[i], m_ModelMatrix);
			domain.options.DrawType = DRAW_QUADS;
			domain.options.DepthTest = 0;
			SceneManager::DrawGameObject(domain);
		}
	}
}
void SkyBox::Destroy()
{
	if (m_IsInit)
	{
		for (int i = 0; i < ARRLEN(m_VertexBuf); i++)
		{
			m_VertexBuf[i].Destory();
			m_Shader[i].Destory();
		}
		m_IsInit = 0;
	}
}