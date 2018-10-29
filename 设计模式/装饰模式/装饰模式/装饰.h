#pragma once
#include "主体.h"

class flyCar :public Car
{
public:
	flyCar(Car *car);
	//新的功能
	void fly();
	//虚函数重写
	virtual void showCar();
private:
	//组合具体车
	Car *car;
};

//具体功能
class swimCar :public Car
{
public:
	swimCar(Car *car);
	//新的功能
	void swim();
	//虚函数重写
	virtual void showCar();
private:
	//给我一辆车，我帮你组装
	Car *car;
};