#pragma once
#include "Vertex.h"
//EBO
class ElementBuffer
{
public:
	ElementBuffer();
	~ElementBuffer();
	void Init(const int& count);
	void Destory();
	void SetIndexes(const int& indexInCPU, const int& indexInGPU);//������������
public:
	void Begin();//���m_Indexs����ȥ�Դ�
	void End();
private:
	GLuint m_Ebo = _INVALID_ID_;
	int* m_Indexs;
	int m_Length = INVALID;

	bool m_IsInit = false;
};