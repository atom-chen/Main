#pragma once
#include "����.h"

class flyCar :public Car
{
public:
	flyCar(Car *car);
	//�µĹ���
	void fly();
	//�麯����д
	virtual void showCar();
private:
	//��Ͼ��峵
	Car *car;
};

//���幦��
class swimCar :public Car
{
public:
	swimCar(Car *car);
	//�µĹ���
	void swim();
	//�麯����д
	virtual void showCar();
private:
	//����һ�������Ұ�����װ
	Car *car;
};