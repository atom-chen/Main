
#include <iostream>
using namespace std;


class B
{
public:
	B()
	{
		cout<<"B构造函数执行\n";
	}
	int b;
protected:
private:

};

class B1 : virtual public B //12
{
public:
	int b1;

};

class B2 :    public B //8
{
public:
	int b2;
};

class C : public B1, public B2
{
public:
	int c;
};

void main()
{
	cout<<sizeof(B)<<endl; //4
	cout<<sizeof(B1)<<endl; //12 //加上virtual以后 , C++编译器会在给变量偷偷增加属性
	cout<<sizeof(B2)<<endl;  //8
	//cout<<sizeof(B)<<endl;

	system("pause");

}

void main1101()
{

	C  c1;
	c1.b1 = 100;
	c1.b2 = 200;
	c1.c = 300;

	//c1.b = 500; //继承的二义性 和 虚继承解决方案
	//c1.B1::b = 500;
	//c1.B2::b = 500;


	cout<<"hello..."<<endl;
	system("pause");
	return ;
}

class D1 
{
public:
	int k;
protected:
private:
};

class D2 
{
public:
	int k;
protected:
private:

};

class E :  public D1,  public D2
{
public:
protected:
private:
};

void main1202()
{
	 E e1;
	 e1.D1::k = 100;
	 e1.D2::k = 200;

	system("pause");
}