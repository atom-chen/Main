#include "װ��.h"

flyCar::flyCar(Car *car)
{
	this->car = car;
}
//�µĹ���
void flyCar::fly()
{
	cout << "�����˷ɵĹ���" << endl;
}
//�麯����д
void flyCar::showCar()
{
	car->showCar();
	fly();
}

//���幦��
swimCar::swimCar(Car *car)
{
	this->car = car;
}
//�µĹ���
void swimCar::swim()
{
	cout << "��������Ӿ�Ĺ���" << endl;
}
//�麯����д
void swimCar::showCar()
{
	car->showCar();
	swim();
}
