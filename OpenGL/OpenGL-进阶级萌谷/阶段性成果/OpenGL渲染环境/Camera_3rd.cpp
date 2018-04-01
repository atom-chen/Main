#include "Camera.h"


Camera_3rd::Camera_3rd() :Camera_1st(), m_Distance(0,3,3)
{
	m_Radius = 4;
}

void Camera_3rd::Update(float frameTime, const vec3& target)  
{
	  //¸üÐÂdirection
		m_Position = target + vec3(0, 3, 3);
		m_ViewCenter = target;
		
		m_ViewMatrix = glm::lookAt(m_Position, m_ViewCenter, m_Up);
}

void Camera_3rd::SetDistance(float x, float y, float z)
{
	this->m_Distance.x = x;
	this->m_Distance.y = y;
	this->m_Distance.z = z;
}