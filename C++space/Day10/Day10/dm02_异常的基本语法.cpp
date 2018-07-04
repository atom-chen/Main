
#include <iostream>
using namespace std;

//1 基本语法 
//2 发生异常之后,是跨函数 :
//3 接受异常以后 可以不处理 再抛出异常
//4 catch异常的时 按照类型进行catch

//5 异常捕捉严格按照类型匹配

void main()
{
	try
	{
		throw 'Z';
	}
	catch (int e)
	{
		cout << "捕获int类型异常" << endl;
	}
	catch(...)
	{
		cout << " 未知 类型异常" << endl;
	}
	system("pause");
}


void divide(int x, int y)
{
	if (y == 0)
	{
		throw x; //抛出 int类型 异常
	}

	cout << "divide结果:" << x/y<< endl;
}

void myDivide(int x, int y)
{
	try
	{
		divide(x, y);
	}
	catch (...)
	{
		cout << "我接受了 divide的异常 但是我没有处理 我向上抛出" << endl;
		throw ;
	}
}


void main22()
{
	myDivide(100, 0);
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}

void main21()
{
	try
	{
		//divide(10, 2);
		//divide(100, 0);

		myDivide(100, 0);
	}
	catch (int e)
	{
		cout << e << "被零除" << endl;
	}
	catch ( ... )  //
	{
		cout <<  "其他未知类型异常 "<< endl;
	}
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}