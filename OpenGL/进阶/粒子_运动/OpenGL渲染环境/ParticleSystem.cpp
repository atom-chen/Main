#include "ParticleSystem.h"
#include "Utils.h"

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
void Particle::SetPosition(const vec3& position)
{
	
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

///////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////粒子系统 Begin/////////////////////////////////////////
void ParticleSystem::Init(const vec3& position) 
{
	this->m_ModelMatrix = translate(position);
	m_Position = position;
	m_VertexBuf.Init(m_MaxPar);
	m_Texture = CreateProcedureTexture(90);
	for (int32_t i = 0; i < m_MaxPar; i++)
	{
		m_VertexBuf.SetColor(i, 0.1f, 0.4f, 0.6f);
		m_VertexBuf.SetPosition(i, 2 * cosf(float(i)*8.0f*3.14f / 180.0f), 0, 2 * sinf(float(i)*8.0f*3.14f / 180.0f));
	}
	m_Shader.Init("particle.vert", "particle.frag");
	m_Shader.SetTexture2D(m_Texture);
}


void ParticleSystem::Update(float deltime)
{
	static float angle = 0.0f;
	angle += deltime*10.0f;
	MODELMATRIX_NAME = glm::rotate(angle, 0.0f, 1.0f, 0.0f);//粒子系统本身在旋转
	for (int32_t i = 0; i < VBO_NAME.GetLenth(); i++)
	{
		Vertex& vertex = VBO_NAME.mutable_vertex()[i];
		vertex.SetNormal(vertex.GetNormal().x, 0.1f*i, vertex.GetNormal().z, vertex.GetNormal().w);//vec4 pos=vec4(position.x+normal.x,position.y+normal.y,position.z+normal.z,1.0);，使得某个粒子的Y在向上移
		if (i > 90)
		{
			break;
		}
	}
}

void ParticleSystem::Draw(mat4 VIEWMATRIX_NAME, mat4 PROJECTIONMATRIX_NAME)
{
	glEnable(GL_POINT_SPRITE);
	glEnable(GL_PROGRAM_POINT_SIZE);//点的大小由程序控制
	glDisable(GL_DEPTH_TEST);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE);

	BEGIN
		glDrawArrays(GL_POINTS, 0, VBO_NAME.GetLenth());
	END
}
/////////////////////////////////粒子系统 End/////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////