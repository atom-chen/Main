#pragma once

class Worker;

class State
{
public:
	virtual void doSomeThing(Worker *w) = 0;
};

class Worker
{
public:
	Worker();
	int getHour()
	{
		return m_hour;
	}
	void setHour(int hour) //�ı�״̬ 7 
	{
		m_hour = hour;
	}
	State* getCurrentState()
	{
		return m_currstate;
	}
	void setCurrentState(State* state)
	{
		m_currstate = state;
	}

	void doSomeThing() //
	{
		m_currstate->doSomeThing(this);
	}
private:
	int		m_hour;
	State	*m_currstate; //����ĵ�ǰ״̬
};


class State1 : public State
{
public:
	void doSomeThing(Worker *w);
};

class State2 : public State
{
public:
	void doSomeThing(Worker *w);
};