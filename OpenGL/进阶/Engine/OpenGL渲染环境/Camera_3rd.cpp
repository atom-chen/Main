#include "Camera.h"
#include "Time.h"


Camera_3rd::Camera_3rd() :Camera(), m_Distance(0,2,0)
{
	m_Radius = 20;
}

void Camera_3rd::SetTarget(const vec3* target)
{ 
	this->m_Target = target; 
	m_Direction = normalize(*m_Target - m_Position);
};
void Camera_3rd::Update()  
{
	m_ViewCenter = *m_Target + m_Distance;
	m_Position = m_ViewCenter - m_Direction*m_Radius;

	m_ViewMatrix = glm::lookAt(m_Position, m_ViewCenter, m_Up);
	m_Frustum.loadFrustum(translate(m_Position)*m_ViewMatrix*m_ProjectionMatrix);
}
void Camera_3rd::MoveToFront()
{
	if (m_Radius >= 15)
	{
		m_Radius-=5;
	}
}
void Camera_3rd::MoveToBack()
{
	m_Radius+=5;
}

void Camera_3rd::SetDistance(float x, float y, float z)
{
	this->m_Distance.x = x;
	this->m_Distance.y = y;
	this->m_Distance.z = z;
}
void Camera_3rd::Pitch(float angle)
{
	m_Direction = glm::rotateX(m_Direction, angle*20);
}
void Camera_3rd::Yaw(float angle)
{
	m_Direction = glm::rotateY(m_Direction, angle*20);
}