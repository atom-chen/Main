
#include <iostream>
using namespace std;

void myPrint(int a)
{
	printf("a:%d \n", a);
}

void myPrint(char *p)
{
	printf("%s \n", p);
}

void myPrint(int a, int b)
{
	printf("a:%d ", a);
	printf("b:%d \n", b);
}


/*
//����ֵ ���� �жϺ������صı�׼ 
int myPrint(int a, int b)
{
	printf("a:%d ", a);
	printf("b:%d \n", b);
}
*/

//1 ���������Ͳ�ͬ�Ĳ�������ʱ�����ĺ��岻ͬ
//2 �������ص��жϱ�׼
//���� ���� ����ֵ
//������ͬ ������һ��(����/����/)

//3 ����ֵ ���� �жϺ������صı�׼ ///

//4 ���غ����ĵ��ñ�׼
		//
void main1601()
{
	
	myPrint(1);
	myPrint("111222233aaaa");
	myPrint(1, 2);
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}

// ��������  ��  ����Ĭ�ϲ��� ��һ��

void myfunc(int a, int b, int c = 0)
{
	printf("a:%d b:%d c:%d \n", a, b, c);
}

void myfunc(int a, int b)
{
	printf("a:%d b:%d\n", a, b);
}

void myfunc(int a)
{
	printf("a:%d\n", a);
}
void main1602()
{
	//myfunc(1, 2); //��������ʱ,�����������
	myfunc(1);

	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}