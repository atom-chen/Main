#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Utils.h"
#include "Shader.h"

class Ground
{
public:
	Ground();
	bool Init();
	void Draw(glm::mat4& viewMatrix,glm::mat4 &ProjectionMatrix);
	void SetAmbientMaterial(float r, float g, float b, float a = 1);
	void SetDiffuseMaterial(float r, float g, float b, float a = 1);
protected:
private:
	//材质属性
	float m_AmbientMaterial[4], m_DiffuseMaterial[4];

	VertexBuffer m_VertexBuf;
	Shader m_Shader;
	glm::mat4 m_ModelMatrix;//模型视口矩阵
};