#include "Bresenham.h"
#include "Tools.h"
#include <math.h>


void Bresenham(int x0, int y0, int x1, int y1)
{
	int dx = abs(x1 - x0);
	int dy = abs(y1 - y0);
	int x = x0;
	int y = y0;
	int stepX = 1;
	int stepY = 1;
	if (x0 > x1)  //��������  
		stepX = -1;
	if (y0 > y1)
		stepY = -1;

	if (dx > dy)  //��������Ǹ���ǰ��  xÿ����һ������
	{
		int e = dy * 2 - dx;//e��ֵ
		for (int i = 0; i <= dx; i++)
		{
			DrawPoint(x, y);
			x += stepX;
			e += dy;
			if (e >= 0)
			{
				y += stepY;
				e -= dx;
			}
		}
	}
	else  //yÿ����һ������
	{
		int e = 2 * dx - dy;
		for (int i = 0; i <= dy; i++)
		{
			DrawPoint(x, y);
			y += stepY;
			e += dx;
			if (e >= 0)
			{
				x += stepX;
				e -= dy;
			}
		}
	}
} 
