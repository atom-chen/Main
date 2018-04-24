#include "Light.h"

/*-------------------------------
------------灯光基类begin---------
---------------------------------*/
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
------------灯光基类end---------
---------------------------------*/


/*-------------------------------
------------方向光begin---------
---------------------------------*/
DirectionLight::DirectionLight(GLenum ID)
{
	this->m_LightID = ID;
}
void DirectionLight::SetDirection(float x, float y, float z)//设置方向光的照射方向（位置在无穷远处）
{
	float position[] = { x, y, z, 0 };
	glLightfv(this->m_LightID, GL_POSITION, position);
}
/*-------------------------------
------------方向光end---------
---------------------------------*/

/*-------------------------------
------------点光源begin---------
---------------------------------*/
PointLight::PointLight(GLenum ID)
{
	if (ID == NULL)
	{
		return;
	}
	this->m_LightID = ID;
}
void PointLight::SetPosition(float x, float y, float z)//设置位置
{
	float position[] = { x, y, z, 1 };
	glLightfv(m_LightID, GL_POSITION, position);
}
void PointLight::SetConstAttenuation(float v)//设置常数衰减因子
{
	glLightf(m_LightID, GL_CONSTANT_ATTENUATION, v);
}
void PointLight::SetLinearAttenuation(float v)//设置线性衰减因子
{
	glLightf(m_LightID, GL_LINEAR_ATTENUATION, v);
}
void PointLight::SetQuadricASttenuation(float v)//设置平方衰减因子
{
	glLightf(m_LightID, GL_QUADRATIC_ATTENUATION, v);
}
void PointLight::Update(Vector3 cameraPos)
{
	this->SetPosition(m_Position[0] - cameraPos.x, m_Position[1] - cameraPos.y, m_Position[2] - cameraPos.z);
}
/*-------------------------------
------------点光源end---------
---------------------------------*/


/*-------------------------------
------------聚光灯begin---------
---------------------------------*/
SpotLight::SpotLight(GLenum ID):PointLight(ID)
{

}

void SpotLight::SetDirection(float x, float y, float z)//设置聚光灯的照射方向
{
	float distance[] = { x, y, z };
	glLightfv(m_LightID, GL_SPOT_DIRECTION, distance);
}
void SpotLight::SetExponent(float v)//聚光灯不衰减角度
{
	glLightf(m_LightID, GL_SPOT_EXPONENT, v);
}
void SpotLight::SetCutoff(float v)//聚光灯可见角度
{
	glLightf(m_LightID, GL_SPOT_CUTOFF, v);
}
/*-------------------------------
------------聚光灯end---------
---------------------------------*/