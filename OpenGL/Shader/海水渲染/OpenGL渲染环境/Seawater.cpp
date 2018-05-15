#include "Seawater.h"
#include "Time.h"

bool SeaWater::Init(const char* seaTexture, const char* foamTexture, const char* lightTexture)
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		m_VertexBuf.Init(1600);
		for (int z = 0; z < 20; z++)
		{
			//起始坐标
			float zStart = static_cast<float>(100 - z * 10);
			for (int x = 0; x < 20; x++)
			{
				float xStart = static_cast<float>(x * 10 - 100);
				int offset = (x + z * 20) * 4;

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

			m_Shader.Init("res/seawater.vert", "res/seawater.frag");
			SetFoamTexture(foamTexture);
			SetSeaTexture(seaTexture);
			SetLightTexture(lightTexture);
			SetSeaSize(512, 512);
			SetWaveHeight(100);
			SetWaveMoveSpeed(10);
			SetShore_Dark(vec3(0.2f, 0.2f, 0.2f));
			SetSeq_Dark(vec3(0.2f, 0.2f, 0.2f));
			SetShore_Light(vec3(0.3f, 0.5f, 0.5f));
			SetSeq_Light(vec3(0.8f, 0.8f, 0.8f));
		}
		return 1;
	}
}

void SeaWater::Update()
{
	INIT_TEST_VOID
		float time = Time::DeltaTime();
	m_Shader.SetFloat("u_time", time);
}
void SeaWater::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	SceneManager::SetDepthTestState(true);
	m_VertexBuf.Begin();
	{
		m_Shader.Begin();
		{
			m_Shader.Bind(glm::value_ptr(m_ModelMatrix), glm::value_ptr(viewMatrix), glm::value_ptr(ProjectionMatrix));

			for (int i = 0; i < 400; i++)
			{
				glDrawArrays(GL_TRIANGLE_STRIP, i * 4, 4);
			}
		}
		m_Shader.End();
	}
	m_VertexBuf.End();
}
void SeaWater::Destroy()
{
	INIT_TEST_VOID
	m_Shader.Destory();
	m_VertexBuf.Destory();
}
void SeaWater::SetSeaSize(float width, float height)
{
	INIT_TEST_VOID
		m_Shader.SetFloat("u_1DivLevelWidth", width);
	m_Shader.SetFloat("u_1DivLevelHeight", height);
}
void SeaWater::SetWaveHeight(float height)
{
	INIT_TEST_VOID
		m_Shader.SetFloat("WAVE_HEIGHT", height);
}
void SeaWater::SetWaveMoveSpeed(float speed)
{
	INIT_TEST_VOID
		m_Shader.SetFloat("WAVE_MOVEMENT", speed);
}
void SeaWater::SetSeaTexture(const char* picPath)
{
	INIT_TEST_VOID
		ASSERT_PTR_VOID(picPath);
	m_Shader.SetTexture2D(picPath, false, "normal0");

}
void SeaWater::SetFoamTexture(const char* picPath)
{
	INIT_TEST_VOID
		ASSERT_PTR_VOID(picPath);
	m_Shader.SetTexture2D(picPath, "foam");
}
void SeaWater::SetLightTexture(const char* picPath)
{
	INIT_TEST_VOID
		ASSERT_PTR_VOID(picPath);
	m_Shader.SetTexture2D(picPath, "lightmap");
}
void SeaWater::SetLight1(const Light& light)
{
	INIT_TEST_VOID
		m_Shader.SetVec3("u_lightPos", light.GetPosition());
}
void SeaWater::SetShore_Light(const vec3& color)//岸的明rgb
{
	INIT_TEST_VOID
		m_Shader.SetVec3("SHORE_LIGHT", color.r, color.g, color.b);
}
void SeaWater::SetShore_Dark(const vec3& color)//岸的暗rgb
{
	INIT_TEST_VOID
		m_Shader.SetVec3("SHORE_DARK", color.r, color.g, color.b);
}
void SeaWater::SetSeq_Light(const vec3& color)//水的明rgb
{
	INIT_TEST_VOID
		m_Shader.SetVec3("SEA_LIGHT", color.r, color.g, color.b);
}
void SeaWater::SetSeq_Dark(const vec3& color)//水的暗rgb
{
	INIT_TEST_VOID
		m_Shader.SetVec3("SEA_DARK", color.r, color.g, color.b);
}