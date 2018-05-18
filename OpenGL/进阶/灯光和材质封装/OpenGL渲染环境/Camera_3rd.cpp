#include "Camera.h"
#include "Time.h"


Camera_3rd::Camera_3rd(vec3* target) :Camera_1st(), m_Distance(0, 3, 3)
{
	m_Radius = 4;
	this->m_Target = target;
}


void Camera_3rd::Update()  
{
	float frameTime = Time::DeltaTime();
	  //¸üÐÂdirection
	  m_ViewCenter = *m_Target;
		m_Position = m_ViewCenter + vec3(0, 3, 3);
		m_ViewMatrix = glm::lookAt(m_Position, m_ViewCenter, m_Up);
}

void Camera_3rd::SetDistance(float x, float y, float z)
{
	this->m_Distance.x = x;
	this->m_Distance.y = y;
	this->m_Distance.z = z;
}