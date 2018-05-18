#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"


class Model
{
public:
	Model();
	bool Init(const char* path, const char* vertexShader=nullptr, const char* fragmentShader=nullptr);
	void Update(const float& frameTime, const vec3& cameraPos);
	void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
public:
	void SetAmbientMaterial(float r, float g, float b, float a = 1);
	void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	void SetSpecularMaterial(float r, float g, float b, float a = 1);
	void SetTexture2D(const char* path, const char* nameInShader = "U_Texture_1");

	virtual void SetPosition(float x, float y, float z);
	inline const vec3& GetPosition()const { return m_Position; };
	//void SetRotate(float angle);
	virtual void SetScale(float x, float y, float z);

	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
protected:
private:
	VertexBuffer m_VertexBuf;
	Shader m_Shader;

	//Transform
	vec3 m_Position, m_Rotate, m_Scale;
	glm::mat4 m_ModelMatrix;
	//模型的材质
	float m_AmbientMaterial[4], m_DiffuseMaterial[4], m_SpecularMaterial[4];

	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
};