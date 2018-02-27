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
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT);
	glLoadIdentity();
	//ע����ת ���� �����������屾�����������еĻ
	glTranslatef(0, 0, -2.5f);
	glRotatef(30.0f, 0, 0, 1);
	glBegin(GL_TRIANGLES);
	glColor4ub(255, 255, 255, 255);
	glVertex3f(-0.2f, -0.2f,0);

	glColor4ub(255, 0, 0, 255);
	glVertex3f(0.2f, -0.2f,0);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.0f, 0.2f,0);
	glEnd();
}
