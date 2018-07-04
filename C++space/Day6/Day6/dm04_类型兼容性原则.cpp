
#include <iostream>
using namespace std;

class Parent
{
public:
	void printP()
	{
		cout<<"我是爹..."<<endl;
	}

	Parent()
	{
		cout<<"parent构造函数"<<endl;
	}

	Parent(const Parent &obj)
	{
		cout<<"copy构造函数"<<endl;
	}

private:
	int a;
};

class child : public Parent
{
public:
	void printC()
	{
		cout<<"我是儿子"<<endl;
	}
protected:
private:
	int c;
};



/*
兼容规则中所指的替代包括以下情况：
	子类对象可以当作父类对象使用
	子类对象可以直接赋值给父类对象
	子类对象可以直接初始化父类对象
	父类指针可以直接指向子类对象
	父类引用可以直接引用子类对象
	*/



//C++编译器 是不会报错的 .....
void howToPrint(Parent *base)
{
	base->printP(); //父类的 成员函数 

}

void howToPrint2(Parent &base)
{
	base.printP(); //父类的 成员函数 
}
void main()
{
	//

	Parent p1;
	p1.printP();

	child c1;
	c1.printC();
	c1.printP();


	//赋值兼容性原则 
	//1-1 基类指针 (引用) 指向 子类对象
	Parent *p = NULL;
	p = &c1;
	p->printP();  

	//1-2 指针做函数参数

	howToPrint(&p1);
	howToPrint(&c1); 

	//1-3引用做函数参数
	howToPrint2(p1);
	howToPrint2(c1); 


	//第二层含义

	//可以让子类对象   初始化   父类对象
	//子类就是一种特殊的父类
	Parent p3 = c1;


	cout<<"hello..."<<endl;
	system("pause");
	return ;
}