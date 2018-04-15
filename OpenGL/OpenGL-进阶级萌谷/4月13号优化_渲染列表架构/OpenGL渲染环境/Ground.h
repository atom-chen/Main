#pragma  once
#include "ggl.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "Material.h"
#include "Light.h"
#include "RenderAble.h"

class Ground:public RenderAble
{
public:
	Ground();
	bool Init();
	bool Init(const char* picName);
	void Draw();
	void SetAmbientMaterial(float r, float g, float b, float a = 1);
	void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	inline void SetAmbientMaterial(const vec4& ambientMaterial){ SetAmbientMaterial(ambientMaterial.x, ambientMaterial.y, ambientMaterial.z, ambientMaterial.w); }
	inline void SetDiffuseMaterial(const vec4& diffuseMaterial){ SetDiffuseMaterial(diffuseMaterial.x, diffuseMaterial.y, diffuseMaterial.z, diffuseMaterial.w); };
	inline void SetTexture2D(const char* picName);
	inline void SetTexture2D(GLuint texture);
	void Destory();
public:
	void SetLight_1(const Light& light1);
protected:

private:
	//≤ƒ÷  Ù–‘
	Material m_Material;
	float height = -1;
};