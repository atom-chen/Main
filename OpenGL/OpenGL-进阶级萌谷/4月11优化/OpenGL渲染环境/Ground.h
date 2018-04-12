#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"
#include "Material.h"
#include "Light.h"

class Ground
{
public:
	Ground();
	bool Init();
	bool Init(const char* picName);
	void Draw(glm::mat4& viewMatrix,glm::mat4 &ProjectionMatrix);
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
	//材质属性
	Material m_Material;

	VertexBuffer m_VertexBuf;
	Shader m_Shader;
	glm::mat4 m_ModelMatrix;//模型视口矩阵
	float height = -1;
};