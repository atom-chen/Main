/*
#include <iostream>
using namespace std;

//����
class A
{
public:
	A()
	{
		cout << "A�Ĺ��캯��" << endl;
	}
public:
	
	static int a;
	int b;
	
public:
	
	void get()
	{
	cout<<"b "<<b<<endl;
	}
	void print()
	{
	cout<<"AAAAA "<<endl;
	}
	
protected:
private:
};

int A::a = 100; //��仰 ���Ǽ򵥵ı�����ֵ ����Ҫ���� Ҫ����C++������ ��Ҫ���ҷ����ڴ� ,���ټ̳����� �õ���a ��Ȼ�ᱨ��..



class B : private A
{
public:
	int b;
	int c;
	B(int n)
	{
		this->b = n;
	}
public:
	void get_child()
	{
		cout<<"b "<<b<<endl;
		//����ʹ��
		cout<<a<<endl;
		a++;
	}
void print()
{
	cout<<"BBBB "<<endl;
}
};

class C : private A
{
public:
	int b;
	int c;
	C(int n)
	{
		this->b = n;
	}
public:
	void get_child()
	{
		cout << "b " << b << endl;
		//����ʹ��
		cout << a << endl;
	}
	void print()
	{
		cout << "BBBB " << endl;
	}
};


//1 static�ؼ��� ����  ������ķ��ʿ��ƹ���

//2  ���Ǽ򵥵ı�����ֵ ����Ҫ���� Ҫ����C++������ ��Ҫ���ҷ����ڴ� ,���ټ̳����� �õ���a ��Ȼ�ᱨ��..

//3 A������ӹ��캯�� 
//A��Ĺ��캯����   A�Ĺ��캯����˽�еĹ��캯�� ... 
//�������̳�ҪС��....
//�������� .... UML


void main()
{
	A a1;
	a1.print();

	B b1(5);
	b1.get_child();

	C c1(1);
	c1.get_child();

	system("pause");
}

void main01()
{
	//B b1;
	//b1.a
}
*/