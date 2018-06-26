#include "DDA.h"

void DDA(int x0, int y0, int x1, int y1)
{
	//¼ÆËãÐ±ÂÊ
	float k = GetSlope(x0, y0, x1, y1);
	float x = x0;
	float y = y0;
	//Ð±ÂÊ<=1
	if (abs(k) <= 1)
	{
		//k>0
		if (x <= x1)
		{
			for (; x <= x1; x++)
			{
				DrawPoint(x, y);
				y = y + k;
			}
		}
		//k<0
		else
		{
			for (; x >= x1; x--)
			{
				DrawPoint(x, y);
				y += k;
			}
		}


	}
	else
	{

		if (y <= y1)
		{
			for (; y <= y1; y++)
			{
				DrawPoint(x, y);
				x += (1 / k);
			}
		}
		else
		{
			for (; y >= y1; y--)
			{
				DrawPoint(x, y);
				x += (1 / k);
			}
		}
	}
}
