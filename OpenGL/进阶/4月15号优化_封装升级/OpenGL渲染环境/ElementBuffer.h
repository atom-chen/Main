#pragma once
#include "Vertex.h"
//EBO
class ElementBuffer
{
public:
	ElementBuffer();
	~ElementBuffer();
	void Init(const int32_t& count);
	void Destory();
	void SetIndexes(const int32_t& indexInCPU, const int32_t& indexInGPU);//������������
public:
	void Begin();//���m_Indexs����ȥ�Դ�
	void End();
private:
	GLuint m_Ebo = _INVALID_ID_;
	int32_t* m_Indexs;
	int32_t m_Length = INVALID;

	bool m_IsInit = false;
};