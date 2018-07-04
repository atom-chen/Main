
#include <iostream>
using namespace std;
class Test
{
public:
	Test(int a, int b) //---> Test(Test *this, int a, int b)
	{
		this->a = a;
		this-> b = b;	
	}
	void printT()
	{
		cout<<"a: " <<a <<endl;
		cout<< "b: " << this->b <<endl;
	}
protected:
private:
	int a;
	int b;
};

void main()
{
	
	Test t1(1, 2);
	t1.printT();// ===> printT(&t1)
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}