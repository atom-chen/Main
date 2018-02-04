#pragma  once
#include<iostream>
using namespace std;

template<class T>
class Array{
public:
	//构造
	Array(int length = 10);
	//拷贝构造
	Array(const Array& obj);
	//析构
	~Array();
public:
	//重载
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