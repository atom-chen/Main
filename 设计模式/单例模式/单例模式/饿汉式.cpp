#include<iostream>
#include <string>
using namespace std;

class Cat
{
public:
	static Cat*  getInstance()
	{
		return cat;
	}
protected:
private:
	static Cat *cat;
	static string name;
};
Cat *Cat::cat = new Cat;
string Cat::name = "°×°×";

void main()
{
	Cat *cat1 = Cat::getInstance();
	Cat *cat2 = Cat::getInstance();
	cout << cat1 << endl;
	cout<<cat2<<endl;

}