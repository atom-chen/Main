
#include <iostream>
using namespace std;


class A
{
public:
	int a;
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

class B : public A
{
public:
	int b;
	int c;
public:
	void get_child()
	{
		cout<<"b "<<b<<endl;
	}
	void print()
	{
		cout<<"BBBB "<<endl;
	}
protected:
private:
};

void main()
{
	B b1;
	b1.print(); 

	b1.A::print();
	b1.B::print(); //Ĭ�����

	system("pause");
}

//ͬ����Ա����
void main71()
{
	
	B b1;
	b1.b = 1; //
	b1.get_child();

	b1.A::b = 100; //�޸ĸ����b
	b1.B::b = 200; //�޸������b Ĭ�������B

	b1.get();


	cout<<"hello..."<<endl;
	system("pause");
	return ;
}