#include<iostream>
#include <string>
using namespace std;
class T1{
public:
	T1(int n)
	{
		cout << "t1的带n构造被调用"<<endl;
	}
};
class T2{
public:
	T2(int n)
	{
		cout << "t2的引用n构造被调用"<<endl;
	}
	T2& combine(const T2 &obj)
	{
		cout << "conbine被调用"<<endl;
		return *this;
	}
};
void main1()
{
	T1 t1=5;
	T2 t2(5);
	t2.combine(5);

}