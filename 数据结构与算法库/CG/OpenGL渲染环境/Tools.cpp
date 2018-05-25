#include "Tools.h"


float GetSlope(int x0, int y0, int x1, int y1)
{
	//计算斜率
	float k = ((y1 - (float)y0) / ((float)x1 - x0));
	return k;
}

//OpenGL固定管线使用右手坐标系
void DrawPoint(int x, int y, unsigned char r, unsigned char g, unsigned b, unsigned a)
{
	glBegin(GL_POINTS);
	glColor4ub(r, g, b, a);
	glVertex2i(x, y);
	glEnd();
}

void DrawPoint(int x, int y, COLOR color)
{
	glBegin(GL_POINTS);
	switch (color)
	{
	case White:
		glColor4ub(255, 255, 255, 255);
		break;
	case Black:
		glColor4ub(0, 0, 0, 255);
		break;
	case Red:
		glColor4ub(255, 0, 0, 255);
		break;
	case Blue:
		glColor4ub(0, 0, 255, 255);
		break;
	case Green:
		glColor4ub(0, 255, 0, 255);
		break;
	default:
		glColor4ub(255, 255, 255, 255);
		break;
	}
	glVertex3i(x, y,-5);
	glEnd();
}