#include "Camera.h"
#include "ggl.h"

Camera::Camera() :m_Position(0, 0, 0), m_ViewCenter(0, 0, -1), m_Direction(0, 1, 0)
{
	
}
void Camera::Update(float frameTime)
{
	glLoadIdentity();
	//通过前方的方向向量移动，方向为视点-当前位置
	Vector3 forwardDirection = (m_ViewCenter - m_Position);
	forwardDirection.Normalize();
	//右边：前方和向上的叉乘
	Vector3 rightDirection = forwardDirection.Cross(m_Direction);
	rightDirection.Normalize();
	//上方：头顶方向
	//摄像机向右移动，移动的是一个向量
	if (m_IsMoveToRight)
	{
		Vector3 delta = rightDirection*frameTime*m_XMoveSpeed;
		m_Position += delta;
		m_ViewCenter += delta;
	}
	if (m_IsMoveToLeft)
	{
		Vector3 delta = rightDirection*frameTime*m_XMoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	if (m_IsMoveToTop)
	{
		Vector3 delta = m_Direction*frameTime*m_YMoveSpeed;
		m_Position+= delta;
		m_ViewCenter+= delta;
	}
	if (m_IsMoveToBottom)
	{
		Vector3 delta = m_Direction*frameTime*m_YMoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	if (m_IsMoveToFront)
	{
		Vector3 delta = forwardDirection*frameTime*m_ZMoveSpeed;
		m_Position += delta;
		m_ViewCenter += delta;
	}
	if (m_IsMoveToBack)
	{
		Vector3 delta = forwardDirection*frameTime*m_ZMoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	gluLookAt(m_Position.x, m_Position.y, m_Position.z, m_ViewCenter.x, m_ViewCenter.y, m_ViewCenter.z, m_Direction.x, m_Direction.y, m_Direction.z);
}

void Camera::MoveToLeft(bool isMove)
{
	m_IsMoveToLeft = isMove;
}
void Camera::MoveToRight(bool isMove)
{
	m_IsMoveToRight = isMove;
}
void Camera::MoveToTop(bool isMove)
{
	m_IsMoveToTop = isMove;
}
void Camera::MoveToBottom(bool isMove)
{
	m_IsMoveToBottom = isMove;
}
void Camera::SetMoveSpeed(float speed)
{
	this->m_XMoveSpeed = speed;
}
void  Camera::MoveToFront(bool isMove)
{
	m_IsMoveToFront = isMove;
}
void  Camera::MoveToBack(bool isMove)
{
	m_IsMoveToBack = isMove;
}


const Vector3& Camera::GetPosition()
{
	return this->m_Position;
}

void Camera::Pitch(float angle)
{

}
void Camera::Yaw(float angle)
{

}
