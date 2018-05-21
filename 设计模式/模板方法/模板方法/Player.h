#pragma once
#include "absClass.h"

class Player:public AbsClass
{
public:
protected:
	virtual void TestAtack();
	virtual void PlaySound();
	virtual void PlayEffect();
private:
};