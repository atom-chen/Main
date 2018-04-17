#include "Light.h"

/*-------------------------------
------------灯光基类begin---------
---------------------------------*/
void Light::SetAmbientColor(float r, float g, float b, float a)//设置环境照射颜色
{
	this->m_Color.SetAmbientColor(r, g, b, a);
}
void Light::SetDiffuseColor(float r, float g, float b, float a)//设置漫反射颜色
{
	this->m_Color.SetDiffuseColor(r, g, b, a);
}
void Light::SetSpecularColor(float r, float g, float b, float a)//设置镜面反射颜色
{
	this->m_Color.SetSepcularColor(r, g, b, a);
}



void Light::SetConstAttenuation(float v)//设置常数衰减因子
{
	this->m_ConstAttenuation = v;
}
void Light::SetLinearAttenuation(float v)//设置线性衰减因子
{
	this->m_LinearAttenuation = v;
}
void Light::SetQuadricASttenuation(float v)//设置平方衰减因子
{
	this->m_QuadricASttenuation = v;
}

void Light::SetExponent(float v)//聚光灯不衰减角度
{
	this->m_Exponent = v;
}
void Light::SetCutoff(float v)//聚光灯可见角度
{
	this->m_Cutoff = v;
}

Light::Light() 
{
	SetType(LIGHT_TYPE::LIGHT_DIRECTION);
	SetAmbientColor(1, 1, 1, 1);
	SetDiffuseColor(1, 1, 1, 1);
	SetSpecularColor(1, 1, 1, 1);
	SetRotate(0, 80, 0);
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
------------灯光基类end---------
---------------------------------*/


/*-------------------------------
------------方向光begin---------
---------------------------------*/
//DirectionLight::DirectionLight() :Light()
//{
//
//}
//void DirectionLight::SetDirection(float x, float y, float z)//设置方向光的照射方向（位置在无穷远处）
//{
//	this->m_Direction = vec4(x, y, z, 0);
//}
//void DirectionLight::SetDirection(const vec3& direction)
//{
//	this->m_Direction = vec4(direction, 0);
//}
///*-------------------------------
//------------方向光end---------
//---------------------------------*/
//
///*-------------------------------
//------------点光源begin---------
//---------------------------------*/
//PointLight::PointLight() :Light()
//{
//
//}
//void PointLight::SetPosition(float x, float y, float z)//设置位置
//{
//	this->m_Position = vec4(x, y, z, 1);
//}
//void PointLight::SetPosition(const vec3& position)
//{
//	this->m_Position = vec4(position, 1);
//}
//void PointLight::SetConstAttenuation(float v)//设置常数衰减因子
//{
//	this->m_ConstAttenuation = v;
//}
//void PointLight::SetLinearAttenuation(float v)//设置线性衰减因子
//{
//	this->m_LinearAttenuation = v;
//}
//void PointLight::SetQuadricASttenuation(float v)//设置平方衰减因子
//{
//	this->m_QuadricASttenuation = v;
//}
//void PointLight::Update(const vec3& cameraPos)
//{
//	this->SetPosition(m_Position.x - cameraPos.x, m_Position.y - cameraPos.y, m_Position.z - cameraPos.z);
//}
///*-------------------------------
//------------点光源end---------
//---------------------------------*/
//
//
///*-------------------------------
//------------聚光灯begin---------
//---------------------------------*/
//SpotLight::SpotLight():PointLight()
//{
//
//}
//
//void SpotLight::SetDirection(float x, float y, float z)//设置聚光灯的照射方向
//{
//	this->m_Direction = vec4(x, y, z, 1);
//}
//void SpotLight::SetDirection(const vec3& direction)
//{
//	this->m_Direction = vec4(direction, 0);
//}
//void SpotLight::SetExponent(float v)//聚光灯不衰减角度
//{
//
//}
//void SpotLight::SetCutoff(float v)//聚光灯可见角度
//{
//
//}
///*-------------------------------
//------------聚光灯end---------
//---------------------------------*/