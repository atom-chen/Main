#include<iostream>
#include <string>
#include<vector>
using namespace std;

class A
{
public:
	A(int a)
	{

	}
	A& operator=(const A& a)
	{
		cout << "operator=±»µ÷ÓÃ"<<endl;
		return *this;
	}
protected:
private:
	A();
};
void main()
{
	vector<A> v1;
	A a1(9);
	A a2(8);
	A a3(7);
	v1.insert(v1.begin(), a2);
	v1.emplace(v1.begin(),9);
}