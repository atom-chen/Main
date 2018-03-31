#pragma  once
#include "ggl.h"
#include "Vector3.h"
#include "Vertex.h"
#include "Shader.h"


class Model
{
public:
	Model();
	bool Init(const char* path, const char* vertexShader=nullptr, const char* fragmentShader=nullptr);
	void Update(float frameTime);
	void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
public:
	void SetAmbientMaterial(float r, float g, float b, float a = 1);
	void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	void SetSpecularMaterial(float r, float g, float b, float a = 1);
	void SetTexture2D(const char* path, const char* nameInShader = "U_Texture_1");

	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);

	void SetPosition(float x, float y, float z);
protected:
private:
	VertexBuffer m_VertexBuf;
	Shader m_Shader;
	glm::mat4 m_ModelMatrix;

	//模型的材质
	float m_AmbientMaterial[4], m_DiffuseMaterial[4], m_SpecularMaterial[4];

	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
};