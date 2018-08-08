#pragma  once
#include "ggl.h"
#include "RenderAble.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "Camera.h"
#include "Material.h"
#include "Light.h"


class GameObject:public RenderAble
{
public:
	GameObject();
	bool Init(const char* path, const char* vertexShader=nullptr, const char* fragmentShader=nullptr);
	void Update(const vec3& cameraPos);
	void Draw();
public:
	void SetLight_1(const Light& light1);
	inline void SetAmbientMaterial(float r, float g, float b, float a = 1);
	inline void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	inline void SetSpecularMaterial(float r, float g, float b, float a = 1);
	inline void SetAmbientMaterial(const vec4& ambientMaterual);
	inline void SetDiffuseMaterial(const vec4& diffuseMaterual);
	inline void SetSpecularMaterial(const vec4& specularMaterual);
	inline void SetTexture2D(const char* path, const char* nameInShader = "U_Texture_1"){ m_Shader.SetTexture2D(path, nameInShader); }
	inline void SetTexture2D(GLuint texture, const char* nameInShader = "U_Texture_1"){ m_Shader.SetTexture2D(texture, nameInShader); }

	inline void SetPosition(float x, float y, float z);
	inline vec3& GetPosition(){ return m_Transform.m_Position; };
	inline void SetRotate(float angle, float x, float y, float z);
	void SetScale(float x, float y, float z){ m_Transform.m_Scale.x = x; m_Transform.m_Scale.y = y; m_Transform.m_Scale.z = z; m_ModelMatrix = glm::translate(m_Transform.m_Position.x, m_Transform.m_Position.y, m_Transform.m_Position.z)* glm::scale(x, y, z); }

	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
protected:
private:
	//模型的材质
	Material m_Material;
	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
};