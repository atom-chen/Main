#include "SurroundParticle.h"
#include "Resource.h"
#include "Time.h"

void SurroundParticle::Init(const vec3& position, const int& maxNum, const char* picPath)
{
	SetPosition(position);
	m_MaxPar = maxNum;

	m_VertexBuf.Init(m_MaxPar);//��ʼ�����ɸ�����
	m_Texture = ResourceManager::GetPic(picPath);
	for (int32_t i = 0; i < m_MaxPar; i++)
	{
		m_VertexBuf.SetColor(i, 0.1f, 0.4f, 0.6f);
		m_VertexBuf.SetPosition(i, 2 * cosf(float(i)*8.0f*3.14f / 180.0f), 0, 2 * sinf(float(i)*8.0f*3.14f / 180.0f));
	}
	m_Shader.Init("res/particle.vert", "res/particle.frag");
	m_Shader.SetTexture2D(m_Texture);

	m_Options.DrawType = DRAW_POINT;
	m_Options.Program_Point_Size = 1;
	m_Options.alphaBlend.AlphaBlend = 1;
}


void SurroundParticle::Update()
{
	float deltime = Time::DeltaTime();
	angle += deltime*10.0f;
	SetRotate(0, angle, 0);

	for (int32_t i = 0; i < VBO_NAME.GetLenth(); i++)
	{
		Vertex& vertex = VBO_NAME.mutable_vertex()[i];
		vertex.SetNormal(vertex.GetNormal().x, 0.1f*i, vertex.GetNormal().z, vertex.GetNormal().w);//���﷨�����������浥�����ӵ�λ��ƫ��ʹ��
		if (i > 90)
		{
			break;
		}
	}
}