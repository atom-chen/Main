#pragma once
#include "ggl.h"
#include "GameObject.h"
#include "Frustum.hpp"

class Camera_1st
{
public:
	Camera_1st();
public:
	virtual void Update();
	//void Draw();
	void SetViewPortSize(float width, float height);
public:
	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void MoveToFront();
	void MoveToBack();
	void SetMoveSpeed(float speed);

	void Pitch(float angle);//绕自己的X轴旋转
	void Yaw(float angle);//绕世界坐标系y旋转
	void RotateView(float angle, float x, float y, float z);//绕任意轴旋转（x,y,z为轴的方向向量）

public:
	inline const glm::vec3& GetPosition() const{ return this->m_Position; };
	inline void SetPosition(const vec3& position){ this->m_Position = position; };
	inline glm::mat4 GetViewMatrix() const{ return this->m_ViewMatrix; };
	virtual inline void SetTarget(const vec3* target){ this->m_ViewCenter = *target; };
	inline glm::mat4 GetProjectionMatrix() const{ return this->m_ProjectionMatrix; };
	inline float GetWidth() const{ return this->m_ViewportWidget; };
	inline float GetHeight() const{ return this->m_ViewportHeight; };
	inline float GetRadius() const{ return this->m_Radius; };
	inline Frustum GetFrustum() const{ return this->m_Frustum; }

protected:
	glm::vec3 m_Position;//摄像机位置
	glm::vec3 m_ViewCenter;//目标视点
	glm::vec3 m_Up;//发射出去的方向向量

	glm::mat4 m_ModelMatrix;
	glm::mat4 m_ViewMatrix;
	glm::mat4 m_ProjectionMatrix;
	Frustum m_Frustum;

	float m_Radius = 50.0f;//视野
	float m_ViewportWidget = 800;//视口大小
	float m_ViewportHeight = 600;//视口大小
	float m_Near = 0.1f;//最近距离
	float m_Far = 1000;//最远距离
private:
	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom, m_IsMoveToFront, m_IsMoveToBack;
	float m_MoveSpeed = 10.0f;
};


class Camera_3rd:public Camera_1st
{
public:
	Camera_3rd(vec3* target);
	virtual void Update();//跟着Target走
	virtual void SetDistance(float x, float y, float z);
	inline void SetTarget(const vec3* target){ this->m_Target = target; };
private:
	vec3 m_Distance;//和目标的距离
	const vec3 * m_Target;
};