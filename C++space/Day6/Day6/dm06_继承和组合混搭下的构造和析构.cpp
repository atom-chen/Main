
#include <iostream>
using namespace std;

class Object
{
public:
	Object(int a, int b)
	{
		this->a = a;
		this->b = b;
		cout<<"object���캯�� ִ�� "<<"a"<<a<<" b "<<b<<endl;
	}
	~Object()
	{
		cout<<"object�������� \n";
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
		cout<<"���๹�캯��..."<<p<<endl;
	}
	~Parent()
	{
		cout<<"��������..."<<p<<endl;
	}

	void printP(int a, int b)
	{
		cout<<"���ǵ�..."<<endl;
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
		cout<<"����Ĺ��캯��"<<myp<<endl;
	}
	~child()
	{
		cout<<"���������"<<myp<<endl;
	}
	void printC()
	{
		cout<<"���Ƕ���"<<endl;
	}
protected:
	char *myp;
	Object obj1;
	Object obj2;
};


void objplay()
{
	child c1("�̳в���");
}
void main()
{
	objplay();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}