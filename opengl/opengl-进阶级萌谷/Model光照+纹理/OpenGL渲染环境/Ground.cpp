#include "Ground.h"

Ground::~Ground()
{

}
bool Ground::Init()
{
	m_VertexBuf.Init(1600);
	for (int32_t z = 0; z < 20; z++)
	{
		//ÆðÊ¼×ø±ê
		float zStart = static_cast<float>(100 - z * 10);
		for (int32_t x = 0; x < 20; x++)
		{
			float xStart = static_cast<float>(x * 10 - 100);
			int32_t offset = (x + z * 20) * 4;

			m_VertexBuf.SetPosition(offset, xStart, -1, zStart);
			m_VertexBuf.SetNormal(offset, 0, 1, 0);

			m_VertexBuf.SetPosition(offset + 1, xStart + 10, -1, zStart);
			m_VertexBuf.SetNormal(offset + 1, 0, 1, 0);

			m_VertexBuf.SetPosition(offset + 2, xStart, -1, zStart - 10);
			m_VertexBuf.SetNormal(offset + 2, 0, 1, 0);

			m_VertexBuf.SetPosition(offset + 3, xStart + 10, -1, zStart - 10);
			m_VertexBuf.SetNormal(offset + 3, 0, 1, 0);

			if ((x % 2) == 0 && (z % 2) == 0)
			{
				m_VertexBuf.SetColor(offset, 0.1f, 0.1f, 0.1f);
				m_VertexBuf.SetColor(offset + 1, 0.1f, 0.1f, 0.1f);
				m_VertexBuf.SetColor(offset + 2, 0.1f, 0.1f, 0.1f);
				m_VertexBuf.SetColor(offset + 3, 0.1f, 0.1f, 0.1f);
			}
			else
			{
				m_VertexBuf.SetColor(offset, 0.8f, 0.8f, 0.8f);
				m_VertexBuf.SetColor(offset + 1, 0.8f, 0.8f, 0.8f);
				m_VertexBuf.SetColor(offset + 2, 0.8f, 0.8f, 0.8f);
				m_VertexBuf.SetColor(offset + 3, 0.8f, 0.8f, 0.8f);
			}
		}
	}
	
	m_Shader.Init("res/ground.vert", "res/ground.frag");
	return 1;
}

void Ground::Draw(glm::mat4& viewMatrix,glm::mat4 &ProjectionMatrix)
{
	glEnable(GL_DEPTH_TEST);
	m_VertexBuf.Begin();
	{
		m_Shader.Begin();
		{
			m_Shader.Bind(glm::value_ptr(m_ModelMatrix), glm::value_ptr(viewMatrix), glm::value_ptr(ProjectionMatrix));

			for (int32_t i = 0; i < 400; i++)
			{
				glDrawArrays(GL_TRIANGLE_STRIP, i * 4, 4);
			}
		}
		m_Shader.End();
	}
	m_VertexBuf.End();
}

void Ground::SetAmbientMaterialColor(float r, float g, float b, float a)
{
	m_AmbientMaterial[0] = r;
	m_AmbientMaterial[1] = g;
	m_AmbientMaterial[2] = b;
	m_AmbientMaterial[3] = a;
}
void Ground::SetDiffuseMaterialColor(float r, float g, float b, float a)
{
	m_DiffuseMaterial[0] = r;
	m_DiffuseMaterial[1] = g;
	m_DiffuseMaterial[2] = b;
	m_DiffuseMaterial[3] = a;
}
void Ground::SetSpecularMaterialColor(float r, float g, float b, float a)
{
	m_SpecularMaterial[0] = r;
	m_SpecularMaterial[1] = g;
	m_SpecularMaterial[2] = b;
	m_SpecularMaterial[3] = a;
}