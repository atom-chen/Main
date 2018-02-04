
#include<iostream>
using namespace std;
#include "Array.h"

//构造
template<class T>
Array<T>::Array(int length = 10)
{
	this->space = new T[length];
	this->length = length;
}
//拷贝构造
template<class T>
Array<T>::Array(const Array<T>& obj)
{
	//分配空间
	this->length = obj.length;
	this->space = new T[length];
	//拷贝
	for (int i = 0; i < length; i++)
	{
		this->space[i] = obj.space[i];
	}
}
//析构
template<class T>
Array<T>::~Array()
{
	if (this->space != NULL)
	{
		delete[] space;
		space = NULL;
		length = -1;
	}
}
//重载
template<class T>
T& Array<T>::operator[](int index)
{
	return space[index];
}
template<class T>
Array<T>& Array<T>::operator=(const Array<T> &obj)
{
	if (this->space != NULL)
	{
		delete[] space;
		space = NULL;
		length = -1;
	}
	length = obj.length;
	space = new T[length];
	for (int i = 0; i < length; i++)
	{
		space[i] = obj.space[i];
	}
}
template<class T>
ostream& operator<<(ostream& out,const Array<T> &obj)
{
	for (int i = 0; i < obj.length; i++)
	{
		out << obj.space[i] << " ";
	}
	out << endl;
	return out;
}
