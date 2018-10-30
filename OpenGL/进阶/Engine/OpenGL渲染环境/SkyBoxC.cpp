#include "SkyBoxC.h"
#include "Resource.h"

bool SkyBoxC::Init(const char* forwardPath, const char* backPath, const char* topPath, const char* bottomPath, const char* leftPath, const char* rightPath, 
	const char* vertexShader, const char* fragShader)
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		if (ResourceManager::GetModel("res/Cube.obj", m_VertexBuf))
		{
			m_ModelName = "res/Cube.obj";
			m_Shader.Init(vertexShader, fragShader);
			m_Shader.SetCubeMap(forwardPath, backPath, topPath, bottomPath, leftPath, rightPath);
			m_Options.DepthTest = 0;
			m_Options.DrawType = DRAW_TRIANGLES;
			return 1;
		}
	}
	return 0;
}
void SkyBoxC::Update(const vec3& cameraPos)
{
	SetPosition(cameraPos);
}
void SkyBoxC::Destroy()
{
	INIT_TEST_VOID
	RenderAble::Destroy();
	ResourceManager::RemoveModel(m_ModelName);
	m_IsInit = 0;
}