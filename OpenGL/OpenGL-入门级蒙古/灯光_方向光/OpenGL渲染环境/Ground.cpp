#include "Ground.h"

void Ground::Draw()
{
	glEnable(GL_DEPTH_TEST);
	glDisable(GL_TEXTURE_2D);
	glBegin(GL_QUADS);
	//地面：法线向上
	glNormal3f(0, 1, 0);
	for (int32_t z = 0; z < 20;z++)
	{
		//起始坐标
		float zStart = 100 - z * 10;
		for (int32_t x = 0; x < 20; x++)
		{
			float xStar = 100 - x * 10;
			if ((x % 2) == 0 && (z % 2) == 0)
			{
				glColor4ub(41, 41, 41, 255);
			}
			else
			{
				glColor4ub(200, 200, 200, 255);
			}
			glVertex3f(xStar, -1.0f, zStart);
			glVertex3f(xStar+10, -1.0f, zStart);
			glVertex3f(xStar + 10, -1.0f, zStart-10);
			glVertex3f(xStar, -1.0f, zStart - 10);
		}
	}
	glEnd();
}