#include "Vertex.h"



Vertex::Vertex()
{
	memset(m_Position, 0, sizeof(float) * 4);
	memset(m_Color, 0, sizeof(float) * 4);
	memset(m_Texcoord, 0, sizeof(float) * 4);
	memset(m_Normal, 0, sizeof(float) * 4);
}

void Vertex::SetPosition(const float& x, const float& y, const float& z, const float& w)
{
		m_Position[0] = x;
		m_Position[1] = y;
		m_Position[2] = z;
		m_Position[3] = w;
}

void Vertex::SetNormal(const float& x, const float& y, const float& z, const float& w)
{
		m_Normal[0] = x;
		m_Normal[1] = y;
		m_Normal[2] = z;
		m_Normal[3] = w;
}

void Vertex::SetTexcoord(const float& x, const float& y, const float& z, const float& w)
{
		m_Texcoord[0] = x;
		m_Texcoord[1] = y;
		m_Texcoord[2] = z;
		m_Texcoord[3] = w;
}

void Vertex::SetColor(const float& r, const float& g, const float& b, const float& a)
{
	m_Color[0] = r;
	m_Color[1] = g;
	m_Color[2] = b;
	m_Color[3] = a;
}

void Vertex::CopyFrom(const Vertex& from)
{
	for (int i = 0; i < 4; i++)
	{
		if (i >= ARRLEN(m_Normal) || i >= ARRLEN(m_Color) || i >= ARRLEN(m_Position) || i >= ARRLEN(m_Texcoord))
		{
			return;
		}
		this->m_Color[i] = from.m_Color[i];
		this->m_Normal[i] = from.m_Normal[i];
		this->m_Position[i] = from.m_Position[i];
		this->m_Texcoord[i] = from.m_Texcoord[i];
	}
}

void Vertex::operator = (const Vertex& from)
{
	this->CopyFrom(from);
}

VertexBuffer::VertexBuffer(const int32_t& Lenth)
{
	this->m_Vertex = new Vertex[Lenth];
	m_Lenth = Lenth;
}

VertexBuffer::~VertexBuffer()
{
	if (m_Vertex != nullptr)
	{
		delete[] m_Vertex;
		m_Vertex = nullptr;
		m_Lenth = INVALID;
	}
}

void VertexBuffer::SetPosition(const int32_t& index, const float& x, const float& y, const float& z, const float& w)
{
	if (index >= m_Lenth)
	{
		return;
	}
	else
	{
		m_Vertex[index].SetPosition(x, y, z, w);
	}
}

void VertexBuffer::SetColor(const int32_t& index, const float& r, const float& g, const float& b, const float& a)
{
	if (index >= m_Lenth)
	{
		return;
	}
	else
	{
		m_Vertex[index].SetColor(r, g, b, a);
	}
}

void VertexBuffer::SetNormal(const int32_t& index, const float& x, const float& y, const float& z, const float& w)
{
	if (index >= m_Lenth)
	{
		return;
	}
	else
	{
		m_Vertex[index].SetNormal(x, y, z, w);
	}
}

void VertexBuffer::SetTexcoord(const int32_t& index, const float& x, const float& y, const float& z , const float& w)
{
	if (index >= m_Lenth)
	{
		return;
	}
	else
	{
		m_Vertex[index].SetTexcoord(x, y, z, w);
	}
}

Vertex* VertexBuffer::mutable_vertex()
{
	return m_Vertex;
}