#pragma once
#include<iostream>
using namespace std;

template<class T>
class MyVector{
public:
	MyVector(int size = 0);
	MyVector(const MyVector &obj);
	~MyVector();
public:
	T& operator[](int index);
	MyVector& operator=(const MyVector &obj);
	friend ostream& operator<< <T>(ostream& out, const MyVector<T> &obj);
public:
	int size()
	{
		return m_len;
	}
protected:
private:
	T *m_space;
	int m_len;
};