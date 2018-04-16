#pragma once
#include "ggl.h"

class LightColor
{
public:
	LightColor();
public:
	
	void SetAmbientColor(const vec4& ambientColor){ this->m_AmbientColor = ambientColor; };
	void SetAmbientColor(float r, float g, float b, float a = 1){ this->m_AmbientColor.x = r; this->m_AmbientColor.y = g; this->m_AmbientColor.z = b; this->m_AmbientColor.w = a; };
	const vec4& GetAmbientColor() const{ return this->m_AmbientColor; };

	void SetDiffuseColor(const vec4& diffuseColor){ this->m_DiffuseColor = diffuseColor; };
	void SetDiffuseColor(float r, float g, float b, float a = 1){ this->m_DiffuseColor.x = r; this->m_DiffuseColor.y = g; this->m_DiffuseColor.z = b; this->m_DiffuseColor.w = a; };
	const vec4& GetDiffuseColor() const{ return this->m_DiffuseColor; };

	void SetSepcularColor(const vec4& sepcularColor){ this->m_SpecularColor = sepcularColor; };
	void SetSepcularColor(float r, float g, float b, float a = 1){ this->m_SpecularColor.x = r; this->m_SpecularColor.y = g; this->m_SpecularColor.z = b; this->m_SpecularColor.w = a; };
	const vec4& GetSepcularColor() const{ return this->m_SpecularColor; };
protected:
private:
	vec4 m_AmbientColor;
	vec4 m_DiffuseColor;
	vec4 m_SpecularColor;
};