#include "����.h"
#include "װ��.h"


void BMW::showCar()
{
	cout << "�ߵ��ܳ�" << endl;
}


void main()
{
	Car *runcar = new BMW;
	Car *flycar = new flyCar(runcar);
	Car *swimcar = new swimCar(flycar);

	runcar->showCar();;
	flycar->showCar();
	swimcar->showCar();

}