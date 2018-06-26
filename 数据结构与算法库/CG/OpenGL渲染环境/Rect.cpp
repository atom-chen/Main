#include "Rect.h"


void DrawRect(int x0, int y0, int width, int height)
{
	for (int i = 0; i <= height; i++)
	{
		for (int j = 0; j <= width; j++)
		{
			DrawPoint(x0 + i, y0 + j);
		}
	}
}