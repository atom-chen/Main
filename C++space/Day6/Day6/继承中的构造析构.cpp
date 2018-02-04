#include<iostream>
using namespace std;
/*
class Parent{
public:
	void printP(int a,int b)
	{
		this->a = a;
		this->b = b;
		cout << "我是爸爸" << endl;
	}
	Parent(int a, int b)
	{
		this->a = a;
		this->b = b;
		cout << "父类构造函数" << endl;
	}
	Parent(const Parent& obj)
	{
		cout << "父类拷贝构造" << endl;
	}
	~Parent()
	{
		cout << "父类析构函数" << endl;
	}
private:
	int a,b;
};

class Child : public Parent{
public:
	void printC()
	{
		cout << "我是崽" << endl;
	}
	Child(int a,int b,int c) :Parent(a,b)
	{
		this->c = c;
		cout << "子类构造函数" << endl;
	};
	~Child()
	{
		cout << "子类析构函数" << endl;
	}
private:
	int c;
};

void haotoPrint(Parent *base)
{
	base->printP(1,2);
}


void mainplayer()
{
	Child(1,2,3);

	
}
void main44()
{
	mainplayer();
	system("pause");
}
*/