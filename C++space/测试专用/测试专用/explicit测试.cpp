#include<iostream>
#include <string>
using namespace std;
class T1{
public:
	T1(int n)
	{
		cout << "t1�Ĵ�n���챻����"<<endl;
	}
};
class T2{
public:
	T2(int n)
	{
		cout << "t2������n���챻����"<<endl;
	}
	T2& combine(const T2 &obj)
	{
		cout << "conbine������"<<endl;
		return *this;
	}
};
void main1()
{
	T1 t1=5;
	T2 t2(5);
	t2.combine(5);

}