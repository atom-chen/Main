#include "GameObject.h"
#include "Utils.h"
#include "Resource.h"
#include "Time.h"
#include "Frustum.hpp"

GameObject::GameObject()
{

}
bool GameObject::Init(const char* path, const char* vertexShader, const char* fragmentShader)
{
	m_IsMoveToBottom = 0, m_IsMoveToLeft = 0, m_IsMoveToRight = 0, m_IsMoveToTop = 0;
	if (!m_IsInit)
	{
		m_IsInit = 1;
		if (ResourceManager::GetModel(path, m_VertexBuf))
		{
			m_ModelName = path;
			m_Shader.Init(vertexShader, fragmentShader);
			m_Shader.SetVec4("U_CameraPos", 0, 0, 0, 1);
			SetAmbientMaterial(vec4(0.1f, 0.1f, 0.1f, 1.0f));
			SetDiffuseMaterial(vec4(0.6f, 0.6f, 0.6f, 1.0f));
			SetSpecularMaterial(vec4(1, 1, 1, 1.0f));
			m_Options.DepthTest = 1;
			m_Options.DrawType = DRAW_TRIANGLES;
			return 1;
		}
	}
	return 0;
}
void GameObject::Destroy()
{
  INIT_TEST_VOID
	RenderAble::Destroy();
	ResourceManager::RemoveModel(m_ModelName);
	m_IsInit = 0;
}
void GameObject::Update(const vec3& cameraPos)
{
	RenderAble_Light::Update(cameraPos);
	if (m_IsInit)
	{
		const float& frameTime = Time::DeltaTime();
		if (m_IsMoveToRight)
		{
			float delta = frameTime*m_MoveSpeed;
			SetPosition(m_Transform.m_Position.x + delta, m_Transform.m_Position.y, m_Transform.m_Position.z);
		}
		if (m_IsMoveToLeft)
		{
			float delta = frameTime*m_MoveSpeed;
			SetPosition(m_Transform.m_Position.x - delta, m_Transform.m_Position.y, m_Transform.m_Position.z);
		}
		if (m_IsMoveToTop)
		{
			float delta = frameTime*m_MoveSpeed;
			SetPosition(m_Transform.m_Position.x, m_Transform.m_Position.y, m_Transform.m_Position.z - delta);
		}
		if (m_IsMoveToBottom)
		{
			float delta = frameTime*m_MoveSpeed;
			SetPosition(m_Transform.m_Position.x, m_Transform.m_Position.y, m_Transform.m_Position.z + delta);
		}
	}

}



void GameObject::MoveToLeft(bool isMove)
{
	m_IsMoveToLeft = isMove;
}
void GameObject::MoveToRight(bool isMove)
{
	m_IsMoveToRight = isMove;
}
void GameObject::MoveToTop(bool isMove)
{
	m_IsMoveToTop = isMove;
}
void GameObject::MoveToBottom(bool isMove)
{
	m_IsMoveToBottom = isMove;
}
void GameObject::SetMoveSpeed(float speed)
{
	this->m_MoveSpeed = speed;
}