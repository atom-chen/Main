
#include <iostream>
using namespace std;

void myPrint(int x = 3)
{
	cout<<"x"<<x<<endl;
}

//1 �� ����д����,ʹ������д��,����дĬ��
//2 ��Ĭ�ϲ������� �����Ĭ�ϲ������֣���ô�ұߵĶ�������Ĭ�ϲ���
void myPrint2( int m, int n, int x = 3, int y = 4)
//void myPrint2( int m, int n, int x = 3, int y )
{
	cout<<"x"<<x<<endl;
}

void main_Ĭ�ϲ���()
{
	
	myPrint(4);
	myPrint(); 

	//
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}

//����ռλ���� ����������,����д������

void func1(int a, int b, int)
{
	cout<<"a"<<a<<" b"<<b<<endl;
}

void main_ռλ����()
{
	
	//func1(1, 2); //err���ò�����
	func1(1, 2, 3);

	cout<<"hello..."<<endl;
	system("pause");
	return ;
}

//Ĭ�ϲ�����ռλ����

void  func2(int a, int b, int =0)
{
	cout<<"a="<<a<<";b="<<b<<endl;
}

void main()
{
	func2(1, 2); //0k
	func2(1, 2, 3); //ok
	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}