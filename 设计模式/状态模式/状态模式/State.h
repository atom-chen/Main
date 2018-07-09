#pragma once


class State
{
public:
	virtual void doSomeThing(Worker *w) = 0;
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