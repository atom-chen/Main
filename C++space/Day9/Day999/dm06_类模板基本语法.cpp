
#include <iostream>
using namespace std;

//A编程模板 类 
//模板类  类型参数化


//类模板的定义
//类模板的使用
//类模板 做函数参数

//模板类 
template <class T>
class A
{
public:
	A(T a)
	{
		this->a = a;
	}
public:
	void printA()
	{
		cout << "a: " << a << endl;
	}
protected:
	T a;
};


//从模板类 派生了 普通类
// 模板类派生时, 需要具体化模板类. C++编译器需要知道 父类的数据类型具体是什么样子的
//=====> 要知道父类所占的内存大小是多少 只有数据类型固定下来,才知道如何分配内存 
class B : public A<int>
{
public:
	B(int a=10, int b=20) : A<int>(a)
	{
		this->b = b;
	}
	void printB()
	{
		cout << "a:" << a << " b: " << b << endl;
	}

protected:
private:
	int b;
};


//从模板类 派生 模板类

template <typename T>
class C : public A<T>
{
public:
	C(T c, T a) : A<T>(a)
	{
		this->c = c;
	}
	void printC()
	{
		cout << "c:" << c <<endl;
	}
protected:
	T c;
};

void main()
{
	C<int> c1(1, 2);
	c1.printC();
	
	system("pause");
}

//
void main62()
{
	B  b1(1, 2);
	b1.printB();
	system("pause");
}

//类模板 做函数参数

//参数 ,C++编译器 要求具体的类 所以所 要 A<int> &a 
void UseA( A<int> &a )
{
	a.printA();
}

void main61()
{
	//模板类(本身就是类型化的)====具体的类=====>定义具体的变量

	A<int> a1(11), a2(20), a3(30); //模板类是抽象的  ====>需要进行 类型具体
	//a1.printA();

	UseA(a1);
	UseA(a2);
	UseA(a3);
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}