
#include <iostream>
using namespace std;
#include "MyArray.h"

//��Ŀ��������
//��Ĳ��԰���
void main5()
{
	
	Array  a1(10);

	for (int i=0; i<a1.length(); i++)
	{
		a1.setData(i, i);
	}

	cout<<"\n��ӡ����a1: ";
	for (int i=0; i<a1.length(); i++)
	{
		cout<<a1.getData(i)<<" ";
	}
	cout<<endl;

	
	Array a2 = a1;
	cout<<"\n��ӡ����a2: ";
	for (int i=0; i<a2.length(); i++)
	{
		cout<<a2.getData(i)<<" ";
	}
	cout<<endl;
	

	cout<<"hello..."<<endl;
	system("pause");
	return ;
}