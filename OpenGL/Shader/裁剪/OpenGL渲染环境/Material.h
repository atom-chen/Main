#pragma once
#include "ggl.h"

class Material
{
public:
	Material();
public:
	inline void SetAmbientMaterial(const vec4& ambientMaterial){ this->m_AmbientMaterial = ambientMaterial; };
	inline void SetAmbientMaterial(float r, float g, float b, float a = 1){ this->m_AmbientMaterial.x = r; this->m_AmbientMaterial.y = g; this->m_AmbientMaterial.z = b; this->m_AmbientMaterial.w = a; };
	inline const vec4& GetAmbientMaterial() const{ return this->m_AmbientMaterial; };

	inline void SetDiffuseMaterial(const vec4& diffuseMaterial){ this->m_DiffuseMaterial = diffuseMaterial; };
	inline void SetDiffuseMaterial(float r, float g, float b, float a = 1){ this->m_DiffuseMaterial.x = r; this->m_DiffuseMaterial.y = g; this->m_DiffuseMaterial.z = b; this->m_DiffuseMaterial.w = a; };
	inline const vec4& GetDiffuseMaterial() const{ return this->m_DiffuseMaterial; };

	inline void SetSepcularMaterial(const vec4& sepcularMaterial){ this->m_SpecularMaterial = sepcularMaterial; };
	inline void SetSepcularMaterial(float r, float g, float b, float a = 1){ this->m_SpecularMaterial.x = r; this->m_SpecularMaterial.y = g; this->m_SpecularMaterial.z = b; this->m_SpecularMaterial.w = a; };
	inline const vec4& GetSepcularMaterial() const{ return this->m_SpecularMaterial; };

protected:
private:
	vec4 m_AmbientMaterial;
	vec4 m_DiffuseMaterial;
	vec4 m_SpecularMaterial;
};