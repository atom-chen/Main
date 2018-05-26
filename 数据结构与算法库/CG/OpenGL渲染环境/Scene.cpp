#include "Scene.h"
#include "DDA.h"
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
	SwitchTo2D();
	return true;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT);
	DDA(0, 0, 1, 1);
}
