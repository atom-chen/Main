#include "Camera.h"
#include "ggl.h"

Camera::Camera() :m_Position(0, 0, 0), m_ViewCenter(0, 0, -1), m_Direction(0, 1, 0)
{
	
}
void Camera::Update(float frameTime)
{
	glLoadIdentity();
	//ÉãÏñ»úÏòÓÒÒÆ¶¯
	if (m_IsMoveToRight)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.x += delta;
		m_ViewCenter.x += delta;
	}
	if (m_IsMoveToLeft)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.x -= delta;
		m_ViewCenter.x -= delta;
	}
	if (m_IsMoveToTop)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.y += delta;
		m_ViewCenter.y += delta;
	}
	if (m_IsMoveToBottom)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.y -= delta;
		m_ViewCenter.y -= delta;
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
	this->m_MoveSpeed = speed;
}