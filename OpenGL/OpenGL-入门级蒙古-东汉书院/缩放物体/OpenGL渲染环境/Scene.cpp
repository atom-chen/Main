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
	glLoadIdentity();
	//3D世界本质：缩放的是物体的坐标，但是由于透视摄像机的近大远小特性，故其不变
	glScalef(0.2f, 0.2f, 0.2f);
	glBegin(GL_TRIANGLES);
	glColor4ub(255, 255, 255, 255);
	glVertex3f(-0.2f, -0.2f,-5.0f);

	glColor4ub(255, 0, 0, 255);
	glVertex3f(0.2f, -0.2f,-5);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.0f, 0.2f,-5);
	glEnd();
}
