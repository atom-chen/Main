#include "主体.h"
#include "装饰.h"


void BMW::showCar()
{
	cout << "高档跑车" << endl;
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