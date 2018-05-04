#pragma once
#include "RenderAble.h"

class RenderAble_Light:public RenderAble
{
public:
	virtual void Update(const vec3& cameraPos);
	virtual void SetLight_1(const Light& light1);

	virtual void SetAmbientMaterial(float r, float g, float b, float a = 1);
	virtual void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	virtual void SetSpecularMaterial(float r, float g, float b, float a = 1);
	virtual void SetAmbientMaterial(const vec4& ambientMaterual);
	virtual void SetDiffuseMaterial(const vec4& diffuseMaterual);
	virtual void SetSpecularMaterial(const vec4& specularMaterual);
protected:
	Material m_Material;
private:
};