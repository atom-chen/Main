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
		int32_t forwardIndex = 0;
		this->m_VertexBuf[forwardIndex].Init(4);
		this->m_VertexBuf[forwardIndex].SetPosition(0, -0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[forwardIndex].SetPosition(1, 0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[forwardIndex].SetPosition(2, 0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[forwardIndex].SetPosition(3, -0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[forwardIndex].SetTexcoord(3, 0, 1);

		m_Shader[forwardIndex].Init("res/skybox.vert", "res/skybox.frag");
		m_Shader[forwardIndex].SetTexture2D(picPath);
	}
}
void SkyBox::SetBack(const char* picPath)
{
	if (m_IsInit)
	{
		int32_t backIndex = 1;
		this->m_VertexBuf[backIndex].Init(4);
		this->m_VertexBuf[backIndex].SetPosition(0, 0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[backIndex].SetPosition(1, -0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[backIndex].SetPosition(2, -0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[backIndex].SetPosition(3, 0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[backIndex].SetTexcoord(3, 0, 1);

		m_Shader[backIndex].Init("res/skybox.vert", "res/skybox.frag");
		m_Shader[backIndex].SetTexture2D(picPath);
	}
}

void SkyBox::SetLeft(const char* picPath)
{
	if (m_IsInit)
	{
		int32_t leftIndex = 4;
		this->m_VertexBuf[leftIndex].Init(4);
		this->m_VertexBuf[leftIndex].SetPosition(0, -0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[leftIndex].SetPosition(1, -0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[leftIndex].SetPosition(2, -0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[leftIndex].SetPosition(3, -0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[leftIndex].SetTexcoord(3, 0, 1);

		m_Shader[leftIndex].Init("res/skybox.vert", "res/skybox.frag");
		m_Shader[leftIndex].SetTexture2D(picPath);
	}
}
void SkyBox::SetRight(const char* picPath)
{
	if (m_IsInit)
	{
		int32_t rightIndex = 5;
		this->m_VertexBuf[rightIndex].Init(4);
		this->m_VertexBuf[rightIndex].SetPosition(0, 0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[rightIndex].SetPosition(1, 0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[rightIndex].SetPosition(2, 0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[rightIndex].SetPosition(3, 0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[rightIndex].SetTexcoord(3, 0, 1);

		m_Shader[rightIndex].Init("res/skybox.vert", "res/skybox.frag");
		m_Shader[rightIndex].SetTexture2D(picPath);
	}
}
void SkyBox::SetTop(const char* picPath)
{
	if (m_IsInit)
	{
		int32_t bottomIndex = 2;
		this->m_VertexBuf[bottomIndex].Init(4);
		this->m_VertexBuf[bottomIndex].SetPosition(0, -0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(1, 0.5f, 0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(2, 0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[bottomIndex].SetPosition(3, -0.5f, 0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(3, 0, 1);

		m_Shader[bottomIndex].Init("res/skybox.vert", "res/skybox.frag");
		m_Shader[bottomIndex].SetTexture2D(picPath);
	}
}
void SkyBox::SetBottom(const char* picPath)
{
	if (m_IsInit)
	{
		int32_t bottomIndex = 3;
		this->m_VertexBuf[bottomIndex].Init(4);
		this->m_VertexBuf[bottomIndex].SetPosition(0, -0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(0, 0, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(1, 0.5f, -0.5f, 0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(1, 1, 0);
		this->m_VertexBuf[bottomIndex].SetPosition(2, 0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(2, 1, 1);
		this->m_VertexBuf[bottomIndex].SetPosition(3, -0.5f, -0.5f, -0.5f);
		this->m_VertexBuf[bottomIndex].SetTexcoord(3, 0, 1);

		m_Shader[bottomIndex].Init("res/skybox.vert", "res/skybox.frag");
		m_Shader[bottomIndex].SetTexture2D(picPath);
	}
}

void SkyBox::Update(const vec3& cameraPos)
{
	if (m_IsInit)
	{
		this->m_ModelMatrix = translate(cameraPos);
	}
}
void SkyBox::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	if (m_IsInit)
	{
		SceneManager::SetDepthTestState(0);
		for (int i = 0; i < 6; i++)
		{
			m_VertexBuf[i].Begin();
			{
				m_Shader[i].Begin();
				{
					m_Shader[i].Bind(glm::value_ptr(m_ModelMatrix), glm::value_ptr(viewMatrix), glm::value_ptr(ProjectionMatrix));
					glDrawArrays(GL_QUADS, 0, 4);
				}
				m_Shader[i].End();
			}
			m_VertexBuf[i].End();
		}
	}
}