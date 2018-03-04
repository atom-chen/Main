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
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	//�������=(x/w,y/w,z/w)��1/0=����󣬹��λ��������Զ������Ϊ��̫���⣨����⣩
	float lightPos[] = { 0.0f, 0.1f, 0.0f, 0.0f };
	glLightfv(GL_LIGHT0, GL_POSITION, lightPos);//�����
	
	float whiteColor[] = { 1.0f, 1.0f, 1.0f, 1.0f };//����ɫ
	float blackColor[] = { 0.0f, 0.0f, 0.0f, 1.0f };//��ɫ
	float ambientMat[] = { 0.07f, 0.07f, 0.07f, 1.0f };//�����ⷴ��ϵ��
	float diffuseMat[] = { 0.4f, 0.9f, 0.9f, 1.0f };//������ϵ��
	float specularMat[] = { 0.9f, 0.9f, 0.9f, 1.0f };//���淴��ϵ��

	glLightfv(GL_LIGHT0, GL_AMBIENT, whiteColor);//���û�����
	glMaterialfv(GL_FRONT, GL_AMBIENT, ambientMat);//���û����ⷴ��ϵ��
	glLightfv(GL_LIGHT0, GL_DIFFUSE, whiteColor);
	glMaterialfv(GL_FRONT, GL_DIFFUSE, diffuseMat);
	glLightfv(GL_LIGHT0, GL_SPECULAR, whiteColor);
	glMaterialfv(GL_FRONT, GL_SPECULAR, specularMat);
	DrawModel();
}
void DrawModel()
{
	glBegin(GL_TRIANGLES);
	glColor4ub(255, 255, 255, 255);
	glVertex3f(-0.2f, -0.2f, -1.5f);

	glColor4ub(255, 0, 0, 255);
	glVertex3f(0.2f, -0.2f, -1.5f);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.0f, 0.2f, -1.5f);
	glEnd();
}
