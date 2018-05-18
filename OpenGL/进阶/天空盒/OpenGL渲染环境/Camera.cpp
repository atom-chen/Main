#include "Camera.h"
#include "ggl.h"

Camera::Camera() :m_Position(0, 0, 0), m_ViewCenter(0, 0, -1), m_Direction(0, 1, 0)
{
	
}

void Camera::Update(float frameTime)
{
	glLoadIdentity();
	//通过前方的方向向量移动，方向为视点-当前位置
	vec3 forwardDirection = glm::normalize(m_ViewCenter - m_Position);
	//右边：前方和向上的叉乘
	
	vec3 rightDirection = glm::normalize(glm::cross(forwardDirection, m_Direction));
	//上方：头顶方向
	//摄像机向右移动，移动的是一个向量
	if (m_IsMoveToRight)
	{
		vec3 delta = rightDirection*frameTime*m_XMoveSpeed;
		m_Position += delta;
		m_ViewCenter += delta;
	}
	if (m_IsMoveToLeft)
	{
		vec3 delta = rightDirection*frameTime*m_XMoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	if (m_IsMoveToTop)
	{
		vec3 delta = m_Direction*frameTime*m_YMoveSpeed;
		m_Position+= delta;
		m_ViewCenter+= delta;
	}
	if (m_IsMoveToBottom)
	{
		vec3 delta = m_Direction*frameTime*m_YMoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	if (m_IsMoveToFront)
	{
		vec3 delta = forwardDirection*frameTime*m_ZMoveSpeed;
		m_Position += delta;
		m_ViewCenter += delta;
	}
	if (m_IsMoveToBack)
	{
		vec3 delta = forwardDirection*frameTime*m_ZMoveSpeed;
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


void Camera::Pitch(float angle) 
{
	vec3 viewDirection = m_ViewCenter - m_Position;
	viewDirection = glm::normalize(viewDirection);
	vec3 rightDirection = glm::cross(viewDirection, m_Direction);
	rightDirection=glm::normalize(rightDirection);
	RotateView(angle, rightDirection.x, rightDirection.y, rightDirection.z);
}
void Camera::Yaw(float angle)
{
	RotateView(angle, m_Direction.x, m_Direction.y, m_Direction.z);
}
void Camera::RotateView(float angle, float x, float y, float z) {
	vec3 viewDirection =  m_ViewCenter - m_Position;
	vec3 newDirection(0.0f, 0.0f, 0.0f);
	float C = cosf(angle);
	float S = sinf(angle);
	m_ViewCenter = m_Position + newDirection;
}

void Camera::SwitchTo2D()
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(-m_ViewportWidget / 2, m_ViewportWidget / 2, -m_ViewportHeight / 2, m_ViewportHeight / 2);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}
void Camera::SwitchTo3D()
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(m_Angle, m_ViewportWidget / m_ViewportHeight, m_Near, m_Far);//定义视景体
	glMatrixMode(GL_MODELVIEW);
}