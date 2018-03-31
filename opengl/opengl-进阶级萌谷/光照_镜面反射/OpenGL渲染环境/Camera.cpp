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

void Camera::Pitch(float angle) {
	Vector3 viewDirection = m_ViewCenter - m_Position;
	viewDirection.Normalize();
	Vector3 rightDirection = viewDirection.Cross(m_Direction);
	rightDirection.Normalize();
	RotateView(angle, rightDirection.x, rightDirection.y, rightDirection.z);
}
void Camera::Yaw(float angle)
{
	RotateView(angle, m_Direction.x, m_Direction.y, m_Direction.z);
}
void Camera::RotateView(float angle, float x, float y, float z) {
	Vector3 viewDirection =  m_ViewCenter - m_Position;
	Vector3 newDirection(0.0f, 0.0f, 0.0f);
	float C = cosf(angle);
	float S = sinf(angle);
	Vector3 tempX(C + x*x*(1 - C), x*y*(1 - C) - z*S, x*z*(1 - C) + y*S);
	newDirection.x = tempX*viewDirection;
	Vector3 tempY(x*y*(1 - C) + z*S, C + y*y*(1 - C), y*z*(1 - C) - x*S);
	newDirection.y = tempY*viewDirection;
	Vector3 tempZ(x*z*(1 - C) - y*S, y*z*(1 - C) + x*S, C + z*z*(1 - C));
	newDirection.z = tempZ*viewDirection;
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