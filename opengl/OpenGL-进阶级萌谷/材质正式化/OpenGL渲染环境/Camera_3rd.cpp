#include "Camera.h"
#include "Time.h"


Camera_3rd::Camera_3rd() :Camera_1st(), m_Distance(0, 3, 3)
{
	m_Radius = 4;
}


void Camera_3rd::Update()  
{
	float frameTime = Time::DeltaTime();
	  //¸üÐÂdirection
	m_ViewCenter = *m_Target;
	m_Position = m_ViewCenter + vec3(0, 3, 3);
	m_ViewMatrix = glm::lookAt(m_Position, m_ViewCenter, m_Up);
}
void Camera_3rd::MoveToFront()
{

}
void Camera_3rd::MoveToBack()
{

}

void Camera_3rd::SetDistance(float x, float y, float z)
{
	this->m_Distance.x = x;
	this->m_Distance.y = y;
	this->m_Distance.z = z;
}
void Camera_3rd::Pitch(float angle)
{
	vec3 viewDirection = m_ViewCenter - m_Position;
	viewDirection = glm::normalize(viewDirection);
	vec3 rightDirection = glm::cross(viewDirection, m_Up);
	rightDirection = glm::normalize(rightDirection);
	RotateView(angle, rightDirection.x, rightDirection.y, rightDirection.z);
}
void Camera_3rd::Yaw(float angle)
{
	RotateView(angle, m_Up.x, m_Up.y, m_Up.z);
}
void Camera_3rd::RotateView(float angle, float x, float y, float z) {
	vec3 viewDirection = m_ViewCenter - m_Position;
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