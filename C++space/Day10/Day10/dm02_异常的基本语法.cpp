
#include <iostream>
using namespace std;

//1 �����﷨ 
//2 �����쳣֮��,�ǿ纯�� :
//3 �����쳣�Ժ� ���Բ����� ���׳��쳣
//4 catch�쳣��ʱ �������ͽ���catch

//5 �쳣��׽�ϸ�������ƥ��

void main()
{
	try
	{
		throw 'Z';
	}
	catch (int e)
	{
		cout << "����int�����쳣" << endl;
	}
	catch(...)
	{
		cout << " δ֪ �����쳣" << endl;
	}
	system("pause");
}


void divide(int x, int y)
{
	if (y == 0)
	{
		throw x; //�׳� int���� �쳣
	}

	cout << "divide���:" << x/y<< endl;
}

void myDivide(int x, int y)
{
	try
	{
		divide(x, y);
	}
	catch (...)
	{
		cout << "�ҽ����� divide���쳣 ������û�д��� �������׳�" << endl;
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
		cout << e << "�����" << endl;
	}
	catch ( ... )  //
	{
		cout <<  "����δ֪�����쳣 "<< endl;
	}
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}