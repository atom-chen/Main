#include<iostream>
#include <string>
using namespace std;
/*
class Fruit{
public:
	virtual void shoname() = 0;
};

class Apple :public Fruit{
public:
	virtual void shoname()
	{
		cout << "ƻ��Ŷ" << endl;
	}
private:

};

class Pear :public Fruit{
public:
	virtual void shoname()
	{
		cout << "��Ŷ" << endl;
	}
private:

};
class FruitFactory{
public:
	virtual Fruit* Create() = 0;
};

class PearFactory: public FruitFactory{
	virtual Fruit* Create()
	{
		return new Pear;
	}
};

class appleFactory : public FruitFactory{
	virtual Fruit* Create()
	{
		return new Apple;
	}
};

void main()
{
	FruitFactory *factory = new PearFactory;
	Fruit *fruit = NULL;
	fruit=factory->Create();
	fruit->shoname();

	factory = new appleFactory;
	fruit = factory->Create();
	fruit->shoname();

}
*/