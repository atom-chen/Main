
#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
using namespace std;
#include "MyString.h"

void main01()
{
	MyString s1;
	MyString s2("s2");
	MyString s2_2 = NULL;
	MyString s3 = s2;
	MyString s4 = "s4444444444";

	//������������� �� ����[]
	//=

	s4 = s2;
	s4 = "s2222";
	s4[1] = '4';
	printf("%c", s4[1]);

	cout<<s4 <<endl;
	//ostream& operator<<(ostream &out, MyString &s)
	
	//char& operator[] (int index)
	//MyString& operator=(const char *p);
	//MyString& operator=(const MyString &s);

	cout<<"hello..."<<endl;
	system("pause");
	return ;
}

void main02()
{
	MyString s1;
	MyString s2("s2");

	MyString s3 = s2;

	if (s2 == "aa")
	{
		printf("���");
	}
	else
	{
		printf("�����");
	}

	if (s3 == s2)
	{
		printf("���");
	}
	else
	{
		printf("�����");
	}
	
}
void main03()
{
	MyString s1;
	MyString s2("s2");

	MyString s3 = s2;
	s3 = "aaa";

	int tag = (s3 < "bbbb");
	if (tag < 0 )
	{
		printf("s3 С�� bbbb");
	}
	else
	{
		printf("s3 ���� bbbb");
	}

	MyString s4 = "aaaaffff";
	strcpy(s4.c_str(), "aa111"); //MFC
	cout<<s4<<endl;
}

void main011()
{
	MyString s1(128);
	cout<<"\n�������ַ���(�س�����)";
	cin>>s1;

	cout<<s1;
	system("pause");
	
}

void main()
{
	MyString s1(128);
	cout<<"\n�������ַ���(�س�����)";
	cin>>s1;

	cout<<s1<<endl;
	
	system("pause");

}