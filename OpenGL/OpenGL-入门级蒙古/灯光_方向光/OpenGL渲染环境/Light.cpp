#include "Light.h"


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

}
Light::Light()
{

}

DirectionLight::DirectionLight(GLenum ID)
{
	this->m_LightID = ID;
}
void DirectionLight::SetDirection(float x, float y, float z)//���÷��������䷽��λ��������Զ����
{
	float position[] = { x, y, z, 0 };
	glLightfv(this->m_LightID, GL_POSITION, position);
}