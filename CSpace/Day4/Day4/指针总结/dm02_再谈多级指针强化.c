#define  _CRT_SECURE_NO_WARNINGS 
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

void getLen(int *p)
{
		*p = 30; //*˭�ĵ�ַ  �ͼ���޸�˭��ֵ 
}

void main01()
{
	int a = 10;
	int *p = NULL;

	a = 11;

	p = &a; 
	*p = 20;

	{
		*p = 30; //*˭�ĵ�ַ  �ͼ���޸�˭��ֵ 
	}
	printf("a:%d\n", a);
	system("pause");
	return ;
}


void getMem(char **p2 /*out*/)
{
	*p2  = 30; //��Ӹ�ֵ  p2��p�ĵ�ַ 

	*p2  = (char **)malloc(1000);
}

void main22()
{
	char *p = NULL;

	char **p2 = NULL;

	p = 1;
	p = 2;  //ֱ���޸�p��ֵ

	p2 = &p;
	*p2  = 20;

	{
		*p2  = 30; //��Ӹ�ֵ  p2��p�ĵ�ַ 
	}

	
	system("pause");
	return ;
}



int getMem2(char ***p3)
{
	*p3 = 100; //ֱ���޸Ķ���ָ���ֵ
}

void main03()
{

	char **p = NULL;

	char ***p3 = NULL;

	p = 1;
	p = 2;

	p3  = &p;

	*p3 = 10; //ֱ���޸Ķ���ָ���ֵ

	{
		*p3 = 30; //ֱ���޸Ķ���ָ���ֵ
	}

	getMem2(&p);
	system("pause");
	return ;

}

int getMem4(char ********p8)
{
		*p8 = 100; //* ���� p7 �ĵ�ַ ���� ��ӵ��޸���p7��ֵ
}

void main()
{
	char *******p7 = NULL;

	char ********p8 = NULL;

	p7 = 1;  //ֱ���޸�
	p7 = 2;

	p8 = &p7;

	*p8 = 10;

	{
		*p8 = 20; //* ���� p7 �ĵ�ַ ���� ��ӵ��޸���p7��ֵ
	}
	getMem4(&p7);

	printf("p7:%d \n", p7);

	system("pause");
	return ;

}



