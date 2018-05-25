#pragma once
#include "Tools.h"


void DDA(int x0, int y0, int x1, int y1)
{
	//º∆À„–±¬ 
	float k = GetSlope(x0, y0, x1, y1);
	float x = x0;
	float y = y0;
	if (abs(k) <= 1)
	{
		//dx=1 dy=k 
		if (k > 0)
		{
			for (; x <= x1; x++)
			{
				DrawPoint(x, y);
				y = y + k;
			}
		}
		//dx=-1 dy=k
		else
		{
			for (; x >= x1; x--)
			{
				DrawPoint(x, y += k);
			}
		}
	}
	else
	{
		//dy=1 dx=1/k 
		if (k > 0)
		{
			for (; y <= y1; y++)
			{
				DrawPoint(x+=(1/k), y);
			}
		}
		//dy=-1 dx=1/k
		else if (k!=0)
		{
			for (; y >= y1; y--)
			{
				DrawPoint(x -= (1 / k), y);
			}
		}
		else
		{
			for (; y >= y1; y--)
			{
				DrawPoint(x , y);
			}
		}
	}
}
