#include "Light.h"

/*-------------------------------
------------�ƹ����begin---------
---------------------------------*/
void Light::SetAmbientColor(float r, float g, float b, float a)//���û���������ɫ
{
	float ambientColor[] = { r, g, b, a };
	glLightfv(m_LightID, GL_AMBIENT, ambientColor);
}
void Light::SetDiffuseColor(float r, float g, float b, float a)//������������ɫ
{
	float diffuseColor[] = { r, g, b, a };
	glLightfv(m_LightID, GL_DIFFUSE, diffuseColor);
}
void Light::SetSpecularColor(float r, float g, float b, float a)//���þ��淴����ɫ
{
	float specularColor[] = { r, g, b, a };
	glLightfv(m_LightID, GL_SPECULAR, specularColor);
}
void Light::Enable(bool isOpen)
{
	if (isOpen)
	{
		glEnable(GL_LIGHTING);
		glEnable(m_LightID);
	}
	else
	{
		glDisable(m_LightID);
	}
	m_IsEnable = isOpen;

}

bool Light::IsEnable()
{
	return m_IsEnable;
}

Light::Light()
{

}

/*-------------------------------
------------�ƹ����end---------
---------------------------------*/


/*-------------------------------
------------�����begin---------
---------------------------------*/
DirectionLight::DirectionLight(GLenum ID)
{
	this->m_LightID = ID;
}
void DirectionLight::SetDirection(float x, float y, float z)//���÷��������䷽��λ��������Զ����
{
	float position[] = { x, y, z, 0 };
	glLightfv(this->m_LightID, GL_POSITION, position);
}
/*-------------------------------
------------�����end---------
---------------------------------*/

/*-------------------------------
------------���Դbegin---------
---------------------------------*/
PointLight::PointLight(GLenum ID)
{
	if (ID == NULL)
	{
		return;
	}
	this->m_LightID = ID;
}
void PointLight::SetPosition(float x, float y, float z)//����λ��
{
	float position[] = { x, y, z, 1 };
	glLightfv(m_LightID, GL_POSITION, position);
}
void PointLight::SetConstAttenuation(float v)//���ó���˥������
{
	glLightf(m_LightID, GL_CONSTANT_ATTENUATION, v);
}
void PointLight::SetLinearAttenuation(float v)//��������˥������
{
	glLightf(m_LightID, GL_LINEAR_ATTENUATION, v);
}
void PointLight::SetQuadricASttenuation(float v)//����ƽ��˥������
{
	glLightf(m_LightID, GL_QUADRATIC_ATTENUATION, v);
}
void PointLight::Update(vec3 cameraPos)
{
	this->SetPosition(m_Position[0] - cameraPos.x, m_Position[1] - cameraPos.y, m_Position[2] - cameraPos.z);
}
/*-------------------------------
------------���Դend---------
---------------------------------*/


/*-------------------------------
------------�۹��begin---------
---------------------------------*/
SpotLight::SpotLight(GLenum ID):PointLight(ID)
{

}

void SpotLight::SetDirection(float x, float y, float z)//���þ۹�Ƶ����䷽��
{
	float distance[] = { x, y, z };
	glLightfv(m_LightID, GL_SPOT_DIRECTION, distance);
}
void SpotLight::SetExponent(float v)//�۹�Ʋ�˥���Ƕ�
{
	glLightf(m_LightID, GL_SPOT_EXPONENT, v);
}
void SpotLight::SetCutoff(float v)//�۹�ƿɼ��Ƕ�
{
	glLightf(m_LightID, GL_SPOT_CUTOFF, v);
}
/*-------------------------------
------------�۹��end---------
---------------------------------*/