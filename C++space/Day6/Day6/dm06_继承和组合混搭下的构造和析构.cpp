
#include <iostream>
using namespace std;

class Object
{
public:
	Object(int a, int b)
	{
		this->a = a;
		this->b = b;
		cout<<"object构造函数 执行 "<<"a"<<a<<" b "<<b<<endl;
	}
	~Object()
	{
		cout<<"object析构函数 \n";
	}
protected:
	int a;
	int b;
};


class Parent : public Object
{
public:
	Parent(char *p) : Object(1, 2)
	{
		this->p = p;
		cout<<"父类构造函数..."<<p<<endl;
	}
	~Parent()
	{
		cout<<"析构函数..."<<p<<endl;
	}

	void printP(int a, int b)
	{
		cout<<"我是爹..."<<endl;
	}

protected:
	char *p;
	
};


class child : public Parent
{
public:
	child(char *p) : Parent(p) , obj1(3, 4), obj2(5, 6)
	{
		this->myp = p;
		cout<<"子类的构造函数"<<myp<<endl;
	}
	~child()
	{
		cout<<"子类的析构"<<myp<<endl;
	}
	void printC()
	{
		cout<<"我是儿子"<<endl;
	}
protected:
	char *myp;
	Object obj1;
	Object obj2;
};


void objplay()
{
	child c1("继承测试");
}
void main()
{
	objplay();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}