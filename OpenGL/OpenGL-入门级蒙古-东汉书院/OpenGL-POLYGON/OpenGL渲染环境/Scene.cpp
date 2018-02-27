#include "Scene.h"

bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	return true;
}
void Draw()
{
	glClear(GL_COLOR_BUFFER_BIT);
	glColor4ub(255, 0, 0, 255);
	glBegin(GL_POLYGON);
	glVertex3f(-0.5f, -0.25f, -2.5f);
	glVertex3f(0.5f, -0.25f, -2.5f);
	glVertex3f(-1.0f, 0.5f, -2.5f);
	glVertex3f(0.5F, 0.5f, -2.5f);
	glVertex3f(0.0f, 0.25f, -2.5f);;

	glEnd();
}
