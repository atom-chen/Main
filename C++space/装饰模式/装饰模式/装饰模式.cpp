#include<iostream>
#include <string>
using namespace std;

class Car{
public:
	virtual void showCar() = 0;
};
class BMW :public Car{
public:	
	virtual void showCar()
	{
		cout << "跑车"<<endl;
	}
};
//具体功能
class flyCar :public Car{
public:
	flyCar(Car *car)
	{
		this->car = car;
	}
	//新的功能
	void fly()
	{
		cout << "新增了飞的功能"<<endl;
	}
	//虚函数重写
	virtual void showCar()
	{
		car->showCar();
		fly();
	}
private:
	//组合具体车
	Car *car;
};
//具体功能
class swimCar :public Car{
public:
	swimCar(Car *car)
	{
		this->car = car;
	}
	//新的功能
	void swim()
	{
		cout << "新增了游泳的功能" << endl;
	}
	//虚函数重写
	virtual void showCar()
	{
		car->showCar();
		swim();
	}
private:
	//给我一辆车，我帮你组装
	Car *car;
};

void main()
{
	Car *runcar = new BMW;
	Car *flycar = new flyCar(runcar);
	Car *swimcar = new swimCar(flycar);

	runcar->showCar();;
	flycar->showCar();
	swimcar->showCar();

}