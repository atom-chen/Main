#include<iostream>
using namespace std;


class programmer{
public:
	virtual float getSalary() = 0;
};

class JuniorProgrammer :public programmer{
public:
	virtual float getSalary()
	{
		return 8000;
	}
};

class IntermediateProgrammer :public programmer{
public:
	virtual float getSalary()
	{
		return 15000;
	}
};

class seniorProgrammer :public programmer{
public:
	virtual float getSalary()
	{
		return 30000;
	}
};

class architect :public programmer{
public:
	virtual float getSalary()
	{
		return 80000;
	}
};

void main33()
{
	programmer *it;
	JuniorProgrammer di;
	IntermediateProgrammer zhong;
	seniorProgrammer gao;
	architect jiagou;
	it = &zhong;
	cout << di.getSalary() << endl;

}