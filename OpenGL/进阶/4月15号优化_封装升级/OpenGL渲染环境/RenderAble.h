#pragma  once
#include "Node.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "Light.h"
#include "Material.h"

//绘制什么类型
enum DRAW_TYPE
{
	DRAW_TRIANGLES,
	DRAW_TRIANGLES_STRIP,
	DRAW_POINT,
	DRAW_QUADS,
};
//绘制选项
struct RenderOption
{
	DRAW_TYPE DrawType = DRAW_TRIANGLES;
	bool DepthTest=0;
	bool AlphaBlend=0;
	bool Program_Point_Size = 0;
};

//需要被渲染的对象继承此类
class RenderAble:public Node
{
public:
protected:
	RenderAble();
	virtual ~RenderAble();
public:
	void SetEnable(bool isDraw){ this->m_IsDraw = isDraw; }
	bool OnEnable(){ return this->m_IsDraw; }
	VertexBuffer& GetVertexBuffer(){ return m_VertexBuf; }
	Shader& GetShader(){ return m_Shader; }
	mat4& GetModelMatrix(){ return m_ModelMatrix; }
public:
	//渲染选项
	inline bool IsDepthTest()const{ return this->m_Options.DepthTest; }
	inline bool IsAlphaBlend()const{ return this->m_Options.AlphaBlend; }
	inline bool IsProgramPointSize() const{ return this->m_Options.Program_Point_Size; }
	inline DRAW_TYPE GetType()const{ return this->m_Options.DrawType; }
public:
	//设置Shader属性
	inline void SetTexture2D(const char* path, const char* nameInShader = "U_Texture_1"){ m_Shader.SetTexture2D(path, nameInShader); }
	inline void SetTexture2D(GLuint texture, const char* nameInShader = "U_Texture_1"){ m_Shader.SetTexture2D(texture, nameInShader); }
	void SetLight_1(const Light& light1);

	virtual inline void SetAmbientMaterial(float r, float g, float b, float a = 1);
	virtual inline void SetDiffuseMaterial(float r, float g, float b, float a = 1);
	virtual inline void SetSpecularMaterial(float r, float g, float b, float a = 1);
	virtual inline void SetAmbientMaterial(const vec4& ambientMaterual);
	virtual inline void SetDiffuseMaterial(const vec4& diffuseMaterual);
	virtual inline void SetSpecularMaterial(const vec4& specularMaterual);
public:
	//pos函数重写
	virtual void SetPosition(float x, float y, float z);
	virtual void SetRotate(float x, float y, float z);
	virtual void SetScale(float x, float y, float z);
private:
	void UpdateTransform();
protected:
	VertexBuffer m_VertexBuf;
	Shader m_Shader;
	glm::mat4 m_ModelMatrix;
	bool m_IsDraw = 1;
	Material m_Material;

	RenderOption m_Options;//渲染选项
};
