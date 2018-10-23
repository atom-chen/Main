#include "Camera.h"
#include "ggl.h"
#include "Time.h"

Camera::Camera() :m_Position(4, 4, 4), m_ViewCenter(0, 0, -1), m_Up(0, 1, 0), m_ViewportWidget(WINDOW_WIDTH), m_ViewportHeight(WINDOW_HEIGHT)
{
	m_ViewMatrix = glm::lookAt(this->m_Position, this->m_ViewCenter, this->m_Up);
	this->m_ProjectionMatrix = glm::perspective(this->m_Radius, this->m_ViewportWidget / this->m_ViewportHeight, this->m_Near, this->m_Far);//设置投影矩阵
}
void Camera::SetViewPortSize(float width, float height, int xStart, int yStart)
{
	this->m_ViewportWidget = width;
	this->m_ViewportHeight = height;
	m_ViewportXStart = xStart;
	m_ViewportYStart = yStart;
	this->m_ProjectionMatrix = glm::perspective(this->m_Radius, width / height, this->m_Near, this->m_Far);//设置投影矩阵
}
void Camera::InsertToRenderList(RenderAble* render)
{
	m_RenderList.InsertToRenderList(render);
}
void Camera::InsertToRenderList(const RenderDomain& render)
{
	m_RenderList.InsertToRenderList(render);
}
void Camera::Draw()
{
	m_RenderList.Clip(this->m_ViewportXStart,m_ViewportYStart,m_ViewportWidget,m_ViewportHeight);
	m_RenderList.Draw(this->m_ViewMatrix,this->m_ProjectionMatrix);
}


void Camera::Update()
{
	float frameTime = Time::DeltaTime();
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
	this->m_ViewMatrix = glm::lookAt(m_Position, m_ViewCenter, m_Up);
	m_Frustum.loadFrustum(translate(m_Position)*m_ViewMatrix*m_ProjectionMatrix);
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
void  Camera::MoveToFront()
{
	vec3 forwardDirection = glm::normalize(m_ViewCenter - m_Position);
	vec3 delta = forwardDirection*0.1f*m_MoveSpeed;
	m_Position += delta;
	m_ViewCenter += delta;
}
void  Camera::MoveToBack()
{
	vec3 forwardDirection = glm::normalize(m_ViewCenter - m_Position);
	vec3 delta = forwardDirection*0.1f*m_MoveSpeed;
	m_Position -= delta;
	m_ViewCenter -= delta;
}


void Camera::Pitch(float angle) 
{
	vec3 viewDirection = m_ViewCenter - m_Position;
	viewDirection = glm::normalize(viewDirection);
	vec3 rightDirection = glm::cross(viewDirection, m_Up);
	rightDirection=glm::normalize(rightDirection);
	RotateView(angle, rightDirection.x, rightDirection.y, rightDirection.z);
}
void Camera::Yaw(float angle)
{
	RotateView(angle, m_Up.x, m_Up.y, m_Up.z);
}
void Camera::RotateView(float angle, float x, float y, float z) {
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





