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