
#pragma once
#include "MyPoint.h"

class AdvCircle
{
public:
	void setCircle(int _r, int _x0, int _y0);
	int judge(MyPoint &myp);

private:
	int r;
	int x0;
	int y0;
};