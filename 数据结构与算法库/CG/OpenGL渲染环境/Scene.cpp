#include "Scene.h"
#include "DDA.h"
#include "MidDrawLine.h"
void SwitchTo2D()
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(-400, 400, -300, 300);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}
void SwitchTo3D()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}
bool Init()
{

	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glDisable(GL_DEPTH_TEST);
	SwitchTo3D();
	return true;
}
void Draw()
{

	glClear(GL_COLOR_BUFFER_BIT);
	//DDA(0, 0, 500,20);
	Mid(0, 0, 500, 20);
}
