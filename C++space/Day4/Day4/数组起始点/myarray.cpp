
#include "MyArray.h"

//

//int m_length;
//char *m_space;
Array::Array(int length)
{
	if (length < 0)
	{
		length = 0; //
	}

	m_length = length;
	m_space = new int[m_length];
}

//Array a2 = a1;
Array::Array(const Array& obj)
{
	this->m_length = obj.m_length;
	this->m_space = new int[this->m_length]; //分配内存空间

	for (int i=0; i<m_length; i++) //数组元素复制
	{
		this->m_space[i] = obj.m_space[i];
	}
}
Array::~Array()
{
	if (m_space != NULL)
	{
		delete[] m_space;
		m_space = NULL;
		m_length = -1;
	}
}

//a1.setData(i, i);
void Array::setData(int index, int valude)
{
	m_space[index] = valude;
}
int Array::getData(int index)
{
	return m_space[index];
}
int Array::length()
{
	return m_length;
}
