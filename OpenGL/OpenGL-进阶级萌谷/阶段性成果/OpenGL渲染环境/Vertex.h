#pragma once

#include "ggl.h"

//��GPU����һ������
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


protected:
private:
	float m_Position[4];
	float m_Color[4];
	float m_Texcoord[4];
	float m_Normal[4];
};

//����GPU�����ݼ���
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
	Vertex* mutable_vertex();

	void Begin();//���m_Vertex����ȥ�Դ�
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
	void SetIndexes(const int32_t& indexInCPU,const int32_t& indexInGPU);//������������
public:
	void Begin();//���m_Indexs����ȥ�Դ�
	void End();
private:
	GLuint m_Ebo=_INVALID_ID_;
	int32_t* m_Indexs;
	int32_t m_Length=INVALID;
};

//��������texture�ϣ�ʵ�ʻ���������VBO׼��
class FrameBuffer
{
public:
	FrameBuffer();
	~FrameBuffer();
public:
	void Init(const unsigned& width, const unsigned& height);
public:
	void Begin();
	//��Begin��End֮��ʹ��VBO��EBO����
	void End();
public:
	GLuint GetTexture(){ return m_Texture; };
	
private:
	GLuint m_Fbo;
	GLuint m_DepthBuffer;//��Ȼ�����
	GLuint m_Texture;//���õ�2D����
	unsigned m_Width=0;
	unsigned m_Height = 0;
	
};