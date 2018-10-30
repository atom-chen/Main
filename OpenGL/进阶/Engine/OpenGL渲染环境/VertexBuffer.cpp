#include "VertexBuffer.h"
#include "Utils.h"
#include "Resource.h"




////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////VertexBuffer:Begin///////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////
VertexBuffer::VertexBuffer()
{

}
VertexBuffer::~VertexBuffer()
{

}
bool VertexBuffer::Init(const int& Lenth)
{
	if (!m_IsInit)
	{
		this->m_Vertex = new Vertex[Lenth];
		m_Lenth = Lenth;
		m_Vbo = ResourceManager::CreateBufferObject(GL_ARRAY_BUFFER, sizeof(Vertex)*Lenth, GL_STATIC_DRAW, nullptr);
		ASSERT_INT_BOOL(m_Vbo);
		m_IsInit = 1;
		return 1;
	}
	return 0;

}
void VertexBuffer::Destory()
{
	if (m_Vertex != nullptr)
	{
		delete[] m_Vertex;
		m_Vertex = nullptr;
		m_Lenth = INVALID;
	}
	ResourceManager::RemoveBufferObject(m_Vbo);
	m_IsInit = 0;
}

void VertexBuffer::Begin()
{
	if (m_Vbo == _INVALID_ID_ || !m_IsInit)
	{
		return;
	}
	glBindBuffer(GL_ARRAY_BUFFER, m_Vbo);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(Vertex)*m_Lenth, this->m_Vertex);
}
void VertexBuffer::End()
{
	glBindBuffer(GL_ARRAY_BUFFER, 0);
}



void VertexBuffer::SetPosition(const int& index, const float& x, const float& y, const float& z, const float& w)
{
	if (index >= this->m_Lenth || !m_IsInit)
	{
		return;
	}
	m_Vertex[index].SetPosition(x, y, z, w);
}
void VertexBuffer::SetPosition(const int& index, const vec3& position)
{
	this->SetPosition(index, position.x, position.y, position.z);
}

void VertexBuffer::SetColor(const int& index, const float& r, const float& g, const float& b, const float& a)
{
	if (index >= this->m_Lenth || !m_IsInit)
	{
		return;
	}
	m_Vertex[index].SetColor(r, g, b, a);
}
void VertexBuffer::SetColor(const int& index, const vec4& color)
{
	this->SetColor(index, color.x, color.y, color.z, color.w);
}

void VertexBuffer::SetNormal(const int& index, const float& x, const float& y, const float& z, const float& w)
{

	if (index >= this->m_Lenth || !m_IsInit)
	{
		return;
	}
	m_Vertex[index].SetNormal(x, y, z, w);
}
inline void VertexBuffer::SetNormal(const int& index, const vec4& normal)
{
	this->SetPosition(index, normal.x, normal.y, normal.z, normal.w);
}

void VertexBuffer::SetTexcoord(const int& index, const float& x, const float& y, const float& z , const float& w)
{
	if (index >= this->m_Lenth || !m_IsInit)
	{
		return;
	}
	m_Vertex[index].SetTexcoord(x, y, z, w);
}
inline void VertexBuffer::SetTexcoord(const int& index, const vec4& texcoord)
{
	this->SetPosition(index, texcoord.x, texcoord.y, texcoord.z, texcoord.w);
}


Vertex* VertexBuffer::mutable_vertex()
{
	return m_Vertex;
}
////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////VertexBuffer:End///////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////