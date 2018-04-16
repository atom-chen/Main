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
	if (!m_IsInit)
	{
		m_IsInit = 1;
		ResourceManager::GetModel(path, m_VertexBuf);

		m_Shader.Init(vertexShader, fragmentShader);
		m_Shader.SetVec4("U_CameraPos", 0, 0, 0, 1);
		SetAmbientMaterial(vec4(0.1f, 0.1f, 0.1f, 1.0f));
		SetDiffuseMaterial(vec4(0.6f, 0.6f, 0.6f, 1.0f));
		SetSpecularMaterial(vec4(1, 1, 1, 1.0f));
		m_Options.DepthTest = 1;
		m_Options.DrawType = DRAW_TRIANGLES;
	}
	return 1;
}
void GameObject::Update(const vec3& cameraPos)
{
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
		m_Shader.SetVec4("U_CameraPos", cameraPos.x, cameraPos.y, cameraPos.z, 1);
	}

}
void GameObject::Draw()
{
	if (m_IsInit)
	{
		SceneManager::DrawGameObject(this);
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