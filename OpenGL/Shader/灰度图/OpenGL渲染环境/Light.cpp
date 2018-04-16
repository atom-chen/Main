#include "Light.h"

/*-------------------------------
------------�ƹ����begin---------
---------------------------------*/
void Light::SetAmbientColor(float r, float g, float b, float a)//���û���������ɫ
{
	this->m_Color.SetAmbientColor(r, g, b, a);
}
void Light::SetDiffuseColor(float r, float g, float b, float a)//������������ɫ
{
	this->m_Color.SetDiffuseColor(r, g, b, a);
}
void Light::SetSpecularColor(float r, float g, float b, float a)//���þ��淴����ɫ
{
	this->m_Color.SetSepcularColor(r, g, b, a);
}



void Light::SetConstAttenuation(float v)//���ó���˥������
{
	this->m_ConstAttenuation = v;
}
void Light::SetLinearAttenuation(float v)//��������˥������
{
	this->m_LinearAttenuation = v;
}
void Light::SetQuadricASttenuation(float v)//����ƽ��˥������
{
	this->m_QuadricASttenuation = v;
}

void Light::SetExponent(float v)//�۹�Ʋ�˥���Ƕ�
{
	this->m_Exponent = v;
}
void Light::SetCutoff(float v)//�۹�ƿɼ��Ƕ�
{
	this->m_Cutoff = v;
}

Light::Light() 
{
	SetType(LIGHT_TYPE::LIGHT_DIRECTION);
	SetAmbientColor(1, 1, 1, 1);
	SetDiffuseColor(1, 1, 1, 1);
	SetSpecularColor(1, 1, 1, 1);
	SetRotate(0, 45, 0);
}

void Light::SetType(LIGHT_TYPE type)
{
	this->m_Type = type;
	switch (type)
	{
	case LIGHT_INVALID:
		m_Color = LightColor();
		break;
	case LIGHT_DIRECTION:
		break;
	case LIGHT_POINT:
		break;
	case LIGHT_SPECULAR:
		break;
	default:
		break;
	}
}

/*-------------------------------
------------�ƹ����end---------
---------------------------------*/


/*-------------------------------
------------�����begin---------
---------------------------------*/
//DirectionLight::DirectionLight() :Light()
//{
//
//}
//void DirectionLight::SetDirection(float x, float y, float z)//���÷��������䷽��λ��������Զ����
//{
//	this->m_Direction = vec4(x, y, z, 0);
//}
//void DirectionLight::SetDirection(const vec3& direction)
//{
//	this->m_Direction = vec4(direction, 0);
//}
///*-------------------------------
//------------�����end---------
//---------------------------------*/
//
///*-------------------------------
//------------���Դbegin---------
//---------------------------------*/
//PointLight::PointLight() :Light()
//{
//
//}
//void PointLight::SetPosition(float x, float y, float z)//����λ��
//{
//	this->m_Position = vec4(x, y, z, 1);
//}
//void PointLight::SetPosition(const vec3& position)
//{
//	this->m_Position = vec4(position, 1);
//}
//void PointLight::SetConstAttenuation(float v)//���ó���˥������
//{
//	this->m_ConstAttenuation = v;
//}
//void PointLight::SetLinearAttenuation(float v)//��������˥������
//{
//	this->m_LinearAttenuation = v;
//}
//void PointLight::SetQuadricASttenuation(float v)//����ƽ��˥������
//{
//	this->m_QuadricASttenuation = v;
//}
//void PointLight::Update(const vec3& cameraPos)
//{
//	this->SetPosition(m_Position.x - cameraPos.x, m_Position.y - cameraPos.y, m_Position.z - cameraPos.z);
//}
///*-------------------------------
//------------���Դend---------
//---------------------------------*/
//
//
///*-------------------------------
//------------�۹��begin---------
//---------------------------------*/
//SpotLight::SpotLight():PointLight()
//{
//
//}
//
//void SpotLight::SetDirection(float x, float y, float z)//���þ۹�Ƶ����䷽��
//{
//	this->m_Direction = vec4(x, y, z, 1);
//}
//void SpotLight::SetDirection(const vec3& direction)
//{
//	this->m_Direction = vec4(direction, 0);
//}
//void SpotLight::SetExponent(float v)//�۹�Ʋ�˥���Ƕ�
//{
//
//}
//void SpotLight::SetCutoff(float v)//�۹�ƿɼ��Ƕ�
//{
//
//}
///*-------------------------------
//------------�۹��end---------
//---------------------------------*/