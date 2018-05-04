#include "RenderAble_Light.h"


void RenderAble_Light::SetLight_1(const Light& light1)
{
	INIT_TEST_VOID
		switch (light1.GetType())
	{
		case 0:
			break;
		case 1://方向光
			m_Shader.SetVec4("U_Light1_Dir", vec4(light1.GetRotate(), 0));
			m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
			m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
			m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
			m_Shader.SetVec4("U_Light1_Opt", 1, 0, 0, 32);
			break;
		case 2://点光源
			m_Shader.SetVec4("U_Light1_Pos", vec4(light1.GetPosition(), 1));
			m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
			m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
			m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
			m_Shader.SetFloat("U_Light1_Constant", light1.GetConstAttenuation());
			m_Shader.SetFloat("U_Light1_Linear", light1.GetLinearAttenuation());
			m_Shader.SetFloat("U_Light1_Quadric", light1.GetLinearAttenuation());
			m_Shader.SetVec4("U_Light1_Opt", 2, 0, 0, 32);
			break;
		case 3://聚光灯
			m_Shader.SetVec4("U_Light1_Pos", vec4(light1.GetPosition(), 1));
			m_Shader.SetVec4("U_Light1_Dir", vec4(light1.GetRotate(), 1));
			m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
			m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
			m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
			m_Shader.SetFloat("U_Light1_Constant", light1.GetConstAttenuation());
			m_Shader.SetFloat("U_Light1_Linear", light1.GetLinearAttenuation());
			m_Shader.SetFloat("U_Light1_Quadric", light1.GetLinearAttenuation());
			m_Shader.SetFloat("U_Light1_CutOff", light1.GetCutoff());
			m_Shader.SetFloat("U_Light1_Exponent", light1.GetExponent());
			m_Shader.SetVec4("U_Light1_Opt", 3, 0, 0, 32);
		default:
			break;
	}
}

void RenderAble_Light::SetAmbientMaterial(float r, float g, float b, float a)
{
	INIT_TEST_VOID
		m_Material.SetAmbientMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_AmbientMaterial", r, g, b, a);
}

void RenderAble_Light::SetDiffuseMaterial(float r, float g, float b, float a)
{
	INIT_TEST_VOID
		m_Material.SetDiffuseMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_DiffuseMaterial", r, g, b, a);
}

void RenderAble_Light::SetSpecularMaterial(float r, float g, float b, float a)
{
	INIT_TEST_VOID
		m_Material.SetSepcularMaterial(vec4(r, g, b, a));
	m_Shader.SetVec4("U_SpecularMaterial", r, g, b, a);
}
void RenderAble_Light::SetAmbientMaterial(const vec4& ambientMaterual)
{
	INIT_TEST_VOID
		m_Material.SetAmbientMaterial(ambientMaterual);
	m_Shader.SetVec4("U_AmbientMaterial", ambientMaterual.x, ambientMaterual.y, ambientMaterual.z, ambientMaterual.w);
}
void RenderAble_Light::SetDiffuseMaterial(const vec4& diffuseMaterual)
{
	INIT_TEST_VOID
		m_Material.SetDiffuseMaterial(diffuseMaterual);
	m_Shader.SetVec4("U_DiffuseMaterial", diffuseMaterual.x, diffuseMaterual.y, diffuseMaterual.z, diffuseMaterual.w);
}
void RenderAble_Light::SetSpecularMaterial(const vec4& specularMaterual)
{
	INIT_TEST_VOID
		m_Material.SetDiffuseMaterial(specularMaterual);
	m_Shader.SetVec4("U_SpecularMaterial", specularMaterual.x, specularMaterual.y, specularMaterual.z, specularMaterual.w);
}

void RenderAble_Light::Update(const vec3& cameraPos)
{
	INIT_TEST_VOID
	m_Shader.SetVec4("U_CameraPos", cameraPos.x, cameraPos.y, cameraPos.z, 1);
}