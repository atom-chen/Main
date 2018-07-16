#include "SurroundParticle.h"
#include "Resource.h"
#include "Time.h"

void SurroundParticle::Init(const vec3& position, const int& maxNum, const char* picPath)
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		SetPosition(position);
		m_MaxPar = maxNum;

		m_VertexBuf.Init(m_MaxPar);//初始化若干个顶点
		for (int i = 0; i < m_MaxPar; i++)
		{
			m_VertexBuf.SetColor(i, 0.1f, 0.4f, 0.6f);
			m_VertexBuf.SetPosition(i, 2 * cosf(float(i)*8.0f*3.14f / 180.0f), 0, 2 * sinf(float(i)*8.0f*3.14f / 180.0f));
		}
		m_Shader.Init(SHADER_ROOT"SurroundParticle.vert", SHADER_ROOT"SurroundParticle.frag");
		m_Shader.SetTexture2D(picPath,1);
		SetPointSize(m_PointSize);
	}
}


void SurroundParticle::Update()
{
	INIT_TEST_VOID
	float deltime = Time::DeltaTime();
	angle += deltime*10.0f;
	SetRotate(0, angle, 0);
	for (int i = 0; i < VBO_NAME.GetLenth(); i++)
	{
		Vertex& vertex = VBO_NAME.mutable_vertex()[i];
		vertex.SetNormal(vertex.GetNormal().x, 0.1f*i, vertex.GetNormal().z, vertex.GetNormal().w);//这里法线是用来代替单个粒子的位置偏移使用
		if (i > 90)
		{
			break;
		}
	}
}