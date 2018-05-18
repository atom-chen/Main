#pragma once
#include "Vector3.h"




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
	float m_MaxRadius = 300;//���뾶
	float m_RotateSpeed = 200;//��ת�ٶ�
private:
	GLuint m_Texture=0;
	float m_HalfSize=0;
	Vector3 m_Position;

	GLubyte m_Color[4];

	float m_LivingTime=0;//�Ѿ�����˶��
	float m_CurentAngle = 0;//��ǰ�Ƕ�
	float m_CurRadius = 0;//��ǰ��ת�뾶

};
