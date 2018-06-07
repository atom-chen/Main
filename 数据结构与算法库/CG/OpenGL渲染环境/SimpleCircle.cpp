#include "SimpleCircle.h"


void SimpleDrawCircle(int x0, int y0, unsigned int r)
{
	int xStart = x0 + r;
	int xEnd = x0 - r;
	int yStart = y0;
	
	int x = xStart;
	int y = yStart;
	
	while (x != xEnd)
	{
		DrawPoint(x, y);
		DrawPoint(x, -y);
		x--;
		y = sqrt((double)(r*r -x*x));
	}
}