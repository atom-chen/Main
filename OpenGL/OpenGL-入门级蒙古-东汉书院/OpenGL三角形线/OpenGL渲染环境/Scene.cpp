#include "Scene.h"

bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	return true;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT);
	glBegin(GL_TRIANGLE_FAN);
	glColor4ub(255, 0, 0, 255);
	glVertex3f(0.0f, -0.25f, -2.5f);

	glColor4ub(0, 0, 255, 255);
	glVertex3f(0.5f, -0.25f, -2.5f);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.4f, 0.0f, -2.5f);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.2f, 0.15f, -2.5f);

	glColor4ub(0, 0, 255, 255);
	glVertex3f(0.0f, 0.2f, -2.5f);


	glEnd();
}
