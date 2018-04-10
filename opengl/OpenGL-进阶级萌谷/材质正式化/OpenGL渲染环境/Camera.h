#pragma once
#include "ggl.h"
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
	virtual void MoveToFront();
	virtual void MoveToBack();
	void SetMoveSpeed(float speed);

	void Pitch(float angle);//���Լ���X����ת
	void Yaw(float angle);//����������ϵy��ת
	void RotateView(float angle, float x, float y, float z);//����������ת��x,y,zΪ��ķ���������

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
public:

protected:
	glm::vec3 m_Position;//�����λ��
	glm::vec3 m_ViewCenter;//Ŀ���ӵ�
	glm::vec3 m_Up;//�����ȥ�ķ�������

	glm::mat4 m_ModelMatrix;
	glm::mat4 m_ViewMatrix;
	glm::mat4 m_ProjectionMatrix;
	Frustum m_Frustum;

	float m_Radius = 50.0f;//��Ұ
	float m_ViewportWidget = 800;//�ӿڴ�С
	float m_ViewportHeight = 600;//�ӿڴ�С
	float m_Near = 0.1f;//�������
	float m_Far = 1000;//��Զ����
private:
	bool m_IsMoveToLeft=0, m_IsMoveToRight=0, m_IsMoveToTop=0, m_IsMoveToBottom=0;
	float m_MoveSpeed = 10.0f;
};


class Camera_3rd:public Camera_1st
{
public:
	Camera_3rd();
	virtual void Update();//����Target��
	virtual void MoveToFront();
	virtual void MoveToBack();
	virtual void SetDistance(float x, float y, float z);
	inline void SetTarget(const vec3* target){ this->m_Target = target; };
	virtual void Pitch(float angle);//���Լ���X����ת
	virtual void Yaw(float angle);//����������ϵy��ת
	virtual void RotateView(float angle, float x, float y, float z);//����������ת��x,y,zΪ��ķ���������

private:
	vec3 m_Distance;//��Ŀ��ľ���
	const vec3 * m_Target;
};