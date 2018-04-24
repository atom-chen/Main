#include "ParticleSystem.h"
#include "Utils.h"
#include "Mathf.h"

//2D粒子系统：不需要深度，不需要灯光
void Particle::Draw()
{
	glDisable(GL_DEPTH_TEST);
	glDisable(GL_LIGHTING);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

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
	m_CurentAngle = m_RotateSpeed*m_LivingTime;           //当前角度=角速度*生存时间-》活的越久越快
	m_CurRadius = m_MaxRadius*(m_LivingTime / m_LifeTime);//当前半径=最大半径*当前生存时间所占比例

	m_Position.x = m_CurRadius*cosf(m_CurentAngle*PI / FLAT);
	m_Position.y = m_CurRadius*sinf(m_CurentAngle*PI / FLAT);
}