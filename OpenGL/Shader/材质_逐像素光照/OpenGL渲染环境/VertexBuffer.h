#pragma once
#include "Vertex.h"

enum DRAR_TYPE
{
	TRIANGLES=0,
	TRIANGLE_STRIP=1,
	QUADS=2,
};
//传向GPU的数据集合
class VertexBuffer
{
public:
	VertexBuffer();
	bool Init(const int32_t& Lenth);
	void Destory();
	~VertexBuffer();
public:
	void SetPosition(const int32_t& index, const float& x, const float& y, const float& z = 1, const float& w = 1);
	void SetColor(const int32_t& index, const float& r, const float& g, const float& b, const float& a = 1);
	void SetNormal(const int32_t& index, const float& x, const float& y, const float& z = 0, const float& w = 1);
	void SetTexcoord(const int32_t& index, const float& x, const float& y, const float& z = 0, const float& w = 1);

	void SetPosition(const int32_t& index, const vec3& position);
	inline void SetColor(const int32_t& index, const vec4& color);
	inline void SetNormal(const int32_t& index, const vec4& normal);
	inline void SetTexcoord(const int32_t& index, const vec4& texcoord);

	Vertex* mutable_vertex();
	GLuint GetBuffer() const{ return m_Vbo; }

	void Begin();//会把m_Vertex传过去显存
	void End();
protected:
public:
	inline const int32_t& GetLenth() const{ return m_Lenth; };
private:
	GLuint m_Vbo = _INVALID_ID_;
	Vertex *m_Vertex;
	int32_t m_Lenth = INVALID;
	DRAR_TYPE m_DrawType = DRAR_TYPE::TRIANGLES;
	bool m_IsInit = false;
};