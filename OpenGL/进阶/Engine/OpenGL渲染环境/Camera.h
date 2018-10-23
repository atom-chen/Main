#pragma once
#include "ggl.h"
#include "Frustum.hpp"
#include "RenderAble.h"
#include "RenderList.h"

class Camera
{
public:
	Camera();
public:
	virtual void Update();
	virtual void Draw();
	virtual void InsertToRenderList(RenderAble* render);
	virtual void InsertToRenderList(const RenderDomain& render);
	void SetViewPortSize(float width, float height,int xStart=0,int yStart=0);
public:
	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	virtual void MoveToFront();
	virtual void MoveToBack();
	void SetMoveSpeed(float speed);

	virtual void Pitch(float angle);//绕自己的X轴旋转
	virtual void Yaw(float angle);//绕世界坐标系y旋转
	void RotateView(float angle, float x, float y, float z);//绕任意轴旋转（x,y,z为轴的方向向量）

public:
	inline const glm::vec3& GetPosition() const{ return this->m_Position; };
	inline void SetPosition(const vec3& position){ this->m_Position = position; };
	inline glm::mat4 GetViewMatrix() const{ return this->m_ViewMatrix; };
	virtual inline void SetTarget(const vec3* target){ this->m_ViewCenter = *target; };
	inline glm::mat4 GetProjectionMatrix() const{ return this->m_ProjectionMatrix; };
	inline float GetWidth() const{ return this->m_ViewportWidget; };
	inline float GetHeight() const{ return this->m_ViewportHeight; };
	inline float GetXStart() const{ return this->m_ViewportXStart; };
	inline float GetYStart() const{ return this->m_ViewportYStart; };
	inline float GetRadius() const{ return this->m_Radius; };
	inline Frustum GetFrustum() const{ return this->m_Frustum; }
public:

protected:
	RenderList m_RenderList;
	glm::vec3 m_Position;//摄像机位置
	glm::vec3 m_ViewCenter;//目标视点
	glm::vec3 m_Up;//发射出去的方向向量

	glm::mat4 m_ViewMatrix;
	glm::mat4 m_ProjectionMatrix;
	Frustum m_Frustum;

	float m_Radius = 50.0f;//视野
	float m_ViewportWidget = 800;//视口大小
	float m_ViewportHeight = 600;//视口大小
	float m_ViewportXStart = 0;
	float m_ViewportYStart = 0;
	float m_Near = 0.1f;//最近距离
	float m_Far = 1000;//最远距离
	bool m_IsMoveToLeft = 0, m_IsMoveToRight = 0, m_IsMoveToTop = 0, m_IsMoveToBottom = 0;
	float m_MoveSpeed = 10.0f;
};


class Camera_3rd:public Camera
{
public:
	Camera_3rd();
	virtual void Update();//跟着Target走
	virtual void MoveToFront();
	virtual void MoveToBack();
	virtual void SetDistance(float x, float y, float z);
	virtual void SetTarget(const vec3* target);
	virtual void Pitch(float angle);//绕自己的X轴旋转
	virtual void Yaw(float angle);//绕世界坐标系y旋转

private:
	vec3 m_Direction;
	vec3 m_Distance;
	const vec3 * m_Target;
};