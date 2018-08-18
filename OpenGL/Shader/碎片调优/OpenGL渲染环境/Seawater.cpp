#include "Seawater.h"
#include "Time.h"

bool SeaWater::Init(const char* seaTexture, const char* foamTexture, const char* lightTexture)
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		m_VertexBuf.Init(4);

		//ÆðÊ¼×ø±ê
		int size = 100;
		int rept = 100;
		int height = -5;

		m_VertexBuf.SetPosition(0, -size, -0.5f, size);
		m_VertexBuf.SetTexcoord(0, 0, 0);
		m_VertexBuf.SetPosition(1, size, -0.5f, size);
		m_VertexBuf.SetTexcoord(1, rept, 0);
		m_VertexBuf.SetPosition(2, -size, height, -size);
		m_VertexBuf.SetTexcoord(2, rept, rept);
		m_VertexBuf.SetPosition(3, size, height, -size);
		m_VertexBuf.SetTexcoord(3, 0, rept);

		m_Shader.Init(SHADER_ROOT"seawater.vert", SHADER_ROOT"seawater.frag");
		SetFoamTexture(foamTexture);
		SetSeaTexture(seaTexture);
		SetLightTexture(lightTexture);
	}
	return 1;



}

void SeaWater::Update()
{
	float time=Time::DeltaTime();
	m_Shader.SetFloat("u_time", time);
}

void SeaWater::SetSeaSize(float width,float height)
{
	m_Shader.SetFloat("u_1DivLevelWidth", width);
	m_Shader.SetFloat("u_1DivLevelHeight", height);
}
void SeaWater::SetWaveHeight(float height)
{
	m_Shader.SetFloat("WAVE_HEIGHT", height);
}
void SeaWater::SetWaveMoveSpeed(float speed)
{
	m_Shader.SetFloat("WAVE_MOVEMENT", speed);
}
void SeaWater::SetSeaTexture(const char* picPath)
{
	ASSERT_PTR_VOID(picPath);
	m_Shader.SetTexture2D(picPath, "normal0");

}
void SeaWater::SetFoamTexture(const char* picPath)
{
	ASSERT_PTR_VOID(picPath);
	m_Shader.SetTexture2D(picPath, "foam");
}
void SeaWater::SetLightTexture(const char* picPath)
{
	ASSERT_PTR_VOID(picPath);
	m_Shader.SetTexture2D(picPath, "lightmap");
}
void SeaWater::SetLight1(const Light& light)
{
	m_Shader.SetVec3("u_lightPos", light.GetPosition());
}