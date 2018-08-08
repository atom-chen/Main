#pragma once
#include "ggl.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "RenderAble.h"

#define DELAYTIME 0.1f


class ParticleSystem:public RenderAble
{
public:
	void Init(const vec3& position);
public:
	void Update();
	void Draw();

public:
	inline vec3& GetPosition(){ return this->m_Position; };
private:
	float m_StartLiftTime=15;//ÿ�����Ӵ��ʱ��
	int32_t m_MaxPar=180;//���������
	vec3 m_Position;//����ϵͳ��λ��
	GLuint m_Texture;//����ϵͳ��������
};
