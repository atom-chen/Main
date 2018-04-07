#pragma once
#include "ggl.h"

class Material
{
public:
	void SetAmbientMaterial(const vec4& ambientMaterial){ this->m_AmbientMaterial = ambientMaterial; };
	const vec4& GetAmbientMaterial() const{ return this->m_AmbientMaterial; };

	void SetDiffuseMaterial(const vec4& diffuseMaterial){ this->m_DiffuseMaterial = diffuseMaterial; };
	const vec4& GetDiffuseMaterial() const{ return this->m_DiffuseMaterial; };

	void SetSepcularMaterial(const vec4& sepcularMaterial){ this->m_SpecularMaterial = sepcularMaterial; };
	const vec4& GetSepcularMaterial() const{ return this->m_SpecularMaterial; };

protected:
private:
	vec4 m_AmbientMaterial;
	vec4 m_DiffuseMaterial;
	vec4 m_SpecularMaterial;
};