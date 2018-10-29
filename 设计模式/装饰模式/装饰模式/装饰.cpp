#include "装饰.h"

flyCar::flyCar(Car *car)
{
	this->car = car;
}
//新的功能
void flyCar::fly()
{
	cout << "新增了飞的功能" << endl;
}
//虚函数重写
void flyCar::showCar()
{
	car->showCar();
	fly();
}

//具体功能
swimCar::swimCar(Car *car)
{
	this->car = car;
}
//新的功能
void swimCar::swim()
{
	cout << "新增了游泳的功能" << endl;
}
//虚函数重写
void swimCar::showCar()
{
	car->showCar();
	swim();
}
