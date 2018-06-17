#include "BresenhamCircle.h"
inline int GetD(int hx,int hy,int lx,int ly,int r)
{
	return hx*hx + hy*hy + lx*lx + ly*ly - 2 * r*r;
}
void BDraw1(int x0, int y0, int r)
{
	int x = x0+r;
	int y = y0;
	int nextL_X = x - 1;
	int nextL_Y = y;

	int nextH_X = x - 1;
	int nextH_Y = y+1;
	int d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	while (x < y)
	{
		DrawPoint(x, y);
		if (d >= 0)
		{
			x = nextL_X;
			y = nextL_Y;
		}
		else
		{
			x = nextH_X;
			y = nextH_Y;
		}
		nextL_X = x - 1;
		nextL_Y = y;

		nextH_X = x - 1;
		nextH_Y = y + 1;
		d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	}
}
void BDraw2(int x0, int y0, int r)
{
	int x = x0;
	int y = y0+r;
	int nextH_X=x+1;
	int nextH_Y=y;

	int nextL_X = x+1;
	int nextL_Y=y-1;
	int d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	while (x < y)
	{
		DrawPoint(x, y);
		if (d >= 0)
		{
			x = nextL_X;
			y = nextL_Y;
		}
		else
		{
			x = nextH_X;
			y = nextH_Y;
		}
		nextH_X = x + 1;
		nextH_Y = y;

		nextL_X = x + 1;
		nextL_Y = y - 1;
		d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	}
}
void BDraw3(int x0, int y0, int r)
{
	int x = x0 - r;
	int y = y0;

	int nextH_X = x + 1;//外
	int nextH_Y = y + 1;
	int nextL_X = x + 1;//内
	int nextL_Y = y;
	int d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	while (x < y)
	{
		DrawPoint(x, y);
		if (d >= 0)
		{
			x = nextL_X;
			y = nextL_Y;
		}
		else
		{
			x = nextH_X;
			y = nextH_Y;
		}
		nextH_X = x + 1;
		nextH_Y = y + 1;

		nextL_X = x + 1;
		nextL_Y = y;
		d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	}
}
void BDraw4(int x0, int y0, int r)
{
	int x = x0-r;
	int y = y0;

	int nextH_X = x + 1;//外
	int nextH_Y = y + 1;
	int nextL_X = x + 1;//内
	int nextL_Y = y;
	int d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	while (x < y)
	{
		DrawPoint(x, y);
		if (d >= 0)
		{
			x = nextL_X;
			y = nextL_Y;
		}
		else
		{
			x = nextH_X;
			y = nextH_Y;
		}
		nextH_X = x + 1;
		nextH_Y = y + 1;

		nextL_X = x + 1;
		nextL_Y = y ;
		d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	}
}

void BDraw5(int x0, int y0, int r)
{
	int x = x0 - r;
	int y = y0;

	int nextH_X = x + 1;//外
	int nextH_Y = y + 1;
	int nextL_X = x + 1;//内
	int nextL_Y = y;
	int d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	while (x < y)
	{
		DrawPoint(x, y);
		if (d >= 0)
		{
			x = nextL_X;
			y = nextL_Y;
		}
		else
		{
			x = nextH_X;
			y = nextH_Y;
		}
		nextH_X = x + 1;
		nextH_Y = y + 1;

		nextL_X = x + 1;
		nextL_Y = y;
		d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	}
}
void BDraw6(int x0, int y0, int r)
{
	int x = x0 - r;
	int y = y0;

	int nextH_X = x + 1;//外
	int nextH_Y = y + 1;
	int nextL_X = x + 1;//内
	int nextL_Y = y;
	int d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	while (x < y)
	{
		DrawPoint(x, y);
		if (d >= 0)
		{
			x = nextL_X;
			y = nextL_Y;
		}
		else
		{
			x = nextH_X;
			y = nextH_Y;
		}
		nextH_X = x + 1;
		nextH_Y = y + 1;

		nextL_X = x + 1;
		nextL_Y = y;
		d = GetD(nextH_X, nextH_Y, nextL_X, nextL_Y, r);
	}
}

void BDrawCircle(int x0, int y0, unsigned int r)
{
	BDraw1(x0, y0, r);
	BDraw2(x0, y0, r);
	//BDraw4(x0, y0, r);
}