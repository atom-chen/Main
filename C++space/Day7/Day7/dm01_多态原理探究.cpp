
#include <iostream>
using namespace std;

//��̬�������������� 
//Ҫ�м̳�  �麯����д  ����ָ��ָ��������� 

class Parent
{
public:
	Parent(int a=0)
	{
		this->a = a;
	}

	virtual void print()  //1 ���ֽ�  дvirtal�ؼ��� �����⴦�� //�麯����
	{
		cout<<"���ǵ�"<<endl;
	}
	virtual void print2()  //1 ���ֽ�  дvirtal�ؼ��� �����⴦�� //�麯����
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
	//Ч��:��������� ִ�������print���� ���������ִ�и����print���� 
	//C++��������������Ҫ������������� ���Ǹ������
	//���������������ֲ���vptrָ�� , ==>�麯����===>��������ڵ�ַ
	//�ٰ� (����ʱ��ʱ��,c++��������ȥ�ж�)
}

void main01()
{

	Parent	p1; //3 ���ֽ� ��ǰ����  
				//���ඨ������ʱ�� C++���������ڶ��������һ��vptrָ�� 
	Child	c1; //��������Ҳ��һ��vptrָ��

	HowToPlay(&p1);
	HowToPlay(&c1);

	cout<<"hello..."<<endl;
	system("pause");
	return ;
}