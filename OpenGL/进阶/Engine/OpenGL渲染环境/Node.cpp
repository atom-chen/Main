#include "Node.h"

void Node::SetPosition(float x, float y, float z)
{
	m_Transform.m_Position.x = x;
	m_Transform.m_Position.y = y;
	m_Transform.m_Position.z = z;
}
void Node::SetRotate(float x, float y, float z)
{
	m_Transform.m_Rotate.x = x;
	m_Transform.m_Rotate.y = y;
	m_Transform.m_Rotate.z = z;
}
void Node::SetScale(float x, float y, float z)
{
	m_Transform.m_Scale.x = x;
	m_Transform.m_Scale.y = y;
	m_Transform.m_Scale.z = z;
}