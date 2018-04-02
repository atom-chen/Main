#pragma once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"

#define DELAYTIME 0.1f



class Particle
{
public:
	Particle(GLubyte r = 255, GLubyte g = 255, GLubyte b = 255, GLubyte a = 255, float life = 1);
public:
	void SetTexture(GLuint texture);
	void SetHalfSize(float halfSize);
	void SetPosition(const vec3& position);
	void Draw();
	void Update(float deltaTime);
protected:
public:
	float m_LifeTime = 1;//生命周期
	float m_RotateSpeed = 100;//速度
private:
	GLuint m_Texture=0;
	float m_HalfSize=0;
	vec3 m_Position;

	GLubyte m_Color[4];

	float m_LivingTime=0;//已经存活了多久

};

class ParticleSystem
{
public:
	void Init(const vec3& position);
public:
	void Update(float deltime);
	void Draw(mat4 VIEWMATRIX_NAME,mat4 PROJECTIONMATRIX_NAME);

public:
	inline vec3& GetPosition(){ return this->m_Position; };
private:
	std::vector<Particle> m_vParticle;
	float m_StartLiftTime=15;//每个粒子存活时间
	int32_t m_MaxPar=180;//最大粒子数


	VertexBuffer VBO_NAME;
	Shader SHADER_NAME;
	mat4 MODELMATRIX_NAME;
	vec3 m_Position;
	GLuint m_Texture;
};
