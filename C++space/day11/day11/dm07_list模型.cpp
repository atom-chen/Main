
#include <iostream>
using namespace std;
#include "list"


//list�Ļ�������
void main71()
{
	list<int> l;
	cout <<  "list�Ĵ�С:" << l.size() << endl;
	for (int i=0; i<10; i++)
	{
		l.push_back(i); //β������Ԫ�� β�巨
	}
	cout <<  "list�Ĵ�С:" << l.size() << endl;

	list<int>::iterator it = l.begin();

	while (it != l.end())
	{
		cout << *it << " ";
		it ++;
	}
	cout << endl;

	//list�����������
	//0	 1	2	3	4	5
//              ��
	it = l.begin();
	it ++;
	it ++ ;
	it ++ ;
	//it = it + 5;  //��֧������ķ�������
	l.insert(it, 100); //����100����������ôλ��
	for (list<int>::iterator it=l.begin(); it!=l.end(); it++)
	{
		cout << *it <<" ";
	}

	//����1 ����Ľ��index ����Ǵ�0��λ�ÿ�ʼ
	//		��3��λ�ò���Ԫ��, ��ԭ����3��λ�ñ��4��λ��  ԭ����4��λ�ñ��5��λ��

}

//list ɾ��
void main72()
{
	list<int> l;
	cout <<  "list�Ĵ�С:" << l.size() << endl;
	for (int i=0; i<10; i++)
	{
		l.push_back(i); //β������Ԫ�� β�巨
	}
	cout <<  "list�Ĵ�С:" << l.size() << endl;

	for (list<int>::iterator it=l.begin(); it!=l.end(); it++)
	{
		cout << *it <<" ";
	}
	cout << endl;

	//0	 1	2	3	4	5
	//          ��
	list<int>::iterator it1 = l.begin();
	list<int>::iterator it2 = l.begin();
	it2 ++ ;
	it2 ++ ;
	it2 ++ ;

	l.erase(it1, it2);

	for (list<int>::iterator it=l.begin(); it!=l.end(); it++)
	{
		cout << *it <<" ";
	}
	cout << endl;

	l.insert(l.begin(), 100);
	l.insert(l.begin(), 100);
	l.insert(l.begin(), 100);

	l.erase(l.begin()); //
	l.remove(100); //2
	for (list<int>::iterator it=l.begin(); it!=l.end(); it++)
	{
		cout << *it <<" ";
	}
	cout << endl;
}


void main777()
{
	//main71();
	main72();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}