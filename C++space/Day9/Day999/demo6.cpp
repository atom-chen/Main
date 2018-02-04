#include <iostream>
using namespace std;

template<class T>
class A{
public:
	A(T t)
	{
		this->t = t;
	}
	T getT()
	{
		return t;
	}
	void setT(T t)
	{
		this->t=t;
	}
private:
	T t;

};
//从模板类派生时需要具体化模板类
class B :public A<double>{
public:
	B(double a=20,double b=30) :A<double>(a)
	{
		this->b = b;
	}
	void printB()
	{
		cout << "b" << b << endl;
	}
protected:
private:
	int b;

};

//类模板做函数参数
template<class A>
void UseA(A &a)
{
	cout<<a.getT()<<endl;
}

void main66()
{
	/*
	A<int> a;
	a.setT(1);
	A<int> a2;
	a2.setT(2);

	//UseA(a);
	//UseA(a2);
	cout << sizeof(A<string>) << endl;*/

	B b;
	b.printB();


}