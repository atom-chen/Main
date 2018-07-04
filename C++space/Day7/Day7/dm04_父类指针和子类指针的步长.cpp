
#include <iostream>
using namespace std;

//结论:
//多态是用父类指针指向子类对象 和 父类步长++,是两个不同的概念

class Parent
{
public:
	Parent(int a=0)
	{
		this->a = a;
	}

	virtual void print()  
	{
		cout<<"我是爹"<<endl;
	}

private:
	int a;
};


//成功 ,一次偶然的成功 ,必然的失败更可怕
class Child : public Parent
{
public:
	/*
	Child(int a = 0, int b=0):Parent(a)
	{
		this->b = b;
		print();
	}
	*/
	
	Child(int b = 0):Parent(0)
	{
		//this->b = b;
	}
	

	virtual void print()
	{
		cout<<"我是儿子"<<endl;
	}
private:
	//int b;
};

void HowToPlay(Parent *base)
{
	base->print(); //有多态发生  //2 动手脚  

}

void main411()
{

	Child  c1; //定义一个子类对象 ,在这个过程中,在父类构造函数中调用虚函数print 能发生多态吗?
	//c1.print();

	Parent *pP = NULL;
	Child  *pC = NULL;

	Child  array[] = {Child(1), Child(2), Child(3)};
	pP = array;
	pC = array;

	pP->print();
	pC->print(); //多态发生


	pP++;
	pC++;
	pP->print();
	pC->print(); //多态发生


	pP++;
	pC++;
	pP->print();
	pC->print(); //多态发生


	cout<<"hello..."<<endl;
	system("pause");
	return ;
}