#include "Vertex.h"
#include "Utils.h"

ElementBuffer::ElementBuffer()
{
	
}

ElementBuffer::~ElementBuffer()
{
	if (m_Indexs != nullptr)
	{
		delete[] m_Indexs;
		m_Indexs = nullptr;
		m_Length = INVALID;
	}
	glDeleteBuffers(1, &m_Ebo);
}

void ElementBuffer::Init(const int32_t& count)
{
	m_Indexs = new int32_t[count];
	this->m_Length = count;
	m_Ebo = CreateBufferObject(GL_ELEMENT_ARRAY_BUFFER, sizeof(int32_t)*count, GL_STATIC_DRAW, nullptr);
}

void ElementBuffer::SetIndexes(const int32_t& indexInCPU, const int32_t& indexInGPU)//设置索引数据
{
	if (m_Ebo == _INVALID_ID_ || indexInCPU>=m_Length)
	{
		return;
	}
	m_Indexs[indexInCPU] = indexInGPU;
}

void ElementBuffer::Begin()//会把m_Indexs传过去显存
{
	if (m_Ebo == _INVALID_ID_)
	{
		return;
	}
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_Ebo);
	glBufferSubData(GL_ELEMENT_ARRAY_BUFFER, 0, sizeof(int32_t)*m_Length, m_Indexs);
}

void ElementBuffer::End()
{
	if (m_Ebo == _INVALID_ID_)
	{
		return;
	}
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _INVALID_ID_);
}