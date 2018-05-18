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
	float m_LifeTime = 1;//生命周期
	float m_MaxRadius = 300;//最大半径
	float m_RotateSpeed = 200;//旋转速度
private:
	GLuint m_Texture=0;
	float m_HalfSize=0;
	Vector3 m_Position;

	GLubyte m_Color[4];

	float m_LivingTime=0;//已经存活了多久
	float m_CurentAngle = 0;//当前角度
	float m_CurRadius = 0;//当前旋转半径

};
