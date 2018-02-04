#include<iostream>
using namespace std;

class Parent{
public:
	Parent(int a)
	{
		this->a = a;
		cout << "a" << a << endl;
	}
	void printF()
	{
		cout << "Parent" << endl;
	}
protected:

private:
	int a;
};

class Child : public Parent{
public:
	Child(int b) :Parent(b)
	{
		this->b = b;
		cout << "b" << b << endl;
	}
private:
	int b;
};
