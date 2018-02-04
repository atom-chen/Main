#pragma  once
#include<iostream>
using namespace std;

template<class T>
class Array{
public:
	//����
	Array(int length = 10);
	//��������
	Array(const Array& obj);
	//����
	~Array();
public:
	//����
	T& operator[](int index);
	Array& operator=(const Array &obj);
	friend ostream& operator<< <T>(ostream& out,const Array<T> &obj);
	int size()
	{
		return length;
	}
private:
	T *space;
	int length;
};