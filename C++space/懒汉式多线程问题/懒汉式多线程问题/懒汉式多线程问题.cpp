// ����ʽ���߳�����.cpp : �������̨Ӧ�ó������ڵ㡣
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
			//newһ��
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

