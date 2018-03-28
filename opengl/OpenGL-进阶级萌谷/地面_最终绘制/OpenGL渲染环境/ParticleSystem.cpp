#include "ParticleSystem.h"
#include "Utils.h"
#include "Mathf.h"

//2D粒子系统：不需要深度，不需要灯光
void Particle::Draw()
{
	glDisable(GL_DEPTH_TEST);
	glDisable(GL_LIGHTING);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE);

	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, m_Texture);

	glColor4ub(255, 255, 255, 255);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(m_Position.x - m_HalfSize, m_Position.y - m_HalfSize,0);

	glTexCoord2f(1, 0);
	glVertex3f(m_Position.x + m_HalfSize, m_Position.y - m_HalfSize,0);

	glTexCoord2f(1, 1);
	glVertex3f(m_Position.x + m_HalfSize, m_Position.y + m_HalfSize,0);

	glTexCoord2f(0, 1);
	glVertex3f(m_Position.x - m_HalfSize, m_Position.y + m_HalfSize,0);

	glEnd();
	glDisable(GL_BLEND);
}

Particle::Particle(GLubyte r, GLubyte g, GLubyte b, GLubyte a, float life) :m_LifeTime(life), m_LivingTime(0)
{
	m_Color[0] = r;
	m_Color[1] = g;
	m_Color[2] = b;
	m_Color[3] = a;
}



void Particle::SetTexture(GLuint texture)
{
	this->m_Texture = texture;
}

void Particle::SetHalfSize(float halfSize)
{
	this->m_HalfSize = halfSize;
}
void Particle::SetPosition(const Vector3& position)
{
	this->m_Position.CopyFrom(position);
}
void Particle::Update(float deltaTime)
{
	if (m_LivingTime >= m_LifeTime)
	{
		m_LivingTime=0;
	}
	m_LivingTime += deltaTime;

	//向上走一段距离
	m_Position.y += m_RotateSpeed*deltaTime;
	m_Position.x += -rand() % 6 + rand() % 6;
}


void ParticleSystem:: CreatePartice()
{
	//创建一个新粒子

	Particle Particle(220, 150, 50, 255, m_StartLiftTime);
	Particle.SetTexture(CreateProcedureTexture(56, ALPHA_TYPE::ALPHA_GAUSSIAN));
	Particle.SetHalfSize(28);
	Particle.SetPosition(Vector3(0, 0, 0));
	m_vParticle.push_back(Particle);
}

void ParticleSystem::Update(float deltime)
{
	//如果不满足1000 则创建一个新粒子
	static float lastTime = 0; 
	if (m_vParticle.size()<m_MaxPar&& timeGetTime()-lastTime>=DELAYTIME)
	{
		CreatePartice();
		lastTime = timeGetTime();
	}
	for (auto it = m_vParticle.begin(); it != m_vParticle.end(); it++)
	{
		it->Update(deltime);
	}
}

void ParticleSystem::Draw()
{
	for (auto it = m_vParticle.begin(); it != m_vParticle.end(); it++)
	{
		it->Draw();
	}
}