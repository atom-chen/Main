#include<iostream>
using namespace std;
/*
class Object{
public:
	Object(int a, int b)
	{
		this->a = a;
		this->b = b;
		cout << "object构造函数" << "a=" << a << "b=" << b << endl;
	}
	~Object()
	{
		cout << "object析构函数" << "a=" << a << "b=" << b << endl;
	}
protected:
	int a, b;
};


class Parent:public Object{
public:
	void printP()
	{
		cout << "我是爸爸" << endl;
	}
	Parent(char *p) :Object(1,2)
	{
		this->p = p;
		cout << "父类构造函数" << endl;
	}
	~Parent()
	{
		cout << "父类析构函数..." << p << endl;
	}
protected:
	char *p;
};

class Child : public Parent{
public:
	Child(char *p) :Parent(p), o1(3,4), o2(5,6)
	{
		this->c = p;
		cout << "子类构造函数" <<c<< endl;
	};
	~Child()
	{
		cout << "子类的析构" << c << endl;
	}
	void printC()
	{
		cout << "我是崽" << endl;
	}
private:
	char* c;
	Object o1;
	Object o2;
};
void player()
{
	Child c1("<>");
}
void main()
{
	player();
	system("pause");
}
*/