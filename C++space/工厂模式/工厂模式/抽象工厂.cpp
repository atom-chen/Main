#include<iostream>
#include <string>
using namespace std;

class Fruit{
public:
	virtual void shoname() = 0;
};

class SouthApple :public Fruit{
public:
	virtual void shoname()
	{
		cout << "南方苹果哦" << endl;
	}
};
class NorthApple :public Fruit{
public:
	virtual void shoname()
	{
		cout << "北方苹果哦" << endl;
	}
};

class SouthPear :public Fruit{
public:
	virtual void shoname()
	{
		cout << "南方梨哦" << endl;
	}
};
class NorthPear :public Fruit{
public:
	virtual void shoname()
	{
		cout << "北方梨哦" << endl;
	}
};

class FruitFactory{
public:
	virtual Fruit* CreatePear() = 0;
	virtual Fruit* CreateApple() = 0;
};
class SouthFactory:public FruitFactory{
public:
	virtual Fruit* CreatePear()
	{
		return new SouthPear;
	}
	virtual Fruit* CreateApple()
	{
		return new SouthApple;
	}
};
class NorthFactory :public FruitFactory{
public:
	virtual Fruit* CreatePear()
	{
		return new NorthPear;
	}
	virtual Fruit* CreateApple()
	{
		return new NorthApple;
	}
};


void main()
{
	FruitFactory *factory = NULL;
	Fruit *fruit = NULL;

	factory = new SouthFactory;
	fruit = factory->CreateApple();
	fruit->shoname();
	fruit = factory->CreatePear();
	fruit->shoname();

	factory = new NorthFactory;
	fruit = factory->CreatePear();
	fruit->shoname();
	fruit = factory->CreateApple();
	fruit->shoname();
}