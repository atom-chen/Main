#include "MidCircle.h"

void Draw1(int x0, int y0, int r)
{

}
void Draw2(int x0, int y0, int r)
{
	int x = x0;
	int y = y0 + r;
	int d = 1 - r;
	
	while (x < y)
	{
		DrawPoint(x, y);
		if (d < 0)
		{
			d += 2 * x + 3;
			x++;//���ҷ�
		}
		else
		{
			d += 2 * (x - y) + 5;
			x++;
			y--;//���·�
		}
	}
}
void Draw3(int x0, int y0, int r)
{

}
void Draw4(int x0, int y0, int r)
{

}
void Draw5(int x0, int y0, int r)
{

}
void Draw6(int x0, int y0, int r)
{

}
void Draw7(int x0, int y0, int r)
{

}
void Draw8(int x0, int y0, int r)
{

}

void DrawCircle(int x0, int y0, unsigned int r)
{
	Draw1(x0, y0, r);
	Draw2(x0, y0, r);
	Draw3(x0, y0, r);
	Draw4(x0, y0, r);
	Draw5(x0, y0, r);
	Draw6(x0, y0, r);
	Draw7(x0, y0, r);
	Draw8(x0, y0, r);
}