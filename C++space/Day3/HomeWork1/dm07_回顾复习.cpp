
#include <iostream>
using namespace std;

void objplaymain71();

class Test
{
public:
	Test()
	{
		a = 0; 
		b = 0;
		cout << "无参数构造函数  自动被调用" <<endl;
	}
	Test(int _a) //有参数构造函数
	{
		a = _a;
		b = 0;
	}

	Test(const Test& obj) //copy构造函数 作用: 用一个对象初始化另外一个对象
	{
		a = obj.a + 100;
		b = obj.b + 100;
	}
	void printT()
	{
		cout << "a:" << a << "b: "<<b<< endl; 
	}

	~Test()
	{
		cout<<"我是析构函数 对象生命周期结束时,会被c++编译器自动调用" <<endl;
 	}

protected:
private:
	int a;
	int b;
};

// 第3种调用时机
void printTest(Test t)
{
	;
}

// 1 和 2 
void objplaymain72()
{
	//
	Test t1(1); //ok
	Test t2(t1); 
	Test t3 = t1; //会调用copy构造函数
	printTest(t3);
}


//copy构造函数的第4种调用时机
//返回一个元素  匿名对象
Test getTestObj()
{
	Test t(1);
	return t;
}

void TestNoNameObj()
{
	Test  myt1 =getTestObj(); //用匿名对象初始化 另外一个对象  
	Test  myt2(1);
	myt2 = getTestObj(); //用匿名对象 给 另外一个对象 赋值 匿名对象被析构

}

void main()
{
	//objplaymain();
	objplaymain72();
	TestNoNameObj();
	cout<<"hello..."<<endl;
	system("pause");
}

void objplaymain71()
{
	Test t1; //ok
	//Test t2() ; //调用无参数构造函数的 错误方法
	//t2.printT();

	//

	Test  t3(1);	//c++编译器自动的调用构造函数
	Test t4 = 4;	//c++编译器自动的调用构造函数
	Test t5 = Test(5); //程序员手工的调用构造函数

	//
	Test t6 = t1;
	return ;
}
