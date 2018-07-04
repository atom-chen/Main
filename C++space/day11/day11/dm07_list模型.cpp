
#include <iostream>
using namespace std;
#include "list"


//list的基本操作
void main71()
{
	list<int> l;
	cout <<  "list的大小:" << l.size() << endl;
	for (int i=0; i<10; i++)
	{
		l.push_back(i); //尾部插入元素 尾插法
	}
	cout <<  "list的大小:" << l.size() << endl;

	list<int>::iterator it = l.begin();

	while (it != l.end())
	{
		cout << *it << " ";
		it ++;
	}
	cout << endl;

	//list不能随机访问
	//0	 1	2	3	4	5
//              ▲
	it = l.begin();
	it ++;
	it ++ ;
	it ++ ;
	//it = it + 5;  //不支持随机的访问容器
	l.insert(it, 100); //请问100插入在蛇那么位置
	for (list<int>::iterator it=l.begin(); it!=l.end(); it++)
	{
		cout << *it <<" ";
	}

	//结论1 链表的结点index 序号是从0号位置开始
	//		在3号位置插入元素, 让原来的3号位置变成4号位置  原来的4号位置变成5号位置

}

//list 删除
void main72()
{
	list<int> l;
	cout <<  "list的大小:" << l.size() << endl;
	for (int i=0; i<10; i++)
	{
		l.push_back(i); //尾部插入元素 尾插法
	}
	cout <<  "list的大小:" << l.size() << endl;

	for (list<int>::iterator it=l.begin(); it!=l.end(); it++)
	{
		cout << *it <<" ";
	}
	cout << endl;

	//0	 1	2	3	4	5
	//          ▲
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