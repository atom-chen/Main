#include "RenderAble.h"

RenderAble::RenderAble()
{

}
RenderAble::~RenderAble()
{

}
void RenderAble::SetLight_1(const Light& light1)
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

void RenderAble::SetPosition(float x, float y, float z)
{
	Node::SetPosition(x, y, z);
	UpdateTransform();
}
void RenderAble::SetRotate(float x, float y, float z)
{
	Node::SetRotate(x, y, z);
	UpdateTransform();
}
void RenderAble::SetScale(float x, float y, float z)
{
	Node::SetScale(x, y, z);
	UpdateTransform();
}
void RenderAble::UpdateTransform()
{
	m_ModelMatrix = glm::translate(m_Transform.m_Position);
	m_ModelMatrix = glm::rotate(m_ModelMatrix, m_Transform.m_Rotate.x, vec3(1, 0, 0));
	m_ModelMatrix = glm::rotate(m_ModelMatrix, m_Transform.m_Rotate.y, vec3(0, 1, 0));
	m_ModelMatrix = glm::rotate(m_ModelMatrix, m_Transform.m_Rotate.z, vec3(0, 0, 1));
	m_ModelMatrix *= glm::scale(m_Transform.m_Scale);
}
void RenderAble::SetAmbientMaterial(float r, float g, float b, float a)
{
	m_Material.SetAmbientMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_AmbientMaterial", r, g, b, a);
}

void RenderAble::SetDiffuseMaterial(float r, float g, float b, float a)
{
	m_Material.SetDiffuseMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_DiffuseMaterial", r, g, b, a);
}

void RenderAble::SetSpecularMaterial(float r, float g, float b, float a)
{
	m_Material.SetSepcularMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_SpecularMaterial", r, g, b, a);
}
void RenderAble::SetAmbientMaterial(const vec4& ambientMaterual)
{
	m_Material.SetAmbientMaterial(ambientMaterual);
	m_Shader.SetVec4("U_AmbientMaterial", ambientMaterual.x, ambientMaterual.y, ambientMaterual.z, ambientMaterual.w);
}
void RenderAble::SetDiffuseMaterial(const vec4& diffuseMaterual)
{
	m_Material.SetDiffuseMaterial(diffuseMaterual);
	m_Shader.SetVec4("U_DiffuseMaterial", diffuseMaterual.x, diffuseMaterual.y, diffuseMaterual.z, diffuseMaterual.w);
}
void RenderAble::SetSpecularMaterial(const vec4& specularMaterual)
{
	m_Material.SetDiffuseMaterial(specularMaterual);
	m_Shader.SetVec4("U_SpecularMaterial", specularMaterual.x, specularMaterual.y, specularMaterual.z, specularMaterual.w);
}