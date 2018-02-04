#include<iostream>
using namespace std;
/*
class Parent{
public:
	void printP()
	{
		cout << "我是爸爸" << endl;
	}
	Parent(const Parent& obj)
	{
		cout << "父类拷贝构造" << endl;
	}
private:
	int a;
};

class Child : public Parent{
public:
	void printC()
	{
		cout << "我是崽" << endl;
	}
	Child()
	{

	};
private:
	int c;
};

void haotoPrint(Parent *base)
{
	base->printP();
}


void main33()
{	
	Child c1;
	Parent p1 = c1;
	//原则：
	//1、基类指针可以指向子类对象
	Parent *p = NULL;
	p = &c1;
	p->printP();
	haotoPrint(&c1);

	system("pause");
}
*/