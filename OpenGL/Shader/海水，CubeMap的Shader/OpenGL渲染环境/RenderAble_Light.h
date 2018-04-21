#pragma once
#include "RenderAble.h"

class RenderAble_Light:public RenderAble
{
public:
	void SetLight_1(const Light& light1);

	void SetAmbientMaterial(float r, float g, float b, float a = 1);
	void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	void SetSpecularMaterial(float r, float g, float b, float a = 1);
	void SetAmbientMaterial(const vec4& ambientMaterual);
	void SetDiffuseMaterial(const vec4& diffuseMaterual);
	void SetSpecularMaterial(const vec4& specularMaterual);
protected:
	Material m_Material;
private:
};