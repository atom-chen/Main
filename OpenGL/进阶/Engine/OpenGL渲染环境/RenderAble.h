#pragma  once
#include "Node.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "Light.h"
#include "Material.h"
#include "RenderOptions.h"



//需要被渲染的对象继承此类
class RenderAble:public Node
{
public:
protected:
	RenderAble();
	virtual ~RenderAble();
public:
	void SetEnable(bool isDraw){ this->m_IsDraw = isDraw; }
	bool IsEnable(){ return this->m_IsDraw; }
	VertexBuffer& GetVertexBuffer(){ return m_VertexBuf; }
	Shader& GetShader(){ return m_Shader; }
	mat4& GetModelMatrix(){ return m_ModelMatrix; }
public:
	//渲染选项
	inline bool IsDepthTest()const{ return this->m_Options.DepthTest; }
	inline AlphaBlendInfo GetAlphaBlend()const{ return this->m_Options.alphaBlend; }
	inline bool IsProgramPointSize() const{ return this->m_Options.Program_Point_Size; }
	inline DRAW_TYPE GetType()const{ return this->m_Options.DrawType; }
public:
	//设置Shader属性
	void InitShader(const char* vertexShader, const char* fragmentShader);
	inline void SetTexture2D(const char* path, bool isRepeat = 1,const char* nameInShader = "U_Texture_1")
	{
		INIT_TEST_VOID
			m_Shader.SetTexture2D(path, isRepeat, nameInShader);
	}
	void SetTexture2D(GLuint texture, const char* nameInShader = "U_Texture_1");
	inline void SetVec4(const char* nameInShader, const vec4& vec);
	void SetVec4(const char* nameInShader, float x, float y, float z, float w);
	void SetFloat(const char* nameInShader, float value);

public:
	//pos函数重写
	virtual void SetPosition(float x, float y, float z);
	void SetPosition(const vec3& pos);
	virtual void SetRotate(float x, float y, float z);
	void SetRotate(const vec3& rot);
	virtual void SetScale(float x, float y, float z);
	void SetScale(const vec3& scale);

	virtual void Draw();
	virtual void Destroy();
private:
	void UpdateTransform();

protected:
	bool m_IsDraw = 1;
	RenderOption m_Options;//渲染选项
	VertexBuffer m_VertexBuf;
	Shader m_Shader;
private:
	glm::mat4 m_ModelMatrix;

};
