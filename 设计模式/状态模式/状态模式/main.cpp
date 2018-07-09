
#include <iostream>
using namespace std;
#include "State.h"
#include "Worker.h"





void State1::doSomeThing(Worker *w)
{
	if (w->getHour() == 7 || w->getHour() == 8)
	{
		cout << "���緹" << endl;
	}
	else
	{
		delete w->getCurrentState();  //״̬1 ������ Ҫת��״̬2
		w->setCurrentState(new State2);
		w->getCurrentState()->doSomeThing(w); //
	}
}


void State2::doSomeThing(Worker *w)
{
	if (w->getHour() == 9 || w->getHour() == 10)
	{
		cout << "����" << endl;
	}
	else
	{
		delete w->getCurrentState(); //״̬2 ������ Ҫת��״̬3 ���߻ָ�����ʼ��״̬
		w->setCurrentState(new State1); //�ָ�������״̬
		cout << "��ǰʱ��㣺" << w->getHour() << "δ֪״̬" << endl;
	}
}

Worker::Worker()
{
	m_currstate = new State1;
}

void main01()
{
	Worker *w1 = new Worker;
	w1->setHour(7);
	w1->doSomeThing();

	w1->setHour(9);
	w1->doSomeThing();

	delete w1;
	cout << "hello..." << endl;
	system("pause");
	return;
}