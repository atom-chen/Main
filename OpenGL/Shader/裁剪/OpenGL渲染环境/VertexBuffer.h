#pragma once
#include "Vertex.h"


//传向GPU的数据集合
class VertexBuffer
{
public:
	VertexBuffer();
	bool Init(const int& Lenth);
	void Destory();
	~VertexBuffer();
public:
	void SetPosition(const int& index, const float& x, const float& y, const float& z = 1, const float& w = 1);
	void SetColor(const int& index, const float& r, const float& g, const float& b, const float& a = 1);
	void SetNormal(const int& index, const float& x, const float& y, const float& z = 0, const float& w = 1);
	void SetTexcoord(const int& index, const float& x, const float& y, const float& z = 0, const float& w = 1);

	void SetPosition(const int& index, const vec3& position);
	void SetColor(const int& index, const vec4& color);
	void SetNormal(const int& index, const vec4& normal);
	void SetTexcoord(const int& index, const vec4& texcoord);

	Vertex* mutable_vertex();
	GLuint GetBuffer() const{ return m_Vbo; }

	void Begin();//会把m_Vertex传过去显存
	void End();
protected:
public:
	inline const int& GetLenth() const{ return m_Lenth; };
private:
	GLuint m_Vbo = _INVALID_ID_;
	Vertex *m_Vertex;
	int m_Lenth = INVALID;
	bool m_IsInit = false;
};