
#include <iostream>
using namespace std;

//���캯���е����麯���ܷ�����̬��?

class Parent
{
public:
	Parent(int a=0)
	{
		this->a = a;
		print();
	}

	virtual void print()  
	{
		cout<<"���ǵ�"<<endl;
	}
	
private:
	int a;
};

class Child : public Parent
{
public:
	Child(int a = 0, int b=0):Parent(a)
	{
		this->b = b;
		print();
	}

	virtual void print()
	{
		cout<<"���Ƕ���"<<endl;
	}
private:
	int b;
};

void HowToPlay(Parent *base)
{
	base->print(); //�ж�̬����  //2 ���ֽ�  
	
}

void main()
{

	Child  c1; //����һ��������� ,�����������,�ڸ��๹�캯���е����麯��print �ܷ�����̬��?
	//c1.print();
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}