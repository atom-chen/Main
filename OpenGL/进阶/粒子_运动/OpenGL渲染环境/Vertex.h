#pragma once

#include "ggl.h"

//向GPU传的一条数据
class Vertex
{
public:
	Vertex();
	void CopyFrom(const Vertex& from);
	void operator=(const Vertex& from);
public:
	void SetPosition(const float& x, const float& y, const float& z=1, const float& w=1);
	void SetNormal(const float& x, const float& y, const float& z=1, const float& w=1);
	void SetTexcoord(const float& x, const float& y, const float& z=1, const float& w=1);
	void SetColor(const float& r, const float& g, const float& b, const float& a = 1);

	inline vec4 GetNormal() const{ return vec4(m_Normal[0], m_Normal[1], m_Normal[2], m_Normal[3]); };

protected:
private:
	float m_Position[4];
	float m_Color[4];
	float m_Texcoord[4];
	float m_Normal[4];
};

//传向GPU的数据集合
class VertexBuffer
{
public:
	VertexBuffer();
	bool Init(const int32_t& Lenth);
	~VertexBuffer();
public:
	void SetPosition(const int32_t& index,const float& x,const float& y,const float& z,const float& w=1);
	void SetColor(const int32_t& index, const float& r, const float& g, const float& b, const float& a=1);
	void SetNormal(const int32_t& index, const float& x, const float& y, const float& z=0, const float& w=1);
	void SetTexcoord(const int32_t& index, const float& x, const float& y, const float& z=0, const float& w=1);

	void SetPosition(const int32_t& index, const vec3& position);
	inline void SetColor(const int32_t& index, const vec4& color);
	inline void SetNormal(const int32_t& index, const vec4& normal);
	inline void SetTexcoord(const int32_t& index, const vec4& texcoord);
	Vertex* mutable_vertex();

	void Begin();//会把m_Vertex传过去显存
	void End();
protected:
public:
	inline const int32_t& GetLenth() const{ return m_Lenth; };
private:
	GLuint m_Vbo = _INVALID_ID_;
	Vertex *m_Vertex;
	int32_t m_Lenth = INVALID;
};

//EBO
class ElementBuffer
{
public:
	ElementBuffer();
	~ElementBuffer();
	void Init(const int32_t& count);
	void SetIndexes(const int32_t& indexInCPU,const int32_t& indexInGPU);//设置索引数据
public:
	void Begin();//会把m_Indexs传过去显存
	void End();
private:
	GLuint m_Ebo=_INVALID_ID_;
	int32_t* m_Indexs;
	int32_t m_Length=INVALID;
};

//画东西在texture上，实际绘制数据由VBO准备
class FrameBuffer
{
public:
	FrameBuffer();
	~FrameBuffer();
public:
	void Init(const unsigned& width, const unsigned& height);
public:
	void Begin();
	//在Begin和End之间使用VBO、EBO绘制
	void End();
public:
	GLuint GetTexture(){ return m_Texture; };
	
private:
	GLuint m_Fbo;
	GLuint m_DepthBuffer;//深度缓冲区
	GLuint m_Texture;//所用的2D纹理
	unsigned m_Width=0;
	unsigned m_Height = 0;
	
};