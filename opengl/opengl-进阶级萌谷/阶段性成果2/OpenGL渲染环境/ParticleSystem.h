#pragma once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"

#define DELAYTIME 0.1f


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
	float m_StartLiftTime=15;//每个粒子存活时间
	int32_t m_MaxPar=180;//最大粒子数


	VertexBuffer VBO_NAME;
	Shader SHADER_NAME;
	mat4 MODELMATRIX_NAME;
	vec3 m_Position;
	GLuint m_Texture;
};
