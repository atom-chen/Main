#include "UpParticle.h"
#include "Time.h"
#include "Resource.h"
//void Particle::Update(float deltaTime)
//{
//	if (m_LivingTime >= m_LifeTime)
//	{
//		m_LivingTime = 0;
//	}
//	m_LivingTime += deltaTime;
//
//	//向上走一段距离
//	m_Position.y += m_RotateSpeed*deltaTime;
//	m_Position.x += -rand() % 6 + rand() % 6;
//}
////2D粒子系统：不需要深度，不需要灯光
//void Particle::Draw()
//{
//	//glColor4ub(255, 255, 255, 255);
//	//glBegin(GL_QUADS);
//	//glTexCoord2f(0, 0);
//	//glVertex3f(m_Position.x - m_HalfSize, m_Position.y - m_HalfSize, 0);
//
//	//glTexCoord2f(1, 0);
//	//glVertex3f(m_Position.x + m_HalfSize, m_Position.y - m_HalfSize, 0);
//
//	//glTexCoord2f(1, 1);
//	//glVertex3f(m_Position.x + m_HalfSize, m_Position.y + m_HalfSize, 0);
//
//	//glTexCoord2f(0, 1);
//	//glVertex3f(m_Position.x - m_HalfSize, m_Position.y + m_HalfSize, 0);
//
//	//glEnd();
//}
//
//Particle::Particle(GLubyte r, GLubyte g, GLubyte b, GLubyte a, float life) :m_LifeTime(life), m_LivingTime(0)
//{
//	m_Color[0] = r;
//	m_Color[1] = g;
//	m_Color[2] = b;
//	m_Color[3] = a;
//}
//
//
//void Particle::SetHalfSize(float halfSize)
//{
//	this->m_HalfSize = halfSize;
//}
//void Particle::SetPosition(const vec3& position)
//{
//	m_Position=position;
//}

void UpParticle::Init(const vec3& pos)
{
	if (m_IsInit)
	{
		return;
	}
	m_IsInit = 1;
	m_Options.alphaBlend.Type = ALPHA_BLEND_ONE;
	m_Options.alphaBlend.AlphaBlend = 1;
	m_Options.Program_Point_Size = true;
	m_Options.DrawType = DRAW_POINTS;

	SetPosition(pos);
	m_VertexBuf.Init(m_MaxPar);
	for (int i = 0; i < m_MaxPar; i++)
	{
		m_VertexBuf.SetPosition(i, 0, 0, 0);
		//m_VertexBuf.SetColor(i, 0.1f, 0.4f, 0.6f);
		m_VertexBuf.SetColor(i, 1, 1, 1,0);
	}
	m_Shader.Init(SHADER_ROOT"UpParticle.vert", SHADER_ROOT"UpParticle.frag");
	m_Shader.SetTexture2D(ResourceManager::CreateProcedureTexture(56, ALPHA_TYPE::ALPHA_GAUSSIAN));
}
UpParticle::UpParticle()
{

}

void UpParticle::Update()
{
	INIT_TEST_VOID
	float deltime = Time::DeltaTime();
	float time=timeGetTime() / 1000;
	//如果不满足1000 则激活一个新粒子
	if (m_InLife < m_MaxPar - 1 && time - lastTime >= DELAYTIME)
	{
		lastTime = time;
		m_VertexBuf.SetColor(m_InLife, 1, 1, 1, 1);
		m_InLife++;
	}
	bool isChange = false;
	if (time-m_LastChangeTime>= 0.0f)
	{
		isChange = true;
	}
	for (int i = 0; i < m_InLife;i++)
	{
		//向上做随机运动
		Vertex& vertex = m_VertexBuf.mutable_vertex()[i];
		vec4 newPos=vertex.GetPos();
		newPos.y += 0.5f*deltime;
		if (isChange)
		{
			newPos.x += (-rand() % 2 + rand() % 2)/128.0f;
		}
		vertex.SetPosition(newPos.x,newPos.y,newPos.z,newPos.w);
	}
	if (isChange)
	{
		m_LastChangeTime = time;
	}
}

