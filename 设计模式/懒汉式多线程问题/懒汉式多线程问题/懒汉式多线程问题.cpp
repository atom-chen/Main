// 懒汉式多线程问题.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>
using namespace std;
class SingeIton{
private:
	SingeIton()
	{

	}
public:
	SingeIton& getSingeIton()
	{
		if (singIton == NULL)
		{
			//new一个
			singIton = new SingeIton();
		}
		return *singIton;
	}
private:
	static SingeIton* singIton;
};

int _tmain(int argc, _TCHAR* argv[])
{
	cout << "Hello" << endl;
	system("pause");
	return 0;
}

