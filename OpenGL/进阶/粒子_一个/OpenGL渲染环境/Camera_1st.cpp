#include "Camera.h"
#include "ggl.h"

Camera_1st::Camera_1st() :m_Position(4, 4, 4), m_ViewCenter(0, 0, -1), m_Up(0, 1, 0), m_ViewportWidget(WINDOW_WIDTH), m_ViewportHeight(WINDOW_HEIGHT)
{
	m_ViewMatrix = glm::lookAt(this->m_Position, this->m_ViewCenter, this->m_Up);
	SwitchTo3D();
}
void Camera_1st::SetViewPortSize(float width, float height)
{
	this->m_ViewportWidget = width;
	this->m_ViewportHeight = height;
	this->m_ProjectionMatrix = glm::perspective(this->m_Radius, width / height, this->m_Near, this->m_Far);//设置投影矩阵
}


void Camera_1st::Update(float frameTime)
{
	//通过前方的方向向量移动，方向为视点-当前位置
	vec3 forwardDirection = glm::normalize(m_ViewCenter - m_Position);
	//右边：前方和向上的叉乘
	vec3 rightDirection = glm::normalize(glm::cross(forwardDirection, m_Up));
	//上方：头顶方向 UP
	//摄像机向右移动，移动的是一个向量
	if (m_IsMoveToRight)
	{
		vec3 delta = rightDirection*frameTime*m_MoveSpeed;
		m_Position += delta;
		m_ViewCenter += delta;
	}
	if (m_IsMoveToLeft)
	{
		vec3 delta = rightDirection*frameTime*m_MoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	if (m_IsMoveToTop)
	{
		vec3 delta = m_Up*frameTime*m_MoveSpeed;
		m_Position+= delta;
		m_ViewCenter+= delta;
	}
	if (m_IsMoveToBottom)
	{
		vec3 delta = m_Up*frameTime*m_MoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	if (m_IsMoveToFront)
	{
		vec3 delta = forwardDirection*frameTime*m_MoveSpeed;
		m_Position += delta;
		m_ViewCenter += delta;
	}
	if (m_IsMoveToBack)
	{
		vec3 delta = forwardDirection*frameTime*m_MoveSpeed;
		m_Position -= delta;
		m_ViewCenter -= delta;
	}
	this->m_ViewMatrix = glm::lookAt(m_Position, m_ViewCenter, m_Up);
}

void Camera_1st::MoveToLeft(bool isMove)
{
	m_IsMoveToLeft = isMove;
}
void Camera_1st::MoveToRight(bool isMove)
{
	m_IsMoveToRight = isMove;
}
void Camera_1st::MoveToTop(bool isMove)
{
	m_IsMoveToTop = isMove;
}
void Camera_1st::MoveToBottom(bool isMove)
{
	m_IsMoveToBottom = isMove;
}
void Camera_1st::SetMoveSpeed(float speed)
{
	this->m_MoveSpeed = speed;
}
void  Camera_1st::MoveToFront(bool isMove)
{
	m_IsMoveToFront = isMove;
}
void  Camera_1st::MoveToBack(bool isMove)
{
	m_IsMoveToBack = isMove;
}


void Camera_1st::Pitch(float angle) 
{
	vec3 viewDirection = m_ViewCenter - m_Position;
	viewDirection = glm::normalize(viewDirection);
	vec3 rightDirection = glm::cross(viewDirection, m_Up);
	rightDirection=glm::normalize(rightDirection);
	RotateView(angle, rightDirection.x, rightDirection.y, rightDirection.z);
}
void Camera_1st::Yaw(float angle)
{
	RotateView(angle, m_Up.x, m_Up.y, m_Up.z);
}
void Camera_1st::RotateView(float angle, float x, float y, float z) {
	vec3 viewDirection =  m_ViewCenter - m_Position;
	vec3 newDirection(0.0f, 0.0f, 0.0f);
	float C = cosf(angle);
	float S = sinf(angle);
	vec3 tempX(C + x*x*(1 - C), x*y*(1 - C) - z*S, x*z*(1 - C) + y*S);
	newDirection.x = glm::dot(tempX, viewDirection);
	vec3 tempY(x*y*(1 - C) + z*S, C + y*y*(1 - C), y*z*(1 - C) - x*S);
	newDirection.y = glm::dot(tempY, viewDirection); 
	vec3 tempZ(x*z*(1 - C) - y*S, y*z*(1 - C) + x*S, C + z*z*(1 - C));
	newDirection.z = glm::dot(tempZ, viewDirection);

	m_ViewCenter = m_Position + newDirection;
}

void Camera_1st::SwitchTo2D()
{
	this->m_ProjectionMatrix = glm::ortho(-m_ViewportWidget / 2, m_ViewportWidget / 2, -m_ViewportHeight / 2, m_ViewportHeight / 2);
}

void Camera_1st::SwitchTo3D()
{
	this->m_ProjectionMatrix = glm::perspective(this->m_Radius, this->m_ViewportWidget / this->m_ViewportHeight, this->m_Near, this->m_Far);//设置投影矩阵
}




