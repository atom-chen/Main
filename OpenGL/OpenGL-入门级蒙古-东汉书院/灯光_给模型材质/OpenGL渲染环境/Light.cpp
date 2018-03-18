#include "Light.h"


void Light::SetAmbientColor(float r, float g, float b, float a)//设置环境照射颜色
{
	float ambientColor[] = { r, g, b, a };
	glLightfv(m_LightID, GL_AMBIENT, ambientColor);
}
void Light::SetDiffuseColor(float r, float g, float b, float a)//设置漫反射颜色
{
	float diffuseColor[] = { r, g, b, a };
	glLightfv(m_LightID, GL_DIFFUSE, diffuseColor);
}
void Light::SetSpecularColor(float r, float g, float b, float a)//设置镜面反射颜色
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
void DirectionLight::SetDirection(float x, float y, float z)//设置方向光的照射方向（位置在无穷远处）
{
	float position[] = { x, y, z, 0 };
	glLightfv(this->m_LightID, GL_POSITION, position);
}