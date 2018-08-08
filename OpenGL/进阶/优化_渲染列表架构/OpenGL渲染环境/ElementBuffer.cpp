#include "ElementBuffer.h"
#include "Utils.h"
#include "Resource.h"

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
	m_Ebo = ResourceManager::CreateBufferObject(GL_ELEMENT_ARRAY_BUFFER, sizeof(int32_t)*count, GL_STATIC_DRAW, nullptr); 
	m_IsInit = 1;
}
void ElementBuffer::Destory()
{
	ResourceManager::DeleteBufferObject(&m_Ebo);
}
void ElementBuffer::SetIndexes(const int32_t& indexInCPU, const int32_t& indexInGPU)//������������
{
	if (m_Ebo == _INVALID_ID_ || indexInCPU >= m_Length || !m_IsInit)
	{
		return;
	}
	m_Indexs[indexInCPU] = indexInGPU;
}

void ElementBuffer::Begin()//���m_Indexs����ȥ�Դ�
{
	if (m_Ebo == _INVALID_ID_ || !m_IsInit)
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