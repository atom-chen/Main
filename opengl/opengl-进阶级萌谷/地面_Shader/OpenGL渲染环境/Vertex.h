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
	VertexBuffer(const int32_t& Lenth);
	~VertexBuffer();
public:
	void SetPosition(const int32_t& index,const float& x,const float& y,const float& z,const float& w=1);
	void SetColor(const int32_t& index, const float& r, const float& g, const float& b, const float& a=1);
	void SetNormal(const int32_t& index, const float& x, const float& y, const float& z=0, const float& w=1);
	void SetTexcoord(const int32_t& index, const float& x, const float& y, const float& z=0, const float& w=1);
	Vertex* mutable_vertex();
protected:
private:
	Vertex *m_Vertex;
	int32_t m_Lenth;
};