#pragma once
#include "absClass.h"

class Enemy :public AbsClass
{
public:
protected:
	virtual void TestAtack();
	virtual void PlaySound();
	virtual void PlayEffect();
private:
};