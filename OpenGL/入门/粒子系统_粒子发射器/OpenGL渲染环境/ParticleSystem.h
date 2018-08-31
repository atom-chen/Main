#pragma once
#include "ggl.h"
#include "Vector3.h"

#define DELAYTIME 0.1f



class Particle
{
public:
	Particle(GLubyte r = 255, GLubyte g = 255, GLubyte b = 255, GLubyte a = 255, float life = 1);
public:
	//void Init(GLubyte r, GLubyte g, GLubyte b, GLubyte a, float life);
	void SetTexture(GLuint texture);
	void SetHalfSize(float halfSize);
	void SetPosition(const Vector3& position);
	void Draw();
	void Update(float deltaTime);
protected:
public:
	float m_LifeTime = 1;//��������
	float m_RotateSpeed = 100;//�ٶ�
private:
	GLuint m_Texture=0;
	float m_HalfSize=0;
	Vector3 m_Position;

	GLubyte m_Color[4];

	float m_LivingTime=0;//�Ѿ�����˶��

};

class ParticleSystem
{
public:
	void CreatePartice();
	void Update(float deltime);
	void Draw();
private:
	std::vector<Particle> m_vParticle;
	float m_StartLiftTime=15;//ÿ�����Ӵ��ʱ��
	int m_MaxPar=1000;//���������
};
