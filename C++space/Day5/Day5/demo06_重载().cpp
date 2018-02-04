#include <iostream>
using namespace std;

class F{
public:
	double operator()(double a, double b)
	{
		return a*a + b*b;
	}
};


int main()
{
	F f;
	cout<<f(2, 4)<<endl;
	
	//operator()(double a,double c)
	return 0;
}