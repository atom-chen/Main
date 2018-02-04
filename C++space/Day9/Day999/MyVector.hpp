#include<iostream>
using namespace std;
#include "MyVector.h"



template<class T>
MyVector<T>::MyVector(int size = 10)
{
	this->m_space = new T[size];
	m_len = size;
}
template<class T>
MyVector<T>::MyVector(const MyVector<T> &obj)
{
	if (m_space != NULL)
	{
		delete [] m_space;
		m_space = NULL;
		m_len = -1;
	}
	m_len = obj.m_len;
	m_space = new T[size];
	//拷贝数据
	for (int i = 0; i < m_len; i++)
	{
		m_space[i] = obj[i];
	}
}

template<class T>
MyVector<T>::~MyVector()
{
	if (m_space != NULL)
	{
		delete [] m_space;
		m_space = NULL;
		m_len = -1;
	}
}
template<class T>
T& MyVector<T>::operator[](int index)
{
	return m_space[index];
};
template<class T>
MyVector<T>& MyVector<T>::operator=(const MyVector<T> &obj)
{
	if (m_space != NULL)
	{
		delete[] m_space;
		m_space = NULL;
		m_len = -1;
	}
	m_len = obj.m_len;
	this->m_space = new T[m_len];
	//拷贝数据
	for (int i = 0; i < m_len; i++)
	{
		m_space[i] = obj.m_space[i];
	}
	return *this;
};
template<class T>
ostream& operator<<(ostream& out, const MyVector<T> &obj)
{
	
	for (int i = 0; i < obj.m_len; i++)
	{
		cout << obj.m_space[i] << " ";
	}
	cout << endl;
	
	return out;
}