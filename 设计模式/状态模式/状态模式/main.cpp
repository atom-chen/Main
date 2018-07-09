
#include <iostream>
using namespace std;
#include "State.h"
#include "Worker.h"





void State1::doSomeThing(Worker *w)
{
	if (w->getHour() == 7 || w->getHour() == 8)
	{
		cout << "吃早饭" << endl;
	}
	else
	{
		delete w->getCurrentState();  //状态1 不满足 要转到状态2
		w->setCurrentState(new State2);
		w->getCurrentState()->doSomeThing(w); //
	}
}


void State2::doSomeThing(Worker *w)
{
	if (w->getHour() == 9 || w->getHour() == 10)
	{
		cout << "工作" << endl;
	}
	else
	{
		delete w->getCurrentState(); //状态2 不满足 要转到状态3 后者恢复到初始化状态
		w->setCurrentState(new State1); //恢复到当初状态
		cout << "当前时间点：" << w->getHour() << "未知状态" << endl;
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