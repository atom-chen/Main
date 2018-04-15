#include "GameObject.h"
#include "Utils.h"
#include "Resource.h"
#include "Time.h"
#include "Frustum.hpp"

GameObject::GameObject()
{
	m_ModelMatrix = glm::translate(this->m_Transform.m_Position.x, this->m_Transform.m_Position.y, this->m_Transform.m_Position.z)*glm::scale(m_Transform.m_Scale.x, m_Transform.m_Scale.y, m_Transform.m_Scale.z);
	m_Material.SetAmbientMaterial(vec4(0.1f, 0.1f, 0.1f, 1.0f));
	m_Material.SetDiffuseMaterial(vec4(0.6f, 0.6f, 0.6f, 1.0f));
	m_Material.SetSepcularMaterial(vec4(1,1, 1, 1.0f));
}
bool GameObject::Init(const char* path, const char* vertexShader, const char* fragmentShader)
{
	ResourceManager::GetModel(path,m_VertexBuf);
	
	if (vertexShader != nullptr)
	{
		if (fragmentShader != nullptr)
		{
			m_Shader.Init(vertexShader, fragmentShader);
		}
		else
		{
			m_Shader.Init(vertexShader, "res/obj.frag");
		}
	}
	else
	{
		m_Shader.Init("res/obj.vert", "res/obj.frag");
	}
	m_Shader.SetVec4("U_CameraPos", 0, 0, 0, 1);
	this->SetAmbientMaterial(m_Material.GetAmbientMaterial());
	this->SetDiffuseMaterial(m_Material.GetDiffuseMaterial());
	this->SetSpecularMaterial(m_Material.GetSepcularMaterial());
	m_Options.DepthTest = 1;
	m_Options.DrawType = DRAW_TRIANGLES;
	return 1;
}
void GameObject::Update(const vec3& cameraPos)
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
void GameObject::Draw()
{
	SceneManager::DrawGameObject(this);
}

void GameObject::SetPosition(float x, float y, float z)
{
	m_Transform.m_Position.x = x;
	m_Transform.m_Position.y = y;
	m_Transform.m_Position.z = z;
	m_ModelMatrix = glm::translate(x, y, z)*glm::scale(m_Transform.m_Scale.x, m_Transform.m_Scale.y, m_Transform.m_Scale.z);
	//m_ModelMatrix = glm::rotate(this->m_ModelMatrix, this->m_Rotate.w, vec3(this->m_Rotate.x, this->m_Rotate.y, this->m_Rotate.z));
}
void GameObject::SetRotate(float angle, float x, float y, float z)
{


	//m_ModelMatrix = glm::rotate(this->m_ModelMatrix, angle, vec3(x, y, z));
}




void GameObject::SetLight_1(const Light& light1)
{
	switch (light1.GetType())
	{
	case 0:
		break;
	case 1://方向光
		m_Shader.SetVec4("U_Light1_Pos", light1.GetDirection());
		m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
		m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
		m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
		m_Shader.SetVec4("U_Light1_Opt", 1, 0, 0, 32);
		break;
	case 2://点光源
		m_Shader.SetVec4("U_Light1_Pos", light1.GetPosition());
		m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
		m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
		m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
		m_Shader.SetFloat("U_Light1_Constant", light1.GetConstAttenuation());
		m_Shader.SetFloat("U_Light1_Linear", light1.GetLinearAttenuation());
		m_Shader.SetFloat("U_Light1_Quadric", light1.GetLinearAttenuation());
		m_Shader.SetVec4("U_Light1_Opt", 2, 0, 0, 32);
		break;
	case 3://聚光灯
		m_Shader.SetVec4("U_Light1_Pos", light1.GetPosition());
		m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
		m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
		m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
		m_Shader.SetFloat("U_Light1_Constant", light1.GetConstAttenuation());
		m_Shader.SetFloat("U_Light1_Linear", light1.GetLinearAttenuation());
		m_Shader.SetFloat("U_Light1_Quadric", light1.GetLinearAttenuation());
		m_Shader.SetVec4("U_Light1_Opt", light1.GetType(), 0, 0, 32);
	default:
		break;
	}
}

void GameObject::SetAmbientMaterial(float r, float g, float b, float a)
{
	m_Material.SetAmbientMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_AmbientMaterial", r,g,b,a);
}

void GameObject::SetDiffuseMaterial(float r, float g, float b, float a)
{
	m_Material.SetDiffuseMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_DiffuseMaterial",r,g,b,a);
}

void GameObject::SetSpecularMaterial(float r, float g, float b, float a)
{
	m_Material.SetSepcularMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_SpecularMaterial", r,g,b,a);
}
void GameObject::SetAmbientMaterial(const vec4& ambientMaterual)
{
	m_Material.SetAmbientMaterial(ambientMaterual);
	m_Shader.SetVec4("U_AmbientMaterial", ambientMaterual.x, ambientMaterual.y, ambientMaterual.z, ambientMaterual.w);
}
void GameObject::SetDiffuseMaterial(const vec4& diffuseMaterual)
{
	m_Material.SetDiffuseMaterial(diffuseMaterual);
	m_Shader.SetVec4("U_DiffuseMaterial", diffuseMaterual.x, diffuseMaterual.y, diffuseMaterual.z, diffuseMaterual.w);
}
void GameObject::SetSpecularMaterial(const vec4& specularMaterual)
{
	m_Material.SetDiffuseMaterial(specularMaterual);
	m_Shader.SetVec4("U_SpecularMaterial", specularMaterual.x, specularMaterual.y, specularMaterual.z, specularMaterual.w);
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