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
		cout << "�ܳ�"<<endl;
	}
};
//���幦��
class flyCar :public Car{
public:
	flyCar(Car *car)
	{
		this->car = car;
	}
	//�µĹ���
	void fly()
	{
		cout << "�����˷ɵĹ���"<<endl;
	}
	//�麯����д
	virtual void showCar()
	{
		car->showCar();
		fly();
	}
private:
	//��Ͼ��峵
	Car *car;
};
//���幦��
class swimCar :public Car{
public:
	swimCar(Car *car)
	{
		this->car = car;
	}
	//�µĹ���
	void swim()
	{
		cout << "��������Ӿ�Ĺ���" << endl;
	}
	//�麯����д
	virtual void showCar()
	{
		car->showCar();
		swim();
	}
private:
	//����һ�������Ұ�����װ
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