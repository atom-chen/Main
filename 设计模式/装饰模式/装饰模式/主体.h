#pragma  once
#include<iostream>
#include <string>
using namespace std;

class Car
{
public:
	virtual void showCar() = 0;
};

class BMW :public Car
{
public:
	virtual void showCar();

};